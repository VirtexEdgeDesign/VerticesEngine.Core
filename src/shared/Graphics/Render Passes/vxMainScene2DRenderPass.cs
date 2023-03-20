using Microsoft.Xna.Framework.Graphics;
using System;
using VerticesEngine.Diagnostics;
using VerticesEngine;

namespace VerticesEngine.Graphics
{
    /// <summary>
    /// This is the main scene render pass for a 3D scene. This first draws all meshes' with opaque and then performs a alpha pass
    /// </summary>
    public class vxMainScene2DRenderPass : vxRenderPass, vxIRenderPass
    {
        public override string RenderPass
        {
            get { return vxRenderPipeline.Passes.OpaquePass; }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.Graphics.vxMainScene2DRenderPass"/> class.
        /// </summary>
        public vxMainScene2DRenderPass() : base("Main 2D Scene Render Pass", vxInternalAssets.Shaders.MainShader)
        {

        }


        public void Prepare(vxCamera camera)
        {
            // TODO: reorder items based on distance
        }


        public void Apply(vxCamera camera)
        {
            // iOS doesn't like AnisotropicWrap
#if __IOS__
            vxGraphics.SpriteBatch.Begin("Main Scene 2D Draw", 0, null, null, null, null, null, camera.View);
#else
            vxGraphics.SpriteBatch.Begin("Main Scene 2D Draw", 0, null, SamplerState.AnisotropicWrap, null, null, null, camera.View);
#endif

            // Draw the Particle System
            camera.CurrentScene.ParticleSystem.DrawParticles(camera, "Before");

            // TODO: Fix this
            for (int e = 0; e < camera.CurrentScene.Entities.Count; e++)
            {
                var entity = camera.CurrentScene.Entities[e];
                if (entity.IsEnabled)
                {
                    camera.CurrentScene.Entities[e].EntityRenderer.Draw(camera, vxSpriteRenderer.Passes.PreDraw);
                }
            }
            // draw all of th entities
            for (int e = 0; e < camera.CurrentScene.Entities.Count; e++)
            {
                var entity = camera.CurrentScene.Entities[e];
                if (entity.IsEnabled)
                {
                    camera.CurrentScene.Entities[e].EntityRenderer.Draw(camera, vxSpriteRenderer.Passes.MainDraw);
                }
            }
            for (int e = 0; e < camera.CurrentScene.Entities.Count; e++)
            {
                var entity = camera.CurrentScene.Entities[e];
                if (entity.IsEnabled)
                {
                    camera.CurrentScene.Entities[e].EntityRenderer.Draw(camera, vxSpriteRenderer.Passes.PostDraw);
                }
            }

            // Draws the Particles that are infront
            camera.CurrentScene.ParticleSystem.DrawParticles(camera, "After");

            vxGraphics.SpriteBatch.End();
            camera.CurrentScene.PostDraw();
        }
    }
}