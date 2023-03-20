using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

using VerticesEngine.Utilities;
using VerticesEngine.Graphics;

namespace VerticesEngine.Graphics
{
	/// <summary>
	/// The model mesh part which holds the Geometry data such as
	/// Vertices, Normal, UV Texture Coordinates, Binormal and Tangent data.
	/// </summary>
	public class vxTerrainMeshPart : vxModelMeshPart
    {
        /// <summary>
        /// The vertices for this Mesh. By default for the Vertices Engine, it includes Position, Normal, 
        /// UV Texture Coordinate, Tangent and BiNormal.
        /// </summary>
        public vxMeshVertex[] MeshVertices;

        /// <summary>
        /// The indices of this Mesh.
        /// </summary>
        public ushort[] Indices;

        //public vxMeshVertex[] MeshVertices = new vxMeshVertex[4];

        //public ushort[] Indices = new ushort[6];

        public int Dimension
        {
            get { return dimension; }
            set
            {
                dimension = value;
				//Effect.Parameters["textureSize"].SetValue((float)value);
				//Effect.Parameters["texelSize"].SetValue(1 / (float)value);
            }
        }
        int dimension = 128;

       
        public float CellSize
        {
            get { return _cellSize; }
            set
            {
                _cellSize = value;
                //Effect.Parameters["CellSize"].SetValue(value);
            }
        }
        private float _cellSize = 10;

        public Texture2D DisplacementMap
        {
            get { return _displacementMap; }
            set
            {
                _displacementMap = value;
               // Effect.Parameters["displacementMap"].SetValue(value.ToVector4(vxGraphics.GraphicsDevice));
            }
        }
        Texture2D _displacementMap;

		/// <summary>
		/// Gets or sets the max height of the terrain.
		/// </summary>
		/// <value>The height of the max.</value>
		public float MaxHeight
		{
			get { return _maxHeight; }
			set
			{
				_maxHeight = value;
				//Effect.Parameters["maxHeight"].SetValue(_maxHeight);
			}
		}
		float _maxHeight = 92;

        public float[,] HeightData;

        public Vector3 Position;

        /// <summary>
        /// Gets or sets the collection of Vertices.
        /// </summary>
        /// <value>The vertices.</value>
        public Vector3[] VerticesPoints;

        public Vector3 MaxPoint = new Vector3(float.MinValue, float.MinValue, float.MinValue);

        public Vector3 MinPoint = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);



        /// <summary>
        /// Indexer to Access Height Data Array
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public float this[int i, int j]
        {
            get {

                i = MathHelper.Clamp(i, 0, 128);
                j = MathHelper.Clamp(j, 0, 128);


                return HeightData[i,j]; }
            set {

                // The setter has to update a few different Values.

                // First Update the Main Height Data Array.
                HeightData[i, j] = value;

                // Get the previous position for the X and Y Coordinates
                Vector3 PreviousVector = MeshVertices[i * (Dimension + 1) + j].Position;

                // Now Set the Vertices Position
                MeshVertices[i * (Dimension + 1) + j].Position = new Vector3(PreviousVector.X, value, PreviousVector.Z);
                
                VerticesPoints[i * (Dimension + 1) + j] = new Vector3(PreviousVector.X, value, PreviousVector.Z);
            }
        }

        /// <summary>
        /// Returns the Position of a Vertices given a Grid Coordinate of X and Y.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Vector3 GetPositionAt(int x, int y)
        {
            return MeshVertices[x * (Dimension + 1) + y].Position;
        }



        /// <summary>
        /// Creates a Terrain mesh Part
        /// </summary>
        /// <param name="Engine"></param>
        /// <param name="HeightMap"></param>
        /// <param name="Position"></param>
        /// <param name="CellSize"></param>
        public vxTerrainMeshPart(Texture2D HeightMap, int CellSize) :this(HeightMap.ToHeightMapDataArray(), CellSize)
        {

        }

        public vxTerrainMeshPart(float[,] HeightData, int CellSize) : base("", null)
        {
            Dimension = HeightData.GetLength(0)-1;

            this.HeightData = HeightData;

            //this.Position = Position;

            this.CellSize = CellSize;

            // Generate the Vertices Grid
            GenerateVertices();

            // Load the Data from the Height Data Array
            LoadFromHeightArray();

            CalculateNormals();

            TriangleCount = 2 * dimension * dimension;

            // Initialise the Dynamic Buffers
            RefreshMesh();
        }

