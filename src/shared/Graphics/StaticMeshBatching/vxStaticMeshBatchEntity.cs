using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.Graphics.Rendering
{
    /// <summary>
    /// This entity holds the batched-combined mesh for a static mesh archetype
    /// </summary>
    public class vxStaticMeshBatchEntity : vxEntity3D
    {
        public vxStaticMeshBatchEntity() 
        {
            
        }

        protected override vxMesh OnLoadModel()
        {
            return vxInternalAssets.Models.UnitTorus;
        }

        vxStaticMeshBatchRenderer meshBatchRenderer;

        protected override vxEntityRenderer CreateRenderer()
        {
            if(meshBatchRenderer == null)
                meshBatchRenderer = AddComponent<vxStaticMeshBatchRenderer>();

            return meshBatchRenderer;
        }

        public void AddEntity(vxEntity3D entity)
        {
            meshBatchRenderer.entities.Add(entity);
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();
        }
    }
}
