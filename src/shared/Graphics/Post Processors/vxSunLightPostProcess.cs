
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Entities;

namespace VerticesEngine.Graphics
{
    /// <summary>
    /// Renders the sun effects such as lens flare and god rays
    /// </summary>
    public class vxSunLightPostProcess : vxRenderPass, vxIRenderPass
    {
        vxLensFlareComponent lensFlare;

        [vxGraphicalSettingsAttribute("GodRays", "God Rays", true, true)]
        public static bool IsGodRaysEnabled
        {
            get { return m_isGodRaysEnabled; }
            set { m_isGodRaysEnabled = value; }
        }
        static bool m_isGodRaysEnabled = true;

        /// <summary>
        /// The Scene Texture.
        /// </summary>
        public RenderTarget2D SceneTexture
        {
            set { Parameters["SceneTexture"].SetValue(value); }
        }


        /// <summary>
        /// Depth map of the scene.
        /// </summary>
        public RenderTarget2D DepthMap
        {
            set { SetEffectParameter("DepthMap", value); }
        }


        /// <summary>
        /// Intensity of the edge dection.
        /// </summary>
        public Matrix InverseViewProjection
        {
            set
            {
                SetEffectParameter("VX_CAMERA_INV_VP", value);
                SetEffectParameter("InverseViewProjection", value);
            }
        }

        //private vxGameplayScene3D currentScene;

        /// <summary>
        /// Density
        /// </summary>
        public float Density = 0.75f;

        /// <summary>
        /// Decay of rays.
        /// </summary>
        public float Decay = 0.85f;


        public float Weight = 1.0f;


        public float Exposure = 0.15f;


        public vxSunLightPostProcess() : base("Sunlight", vxInternalAssets.PostProcessShaders.SunGodRaysEffect)
        {
            //DepthBuffer = Effect.Parameters["DepthTexture"];

            //currentScene = vxEngine.Instance.GetCurrentScene<vxGameplayScene3D>();

            lensFlare = new vxLensFlareComponent();
            lensFlare.LoadContent();
        }
        public override void RegisterRenderTargetsForDebug()
        {
            base.RegisterRenderTargetsForDebug();

            Renderer.RegisterDebugRenderTarget("Sun Map", m_sunDepthMask);
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();

            m_sunDepthMask.Dispose();
            m_sunDepthMask = null;

            //currentScene = null;
            lensFlare = null;
        }

        RenderTarget2D m_sunDepthMask;
        public override void OnGraphicsRefresh()
        {
            base.OnGraphicsRefresh();

            //int width = Renderer.Camera.Viewport.Width;
            //int height = Renderer.Camera.Viewport.Height;

            // Create two custom rendertargets.
            PresentationParameters pp = vxGraphics.PresentationParameters;

            m_sunDepthMask = new RenderTarget2D(vxGraphics.GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, pp.BackBufferFormat, DepthFormat.None);
        }

