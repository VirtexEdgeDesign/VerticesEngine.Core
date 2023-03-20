
#region Using Statements
using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;
using VerticesEngine;
using VerticesEngine.Graphics;

#endregion

namespace VerticesEngine
{
    public partial class vxGameplayScene3D : vxGameplaySceneBase
    {
        int _totalEntityCount = 0;


        /// <summary>
        /// Is encoded index rendertarget needed for handling selection.
        /// </summary>
        public bool IsEncodedIndexNeeded = true;

        /// <summary>
        /// Gets a new entity handle for this scene.
        /// </summary>
        /// <returns>The new entity handle.</returns>
        public virtual int GetNewEntityHandle()
        {
            return _totalEntityCount++;
        }




        /// <summary>
        /// Draws the HUD Once the entire scene has been rendered.
        /// </summary>
        protected internal override void DrawHUD()
        {
            base.DrawHUD();
            DrawViewportSplitters();
        }

        protected internal override void DrawTempEntities()
        {
            for (int c = 0; c < Cameras.Count; c++)
            {
                if (TempPart != null)
                {
                    var tempMeshRenderer = ((vxMeshRenderer)TempPart.EntityRenderer);

                    if (tempMeshRenderer.IsMainRenderingEnabled)
                    {
                        tempMeshRenderer.IsMainRenderingEnabled = false;
                        foreach(var material in tempMeshRenderer.Materials)
                        {
                            material.IsDefferedRenderingEnabled = false;
                            material.IsShadowCaster = false;
                        }
                    }

                    TempPart.OnWillDraw(Cameras[c]);                    
                    tempMeshRenderer.DrawTempEntity(Cameras[c],Color.White);
                }
            }
        }

        public void PreDrawSelectedItems()
        {
            if (SandboxCurrentState == vxEnumSandboxStatus.EditMode)
            {
                for (int c = 0; c < Cameras.Count; c++)
                {
                    for (int i = 0; i < SelectedItems.Count; i++)
                    {
                        if (SelectedItems[i] != null && SelectedItems[i].Model != null && SelectedItems[i].MeshRenderer.IsRenderedThisFrame)
                        {
                            foreach (vxModelMesh mesh in SelectedItems[i].Model.Meshes)
                            {
                                // set camera values
                                vxGraphics.Util.EditorTempEntityShader.WVP = SelectedItems[i].Transform.RenderPassData.WVP;
                                vxGraphics.Util.EditorTempEntityShader.NormalColour = Color.Orange;// * 0.5f;
                                vxGraphics.Util.EditorTempEntityShader.LineThickness = 1;
                                vxGraphics.Util.EditorTempEntityShader.Alpha = 0.5f;

                                //mesh.Draw(vxGraphics.Util.EditorTempEntityShader);
                            }
                        }
                    }
                }
            }
        }

        public void DrawSelectedItems()
        {
            if (SandboxCurrentState == vxEnumSandboxStatus.EditMode)
            {
                //for (int c = 0; c < Cameras.Count; c++)
                //{
                //    for (int i = 0; i < SelectedItems.Count; i++)
                //    {
                //        if (SelectedItems[i] != null && SelectedItems[i].Model != null && SelectedItems[i].MeshRenderer.IsRenderedThisFrame)
                //        {
                //            foreach (vxModelMesh mesh in SelectedItems[i].Model.Meshes)
                //            {
                //                // set camera values
                //                vxGraphics.Util.EditorTempEntityShader.WVP = SelectedItems[i].Transform.RenderPassData.WVP;
                //                vxGraphics.Util.EditorTempEntityShader.NormalColour = Color.Orange;// * 0.5f;
                //                vxGraphics.Util.EditorTempEntityShader.LineThickness = 1;
                //                vxGraphics.Util.EditorTempEntityShader.Alpha = 0.5f;

                //                mesh.Draw(vxGraphics.Util.EditorTempEntityShader);
                //            }
                //        }
                //    }
                //}

                vxGraphics.SetRasterizerState(FillMode.WireFrame);

                for (int c = 0; c < Cameras.Count; c++)
                {
                    for (int i = 0; i < SelectedItems.Count; i++)
                    {
                        if (SelectedItems[i] != null && SelectedItems[i].Model != null && SelectedItems[i].MeshRenderer.IsRenderedThisFrame)
                        {
                            foreach (vxModelMesh mesh in SelectedItems[i].Model.Meshes)
                            {
                                vxGraphics.Util.WireframeShader.DoDebugWireFrame = true;
                                vxGraphics.Util.WireframeShader.WireColour = Color.Orange;                                
                                vxGraphics.Util.WireframeShader.World = SelectedItems[i].Transform.RenderPassData.World;
                                vxGraphics.Util.WireframeShader.WVP = SelectedItems[i].Transform.RenderPassData.WVP;

                                mesh.Draw(vxGraphics.Util.WireframeShader);
                            }
                        }
                    }
                }
                vxGraphics.SetRasterizerState(FillMode.Solid);
            }
        }

        

        protected internal override void DrawOverlayItems()
        {
            vxGraphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            vxGraphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            vxGraphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            if (!vxScreen.IsTakingScreenshot)
            {
                for (int c = 0; c < Cameras.Count; c++)
                {
                    for (int i = 0; i < EditorEntities.Count; i++)
                    {
                        if (EditorEntities[i] != null && EditorEntities[i].EntityRenderer != null)
                        {
                            if (EditorEntities[i].IsVisible && EditorEntities[i].EntityRenderer.IsDisposed == false)
                            {
                                EditorEntities[i].EntityRenderer.OnWillDraw(Cameras[c]);
                                EditorEntities[i].RenderOverlayMesh((vxCamera3D)Cameras[c]);
                            }
                        }
                    }
                }
            }
        }

        
        protected internal override void DrawViewportSplitters()
        {
            // Draw Split line(s)

            vxGraphics.SpriteBatch.Begin();
            // Always draw the Horizontal Line if the Viewport Count is greater than 1
            if (ViewportManager.NumberOfViewports > 1)
            {
                vxGraphics.SpriteBatch.Draw(vxInternalAssets.Textures.Blank,
                                        new Rectangle(
                                            0,
                                            Cameras[0].Viewport.Bounds.Bottom - 1,
                                            ViewportManager.MainViewport.Width,
                                            2),
                                        Color.Black);
                // Now draw the Verticle Line if the Viewport Count is greater than 2
                if (ViewportManager.NumberOfViewports > 2)
                {

                    vxGraphics.SpriteBatch.Draw(vxInternalAssets.Textures.Blank,
                                            new Rectangle(
                                                Cameras[0].Viewport.Bounds.Right - 1,
                                                0,
                                                2,
                                               ViewportManager.MainViewport.Height),
                                            Color.Black);
                }
            }

            vxGraphics.SpriteBatch.End();
        }
    }
}