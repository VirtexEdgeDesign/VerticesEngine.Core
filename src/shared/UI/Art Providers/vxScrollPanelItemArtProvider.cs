using Microsoft.Xna.Framework;
using VerticesEngine.Graphics;
using VerticesEngine.UI.Dialogs;

namespace VerticesEngine.UI.Themes
{
    public class vxScrollPanelItemArtProvider : vxArtProviderBase, IGuiArtProvider
	{
		public vxScrollPanelItemArtProvider() : base()
		{
            Padding = new Vector2(4);


            Theme = new vxUIControlTheme(
                new vxColourTheme(new Color(0.15f, 0.15f, 0.15f, 0.5f), Color.DarkOrange),
                new vxColourTheme(Color.LightGray));

            
		}



		public object Clone()
		{
			return this.MemberwiseClone();
		}

        int iconOffset = 0;
		public void Draw(object guiItem)
		{
			vxScrollPanelItem item = (vxScrollPanelItem)guiItem;

            Theme.SetState(item);

            //Draw Button Background
            vxGraphics.SpriteBatch.Draw(DefaultTexture, item.Bounds, Color.Black);
            vxGraphics.SpriteBatch.Draw(DefaultTexture, item.Bounds.GetBorder(-1), Theme.Background.Color);

			//Draw Icon
			if (item.ButtonImage != null)
			{
                vxGraphics.SpriteBatch.Draw(item.ButtonImage, new Rectangle(
                    (int)(item.Position.X + Padding.X), 
                    (int)(item.Position.Y + Padding.Y),
                    (int)(item.Height - Padding.X * 2), 
                    (int)(item.Height - Padding.Y * 2)), 
                                        Color.LightGray);

                iconOffset = item.Height;
			}

            //Draw Text String
            vxGraphics.SpriteBatch.DrawString(this.Font, item.Text,
                                          new Vector2(
                                              (int)(item.Position.X + iconOffset + Padding.X * 2), 
                                              (int)(item.Position.Y + 8)),
				Theme.Text.Color);
		}
	}
}
