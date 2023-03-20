
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using VerticesEngine;
//using VerticesEngine.Settings;
//using VerticesEngine;
//using VerticesEngine.Serilization;

//namespace VerticesEngine.Graphics
//{
//    public class vxSSRGraphicsConfig : vxGraphicsConfig
//    {
//        public float DepthCheckBias = 0.00001f;
//        public float Loops = 12;
//        public float EdgeCutOff = 0.05f;

//        /// <summary>
//        /// The number of samples for ray marching. The value must be between 4 and 50.
//        /// </summary>
//        public int RayMarchLoops
//        {
//            get { return _rayMarchLoops; }
//            set { _rayMarchLoops = (int)MathHelper.Clamp(value, 4, 50); }
//        }
//        int _rayMarchLoops = 10;


//        /// <summary>
//        /// The number of samples for ray marching. The value must be between 1 and 5.
//        /// </summary>
//        public int RayMarchSamples
//        {
//            get { return _rayMarchSamples; }
//            set { _rayMarchSamples = (int)MathHelper.Clamp(value, 1, 5); }
//        }
//        int _rayMarchSamples = 5;
//    }
//    /// <summary>
//    /// Screen Space Local Reflections Post Processing Shader.
//    /// </summary>
//    public class vxSSRPostProcess : vxPostProcessor3D
//    {
//        /// <summary>
//        /// The Reflection Map
//        /// </summary>
//        public RenderTarget2D ColorMap
//        {
//            set { Parameters["ColorMap"].SetValue(value); }
//        }


//        /// <summary>
//        /// Render Target which holds all of the Reflection Maps UV Coordinates.
//        /// </summary>
//        public RenderTarget2D SSRUVCoordMap;



//        /// <summary>
//        /// Normal Map.
//        /// </summary>
//        public RenderTarget2D NormalMap
//        {
//            set { Parameters["NormalMap"].SetValue(value); }
//        }

//        /// <summary>
//        /// Depth map of the scene.
//        /// </summary>
//        public RenderTarget2D DepthMap
//        {
//            set { Parameters["DepthMap"].SetValue(value); }
//        }

//        /// <summary>
//        /// Edge Width for edge detection.
//        /// </summary>
//        [vxShowInInspector(vxInspectorShaderCategory.ScreenSpaceReflections)]
//        public Vector3 CameraPosition
//        {
//            get { return Parameters["CameraPosition"].GetValueVector3(); }
//            set { Parameters["CameraPosition"].SetValue(value); }
//        }


//        public Matrix ViewProjection
//        {
//            set { Parameters["ViewProjection"].SetValue(value); }
//        }

//        /// <summary>
//        /// Intensity of the edge dection.
//        /// </summary>
//        public Matrix InverseViewProjection
//        {
//            set { Parameters["InverseViewProjection"].SetValue(value); }
//        }


//        /*
//        [vxPostProcessingPropertyAttribute(Title = "LScale")]
//        public float LScale
//        {
//            get { return Parameters["LScale"].GetValueSingle(); }
//            set { Parameters["LScale"].SetValue(value); }
//        }

//        [vxPostProcessingPropertyAttribute(Title = "depthCutoff")]
//		public float depthCutoff
//        {
//            get { return Parameters["depthCutoff"].GetValueSingle(); }
//			set { Parameters["depthCutoff"].SetValue(value); }
//		}
//        /* 9965
//        [vxPostProcessingPropertyAttribute(Title = "Loops")]
//		public float Loops
//		{
//            get { return Parameters["Loops"].GetValueSingle(); }
//			set { Parameters["Loops"].SetValue(value); }
//		}
        
//         */

//        [vxShowInInspector(vxInspectorShaderCategory.ScreenSpaceReflections)]
//        public float DepthCheckBias
//        {
//            get { return Parameters["DepthCheckBias"].GetValueSingle(); }
//            set { Parameters["DepthCheckBias"].SetValue(value); }
//        }

