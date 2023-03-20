using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using VerticesEngine.ContentManagement;
using VerticesEngine.Graphics;
using VerticesEngine.UI;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Themes;

namespace VerticesEngine.Localization.UI
{
    public class vxLocComboBoxItem : vxComboBoxItem
    {
        public SpriteFont LocFont;

        public static int FontSize = 20;

        public vxLocComboBoxItem(string isoCode, string languageName, int index, Vector2 pos) : base(languageName, index, pos)
        {
            // load the font
            LocFont = vxContentManager.Instance.Load<SpriteFont>($"{vxUITheme.FontRootPath}/{isoCode}/font_{isoCode}_{FontSize}");
        }

        protected override SpriteFont GetFont()
        {
            return vxUITheme.Fonts.Size16;
        }

        public override void Draw()
        {
            Font = LocFont;

            switch (TextJustification)
            {
                case vxEnumTextHorizontalJustification.Left:
                    TextPosition = new Vector2(Position.X + Padding.X, Position.Y - 1);
                    break;
                case vxEnumTextHorizontalJustification.Center:
                    TextPosition = new Vector2(Position.X + Width / 2 - vxLayout.GetScaledSize(Font.MeasureString(Text).X / 2) - Padding.X, Position.Y - 1);
                    break;
            }

            Vector2 txtSize = Font.MeasureString(Text);

            //Draw Button
            vxGraphics.SpriteBatch.Draw(DefaultTexture, Bounds.GetBorder(-1), Theme.Background.Color);

            //vxGraphics.SpriteBatch.DrawString(Font, Text, Bounds.Center.ToVector2() + Vector2.UnitY, Theme.Text.Color, vxLayout.Scale, txtSize/2);
            vxGraphics.SpriteBatch.DrawString(Font, Text, (Bounds.Center.ToVector2() - Vector2.UnitY).ToIntValue(), Theme.Text.Color, Vector2.One, (txtSize / 2).ToIntValue());
        }
    }
}
