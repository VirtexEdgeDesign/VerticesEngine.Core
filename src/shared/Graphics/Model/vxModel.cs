using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using VerticesEngine.ContentManagement;

namespace VerticesEngine.Graphics
{
    /// <summary>
    /// A custom mesh class which holds vertices mesh data and texture references. Note a material should
    /// be applied to this mesh. 
    /// </summary>
    public class vxMesh :  IDisposable
    {
        /// <summary>
        /// The Models Name (Most often just the File Name).
        /// </summary>
        public string Name;

        /// <summary>
        /// The Primitive Count for this Entity Model. It is the summation of all Meshes and Parts.
        /// This can be used when debuging how many primitives are being drawn per draw call.
        /// </summary>
        public int TotalPrimitiveCount
        {
            get { return _totalCount; }
        }
        private int _totalCount = 0;


        public BoundingBox BoundingBox
        {
            get { return _boundingBox; }
        }
        BoundingBox _boundingBox;


        /// <summary>
        /// The model meshes.
        /// </summary>
        [ContentSerializer]
        public List<vxModelMesh> Meshes = new List<vxModelMesh>();



        public vxMesh()
        {

        }

        /// <summary>
        /// Basic Constructor. Note: All Items must be instantiated outside of this function.
        /// </summary>
        public vxMesh(string Name="") : base()
        {
            this.Name = Name;
            Meshes = new List<vxModelMesh>();
        }

        public void AddModelMesh(vxModelMesh mesh)
        {
            Meshes.Add(mesh);
        }

        public void Dispose()
        {
            OnDisposed();
        }

        protected virtual void OnDisposed()
        {
            foreach (var mesh in Meshes)
                mesh.Dispose();

            Meshes.Clear();
        }

        public void UpdateBoundingBox()
        {
            if (vxEngine.PlatformType == vxPlatformHardwareType.Mobile)
            {
                _boundingBox = new BoundingBox(Vector3.One * -10, Vector3.One * 10);
            }
            else
            {
                _boundingBox = vxMeshHelper.GetModelBoundingBox(this, Matrix.Identity);
            }
            int primCount = 0;
            foreach (vxModelMesh m in this.Meshes)
            {
                if(m.MeshParts.Count > 1)
                {
                    vxConsole.WriteWarning("", $">>>>>>>> {Name} Has multiple parts");
                }
                foreach (vxModelMeshPart part in m.MeshParts)
                {
                    primCount += part.TriangleCount;
                }
            }
            _totalCount = primCount;
        }        
    }
}

