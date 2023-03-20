//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using VerticesEngine;
//using VerticesEngine.Settings;
//using VerticesEngine.Utilities;
//using VerticesEngine;

//namespace VerticesEngine.Graphics
//{
//    //public class vxSSAOGraphicsConfig : vxGraphicsConfig
//    //{
//    //    public Vector2 Radius = new Vector2(0.2f, 0.4f);
//    //    public float Bias = 0.000001f;
//    //    //Radius.SetValue(new Vector2(0.2f, 0.4f));
//    //    //Bias.SetValue(0.000001f);
//    //    public float Intensity = 1.150f;
//    //    public float Tiles = 2500.0f;

//    //    public float RangeCutOff = (0.01f);

//    //    public override void VerboseOutput(int depth)
//    //    {
//    //        base.VerboseOutput(depth);
//    //        //vxConsole.WriteLine(nameof(BloomBlurAmount), BloomBlurAmount, ConfigConsoleColour);
//    //    }
//    //}

//    /// <summary>
//    /// Screen Space Ambient Occlusion Post Processing Shader. 
//    /// </summary>
//    public class vxSSAOPostProcess : vxPostProcessor, vxPostProcessorInterface
//    {
//        /// <summary>
//        /// SSAO Render Target
//        /// </summary>
//        public RenderTarget2D SSAOStippleMap;

//        public vxEffectParameter NormalBuffer;
//        public vxEffectParameter DepthBuffer;
//        public vxEffectParameter RandomBuffer;
//        //public vxEffectParameter Intensity;
//        public vxEffectParameter RangeCutOff;
//        //public vxEffectParameter Bias;
//        //public vxEffectParameter Radius;


//        /// <summary>
//        /// Intensity of the edge dection.
//        /// </summary>
//        public Matrix InverseViewProjection
//        {
//            set
//            {
//                if (Parameters["InverseViewProjection"] != null)
//                    Parameters["InverseViewProjection"].SetValue(value);
//            }
//        }

//        public Matrix ViewProjection
//        {
//            set
//            {
//                if (Parameters["ViewProjection"] != null)
//                    Parameters["ViewProjection"].SetValue(value);
//            }
//        }


//        public Vector3[] RAND_SAMPLES
//        {
//            set
//            {
//                if (value != null)
//                    Parameters["RAND_SAMPLES"].SetValue(value);
//            }
//        }

//        [vxShowInInspector(vxInspectorShaderCategory.ScreenSpaceAmbientOcclusion)]
//        public float Bias
//        {
//            get { return Parameters["Bias"].GetValueSingle(); }
//            set { Parameters["Bias"].SetValue(value); }
//        }

//        [vxShowInInspector(vxInspectorShaderCategory.ScreenSpaceAmbientOcclusion)]
//        public Vector2 Radius
//        {
//            get { return Parameters["Radius"].GetValueVector2(); }
//            set { Parameters["Radius"].SetValue(value); }
//        }

//        [vxShowInInspector(vxInspectorShaderCategory.ScreenSpaceAmbientOcclusion)]
//        public float Tiles
//        {
//            get { return Parameters["Tiles"].GetValueSingle(); }
//            set { Parameters["Tiles"].SetValue(value); }
//        }

//        [vxShowInInspector(vxInspectorShaderCategory.ScreenSpaceAmbientOcclusion)]
//        public float Intensity
//        {
//            get { return Parameters["Intensity"].GetValueSingle(); }
//            set { Parameters["Intensity"].SetValue(value); }
//        }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="T:VerticesEngine.Graphics.vxSSAOPostProcess"/> class.
//        /// </summary>
//        /// <param name="Engine">Engine.</param>
//        public vxSSAOPostProcess(vxRenderPipeline Renderer) :
//        base(Renderer, "Screen Space Ambient Occulsion", vxInternalAssets.PostProcessShaders.SSAOEffect)
//        {
//            NormalBuffer = new vxEffectParameter(Effect.Parameters["NormalMap"]);
//            DepthBuffer = new vxEffectParameter(Effect.Parameters["DepthMap"]);
//            RandomBuffer = new vxEffectParameter(Effect.Parameters["RandomMap"]);

