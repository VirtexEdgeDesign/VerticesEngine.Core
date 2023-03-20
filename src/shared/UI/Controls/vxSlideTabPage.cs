using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.ContentManagement;
using VerticesEngine.Input;
using VerticesEngine;
using VerticesEngine.UI.Themes;
using VerticesEngine.Utilities;
using VerticesEngine.Graphics;

namespace VerticesEngine.UI.Controls
{
    /// <summary>
    /// Tab Page
    /// </summary>
    public class vxSlideTabPage : vxUIControl
    {

        /// <summary>
        /// The tab.
        /// </summary>
        public vxSlideTabPageTab Tab
        {
            get { return _tab; }
        }
        private vxSlideTabPageTab _tab;

        /// <summary>
        /// The Tab Control which Manages this page.
        /// </summary>
        private vxSlideTabControl m_parentTabControl;

        /// <summary>
        /// Tab Height
        /// </summary>
        public int TabHeight
        {
            get { return m_parentTabControl.TabHeight; }
        }

        //int _tabHeight = 100;

        /// <summary>
        /// Tab Width
        /// </summary>
        public int TabWidth
        {
            get { return Tab.Width; }
        }

        /// <summary>
        /// The force select.
        /// </summary>
        bool ForceSelect = false;

        public bool IsOpen
        {
            get { return _isOpen; }
        }
        bool _isOpen = false;

        private Vector2 m_position_Original = new Vector2();

        private Vector2 m_position_Requested = new Vector2();

        private Vector2 m_tabPositionOffset = Vector2.Zero;

        private Vector2 m_childElementOffset = Vector2.Zero;

        public Vector2 TabOffset = Vector2.Zero;

        Texture2D m_tabTexture;
        Texture2D m_backTexture;

        private vxToggleImageButton m_uiPinOpen;

        private vxLabel uiTopLabel;

        public bool IsTitleVisible
        {
            get { return uiTopLabel.IsEnabled; }
            set
            {
                uiTopLabel.IsEnabled = value;
                uiTopLabel.IsVisible = value;
            }
        }

        /// <summary>
        /// List of Items owned by this Tab Page
        /// </summary>
        public List<vxUIControl> Items = new List<vxUIControl>();

        public Color TitleColor = Color.DarkOrange;

        /// <summary>
        /// The distance offset for each tab 
        /// </summary>
        public Vector2 TabLayoutOffset;

        public bool IsToggleControledInternally = true;

