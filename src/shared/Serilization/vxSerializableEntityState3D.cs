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
    public class vxSerializableEntityState3D
    {
        [XmlAttribute("id")]
        public string id;

        [XmlAttribute("v")]
        public string v;

        [XmlAttribute("type")]
        public string Type;

        [XmlElement("world")]
        public Matrix Orientation;

        [XmlElement("usrData")]
        public string UserDefinedData;

        [XmlElement("sandboxData")]
        public string SandboxData;

        [XmlElement("usrData1")]
        public string UserDefinedData01;

        [XmlElement("usrData2")]
        public string UserDefinedData02;

        [XmlElement("usrData3")]
        public string UserDefinedData03;

        [XmlElement("usrData4")]
        public string UserDefinedData04;

        [XmlElement("usrData5")]
        public string UserDefinedData05;

        public vxSerializableEntityState3D()
        {
            id = string.Empty;
            Type = "null";
            Orientation = Matrix.Identity;
            //SandboxItemType = vxSandboxItemType.Entity;
        }

        public vxSerializableEntityState3D(
            string ID,
            string type,
            Matrix orientation)
        {
            id = ID;
            Type = type;
            Orientation = orientation;
            //FilePath = "NA";
            //SandboxItemType = vxSandboxItemType.Entity;
        }

        public vxSerializableEntityState3D(
            string ID,
            string type,
            Matrix orientation,
            string userDefinedData,
            string sandboxData,
            string userDefinedData01,
            string userDefinedData02,
            string userDefinedData03,
            string userDefinedData04,
            string userDefinedData05)
        {
            id = ID;
            Type = type;
            Orientation = orientation;
            //FilePath = "NA";
            //SandboxItemType = vxSandboxItemType.Entity;
            UserDefinedData = userDefinedData;
            SandboxData = sandboxData;
            UserDefinedData01 = userDefinedData01;
            UserDefinedData02 = userDefinedData02;
            UserDefinedData03 = userDefinedData03;
            UserDefinedData04 = userDefinedData04;
            UserDefinedData05 = userDefinedData05;
        }
    }
}
