using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine;

namespace VerticesEngine.UI.Themes
{
	public class vxLoadingScreenTheme : vxBaseItemTheme {

		public Texture2D SplashScreen;

		public float PercentageComplete;

		public Vector2 Position;

		public vxLoadingScreenTheme(vxEngine Engine) :base(Engine)
		{
			BackgroundColour = Color.LightGray;
			TextColour = Color.Black;

			Position = new Vector2 (0, 0);
		}
	}
}