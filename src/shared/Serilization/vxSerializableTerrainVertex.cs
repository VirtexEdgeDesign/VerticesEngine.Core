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
    /// This holds the Serializable data for a singular Hieght Map Vertex.
    /// </summary>
    public class vxSerializableTerrainVertex
    {
        [XmlAttribute("x")]
        public int X;

        [XmlAttribute("y")]
        public float Y;

        [XmlAttribute("z")]
        public int Z;


        [XmlAttribute("txtW")]
        public float TextureWeight = 1;


        public vxSerializableTerrainVertex()
        {

        }


        public vxSerializableTerrainVertex(Vector3 position, float TextureWeight)
        {
            this.X = (int)position.X;
            this.Y = position.Y;
            this.Z = (int)position.Z;
            this.TextureWeight = TextureWeight;
        }

        public vxSerializableTerrainVertex(int X, float Y, int Z, float TextureWeight)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.TextureWeight = TextureWeight;
        }
    }
}
