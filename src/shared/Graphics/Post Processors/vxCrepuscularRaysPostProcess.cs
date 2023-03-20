//#if VIRTICES_3D
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Virtex.Lib.Vrtc.Core;
//using Virtex.Lib.Vrtc.Core.Settings;
//using Virtex.Lib.Vrtc.Utilities;
//using Virtex.Lib.Vrtc.Core.Cameras;

//namespace VerticesEngine.Graphics
//{
//    public class vxCrepuscularRaysPostProcess : vxPostProcessor
//    {
//        /// <summary>
//        /// Normal Map.
//        /// </summary>
//        public RenderTarget2D NormalMap
//        {
//            set { Parameters["NormalTexture"].SetValue(value); }
//        }

//        /// <summary>
//        /// Depth map of the scene.
//        /// </summary>
//        public RenderTarget2D DepthMap
//        {
//            set { Parameters["DepthTexture"].SetValue(value); }
//        }

//        /// <summary>
//        /// Density
//        /// </summary>
//        public float Density
//        {
//            set { Parameters["Density"].SetValue(value); }
//        }

//        /// <summary>
//        /// Decay of rays.
//        /// </summary>
//        public float Decay
//        {
//            set { Parameters["Decay"].SetValue(value); }
//        }


//        public float Weight
//        {
//            set { Parameters["Weight"].SetValue(value); }
//        }


//        public float Exposure
//        {
//            set { Parameters["Exposure"].SetValue(value); }
//        }

//        public vxCrepuscularRaysPostProcess(vxEngine Engine) :base(Engine, Engine.InternalAssets.PostProcessShaders.LightRaysEffect)
//        {

//        }

//        public override void LoadContent()
//        {
//            base.LoadContent();


//            Density = .55f;
//            Decay = .9f;
//            Weight = 1.0f;
//            Exposure = .15f;
//        }

//        public override void SetResoultion()
//        {
//            base.SetResoultion();
//            Engine.InternalAssets.PostProcessShaders.GodRaysCombineEffect.Parameters["MatrixTransform"].SetValue(MatrixTransform);
//            Engine.InternalAssets.PostProcessShaders.MaskedSunEffect.Parameters["MatrixTransform"].SetValue(MatrixTransform);
//        }


//        public void ApplyPrePass()
//        {
//            Engine.GraphicsDevice.SetRenderTarget(Engine.Renderer.RT_SunMap);
//            Engine.GraphicsDevice.BlendState = BlendState.Opaque;
//            Engine.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
//            Engine.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

//			foreach (vxCamera3D Camera in Engine.Current3DSceneBase.Cameras)
//			{
//				Camera.SetAsActiveCamera();
//				Engine.Current3DSceneBase.SunEmitter.DrawGlow(Camera);
//			}

//            Engine.GraphicsDevice.SetRenderTarget(Engine.Renderer.RT_MaskMap);
//            //Engine.GraphicsDevice.BlendState = BlendState.Opaque;
//            //Engine.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
//            Engine.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

//            //Matrix orthoproj = Matrix.CreateOrthographicOffCenter(0,
//            //                           Engine.GraphicsDevice.Viewport.Width,
//            //                           Engine.GraphicsDevice.Viewport.Height,
//            //                           0, 0, 1);
//            //Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);

//            Engine.InternalAssets.PostProcessShaders.MaskedSunEffect.Parameters["DepthMap"].SetValue(Engine.Renderer.RT_DepthMap);

//            try
//            {
//                Engine.SpriteBatch.Begin(0, BlendState.Opaque, null, null, null,
//                    Engine.InternalAssets.PostProcessShaders.MaskedSunEffect);
//            }
//            catch
//            {
//                Engine.SpriteBatch.End();
//                Engine.SpriteBatch.Begin(0, BlendState.Opaque, null, null, null,
//                    Engine.InternalAssets.PostProcessShaders.MaskedSunEffect);

//                vxConsole.WriteLine("ERROR DRAWING MASK MAP");
//            }
//            Engine.SpriteBatch.Draw(Engine.Renderer.RT_SunMap, Vector2.Zero, Color.White);
//            Engine.SpriteBatch.End();
//        }

//        public override void Apply()
//        {

//            Vector2 lighScreenSourcePos = new Vector2(
//                Engine.Current3DSceneBase.SunEmitter.ScreenSpacePosition.X / Engine.GraphicsDevice.Viewport.Width,
//                Engine.Current3DSceneBase.SunEmitter.ScreenSpacePosition.Y / Engine.GraphicsDevice.Viewport.Height);

            

//            if (Engine.Profile.Settings.Graphics.GodRays != vxEnumQuality.None && lighScreenSourcePos.X > 0 && lighScreenSourcePos.Y > 0 &&
//                lighScreenSourcePos.X < Engine.GraphicsDevice.Viewport.Width && lighScreenSourcePos.Y < Engine.GraphicsDevice.Viewport.Height
//            && Engine.Current3DSceneBase.SunEmitter.IsOnScreen)
//            {

//                Parameters["Texture"].SetValue(Renderer.RT_MaskMap);
//                Parameters["lightScreenPosition"].SetValue(lighScreenSourcePos);

//                Engine.GraphicsDevice.SetRenderTarget(Renderer.RT_GodRaysScene);
                
//                // Now Blend the Sun Image
//                DrawFullscreenQuad(Renderer.RT_MaskMap,
//                                   Engine.GraphicsDevice.Viewport.Width, Engine.GraphicsDevice.Viewport.Height,
//                                   Effect);

//                Engine.GraphicsDevice.SetRenderTarget(null);

//                Engine.InternalAssets.PostProcessShaders.GodRaysCombineEffect.Parameters["TextureSampler"].SetValue(Renderer.RT_FinalScene);

//                DrawFullscreenQuad(Renderer.RT_GodRaysScene,
//                                   Engine.GraphicsDevice.Viewport.Width, Engine.GraphicsDevice.Viewport.Height,
//                                    Engine.InternalAssets.PostProcessShaders.GodRaysCombineEffect);

//            }

//            else
//            {
//                Engine.GraphicsDevice.SetRenderTarget(null);

//                DrawFullscreenQuad(Renderer.RT_FinalScene,
//                                   Engine.GraphicsDevice.Viewport.Width, Engine.GraphicsDevice.Viewport.Height);
//            }
//        }
//    }
//}
//#endif