using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine;
using VerticesEngine.Graphics;

namespace VerticesEngine.UI.Controls
{
    public class vxListViewSpliter : vxUIControl
    {
        /// <summary>
        /// Gets or sets the button image.
        /// </summary>
        /// <value>The button image.</value>
        public Texture2D ButtonImage;

        public vxListViewSpliter(string Text)
        {
            this.Text = Text;

            Position = Vector2.Zero;

            ButtonImage = vxInternalAssets.Textures.Blank;

            this.Width = 1500;
            this.Height = 40;

            Bounds = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }



		public override void Draw()
        {
            base.Draw();

            //
            //Update Rectangle
            //     
            //BoundingRectangle = new Rectangle((int)(Position.X - Padding.X), (int)(Position.Y - Padding / 2), Width, Height + Padding / 2);
            Rectangle BackRectangle = new Rectangle((int)(Position.X - Padding.X) - 2, (int)(Position.Y - Padding.Y / 2) - 2, Width + 4, (int)(Height + Padding.Y / 2 + 4));

            //
            //Draw Button
            //
            /*
            if (HasFocus)
            {
                vxGraphics.SpriteBatch.Draw(Engine.Assets.Textures.Blank, BackRectangle, Color.LightBlue * 1.1f);
                vxGraphics.SpriteBatch.Draw(Engine.Assets.Textures.Blank, BoundingRectangle, Color.LightBlue);
            }
            */
            vxGraphics.SpriteBatch.Draw(ButtonImage, Bounds, Color.Black * 0.5f);


            if (Text != null)
            {
                int BackHeight = 4;
                
                vxGraphics.SpriteBatch.Draw(
                    vxInternalAssets.Textures.Blank,
                    new Rectangle(
                        Bounds.Location.X,
                        Bounds.Location.Y + BackHeight + (int)vxInternalAssets.Fonts.MenuFont.MeasureString(Text).Y,
                        Bounds.Width,
                        2),
                    Color.Gray * 0.5f);
                
                vxGraphics.SpriteBatch.DrawString(
                    vxInternalAssets.Fonts.MenuFont,
                    Text,
                    new Vector2(
                        Bounds.Location.X + 5,
                        Bounds.Location.Y + BackHeight + 5),
                    Color.LightGray);
            }
        }
    }
}
