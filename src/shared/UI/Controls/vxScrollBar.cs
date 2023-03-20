using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VerticesEngine;
using VerticesEngine.Input;
using VerticesEngine.UI.Themes;

namespace VerticesEngine.UI.Controls
{
    /// <summary>
    /// Scroll bar gui item which controls the scroll position of a <see cref="T:VerticesEngine.UI.Controls.vxScrollPanel"/>.
    /// </summary>
    public class vxScrollBar : vxUIControl
    {
        /// <summary>
        /// The parent panel.
        /// </summary>
        public vxScrollPanel ParentPanel;

        /// <summary>
        /// The Deafult Width of the Scrollbar for this game. It's recommended to set this in the LoadGUITheme method in your Game class.
        /// </summary>
        public static int DefaultWidth = 12;

        //public static Texture2D Texture;

        /// <summary>
        /// The width of the bar.
        /// </summary>
        public int BarWidth = 12;

        int TotalHeight = 0;
        private int scrollLength = 0;
        public int ScrollLength
        {
            get { return scrollLength; }
            set { scrollLength = value; ResetLayout(); }
        }


        /// <summary>
        /// The max travel of the scroll panel.
        /// </summary>
        public int MaxTravel = 0;


        public int ScrollBarHeight
        {
            get { return _scrollBarHeight; }
            set { _scrollBarHeight = value; }
        }
        private int _scrollBarHeight = 0;


        public float TravelPosition
        {
            get { return _travelPosition; }
            set { _travelPosition = value; ResetLayout(); }
        }
        float _travelPosition = 0;


        /// <summary>
        /// Gets the percentage of travel.
        /// </summary>
        /// <value>The percentage.</value>
        public float Percentage
        {
            get
            {
                if (MaxTravel < 1)
                    return 0;
                else
                    return (float)(TravelPosition) / ((float)(MaxTravel));
            }
        }

        /// <summary>
        /// The start mouse position when the mouse is first clicked.
        /// </summary>
        int StartMousePosition;

        /// <summary>
        /// Is the panel scrolling.
        /// </summary>
        public bool IsScrolling = false;

        /// <summary>
        /// Previous scroll wheel value.
        /// </summary>
        int ScrollWheel_Previous;

        /// <summary>
        /// Gets or sets the art provider.
        /// </summary>
        /// <value>The art provider.</value>
        public vxScrollBarArtProvider ArtProvider;


        //#if __IOS__ || __ANDROID__
        //		bool isFirstTouchDown = true;
        //#endif

        public static Color NormalColor = Color.DarkOrange;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxScrollBar"/> class.
        /// </summary>
        /// <param name="Parent">Parent.</param>
        /// <param name="position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
        /// <param name="TotalHeight">Total height.</param>
        /// <param name="ViewHeight">View height.</param>
        public vxScrollBar(vxScrollPanel Parent, Vector2 Position, int TotalHeight, int ViewHeight)
        {
            //Get Parent Panel
            ParentPanel = Parent;

            //Set Position
            this.Position = Position;

            //Set Total Height
            this.TotalHeight = TotalHeight;

            // Set the View Length
            scrollLength = ViewHeight;

            //Color_Normal = NormalColor;

            // Set Default
            BarWidth = vxEngine.PlatformType == vxPlatformHardwareType.Mobile ? DefaultWidth : 12;


            ResetLayout();
			ArtProvider = (vxScrollBarArtProvider)vxUITheme.ArtProviderForScrollBars.Clone();
        }
        public override void ResetLayout()
        {
            base.ResetLayout();

            // Set the Total Height as the Parent Panel Height
            TotalHeight = ParentPanel.Height;

            // Set the Scroll Bar height as a ratio of the Total Height with the Scroll Length
            ScrollBarHeight = ((TotalHeight) * (ParentPanel.Height) / ScrollLength);

            // Set the Max Travel
            MaxTravel = this.TotalHeight - ScrollBarHeight;// - (int)Padding.Y;

            // Reset the Bounds
            Bounds = new Rectangle((int)Position.X,
                                   (int)(Position.Y + TravelPosition),
                BarWidth, ScrollBarHeight);
        }

        public override void Select()
        {
            base.Select();

            // When the Scrollbar is Selected, then set the intitial click point
            StartMousePosition = (int)vxInput.Cursor.Y;
            IsScrolling = true;
        }


        protected internal override void Update()
        {
            MouseState mouseState = vxInput.MouseState;

            //#if __IOS__ || __ANDROID__

            //            if (vxInput.TouchCollection.Count == 0)
            //            {
            //                IsScrolling = false;
            //                StartMousePosition = (int)vxInput.Cursor.Y;
            //            }
            //#else

            //            if (mouseState.LeftButton == ButtonState.Released)
            //                IsScrolling = false;
            //#endif
            if (this.HasFocus && vxInput.IsMainInputDown())
                IsScrolling = true;
            else if (Math.Abs(vxInput.GamePadState.ThumbSticks.Right.Y) > 0.1f)
                IsScrolling = true;
            else if (vxInput.IsMainInputUp())
                IsScrolling = false;

#if !__MOBILE__
            if (IsScrolling)
                TravelPosition += (int)vxInput.Cursor.Y - (int)vxInput.PreviousCursor.Y;
#endif            
            if (ParentPanel.HasFocus)
            {
                TravelPosition += (mouseState.ScrollWheelValue - ScrollWheel_Previous) / -10;
                TravelPosition += vxInput.GamePadState.ThumbSticks.Right.Y * -7;
            }

            TravelPosition = (int)MathHelper.Clamp(TravelPosition, 0, MaxTravel);

            Bounds = new Rectangle((int)Position.X, (int)(Position.Y + TravelPosition), BarWidth, ScrollBarHeight);

            base.Update();

            ScrollWheel_Previous = mouseState.ScrollWheelValue;
        }

        public override void Draw()
        {
			//Now get the Art Provider to draw the scene
			this.ArtProvider.DrawUIControl(this);

			base.Draw();

        }
    }
}
