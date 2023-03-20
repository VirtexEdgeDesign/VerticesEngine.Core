using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using VerticesEngine.Graphics;
using VerticesEngine.UI.Controls;

namespace VerticesEngine.UI.Themes
{
    public class vxScrollPanelArtProvider : vxUIArtProvider<vxScrollPanel>
    {
		/// <summary>
		/// A Specific Rasterizer State to preform Rectangle Sizzoring.
		/// </summary>
		protected RasterizerState rasterizerState;

		/// <summary>
		/// The scissor rectangle for rendering the scroll panels
		/// </summary>
        protected Rectangle scissorRectangle;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Themes.vxScrollPanelArtProvider"/> class.
        /// </summary>
        /// <param name="Engine">Engine.</param>
        public vxScrollPanelArtProvider():base()
		{
			rasterizerState = new RasterizerState() { ScissorTestEnable = true };

            //Theme = new vxUIControlTheme(
            //    new vxColourTheme(Color.Black * 0.75f));

			Theme.Background.SetTheme(Color.Black * 0.75f);
        }

        public object Clone()
		{
			return this.MemberwiseClone();
        }

        protected internal override void DrawUIControl(vxScrollPanel panel)
        {

  //      }

  //      public void Draw(object guiItem)
		//{
		//	vxScrollPanel panel = (vxScrollPanel)guiItem;


			scissorRectangle = panel.Bounds;
			
            //Set Scissor Rectangle Size

			try
			{
				//Set up Minimum 
				int x = MathHelper.Clamp(panel.Bounds.X, 0, vxGraphics.GraphicsDevice.Viewport.Width - 1);
				int y = MathHelper.Clamp(panel.Bounds.Y, 0, vxGraphics.GraphicsDevice.Viewport.Height - 1);

				int Width = panel.Bounds.Width;
				if (x + panel.Bounds.Width > vxGraphics.GraphicsDevice.Viewport.Width)
					Width = vxGraphics.GraphicsDevice.Viewport.Width - x;
				Width = Math.Max(Width, 1);

				int Height = panel.Bounds.Height;
				if (y + panel.Bounds.Height > vxGraphics.GraphicsDevice.Viewport.Height)
					Height = (panel.Bounds.Height - (panel.Bounds.Height - vxGraphics.GraphicsDevice.Viewport.Height));

				if (panel.Bounds.Y < 0)
					Height = panel.Bounds.Height + panel.Bounds.Y;

				Height = Math.Max(Height, 1);

				scissorRectangle =
					new Rectangle(
						x,
						y,
						Width,
						Height);
				//rec = BoundingRectangle;
			}
			catch (Exception ex)
			{
				vxConsole.WriteException(this,ex);
			}


			//Only draw if the rectangle is larger than 2 pixels. This is to avoid
			//artifacts where the minimum size is 1 pixel, which is pointless.
			if (scissorRectangle.Height > 5 && panel.Position.X + scissorRectangle.Width / 2 > 0)
			{
                // First End the Main GUImanager Spritebatch
                vxGraphics.SpriteBatch.End();

                // Now Draw Background the background with a new spritebatch
                vxGraphics.SpriteBatch.Begin("UI - Scroll Panel - Internals", SpriteSortMode.Immediate, BlendState.AlphaBlend,
					null, null, rasterizerState);


                //First Draw The Background
                DrawPanelBackground(panel);


                //Copy the current scissor rect so we can restore it after
                Rectangle OriginalScissorRectangle = vxGraphics.SpriteBatch.GraphicsDevice.ScissorRectangle;

                //Set the current scissor rectangle
                vxGraphics.SpriteBatch.GraphicsDevice.ScissorRectangle = scissorRectangle;

                // Now draw the panel internals.
                DrawPanelInternals(panel);


                // Now end this special sprite batch
                vxGraphics.SpriteBatch.End();

                //Reset scissor rectangle to the saved value
                vxGraphics.SpriteBatch.GraphicsDevice.ScissorRectangle = OriginalScissorRectangle;

                // Finally, restart the base Sprite Batch
                vxGraphics.SpriteBatch.Begin("UI - Post Scroll Panel");
			}
			else
			{
                // Do nothing otherwise
			}
        }

		/// <summary>
		/// Draws the panel background. Override to modify.
		/// </summary>
		/// <param name="panel">Panel.</param>
		public virtual void DrawPanelBackground(vxScrollPanel panel)
		{
            vxGraphics.SpriteBatch.Draw(DefaultTexture, panel.Bounds, Theme.Background.Color * Alpha);
        }

        /// <summary>
        /// Draws the panel internals. Override to modify.
        /// </summary>
        /// <param name="panel">Panel.</param>
        public virtual void DrawPanelInternals(vxScrollPanel panel)
		{
			//Then draw the scroll bar
			panel.ScrollBar.Draw();


			//use for loops, items can be removed while rendereing through the
			//loop. This is generally an issue during networking games when a
			//signal is recieved to remove an item while it's already rendering.
			for (int i = 0; i < panel.Items.Count; i++)
			{
				panel.Items[i].Draw();
			}


            //use for loops, items can be removed while rendereing through the
            //loop. This is generally an issue during networking games when a
            //signal is recieved to remove an item while it's already rendering.
            for (int i = 0; i < panel.Items.Count; i++)
            {
                panel.Items[i].DrawText();
            }

            if (panel.DoBorder)
                panel.DrawBorder();
        }

	}
}
