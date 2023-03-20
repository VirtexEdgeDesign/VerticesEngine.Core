using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using VerticesEngine;
using VerticesEngine.UI.Controls;
using VerticesEngine.Utilities;
using VerticesEngine.UI.MessageBoxs;
using VerticesEngine.UI.Dialogs;
using VerticesEngine.Graphics;

namespace VerticesEngine.UI.Themes
{
	public class vxFileExplorerItemArtProvider : vxArtProviderBase, IGuiArtProvider
	{
        public vxFileExplorerItemArtProvider() : base()
		{
            Theme.Background = new vxColourTheme(Color.Gray * 0.25f, Color.Gray * 0.5f, Color.DeepSkyBlue);
            Theme.Text = new vxColourTheme(Color.White * 0.65f, Color.White, Color.Black);

            Padding = new Vector2(2, 2);
            //Font = vxInternalAssets.Fonts.ViewerFont;
		}

        public override SpriteFont GetFont()
        {
            return vxInternalAssets.Fonts.ViewerFont;
        }

        public object Clone()
		{
			return this.MemberwiseClone();
		}

		public virtual void Draw(object guiItem)
		{
            vxFileExplorerItem item = (vxFileExplorerItem)guiItem;

			float i = 1;

            Theme.SetState(item);

            //
            //Draw Button Background
            vxGraphics.SpriteBatch.Draw(DefaultTexture, item.Bounds, Theme.Border.Color);
            vxGraphics.SpriteBatch.Draw(DefaultTexture, item.Bounds.GetBorder(-1), Theme.Background.Color * i);


			//Draw Icon
			if (item.ButtonImage != null)
			{
                vxGraphics.SpriteBatch.Draw(item.ButtonImage, new Rectangle((int)(item.Position.X), (int)(item.Position.Y),
                                                                        16, 16), Color.White);
			}

            Vector2 TextPos = new Vector2((int)(item.Position.X + item.Height + Padding.X * 2), (int)(item.Position.Y + Padding.Y));

            //Draw Text String
            vxGraphics.SpriteBatch.DrawString(Font, item.FileName,TextPos, Theme.Text.Color);

            if (item.IsDirectory == false)
            {
                vxGraphics.SpriteBatch.DrawString(Font, item.LastUsed, TextPos + Vector2.UnitX * item.Width * 0.5f, Theme.Text.Color);

                vxGraphics.SpriteBatch.DrawString(Font, item.FileSize, TextPos + Vector2.UnitX * item.Width * 3 / 4, Theme.Text.Color);
            }
		}
	}
}
