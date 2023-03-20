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
    public class vxFpsCounterDebugControl : vxDebugUIControlBaseClass
    {

        #region Fields

        // stringBuilder for FPS counter draw.
        private StringBuilder stringBuilder = new StringBuilder(16);

        #endregion

        #region Initialize

        public vxFpsCounterDebugControl() : base( "FPS Counter")
        {
            IsVisible = false;

            stringBuilder.Length = 0;
        }

        public override string GetCommand()
        {
            return "fps";
        }

        public override string GetDescription()
        {
            return "Toggles the FPS Counter";
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

            vxProfiler.IsEnabled = true;

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
            stringBuilder.Length = 0;
            stringBuilder.Append("FPS: ");
            stringBuilder.AppendNumber(vxProfiler.FPS);
        }

        protected internal override void Draw()
        {
            SpriteFont font = vxInternalAssets.Fonts.DebugFont;
            vxSpriteBatch spriteBatch = vxGraphics.SpriteBatch;
            Texture2D whiteTexture = vxInternalAssets.Textures.Blank;

            // Compute size of border area.
            Vector2 size = font.MeasureString("X");
            Rectangle rc =
                new Rectangle(0, 0, (int)(size.X * 14f), (int)(size.Y * 1.3f));

            var layout = new vxLayout(spriteBatch.GraphicsDevice.Viewport);
            rc = layout.Place(rc, 0.01f, 0.01f, Alignment.TopRight);

            // Place FPS string in border area.
            size = font.MeasureString(stringBuilder);
            layout.ClientArea = rc;
            Vector2 pos = layout.Place(size, 0, 0.1f, Alignment.Center);

            // Draw
            spriteBatch.Begin("Debug - FPS");
            spriteBatch.Draw(whiteTexture, rc, new Color(0, 0, 0, 128));
            spriteBatch.DrawString(font, stringBuilder, pos, Color.White);
            spriteBatch.End();
        }

        #endregion

    }
}
