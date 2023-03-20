using System;
using System.IO;
using System.Xml.Serialization;
using VerticesEngine;
using VerticesEngine.Graphics;
using VerticesEngine.Utilities;

namespace VerticesEngine.Serilization
{
    public class GameInfo
    {
		[XmlAttribute("name")]
		public string Name = "game_name";


		[XmlAttribute("version")]
		public string Version = "0.0.0.0";

        public GameInfo(){}
    }

    public class EngineInfo
	{
		[XmlAttribute("version")]
		public string Version = "0.0.0.0";

		public EngineInfo() { }
	}

    public class SandboxFileInfo
    {
        [XmlAttribute("title")]
        public string Title = "Title";


        [XmlAttribute("desc")]
        public string Description = "Description";

        public SandboxFileInfo() { }
    }


    public class WorkshopFileInfo
    {
        [XmlAttribute("id")]
        public string id = "";

        [XmlAttribute("author")]
        public string Author = "";

        public WorkshopFileInfo() { }
    }


    /// <summary>
    /// This class holds serializable file info for basic Vertices Engine Game Files. It holds information such as
    /// game title, Platform it was saved on, file io version etc.
    /// </summary>
    [XmlRoot]
    public class vxFileInfo
	{
		[XmlAttribute("version")]
		public int Version = 1;

		[XmlAttribute("PlatformSavedOn")]
        public vxGraphicalBackend PlatformSavedOn;

        public EngineInfo EngineInfo;

        public GameInfo GameInfo;

        public SandboxFileInfo SandboxFileInfo;

        public WorkshopFileInfo WorkshopFileInfo;

        public vxFileInfo()
        {
            GameInfo = new GameInfo();
            EngineInfo = new EngineInfo();
            SandboxFileInfo = new SandboxFileInfo();

            WorkshopFileInfo = new WorkshopFileInfo();
        }



        public void Initialise()
        {
			GameInfo.Name = vxEngine.Game.Name;
			GameInfo.Version = vxEngine.Game.Version.ToString();
			EngineInfo.Version = vxEngine.EngineVersion;
			PlatformSavedOn = vxEngine.GraphicalBackend;
        }


        public void Initialise(vxFileInfo info)
        {
			GameInfo.Name = info.GameInfo.Name;
			Version = info.Version;
            SandboxFileInfo = info.SandboxFileInfo;
            WorkshopFileInfo = info.WorkshopFileInfo;
        }

        public void Load(string path)
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(vxFileInfo));
                TextReader reader = new StreamReader(path + "/info.xml");
                object obj = deserializer.Deserialize(reader);
                vxFileInfo info = (vxFileInfo)obj;

                Initialise(info);

				reader.Close();
			}
			catch (Exception exception)
			{
				vxConsole.WriteLine("Error Loading File Info : " + exception.Message);
			}
        }

		public void Save(string path)
		{
			try
			{
				//Write The Sandbox File
				XmlSerializer serializer = new XmlSerializer(typeof(vxFileInfo));
				using (TextWriter writer = new StreamWriter(path + "/info.xml"))
				{
					serializer.Serialize(writer, this);
				}
			}
			catch (Exception exception)
			{
				vxConsole.WriteLine("Error Saving File Info : " + exception.Message);
			}
		}
    }
}
