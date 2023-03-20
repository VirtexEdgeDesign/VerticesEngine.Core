using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Graphics;
using VerticesEngine.UI.Controls;

namespace VerticesEngine.UI
{
    /// <summary>
    /// The label art provider for drawing 
    /// </summary>
	public class vxLabelArtProvider : vxUIArtProvider<vxLabel>
    {
        public vxLabelArtProvider() : base()
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


        public object Clone()
        {
            return this.MemberwiseClone();
        }




        protected internal override void DrawUIControl(vxLabel label)
        {
            if (label.IsShadowVisible)
                vxGraphics.SpriteBatch.DrawString(this.Font, label.Text,
                                                label.Position + label.ShadowOffset,
                                                label.ShadowColour * label.ShadowTransparency,
                                             label.Rotation,
                                             label.Origin,
                                            label.IsScaleFixed ? 1 : label.Scale,
                                            SpriteEffects.None, 0);


            vxGraphics.SpriteBatch.DrawString(this.Font, label.Text, label.Position, Theme.Text.Color,
                                             label.Rotation,
                                             label.Origin,
                                            label.IsScaleFixed ? 1 : label.Scale,
                                            SpriteEffects.None, 0);
        }
    }
}

