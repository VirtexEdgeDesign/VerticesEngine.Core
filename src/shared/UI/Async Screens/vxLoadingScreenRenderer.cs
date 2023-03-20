using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using VerticesEngine.Graphics;
using VerticesEngine.Screens.Async;
using VerticesEngine.UI.Themes;

namespace VerticesEngine.UI
{
    /// <summary>
    /// This is use for rendering loading screens
    /// </summary>
    public class vxLoadingScreenRenderer : vxArtProviderBase
    {
        protected vxLoadingScreen activeScreen;

        float loadAnimationTimer;

        /// <summary>
        /// Initialises a new loading screen
        /// </summary>
        public virtual void Init(vxLoadingScreen screen)
        {
            this.activeScreen = screen;
        }

        public virtual void OnExit()
        {

        }

        public virtual void Update(vxLoadingScreen screen)
        {

        }

        public virtual void Draw(vxLoadingScreen screen)
        {
            SpriteFont font = vxUITheme.Fonts.Size24;

            string message = "loading";

            // Center the text in the viewport.
            Viewport viewport = vxGraphics.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = font.MeasureString(message + "...");
            Vector2 textPosition = (viewportSize - textSize) * 95 / 100 - new Vector2(0, font.LineSpacing);

            Color color = vxSceneManager.LoadingScreenTextColor * screen.TransitionAlpha;

            // Animate the number of dots after our "Loading..." message.
            loadAnimationTimer += 0.0167f;

            int dotCount = (int)(loadAnimationTimer * 10) % 5;

            message += new string('.', dotCount);


            // Draw the text.
            SpriteBatch.Begin("Loading Screen");
            SpriteBatch.DrawString(font, message, textPosition, color);
            var totalLoadingRect = vxLayout.GetRect(64, vxScreen.Height - 64, (vxScreen.Width - 128) , 4);
            var percLoadingRect = vxLayout.GetRect(64, vxScreen.Height - 64, (vxScreen.Width - 128) * screen.LoadedPercentage, 4);

            SpriteBatch.Draw(DefaultTexture, totalLoadingRect, Color.Gray*.5f);

            SpriteBatch.Draw(DefaultTexture, percLoadingRect, Color.DarkOrange);
            SpriteBatch.End();
        }
    }
}
