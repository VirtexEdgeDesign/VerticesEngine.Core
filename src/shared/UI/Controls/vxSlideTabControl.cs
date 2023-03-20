using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Utilities;
using VerticesEngine;
using VerticesEngine.Graphics;

namespace VerticesEngine.UI.Controls
{
	/// <summary>
	/// Tab Control which managers Tab Pages.
	/// </summary>
    public class vxSlideTabControl : vxUIControl
    {
        /// <summary>
        /// List of Tab Pages
        /// </summary>
        public List<vxSlideTabPage> Pages = new List<vxSlideTabPage>();

        /// <summary>
        /// Space in between tabs
        /// </summary>
        int m_pagePadding = 1;

        /// <summary>
        /// The response time. Higher Number is slower.
        /// </summary>
        public int ResponseTime = 3;

        /// <summary>
        /// The Amount that the page entends out
        /// </summary>
        public int SelectionExtention
        {
            get{
                if (ItemOrientation == vxUIItemOrientation.Left || ItemOrientation == vxUIItemOrientation.Right)
                    return this.Width;
                else
                    return this.Height;
            }   
        }

		/// <summary>
		/// The height of the tab stub.
		/// </summary>
        public int TabHeight = 24;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxSlideTabControl"/> class.
		/// </summary>
		/// <param name="Width">Width.</param>
		/// <param name="Height">Height.</param>
		/// <param name="offSetPosition">Off set position.</param>
		/// <param name="orientation">GUIO rientation.</param>
        public vxSlideTabControl(vxUIItemOrientation orientation, Vector2 offSetPosition, int Width, int Height)
        { 
            this.Width = Width;
            this.Height = Height;

            this.offSetPosition = offSetPosition;
            

            ItemOrientation = orientation;
            switch (orientation)
            {
                case vxUIItemOrientation.Bottom:

                    Position = new Vector2(0, vxScreen.Viewport.Height) + offSetPosition;
                    break;

                case vxUIItemOrientation.Left:

                    Position = offSetPosition - new Vector2(this.Width, 0);
                    break;

                case vxUIItemOrientation.Right:

                    Position = new Vector2(vxScreen.Viewport.Width, 0) + offSetPosition;
                    TabHeight *= -1;
                    break;
            }
			OriginalPosition = Position;
            HoverAlphaMax = 0.75f;
            HoverAlphaMin = 0.5f;
            HoverAlphaDeltaSpeed = 10;

            vxScreen.OnScreenResChanged += OnScreenResChanged;
        }
        
        Vector2 offSetPosition;

        private void OnScreenResChanged()
        {            
            switch (ItemOrientation)
            {
                case vxUIItemOrientation.Bottom:

                    Position = new Vector2(0, vxScreen.Viewport.Height) + offSetPosition;
                    break;

                case vxUIItemOrientation.Left:

                    Position = offSetPosition - new Vector2(this.Width, 0);
                    break;

                case vxUIItemOrientation.Right:

                    Position = new Vector2(vxScreen.Viewport.Width, 0) + offSetPosition;
                    TabHeight *= -1;
                    break;
            }
            OriginalPosition = Position;


            if(Pages != null)
            foreach (vxSlideTabPage page in Pages)
            {
                    if (page != null)
                    {
                        page.ResetRootPosition(Position);
                        //page.Position = this.Position;
                        //page.OriginalPosition = this.Position;
                    }
            }
        }



        /// <summary>
        /// Adds a vxTabPage.
        /// </summary>
        /// <param name="guiItem">GUI item.</param>
        public void AddPage(vxSlideTabPage guiItem)
        {
            int tempPosition = 0;
            //First Set Position
            foreach (vxSlideTabPage page in Pages)
            {
                tempPosition += page.TabWidth * 2 + m_pagePadding;
            }

            guiItem.TabOffset = new Vector2(0, tempPosition);
            guiItem.ItemOrientation = this.ItemOrientation;
            
            Pages.Add(guiItem);
        }

        protected override void OnDisposed()
        {
            vxScreen.OnScreenResChanged -= OnScreenResChanged;
            base.OnDisposed();

            foreach (var page in Pages)
                page.Dispose();

            Pages.Clear();
        }

        /// <summary>
        /// Closes all tabs.
        /// </summary>
        public void CloseAllTabs()
        {
            foreach (vxSlideTabPage tabPage in Pages)
            {
				tabPage.Close ();
            }
            this.HasFocus = false;
        }

        /// <summary>
        /// Updates the GUI Item
        /// </summary>
        protected internal override void Update()
        {
            base.Update();
            Vector2 runningTabOffset = Vector2.Zero;
            foreach (var page in Pages)
            {
                page.TabLayoutOffset = runningTabOffset;
                if(ItemOrientation == vxUIItemOrientation.Left || ItemOrientation == vxUIItemOrientation.Right)
                    runningTabOffset += Vector2.UnitY *( page.Tab.Height + (int)page.Tab.Padding.Y);
                else
                    runningTabOffset += Vector2.UnitX * (page.Tab.Width + (int)page.Tab.Padding.X);

                page.Update();
                if (page.HasFocus == true)
                    HasFocus = true;
            }
        }

		/// <summary>
		/// Draws the GUI Item
		/// </summary>
		public override void Draw()
        {
            base.Draw();

            // draws the background
            vxGraphics.SpriteBatch.Draw(DefaultTexture, Bounds, Color.Black * HoverAlpha);

            //Draw Pages
            foreach (vxSlideTabPage page in Pages)
                page.Draw();


            // draw covering
            if(ItemOrientation == vxUIItemOrientation.Left || ItemOrientation == vxUIItemOrientation.Right)
                vxGraphics.SpriteBatch.Draw(DefaultTexture, vxLayout.GetRect(Position.X - TabHeight, Position.Y, Width, Height), Color.DarkOrange);             
            else
                vxGraphics.SpriteBatch.Draw(DefaultTexture, vxLayout.GetRect(Position.X, Position.Y + TabHeight, Width, Height), Color.DarkOrange);
        }
    }
}
