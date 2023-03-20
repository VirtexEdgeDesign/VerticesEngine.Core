using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using VerticesEngine.Graphics;
using VerticesEngine.Input;

namespace VerticesEngine.Editor.Entities
{
    internal class WorkingPlaneMesh : vxModelMesh
    {
        const int GRID_SIZE = 10000;

        internal BasicEffect _wireFrameBasicEffect;

        private int _primitiveleCount = 0;

        /// <summary>
        /// The vertex buffer.
        /// </summary>
        private VertexBuffer _vertexBuffer;

        /// <summary>
        /// The index buffer.
        /// </summary>
        private IndexBuffer _indexBuffer;

        internal WorkingPlaneMesh()
        {
            _wireFrameBasicEffect = new BasicEffect(vxGraphics.GraphicsDevice);
            _wireFrameBasicEffect.VertexColorEnabled = true;

            List<VertexPositionColor> verticesBuffer = new List<VertexPositionColor>();
            List<ushort> indicesBuffer = new List<ushort>();
            
            for (int i = -GRID_SIZE; i < GRID_SIZE + 1; i += 25)
            {
                Color color = i % 500 == 0 ? Color.White : Color.Gray * 1;

                indicesBuffer.Add((ushort)verticesBuffer.Count);
                verticesBuffer.Add(new VertexPositionColor(
                     new Vector3(i, 0, -GRID_SIZE),
                     color
                     ));

                indicesBuffer.Add((ushort)verticesBuffer.Count);
                verticesBuffer.Add(new VertexPositionColor(
                     new Vector3(i, 0, GRID_SIZE),
                     color
                     ));

                indicesBuffer.Add((ushort)verticesBuffer.Count);
                verticesBuffer.Add(new VertexPositionColor(
                    new Vector3(-GRID_SIZE, 0, i),
                    color
                    ));

                indicesBuffer.Add((ushort)verticesBuffer.Count);
                verticesBuffer.Add(new VertexPositionColor(
                     new Vector3(GRID_SIZE, 0, i),
                     color
                     ));

            }


            _primitiveleCount = verticesBuffer.Count/2;

            SetData(verticesBuffer.ToArray(), indicesBuffer.ToArray());
        }

        private void SetData(VertexPositionColor[] vertices, ushort[] indices)
        {
            _vertexBuffer = new VertexBuffer(vxGraphics.GraphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.None);
            _indexBuffer = new IndexBuffer(vxGraphics.GraphicsDevice, typeof(ushort), indices.Length, BufferUsage.None);

            _vertexBuffer.SetData<VertexPositionColor>(vertices);
            _indexBuffer.SetData(indices);
        }

        public override void Draw(vxMaterial material)
        {
            if (_vertexBuffer != null)
            {
                vxGraphics.GraphicsDevice.Indices = _indexBuffer;
                vxGraphics.GraphicsDevice.SetVertexBuffer(_vertexBuffer);

                foreach (EffectPass pass in _wireFrameBasicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    vxGraphics.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.LineList, 0, 0, _primitiveleCount);
                }
            }
        }
    }
    public class WorkingPlaneRenderer : vxMeshRenderer
    {
        WorkingPlaneMesh workingPlaneMesh;


        protected override void Initialise()
        {
            base.Initialise();
            IsShadowCaster = false;
            
            workingPlaneMesh = new WorkingPlaneMesh();
        }

        public void InitMesh()
        {
            Mesh = new vxMesh("WorkingPlane");
            Mesh.AddModelMesh(workingPlaneMesh);
        }

        protected internal override void OnWillDraw(vxCamera Camera)
        {
            base.OnWillDraw(Camera);

            workingPlaneMesh._wireFrameBasicEffect.View = Camera.View;
            workingPlaneMesh._wireFrameBasicEffect.Projection = Camera.Projection;
            workingPlaneMesh._wireFrameBasicEffect.World = RenderPassData.World * Matrix.CreateTranslation(0, 0.01f, 0);
        }
    }
    public class vxWorkingPlane : vxEntity3D
    {
        public static vxWorkingPlane Instance
        {
            get { return _instance; }
        }
        private static vxWorkingPlane _instance;

        /// <summary>
        /// Working Plane Object
        /// </summary>
        public Plane WrknPlane;


        private float m_heightOffset = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.Entities.Util.vxWorkingPlane"/> class.
        /// </summary>
        /// <param name="scene">Scene.</param>
        /// <param name="entityModel">Entity model.</param>
        /// <param name="StartPosition">Start position.</param>
        internal vxWorkingPlane(vxGameplayScene3D scene, vxMesh entityModel, Vector3 StartPosition)
            : base(scene, entityModel, StartPosition)
        {
            _instance = this;
            RemoveSandboxOption(SandboxOptions.Save);
            RemoveSandboxOption(SandboxOptions.Export);
            //Render even in debug mode
            RenderEvenInDebug = true;
            IsEntityCullable = false;
            

            foreach (var mat in MeshRenderer.Materials)
                mat.IsDefferedRenderingEnabled = false;

            WrknPlane = new Plane(Vector3.Up, -Position.Y);
            
            this.IsVisible = false;

            workingPlaneRenderer.InitMesh();
        }
        WorkingPlaneRenderer workingPlaneRenderer;
        protected override vxEntityRenderer CreateRenderer()
        {
            if(workingPlaneRenderer == null)
                workingPlaneRenderer = AddComponent<WorkingPlaneRenderer>();

            return workingPlaneRenderer;
        }

        /// <summary>
        /// Updates the Working Plane
        /// </summary>
        /// <param name="gameTime"></param>
        protected internal override void Update()
        {
            //Update If In Edit Mode
            if (Scene.SandboxCurrentState == vxEnumSandboxStatus.EditMode)
            {
                //Set the Working Plane Height
                if (Scene.UIManager.HasFocus == false &&
                    vxInput.MouseState.MiddleButton == ButtonState.Released &&
                    Scene.SandboxEditMode == vxEnumSanboxEditMode.AddItem &&
                    vxInput.IsKeyDown(Keys.LeftShift))
                {
                    if(vxInput.ScrollWheelDelta != 0)
                    {
                        m_heightOffset += vxInput.ScrollWheelDelta / 100f;
                    }
                }
            }

            if (Scene.SandboxCurrentState == vxEnumSandboxStatus.EditMode &&
                (Scene.SandboxEditMode == vxEnumSanboxEditMode.AddItem||
                 Scene.SandboxEditMode == vxEnumSanboxEditMode.TerrainEdit))
            {
                Position = (Vector3.Up * (0.5f + m_heightOffset));
                WrknPlane.D = -m_heightOffset - 0.5f;
            }
            else
            {
                Transform.Scale = Vector3.One * 0.35f;
                WrknPlane.D = -1000000 - 0.5f;
            }
            base.Update();

            IsVisible = (Scene.SandboxCurrentState == vxEnumSandboxStatus.EditMode);
        }
    }
}
