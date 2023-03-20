using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace VerticesEngine.Graphics
{
	/// <summary>
	/// Imports an *.obj file, parsing out the Vertices, Texture UV Coordinates as well as Vertice Normals
	/// </summary>
	public class vxModelVoxel
    {
		/// <summary>
		/// The Model Texture
		/// </summary>
		/// <value>The texture.</value>
		public Texture2D Texture
		{
			get{ return _texture; }
			set { _texture = value; }
		}
		Texture2D _texture;

		/// <summary>
		/// Gets or sets the collection of Vertices.
		/// </summary>
		/// <value>The vertices.</value>
		public List<Vector3> Vertices;


		/// <summary>
		/// Gets or sets the collection of Normals.
		/// </summary>
		/// <value>The vertices.</value>
		public List<Vector3> Normals;

		/// <summary>
		/// Gets or sets the collection of texture UV coordinates.
		/// </summary>
		/// <value>The texture UV coordinate.</value>
		public List<Vector2> TextureUVCoordinate;

		/// <summary>
		/// Gets or sets the collection of Mesh Vertices.
		/// </summary>
		/// <value>The collection of Mesh Verteices which hold all of the arraies of 
		/// Vertex, Normal, UV Coordinate Data to be passed as one large pool of data
		/// to the graphics card to limit draw calls as well as allowing for a psuedo
		/// Instance Mesh look on Multiple Platforms.</value>
		//public List<vxMeshVertex> MeshVertices;


        public List<VertexPositionNormalTexture> MeshVertices;
        //public VertexPositionTexture[] verts;


        /// <summary>
        /// Load's a *.OBJ File
        /// </summary>
        /// <param name="path">Path.</param>
        public vxModelVoxel(string path)
		{
            MeshVertices = new List<VertexPositionNormalTexture>();
			Vertices = new List<Vector3> ();
			Normals = new List<Vector3> ();
			TextureUVCoordinate = new List<Vector2> ();

			//Create a Stream Reader
			StreamReader reader = new StreamReader (path);

			//Read in each line
			string line = reader.ReadLine ();

			//Loop for each line in the file
			while (line != null) {

				//Console.WriteLine (line);

				//Split the string based off of spaces
				string[] chunks = line.Split (' ');

				//Now Process based off what the first chunk is
				if (chunks.Length > 0) {

					switch (chunks [0]) {
					case "#":
						//Commen Line, do nothing
						break;

					//Add Vertice
					case "v":
						//Ensure there are enough chunks to make up a 3D Vector.
						if (chunks.Length > 3) {
							Vertices.Add( new Vector3 (
								float.Parse(chunks[1]),
								float.Parse(chunks[2]),
								float.Parse(chunks[3])));
						}
						break;

						//Add UV Texture Coordinate
					case "vt":
						//Ensure there are enough chunks to make up a 2D Texture Coordinate.
						if (chunks.Length > 2) {
							TextureUVCoordinate.Add( new Vector2 (
								float.Parse(chunks[1]),
								float.Parse(chunks[2])));
						}
						break;

						//Add Vertice Normal
					case "vn":
						//Ensure there are enough chunks to make up a 3D Normal Vector.
						if (chunks.Length > 3) {
							Normals.Add( new Vector3 (
								float.Parse(chunks[1]),
								float.Parse(chunks[2]),
								float.Parse(chunks[3])));
						}
						break;

						//Add Triangular Face
					case "f":
						//Ensure there are enough chunks to make up a Triangular Face.
						if (chunks.Length > 3) {

							//Loop through all Vertices for the face
							for (int i = 1; i < 4; i++) {

								//Now spilt the individual vertex info 
								string[] vertexChunk = chunks[i].Split('/');

                                    //vxMeshVertex vert;

                                    //vert.Position = Vertices[int.Parse(vertexChunk[1])];
                                    //vert.TextureCoordinate = TextureUVCoordinate[int.Parse(vertexChunk[2])];
                                    //vert.Normal = Normals[int.Parse(vertexChunk[3])];

                                    //Console.WriteLine("VERT\tPOS: {0}\tUV: {1}", Vertices[int.Parse(vertexChunk[0])-1], TextureUVCoordinate[int.Parse(vertexChunk[1])-1]);
                                    
                                MeshVertices.Add(new VertexPositionNormalTexture(Vertices[int.Parse(vertexChunk[0]) - 1], Normals[int.Parse(vertexChunk[2]) - 1], TextureUVCoordinate[int.Parse(vertexChunk[1]) - 1]));

                                }

						}
						break;
					default:
						break;
					}
				}

				line = reader.ReadLine ();
			}
		}
	}
}

