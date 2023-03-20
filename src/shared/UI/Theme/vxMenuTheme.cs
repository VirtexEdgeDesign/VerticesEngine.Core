using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine;
using VerticesEngine.Graphics;
using VerticesEngine.UI.Controls;

namespace VerticesEngine.UI.Themes
{
	public class vxMenuTheme : vxBaseItemTheme {

		public int vxMenuItemWidth;
		public int vxMenuItemHeight;

        public vxEnumTextHorizontalJustification TextJustification;
		public Texture2D vxMenuItemBackground;

        public bool DrawBackgroungImage;

        //Title Code
        public Vector2 TitlePosition;
		public Texture2D TitleBackground;
		public Color TitleColor;

		public Vector2 BoundingRectangleOffset;

		public vxMenuTheme(vxEngine Engine) : base(Engine)
		{
            TextJustification = vxEnumTextHorizontalJustification.Center;

			FineTune = new Vector2 (0, 5);

			vxMenuItemWidth = 100;
			vxMenuItemHeight = 34;

			TitleColor = Color.White;
			TitlePosition = new Vector2(vxGraphics.GraphicsDevice.Viewport.Width / 2, 80);

			BoundingRectangleOffset = new Vector2(0,0);

            DrawBackgroungImage = true;
        } 
	}
}

