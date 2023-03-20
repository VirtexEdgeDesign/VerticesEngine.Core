using Microsoft.Xna.Framework;
using VerticesEngine.Graphics;
using VerticesEngine.UI.Controls;

namespace VerticesEngine.UI.Themes
{
    public class vxSpinnerControlArtProvider : vxArtProviderBase, IGuiArtProvider
	{
        public vxSpinnerControlArtProvider():base()
		{

		}

		public object Clone()
		{
            return this.MemberwiseClone();
		}

		public virtual void Draw(object guiItem)
		{
            vxToolbar toolbar = (vxToolbar)guiItem;
            //Theme.SetState(toolbar);
            toolbar.Bounds = new Rectangle((int)(toolbar.Position.X), (int)(toolbar.Position.Y), 
                vxGraphics.GraphicsDevice.Viewport.Width, 
                (int)(toolbar.Height + Padding.Y));

            //Draw Toolbar
            vxGraphics.SpriteBatch.Draw(DefaultTexture,
                                    toolbar.Bounds, Theme.Background.Color * toolbar.HoverAlpha);
        }
	}
}
