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
    /// This holds the Serializable data of a vxEntity3D to be used when saving a file.
    /// </summary>
    public class vxSerializableEntityState2D
    {
        [XmlAttribute("id")]
        public string id = "";

        [XmlAttribute("type")]
        public string Type;

        [XmlElement("pos")]
        public Vector2 Position;

        [XmlElement("rot")]
        public float Rotation;

        [XmlElement("dir")]
        public ItemDirection ItemDirection;

        [XmlElement("data1")]
        public string UserDefinedData01;

        [XmlElement("data2")]
        public string UserDefinedData02;

        [XmlElement("data3")]
        public string UserDefinedData03;

        [XmlElement("data4")]
        public string UserDefinedData04;

        [XmlElement("data5")]
        public string UserDefinedData05;

        public vxSerializableEntityState2D()
        {
            id = "";
            Type = "null";

            //SandboxItemType = vxSandboxItemType.Entity;
        }

        public vxSerializableEntityState2D(
            string ID,
            string type,
            Vector2 pos,
            float rot)
        {
            id = ID;
            Type = type;
            Position = pos;
            Rotation = rot;
            ItemDirection = ItemDirection.Left;
            //FilePath = "NA";
            //SandboxItemType = vxSandboxItemType.Entity;
        }

        public vxSerializableEntityState2D(
            string ID,
            string type,
            Vector2 pos,
            float rot,
            string userDefinedData01,
            string userDefinedData02,
            string userDefinedData03,
            string userDefinedData04,
            string userDefinedData05)
        {
            id = ID;
            Type = type;
            Position = pos;
            Rotation = rot;
            ItemDirection = ItemDirection.Left;
           // Orientation = orientation;
            //FilePath = "NA";
            //SandboxItemType = vxSandboxItemType.Entity;
            UserDefinedData01 = userDefinedData01;
            UserDefinedData02 = userDefinedData02;
            UserDefinedData03 = userDefinedData03;
            UserDefinedData04 = userDefinedData04;
            UserDefinedData05 = userDefinedData05;
        }
    }
}
