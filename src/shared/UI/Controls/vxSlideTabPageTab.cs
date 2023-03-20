using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.ContentManagement;
using VerticesEngine.Input;
using VerticesEngine;
using VerticesEngine.UI.Themes;
using VerticesEngine.Utilities;
using VerticesEngine.Graphics;

namespace VerticesEngine.UI.Controls
{
    public class vxSlideTabPageTab : vxButtonControl
    {
        //public new Vector2 Padding
        //{
        //    get { return ArtProvider.Padding; }
        //    set { ArtProvider.Padding = value; }
        //}

        public vxSlideTabPageTab(string text, Vector2 position, int Width, int Height)
            : base(text, position, Width, Height)
        {

        }

        public override void Draw()
        {
            //base.Draw();
            vxUITheme.ArtProviderForSlidePageTab.DrawUIControl(this);
        }
    }
         
    public class vxSlidePageTabArtProvider : vxButtonArtProvider
    {
        public vxSlidePageTabArtProvider() : base()
        {
            Padding = new Vector2(2, 5);
            Theme = new vxUIControlTheme(
                new vxColourTheme(Color.Black * 0.5f, Color.Black * 0.75f, Color.DeepSkyBlue),
                new vxColourTheme(Color.White * 0.75f, Color.White, Color.Black));
        }


        public new object Clone()
        {
            return this.MemberwiseClone();
        }

        

        public override SpriteFont Font
        {
            get { return vxUITheme.Fonts.Size12; }
        }

        protected internal override void DrawUIControl(vxButtonControl tab)
        {
            float tabRotation = 0;

            Theme.SetState(tab);
            
            Vector2 TextSize = Font.MeasureString(tab.Text);

            if (tab.IsOrientationHorizontal)
            {
                tab.Height = (int)(TextSize.X + Padding.Y * 2);
                tab.Width = 24;
                tabRotation = MathHelper.PiOver2;
            }
            else
            {
                tab.Width = (int)(TextSize.X + Padding.Y * 2);
                tab.Height = 24;
            }

            //Draw Hover Rectangle
            vxGraphics.SpriteBatch.Draw(DefaultTexture, tab.Bounds, Theme.Background.Color);

            //Draw Text
            if (tab.IsOrientationHorizontal)
            {
                vxGraphics.SpriteBatch.DrawString(Font, tab.Text,
                                          new Vector2(tab.Bounds.X + Padding.Y + TextSize.Y, tab.Bounds.Y + Padding.X),
                                          Theme.Text.Color, tabRotation, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
            else
            {
                vxGraphics.SpriteBatch.DrawString(Font, tab.Text,
                                          new Vector2(tab.Bounds.X + Padding.X, tab.Bounds.Y + Padding.Y),
                                          Theme.Text.Color, tabRotation, Vector2.Zero, 1, SpriteEffects.None, 0);
            }

        }
    }
}
