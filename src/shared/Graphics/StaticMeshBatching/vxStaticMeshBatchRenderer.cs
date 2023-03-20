using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VerticesEngine.Input;

namespace VerticesEngine.Graphics.Rendering
{
    /// <summary>
    /// This batches all of the <see cref="vxStaticMeshRenderer"/> for the given archetype together and then renderers them as a single draw call.
    /// </summary>
    public class vxStaticMeshBatchRenderer : vxMeshRenderer
    {
        internal List<vxEntity3D> entities = new List<vxEntity3D>();

        protected override void OnDisposed()
        {
            entities.Clear();
            base.OnDisposed();
        }

        public void SetDirty()
        {
            _isMeshDirty = true;
        }
        private bool _isMeshDirty = false;

        protected internal override void PostUpdate()
        {
            base.PostUpdate();
            if(_isMeshDirty)
            {
                _isMeshDirty = false;
                FullMeshRefresh();
            }
        }

        /// <summary>
        /// Tear down what we have and do a full refresh. This is spicy.
        /// </summary>
        public void FullMeshRefresh()
        {
            // clear the current mesh
            for(int mm = 0; mm < Mesh.Meshes.Count; mm++)
            {
                Mesh.Meshes[mm].Dispose();
            }
            Mesh.Meshes.Clear();

            // Materials.Clear();


            vxMesh mesh = new vxMesh("StaticMesh");  
            for(int m = 0; m < 2; m++)
            {
                mesh.AddModelMesh(new vxModelMesh());
            }

            // create the buffers to create the static entities
            List<vxMeshVertex> meshVerticesBuffer = new List<vxMeshVertex>();
            List<ushort> indicesBuffer = new List<ushort>();

            // now loop through each entity, get their mesh data and add it to the mega buffer
            ushort runningIndex = 0;
            for(int e = 0; e < entities.Count; e++)
            {
                if (entities[e] == null || entities[e].MeshRenderer.Mesh == null)
                    continue;

                // if (Materials.Count == 0)
                //     Materials.Add(entities[e].MeshRenderer.GetMaterial(0));

                var entityMesh = entities[e].MeshRenderer.Mesh;
                for(int mm = 0; mm < entityMesh.Meshes.Count; mm++)
                {
                    foreach (var entityPart in entityMesh.Meshes[mm].MeshParts)
                    {
                        entityPart.GetData(out var partVerts, out var partIndices);

                        // add in each vertex
                        for(int v =0; v < partVerts.Length; v++)
                        {
                            var vert = partVerts[v];
                            var mat = entities[e].Transform.Matrix4x4Transform;
                            partVerts[v].Position = Vector3.Transform(vert.Position, mat );

                            partVerts[v].Normal = Vector3.TransformNormal(vert.Normal, mat );
                            partVerts[v].Tangent = Vector3.TransformNormal(vert.Tangent, mat );
                            partVerts[v].BiNormal = Vector3.TransformNormal(vert.BiNormal, mat );
                            meshVerticesBuffer.Add(partVerts[v]);
                        }

                        for(int pi = 0; pi < partIndices.Length; pi++)
                        {
                            var indiciesVal = (ushort)(partIndices[pi] + runningIndex);
                            indicesBuffer.Add(indiciesVal);
                        }
                        runningIndex += (ushort)(partVerts.Length);


                        // if we go over then we should create a new mesh part
                        if(indicesBuffer.Count >= ushort.MaxValue * 3)
                        {
                            mesh.Meshes[mm].MeshParts.Add(new vxModelMeshPart(meshVerticesBuffer.ToArray(), indicesBuffer.ToArray(), indicesBuffer.Count));
                            meshVerticesBuffer.Clear();
                            indicesBuffer.Clear();
                            runningIndex = 0;
                        }
                    }

                    if (indicesBuffer.Count > 0)
                    {
                        mesh.Meshes[mm].MeshParts.Add(new vxModelMeshPart(meshVerticesBuffer.ToArray(), indicesBuffer.ToArray(), indicesBuffer.Count));
                    }
                }
            }

            mesh.UpdateBoundingBox();
            this.Mesh = mesh;
        }
    }
}