        public void RefreshMesh()
        {
            SetData(MeshVertices, Indices);
        }

        public override void SetData(vxMeshVertex[] vertices, ushort[] indices)
        {
            VertexBuffer = new DynamicVertexBuffer(vxGraphics.GraphicsDevice, typeof(vxMeshVertex), vertices.Length, BufferUsage.None);
            VertexBuffer.SetData<vxMeshVertex>(vertices);

            IndexBuffer = new DynamicIndexBuffer(vxGraphics.GraphicsDevice, typeof(ushort), indices.Length, BufferUsage.None);
            IndexBuffer.SetData(indices);
		}


        /// <summary>
        /// Generates a Grid with the Dimension
        /// </summary>
        public void GenerateVertices()
        {
            MeshVertices = new vxMeshVertex[(dimension + 1) * (dimension + 1)];

            VerticesPoints = new Vector3[MeshVertices.Length];

            Indices = new ushort[dimension * dimension * 6];


            for (int i = 0; i < dimension + 1; i++)
            {
                for (int j = 0; j < dimension + 1; j++)
                {
                    vxMeshVertex vert = new vxMeshVertex();

                    // Set the Position based on the cell size
                    vert.Position = Position + new Vector3(i * _cellSize, 0, j * _cellSize);
                                        
                    MinPoint = Vector3.Min(MinPoint, vert.Position);
                    MaxPoint = Vector3.Max(MaxPoint, vert.Position);

                    // always have the normal up
                    vert.Normal = Vector3.Up;

                    // set the UV coordinates
                    vert.TextureCoordinate = new Vector2((float)i / dimension, (float)j / dimension);

                    vert.Tangent = Vector3.Right;
                    vert.BiNormal = Vector3.Cross(vert.Normal, vert.Tangent);

                    MeshVertices[i * (dimension + 1) + j] = vert;
                    VerticesPoints[i * (dimension + 1) + j] = vert.Position;
                }
            }

            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    Indices[6 * (i * dimension + j) + 2] = (ushort)(i * (dimension + 1) + j);
                    Indices[6 * (i * dimension + j) + 1] = (ushort)(i * (dimension + 1) + j + 1);
                    Indices[6 * (i * dimension + j)] = (ushort)((i + 1) * (dimension + 1) + j + 1);

                    Indices[6 * (i * dimension + j) + 5] = (ushort)(i * (dimension + 1) + j);
                    Indices[6 * (i * dimension + j) + 4] = (ushort)((i + 1) * (dimension + 1) + j + 1);
                    Indices[6 * (i * dimension + j) + 3] = (ushort)((i + 1) * (dimension + 1) + j);
                }

            }

            //base.MeshVertices = MeshVertices;
            //base.Indices = Indices;
        }

        void LoadFromHeightArray()
        {
            for (int i = 0; i < dimension+1; i++)
            {
                for (int j = 0; j < dimension+1; j++)
                {
                    vxMeshVertex vert = MeshVertices[i * (dimension + 1) + j];
                    
                    MeshVertices[i * (dimension + 1) + j].Position = new Vector3(vert.Position.X, vert.Position.Y + HeightData[i, j], vert.Position.Z);
                    VerticesPoints[i * (dimension + 1) + j] = MeshVertices[i * (dimension + 1) + j].Position;
                }
            }
        }

        public void CalculateNormals()
        {
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    int i1 = (i * (dimension + 1) + j);
                    int i2 = (i * (dimension + 1) + j + 1);
                    int i3 = ((i + 1) * (dimension + 1) + j + 1);


                    Vector3 v1 = MeshVertices[i1].Position;
                    Vector3 v2 = MeshVertices[i2].Position;
                    Vector3 v3 = MeshVertices[i3].Position;


                    // The Normal is the Cross Product of v2-v1 and v3-v1
                    Vector3 d1 = v1 - v3;
                    Vector3 d2 = v1 - v2;

                    Vector3 normal = Vector3.Cross(d2, d1);

                    normal.Normalize();

                    MeshVertices[i1].Normal = normal;
                    MeshVertices[i2].Normal = normal;
                    MeshVertices[i3].Normal = normal;

                }
            }
        }


        public virtual void UpdateDynamicVertexBuffer()
        {
            VertexBuffer.SetData<vxMeshVertex>(MeshVertices);
        }


        public virtual void UpdateDynamicIndexBuffer()
        {
            IndexBuffer.SetData(Indices);
        }
	}
}
