using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using VerticesEngine.UI;

namespace VerticesEngine.Graphics
{
    /// <summary>
    /// The sprite batch used by the Vertices Engine to measure and count Batch Calls. This is
    /// mostly for profiling and organization as well as allows for extention methods to be created.
    /// </summary>
    public class vxSpriteBatch : SpriteBatch
    {

        /// <summary>
        /// Sprite batch
        /// </summary>
        /// <param name="graphicsDevice"></param>
        public vxSpriteBatch(GraphicsDevice graphicsDevice) : base(graphicsDevice) { }

        /// <summary>
        /// The batch call count
        /// </summary>
        public int BatchCallCount
        {
            get { return _batchCallCount; }
        }
        int _batchCallCount = 0;



        //string text;

        //public GraphicsDevice GraphicsDevice
        //{
        //    get { return spriteBatch.GraphicsDevice; }
        //}

        /// <summary>
        /// Begins a new sprite and text batch with the specified render state.
        /// </summary>
        /// <param name="sortMode">The drawing order for sprite and text drawing. <see cref="F:Microsoft.Xna.Framework.Graphics.SpriteSortMode.Deferred" /> by default.</param>
        /// <param name="blendState">State of the blending. Uses <see cref="F:Microsoft.Xna.Framework.Graphics.BlendState.AlphaBlend" /> if null.</param>
        /// <param name="samplerState">State of the sampler. Uses <see cref="F:Microsoft.Xna.Framework.Graphics.SamplerState.LinearClamp" /> if null.</param>
        /// <param name="depthStencilState">State of the depth-stencil buffer. Uses <see cref="F:Microsoft.Xna.Framework.Graphics.DepthStencilState.None" /> if null.</param>
        /// <param name="rasterizerState">State of the rasterization. Uses <see cref="F:Microsoft.Xna.Framework.Graphics.RasterizerState.CullCounterClockwise" /> if null.</param>
        /// <param name="effect">A custom <see cref="T:Microsoft.Xna.Framework.Graphics.Effect" /> to override the default sprite effect. Uses default sprite effect if null.</param>
        /// <param name="transformMatrix">An optional matrix used to transform the sprite geometry. Uses <see cref="P:Microsoft.Xna.Framework.Matrix.Identity" /> if null.</param>
        /// <exception cref="T:System.InvalidOperationException">Thrown if <see cref="M:Microsoft.Xna.Framework.Graphics.SpriteBatch.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode,Microsoft.Xna.Framework.Graphics.BlendState,Microsoft.Xna.Framework.Graphics.SamplerState,Microsoft.Xna.Framework.Graphics.DepthStencilState,Microsoft.Xna.Framework.Graphics.RasterizerState,Microsoft.Xna.Framework.Graphics.Effect,System.Nullable{Microsoft.Xna.Framework.Matrix})" /> is called next time without previous <see cref="M:Microsoft.Xna.Framework.Graphics.SpriteBatch.End" />.</exception>
        /// <remarks>This method uses optional parameters.</remarks>
        /// <remarks>The <see cref="M:Microsoft.Xna.Framework.Graphics.SpriteBatch.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode,Microsoft.Xna.Framework.Graphics.BlendState,Microsoft.Xna.Framework.Graphics.SamplerState,Microsoft.Xna.Framework.Graphics.DepthStencilState,Microsoft.Xna.Framework.Graphics.RasterizerState,Microsoft.Xna.Framework.Graphics.Effect,System.Nullable{Microsoft.Xna.Framework.Matrix})" /> Begin should be called before drawing commands, and you cannot call it again before subsequent <see cref="M:Microsoft.Xna.Framework.Graphics.SpriteBatch.End" />.</remarks>
        public void Begin(string batchCall, SpriteSortMode sortMode = SpriteSortMode.Deferred, BlendState blendState = null, SamplerState samplerState = null, DepthStencilState depthStencilState = null, RasterizerState rasterizerState = null, Effect effect = null, Matrix? transformMatrix = default(Matrix?))
        {
            BatchCallNames.Add(batchCall);
            base.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);
        }

        public List<string> BatchCallNames = new List<string>(1024);

        public new void End()
        {
            _batchCallCount++;
            base.End();

        }


        internal void StartNewFrame()
        {
            _batchCallCount = 0;
            BatchCallNames.Clear();
        }

        /*
        #region -- Draw --

        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color,
float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }


        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color,
        float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }

        public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle,
    Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth);
        }

        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, color);
        }

        public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
        {
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color);
        }

        public void Draw(Texture2D texture, Vector2 position, Color color)
        {
            spriteBatch.Draw(texture, position, color);
        }

        public void Draw(Texture2D texture, Rectangle destinationRectangle, Color color)
        {
            spriteBatch.Draw(texture, destinationRectangle, color);
        }

        #endregion
        */
        #region -- Draw String --
        /*

        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color)
        {
            spriteBatch.DrawString(spriteFont, text, position, color);
        }


        public void DrawString(
            SpriteFont spriteFont, string text, Vector2 position, Color color,
            float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
        }


        public void DrawString(
            SpriteFont spriteFont, string text, Vector2 position, Color color,
            float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
        }


        public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color)
        {
            spriteBatch.DrawString(spriteFont, text, position, color);
        }

        public void DrawString(
    SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color,
    float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            var scaleVec = new Vector2(scale, scale);
            DrawString(spriteFont, text, position, color, rotation, origin, scaleVec, effects, layerDepth);
        }


        public void DrawString(
            SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color,
            float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
        }
        */

        // Vertices Specific
        public void DrawString(SpriteFont font, string text, Vector2 position, Color color, float scale,
       vxHorizontalJustification horizontalJustification = vxHorizontalJustification.Left, vxVerticalJustification verticalJustification = vxVerticalJustification.Top,
                                 float rotation = 0)
        {
            var origin = Vector2.Zero;

            // If its centered, then set the origin
            if (horizontalJustification == vxHorizontalJustification.Center)
            {
                origin = new Vector2(font.MeasureString(text).X / 2, origin.Y);
            }

            if (verticalJustification == vxVerticalJustification.Middle)
            {
                origin = new Vector2(origin.X, font.MeasureString(text).Y / 2);
            }


            base.DrawString(font, text, position, color, rotation, origin, scale, SpriteEffects.None, 1);
        }

        public void DrawString(SpriteFont font, string text, Vector2 position, Color color, float scale, Vector2 origin)
        {
            base.DrawString(font, text, position, color, 0, origin, scale, SpriteEffects.None, 1);
        }

        #endregion

        //public void Dispose()
        //{
        //    spriteBatch.Dispose();
        //}
    }
}
