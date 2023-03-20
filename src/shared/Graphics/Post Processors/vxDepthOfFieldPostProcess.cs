//#if VIRTICES_3D
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Virtex.Lib.Vrtc.Core;
//using Virtex.Lib.Vrtc.Core.Settings;

//namespace VerticesEngine.Graphics
//{
//    public class vxDepthOfFieldPostProcess : vxPostProcessor
//    {
//        /// <summary>
//        /// Normal Map.
//        /// </summary>
//        public RenderTarget2D SceneMap
//        {
//            set { Parameters["SceneTexture"].SetValue(value); }
//        }


//        public RenderTarget2D DepthMap
//        {
//            set { Parameters["DepthTexture"].SetValue(value); }
//        }

//        public RenderTarget2D BlurredScene
//        {
//            set { Parameters["BlurTexture"].SetValue(value); }
//        }


//        public float FarClip
//        {
//            set { Parameters["FarClip"].SetValue(value); }
//        }


//        public float FocalDistance
//        {
//            set { Parameters["FocalDistance"].SetValue(value); }
//        }


//        public float FocalWidth
//        {
//            set { Parameters["FocalWidth"].SetValue(value); }
//        }
        

//        public vxDepthOfFieldPostProcess(vxEngine Engine) :base(Engine, Engine.InternalAssets.PostProcessShaders.DepthOfFieldEffect)
//        {

//        }

//        public override void LoadContent()
//        {
//            base.LoadContent();


//#if VIRTICES_3D
//            FarClip = 1000;
//            FocalDistance = 40;
//            FocalWidth = 75;
//#endif
//        }

//        public override void Apply()
//        {
//			//Set Render Target
//			Engine.GraphicsDevice.SetRenderTarget(Renderer.RT_DepthOfField);

//            if (Engine.Settings.Graphics.DepthOfField.Quality != vxEnumQuality.None)
//            {

//                // Pass in the Scene Map
//                //SceneMap = Renderer.RT_EdgeDetected;

//                // Pass in the Depth Map
//                DepthMap = Renderer.RT_DepthMap;

//                BlurredScene = Renderer.RT_BlurredScene;

//                // Draw a fullscreen sprite to apply the postprocessing effect.
//                //Engine.SpriteBatch.Begin(0, BlendState.Opaque, null, null, null, Effect);
//                //Engine.SpriteBatch.Draw(Renderer.RT_EdgeDetected, Vector2.Zero, Color.White);
//                //Engine.SpriteBatch.End();
//            }
//            else
//            {

//				//Renderer.RT_DepthMap = Renderer.RT_EdgeDetected;
//                //If the user elects to not use the effect, simply draw the previous scene into the current 
//                //active render target.
//                //Engine.SpriteBatch.Begin();
//                //Engine.SpriteBatch.Draw(Renderer.RT_EdgeDetected, Vector2.Zero, Color.White);
//                //Engine.SpriteBatch.End();
//            }
//        }
//    }
//}
//#endif