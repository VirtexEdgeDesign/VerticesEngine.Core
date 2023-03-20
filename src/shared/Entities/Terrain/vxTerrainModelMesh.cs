using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

using VerticesEngine.Utilities;
using VerticesEngine.Graphics;
using VerticesEngine.EnvTerrain;

namespace VerticesEngine.Graphics
{
	/// <summary>
	/// The model mesh part which holds the Geometry data such as
	/// Vertices, Normal, UV Texture Coordinates, Binormal and Tangent data.
	/// </summary>
	public class vxTerrainModelMesh : vxModelMesh
    {
		vxTerrainChunk terrain;

        public vxTerrainModelMesh(vxTerrainChunk terrain)
		{
			this.terrain = terrain;
		}
    }
}
