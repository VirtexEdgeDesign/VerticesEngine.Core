
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace VerticesEngine.Graphics
{
    /// <summary>
    /// Takes the current scene and applies Bloom, Depth of Field and Chromatic Aboration to that scene
    /// </summary>
    public class vxBlurEffectsPostProcess : vxRenderPass, vxIRenderPass
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


        Effect BlurEffect
        {
            get { return vxInternalAssets.PostProcessShaders.GaussianBlurEffect; }
        }

        Effect BloomExtractEffect
        {
            get { return vxInternalAssets.PostProcessShaders.BloomExtractEffect; }
        }


        EffectParameterCollection BloomExtractParameters
        {
            get { return vxInternalAssets.PostProcessShaders.BloomExtractEffect.Parameters; }
        }

        EffectParameterCollection BloomCombineParameters
        {
            get { return vxInternalAssets.PostProcessShaders.BloomCombineEffect.Parameters; }
        }


        [vxGraphicalSettings("Bloom.Quality", isMenuSetting: true, usage: vxGameEnviromentType.ThreeDimensional)]
        public static vxEnumQuality BloomQuality
        {
            get { return _bloomQuality; }
            set { _bloomQuality = value; }
        }
       static vxEnumQuality _bloomQuality = vxEnumQuality.Ultra;

        /// <summary>
        /// Controls how much blurring is applied to the bloom image.
        /// The typical range is from 1 up to 10 or so.
        /// </summary>
        [vxShowInInspector(vxInspectorShaderCategory.Bloom)]
        public float BloomBlurAmount
        {
            get { return _blurAmount; }
            set
            {
                _blurAmount = value;
                OnGraphicsRefresh();
            }
        }
        float _blurAmount = 5;


        [vxShowInInspector(vxInspectorShaderCategory.Bloom)]
        public bool DoFullSceneBloom
        {
            get { return BloomExtractEffect.Parameters["DoFullSceneBloom"].GetValueBoolean(); }
            set { BloomExtractEffect.Parameters["DoFullSceneBloom"].SetValue(value); }
        }


        /// <summary> 
        /// Controls how bright a pixel needs to be before it will bloom.
        /// Zero makes everything bloom equally, while higher values select
        /// only brighter colors. Somewhere between 0.25 and 0.5 is good.
        /// </summary>
        [vxShowInInspector(vxInspectorShaderCategory.Bloom)]
        public float BloomThreshold
        {
            get { return BloomExtractParameters["BloomThreshold"].GetValueSingle(); }
            set { BloomExtractParameters["BloomThreshold"].SetValue(value); }
        }

        /// <summary>
        /// Controls the amount of the bloom and base images that
        /// will be mixed into the final scene. Range 0 to 1.
        /// </summary>
        [vxShowInInspector(vxInspectorShaderCategory.Bloom)]
        public float BloomIntensity
        {
            get { return Parameters["BloomIntensity"].GetValueSingle(); }
            set { Parameters["BloomIntensity"].SetValue(value); }
        }

        /// <summary>
        /// The base intensity.
        /// </summary>
        [vxShowInInspector(vxInspectorShaderCategory.Bloom)]
        public float BaseIntensity
        {
            get { return Parameters["BaseIntensity"].GetValueSingle(); }
            set { Parameters["BaseIntensity"].SetValue(value); }
        }


        /// <summary>
        /// Independently control the color saturation of the bloom and
        /// base images. Zero is totally desaturated, 1.0 leaves saturation
        /// unchanged, while higher values increase the saturation level.
        /// </summary>
        [vxShowInInspector(vxInspectorShaderCategory.Bloom)]
        public float BloomSaturation
        {
            get { return Parameters["BloomSaturation"].GetValueSingle(); }
            set { Parameters["BloomSaturation"].SetValue(value); }
        }

        /// <summary>
        /// The base saturation.
        /// </summary>
        [vxShowInInspector(vxInspectorShaderCategory.Bloom)]
        public float BaseSaturation
        {
            get { return Parameters["BaseSaturation"].GetValueSingle(); }
            set { Parameters["BaseSaturation"].SetValue(value); }
        }


        float[] sampleWeightsX, sampleWeightsY;// = new float[sampleCount];
        Vector2[] sampleOffsetsX, sampleOffsetsY;// = new Vector2[sampleCount];

        int SampleCount = 1;

        private vxEnumQuality quality = vxEnumQuality.Ultra;

        // Bloom Render Targets
        // **********************************************

        /// <summary>
        /// Render target which holds the final Gaussian Bloom Scene
        /// </summary>
        public RenderTarget2D BloomSceneResult;

        /// <summary>
        /// TempRender target for blurring
        /// </summary>
        public RenderTarget2D BloomExtractResult;

        /// <summary>
        /// TempRender target for blurring
        /// </summary>
        public RenderTarget2D BloomTempRenderTarget;

        public Vector3[] RAND_SAMPLES
        {
            set
            {
                if (value != null)
                    if (Parameters["RAND_SAMPLES"] != null)
                        Parameters["RAND_SAMPLES"].SetValue(value);
            }
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();

            BloomSceneResult.Dispose();
            BloomSceneResult = null;

            BloomExtractResult.Dispose();
            BloomExtractResult = null;

            BloomTempRenderTarget.Dispose();
            BloomTempRenderTarget = null;
        }

        public vxBlurEffectsPostProcess() : base("Scene Blur Effects", vxInternalAssets.PostProcessShaders.BloomCombineEffect)
        {
            RAND_SAMPLES = new Vector3[]
            {
                      new Vector3( 0.5381f, 0.1856f,-0.4319f),
      new Vector3( 0.1379f, 0.2486f, 0.4430f),
      new Vector3( 0.3371f, 0.5679f,-0.0057f),
      new Vector3(-0.6999f,-0.0451f,-0.0019f),
      new Vector3( 0.0689f,-0.1598f,-0.8547f),
      new Vector3( 0.0560f, 0.0069f,-0.1843f),
      new Vector3(-0.0146f, 0.1402f, 0.0762f),
      new Vector3( 0.0100f,-0.1924f,-0.0344f),
      new Vector3(-0.3577f,-0.5301f,-0.4358f),
      new Vector3(-0.3169f, 0.1063f, 0.0158f),
      new Vector3( 0.0103f,-0.5869f, 0.0046f),
      new Vector3(-0.0897f,-0.4940f, 0.3287f),
      new Vector3( 0.7119f,-0.0154f,-0.0918f),
      new Vector3(-0.0533f, 0.0596f,-0.5411f),
      new Vector3( 0.0352f,-0.0631f, 0.5460f),
      new Vector3(-0.4776f, 0.2847f,-0.0271f)
        };
        }

        public override void LoadContent()
        {
            base.LoadContent();

            BloomBlurAmount = 15;
            BloomThreshold = .75f;
            BloomIntensity = 0.5f;
            BaseIntensity = .25f;

            BloomSaturation = 1.0f;
            BaseSaturation = 0.5f;

            //DOFFarClip = config.DepthOfField.DOFFarClip;
            //DOFFocalDistance = config.DepthOfField.DOFFocalDistance;
            //DOFFocalWidth = config.DepthOfField.DOFFocalWidth;
        }

        public override void RegisterRenderTargetsForDebug()
        {
            base.RegisterRenderTargetsForDebug();

            Renderer.RegisterDebugRenderTarget("BloomExtractResult", BloomExtractResult);
            Renderer.RegisterDebugRenderTarget("BloomTempRenderTarget", BloomTempRenderTarget);
            Renderer.RegisterDebugRenderTarget("BloomSceneResult", BloomSceneResult);

        }

        public override void OnGraphicsRefresh()
        {
            base.OnGraphicsRefresh();

            //BloomQuality = vxEnumQuality.Medium;
            quality = _bloomQuality;
            // Create two custom rendertargets.
            PresentationParameters pp = vxGraphics.PresentationParameters;


            var viewport = vxGraphics.FinalViewport;

            //Blur Render Targets
            int width = viewport.Width;
            int height = viewport.Height;

            // Create a texture for rendering the main scene, prior to applying bloom.
            BloomSceneResult = new RenderTarget2D(vxGraphics.GraphicsDevice, width, height, false,
                                                   pp.BackBufferFormat, pp.DepthStencilFormat, pp.MultiSampleCount,
                                                   RenderTargetUsage.DiscardContents);

            // Create two rendertargets for the bloom processing.
            width /= (5 - (int)BloomQuality);
            height /= (5 - (int)BloomQuality);

            BloomExtractResult = new RenderTarget2D(vxGraphics.GraphicsDevice, width, height, false,
                pp.BackBufferFormat, DepthFormat.None);
            BloomTempRenderTarget = new RenderTarget2D(vxGraphics.GraphicsDevice, width, height, false,
                pp.BackBufferFormat, DepthFormat.None);




            //First Setup the Sample Count
            SampleCount = BlurEffect.Parameters["SampleWeights"].Elements.Count;

            sampleWeightsX = new float[SampleCount];
            sampleOffsetsX = new Vector2[SampleCount];

            sampleWeightsY = new float[SampleCount];
            sampleOffsetsY = new Vector2[SampleCount];

            //Now Generate the Sample Weights and Sample Offsets for this given Resolution
            SetBloomEffectParameters(1.0f / (float)BloomTempRenderTarget.Width, 0, BloomBlurAmount,
                vxEnumBlurDirection.BlurredHorizontally, SampleCount);


            SetBloomEffectParameters(0, 1.0f / (float)BloomTempRenderTarget.Height, BloomBlurAmount,
                vxEnumBlurDirection.BlurredBothWays, SampleCount);
        }

        public void Prepare(vxCamera camera)
        {
            if (quality != _bloomQuality)
            {
                OnGraphicsRefresh();
            }
        }

        public void GaussianBlueScene(RenderTarget2D SceneToBlur, RenderTarget2D BlurredScene)
        {
            //Set First Temp Blur Rendertarget 
            BlurEffect.Parameters["SampleWeights"].SetValue(sampleWeightsX);
            BlurEffect.Parameters["SampleOffsets"].SetValue(sampleOffsetsX);

            DrawRenderTargetIntoOther("Bloom.HorzPass", SceneToBlur, BloomTempRenderTarget, BlurEffect);

            BlurEffect.Parameters["SampleWeights"].SetValue(sampleWeightsY);
            BlurEffect.Parameters["SampleOffsets"].SetValue(sampleOffsetsY);

            DrawRenderTargetIntoOther("Bloom.HorzPass", BloomTempRenderTarget, BlurredScene, BlurEffect);
        }
        

        public void Apply(vxCamera camera)
        {
            if (quality != vxEnumQuality.None)
            {
                vxInternalAssets.PostProcessShaders.BloomExtractEffect.Parameters["EmissiveMapTexture"]
                    .SetValue(Renderer.SurfaceDataMap);

                // Pass 1: draw the main scene (without edge detection) into rendertarget 1, using a
                // shader that extracts only the brightest parts of the image.
                BloomExtractEffect.Parameters["BloomThreshold"].SetValue(0.995f);
                Parameters["BloomQuality"].SetValue((float)_bloomQuality);
                //BloomExtractEffect.Parameters["DoFullSceneBloom"].SetValue(false);
                Parameters["BloomIntensity"].SetValue(1.25f);
                Parameters["BaseIntensity"].SetValue(1.0f);
                Parameters["BloomSaturation"].SetValue(1.0f);
                Parameters["BaseSaturation"].SetValue(1f);
                //BloomExtractEffect.Parameters["lightMap"].SetValue(Renderer.LightMap);
                DrawRenderTargetIntoOther("PostProcess.BloomExtract", Renderer.GetCurrentTempTarget(),
                    BloomExtractResult, BloomExtractEffect);

                // blur the scene
                GaussianBlueScene(BloomExtractResult, BloomSceneResult);

                // Pass the bloom texture in
                Parameters["BloomTexture"].SetValue(BloomSceneResult);
                
                vxGraphics.GraphicsDevice.SetRenderTarget(Renderer.GetNewTempTarget("Touchup Pass"));
                
                Parameters["SceneTexture"].SetValue(Renderer.GetCurrentTempTarget());
                
                Effect.CurrentTechnique = Effect.Techniques["BloomCombine"];
                vxGraphics.SpriteBatch.Begin("PostProcess.BloomCombine()", 0, BlendState.Opaque, null, null, null,
                    Effect);
                vxGraphics.SpriteBatch.Draw(Renderer.GetCurrentTempTarget(), vxGraphics.GraphicsDevice.Viewport.Bounds,
                    Color.White);
                vxGraphics.SpriteBatch.End();
            }
        }



        void SetBloomEffectParameters(float dx, float dy, float theta, vxEnumBlurDirection BlurDirection, int sampleCount)
        {
            // Create temporary arrays for computing our filter settings.
            float[] sampleWeights = new float[sampleCount];
            Vector2[] sampleOffsets = new Vector2[sampleCount];

            // The first sample always has a zero offset.
            sampleWeights[0] = ComputeBloomGaussian(0, theta);
            sampleOffsets[0] = new Vector2(0);

            // Maintain a sum of all the weighting values.
            float totalWeights = sampleWeights[0];

            // Add pairs of additional sample taps, positioned
            // along a line in both directions from the center.
            for (int i = 0; i < sampleCount / 2; i++)
            {
                // Store weights for the positive and negative taps.
                float weight = ComputeBloomGaussian(i + 1, theta);

                sampleWeights[i * 2 + 1] = weight;
                sampleWeights[i * 2 + 2] = weight;

                totalWeights += weight * 2;

                // To get the maximum amount of blurring from a limited number of
                // pixel shader samples, we take advantage of the bilinear filtering
                // hardware inside the texture fetch unit. If we position our texture
                // coordinates exactly halfway between two texels, the filtering unit
                // will average them for us, giving two samples for the price of one.
                // This allows us to step in units of two texels per sample, rather
                // than just one at a time. The 1.5 offset kicks things off by
                // positioning us nicely in between two texels.
                float sampleOffset = i * 4 + 1.5f;

                Vector2 delta = new Vector2(dx, dy) * sampleOffset;

                // Store texture coordinate offsets for the positive and negative taps.
                sampleOffsets[i * 2 + 1] = delta;
                sampleOffsets[i * 2 + 2] = -delta;
            }

            // Normalize the list of sample weightings, so they will always sum to one.
            for (int i = 0; i < sampleWeights.Length; i++)
            {
                sampleWeights[i] /= totalWeights;
            }

            // Tell the effect about our new filter settings.
            if (BlurDirection == vxEnumBlurDirection.BlurredHorizontally)
            {
                sampleWeightsX = sampleWeights;
                sampleOffsetsX = sampleOffsets;
            }
            else
            {
                sampleWeightsY = sampleWeights;
                sampleOffsetsY = sampleOffsets;
            }
        }


        /// <summary>
        /// Evaluates a single point on the gaussian falloff curve.
        /// Used for setting up the blur filter weightings.
        /// </summary>
        float ComputeBloomGaussian(float n, float theta)
        {
            return (float)((1.0 / Math.Sqrt(2 * Math.PI * theta)) *
                           Math.Exp(-(n * n) / (2 * theta * theta)));
        }
    }
}
