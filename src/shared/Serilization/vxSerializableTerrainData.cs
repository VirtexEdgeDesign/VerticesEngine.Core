using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace VerticesEngine.Serilization
{
    /// <summary>
    /// This holds the Serializable data of a Height Map Terrain.
    /// </summary>
    public class vxSerializableTerrainData : vxSerializableEntityState3D
    {
        [XmlAttribute("dim")]
        public int Dimension = 128;

        [XmlAttribute("cellsz")]
        public float CellSize = 3;

        [XmlElement("hmd")]
        public List<vxSerializableTerrainVertex> HeightData;

        public vxSerializableTerrainData() : base()
        {
            HeightData = new List<vxSerializableTerrainVertex>();
        }

        public vxSerializableTerrainData(
            string ID,
            string type,
            Matrix orientation,
            int Dimension,
            float CellSize) :
            base(ID, type, orientation)
        {
            this.Dimension = Dimension;
            this.CellSize = CellSize;

            // Create a new Height Data List.
            HeightData = new List<vxSerializableTerrainVertex>();
            
            // Now loop through the 2D array and add in the new data.
            //for (int i = 0; i < HghtData.GetLength(0); i++)
            //{
            //    for (int j = 0; j < HghtData.GetLength(1); j++)
            //        HeightData.Add(new vxSerializableTerrainVertex(i, HghtData[i, j], j, 1));
            //}
        }
    }
}
