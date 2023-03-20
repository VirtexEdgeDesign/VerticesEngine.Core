using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine;

namespace VerticesEngine.UI.Themes
{
	public class vxBaseItemTheme
	{
        public vxEngine Engine;

        public int Width;
        public int Height;

        public Color TextColour;
		public Color TextHover;

		public Color BackgroundColour;
		public Color BackgroundHoverColour;


        public Color BorderColour;
        public Color BorderHoverColour;
        
        public int BorderWidth;
        public bool DoBorder;

		public Vector2 Margin;
		public Vector2 Padding;
		public Vector2 FineTune;

        public Texture2D BackgroundImage;


		/// <summary>
		/// Should the GUI Item draw a Drop Shadow. Default value is 'false'.
		/// </summary>
		public bool DoShadow = false;

        public vxBaseItemTheme(vxEngine Engine)
		{
            this.Engine = Engine;

            BackgroundImage = vxInternalAssets.Textures.Blank;

			Margin = new Vector2 (10, 10);
            Padding = new Vector2 (10, 10);
			FineTune = new Vector2 (0);

            int s = 35;
			TextColour = new Color(s, s, s, 255);
			TextHover = Color.Black;

			BackgroundColour = Color.Gray;
			BackgroundHoverColour = Color.DarkOrange;


			BorderColour = Color.Black;
			BorderHoverColour = Color.Black;

            BorderWidth = 1;
            DoBorder = false;
        }
	}
}

