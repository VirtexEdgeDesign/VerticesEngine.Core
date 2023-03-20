using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using VerticesEngine.Graphics;
using VerticesEngine.UI.Controls;

namespace VerticesEngine.UI.Themes
{
    /// <summary>
    /// The button art provider.
    /// </summary>
	public class vxTabPageTabArtProvider : vxArtProviderBase, IGuiArtProvider
	{
		public vxTabPageTabArtProvider ():base()
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

        public virtual void SetDefaults()
		{
			DefaultWidth = 150;//(int)(150 * vxLayout.ScreenSizeScaler);
			DefaultHeight = 24;//(int)(24 * vxLayout.ScreenSizeScaler);

		}

        public override SpriteFont GetFont()
        {
            return vxUITheme.Fonts.Size12;
        }

        public virtual void Draw(object guiItem)
		{
            vxTabPageTab tab = (vxTabPageTab)guiItem;

            Theme.SetState(tab);

            //Update Rectangle
            if (tab.UseDefaultWidth)
            {
                //Set Width and Height
                tab.Width = (int)(Math.Max(this.DefaultWidth, (int)(this.Font.MeasureString(tab.Text).X + Padding.X * 2))*vxLayout.Scale.X);
			    tab.Height = (int)(Math.Max(this.DefaultHeight, (int)(this.Font.MeasureString(tab.Text).Y + Padding.Y * 2))* vxLayout.Scale.Y);


                tab.Bounds = new Rectangle(
                    (int)(tab.Position.X - Padding.X),
                    (int)(tab.Position.Y - Padding.Y / 2),
                    tab.Width, tab.Height);
            }

			float GUIAlpha = 1;
			if (tab.UIManager != null)
				GUIAlpha = tab.UIManager.Alpha;


			if(tab.DoBorder)
                vxGraphics.SpriteBatch.Draw(vxUITheme.SpriteSheet, tab.BorderBounds, SpriteSheetRegion, tab.BorderColour * Opacity * GUIAlpha);

            vxGraphics.SpriteBatch.Draw(vxUITheme.SpriteSheet, tab.Bounds, SpriteSheetRegion, Theme.Background.Color * Opacity * GUIAlpha);

            vxGraphics.SpriteBatch.DrawString(this.Font, tab.Text,
				new Vector2(
					tab.Position.X + tab.Width / 2 - this.Font.MeasureString(tab.Text).X / 2* vxLayout.ScaleAvg - Padding.X,
					tab.Position.Y + tab.Height / 2 - this.Font.MeasureString(tab.Text).Y / 2* vxLayout.ScaleAvg),
                                          Theme.Text.Color * Opacity* GUIAlpha,
                                         0,
                                         Vector2.Zero,
                                         vxLayout.ScaleAvg,
                                         SpriteEffects.None,
                                         1);
		}
	}
}

