using Microsoft.Xna.Framework;
using System;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using VerticesEngine.EnvTerrain;
using VerticesEngine.Serilization;
using VerticesEngine.Utilities;

namespace VerticesEngine
{

    /// <summary>
    /// The save busy screen for 3D Scene.
    /// </summary>
    public class vxSaveBusyScreen3D : vxSaveBusyScreen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.vxSaveBusyScreen"/> class.
        /// </summary>
        public vxSaveBusyScreen3D(vxGameplayScene3D Scene)
            : base(Scene)
        {

        }


        //float percent = 0;

        public override string SaveFile(vxGameplaySceneBase sceneBase, bool IsWorkshop = false)
        {
            vxGameplayScene3D CurrentScene = (vxGameplayScene3D)sceneBase;

            // Now Proceed with Saving
            string path = vxIO.PathToSandbox;

            if (IsWorkshop)
            {
                path = System.IO.Path.Combine(vxIO.PathToTempFolder, new Random((int)DateTime.Now.Ticks).Next().ToString());
            }

            //First Check, if the Items Directory Doesn't Exist, Create It
            string ExtractionPath = vxIO.PathToTempFolder;

            if (CurrentScene.IsDumping)
                ExtractionPath = path + "/dump";

            // ensure the sandbox exists
            vxIO.EnsureDirExists(path);
            vxIO.EnsureDirExists(ExtractionPath);


            var SandBoxFile3D = CurrentScene.SandBoxFile;
            SandBoxFile3D.Clear();

            // first save out all imported entities
            foreach (var entity in CurrentScene.importedFiles)
            {
                if (entity.Key != string.Empty && entity.Value.ExternalFilePath != null)
                {
                    SandBoxFile3D.ImportedEntities.Add(new vxSerializableImportedEntityInfo3D(entity.Key, entity.Value.ExternalFilePath, ImportedEntityType.Model));

                    // first check whether the imported entity exists in the bundle, if it doesnt then let's copy it into the group
                    string importedFilePath = Path.Combine(vxIO.PathToImportFolder, entity.Key);
                    if (Directory.Exists(importedFilePath) == false)
                    {
                        // ensure the file still exists, and then copy it into the temp folder to be saved
                        if (File.Exists(entity.Value.ExternalFilePath))
                        {
                            var dir = Path.GetDirectoryName(entity.Value.ExternalFilePath);
                            vxIO.CopyDirectory(dir, importedFilePath, "");
                        }
                    }
                }
            }

            float per = 0;
            float tot = CurrentScene.Entities.Count;
            foreach (var entity in CurrentScene.Entities)
            {
                vxEntity3D part = (vxEntity3D)entity;
                // Prepare the entity for saving

                // we want to prep the entity for serialization first
                part.OnBeforeEntitySerialize();

                // then we want to serialise any and all vxSerialise tagged entities
                part.GetSerialisableSandboxData();

                //Don't Save Construction Geometry or items specifeid not to be saved
                Type partType = part.GetType();
                if (part.HasSandboxOption(SandboxOptions.Save))
                {
                    if (partType.IsSubclassOf(typeof(vxTerrainChunk)))
                    {
                        vxTerrainChunk terrain = (vxTerrainChunk)part;

                        SandBoxFile3D.Terrains.Add(terrain.TerrainData);
                    }
                    else
                    {
                        SandBoxFile3D.Entities.Add(new vxSerializableEntityState3D(part.Id,
                            part.ToString(),
                            part.Transform.Matrix4x4Transform,
                            part.UserDefinedData,
                            part.SandboxData,
                            part.UserDefinedData01,
                            part.UserDefinedData02,
                            part.UserDefinedData03,
                            part.UserDefinedData04,
                            part.UserDefinedData05));
                    }
                }

                System.Threading.Thread.Sleep(3);
                per++;
                ReportProgress((int)(per * 70 / tot));
            }



            // Increment File Type
            SandBoxFile3D.FileReversion++;

            // Set Sun Position
            SandBoxFile3D.Enviroment.SunRotations = new Vector2(CurrentScene.SunEmitter.RotationX, CurrentScene.SunEmitter.RotationZ);


            // Set Fog
            //SandBoxFile3D.Enviroment.Fog.DoFog = CurrentScene.Renderer.IsFogEnabled;
            //SandBoxFile3D.Enviroment.Fog.FogNear = CurrentScene.Renderer.FogNear;
            //SandBoxFile3D.Enviroment.Fog.FogFar = CurrentScene.Renderer.FogFar;
            //SandBoxFile3D.Enviroment.Fog.FogColour.Color = CurrentScene.Renderer.FogColour;
            //SaveFileAsyncWriter.ReportProgress(80);
            //Write The Sandbox File
            XmlSerializer serializer = new XmlSerializer(SandBoxFile3D.GetType());
            using (TextWriter writer = new StreamWriter(ExtractionPath + "/level.xml"))
            {
                serializer.Serialize(writer, SandBoxFile3D);
            }
            ReportProgress(90);


            //Lastly, save the file info xml file.
            var fileInfo = CurrentScene.GetFileInfo();
            fileInfo.Save(ExtractionPath);

            string compFile = path + "/" + CurrentScene.FileName + ".sbx";

            // Get Compressed File Name
            if (CurrentScene.IsDumping == false)
            {
                //string compFile = path + "/" + CurrentScene.FileName + ".sbx";

                if (File.Exists(compFile))
                    File.Delete(compFile);

                vxIO.CompressDirectory(ExtractionPath, compFile, null);
            }
            CurrentScene.IsDumping = false;
            ReportProgress(100);
            return compFile;
        }

        protected virtual void ReportProgress(int perc)
        {
            if(SaveFileAsyncWriter != null && SaveFileAsyncWriter.IsBusy)
                SaveFileAsyncWriter.ReportProgress(perc);
        }

        public override void OnAsyncWriteSaveFile(object sender, DoWorkEventArgs e)
        {
            SaveFile((vxGameplayScene3D)e.Argument);
            e.Result = true;
        }
    }
}
