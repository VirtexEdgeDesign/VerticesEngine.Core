using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Utilities;
using VerticesEngine;
using VerticesEngine.UI.Dialogs;
using VerticesEngine.UI.Themes;
using VerticesEngine.Input;

namespace VerticesEngine.UI.Controls
{
    /// <summary>
    /// Panel layout.
    /// </summary>
    public enum PanelLayout
    {
        /// <summary>
        /// Set the layout as a grid.
        /// </summary>
        Grid,

        /// <summary>
        /// Set the layout as a list.
        /// </summary>
        List
    }

    /// <summary>
    /// Scroll Panel Control which allows for any item too be added to it.
    /// </summary>
    public class vxScrollPanel : vxUIControl
    {
        /// <summary>
        /// The panel layout.
        /// </summary>
        public PanelLayout PanelLayout = PanelLayout.List;

        /// <summary>
        /// Collection of the vxGUIBaseItem's in this Panel.
        /// </summary>
        public List<vxUIControl> Items
        {
            get { return _items; }
        }
        private List<vxUIControl> _items = new List<vxUIControl>();

        /// <summary>
        /// The Scroll Bar Control.
        /// </summary>
        public vxScrollBar ScrollBar
        {
            get { return _scrollBar; }
        }
        private vxScrollBar _scrollBar;

        Vector2 PaddingVector = new Vector2(0, 0);

        /// <summary>
        /// The panel background texture.
        /// </summary>
        //public Texture2D PanelBackground;


        /// <summary>
        /// Gets or sets the art provider.
        /// </summary>
        /// <value>The art provider.</value>
        public vxScrollPanelArtProvider ArtProvider;


        /// <summary>
        /// The first touch down.
        /// </summary>
        Vector2 FirstTouchDownPosition = Vector2.Zero;


        /// <summary>
        /// Should items be auto arranged when added?
        /// </summary>
        public bool AutoArrangeItems = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxScrollPanel"/> class which
        /// can hold a Grid or Detail list of Items.
        /// </summary>
        /// <param name="position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
        /// <param name="Width">Width.</param>
        /// <param name="Height">Height.</param>
        public vxScrollPanel(Vector2 Position, int Width, int Height)
        {
            Alpha = 0.75f;

            this.Position = Position;
            this.OriginalPosition = Position;
            this.Width = Width;
            this.Height = Height;

            //BoundingRectangle = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            Padding = new Vector2(10);

            _scrollBar = new vxScrollBar(this,
                new Vector2(
                    Bounds.X + Bounds.Width - Padding.X - 20,
                    Bounds.Y + Padding.Y),
                Bounds.Height, Bounds.Height);

            this.EnabledStateChanged += ScrollPanel_EnabledStateChanged;


            ClickType = vxEnumClickType.OnRelease;

            //Have this button get a clone of the current Art Provider
            ArtProvider = (vxScrollPanelArtProvider)vxUITheme.ArtProviderForScrollPanel.Clone();
        }

        

        void ScrollPanel_EnabledStateChanged(object sender, EventArgs e)
        {
            //Get The Max and Min Positions of Items to get the overall Height
            foreach (vxUIControl bsGuiItm in Items)
            {
                bsGuiItm.IsEnabled = this.IsEnabled;
            }
        }

        /// <summary>
        /// When the GUIItem is Selected
        /// </summary>
        public override void Select()
        {
            HasFocus = true;
        }

        /// <summary>
        /// Clear the scrollbar of all items.
        /// </summary>
        public void Clear()
        {
            Items.Clear();
        }

        public int ScrollBarWidth
        {
            get { return ScrollBar.BarWidth; }
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();
            foreach (var item in _items)
                item.Dispose();
            _items.Clear();
        }

