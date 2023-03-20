using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VerticesEngine.Graphics
{
    /// <summary>
    /// Grab a scene that has already been rendered, 
    /// and add a distortion effect over the top of it.
    /// </summary>
    public class vxCameraMotionBlurPostProcess : vxRenderPass, vxIRenderPass
    {
        [vxGraphicalSettings("MotionBlur.Enabled", isMenuSetting: true, usage: vxGameEnviromentType.ThreeDimensional)]
        public static bool IsMotionBlurEnabled
        {
            get { return _isMotionBlurEnabled; }
            set { _isMotionBlurEnabled = value; }
        }

        private static bool _isMotionBlurEnabled = true;

        [vxGraphicalSettings("MotionBlur.Strength")]
        public static float MotionBlurStrength = 1;

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
        /// Sets the view projection.
        /// </summary>
        /// <value>The view projection.</value>
        public Matrix PreviousViewProjection
        {
            set { SetEffectParameter("PrevViewProjection", value); }
        }

        /// <summary>
        /// Intensity of the edge dection.
        /// </summary>
        public Matrix InverseViewProjection
        {
            set
            {
                SetEffectParameter("VX_CAMERA_INV_VP", value);
                SetEffectParameter("VX_MATRIX_INV_VP", value);
                
            }
        }

        public Vector2 MotionBlurFactor
        {
            set { SetEffectParameter("BlurLength", value); }
        }

        public Vector3[] RAND_SAMPLES
        {
            set
            {
                if (value != null)
                    if (Parameters["RAND_SAMPLES"] != null)
                        Parameters["RAND_SAMPLES"].SetValue(value);
            }
        }

        public vxCameraMotionBlurPostProcess() : base("Camera Motion Blur", vxInternalAssets.PostProcessShaders.CameraMotionBlurEffect)
        {
            RAND_SAMPLES = new Vector3[]
            {
                new Vector3(0.5381f, 0.1856f, -0.4319f),
                new Vector3(0.1379f, 0.2486f, 0.4430f),
                new Vector3(0.3371f, 0.5679f, -0.0057f),
                new Vector3(-0.6999f, -0.0451f, -0.0019f),
                new Vector3(0.0689f, -0.1598f, -0.8547f),
                new Vector3(0.0560f, 0.0069f, -0.1843f),
                new Vector3(-0.0146f, 0.1402f, 0.0762f),
                new Vector3(0.0100f, -0.1924f, -0.0344f),
                new Vector3(-0.3577f, -0.5301f, -0.4358f),
                new Vector3(-0.3169f, 0.1063f, 0.0158f),
                new Vector3(0.0103f, -0.5869f, 0.0046f),
                new Vector3(-0.0897f, -0.4940f, 0.3287f),
                new Vector3(0.7119f, -0.0154f, -0.0918f),
                new Vector3(-0.0533f, 0.0596f, -0.5411f),
                new Vector3(0.0352f, -0.0631f, 0.5460f),
                new Vector3(-0.4776f, 0.2847f, -0.0271f)
            };
        }


        public override void OnGraphicsRefresh()
        {
            base.OnGraphicsRefresh();
            //vxInternalAssets.PostProcessShaders.CameraMotionBlurEffect.Parameters["MatrixTransform"].SetValue(MatrixTransform);
        }

        public void Prepare(vxCamera camera)
        {
        }


        public void Apply(vxCamera camera)
        {
            if (IsMotionBlurEnabled && IsEnabled &&
                vxEngine.Instance.CurrentScene.SandboxCurrentState == vxEnumSandboxStatus.Running)
            {
                var Scene = vxEngine.Instance.CurrentScene;
                vxGraphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                vxGraphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                vxGraphics.GraphicsDevice.SetRenderTarget(Renderer.GetNewTempTarget("Motion Blur"));

                DepthMap = Renderer.AuxDepthMap;
                vxGraphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;

                PreviousViewProjection = camera.PreviousViewProjection;

                InverseViewProjection = Matrix.Invert(camera.ViewProjection);

                Parameters["SceneTextureSampler"].SetValue(Renderer.GetCurrentTempTarget());
                Parameters["DepthMap"].SetValue(Renderer.AuxDepthMap);
                Parameters["MaskTexture"].SetValue(Renderer.EntityMaskValues);
                Parameters["BlurFactor"].SetValue(5.0f);
                MotionBlurFactor = MotionBlurStrength * Vector2.One * 2.5f;


                Effect.CurrentTechnique = Effect.Techniques["Technique_CameraMotionBlur"];
                vxGraphics.SpriteBatch.Begin("PostProcess.Technique_CameraMotionBlur()", 0, BlendState.Opaque, null,
                    null, null, Effect);
                vxGraphics.SpriteBatch.Draw(Renderer.GetCurrentTempTarget(), Vector2.Zero, Color.White);
                vxGraphics.SpriteBatch.End();
            }
            //else do nothing
        }
    }
}