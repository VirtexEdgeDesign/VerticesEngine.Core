/*
namespace VerticesEngine.Graphics
{

    /// <summary>
    /// Intermediate buffer. 
    /// Optionally displays one of the intermediate buffers used 
    /// by the bloom postprocess, so you can see exactly what is
    /// being drawn into each rendertarget.
    /// </summary>
    public enum vxEnumBlurDirection
    {
        /// <summary>
        /// The pre bloom.
        /// </summary>
        PreBloom,

        /// <summary>
        /// The blurred horizontally.
        /// </summary>
        BlurredHorizontally,

        /// <summary>
        /// The blurred both ways.
        /// </summary>
        BlurredBothWays,

        /// <summary>
        /// The final result.
        /// </summary>
        FinalResult,
    }

    public class vxBloomGraphicsConfig : vxGraphicsConfig
    {
        public float BloomBlurAmount = 150;
        public float BloomThreshold = 0.5f;
        public float BloomIntensity = 2;
        public float BaseIntensity = 1;
        public float BloomSaturation = 1;
        public float BaseSaturation = 1;

        public override void VerboseOutput(int depth)
        {
            base.VerboseOutput(depth);
            //vxConsole.WriteLine(nameof(BloomBlurAmount), BloomBlurAmount, ConfigConsoleColour);
        }
    }


    public class vxDepthofFieldGraphicsConfig : vxGraphicsConfig
    {
        public float DOFFarClip = 100;
        public float DOFFocalDistance = 200;
        public float DOFFocalWidth = 40;
    }

    /// <summary>
    /// This is the final set of Post Process Passes. This includes Gaussin Bloom and Depth of Field.
    /// </summary>
    public class vxFinalScenePostProcess : vxPostProcessor
    {

        vxGameplayScene3D Scene
        {
            get { return (vxGameplayScene3D)vxEngine.Instance.CurrentScene; }
        }


        [vxShowInInspector(vxInspectorShaderCategory.DepthOfField)]
        public bool DoDepthOfField
        {
            get { return _doDepthOfField; }
            set
            {
                _doDepthOfField = value;
                Parameters["DoDepthOfField"].SetValue(value);
            }
        }
        bool _doDepthOfField;

        public RenderTarget2D DepthMap
        {
            set { Parameters["DepthMap"].SetValue(value); }
        }

        public RenderTarget2D BlurredScene
        {
            set { Parameters["BlurTexture"].SetValue(value); }
        }

        [vxShowInInspector(vxInspectorShaderCategory.DepthOfField)]
        public float DOFFarClip
        {
            get { return Parameters["FarClip"].GetValueSingle(); }
            set { Parameters["FarClip"].SetValue(value); }
        }

        [vxShowInInspector(vxInspectorShaderCategory.DepthOfField)]
        public float DOFFocalDistance
        {
            get { return Parameters["FocalDistance"].GetValueSingle(); }
            set { Parameters["FocalDistance"].SetValue(value); }
        }

        [vxShowInInspector(vxInspectorShaderCategory.DepthOfField)]
        public float DOFFocalWidth
        {
            get { return Parameters["FocalWidth"].GetValueSingle(); }
            set { Parameters["FocalWidth"].SetValue(value); }
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






        [vxShowInInspector(vxInspectorShaderCategory.Bloom)]
        public vxEnumQuality BloomQuality
        {
            get { return Settings.Graphics.Bloom.Quality; }
            set
            {
                Settings.Graphics.Bloom.Quality = value;
                Parameters["BloomQuality"].SetValue((float)Settings.Graphics.Bloom.Quality);
                //OnGraphicsRefresh();
            }
        }

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


        /// <summary>
        /// Chromatic Spread.
        /// </summary>
        [vxShowInInspector(vxInspectorShaderCategory.Bloom)]
        public Vector2 ChromaticSpread
        {
            get { return Parameters["ChromaticSpread"].GetValueVector2(); }
            set { Parameters["ChromaticSpread"].SetValue(value); }
        }




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




        public vxFinalScenePostProcess(vxRenderPipeline3D Renderer) :
        base(Renderer, "Final Scene Combine", vxInternalAssets.PostProcessShaders.BloomCombineEffect)
        {

        }

        public override void LoadContent(vxRenderPipelineConfig config)
        {
            base.LoadContent(config);

            BloomBlurAmount = config.Bloom.BloomBlurAmount;
            BloomThreshold = config.Bloom.BloomThreshold;
            BloomIntensity = config.Bloom.BloomIntensity;
            BaseIntensity = config.Bloom.BaseIntensity;

            BloomSaturation = config.Bloom.BloomSaturation;
            BaseSaturation = config.Bloom.BaseSaturation;

            DOFFarClip = config.DepthOfField.DOFFarClip;
            DOFFocalDistance = config.DepthOfField.DOFFocalDistance;
            DOFFocalWidth = config.DepthOfField.DOFFocalWidth;
        }

        public override void OnGraphicsRefresh()
        {
            base.OnGraphicsRefresh();

            BloomQuality = Settings.Graphics.Bloom.Quality;

            // Create two custom rendertargets.
            PresentationParameters pp = GraphicsDevice.PresentationParameters;


            //Blur Render Targets
            int width = pp.BackBufferWidth;
            int height = pp.BackBufferHeight;

            // Create a texture for rendering the main scene, prior to applying bloom.
            BloomSceneResult = new RenderTarget2D(GraphicsDevice, width, height, false,
                                                   pp.BackBufferFormat, pp.DepthStencilFormat, pp.MultiSampleCount,
                                                   RenderTargetUsage.DiscardContents);

            // Create two rendertargets for the bloom processing.
            width /= (5 - (int)Settings.Graphics.Bloom.Quality);
            height /= (5 - (int)Settings.Graphics.Bloom.Quality);

            BloomExtractResult = new RenderTarget2D(GraphicsDevice, width, height, false,
                pp.BackBufferFormat, DepthFormat.None);
            BloomTempRenderTarget = new RenderTarget2D(GraphicsDevice, width, height, false,
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

        public override void Apply()
        {
            if (Settings.Graphics.Bloom.Quality != vxEnumQuality.None)
            {
                vxInternalAssets.PostProcessShaders.BloomExtractEffect.Parameters["EmissiveMapTexture"].SetValue(RenderTargets.SurfaceDataMap);

                // Pass 1: draw the main scene (without edge detection) into rendertarget 1, using a
                // shader that extracts only the brightest parts of the image.

                BloomExtractEffect.Parameters["lightMap"].SetValue(RenderTargets.LightMap);
                DrawRenderTargetIntoOther("PostProcess.BloomExtract", Renderer.PeekAtCurrentTempTarget(), BloomExtractResult, BloomExtractEffect);

                // blur the scene
                GaussianBlueScene(BloomExtractResult, BloomSceneResult);

                // Pass the bloom texture in
                Parameters["BloomTexture"].SetValue(BloomSceneResult);
            }

            if (DoDepthOfField)
            {
                DOFFarClip = Scene.Camera.FarPlane;

                DepthMap = RenderTargets.DepthMap;
                BlurredScene = RenderTargets.BlurredSceneResult;
            }
            vxGraphics.GraphicsDevice.SetRenderTarget(Renderer.GetNewTempTarget("Touchup Pass"));

            DrawFullscreenQuad("PostProcess.FinalScene.Apply()", Renderer.GetCurrentTempTarget(),
                                   vxGraphics.GraphicsDevice.Viewport.Width, vxGraphics.GraphicsDevice.Viewport.Height,
                                   vxInternalAssets.PostProcessShaders.BloomCombineEffect);
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
                float sampleOffset = i * 2 + 1.5f;

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
}*/