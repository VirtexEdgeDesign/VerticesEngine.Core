using System;
using Microsoft.Xna.Framework;
using VerticesEngine;
using VerticesEngine.Graphics;

namespace VerticesEngine.UI.Controls
{
	/// <summary>
	/// Ribbon control.
	/// </summary>
	public class vxRibbonToolbar : vxToolbar 
    {
		public string TitleText = "Vertices Engine Sandbox Editor";
        Color BackgroundColour = new Color(50, 50, 50, 255);

        public bool IsTitleShown = true;

		public vxRibbonToolbar(Vector2 Position): base(Position)
		{
			Width = 4096;
            Height = 24;
			Font = vxInternalAssets.Fonts.ViewerFont;
           // Height = 16;
            Padding = new Vector2(0);

		}


		public override void Draw()
        {
            SpriteBatch.Draw(DefaultTexture, Bounds, BackgroundColour);

            // Now draw each of the Toolbar Items.
            foreach (vxUIControl item in ToolbarItems)
                item.Draw();

            if (IsTitleShown)
            {
                Vector2 TitleSize = Font.MeasureString(TitleText);
                Vector2 TitlePos = Position + new Vector2(vxGraphics.GraphicsDevice.Viewport.Width / 2, Height / 2) - TitleSize / 2;
                TitlePos = new Vector2((int)TitlePos.X, (int)TitlePos.Y);

                SpriteBatch.DrawString(Font, TitleText, TitlePos, Color.WhiteSmoke * 0.65f);
            }
		}
	}
}