//            //Intensity = new vxEffectParameter(Effect.Parameters["Intensity"], 0, 1.5f);
//            RangeCutOff = new vxEffectParameter(Effect.Parameters["RangeCutOff"], 0, 1.5f);
//            //Bias = new vxEffectParameter(Effect.Parameters["Bias"], 0, 1.5f);
//            //Radius = new vxEffectParameter(Effect.Parameters["Radius"], 0, 1.5f);


//            Radius = new Vector2(0.2f, 0.4f);
//            Bias = 0.000001f;
//            Intensity = 1.150f;
//            Tiles = 2500.0f;
//            RangeCutOff.SetValue(0.01f);

//            //    public Vector2 Radius = new Vector2(0.2f, 0.4f);
//            //    public float Bias = 0.000001f;
//            //    //Radius.SetValue(new Vector2(0.2f, 0.4f));
//            //    //Bias.SetValue(0.000001f);
//            //    public float Intensity = 1.150f;
//            //    public float Tiles = 2500.0f;

//            //    public float RangeCutOff = (0.01f);

//            RandomBuffer.SetValue(vxInternalAssets.Textures.RandomValues);



//            RAND_SAMPLES = new Vector3[]
//            {
//                      new Vector3( 0.5381f, 0.1856f,-0.4319f),
//      new Vector3( 0.1379f, 0.2486f, 0.4430f),
//      new Vector3( 0.3371f, 0.5679f,-0.0057f),
//      new Vector3(-0.6999f,-0.0451f,-0.0019f),
//      new Vector3( 0.0689f,-0.1598f,-0.8547f),
//      new Vector3( 0.0560f, 0.0069f,-0.1843f),
//      new Vector3(-0.0146f, 0.1402f, 0.0762f),
//      new Vector3( 0.0100f,-0.1924f,-0.0344f),
//      new Vector3(-0.3577f,-0.5301f,-0.4358f),
//      new Vector3(-0.3169f, 0.1063f, 0.0158f),
//      new Vector3( 0.0103f,-0.5869f, 0.0046f),
//      new Vector3(-0.0897f,-0.4940f, 0.3287f),
//      new Vector3( 0.7119f,-0.0154f,-0.0918f),
//      new Vector3(-0.0533f, 0.0596f,-0.5411f),
//      new Vector3( 0.0352f,-0.0631f, 0.5460f),
//      new Vector3(-0.4776f, 0.2847f,-0.0271f)
//        };
//        }


//        public override void OnGraphicsRefresh()
//        {
//            base.OnGraphicsRefresh();

//            // Create two custom rendertargets.
//            PresentationParameters pp = GraphicsDevice.PresentationParameters;

//            SSAOStippleMap = new RenderTarget2D(GraphicsDevice,
//                pp.BackBufferWidth, pp.BackBufferHeight, false,
//                pp.BackBufferFormat, pp.DepthStencilFormat);

//        }

//        public void Prepare()
//        {
//            if (Settings.Graphics.SSAO.Quality > vxEnumQuality.None)
//            {
//                vxGraphics.GraphicsDevice.SetRenderTarget(SSAOStippleMap);
//                vxGraphics.GraphicsDevice.Clear(Color.White);

//                Effect.CurrentTechnique = Effect.Techniques[0];

//                ViewProjection = Scene.Camera.View * Scene.Camera.Projection;
//                //Projection = OrthogonalProjection;
//                InverseViewProjection = Matrix.Invert(Scene.Camera.View * Scene.Camera.Projection);
//                DepthBuffer.SetValue(RenderTargets.DepthMap);
//                NormalBuffer.SetValue(RenderTargets.NormalMap);

//                Effect.CurrentTechnique = Effect.Techniques["Technique1"];

//                // Draw a fullscreen sprite to apply the postprocessing effect.
//                vxGraphics.SpriteBatch.Begin("PostProcess.SSAO.Prepare()", 0, BlendState.Opaque, null, null, null, Effect);
//                vxGraphics.SpriteBatch.Draw(DefaultTexture, Scene.ViewportManager.MainViewport.Bounds, Color.White);
//                vxGraphics.SpriteBatch.End();
//            }
//        }

//        public void Apply()
//        {

//        }
//    }
//}