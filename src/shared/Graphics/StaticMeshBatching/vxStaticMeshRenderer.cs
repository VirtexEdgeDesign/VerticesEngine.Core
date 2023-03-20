using System;
using System.Collections.Generic;
using System.Text;
using VerticesEngine.Graphics.Rendering;
using VerticesEngine.Input;

namespace VerticesEngine.Graphics
{
    /// <summary>
    /// Static Mesh Renderer is used for grouping static objects together to all be rendered as a group instead of rendering
    /// each mesh/object individually. Note that this must be an object which doesn't move and will use the same material for 
    /// all objects
    /// </summary>
    public class vxStaticMeshRenderer : vxMeshRenderer
    {
        // this is the associated batch renderer. We hold a reference to it so that we can
        // update it if/when something changes with this entity and/or mesh
        vxStaticMeshBatchRenderer batchRenderer;


        protected override void OnMeshSet()
        {
            base.OnMeshSet();

            // our mesh was updated, we should tell our static batcher that we need to update
            if(Mesh != null)
            {
                batchRenderer.SetDirty();
            }
        }

        /// <summary>
        /// Registeres this static mesh renderer for a given archetype
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        public void RegisterAsArchetype<T>() where T : vxEntity3D
        {
            // get the subsystem
            var staticRendererSubSystem = vxEngine.Instance.CurrentScene.GetSubSystem<vxStaticMeshBatchRenderSystem>();
            batchRenderer = staticRendererSubSystem.RegisterEntityToArchetype<T>((vxEntity3D)this.Entity);
        }


        public override void Draw(vxCamera Camera, string renderpass)
        {
            if (batchRenderer.IsEnabled && vxInput.IsNewKeyPress(Microsoft.Xna.Framework.Input.Keys.M))
                batchRenderer.IsEnabled = false;

            else if (!batchRenderer.IsEnabled && vxInput.IsNewKeyPress(Microsoft.Xna.Framework.Input.Keys.N))
                batchRenderer.IsEnabled = true;

            IsMainRenderingEnabled = !batchRenderer.IsEnabled;

            // we won't render the mesh in this renderer, we're going to use the internal system to do so
            if (batchRenderer != null && !batchRenderer.IsEnabled)
                base.Draw(Camera, renderpass);
        }
    }
}
