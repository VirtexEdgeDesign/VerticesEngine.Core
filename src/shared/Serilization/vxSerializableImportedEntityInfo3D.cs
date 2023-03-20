using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace VerticesEngine.Serilization
{
    public enum ImportedEntityType
    {
        Model = 0
    }

    /// <summary>
    /// Holds a reference about an internally held imported object
    /// </summary>
    public class vxSerializableImportedEntityInfo3D
    {
        [XmlAttribute("guid")]
        public string guid;

        /// <summary>
        /// The file name
        /// </summary>
        [XmlAttribute("fileName")]
        public string fileName;

        [XmlAttribute("importedEntityType")]
        public ImportedEntityType importedEntityType;

        /// <summary>
        /// The original path to the folder holding this object
        /// </summary>
        public string originalFilePath;

        public vxSerializableImportedEntityInfo3D()
        {
            guid = string.Empty;
            importedEntityType = ImportedEntityType.Model;
            this.originalFilePath = string.Empty;
            this.fileName = string.Empty;
        }

        public vxSerializableImportedEntityInfo3D(string guid, string originalFilePath, ImportedEntityType importedEntityType = ImportedEntityType.Model)
        {
            this.guid = guid;
            this.importedEntityType = importedEntityType;
            this.originalFilePath = originalFilePath;
            this.fileName = new FileInfo(originalFilePath).Name;
        }
    }
}
