using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using VerticesEngine.Commands;
using VerticesEngine.Graphics;

namespace VerticesEngine.Diagnostics
{
    /// <summary>
    /// This control handles all of the internal engine stop watches.
    /// </summary>
    [vxDebugControl]
    public class vxSandboxCommandDebugControl : vxDebugUIControlBaseClass
    {
        vxGameplaySceneBase CurrentScene;

        /// <summary>
        /// Is the Debug Timer Control Active?
        /// </summary>
        public new bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;

            }
        }
        bool _isVisible = false;

        Rectangle Backing;
        Rectangle DisplayBacking;
        Rectangle TextDisplayBacking;
        int buffer = 4;

        public override string GetCommand()
        {
            return "cs";
        }

        public override string GetDescription()
        {
            return "Toggles the sandbox command list";
        }

        public vxSandboxCommandDebugControl() : base("Sandbox Command List")
        {

            Backing = new Rectangle(15, vxGraphics.GraphicsDevice.Viewport.Height - 75, 300, 60);

            DisplayBacking = new Rectangle(Backing.X - buffer, Backing.Y - buffer,
                Backing.Width + 2 * buffer + 25, Backing.Height + 2 * buffer);

            TextDisplayBacking = new Rectangle(DisplayBacking.Right + buffer, DisplayBacking.Y,
                150, DisplayBacking.Height);
        }

        public override void CommandExecute(IDebugCommandHost host, string command, IList<string> args)
        {
            base.CommandExecute(host, command, args);

            this.IsVisible = !this.IsVisible;
        }


        int debugStringCount = 0;
        Vector2 startPos = new Vector2(8, 148);
        Rectangle bounds = new Rectangle(1, 1, 128, 1);

        int maxWidth = 1;

        void DrawString(string text)
        {
            var font = vxInternalAssets.Fonts.ViewerFont;
            Vector2 Pos = startPos + Vector2.UnitY * font.LineSpacing * debugStringCount;
            vxGraphics.SpriteBatch.DrawString(font, text, Pos + Vector2.One, Color.Black);

            maxWidth = Math.Max((int)font.MeasureString(text).X, maxWidth);

            Color color = Color.White;
            if (text.Contains("Added:"))
                color = Color.LimeGreen;
            if (text.Contains("Delete"))
                color = Color.OrangeRed;
            if (text.Contains("Move"))
                color = Color.Yellow;

            vxGraphics.SpriteBatch.DrawString(font, text, Pos, color);


            bounds.Height = (int)(Pos.Y - startPos.Y) + font.LineSpacing;
            bounds.Width = maxWidth;

        }

        protected internal override void Draw()
        {
            if (IsVisible && vxEngine.Instance.CurrentScene!= null)
            {
                if (CurrentScene == null)
                    CurrentScene = vxEngine.Instance.CurrentScene;

                debugStringCount = 0;
                maxWidth = 2;
                bounds.Location = startPos.ToPoint();

                vxGraphics.SpriteBatch.Begin("Debug - Command Manager");

                var blurBounds = bounds.GetBorder(2);

                vxGraphics.SpriteBatch.Draw(vxInternalAssets.Textures.Blank, bounds.GetBorder(3), Color.Black);
                vxGraphics.SpriteBatch.Draw(vxRenderPipeline.Instance.BlurredScene, blurBounds, blurBounds, Color.White * 0.5f);

                DrawString("Commands");
                debugStringCount++;
                DrawString("========================");
                int i = 1;
                foreach (vxCommand cmd in CurrentScene.CommandManager.Commands)
                {
                    debugStringCount++;
                    if (i == CurrentScene.CommandManager.CurrentCmdIndex + 1)
                    {
                        startPos -= Vector2.UnitX * 8;
                        DrawString(">");
                        startPos += Vector2.UnitX * 8;
                    }
                    i++;
                    DrawString(cmd.Tag);
                }
                vxGraphics.SpriteBatch.End();
            }

        }
    }
}