        public void Prepare(vxCamera camera)
        {
            if (vxSkyBox.Instance.IsSunEnabled && IsGodRaysEnabled)
            {
                // First we'll draw to the temp mask
                vxGraphics.GraphicsDevice.SetRenderTarget(m_sunDepthMask);
                //vxGraphics.GraphicsDevice.BlendState = BlendState.Opaque;
                //vxGraphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                vxGraphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

                var currentScene = (vxGameplayScene3D)camera.CurrentScene;

                if (Vector3.Dot(camera.WorldMatrix.Forward, currentScene.SunEmitter.LightDirection) < 0)
                {
                    //var LightScreenSpacePos = vxGraphics.GraphicsDevice.Viewport.Project(sun.SunWorldPosition, Renderer.Camera.Projection, Renderer.Camera.View, Matrix.Identity);
                    var LightScreenSpacePos = currentScene.SunEmitter.GetScreenSpacePosition(camera);

                    float sWidth = currentScene.SunEmitter.SunTexture.Width * 2;
                    float sHeight = currentScene.SunEmitter.SunTexture.Height * 2;

                    var TextureScale = new Vector2(
                       camera.Viewport.Width / sWidth,
                       camera.Viewport.Height / sHeight);

                    var TextureSize = new Vector2((sWidth / 2) /camera.Viewport.Width, (sHeight / 2) / camera.Viewport.Height);

                    TextureSize = Vector2.One * currentScene.SunEmitter.SunSize;

                    int Width = (int)(vxInternalAssets.Textures.Texture_Sun_Glow.Width * TextureSize.X);
                    int Height = (int)(vxInternalAssets.Textures.Texture_Sun_Glow.Height * TextureSize.Y);

                    //vxSkyBox.Instance.Draw(Renderer.Camera, vxRenderPipeline.Passes.OpaquePass);

                    vxGraphics.SpriteBatch.Begin();
                    vxGraphics.SpriteBatch.Draw(currentScene.SunEmitter.SunTexture,
                        new Rectangle((int)(LightScreenSpacePos.X - Width / 2), (int)(LightScreenSpacePos.Y - Height / 2), Width, Height),
                                            Color.White);

                    vxGraphics.SpriteBatch.End();



                    // Screen Space Position Normalised against Viewport Width and Height
                    Vector2 lighScreenSourcePos = new Vector2(
                        LightScreenSpacePos.X / vxGraphics.GraphicsDevice.Viewport.Width,
                        LightScreenSpacePos.Y / vxGraphics.GraphicsDevice.Viewport.Height);

                    vxConsole.WriteToScreen("Sun Pos", LightScreenSpacePos, Color.Orange);

                    vxInternalAssets.PostProcessShaders.LensFlareEffect.Parameters["lightScreenPosition"].SetValue(lighScreenSourcePos);

                    vxConsole.WriteToScreen("lighScreenSourcePos", lighScreenSourcePos, Color.Orange);
                    Effect.CurrentTechnique = Effect.Techniques["GenerateSunMaskTechnique"];

                    SetEffectParameter("TextureSize", TextureSize * currentScene.SunEmitter.SunSize);
                    SetEffectParameter("TextureScale", TextureScale * currentScene.SunEmitter.SunSize);
                    SetEffectParameter("SunTexture", vxInternalAssets.Textures.Texture_Sun_Glow);

                    Effect.Parameters["DepthMap"].SetValue(Renderer.DepthMap);
                    Parameters["lightScreenPosition"].SetValue(lighScreenSourcePos);


                    Effect.CurrentTechnique = Effect.Techniques["GenerateSunMaskTechnique"];
                    foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        Renderer.RenderQuad(Vector2.One * -1, Vector2.One);
                    }
                }
            }
        }


        public void Apply(vxCamera camera)
        {
            if (vxSkyBox.Instance.IsSunEnabled && IsGodRaysEnabled)
            {
                vxGraphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                vxGraphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                vxGraphics.GraphicsDevice.SetRenderTarget(Renderer.GetNewTempTarget("GodRays"));

                DepthMap = Renderer.DepthMap;
                InverseViewProjection = Matrix.Invert(camera.ViewProjection);

                if (((vxGameplayScene3D)camera.CurrentScene).WorldProperties != null)
                {
                    //Parameters["DepthMap"].SetValue(Renderer.DepthMap);
                    SetEffectParameter("IlluminationDecay", 0.75f);
                    SetEffectParameter("Density", Density);
                    SetEffectParameter("Decay", Decay);
                    SetEffectParameter("Weight", Weight);
                }
                SetEffectParameter("SceneTexture", Renderer.GetCurrentTempTarget());
                SetEffectParameter("SunMaskTexture", m_sunDepthMask);
                SetEffectParameter("VX_CAMERA_POS", camera.Position);
                SetEffectParameter("VX_ProjectionParams", camera.Util_VX_ProjectionParams);

                Effect.CurrentTechnique = Effect.Techniques["Technique_ApplyEffect"];

                foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    Renderer.RenderQuad(Vector2.One * -1, Vector2.One);
                }



                vxInternalAssets.PostProcessShaders.LensFlareEffect.Parameters["DepthMap"].SetValue(Renderer.DepthMap);
                vxInternalAssets.PostProcessShaders.LensFlareEffect.Parameters["MatrixTransform"].SetValue(MatrixTransform);
                vxInternalAssets.PostProcessShaders.LensFlareEffect.Parameters["HalfPixel"].SetValue(HalfPixel);

                lensFlare.View = camera.View;
                lensFlare.Projection = camera.Projection;
                lensFlare.Draw();
            }
        }
    }
}
