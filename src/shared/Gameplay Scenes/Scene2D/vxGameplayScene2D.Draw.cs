using FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.DebugUtilities;
using VerticesEngine.Diagnostics;
using VerticesEngine.Graphics;

namespace VerticesEngine
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public partial class vxGameplayScene2D : vxGameplaySceneBase
    {
        /// <summary>
        /// The debug view.
        /// </summary>
		protected vxFarseerDebugView DebugView;


        /*
        /// <summary>
        /// Draws the game from background to foreground.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void DrawScene(GameTime gameTime)
        {
            //Renderer.Prepare();

            DrawBackground();

            PreDraw();

            //Renderer.Draw();

            base.DrawScene(gameTime);

            PostDraw();

            //Renderer.ApplyPostProcessors();
        }
        */

        public override void DrawScene()
        {
            base.DrawScene();
        }


        //public override RenderTarget2D GetFinalSceneRenderTarget()
        //{
        //    //return Renderer.Finalise();
        //    return null;
        //}

        private void EnableOrDisableFlag(DebugViewFlags flag)
        {
            if ((DebugView.Flags & flag) == flag)
                DebugView.RemoveFlags(flag);
            else
                DebugView.AppendFlags(flag);
        }


        /// <summary>
        /// Draws the shadowed string.
        /// </summary>
        /// <param name="font">Font.</param>
        /// <param name="value">Value.</param>
        /// <param name="position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
        /// <param name="color">Color.</param>
        public virtual void DrawShadowedString(SpriteFont font, string value, Vector2 position, Color color)
        {
            vxGraphics.SpriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            vxGraphics.SpriteBatch.DrawString(font, value, position, color);
        }

        protected internal override void DrawDebug()
        {
            base.DrawDebug();

            for (int c = 0; c < Cameras.Count; c++)
            {
                DebugView.RenderDebugData(ref Cameras[c].CastAs<vxCamera2D>().SimProjection, ref Cameras[c].CastAs<vxCamera2D>().SimView);

                vxDebug.DrawShapes(Cameras[c].View, Cameras[c].Projection);
            }
        }
        /// <summary>
        /// Render the shadow map texture to the screen
        /// </summary>
        //void DrawDebugRenderTargetsToScreen()
        //{
        //    SpriteFont font = vxInternalAssets.Fonts.DebugFont;


        //    if (vxInput.IsNewKeyPress(Keys.OemPlus))
        //        debugRTPos -= Vector2.UnitX * debugRTWidth;
        //    else if (vxInput.IsNewKeyPress(Keys.OemMinus))
        //        debugRTPos += Vector2.UnitX * debugRTWidth;


        //    // Clamp Debug Position
        //    debugRTPos.X = MathHelper.Clamp(debugRTPos.X, -debugRTWidth * rtDb_count, 0);
        //    rtDb_count = 0;

        //    vxGraphics.SpriteBatch.Begin("Debug.RenderTargetsDump", 0, BlendState.Opaque, SamplerState.PointClamp, null, null);


        //    vxGraphics.SpriteBatch.Draw(vxInternalAssets.Textures.Blank,
        //        new Rectangle(0, 0, vxGraphics.GraphicsDevice.Viewport.Width, debugRTHeight + font.LineSpacing + 2 * debugRTPadding), Color.Black * 0.5f);

        //    DrawRT(nameof(MainSceneResult), MainSceneResult);

        //    DrawRT(nameof(Renderer.DistortionPostProcess.DistortionMap), Renderer.DistortionPostProcess.DistortionMap);

        //    for (int i = 0; i < Renderer.TempTargetsUsed; i++)
        //    {
        //        var trgt = Renderer.PostProcessTargets[i];
        //        if (trgt != null)
        //            DrawRT(trgt.Name, trgt);
        //    }


        //    vxGraphics.SpriteBatch.End();
        //}

    }
}