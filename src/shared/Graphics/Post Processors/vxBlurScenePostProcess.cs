using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VerticesEngine;

/*
namespace VerticesEngine.Graphics
{

    public class vxBlurUtilityGraphicsConfig : vxGraphicsConfig
    {
        public float UtilityBlurAmount = 5;
    }

    public class vxBlurScenePostProcess : vxPostProcessor
    {
        /// <summary>
        /// Controls how much blurring is applied to the bloom image.
        /// The typical range is from 1 up to 10 or so.
        /// </summary>
        public float UtilityBlurAmount = 2;

        float[] sampleWeightsX, sampleWeightsY;// = new float[sampleCount];
        Vector2[] sampleOffsetsX, sampleOffsetsY;// = new Vector2[sampleCount];

        int SampleCount = 15;

        public vxBlurScenePostProcess(vxRenderPipeline3D Renderer) :
        base(Renderer, "Gaussian Blur", vxInternalAssets.PostProcessShaders.GaussianBlurEffect)
        {

        }

        public override void LoadContent(vxRenderPipelineConfig config)
        {
            base.LoadContent(config);

            UtilityBlurAmount = config.Blur.UtilityBlurAmount;
        }

        public override void OnGraphicsRefresh()
        {
            base.OnGraphicsRefresh();

            //First Setup the Sample Count
            SampleCount = Effect.Parameters["SampleWeights"].Elements.Count;

            sampleWeightsX = new float[SampleCount];
            sampleOffsetsX = new Vector2[SampleCount];

            sampleWeightsY = new float[SampleCount];
            sampleOffsetsY = new Vector2[SampleCount];

            //Now Generate the Sample Weights and Sample Offsets for this given Resolution
            SetBloomEffectParameters(1.0f / (float)RenderTargets.BlurredSceneResult.Width, 0, UtilityBlurAmount,
                vxEnumBlurDirection.BlurredHorizontally, SampleCount);


            SetBloomEffectParameters(0, 1.0f / (float)RenderTargets.BlurredSceneResult.Height, UtilityBlurAmount,
                vxEnumBlurDirection.BlurredBothWays, SampleCount);
        }



        public override void Apply()
        {
            GaussianBlueScene(Renderer.PeekAtCurrentTempTarget(), RenderTargets.BlurredSceneResult);
        }

        public void GaussianBlueScene(RenderTarget2D SceneToBlur, RenderTarget2D BlurredScene)
        {

            vxRenderPipeline3D renderer = (vxRenderPipeline3D)this.Renderer;
            //Set First Temp Blur Rendertarget 
            Effect.Parameters["SampleWeights"].SetValue(sampleWeightsX);
            Effect.Parameters["SampleOffsets"].SetValue(sampleOffsetsX);

            DrawRenderTargetIntoOther("Blur.HorzPass", SceneToBlur, renderer.FinalScenePostProcess.BloomTempRenderTarget, Effect);

            Effect.Parameters["SampleWeights"].SetValue(sampleWeightsY);
            Effect.Parameters["SampleOffsets"].SetValue(sampleOffsetsY);

            DrawRenderTargetIntoOther("Blur.VertPass", renderer.FinalScenePostProcess.BloomTempRenderTarget, BlurredScene, Effect);
        }
        public void SetBloomEffectParameters(float dx, float dy, float theta, vxEnumBlurDirection BlurDirection, int sampleCount)
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
        public float ComputeBloomGaussian(float n, float theta)
        {
            return (float)((1.0 / Math.Sqrt(2 * Math.PI * theta)) *
                           Math.Exp(-(n * n) / (2 * theta * theta)));
        }
    }
}
*/