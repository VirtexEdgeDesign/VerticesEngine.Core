using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VerticesEngine;
using VerticesEngine.Graphics;
using VerticesEngine.Input;

namespace VerticesEngine.UI.Controls
{
    public class vxListViewScrollBar : vxUIControl
    {
        public vxListView ParentPanel;
        public int BarWidth = 12;

        int TotalHeight = 0;
        private int scrollLength = 0;
        public int ScrollLength
        {
            get { return scrollLength; }
            set { scrollLength = value; SetBounds(); }
        }
        int MaxTravel = 0;

        private int scrollBarHeight = 0;
        public int ScrollBarHeight
        {
            get { return scrollBarHeight; }
            set { scrollBarHeight = value; }
        }

        int travelPosition = 0;
        public int TravelPosition
        {
            get { return travelPosition; }
            set { travelPosition = value; SetBounds(); }
        }

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

        //public Vector2 BasePosition;
        int StartMousePosition;

        bool IsScrolling = false;
        int ScrollWheel_Previous;

        public vxListViewScrollBar(vxListView Parent, Vector2 Position, int TotalHeight, int ViewHeight)
        {
            this.ParentPanel = Parent;
            this.Position = Position;
            //BasePosition = Position;

            this.TotalHeight = TotalHeight;

            scrollLength = ViewHeight;

            SetBounds();
        }
        
        void SetBounds()
        {
            ScrollBarHeight = (this.TotalHeight - (int)Padding.Y * 2) * (this.ParentPanel.Height 
                - (int)ParentPanel.Padding.Y * 2) / this.ScrollLength;

            MaxTravel = this.TotalHeight - ScrollBarHeight - (int)Padding.Y * 4;

            Bounds = new Rectangle((int)Position.X,
                (int)Position.Y + TravelPosition,
                BarWidth, ScrollBarHeight);
        }

        protected internal override void Update()
        {
            Bounds = new Rectangle((int)Position.X,
    (int)Position.Y + TravelPosition,
    BarWidth, ScrollBarHeight);

            if (HasFocus)
            {
                if (vxInput.MouseState.LeftButton == ButtonState.Pressed &&
                    PreviousMouseState.LeftButton == ButtonState.Released)
                {
                    StartMousePosition = vxInput.MouseState.Y;
                    IsScrolling = true;
                }
            }

            if (vxInput.MouseState.LeftButton == ButtonState.Released)
                IsScrolling = false;


            if (IsScrolling)
                TravelPosition = vxInput.MouseState.Y - StartMousePosition;

            if (HasFocus || ParentPanel.HasFocus)
            {
                TravelPosition += (vxInput.MouseState.ScrollWheelValue - ScrollWheel_Previous) / -10;

                TravelPosition = Math.Max(Math.Min(TravelPosition, MaxTravel), 0);
            }
            base.Update();

            ScrollWheel_Previous = vxInput.MouseState.ScrollWheelValue;
        }

		public override void Draw()
        {
            base.Draw();

            //
            //Draw Button
            //
            vxGraphics.SpriteBatch.Draw(vxInternalAssets.Textures.Blank, Bounds, Theme.Background.Color);
        }
    }
}
