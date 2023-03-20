using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace VerticesEngine.Graphics
{
	/// <summary>
	/// The vertex structure for drawing Models in the Vertices Engine. It uses Position, Normal, UV, Tangent and BiNormal.
	/// </summary>
	public struct vxMeshVertex : IVertexType
	{
		//Vertx Graphical Properties
		public Vector3 Position;
		public Vector3 Normal;
		public Vector2 TextureCoordinate;
		public Vector3 Tangent;
		public Vector3 BiNormal;

		//Size of the Vertes Declaration
		public static int SizeInBytes = (3 + 3 + 2 + 3 + 3) * 4;

		// Describe the layout of this vertex structure.
		public static VertexDeclaration VertexDeclaration = new VertexDeclaration
			(
				new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
				new VertexElement(sizeof(float) * 3, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
				new VertexElement(sizeof(float) * 6, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
				new VertexElement(sizeof(float) * 8, VertexElementFormat.Vector3, VertexElementUsage.Tangent, 0),
				new VertexElement(sizeof(float) * 11, VertexElementFormat.Vector3, VertexElementUsage.Binormal, 0)
			);

		VertexDeclaration IVertexType.VertexDeclaration
		{
			get
			{
				return VertexDeclaration;
			}
		}


		/// <summary>
		/// Transform the specified vertex by the given matrix.
		/// </summary>
		/// <returns>The transform.</returns>
		/// <param name="vertex">Vertex struct.</param>
		/// <param name="matrix">Transformation Matrix.</param>
		public static vxMeshVertex Transform(vxMeshVertex vertex, Matrix matrix)
		{
			vertex.Position = Vector3.Transform(vertex.Position, matrix);
			vertex.Normal = Vector3.Transform(vertex.Normal, matrix);
			vertex.Tangent = Vector3.Transform(vertex.Tangent, matrix);
			vertex.BiNormal = Vector3.Transform(vertex.BiNormal, matrix);

			return vertex;
		}
	}

}
