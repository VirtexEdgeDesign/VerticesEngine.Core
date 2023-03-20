using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine;
using VerticesEngine.UI.Themes;

namespace VerticesEngine.UI.Controls
{

	/// <summary>
	/// Ribbon control.
	/// </summary>
	public class vxRibbonLabelControl : vxLabel
	{

		public vxRibbonLabelControl(string Text):
		base(Text, Vector2.Zero)
		{

            Height = 24;
            Width = 72;

            Font = vxInternalAssets.Fonts.ViewerFont;

            Position = new Vector2(vxScreen.Width, vxScreen.Height);

            Width = Math.Max((int)Font.MeasureString(Text).X + 32, 72);


            Theme = new vxUIControlTheme(
                new vxColourTheme(Color.Transparent, Color.DeepSkyBlue, Color.DarkOrange),
                new vxColourTheme(Color.Black * 0.75f, Color.Black, Color.Black));
		}




        public override void Draw()
        {
            Font = vxInternalAssets.Fonts.ViewerFont;
            SpriteBatch.DrawString(Font,
                                   Text,
                                   Bounds.Location.ToVector2() + new Vector2(Padding.X , Padding.Y+3),
                               GetStateColour(Theme.Text));

        }
	}
}