//        [vxShowInInspector(vxInspectorShaderCategory.ScreenSpaceReflections)]
//        public float EdgeCutOff
//        {
//            get { return Parameters["EdgeCutOff"].GetValueSingle(); }
//            set { Parameters["EdgeCutOff"].SetValue(value); }
//        }



//        public Vector3[] RAND_SAMPLES
//        {
//            set
//            {
//                if (value != null)
//                    if (Parameters["RAND_SAMPLES"] != null)
//                        Parameters["RAND_SAMPLES"].SetValue(value);
//            }
//        }

//        public vxSSRPostProcess(vxRenderPipeline3D Renderer) :
//        base(Renderer, "Screen Space Reflections", vxInternalAssets.PostProcessShaders.ScreenSpaceReflectionEffect)
//        {

//        }

//        public override void LoadContent(vxRenderPipelineConfig config)
//        {
//            base.LoadContent(config);

//            //Loops = config.SSR.Loops;
//            //depthCutoff = 1;
//            //LScale = 1;
//            EdgeCutOff = config.SSR.EdgeCutOff;
//             DepthCheckBias = config.SSR.DepthCheckBias;

//            RAND_SAMPLES = new Vector3[]
//            {
//                      new Vector3( 0.5381f, 0.1856f,-0.4319f),
//      new Vector3( 0.1379f, 0.2486f, 0.4430f),
//      new Vector3( 0.3371f, 0.5679f,-0.0057f),
//      new Vector3(-0.6999f,-0.0451f,-0.0019f),
//      new Vector3( 0.0689f,-0.1598f,-0.8547f),
//      new Vector3( 0.0560f, 0.0069f,-0.1843f),
//      /*
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
//      */
//        };
//        }

//        public override void OnGraphicsRefresh()
//        {
//            base.OnGraphicsRefresh();

//            // Create two custom rendertargets.
//            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            

//            //Blur Render Targets
//            int width = pp.BackBufferWidth;
//            int height = pp.BackBufferHeight;
            
//            // Create two rendertargets for the bloom processing.
//            width /= (5 - (int)Settings.Graphics.Reflections.SSRSettings.Quality);
//            height /= (5 - (int)Settings.Graphics.Reflections.SSRSettings.Quality);

//            //int scale = 4;
//            SSRUVCoordMap = new RenderTarget2D(GraphicsDevice, width, height, false,
//                pp.BackBufferFormat, pp.DepthStencilFormat);
//        }
        

//        public virtual void GenerateUVMap()
//        {
//            if (Settings.Graphics.Reflections.ReflectionType != vxEnumReflectionType.None)
//            {
//                vxGraphics.GraphicsDevice.SetRenderTarget(SSRUVCoordMap);

//                // Pass in the Normal Map
//                NormalMap = RenderTargets.NormalMap;

//                // Pass in the Depth Map
//                DepthMap = RenderTargets.DepthMap;
//                vxGraphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;

                
//                CameraPosition = Scene.Camera.Position;

//                ViewProjection = Scene.Camera.View * Scene.Camera.Projection;
//                //Projection = OrthogonalProjection;
//                InverseViewProjection = Matrix.Invert(Scene.Camera.View * Scene.Camera.Projection);


//                // Activate the appropriate effect technique.
//                Effect.CurrentTechnique = Effect.Techniques["Technique1"];

//                //Combine everything
//                //Parameters["SceneTexture"].SetValue(RenderTargets.BlurredSceneResult);
//                //Parameters["EdgeCutOff"].SetValue(0.05f);
//                //Parameters["depthCutoff"].SetValue(0.95f);

//                // Draw a fullscreen sprite to apply the postprocessing effect.
//                vxGraphics.SpriteBatch.Begin("PostProcess.SSR.GenerateUVMap()", 0, BlendState.Opaque, null, null, null, Effect);
//                vxGraphics.SpriteBatch.Draw(Renderer.PeekAtCurrentTempTarget(), Scene.ViewportManager.MainViewport.Bounds,
//                    Color.White);
//                vxGraphics.SpriteBatch.End();
//            }
//        }
//    }
//}
