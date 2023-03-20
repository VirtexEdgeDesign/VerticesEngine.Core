using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Utilities;
using VerticesEngine;
using VerticesEngine.UI.Events;
using VerticesEngine.UI.Themes;
using VerticesEngine.UI.Controls;
using System.Collections.Generic;
using VerticesEngine.Graphics;
using VerticesEngine.ContentManagement;
using System.IO;

namespace VerticesEngine.UI.Dialogs
{
    public class vxFileExplorerURLBarButton : vxButtonControl
    {
        private DirectoryInfo directory;

        public string Path
        {
            get { return directory.FullName; }
        }

        public vxFileExplorerURLBarButton(vxFileExplorerURLBar ControlBar, string Text, Vector2 position, string urlPath) :
        base(Text, position)
        {
            Font = vxUITheme.Fonts.Size24;
            Width = (int)(Font.MeasureString(Text).X + Padding.X * 2);
            Height = 32;

            if (urlPath != "")
            {
                directory = new DirectoryInfo(urlPath);
            }
            Theme = new vxUIControlTheme(
                new vxColourTheme(Color.Gray * 0.25f, Color.DarkOrange, Color.DeepSkyBlue));
        }

        public override void Draw()
        {
            vxGraphics.SpriteBatch.Draw(DefaultTexture, Bounds, GetStateColour(Theme.Background));
            Vector2 TextSize = Font.MeasureString(Text).ToPoint().ToVector2();
            Vector2 pos = Position + new Vector2(Width / 2, Height / 2);
            pos = pos.ToPoint().ToVector2();
            vxGraphics.SpriteBatch.DrawString(Font, Text, pos,
                GetStateColour(Theme.Text), 0, TextSize / 2, 1, SpriteEffects.None, 1);
        }
    }
}
