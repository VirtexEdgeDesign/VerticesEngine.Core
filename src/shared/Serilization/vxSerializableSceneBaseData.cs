using System.Xml.Serialization;

namespace VerticesEngine.Serilization
{

    /// <summary>
    /// This holds the Serializable data for a vxScene3D including all Entities, Terrains, as well as
    /// Level and Enviroment Data.
    /// </summary>
    public class vxSerializableSceneBaseData
    {
        [XmlAttribute("filerev")]
        public int FileReversion = 1;


        // Level Title
        [XmlElement("title")]
        public string LevelTitle = "Enter A Title";

        // Level Description
        [XmlElement("desc")]
        public string LevelDescription="Enter A Description";

        [XmlElement("guid")]
        public string guid;

        [XmlElement("workshopId")]
        public string workshopId;


        public vxSerializableSceneBaseData()
        {
           
        }

        // Clears the File to take a new file in.
        public virtual void Clear()
        {
            
        }
    }
}
