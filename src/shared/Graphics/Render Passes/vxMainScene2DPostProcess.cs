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
//using VerticesEngine.Graphics;


//namespace VerticesEngine.Graphics
//{
//    /// <summary>
//    /// This is the diagram renderer of items.
//    /// </summary>
//    public class vxMainScene2DPostProcess : vxPostProcessor2D
//    {
//        public Color Colour
//        {
//            set { Parameters["dia_colour"].SetValue(value.ToVector3()); }
//        }

//        vxGameplayScene2D CurrentScene;

//        public override int RenderPass
//        {
//            get { return 0; }
//        }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="T:VerticesEngine.Graphics.vxSSAOPostProcess"/> class.
//        /// </summary>
//        /// <param name="Engine">Engine.</param>
//        public vxMainScene2DPostProcess(vxRenderPipeline2D Renderer) :
//        base(Renderer, "Main Scene Post Process", vxInternalAssets.Shaders.MainShader)
//        {
//            CurrentScene = (vxGameplayScene2D)vxEngine.Instance.CurrentScene;
//        }



//        public override void Apply()
//        {
//            base.Apply();

//            SpriteBatch.Begin("Main Scene 2D Draw",0, null, null, null, null, null, CurrentScene.Camera.View);
            


//            // Draw the Particle System
//            CurrentScene.ParticleSystem.DrawUnderlayItems();

//            foreach (var entity in CurrentScene.Entities)
//                entity.PreDraw(RenderPass);

//            // draw all of th entities
//            foreach (var entity in CurrentScene.Entities)
//                entity.Draw(RenderPass);

//            foreach (var entity in CurrentScene.Entities)
//              entity.PostDraw(RenderPass);

//            // Draws the Particles that are infront
//            CurrentScene.ParticleSystem.DrawOverlayItems();


//            // draw animations on top
//            //foreach (vxEntity2D entity in Entities)
//            //  entity.DrawAnimation();

//            SpriteBatch.End();
//        }
//    }
//}