        /// <summary>
        /// The offset distance 
        /// </summary>
        Vector2 PanelActiveOffsetVector
        {
            get
            {
                Vector2 orientation = Vector2.Zero;
                switch (this.ItemOrientation)
                {
                    case vxUIItemOrientation.Top:
                        orientation = Vector2.UnitY * m_parentTabControl.SelectionExtention;
                        break;
                    case vxUIItemOrientation.Bottom:
                        orientation = -Vector2.UnitY * m_parentTabControl.SelectionExtention;
                        break;
                    case vxUIItemOrientation.Left:
                        orientation = Vector2.UnitX * m_parentTabControl.SelectionExtention;
                        break;

                    case vxUIItemOrientation.Right:
                        orientation = -Vector2.UnitX * m_parentTabControl.SelectionExtention;
                        break;
                }
                return orientation;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxSlideTabPage"/> class.
        /// </summary>
        /// <param name="ParentTabControl">Parent tab control.</param>
        /// <param name="tabName">Tab name.</param>
        public vxSlideTabPage(vxSlideTabControl ParentTabControl, string tabName)
        {
            Font = vxInternalAssets.Fonts.BaseFont;

            m_parentTabControl = ParentTabControl;

            //Set Tab Name
            Text = tabName;

            //Set up Events
            ItemOrientation = ParentTabControl.ItemOrientation;
            this.ItemOreintationChanged += new EventHandler<EventArgs>(OnTabControlItemOreintationChanged);

            Height = 46 + 2;

            //Position is Set By Tab Control
            Position = this.m_parentTabControl.Position;
            if (ParentTabControl.ItemOrientation == vxUIItemOrientation.Left)
            {
                m_position_Original = this.m_parentTabControl.Position;
            }
            else if (ParentTabControl.ItemOrientation == vxUIItemOrientation.Right)
            {
                m_position_Original = this.m_parentTabControl.Position - new Vector2(this.m_parentTabControl.TabHeight, 0);
            }
            else if (ParentTabControl.ItemOrientation == vxUIItemOrientation.Bottom)
            {
                m_position_Original = this.m_parentTabControl.Position + new Vector2(0, this.m_parentTabControl.TabHeight);
                m_tabPositionOffset = new Vector2(0, -24);
            }
            else
            {
                m_position_Original = this.m_parentTabControl.Position;
            }



            m_tabTexture = vxInternalAssets.Textures.Blank;
            m_backTexture = vxInternalAssets.Textures.Blank;
            Padding = new Vector2(5);


            HoverAlphaMax = 1.0f;
            HoverAlphaMin = 0.0f;
            HoverAlphaDeltaSpeed = 10;



            if (IsOrientationHorizontal)
                _tab = new vxSlideTabPageTab(Text, Vector2.Zero, 32, 100);
            else
                _tab = new vxSlideTabPageTab(Text, Vector2.Zero, 100, 32);

            _tab.Font = this.Font;
            _tab.Clicked += OnTabClicked;
            _tab.ItemOrientation = ItemOrientation;

            if (IsOrientationHorizontal)
            {
                this.Width = ParentTabControl.Width - Tab.Width;
                this.Height = ParentTabControl.Height;
            }
            else
            {
                this.Width = ParentTabControl.Width;
                this.Height = ParentTabControl.Height - m_parentTabControl.TabHeight;
            }



            uiTopLabel = new vxLabel(tabName, new Vector2(5));
            uiTopLabel.Font = vxUITheme.Fonts.Size12;
            uiTopLabel.Theme.Text = new vxColourTheme(Color.Black);
            AddItem(uiTopLabel);



            m_uiPinOpen = new vxToggleImageButton(vxInternalAssets.SandboxUI.TabbedPinOff, vxInternalAssets.SandboxUI.TabbedPinOn, new Vector2(Width - 24, 5));
            m_uiPinOpen.IsTogglable = true;
            m_uiPinOpen.Width = 24;
            m_uiPinOpen.UnFocusAlpha = 1;
            AddItem(m_uiPinOpen);

            Close();
        }


        protected override void OnDisposed()
        {
            base.OnDisposed();

            foreach (var item in Items)
                item.Dispose();

            Items.Clear();

            Tab.Dispose();
        }


        void OnTabClicked(object sender, Events.vxUIControlClickEventArgs e)
        {
            m_parentTabControl.CloseAllTabs();
            Open();
        }

        void OnTabControlItemOreintationChanged(object sender, EventArgs e)
        {
            switch (this.ItemOrientation)
            {
                case vxUIItemOrientation.Left:
                    //Set Tab Offsets
                    m_tabPositionOffset = new Vector2(this.Width, 0) + TabOffset;
                    m_childElementOffset = Padding;
                    m_position_Original = Position;

                    break;

                case vxUIItemOrientation.Right:
                    //Set Tab Offsets
                    m_tabPositionOffset = new Vector2(TabHeight, 0);
                    //Position is Set By Tab Control
                    Position = this.m_parentTabControl.Position - new Vector2(TabHeight, 0);
                    m_position_Original = Position;

                    break;
            }
        }


        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <param name="guiItem">GUI item.</param>
        public void AddItem(vxUIControl guiItem)
        {
            Items.Add(guiItem);

            if (guiItem.Bounds.Right > Bounds.Right)
                guiItem.Width = Width - (int)Padding.X * 2;
        }


        //Clears out the GUI Items
        public void ClearItems()
        {
            Items.Clear();
        }


        /// <summary>
        /// Open this tab page. It won't close untill either Close(); is called, or if the tabpage recieves focus, and then loses focus.
        /// </summary>
        public void Open()
        {
            ForceSelect = true;
            _isOpen = true;
        }

        internal void ResetRootPosition(Vector2 vector)
        {
            //Position = vector;
            //    OriginalPosition= vector;
            //    m_position_Original= vector;
            //    m_position_Original= vector;
            RefreshPageLocation();
        }

        private void RefreshPageLocation()
        {
            Position = this.m_parentTabControl.Position;
            OriginalPosition = Position;
            if (m_parentTabControl.ItemOrientation == vxUIItemOrientation.Left)
            {
                m_position_Original = this.m_parentTabControl.Position;
            }
            else if (m_parentTabControl.ItemOrientation == vxUIItemOrientation.Right)
            {
                m_position_Original = this.m_parentTabControl.Position - new Vector2(this.m_parentTabControl.TabHeight, 0);
            }
            else if (m_parentTabControl.ItemOrientation == vxUIItemOrientation.Bottom)
            {
                m_position_Original = this.m_parentTabControl.Position + new Vector2(0, this.m_parentTabControl.TabHeight);
                m_tabPositionOffset = new Vector2(0, -24);
            }
            else
            {
                m_position_Original = this.m_parentTabControl.Position;
            }
        }

        public void Close()
        {
            if (m_uiPinOpen.ToggleState == false)
            {
                //Clear Focus
                HasFocus = false;

                //Clear Force Select just in case it was set.
                ForceSelect = false;

                //Reset Initial Position
                m_position_Requested = m_position_Original;
                _isOpen = false;
            }
        }

        /// <summary>
        /// Updates the GUI Item
        /// </summary>
        protected internal override void Update()
        {
            if (HasFocus == true)
                ForceSelect = false;

            m_uiPinOpen.Width = 16;
            //First Set Position based off of Selection Status
            if (ForceSelect)
            {
                m_position_Requested = m_position_Original + PanelActiveOffsetVector;
            }
            else if (HasFocus == false && IsToggleControledInternally)
            {
                if (vxInput.IsNewMouseButtonPress(MouseButtons.LeftButton) || vxInput.IsNewTouchPressed())
                    Close();
            }

            // Smooth ou tthe Position
            Position = vxMathHelper.Smooth(Position, m_position_Requested, m_parentTabControl.ResponseTime);

            // Now set the Tab Position
            Tab.Position = Position + m_tabPositionOffset + TabLayoutOffset;


            base.Update();


			//First Set Position
			if (IsOpen)
			{
				for (int i = 0; i < Items.Count; i++)
				{
					vxUIControl bsGuiItm = Items[i];

					bsGuiItm.Position = this.Position + m_childElementOffset + bsGuiItm.OriginalPosition;

					bsGuiItm.Update();

					if (bsGuiItm.HasFocus == true)
						HasFocus = true;

					//Re-eable all child items
					if (Math.Abs(Vector2.Subtract(Position, m_position_Requested).Length()) < 10)
					{
						bsGuiItm.IsEnabled = true;
						bsGuiItm.HoverAlphaReq = 1;
					}
					else
					{
						bsGuiItm.IsEnabled = false;
						bsGuiItm.HoverAlphaReq = 0;
					}
				}
			}
            Tab.Update();
        }

        /// <summary>
        /// Draws the GUI Item
        /// </summary>
        public override void Draw()
        {
            base.Draw();

            //Draw Button
            Color ColorReq = Theme.Background.Color;

            if (HasFocus)
                ColorReq = Color.Black;

#if __MOBILE__
            SpriteBatch.Draw(m_backTexture, Bounds, ColorReq * 0.5f);
#else
            SpriteBatch.Draw(m_backTexture, Bounds, Color.Black);
            SpriteBatch.Draw(vxRenderPipeline.Instance.BlurredScene, Bounds, Bounds, Color.White * 0.5f);
#endif

            Tab.Draw();

			// Draw Items
			if (IsOpen)
			{
                foreach (vxUIControl bsGuiItm in Items)
                {
                    if(bsGuiItm.IsEnabled && bsGuiItm.IsVisible)
                        bsGuiItm.Draw();
                }
			}

        }

        /// <summary>
        /// When the GUIItem is Selected
        /// </summary>
        public override void Select()
        {
            foreach (vxSlideTabPage tabPage in m_parentTabControl.Pages)
                tabPage.HasFocus = false;

            HasFocus = true;
            base.Select();
        }
    }
}
