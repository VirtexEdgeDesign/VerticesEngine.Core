
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace VerticesEngine.Graphics
//{
//    /// <summary>
//    /// Grab a scene that has already been rendered, 
//    /// and add a distortion effect over the top of it.
//    /// </summary>
//    public class vxDistortionPostProcess2D : vxPostProcessor2D
//    {
//        /// <summary>
//        /// The Scene Texture.
//        /// </summary>
//        public RenderTarget2D SceneTexture
//        {
//            set { Parameters["SceneTexture"].SetValue(value); }
//        }

//        /// <summary>
//        /// The Distortion Map
//        /// </summary>
//        public RenderTarget2D DistortionMap
//        {
//            set { Parameters["DistortionMap"].SetValue(value); }
//        }

//        /// <summary>
//        /// Depth map of the scene.
//        /// </summary>
//        public RenderTarget2D DepthMap
//        {
//            set { Parameters["DepthMap"].SetValue(value); }
//        }

//        /// <summary>
//        /// Should the Distortion be blurred?
//        /// </summary>
//        public bool DoDistortionBlur
//        {
//            set { Parameters["distortionBlur"].SetValue(value); }
//        }
        

//        public vxDistortionPostProcess2D(vxEngine Engine) :base(Engine, Engine.InternalAssets.PostProcessShaders.DistortSceneEffect)
//        {
//			//Engine.Assets.PostProcessShaders.DistortSceneEffect.Parameters["ZeroOffset"].SetValue(0.5f / 255.0f);
//        }

//        public override void LoadContent()
//        {
//            base.LoadContent();

//            DoDistortionBlur = false;
//        }

//        public override void SetResoultion()
//        {
//            base.SetResoultion();
//            Engine.InternalAssets.PostProcessShaders.DistortSceneEffect.Parameters["MatrixTransform"].SetValue(MatrixTransform);
//        }


//        public void Apply(RenderTarget2D PreScene, RenderTarget2D DistortionMap, RenderTarget2D FinalScene)
//        {
//            // if we want to show the distortion map, then the backbuffer is done.
//            // if we want to render the scene distorted, then we need to resolve the
//            // backbuffer as the distortion map and use it to distort the scene
//            Engine.GraphicsDevice.SetRenderTarget(FinalScene);


//            // draw the scene image again, distorting it with the distortion map

//#if VRTC_PLTFRM_XNA
//            Engine.GraphicsDevice.Textures[1] = Engine.Renderer.RT_DistortionMap;
//#else
//            this.DistortionMap = DistortionMap;
//#endif
//            Engine.GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;


//            Viewport viewport = Engine.GraphicsDevice.Viewport;

//            Engine.InternalAssets.PostProcessShaders.DistortSceneEffect.CurrentTechnique = false ? Engine.InternalAssets.PostProcessShaders.distortBlurTechnique : Engine.InternalAssets.PostProcessShaders.distortTechnique;

//            DrawFullscreenQuad(PreScene,
//                viewport.Width, viewport.Height, Effect);
//        }
//    }
//}
