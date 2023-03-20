using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine;
using VerticesEngine.UI.Controls;

namespace VerticesEngine.UI.Themes
{
    /// <summary>
    /// The button art provider.
    /// </summary>
	public class vxToolTipArtProvider : vxArtProviderBase, IGuiArtProvider
	{
		public vxToolTipArtProvider():base()
		{
            SpriteSheetRegion = new Rectangle(0, 0, 4, 4);

			DefaultWidth = (int)(150 * vxLayout.ScaleAvg);
            DefaultHeight = (int)(24 * vxLayout.ScaleAvg);

			DoBorder = true;
            BorderWidth = 2;

            Theme = new vxUIControlTheme(
                new vxColourTheme(Color.Black),
                new vxColourTheme(Color.Black),
            new vxColourTheme(Color.Black * 0.75f, Color.Black, Color.Black));
		}

        SpriteFont ToolTipFont
        {
            get { return vxInternalAssets.Fonts.ViewerFont; }
        }

        Rectangle ToolTipBounds = new Rectangle(0, 0, 1, 1);


        Vector2 ToolTipPadding = new Vector2(10, 2);



        public object Clone()
		{
			return this.MemberwiseClone();
		}



		public virtual void Draw(object guiItem)
		{
            vxUIControl control = (vxUIControl)guiItem;

            ToolTipBounds.Location = new Point(control.Bounds.Left + 5, control.Bounds.Bottom + 5);

            var ToolTipText = control.ToolTipText;
            var ToolTipAlpha = control.ToolTipAlpha;

            ToolTipBounds.Width = (int)(ToolTipFont.MeasureString(ToolTipText).X + ToolTipPadding.X);
            ToolTipBounds.Height = (int)(ToolTipFont.MeasureString(ToolTipText).Y + ToolTipPadding.Y);

            control.ToolTipAlpha = vxMathHelper.Smooth(ToolTipAlpha, 1, 8);

            SpriteBatch.Draw(DefaultTexture, ToolTipBounds.GetBorder(1), Color.Black * 0.65f * ToolTipAlpha);
            SpriteBatch.Draw(DefaultTexture, ToolTipBounds, Color.DimGray * ToolTipAlpha);
            SpriteBatch.Draw(DefaultTexture, ToolTipBounds.GetBorder(-1), Color.Black * 0.5f * ToolTipAlpha);

            SpriteBatch.DrawString(
                vxInternalAssets.Fonts.ViewerFont,
                ToolTipText,
                ToolTipBounds.Location.ToVector2() + ToolTipPadding / 2,
                Color.White * 0.7f * ToolTipAlpha);
        }
	}
}