        /// <summary>
        /// Add an Item to the Scroll Panel
        /// </summary>
        /// <param name="uiControl">GUI item.</param>
        public virtual void AddItem(vxUIControl uiControl)
        {
            uiControl.DisableTouchSelectOnScroll = true;
            uiControl.ClickType = vxEnumClickType.OnRelease;


            //First, Check that the Width of the item is not wider than the panel it's self;
            /****************************************************************************************************/
            if (uiControl.Width > this.Bounds.Width)
            {
                uiControl.Width = this.Bounds.Width - (int)Padding.X * 3 - this.ScrollBarWidth;
            }

            //First set position relative too last item
            /****************************************************************************************************/
            if (AutoArrangeItems)
            {
                if (Items.Count > 0)
                {
                    //If there's more than one item in teh scroll panel, get it.
                    vxUIControl LastGuiItem = Items[Items.Count - 1];

                    //Now Set the Position of the new item relative too the previous one.
                    uiControl.Position = LastGuiItem.Position + new Vector2(LastGuiItem.Bounds.Width + Padding.X, 0);
                }
                else
                {
                    //If this is the first item, stick it in the top left corner.
                    uiControl.Position = Padding;// new Vector2(Padding);
                }


                //Check if it is inside the bounding rectangle, if not, move it down one row.
                /****************************************************************************************************/

                if (uiControl.Position.X + uiControl.Bounds.Width > Bounds.Width - Padding.X * 2 - ScrollBarWidth ||
                    uiControl.GetType() == typeof(vxFileDialogItem) ||
                    uiControl.GetBaseGuiType() == typeof(vxScrollPanelItem))
                {
                    if (Items.Count > 0)
                    {
                        vxUIControl LastGuiItem = Items[Items.Count - 1];
                        uiControl.Position = new Vector2(Padding.X, LastGuiItem.Position.Y + LastGuiItem.Bounds.Height + Padding.Y);
                    }

                    //There's a chance that This item is the width of the scroll panel, so it 
                    //should be set as the minimum between it's width, and the scroll panels width
                    //guiItem.Width = Math.Min(guiItem.Width, BoundingRectangle.Width - Padding * 2 - ScrollBarWidth);
                }
            }
            //Reset the Original Position
            uiControl.OriginalPosition = uiControl.Position;

            // Now set Width
            if (uiControl.GetBaseGuiType() == typeof(vxScrollPanelItem))
                ((vxScrollPanelItem)uiControl).Width = Bounds.Width - (int)Padding.X * 4 - ScrollBarWidth;

            //Finally Add the newly Positioned and Sized Item.
            Items.Add(uiControl);

            SetScrollLength();

            uiControl.OnGUIManagerAdded(this.UIManager);

            //UpdateItemPositions();
        }

        public override void ResetLayout()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                var item = Items[i];

                if (item.IsVisible)
                {
                    //First, Clamp the item width to the scroll panel width if its beyond or if its flagged too;
                    /****************************************************************************************************/
                    if (item.Width > this.Bounds.Width || item.IsFullWidth == true)
                    {
                        item.Width = this.Bounds.Width - (int)Padding.X * 3 - this.ScrollBarWidth;
                    }

                    //First set position relative too last item
                    /****************************************************************************************************/
                    if (AutoArrangeItems)
                    {
                        if (i > 0)
                        {
                            //If there's more than one item in teh scroll panel, get it.
                            vxUIControl LastGuiItem = Items[i - 1];

                            //Now Set the Position of the new item relative too the previous one.
                            item.Position = LastGuiItem.Position + new Vector2(LastGuiItem.Bounds.Width + Padding.X, 0);

                        }
                        else
                        {
                            //If this is the first item, stick it in the top left corner.
                            item.Position = Padding;// new Vector2(Padding);
                        }

                        //Check if it is inside the bounding rectangle, if not, move it down one row.
                        /****************************************************************************************************/

                        if (item.Position.X + item.Bounds.Width > Bounds.Width - Padding.X * 2 - ScrollBarWidth ||
                            item.GetType() == typeof(vxFileDialogItem) ||
                            item.GetBaseGuiType() == typeof(vxScrollPanelItem))
                        {
                            if (i > 0)
                            {
                                vxUIControl LastGuiItem = Items[i - 1];
                                item.Position = new Vector2(Padding.X, LastGuiItem.Position.Y + LastGuiItem.Bounds.Height + Padding.Y);
                            }

                            //There's a chance that This item is the width of the scroll panel, so it 
                            //should be set as the minimum between it's width, and the scroll panels width
                            //guiItem.Width = Math.Min(guiItem.Width, BoundingRectangle.Width - Padding * 2 - ScrollBarWidth);
                        }


                        //Reset the Original Position
                        item.OriginalPosition = item.Position;
                        item.ResetLayout();
                    }
                }
            }

            ScrollBar.ResetLayout();
            SetScrollLength();
            OnPanelScroll(0);
            /*
            Vector2 RunningLength = new Vector2(0, 0);// Position;

            foreach (var item in Items)
            {
                if (item.IsVisible)
                {
                    // Set position

                    if (item.GetBaseGuiType() == typeof(vxScrollPanelItem))
                        item.Width = this.Width - (int)Padding.X * 10 - ScrollBarWidth;
                    
                    // Set Position
                    item.Position = RunningLength + Padding;//+ new Vector2(1, 0);
                    item.OriginalPosition = item.Position;

                    // Update Running Length
                    RunningLength += Vector2.UnitY * (item.Height + Padding.Y);

                    // Reset the Layout Internally
                    item.ResetLayout();

                    //if (item.GetBaseGuiType() == typeof(vxScrollPanelItem))
                      //  BorderBounds.Width = this.Width;
                }
            }

            SetScrollLength();
            OnPanelScroll(0);
            */
        }

        public void SetScrollLength()
        {
            //Finally, set the Scrollbar scroll length.
            /****************************************************************************************************/
            float totalHeight = this.Height;
            float tempPos_min = this.Height - 1;
            float tempPos_max = this.Height;

            ScrollBar.Height = Height;

            //Get The Max and Min Positions of Items to get the overall Height
            foreach (vxUIControl item in Items)
            {
                if (item.IsVisible)
                {
                    tempPos_min = Math.Min(item.Position.Y, tempPos_min);
                    tempPos_max = Math.Max(item.Position.Y + item.Bounds.Height + Padding.Y, tempPos_max);
                }
            }

            ScrollBar.ScrollLength = (int)((tempPos_max - tempPos_min));
        }



        /// <summary>
        /// Adds a Range of Values for to the Scroll Panel
        /// </summary>
        /// <param name="range"></param>
        public void AddRange(IEnumerable<vxUIControl> range)
        {
            foreach (vxUIControl item in range)
            {
                AddItem(item);
            }
        }
        Vector2 PositionPreviousFrame = Vector2.Zero;

