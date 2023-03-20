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
    /// <summary>
    /// A Seperator Used in the vxScrollPanel.
    /// </summary>
    public class vxScrollPanelSpliter : vxUIControl
    {
        /// <summary>
        /// Gets or sets the button image.fdsf
        /// </summary>
        /// <value>The button image.</value>
        public Texture2D ButtonImage;

		public static Color BackgrounColour = Color.DarkOrange;
		public static bool DoUnderline = true;

        public vxScrollPanelSpliter(string Text)
        { 
            this.Text = Text;

            Position = Vector2.Zero;

            ButtonImage = vxInternalAssets.Textures.Blank;


            Font = vxUITheme.Fonts.Size16;
            this.Width = vxLayout.GetScaledWidth(4096);
            this.Height = vxLayout.GetScaledHeight(Font.LineSpacing * 2);
            Bounds = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }

        public override void DrawText()
        {
            vxGraphics.SpriteBatch.DrawString(Font,
                Text, Bounds.Location.ToVector2() + vxLayout.GetScaledSize(4) * Vector2.One,
                Color.Black);
        }

        public override void Draw()
        {

            vxGraphics.SpriteBatch.Draw(vxInternalAssets.Textures.Blank, Bounds.GetBorder(vxLayout.GetScaledSize(2)), Color.Black);
            vxGraphics.SpriteBatch.Draw(ButtonImage, Bounds, BackgrounColour);


            if (Text != null)
            {
				if(DoUnderline)
                vxGraphics.SpriteBatch.Draw(
                    vxInternalAssets.Textures.Blank,
                    new Rectangle(
                        Bounds.Location.X,
                        Bounds.Location.Y + vxLayout.GetScaledSize(4) + (int)vxInternalAssets.Fonts.MenuFont.MeasureString(Text).Y,
                        Bounds.Width,
                        1),
                    Color.Black * 0.5f);
            }
        }
    }
}
