
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.ContentManagement;

namespace VerticesEngine.Graphics
{
    /// <summary>
    /// Grab a scene that has already been rendered, 
    /// and add a distortion effect over the top of it.
    /// </summary>
    public class vxFogPostProcess : vxRenderPass, vxIRenderPass
    {
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
        /// Depth map of the scene.
        /// </summary>
        public RenderTarget2D DataMap
        {
            set { SetEffectParameter("DepthMap", value); }
        }


        /// <summary>
        /// Intensity of the edge dection.
        /// </summary>
        public Matrix InverseViewProjection
        {
            set {
                SetEffectParameter("VX_CAMERA_INV_VP", value);
                SetEffectParameter("VX_MATRIX_INV_VP", value);
                SetEffectParameter("InverseViewProjection", value);
            }
        }

        //private vxGameplayScene3D scene;

        

        public vxFogPostProcess() : base("Fog", vxInternalAssets.PostProcessShaders.FogPostProcessShader)
        {
            DepthBuffer = Effect.Parameters["DepthTexture"];
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();

            //scene = null;
            DepthBuffer = null;

        }


        public override void OnGraphicsRefresh()
        {
            base.OnGraphicsRefresh();
        }

        public void Prepare(vxCamera camera)
        {

        }

        EffectParameter DepthBuffer;

        bool isValid = false;
        Texture2D heightMaptxtr;
        Vector2 windOffset = Vector2.Zero;
        public void Apply(vxCamera camera)
        {
            var scene = (vxGameplayScene3D)camera.CurrentScene;

            if (scene.WorldProperties != null)
            {
                isValid = true;
                //Parameters["DepthMap"].SetValue(Renderer.DepthMap);
                SetEffectParameter("FogNear", scene.WorldProperties.FogStartPosition);
                SetEffectParameter("FogFar", scene.WorldProperties.FogThickness);
                SetEffectParameter("FogColor", scene.WorldProperties.FogColour);

                SetEffectParameter("FogHeight", scene.WorldProperties.FogHeight);
                SetEffectParameter("FogHeightDepth", scene.WorldProperties.FogHeightDepth);
                SetEffectParameter("FogHeightNear", scene.WorldProperties.FogHeightStart);
                SetEffectParameter("FogHeightFar", scene.WorldProperties.FogHeightThickness);
                SetEffectParameter("isFogDepthEnabled", scene.WorldProperties.IsFogHeightEnabled ? 1f : 0f);
                SetEffectParameter("FogWindSpeed", windOffset);
                SetEffectParameter("FogHeightMapFactor", 0);

                windOffset += Vector2.UnitX * vxTime.DeltaTime/3;

                if (heightMaptxtr == null)
                    heightMaptxtr = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/terrain/Heightmap");

                SetEffectParameter("HeightMapTexture", heightMaptxtr);
            }
            else
            {
                SetEffectParameter("isFogDepthEnabled", 0f);
            }
            if (isValid == false || (scene.WorldProperties != null && !scene.WorldProperties.IsFogEnabled))
                return;

            var Scene = vxEngine.Instance.CurrentScene;
            vxGraphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            vxGraphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            vxGraphics.GraphicsDevice.SetRenderTarget(Renderer.GetNewTempTarget("Fog"));

            //DepthMap = Renderer.AuxDepthMap;
            SetEffectParameter("EmissiveMapTexture", Renderer.SurfaceDataMap);
            //InverseViewProjection = Matrix.Invert(camera.ViewProjection);

            DepthMap = Renderer.AuxDepthMap;
            InverseViewProjection = Matrix.Invert(camera.ViewProjection);

            SetEffectParameter("VX_CAMERA_POS",camera.Position);
            SetEffectParameter("VX_ProjectionParams", camera.Util_VX_ProjectionParams);
            

            Effect.CurrentTechnique = Effect.Techniques["Technique_Fog"];
            vxGraphics.SpriteBatch.Begin("PostProcess.Technique_Fog()", 0, BlendState.Opaque, null, null, null, Effect);
            vxGraphics.SpriteBatch.Draw(Renderer.GetCurrentTempTarget(), Vector2.Zero, Color.White);
            vxGraphics.SpriteBatch.End();
        }
    }
}
