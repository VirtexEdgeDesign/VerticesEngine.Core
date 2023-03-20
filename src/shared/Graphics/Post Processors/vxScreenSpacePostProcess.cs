
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
//
//using VerticesEngine;

//namespace VerticesEngine.Graphics
//{
//    /// <summary>
//    /// This post processor handles all Screen Space Post Processes such as Screen Space Reflections
//    /// and Ambient Occulusions.
//    /// </summary>
//    public class vxScreenSpacePostProcess : vxPostProcessor3D
//    {

//        [vxShowInInspector(vxInspectorShaderCategory.ScreenSpaceAmbientOcclusion)]
//        public float BlurFactor
//        {
//            get { return Parameters["BlurFactor"].GetValueSingle(); }
//            set { Parameters["BlurFactor"].SetValue(value); }
//        }
//        /// <summary>
//        /// Normal Map.
//        /// </summary>
//        public RenderTarget2D NormalMap
//        {
//            set { Parameters["NormalTexture"].SetValue(value); }
//        }

//        public Vector3[] RAND_SAMPLES
//        {
//            set
//            {
//                if (value != null)
//                    Parameters["RAND_SAMPLES"].SetValue(value);
//            }
//        }

//        /// <summary>
//        /// Depth map of the scene.
//        /// </summary>
//        //[vxPostProcessingPropertyAttribute(Title = "Samples")]
//        public int Samples
//        {
//            set { Parameters["Samples"].SetValue(value); }
//        }

//        [vxShowInInspector(vxInspectorShaderCategory.ScreenSpaceReflections)]
//        public vxEnumQuality SSRQuality
//        {
//            get { return Settings.Graphics.Reflections.SSRSettings.Quality; }
//            set
//            {
//                Settings.Graphics.Reflections.SSRSettings.Quality = value;
//                Parameters["SSRQuality"].SetValue((float)Settings.Graphics.Reflections.SSRSettings.Quality);
//                //OnGraphicsRefresh();
//            }
//        }


//        public vxScreenSpacePostProcess(vxRenderPipeline3D Renderer) :
//        base(Renderer, "Screen Space Combine", vxInternalAssets.PostProcessShaders.ScreenSpaceCombine)
//        {

//        }

//        public override void LoadContent(vxRenderPipelineConfig config)
//        {
//            base.LoadContent(config);


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

//            BlurFactor = 100;
//            SSRQuality = Settings.Graphics.Reflections.SSRSettings.Quality;
//        }

//        Texture2D NullTex;
//        public override void Apply()
//        {
//            vxRenderPipeline3D renderer = (vxRenderPipeline3D)this.Renderer;
//            renderer.SSRPostProcess.GenerateUVMap();
			
//            Renderer.GraphicsDevice.SetRenderTarget(Renderer.GetNewTempTarget("Screen Space Combine"));

//            //Set Render State
//            Renderer.GraphicsDevice.BlendState = BlendState.AlphaBlend;
//            Renderer.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
//            Renderer.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

//            Effect.CurrentTechnique = Effect.Techniques["Technique_ApplyPostLighting"];

//            if (Settings.Graphics.Reflections.SSRSettings.Quality != vxEnumQuality.None)
//            {
//                Parameters["SSRUVs"].SetValue(renderer.SSRPostProcess.SSRUVCoordMap);
//                Parameters["BlurredSceneTexture"].SetValue(RenderTargets.BlurredSceneResult);
//            }
//            else
//                Parameters["SSRUVs"].SetValue(NullTex);

//            if(Settings.Graphics.SSAO.Quality != vxEnumQuality.None)
//                Parameters["SSAOMap"].SetValue(renderer.SSAOPostProcess.SSAOStippleMap);
//            else
//                Parameters["SSAOMap"].SetValue(DefaultTexture);

//            //Combine everything
//            Parameters["SceneTexture"].SetValue(renderer.DefferredRenderPostProcess.DeferredLitScene);
//            Renderer.GetCurrentTempTarget();

//            foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
//            {
//                pass.Apply();
//                Renderer.RenderQuad(Vector2.One * -1, Vector2.One);
//            }
//        }

//    }
//}