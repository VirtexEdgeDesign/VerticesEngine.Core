using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using VerticesEngine.Graphics;
using VerticesEngine.UI;
using VerticesEngine.UI.Themes;

namespace VerticesEngine.Workshop.UI
{
    public class vxWorkshopDialogItemArtProvider : vxUIArtProvider<vxWorkshopDialogItem>
    {
        float loadingRot = 0;

        float ReqLoadedAlpha = 0;

        float LoadedAlpha = 0;

        public vxWorkshopDialogItemArtProvider() : base()
		{
            Theme.Text.DisabledColour = Color.DimGray;
			Theme.Text.SelectedColour = Color.White;
            Theme.Text.HoverColour = Color.Black;

            Theme.Background.DisabledColour = new Color(0.15f, 0.15f, 0.15f, 0.15f);
			Theme.Background.SelectedColour = Color.DeepSkyBlue;
			Theme.Background.HoverColour = Color.DarkOrange;

            Padding = new Vector2(4);
		}

		public object Clone()
		{
			return this.MemberwiseClone();
		}

        protected internal override void DrawUIControl(vxWorkshopDialogItem item)
        {
            loadingRot += 0.1f;
            float i = 1;

            if (item.IsProcessed)
                ReqLoadedAlpha = 1;

            LoadedAlpha = vxMathHelper.Smooth(LoadedAlpha, ReqLoadedAlpha, 2);

            Theme.SetState(item);

            //SpriteFont SubFont = vxInternalAssets.Fonts.ViewerFont;
            SpriteFont SubFont = vxUITheme.Fonts.Size12;

            //Draw Button Background
            vxGraphics.SpriteBatch.Draw(DefaultTexture, item.Bounds, Theme.Border.Color);
            vxGraphics.SpriteBatch.Draw(DefaultTexture, item.Bounds.GetBorder(-1), Theme.Background.Color * i);

            float w = (item.Height * 600.0f / 380.0f);
            Rectangle buttonRect = new Rectangle((int)(item.Position.X + Padding.X), (int)(item.Position.Y + Padding.Y),
                                            (int)(w - Padding.X * 2), (int)(item.Height - Padding.Y * 2));
            //Draw Icon
            if (item.ButtonImage != null)
            {
                vxGraphics.SpriteBatch.Draw(item.ButtonImage, buttonRect.GetBorder(2), Color.Black);
                vxGraphics.SpriteBatch.Draw(item.ButtonImage, buttonRect, Color.White * LoadedAlpha);
            }
            else
            {

                vxGraphics.SpriteBatch.Draw(DefaultTexture, buttonRect.GetBorder(2), Color.Black);
                vxGraphics.SpriteBatch.Draw(DefaultTexture, buttonRect, Color.Gray * 0.25f);
                // 957 674 32 32
                vxGraphics.SpriteBatch.Draw(vxUITheme.SpriteSheet, buttonRect.Center.ToVector2(), new Rectangle(957, 674, 32, 32),
                                 Color.White, loadingRot, Vector2.One * 16, 1.5f, SpriteEffects.None, 1);
            }


            string text = item.Text;
            float width = item.Bounds.Width - (item.Height * 2 + Padding.X * 4);

            float textWidth = Font.MeasureString(text).X;

            if (textWidth > width)
            {
                for (int ci = 0; ci < text.Length; ci++)
                {
                    string txt = text.Substring(0, ci);
                    float subTxtWidth = vxLayout.GetScaledWidth(Font.MeasureString(txt).X);
                    if (subTxtWidth > width)
                    {
                        text = text.Substring(0, ci) + "...";
                        break;
                    }
                }
            }


            //if (item.ToggleState || item.HasFocus)

            Vector2 TitlePos = new Vector2((int)(item.Position.X + w + Padding.X * 2), (int)(item.Position.Y + 8));
            Vector2 DescriptioPos = new Vector2(TitlePos.X, TitlePos.Y + vxLayout.GetScaledHeight(Font.LineSpacing));
            Vector2 sizePos = new Vector2(item.Bounds.Right - vxLayout.GetScaledSize(SubFont.MeasureString(item.Author).X) - Padding.X, item.Position.Y + Padding.Y);

            Color textShadow = ((item.ToggleState || item.HasFocus) ? Color.Black : Color.White);
            if (item.IsEnabled == false)
                textShadow = Color.Black;

            vxGraphics.SpriteBatch.DrawString(Font, text, TitlePos + new Vector2(2), textShadow * 0.25f, vxLayout.Scale);

            var descpLines = SubFont.WrapStringToArray(item.Description, (int)width);

            int descpHeight = item.Bounds.Bottom - (int)DescriptioPos.Y;


            string desp = string.Empty;
            int runningHeight = 0;
            for (int h = 0; h < descpLines.Length; h++)
            {
                runningHeight += SubFont.LineSpacing;

                if (runningHeight < descpHeight)
                    desp += descpLines[h] + Environment.NewLine;
            }

            if (runningHeight > descpHeight)
                desp += "...";


            //Draw Text String
            vxGraphics.SpriteBatch.DrawString(Font, text, TitlePos, Theme.Text.Color, vxLayout.Scale);
            vxGraphics.SpriteBatch.DrawString(SubFont, desp, DescriptioPos, Theme.Text.Color * 0.75f, vxLayout.Scale);
            vxGraphics.SpriteBatch.DrawString(SubFont, item.Author, sizePos, Theme.Text.Color, vxLayout.Scale);

            //SpriteBatch.DrawString(SubFont, "Subscribed: " + item.IsSubscribed.ToString(),
            //                       new Vector2(TitlePos.X, item.Bounds.Bottom - SubFont.LineSpacing * 2),
            //                       Theme.Text.Color);

            //SpriteBatch.DrawString(SubFont, "Installed:  "+ item.Item.IsInstalled.ToString(),
            //new Vector2(TitlePos.X, item.Bounds.Bottom - SubFont.LineSpacing),
            //Theme.Text.Color);

            // File IO Version
            //string fileVersion = item.FileVersion;
            //SpriteBatch.DrawString(SubFont, fileVersion,
            //new Vector2(item.Bounds.Right - SubFont.MeasureString(fileVersion).X - Padding.X,
            //            item.Bounds.Bottom - SubFont.MeasureString(fileVersion).Y - Padding.Y),
            //Theme.Text.Color * 0.5f);
        }
    }
}