#if __MOBILE__

        /// <summary>
        /// Panel Speed
        /// </summary>
        float PanelSpeed = 0;

        /// <summary>
        /// Pervious Travel Pos
        /// </summary>
        float PreviousTravelPos;

        /// <summary>
        /// Do Speed effect
        /// </summary>
        bool DoSpeed = false;



        /// <summary>
        /// The initial travel position of the scroll bar.
        /// </summary>
        //float InitialTravelPos = 0;
#endif
        Vector2 prevPos = Vector2.Zero;
        /// <summary>
        /// Update this instance.
        /// </summary>
        protected internal override void Update()
        {
            base.Update();

#if __MOBILE__
            // Handle Touch Scrolling
            if (this.HasFocus && ScrollBar.IsScrolling == false)
			{
				// Get the Initial Touch Positions
				if (vxInput.IsNewMainInputDown())
				{
                    PanelSpeed = 0;
                    prevPos = vxInput.Cursor;
					//InitialTravelPos = ScrollBar.TravelPosition;
                    DoSpeed =false;
				}
				// If it's 'Not Released',
				else if (vxInput.IsMainInputDown())
				{
                    //PreviousTravelPos = ScrollBar.TravelPosition;

                    ScrollBar.TravelPosition -= (2 * (vxInput.Cursor.Y - prevPos.Y) / ((float)Height / (float)ScrollBar.ScrollBarHeight));
                    //ScrollBar.TravelPosition += (vxInput.Cursor.Y - prevPos.Y);

                    //FirstTouchDownPosition = vxInput.Cursor;

                    prevPos = vxInput.Cursor;
                    //PanelSpeed = ScrollBar.TravelPosition - PreviousTravelPos;

                    if (Math.Abs(PanelSpeed) > 1)
                        DoSpeed = true;
                }
                else if (vxInput.IsMainInputUp())
                {
                    if (DoSpeed)
                    {
                        ScrollBar.TravelPosition += PanelSpeed;
                        PanelSpeed *= 0.95f;
                        //vxConsole.WriteLine("Speed: " + Math.Abs(PanelSpeed));
                    }
                }
			}
            else if(HasFocus == false)
            {
                PanelSpeed = 0;
            }
#endif

			ScrollBar.Position = new Vector2(
				Bounds.X + Bounds.Width - Padding.X - ScrollBar.BarWidth,
				Bounds.Y);

			ScrollBar.Update();

            float PositionDifference = Vector2.Subtract(Position, PositionPreviousFrame).Length();

            float scrollChange = ScrollBar.Percentage - PreviousScrollbarPosition;

			if (Math.Abs(scrollChange) > 0)
				OnPanelScroll(scrollChange);

			//foreach (vxGUIBaseItem bsGuiItm in Items)
			for (int i = 0; i < Items.Count; i++)
			{
                if(PositionDifference > 0.05f)
                    UpdateItemPositions();

				// Only draw the item if it's on screen.
				//if (Items[i].Position.X < Bounds.Bottom && Items[i].Position.X > Bounds.X)
				{
                    Items[i].PositionDifference = PositionDifference;


					//if (this.HasFocus && dif < 0.05f)
					if (PositionDifference < 0.05f && HasFocus)
						Items[i].Update();
					else
						Items[i].NotHover();
				}
			}

			PositionPreviousFrame = this.Position;
            PreviousScrollbarPosition = ScrollBar.Percentage;
		}

		public virtual void OnPanelScroll(float ScrollDistance)
		{
            UpdateItemPositions();
		}

        public void UpdateItemPositions()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                //if (Math.Abs(scrollChange) > 0 || PositionDifference < 0.05f)
                Items[i].Position = this.Position + PaddingVector + Items[i].OriginalPosition
                        - new Vector2(0, (ScrollBar.Percentage * (ScrollBar.ScrollLength - this.Height + 2 * Padding.Y)));
            }
        }


        float PreviousScrollbarPosition =0;

		/// <summary>
		/// Draw this instance.
		/// </summary>
		public override void Draw()
		{
			base.Draw();

			//Now get the Art Provider to draw the scene
			this.ArtProvider.DrawUIControl(this);
		}
	}
}
