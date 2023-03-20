using System;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

using VerticesEngine.Utilities;
using VerticesEngine.Graphics;
using Microsoft.Xna.Framework.Content;

namespace VerticesEngine.Graphics
{
	/// <summary>
	/// The model mesh part which holds the Geometry data such as
	/// Vertices, Normal, UV Texture Coordinates, Binormal and Tangent data.
	/// </summary>
	public class vxModelMeshPart : IDisposable
	{
		public int TriangleCount = 0;
		public int VertexCount;

		/// <summary>
		/// The vertex buffer.
		/// </summary>
		public VertexBuffer VertexBuffer;

		/// <summary>
		/// The index buffer.
		/// </summary>
		public IndexBuffer IndexBuffer;

		/// <summary>
		/// Gets the number vertices.
		/// </summary>
		/// <value>The number vertices.</value>
		[ContentSerializerIgnore]
		public int NumVertices
		{
			get
			{
                if (VertexBuffer != null)
                    return VertexBuffer.VertexCount;
                else
                    return 0;
			}
		}


		/// <summary>
		/// Mesh Name
		/// </summary>
		[ContentSerializerIgnore]
		public object Tag;

		[ContentSerializerIgnore]
		public int StartIndex = 0;

		[ContentSerializerIgnore]
		public int VertexOffset = 0;

		public vxModelMeshPart() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:VerticesEngine.Base.vxModelMeshPart"/> class.
		/// </summary>
		/// <param name="Engine">Engine.</param>
		/// <param name="part">The part to extract the Position, Normal, UV, Tangent and BiNormal data from.</param>
		public vxModelMeshPart(string modelPath, ModelMeshPart part)
        {
            if (part != null)
			{
				// Now Parse out the Vertex Info from the model mesh part
				try
				{					
					if (part.VertexBuffer.VertexDeclaration.VertexStride != 56)
					{
						vxConsole.WriteError("Error With Vertex Stride. Stride must be 56");
						vxConsole.WriteError("=======================================");
                        vxConsole.WriteError("File Path: " + modelPath);
						vxConsole.WriteError("Vertex Element Layout is:");
						foreach (VertexElement elmnt in part.VertexBuffer.VertexDeclaration.GetVertexElements())
							vxConsole.WriteError(elmnt.VertexElementUsage.ToString());

						vxConsole.WriteError("Please re-compile model with proper vertex elements.");

						throw new Exception("Vertex Stride Exception");
					}

					// Create the Mesh Vertice Array
				var	MeshVertices = new vxMeshVertex[part.VertexBuffer.VertexCount];

					// Now extract the data from the part's Vertex Buffer
					part.VertexBuffer.GetData<vxMeshVertex>(MeshVertices);

					// Account for the World Orientation in XNA/MG compared to Blender
					for (int vi = 0; vi < MeshVertices.Length; vi++)
					{
						var v = MeshVertices[vi];
						var matrix = Matrix.CreateRotationX(-MathHelper.PiOver2);
						MeshVertices[vi].Position = Vector3.Transform(v.Position, matrix);
						MeshVertices[vi].Normal = Vector3.TransformNormal(v.Normal, matrix);
						MeshVertices[vi].Tangent = Vector3.TransformNormal(v.Tangent, matrix);
						MeshVertices[vi].BiNormal = Vector3.TransformNormal(v.BiNormal, matrix);

					}

					// Create the Mesh Indices Array
					var Indices = new ushort[part.IndexBuffer.IndexCount];

					// Now extract the data from the part's Indices Buffer
					part.IndexBuffer.GetData<ushort>(Indices);

					TriangleCount = part.PrimitiveCount;
                    part.Tag = modelPath;

					StartIndex = part.StartIndex;
					VertexOffset = part.VertexOffset;
					if (StartIndex != 0 || VertexOffset != 0)
					{
						//Console.WriteLine("");
						Console.WriteLine("Part: " + modelPath);
						Console.WriteLine("======================");
						Console.WriteLine("StartIndex : " + StartIndex);
						Console.WriteLine("VertexOffset : " + VertexOffset);
					}

					SetData(MeshVertices, Indices);
				}
				catch (Exception ex)
				{
					// Sometimes there's errors, often time due to vertex stride is different than what's expected.
					vxConsole.WriteLine("ERROR: " + ex.Message + " >> Stride is: " + part.VertexBuffer.VertexDeclaration.VertexStride);
				}
			}
		}



        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.Graphics.vxModelMeshPart"/> class.
        /// </summary>
        /// <param name="Engine">Engine.</param>
        /// <param name="vertices">Vertices.</param>
        /// <param name="indices">Indices.</param>
        /// <param name="primitiveCount">Primitive count.</param>
		public vxModelMeshPart(vxMeshVertex[] vertices, ushort[] indices, int primitiveCount)
		{
			TriangleCount = primitiveCount;

			StartIndex = 0;
			VertexOffset = 0;

			SetData(vertices, indices);
		}

		public vxModelMeshPart(Vector3[] vertices, ushort[] indices, int primitiveCount)
		{
			List<vxMeshVertex> meshVertices = new List<vxMeshVertex>();

            foreach (var vert in vertices)
            {
                meshVertices.Add(new vxMeshVertex()
                {
                    Position = vert,
                    Normal = Vector3.Up,
                    TextureCoordinate = Vector2.Zero,
                    BiNormal = Vector3.Left,
                    Tangent = Vector3.Right
                });
            }

			TriangleCount = primitiveCount;

			StartIndex = 0;
			VertexOffset = 0;

			SetData(meshVertices.ToArray(), indices);
		}

		public virtual void SetData(vxMeshVertex[] vertices, ushort[] indices)
		{
			VertexBuffer = new VertexBuffer(vxGraphics.GraphicsDevice, typeof(vxMeshVertex), vertices.Length, BufferUsage.None);
			IndexBuffer = new IndexBuffer(vxGraphics.GraphicsDevice, typeof(ushort), indices.Length, BufferUsage.None);

			VertexBuffer.SetData<vxMeshVertex>(vertices);
			IndexBuffer.SetData(indices);
		}

		public virtual void GetData(out vxMeshVertex[] vertices, out ushort[] indices)
        {
			vertices = new vxMeshVertex[VertexBuffer.VertexCount];
			VertexBuffer.GetData<vxMeshVertex>(vertices);


			indices = new ushort[IndexBuffer.IndexCount];
			IndexBuffer.GetData<ushort>(indices);
		}


		/// <summary>
		/// Draws this mesh with the given effect.
		/// </summary>
		/// <param name="drawEffect">Draw effect.</param>
		public virtual void Draw(Effect drawEffect)
		{
            if (VertexBuffer != null)
			{
                vxGraphics.GraphicsDevice.Indices = IndexBuffer;
                vxGraphics.GraphicsDevice.SetVertexBuffer(VertexBuffer);

				foreach (EffectPass pass in drawEffect.CurrentTechnique.Passes)
				{
					pass.Apply();
                    vxGraphics.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, VertexOffset, StartIndex, TriangleCount);
				}
			}
        }

        public void Dispose()
        {
            IndexBuffer?.Dispose();
            VertexBuffer?.Dispose();

			VertexBuffer = null;
			IndexBuffer = null;
        }
    }
}
