using System.IO;
using System.Xml.Serialization;
using VerticesEngine.Entities;
using VerticesEngine.Serilization;
using VerticesEngine.Utilities;

namespace VerticesEngine
{
    public partial class vxGameplayScene3D
    {
        protected virtual vxImportedEntity3D GetDefaultImportedEntity()
        {
            return new vxImportedEntity3D(this);
        }


        /// <summary>
        /// Initialises the Save File. If the XML Save file uses a different type, then
        /// this can be overridden.
        /// </summary>
        /// <returns></returns>
        public override vxSerializableSceneBaseData InitSaveFile()
        {
            return new vxSerializableScene3DData();
        }


        /// <summary>
        /// Returns a Deserializes the File. If you want to use a different type to Serialise than the base 'vxSerializableScene3DData'
        /// then you must override this or it will throw an error.
        /// </summary>
        public override vxSerializableSceneBaseData DeserializeFile(string path)
        {
            vxSerializableScene3DData file;
            XmlSerializer deserializer = new XmlSerializer(typeof(vxSerializableScene3DData));
            TextReader reader = new StreamReader(path);
            file = (vxSerializableScene3DData)deserializer.Deserialize(reader);
            reader.Close();

            return file;
        }

        public override vxGameplaySceneBase OnNewSandbox()
        {
            return new vxGameplayScene3D(vxStartGameMode.Editor);
        }



        protected override vxSandboxFileLoadResult LoadXMLFile(string FilePath, int version)
        {
            switch (version)
            {
                case 1:
                    LoadXMLFileVersion1(FilePath);
                    break;
                //case 2:
                //LoadFileVersion2(FilePath);
                //break;
                default:
                    base.LoadXMLFile(FilePath, version);
                    break;
            }

            return new vxSandboxFileLoadResult();
        }

        /// <summary>
        /// Loads the Current File.
        /// </summary>
        private void LoadXMLFileVersion1(string FilePath)
        {
            //If it's not a new file, then open the specifeid file
            if (FilePath != "")
            {
                //Deserialize the input xml file
                string xmlFilePath = vxIO.PathToTempFolder + "/level.xml";
                base.SandBoxFile = DeserializeFile(xmlFilePath);

                // Set Sun Position
                SunEmitter.RotationX = SandBoxFile.Enviroment.SunRotations.X;
                SunEmitter.RotationZ = SandBoxFile.Enviroment.SunRotations.Y;

                // Set Fog
                //Renderer.IsFogEnabled = SandBoxFile.Enviroment.Fog.DoFog;
                //Renderer.FogNear = SandBoxFile.Enviroment.Fog.FogNear;
                //Renderer.FogFar = SandBoxFile.Enviroment.Fog.FogFar;
                //Renderer.FogColour = SandBoxFile.Enviroment.Fog.FogColour.Color;

            }
        }

        public override vxSaveBusyScreen GetAsyncSaveScreen()
        {
            return new vxSaveBusyScreen3D(this);
        }

    }
}