using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using VerticesEngine.Graphics;
using Microsoft.Xna.Framework.Content;

namespace VerticesEngine.Graphics
{
    public enum MeshTextureType
    {
        Diffuse,
        NormalMap,
        RMAMap,
        DistortionMap,
        CubeMap,
        EmissiveMap
    }

    /// <summary>
    /// The model mesh.
    /// </summary>
    public class vxModelMesh : IDisposable
	{
		/// <summary>
		/// Mesh Name
		/// </summary>
		public string Name;

        /// <summary>
        /// The model mesh parts.
        /// </summary>
        [ContentSerializer]
        public List<vxModelMeshPart> MeshParts = new List<vxModelMeshPart>();

        /// <summary>
        /// This is a look up of related texture for this mesh
        /// </summary>
        internal Dictionary<MeshTextureType, Texture2D> MeshTextures
        {
            get { return m_meshTextures; }
        }
        private Dictionary<MeshTextureType, Texture2D> m_meshTextures = new Dictionary<MeshTextureType, Texture2D>();

        public Texture2D GetTexture(MeshTextureType type)
        {
            if (m_meshTextures.ContainsKey(type))
            {
                return m_meshTextures[type];
            }
            else
            {
                Texture2D result = null;
                switch(type)
                {
                    case MeshTextureType.Diffuse:
                        result = vxInternalAssets.Textures.DefaultDiffuse;
                        break;
                    case MeshTextureType.NormalMap:
                        result = vxInternalAssets.Textures.DefaultNormalMap;
                        break;
                    case MeshTextureType.RMAMap:
                        result = vxInternalAssets.Textures.DefaultSurfaceMap;
                        break;
                }

                return result;
            }
        }

        public void AddTexture(MeshTextureType type, Texture2D texture)
        {
            if (m_meshTextures.ContainsKey(type))
            {
                //vxConsole.WriteError($"Texture type {type} already defined");
                //m_meshTextures[type] = texture;
            }
            else
            {
                m_meshTextures.Add(type, texture);
            }
        }


        /// <summary>
		/// Initializes a new instance of the <see cref="T:VerticesEngine.Base.vxModelMesh"/> class.
        /// </summary>
        /// <param name="Engine">Engine Reference</param>


        public vxModelMesh()
        {

        }

        public vxModelMesh(vxModelMesh mesh) : this()
        {
            MeshParts = new List<vxModelMeshPart>();

            this.Name = mesh.Name;

            // clone over the textures
            foreach(var txtr in mesh.m_meshTextures)
            {
                m_meshTextures.Add(txtr.Key, txtr.Value);
            }
        }

        public vxModelMesh(Vector3[] Vertices, int[] indices)
        {
            MeshParts = new List<vxModelMeshPart>();
        }

        public void Dispose()
        {
            OnDisposed();
        }

        protected virtual void OnDisposed()
        {
            foreach(var part in MeshParts)
            {
                part.Dispose();
            }

            MeshParts.Clear();

            m_meshTextures.Clear();
        }


        /// <summary>
        /// Draws a mesh with the specified material
        /// </summary>
        /// <param name="material"></param>
        public virtual void Draw(vxMaterial material)
        {
            material.SetPass();

            for (int mp = 0; mp < MeshParts.Count; mp++)
                MeshParts[mp].Draw(material.Shader);

        }

        /// <summary>
        /// Draws a mesh with the specified effect
        /// </summary>
        /// <param name="drawEffect"></param>
        public virtual void Draw(Effect drawEffect)
        {
            for (int mp = 0; mp < MeshParts.Count; mp++)
                MeshParts[mp].Draw(drawEffect);
        }
	}
}
