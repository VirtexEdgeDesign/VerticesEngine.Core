#region File Description
//-----------------------------------------------------------------------------
// FpsCounter.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Graphics;
using VerticesEngine.UI;

#endregion

namespace VerticesEngine.Diagnostics
{
    /// <summary>
    /// Component for FPS measure and draw.
    /// </summary>
    [vxDebugControl()]
    public class vxSpriteBatchCountDebugControl : vxDebugUIControlBaseClass
    {

        #region Fields

        // stringBuilder for FPS counter draw.
        private StringBuilder stringBuilder = new StringBuilder(16);

        #endregion

        #region Initialize

        public vxSpriteBatchCountDebugControl() : base("Sprite Batch Profiler")
        {
            IsVisible = false;

            stringBuilder.Length = 0;
        }

        public override string GetCommand()
        {
            return "sb";
        }

        public override string GetDescription()
        {
            return "Toggles the Sprite Batch Profiler";
        }

        #endregion

        /// <summary>
        /// FPS command implementation.
        /// </summary>
        public override void CommandExecute(IDebugCommandHost host,
                                    string command, IList<string> arguments)
        {
            if (arguments.Count == 0)
                IsVisible = !IsVisible;


            foreach (string arg in arguments)
            {
                switch (arg.ToLower())
                {
                    case "on":
                        IsVisible = true;
                        break;
                    case "off":
                        IsVisible = false;
                        break;
                }
            }
            base.CommandExecute(host, command, arguments);
        }

        #region Update and Draw

        protected internal override void Update()
        {
            // Update draw string.
            if (IsVisible)
            {
                stringBuilder.Length = 0;
                stringBuilder.Append("SB: ");
                stringBuilder.AppendNumber(vxProfiler.FPS);
            }
        }

        protected internal override void Draw()
        {
            SpriteFont font = vxInternalAssets.Fonts.DebugFont;

            Texture2D whiteTexture = vxInternalAssets.Textures.Blank;

            // Compute size of border area.
            Vector2 size = font.MeasureString("X");
            Rectangle rc =
                new Rectangle(0, 0, (int)(size.X * 14f), (int)(size.Y * 1.3f));

            var layout = new vxLayout(vxGraphics.SpriteBatch.GraphicsDevice.Viewport);
            rc = layout.Place(rc, 0.01f, 0.01f, Alignment.TopRight);

            // Place FPS string in border area.
            size = font.MeasureString(stringBuilder);
            layout.ClientArea = rc;
            Vector2 pos = layout.Place(size, 0, 0.1f, Alignment.Center);

            // Draw
            vxGraphics.SpriteBatch.Begin("Debug.Profiler.SpriteBatch");

            string sbDebug = "Sprite Batch Profiler";
            sbDebug += "\n-----------------------------------";
            sbDebug += "\nBatch Count: " + vxGraphics.SpriteBatch.BatchCallCount;

            int i = 0;
            foreach(var spriteBatchCall in vxGraphics.SpriteBatch.BatchCallNames)
            {
                i++;
                sbDebug += "\n" + i + ": " + spriteBatchCall;
            }

            vxGraphics.SpriteBatch.DrawString(vxInternalAssets.Fonts.DebugFont, sbDebug, Vector2.One, Color.Black);
            vxGraphics.SpriteBatch.DrawString(vxInternalAssets.Fonts.DebugFont, sbDebug, Vector2.Zero, Color.White);

            vxGraphics.SpriteBatch.End();
        }

        #endregion

    }
}
