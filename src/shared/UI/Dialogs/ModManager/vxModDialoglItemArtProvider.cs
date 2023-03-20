using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.UI.Dialogs;

namespace VerticesEngine.UI.Themes
{
    public class vxModDialoglItemArtProvider : vxArtProviderBase, IGuiArtProvider
	{
	    public vxModDialoglItemArtProvider() : base()
		{

			Theme.Text.SelectedColour = Color.White;
			Theme.Background.SelectedColour = Color.DeepSkyBlue;

			Theme.Text.HoverColour = Color.Black;
			Theme.Background.HoverColour = Color.DarkOrange;
            //Theme.Border.SelectedColour = Color.WhiteSmoke * 0.5f;

            Padding = new Vector2(4);
		}

		public object Clone()
		{
			return this.MemberwiseClone();
		}

        public override SpriteFont GetFont()
        {
            return vxUITheme.Fonts.Size12;
        }

        public virtual void Draw(object guiItem)
		{
            vxModDialoglItem item = (vxModDialoglItem)guiItem;

			float i = 1;
				
            Theme.SetState(item);

            SpriteFont SubFont = vxInternalAssets.Fonts.ViewerFont;


			//Draw Button Background
			SpriteBatch.Draw(DefaultTexture, item.Bounds, Theme.Border.Color);
            SpriteBatch.Draw(DefaultTexture, item.Bounds.GetBorder(-1), Theme.Background.Color * i);

			//Draw Icon
			if (item.ButtonImage != null)
			{
                Rectangle imgRect = new Rectangle(
                                            (int)(item.Position.X + Padding.X),
                                            (int)(item.Position.Y + Padding.Y),
                                            (int)(item.Height - Padding.X * 2),
                    (int)(item.Height - Padding.Y * 2));


                SpriteBatch.Draw(item.ButtonImage, imgRect.GetBorder(1), Color.Black);
                
                SpriteBatch.Draw(item.ButtonImage, imgRect, 
                                        ((item.ToggleState || item.HasFocus) ? Color.White : Color.LightGray) * i);
			}


            string text = item.Text;
            float width = item.Bounds.Width - (item.Height * 2 + Padding.X * 4);

            float textWidth = Font.MeasureString(text).X;

            if (textWidth > width)
            {
                for (int ci = 0; ci < text.Length; ci++)
                {
                    string txt = text.Substring(0, ci);
                    float subTxtWidth = Font.MeasureString(txt).X;
                    if (subTxtWidth > width)
                    {
                        text = text.Substring(0, ci) + "...";
                        break;
                    }
                }
            }


            //if (item.ToggleState || item.HasFocus)
            if(item.IsEnabled)
            SpriteBatch.DrawString(Font, text,
					new Vector2((int)(item.Position.X + item.Height + Padding.X * 2), (int)(item.Position.Y + 8)) + new Vector2(2),
			                                  ((item.ToggleState || item.HasFocus) ? Color.Black : Color.White) * 0.25f);


            
			//Draw Text String
            SpriteBatch.DrawString(Font, text,
				new Vector2((int)(item.Position.X + item.Height + Padding.X * 2), (int)(item.Position.Y + 8)),
				Theme.Text.Color);

            SpriteBatch.DrawString(Font, item.Description,
                new Vector2((int)(item.Position.X + item.Height + Padding.X * 2), (int)(item.Position.Y + 8 + Font.LineSpacing)),
                Theme.Text.Color * 0.75f, 0.75f, Vector2.Zero);

            /*
            // File Name
            string author = (item.MetaData.WorkshopFileInfo.Author == "") ? "" : "By: "+item.MetaData.WorkshopFileInfo.Author;
            SpriteBatch.DrawString(SubFont, author,
				new Vector2((int)(item.Position.X + item.Height + Padding.X * 2), (int)(item.Position.Y + Font.LineSpacing + 10)),
                                          Theme.Text.Color * 0.75f);
                                         
            // File Size
            SpriteBatch.DrawString(SubFont, item.FileSize, 
                                   new Vector2(item.Bounds.Right - SubFont.MeasureString(item.FileSize).X - Padding.X,
                                           item.Position.Y + Padding.Y), 
                                          Theme.Text.Color); */

            // File IO Version
            string fileVersion = item.FileInfo.FullName;
            SpriteBatch.DrawString(SubFont, fileVersion,
                                          new Vector2(item.Bounds.Right - SubFont.MeasureString(fileVersion).X - Padding.X,
                                                      item.Bounds.Bottom - SubFont.MeasureString(fileVersion).Y - Padding.Y),
                                          Theme.Text.Color * 0.35f);

		}
	}
}
