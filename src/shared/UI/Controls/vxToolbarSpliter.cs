using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine;
using VerticesEngine.UI.Themes;
using VerticesEngine.Graphics;

namespace VerticesEngine.UI.Controls
{
    public class vxToolbarSpliter : vxUIControl
    {
		public Rectangle SpriteSheetRegion = new Rectangle(0,0,4,4);


        /// <summary>
        /// Splitter for vxGUI Toolbar
        /// </summary>
        /// <param name="Engine"></param>
        /// <param name="width"></param>
        public vxToolbarSpliter(int width = 2)
        {
            //Position is Set by Toolbar
            Position = Vector2.Zero;

            //Set Initial Bounding Rectangle
            Width = width;
            Height = 48;
			Bounds = new Rectangle(0, 0, this.Width, this.Height);

            //Color_Normal = Color.Black;
            //Color_Highlight = Color.Black;
            Theme.Background = new vxColourTheme(Color.White);
        }

        int clickCapture() { return 0; }

        protected internal override void Update()
        { }

		public override void Draw()
        {
            base.Draw();

            //Update Rectangle
            //BoundingRectangle = new Rectangle((int)(Position.X - Padding.X), (int)(Position.Y - Padding / 2), Width, Height + Padding / 2);

            HoverAlpha = 0.75f;
			vxGraphics.SpriteBatch.Draw(vxUITheme.SpriteSheet, Bounds, SpriteSheetRegion, Theme.Background.Color * HoverAlpha);
        }
    }
}
