using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using VerticesEngine.Commands;
using VerticesEngine.Graphics;
using VerticesEngine.UI;

namespace VerticesEngine.Diagnostics
{
    /// <summary>
    /// This control handles all of the internal engine stop watches.
    /// </summary>
    [vxDebugControl]
    public class vxScreenConsoleDebugControl : vxDebugUIControlBaseClass
    {
        /// <summary>
        /// Is the Debug Timer Control Active?
        /// </summary>
        public new bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                vxDebug.IsGameStatusConsoleVisible = value;
                _isVisible = value;

            }
        }
        bool _isVisible = false;

        Rectangle Backing;


        public override string GetCommand()
        {
            return "gcon";
        }

        public override string GetDescription()
        {
            return "Toggle the in-game console which won't pause the game (Different than this console)";
        }

        public vxScreenConsoleDebugControl() : base("Sandbox Console List")
        {

            Backing = new Rectangle(15, vxGraphics.GraphicsDevice.Viewport.Height - 75, 300, 60);

        }

        public override void CommandExecute(IDebugCommandHost host, string command, IList<string> args)
        {
            base.CommandExecute(host, command, args);

            this.IsVisible = !this.IsVisible;
        }

        /// <summary>
        /// The debug string location.
        /// </summary>
        public Vector2 DebugStringLocation = new Vector2(8, 148);

        int runningWidth = 1;
        int runningHeight = 1;

        Vector2 runningText = Vector2.One;
        SpriteFont font = vxInternalAssets.Fonts.DebugFont;

        private void DrawString(string text, Color color)
        {
            vxGraphics.SpriteBatch.DrawString(font, text, runningText, color);

            runningWidth = Math.Max((int)font.MeasureString(text).X, runningWidth);
            runningText.Y += font.LineSpacing;
            runningHeight = (int)runningText.Y;

        }

        protected internal override void Draw()
        {
            if (IsVisible && vxEngine.Instance.CurrentScene != null && vxEngine.Instance.CurrentScene.Cameras != null && vxEngine.Instance.CurrentScene.Cameras.Count > 0)
            {
                if (vxDebug.IsGameStatusConsoleVisible)
                {
                    vxConsole.CurrentUpdateTick++;

                    Rectangle backRect = vxLayout.GetRect(DebugStringLocation, runningWidth, runningHeight - DebugStringLocation.Y);

                    runningWidth = 1;
                    runningHeight = 1;
                    runningText = DebugStringLocation;

                    vxGraphics.SpriteBatch.Begin("Debug - Command Manager");

                    vxGraphics.SpriteBatch.Draw(DefaultTexture, backRect.GetBorder(4), Color.Black);
                    vxGraphics.SpriteBatch.Draw(vxRenderPipeline.Instance.BlurredScene, backRect.GetBorder(3), backRect.GetBorder(3), Color.White * 0.45f);

                    DrawString("In-Game Debug Console: " + vxEngine.PlatformOS, Color.White);
                    DrawString("===============================================", Color.White);
                    for (int l = 0; l < vxConsole.InGameDebugLines.Count; l++)
                    {
                        DrawString(vxConsole.InGameDebugLines[l].text, vxConsole.InGameDebugLines[l].color);
                    }
                    vxGraphics.SpriteBatch.End();

                    //Clear it ever loop to prevent memory leaks
                    //vxConsole.InGameDebugLines.Clear();
                }
            }

        }
    }
}
