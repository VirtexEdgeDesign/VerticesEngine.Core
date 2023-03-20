using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VerticesEngine;
using VerticesEngine.Input;

namespace VerticesEngine.Graphics
{
    public enum vxAntiAliasType
    {
        None,
        FXAA,
        //TXAA
    }


    public class vxAntiAliasPostProcess : vxRenderPass, vxIRenderPass
    {
        #region -- Settings --
#if __MOBILE__
        [vxGraphicalSettingsAttribute("AntiAliasType", "Anti Alias Type", true, true, vxGameEnviromentType.ThreeDimensional)]
#else
        //[vxGraphicalSettingsAttribute("AntiAliasType", "Anti Alias Type", true, true, vxGameEnviromentType.TwoDimensional | vxGameEnviromentType.ThreeDimensional)]
#endif
        public static vxAntiAliasType AntiAliasType
        {
            get { return _antiAliasType; }
            set { _antiAliasType = value; }
        }
        static vxAntiAliasType _antiAliasType  = vxAntiAliasType.FXAA;

#endregion

        /// <summary>
        /// Normal Map.
        /// </summary>
        //public RenderTarget2D NormalMap
        //{
        //    set { Parameters["NormalTexture"].SetValue(value); }
        //}



        /// <summary>
        // This effects sub-pixel AA quality and inversely sharpness.
        //   Where N ranges between,
        //     N = 0.50 (default)
        //     N = 0.33 (sharper)
        /// </summary>
        [vxGraphicalSettingsAttribute("FXAA.SubPixelQuality", "Anti Alias Type")]
        public static float N = 0.40f;


        // Choose the amount of sub-pixel aliasing removal.
        // This can effect sharpness.
        //   1.00 - upper limit (softer)
        //   0.75 - default amount of filtering
        //   0.50 - lower limit (sharper, less sub-pixel aliasing removal)
        //   0.25 - almost off
        //   0.00 - completely off
        [vxGraphicalSettingsAttribute("FXAA.SubPixelAliasingRemoval", "Anti Alias Type")]
        public static float SubPixelAliasingRemoval
        {
            get { return _subPixelAliasingRemoval; }
            set
            {
                _subPixelAliasingRemoval = (int)MathHelper.Clamp(value, 0, 1);
                //Parameters["SubPixelAliasingRemoval"].SetValue(_subPixelAliasingRemoval);
            }
        }
        private static float _subPixelAliasingRemoval = 0.75f;

        // The minimum amount of local contrast required to apply algorithm.
        //   0.333 - too little (faster)
        //   0.250 - low quality
        //   0.166 - default
        //   0.125 - high quality 
        //   0.063 - overkill (slower)
        [vxShowInInspector(vxInspectorShaderCategory.AntiAliasing)]
        [vxGraphicalSettingsAttribute("FXAA.EdgeTheshold", "Anti Alias Type")]
        public static float EdgeTheshold
        {
            get { return _edgeTheshold; }
            set
            {
                _edgeTheshold = (int)MathHelper.Clamp(value, 0.063f, 0.333f);
                
               // Parameters["EdgeTheshold"].SetValue(_subPixelAliasingRemoval);
            }
        }
        private static float _edgeTheshold = 0.166f;

        // Trims the algorithm from processing darks.
        //   0.0833 - upper limit (default, the start of visible unfiltered edges)
        //   0.0625 - high quality (faster)
        //   0.0312 - visible limit (slower)
        // Special notes when using FXAA_GREEN_AS_LUMA,
        //   Likely want to set this to zero.
        //   As colors that are mostly not-green
        //   will appear very dark in the green channel!
        //   Tune by looking at mostly non-green content,
        //   then start at zero and increase until aliasing is a problem.
        [vxShowInInspector(vxInspectorShaderCategory.AntiAliasing)]
        [vxGraphicalSettingsAttribute("FXAA.EdgeThesholdMin", "Anti Alias Type")]
        public static float EdgeThesholdMin
        {
            get { return _edgeThesholdMin; }
            set
            {
                _edgeThesholdMin = (int)MathHelper.Clamp(value, 0.0312f, 0.0833f);
                //Parameters["EdgeThesholdMin"].SetValue(_subPixelAliasingRemoval);
            }
        }
        private static float _edgeThesholdMin = 0f;

        // This does not effect PS3, as this needs to be compiled in.
        //   Use FXAA_CONSOLE__PS3_EDGE_SHARPNESS for PS3.
        //   Due to the PS3 being ALU bound,
        //   there are only three safe values here: 2 and 4 and 8.
        //   These options use the shaders ability to a free *|/ by 2|4|8.
        // For all other platforms can be a non-power of two.
        //   8.0 is sharper (default!!!)
        //   4.0 is softer
        //   2.0 is really soft (good only for vector graphics inputs)
        private static float consoleEdgeSharpness = 8.0f;

