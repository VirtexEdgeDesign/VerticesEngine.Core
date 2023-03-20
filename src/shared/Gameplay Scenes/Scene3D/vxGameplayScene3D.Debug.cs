

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine;
using VerticesEngine.Diagnostics;
using VerticesEngine.Graphics;

#endregion

namespace VerticesEngine
{
    public partial class vxGameplayScene3D : vxGameplaySceneBase
    {
        protected internal override void DrawPhysicsDebug(vxCamera camera)
        {
            PhysicsDebugViewer.Update();
            PhysicsDebugViewer.Draw(camera.View, camera.Projection);

            vxGraphics.SetRasterizerState(FillMode.WireFrame);

            for (int c = 0; c < Cameras.Count; c++)
            {
                for (int i = 0; i < Entities.Count; i++)
                {
                    vxEntity3D entity = Entities[i].CastAs<vxEntity3D>();
                    if (entity != null && entity.Model != null && entity.MeshRenderer.IsRenderedThisFrame)
                    {
                        foreach (vxModelMesh mesh in entity.Model.Meshes)
                        {
                            vxGraphics.Util.WireframeShader.DoDebugWireFrame = true;
                            vxGraphics.Util.WireframeShader.WireColour = Color.White * 0.5f;
                            vxGraphics.Util.WireframeShader.World = entity.Transform.RenderPassData.World;
                            vxGraphics.Util.WireframeShader.WVP = entity.Transform.RenderPassData.WVP;

                            mesh.Draw(vxGraphics.Util.WireframeShader);
                        }
                    }
                }
            }
            vxGraphics.SetRasterizerState(FillMode.Solid);
        }

        void UpdateDebug()
        {
            if (vxDebug.IsDebugMeshVisible)
                PhysicsDebugViewer.Update();
        }


        protected internal override void DrawDebug()
        {
            base.DrawDebug();

            foreach (vxCamera3D camera in Cameras)
            {
                vxDebug.DrawShapes(camera.View, camera.Projection);
            }
        }
    }
}

