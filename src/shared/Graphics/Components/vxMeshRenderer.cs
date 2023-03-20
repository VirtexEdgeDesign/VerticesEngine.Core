using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using VerticesEngine;

namespace VerticesEngine.Graphics
{
    /// <summary>
    /// Mesh Renderer Component for centralizing the logic for rendering a 3D Mesh with a specific set of materials
    /// </summary>
    public class vxMeshRenderer : vxEntityRenderer
    {
        /// <summary>
        /// The mesh reference rendererd by this component. A single mesh can be rendered multiple times by different entities
        /// using different materials
        /// </summary>
        public vxMesh Mesh
        {
            get { return m_mesh; }
            set
            {
                m_mesh = value;
                OnMeshSet();
            }
        }
        private vxMesh m_mesh;

        protected virtual void OnMeshSet()
        {

        }

        /// <summary>
        /// Does this renderer cast shadows
        /// </summary>
        public bool IsShadowCaster
        {
            get { return m_isShadowCaster; }
            set { m_isShadowCaster = value; }
        }
        private bool m_isShadowCaster = true;

        public BoundingSphere BoundingShape
        {
            get { return Entity.BoundingShape; }
        }

        /// <summary>
        /// The material to render the mesh with
        /// </summary>
        public List<vxMaterial> Materials
        {
            get { return m_materials; }
        }

        internal Color IndexEncodedColour { get { return ((vxEntity3D)Entity).IndexEncodedColour; } }


        private List<vxMaterial> m_materials = new List<vxMaterial>();

        public vxMaterial GetMaterial(int i)
        {
            int index = vxMathHelper.Clamp(i, 0, m_materials.Count - 1);

            return m_materials[index];
        }

        public List<T> GetMaterials<T>() where T : vxMaterial
        {
            List<T> materials = new List<T>();

            foreach (var mat in m_materials)
            {
                if (mat.GetType() == typeof(T))
                {
                    materials.Add((T)mat);
                }
            }


            return materials;
        }

        void Log(string text)
        {
#if LOGGING
            vxConsole.WriteLine($"MeshRenderer: {this.Entity.Id} - {text}");
#endif
        }

        protected virtual void AddToSceneCollection()
        {
            if (Entity != null && Entity.CurrentScene.MeshRenderers.Contains(this) == false)
                Entity.CurrentScene.MeshRenderers.Add(this);
        }

        protected virtual void RemoveFromSceneCollection()
        {
            if (Entity != null && Entity.CurrentScene.MeshRenderers.Contains(this))
                Entity.CurrentScene.MeshRenderers.Remove(this);
        }

        /// <summary>
        /// Called once when created
        /// </summary>
        protected override void Initialise()
        {
            base.Initialise();

            Log("Initialise()");
            AddToSceneCollection();
        }

        protected internal override void OnEnabled()
        {
            base.OnEnabled();

            // add this mesh renderer to the scenes render list
            AddToSceneCollection();

            Log("OnEnabled()");
        }

        protected internal override void OnDisabled()
        {
            base.OnDisabled();

            // add this mesh renderer to the scenes render list
            RemoveFromSceneCollection();

            Log("OnDisabled()");
        }


        /// <summary>
        /// Called each frame 
        /// </summary>
        protected internal override void Update()
        {
            base.Update();
        }


        /// <summary>
        /// When the component or owning entity is disposed
        /// </summary>
        protected override void OnDisposed()
        {
            RemoveFromSceneCollection();

            for (int m = 0; m < Materials.Count; m++)
            {
                Materials[m].Dispose();
            }

            Materials.Clear();
            m_materials = null;

            m_mesh = null;

            base.OnDisposed();
        }

        protected internal override void OnWillDraw(vxCamera Camera)
        {
            base.OnWillDraw(Camera);

            RenderPassData.IsShadowCaster = m_isShadowCaster;
            RenderPassData.IndexColour = IndexEncodedColour;
        }


        /// <summary>
        /// Draws this mesh renderer with it's mesh and it's materials
        /// </summary>
        /// <param name="Camera"></param>
        /// <param name="renderpass"></param>
        public override void Draw(vxCamera Camera, string renderpass)
        {
            if(IsMainRenderingEnabled)
            {
                for (int m = 0; m < Mesh.Meshes.Count; m++)
                {
                    var mesh = Mesh.Meshes[m];
                    var material = GetMaterial(m);
                    if (renderpass == material.MaterialRenderPass)
                    {
                        // set transforms
                        material.World = RenderPassData.World;
                        material.WorldInverseTranspose = RenderPassData.WorldInvT;
                        material.WVP = RenderPassData.WVP;

                        // set camera values
                        material.View = Camera.View;
                        material.Projection = Camera.Projection;
                        material.CameraPosition = Camera.Position;

                        mesh.Draw(material);
                    }
                }
            }
        }

        internal void Draw(Matrix World, Matrix View, Matrix Projection, Vector3 CameraPosition, string renderpass)
        {
            var worldInvT = Matrix.Transpose(Matrix.Invert(World));
            if (IsMainRenderingEnabled)
            {
                for (int m = 0; m < Mesh.Meshes.Count; m++)
                {
                    var mesh = Mesh.Meshes[m];
                    var material = GetMaterial(m);
                    if (renderpass == material.MaterialRenderPass)
                    {
                        // set transforms
                        material.World = World;
                        material.WorldInverseTranspose = worldInvT;
                        material.WVP = World * View * Projection;

                        // set camera values
                        material.View = View;
                        material.Projection = Projection;
                        material.CameraPosition = CameraPosition;

                        mesh.Draw(material);
                    }
                }
            }
        }

        internal void DrawShadow(vxShadowEffect shadowEffect)
        {
            if (IsMainRenderingEnabled)
            {
                shadowEffect.World.SetValue(RenderPassData.World);
                for (int mi = 0; mi < Mesh.Meshes.Count; mi++)
                {
                    var mesh = Mesh.Meshes[mi];
                    var material = GetMaterial(mi);
                    if (material.IsShadowCaster)
                    {
                        mesh.Draw(shadowEffect);
                    }
                }
            }
        }


        internal bool IsMainRenderingEnabled = true;


        public void DrawTempEntity(vxCamera Camera, Color wireColour)
        {
            
            foreach (vxModelMesh mesh in Mesh.Meshes)
            {
                // set camera values
                vxGraphics.Util.EditorTempEntityShader.WVP = RenderPassData.WVP;
                vxGraphics.Util.EditorTempEntityShader.NormalColour = wireColour * 0.5f;

                mesh.Draw(vxGraphics.Util.EditorTempEntityShader);
            }
            
            DrawWireFrame(wireColour);
        }


        public void DrawWireFrame(Color wireColour)
        {
            vxGraphics.SetRasterizerState(FillMode.WireFrame);
            foreach (vxModelMesh mesh in Mesh.Meshes)
            {
                vxGraphics.Util.WireframeShader.DoDebugWireFrame = true;
                vxGraphics.Util.WireframeShader.WireColour = wireColour;
                vxGraphics.Util.WireframeShader.World = RenderPassData.World;
                vxGraphics.Util.WireframeShader.WVP = RenderPassData.WVP;

                mesh.Draw(vxGraphics.Util.WireframeShader);
            }
            vxGraphics.SetRasterizerState(FillMode.Solid);
        }
    }
}
