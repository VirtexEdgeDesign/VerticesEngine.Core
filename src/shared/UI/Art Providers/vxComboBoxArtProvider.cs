using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using VerticesEngine.Graphics;
using VerticesEngine.UI.Controls;

namespace VerticesEngine.UI.Themes
{
    public class vxComboBoxArtProvider : vxUIArtProvider<vxComboBox>
    {
        public override SpriteFont Font => vxUITheme.Fonts.Size12;
        public vxComboBoxArtProvider()
        {
            SpriteSheetRegion = new Rectangle(0, 0, 4, 4);

            DefaultWidth = vxLayout.GetScaledWidth(225);
            DefaultHeight = vxLayout.GetScaledHeight(36);

            DoBorder = true;
            BorderWidth = 2;

            Theme = new vxUIControlTheme(
                new vxColourTheme(Color.DarkOrange, Color.DarkOrange * 1.2f, Color.DeepSkyBlue),
                new vxColourTheme(Color.Black),
            new vxColourTheme(Color.Black * 0.75f, Color.Black, Color.Black));
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }


        protected internal override void DrawUIControl(vxComboBox control)
        {

        }
    }
}
