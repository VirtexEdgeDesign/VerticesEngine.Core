using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace VerticesEngine.Graphics
{
    public class vxLineRenderer : vxMeshRenderer
    {
        /// <summary>
        /// The vertices.
        /// </summary>
        public List<VertexPositionColor> Vertices = new List<VertexPositionColor>();

        /// <summary>
        /// The indices.
        /// </summary>
        public List<short> Indices = new List<short>();


        /// <summary>
        /// The effect to draw this mesh part with.
        /// </summary>
        public BasicEffect Effect
        {
            get { return trailRendererMesh.basicEffect; }
        }

        public float LineThickness = 3;

        public int stride = 5;

        public Color LineColour = Color.WhiteSmoke;




        public int PrimitiveCount = 0;

        public int StartIndex = 0;

        public int VertexOffset = 0;

        short indcnt = 4;



        /// <summary>
        /// once the owner is gone, start fading the trails
        /// </summary>
        public float DecrementRateFactor = 0;

        float Transparancy = 1;


        protected override void Initialise()
        {
            LineColour = Color.White;
            //Effect = new BasicEffect(vxGraphics.GraphicsDevice);

            Mesh = new vxMesh("LineRenderer");
            trailRendererMesh = new vxLineRendererMesh();
            Mesh.AddModelMesh(trailRendererMesh);
            this.Materials.Add(new vxMaterial(new BasicEffect(vxGraphics.GraphicsDevice)));

            base.Initialise();
        }


        private vxLineRendererMesh trailRendererMesh;

        public class vxLineRendererMesh : vxModelMesh
        {

            /// <summary>
            /// The vertex buffer.
            /// </summary>
            public DynamicVertexBuffer VertexBuffer;

            /// <summary>
            /// The index buffer.
            /// </summary>
            public DynamicIndexBuffer IndexBuffer;

            public vxLineRendererMesh()
            {
                basicEffect = new BasicEffect(vxGraphics.GraphicsDevice);
            }

            public BasicEffect basicEffect;
            int indcnt = 0;
            public void SetData(List<VertexPositionColor> Vertices, List<short> Indices, int indcnt)
            {
                this.indcnt = indcnt;
                VertexBuffer = new DynamicVertexBuffer(vxGraphics.GraphicsDevice, typeof(VertexPositionColor), Vertices.ToArray().Length, BufferUsage.None);
                VertexBuffer.SetData<VertexPositionColor>(Vertices.ToArray());

                IndexBuffer = new DynamicIndexBuffer(vxGraphics.GraphicsDevice, typeof(short), Indices.ToArray().Length, BufferUsage.None);
                IndexBuffer.SetData(Indices.ToArray());

            }


            public override void Draw(vxMaterial material)
            {
                // now draw tail
                if (VertexBuffer != null )
                {
                    //Init();
                    vxGraphics.GraphicsDevice.Indices = IndexBuffer;
                    vxGraphics.GraphicsDevice.SetVertexBuffer(VertexBuffer);

                    vxGraphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;

                    foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
                    {
                        pass.Apply();

                        vxGraphics.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, indcnt);
                    }
                }
            }
        }

        protected internal override void OnWillDraw(vxCamera Camera)
        {
            base.OnWillDraw(Camera);

            Effect.View = Camera.View;
            Effect.Projection = Camera.Projection;
            Effect.World = Matrix.Identity;// this.World;
            Effect.VertexColorEnabled = true;
            //Effect.AmbientLightColor
            Effect.DiffuseColor = LineColour.ToVector3();

            Effect.Alpha = Transparancy;

        }

        protected override void OnDisposed()
        {
            base.OnDisposed();

            //Effect.Dispose();
            Vertices.Clear();

        }

        public void Clear()
        {
            Vertices.Clear();
            Indices.Clear();
            indcnt=0;
        }

        public void InitLine(Vector3 Position)
        {

            IsCullable = false;
            Vertices.Add(new VertexPositionColor(Position + new Vector3(LineThickness, 0, 0), LineColour * Transparancy));
            Vertices.Add(new VertexPositionColor(Position + new Vector3(-LineThickness, 0, 0), LineColour * Transparancy));
            Vertices.Add(new VertexPositionColor(Position + new Vector3(LineThickness, 0, 0), LineColour * Transparancy));
            Vertices.Add(new VertexPositionColor(Position + new Vector3(-LineThickness, 0, 0), LineColour * Transparancy));

            Indices.Add(0);
            Indices.Add(1);
            Indices.Add(2);

            Indices.Add(1);
            Indices.Add(2);
            Indices.Add(3);
        }

        public void AddStep(Vector3 Position, float Rotation)
        {
            indcnt += 2;

            Transparancy = Math.Min(1, Transparancy + 0.1f);


            Vector3 rotationalOffset = Vector3.Transform(Vector3.UnitX * LineThickness, Matrix.CreateRotationY(vxMathHelper.DegToRad(Rotation)));
            LineColour = Color.White;
            Vertices.Add(new VertexPositionColor(Position + rotationalOffset, LineColour * Transparancy));
            Vertices.Add(new VertexPositionColor(Position - rotationalOffset, LineColour * Transparancy));


            Indices.Add((short)(indcnt - 4));
            Indices.Add((short)(indcnt - 3));
            Indices.Add((short)(indcnt - 2));

            Indices.Add((short)(indcnt - 3));
            Indices.Add((short)(indcnt - 2));
            Indices.Add((short)(indcnt - 1));

            trailRendererMesh.SetData(Vertices, Indices, indcnt);
        }
    }
}
