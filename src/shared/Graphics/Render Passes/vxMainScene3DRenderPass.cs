using Microsoft.Xna.Framework.Graphics;
using System;
using VerticesEngine.Diagnostics;
using VerticesEngine;

namespace VerticesEngine.Graphics
{
    /// <summary>
    /// This is the main scene render pass for a 3D scene. This first draws all meshes' with opaque and then performs a alpha pass
    /// </summary>
    public class vxMainScene3DRenderPass : vxRenderPass, vxIRenderPass
    {
        public RenderTarget2D AlbedoPass;

        public override string RenderPass
        {
            get { return vxRenderPipeline.Passes.OpaquePass; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.Graphics.vxMainScene3DRenderPass"/> class.
        /// </summary>
        /// <param name="Engine">Engine.</param>
        public vxMainScene3DRenderPass() : base("Main Scene Post Process", vxInternalAssets.Shaders.MainShader)
        {
            PresentationParameters pp = vxGraphics.GraphicsDevice.PresentationParameters;

            AlbedoPass = new RenderTarget2D(vxGraphics.GraphicsDevice, vxGraphics.FinalViewport.Width, vxGraphics.FinalViewport.Height , false, pp.BackBufferFormat, pp.DepthStencilFormat);
        }

        public override void OnGraphicsRefresh()
        {
            base.OnGraphicsRefresh();
        }

        public void Prepare(vxCamera camera)
        {
            // TODO: reorder items based on distance
        }


        public void Apply(vxCamera camera)
        {
            AlbedoPass = Renderer.GetNewTempTarget("Albedo Pass");

            vxGraphics.GraphicsDevice.SetRenderTarget(AlbedoPass);
            // draw the main scene
            vxGraphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            vxGraphics.GraphicsDevice.BlendState = BlendState.Opaque;
            vxGraphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            vxGraphics.GraphicsDevice.Clear(camera.BackBufferColour);

            vxEngine.Instance.GetCurrentScene<vxGameplayScene3D>().PreDrawSelectedItems();

            if (camera.IsRenderListEnabled)
            {
                for (int i = 0; i < camera.opaqueCount; i++)
                {
                    var renderData = camera.renderList[i];
                    renderData.material.World = renderData.renderPassData.World;
                    renderData.material.WorldInverseTranspose = renderData.renderPassData.WorldInvT;
                    renderData.material.WVP = renderData.renderPassData.WVP;

                    // set camera values
                    renderData.material.View = camera.View;
                    renderData.material.Projection = camera.Projection;
                    renderData.material.CameraPosition = camera.Position;

                    renderData.mesh.Draw(renderData.material);
                }
            }
            else
            {
                // draw all of th entities
                for (int i = 0; i < camera.totalItemsToDraw; i++)
                {
                    // render everything for this pass except particles
                    int drawIndex = camera.drawList[i];
                    if (drawIndex < vxEngine.Instance.CurrentScene.MeshRenderers.Count)
                    {
                        camera.CurrentScene.MeshRenderers[drawIndex].Draw(camera, vxRenderPipeline.Passes.OpaquePass);
                    }
                }
            }

            camera.CurrentScene.ParticleSystem.DrawParticles(camera, vxRenderPipeline.Passes.OpaquePass);

            // draw the main scene
            vxGraphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            vxGraphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            vxGraphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            
            if (camera.IsRenderListEnabled)
            {
                for (int i = camera.opaqueCount; i < (camera.opaqueCount + camera.transparentCount); i++)
                {
                    var renderData = camera.renderList[i];
                    renderData.material.World = renderData.renderPassData.World;
                    renderData.material.WorldInverseTranspose = renderData.renderPassData.WorldInvT;
                    renderData.material.WVP = renderData.renderPassData.WVP;

                    // set camera values
                    renderData.material.View = camera.View;
                    renderData.material.Projection = camera.Projection;
                    renderData.material.CameraPosition = camera.Position;

                    renderData.mesh.Draw(renderData.material);
                }
            }
            else
            {
                for (int i = 0; i < camera.totalItemsToDraw; i++)
                {
                    // render everything for this pass except particles
                    int drawIndex = camera.drawList[i];
                    if (drawIndex < vxEngine.Instance.CurrentScene.MeshRenderers.Count)
                    {
                        camera.CurrentScene.MeshRenderers[drawIndex]
                            .Draw(camera, vxRenderPipeline.Passes.TransparencyPass);
                    }
                }
            }

            // now draw all particles   
            camera.CurrentScene.ParticleSystem.DrawParticles(camera, vxRenderPipeline.Passes.TransparencyPass);

            // now draw selected items
            vxEngine.Instance.GetCurrentScene<vxGameplayScene3D>().DrawSelectedItems();

            if (shouldDumpCurrentDrawList)
            {
                shouldDumpCurrentDrawList = false;

                string txt = "Draw List : " + camera.totalItemsToDraw + " Items";

                for (int i = 0; i < camera.totalItemsToDraw; i++)
                {
                    txt += string.Format("\n{0}: {1}", i, camera.CurrentScene.Entities[camera.drawList[i]].Id);
                    Console.WriteLine(string.Format("{0}: {1}", i, camera.CurrentScene.Entities[camera.drawList[i]].Id));
                }

                System.IO.File.WriteAllText("drawlist.txt", txt);
                //System.Diagnostics.Process.Start(Environment.CurrentDirectory);
            }

            // draw any and all temp entities
            camera.CurrentScene.DrawTempEntities();

        }

        static bool shouldDumpCurrentDrawList = false;

        [vxDebugMethodAttribute("drawlist", "Dumps out the most recent draw list")]
        static void DumpDrawList()
        {
            shouldDumpCurrentDrawList = true;
        }

    }
}