        // This does not effect PS3, as this needs to be compiled in.
        //   Use FXAA_CONSOLE__PS3_EDGE_THRESHOLD for PS3.
        //   Due to the PS3 being ALU bound,
        //   there are only two safe values here: 1/4 and 1/8.
        //   These options use the shaders ability to a free *|/ by 2|4|8.
        // The console setting has a different mapping than the quality setting.
        // Other platforms can use other values.
        //   0.125 leaves less aliasing, but is softer (default!!!)
        //   0.25 leaves more aliasing, and is sharper
        private static float consoleEdgeThreshold = 0.125f;

        // Trims the algorithm from processing darks.
        // The console setting has a different mapping than the quality setting.
        // This only applies when FXAA_EARLY_EXIT is 1.
        // This does not apply to PS3, 
        // PS3 was simplified to avoid more shader instructions.
        //   0.06 - faster but more aliasing in darks
        //   0.05 - default
        //   0.04 - slower and less aliasing in darks
        // Special notes when using FXAA_GREEN_AS_LUMA,
        //   Likely want to set this to zero.
        //   As colors that are mostly not-green
        //   will appear very dark in the green channel!
        //   Tune by looking at mostly non-green content,
        //   then start at zero and increase until aliasing is a problem.
        private static float consoleEdgeThresholdMin = 0f;



        [vxShowInInspector(vxInspectorShaderCategory.AntiAliasing)]
        [vxGraphicalSettingsAttribute("FXAA.DepthSensitivity", "Anti Alias Type")]
        public static float DepthSensitivity
        {
            get { return _depthSensitivity; }
            set { //Parameters["DepthSensitivity"].SetValue(value);
                _depthSensitivity = value; }
        }
        static float _depthSensitivity = 0;



        public vxAntiAliasPostProcess() : base("FXAA", vxInternalAssets.PostProcessShaders.FXAA)
        {
            
        }


        public void Prepare(vxCamera camera)
        {

        }

        public void Apply(vxCamera camera)
        {
            //            if (AntiAliasType == vxAntiAliasType.FXAA
            //#if DEBUG
            //                && vxInput.KeyboardState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.F)
            //                && vxInput.KeyboardState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.LeftShift)
            //#endif
            //                )
            if (true)
            {
                vxGraphics.GraphicsDevice.SetRenderTarget(Renderer.GetNewTempTarget("Anti Aliasing Pass"));

                Viewport viewport = vxGraphics.GraphicsDevice.Viewport;
                Matrix projection = Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, 0, 1);
                Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);
                Effect.Parameters["World"].SetValue(Matrix.Identity);
                Effect.Parameters["View"].SetValue(Matrix.Identity);
                Effect.Parameters["Projection"].SetValue(projection);
                Effect.Parameters["InverseViewportSize"].SetValue(new Vector2(1f / viewport.Width, 1f / viewport.Height));

                if (Effect.Parameters["ConsoleSharpness"] != null)
                    Effect.Parameters["ConsoleSharpness"].SetValue(new Vector4(
                    -N / viewport.Width,
                    -N / viewport.Height,
                    N / viewport.Width,
                    N / viewport.Height
                    ));

                if (Effect.Parameters["ConsoleOpt1"] != null)
                    Effect.Parameters["ConsoleOpt1"].SetValue(new Vector4(
                    -2.0f / viewport.Width,
                    -2.0f / viewport.Height,
                    2.0f / viewport.Width,
                    2.0f / viewport.Height
                    ));
                if (Effect.Parameters["ConsoleOpt2"] != null)
                    Effect.Parameters["ConsoleOpt2"].SetValue(new Vector4(
                    8.0f / viewport.Width,
                    8.0f / viewport.Height,
                    -4.0f / viewport.Width,
                    -4.0f / viewport.Height
                    ));
                Effect.Parameters["SubPixelAliasingRemoval"].SetValue(_subPixelAliasingRemoval);
                Effect.Parameters["EdgeThreshold"].SetValue(_edgeTheshold);
                Effect.Parameters["EdgeThresholdMin"].SetValue(_edgeThesholdMin);



                if (Effect.Parameters["ConsoleEdgeSharpness"] != null)
                    Effect.Parameters["ConsoleEdgeSharpness"].SetValue(consoleEdgeSharpness);

                if (Effect.Parameters["ConsoleEdgeThreshold"] != null)
                    Effect.Parameters["ConsoleEdgeThreshold"].SetValue(consoleEdgeThreshold);

                if (Effect.Parameters["ConsoleEdgeThresholdMin"] != null)
                    Effect.Parameters["ConsoleEdgeThresholdMin"].SetValue(consoleEdgeThresholdMin);

                Effect.CurrentTechnique = Effect.Techniques["FXAA"];

                // Draw a fullscreen sprite to apply the postprocessing effect.
                vxGraphics.SpriteBatch.Begin("PostProcess.AntiAliasing.Apply()", 0, BlendState.Opaque, null, null, null, Effect);
                vxGraphics.SpriteBatch.Draw(Renderer.GetCurrentTempTarget(), Vector2.Zero, Color.White);
                vxGraphics.SpriteBatch.End();
            }
        }
    }
}