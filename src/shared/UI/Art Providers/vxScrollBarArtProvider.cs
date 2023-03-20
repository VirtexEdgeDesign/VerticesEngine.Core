using Microsoft.Xna.Framework;
using VerticesEngine.Graphics;
using VerticesEngine.UI.Controls;

namespace VerticesEngine.UI.Themes
{
    /// <summary>
    /// The scrollbar art provider.
    /// </summary>
	public class vxScrollBarArtProvider : vxUIArtProvider<vxScrollBar>
	{
		public vxScrollBarArtProvider ():base()
		{
            SpriteSheetRegion = new Rectangle(0, 0, 4, 4);

			DefaultWidth = (int)(150 * vxLayout.ScaleAvg);
            DefaultHeight = (int)(24 * vxLayout.ScaleAvg);

			DoBorder = true;
			BorderWidth = 2;

            Theme = new vxUIControlTheme(
            new vxColourTheme(Color.DarkOrange, Color.DarkOrange * 1.2f, Color.DeepSkyBlue),
                new vxColourTheme(Color.Black));
		}


		public object Clone()
		{
			return this.MemberwiseClone();
		}

        protected internal override void DrawUIControl(vxScrollBar control)
        {
            Theme.SetState(control);

            // get the background size
            var rect = new Rectangle(control.Bounds.X, control.ParentPanel.Bounds.Y, control.Bounds.Width, control.ParentPanel.Height);

            // draw the background
            vxGraphics.SpriteBatch.Draw(vxUITheme.SpriteSheet, rect, new Rectangle(), Color.Black * 0.5f);

            // draw the bar
            vxGraphics.SpriteBatch.Draw(vxUITheme.SpriteSheet, control.Bounds, SpriteSheetRegion, Theme.Background.Color);
        }
    }
}

