#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using VerticesEngine.Audio;
using VerticesEngine;
using VerticesEngine.Commands;
using VerticesEngine.Diagnostics;

using VerticesEngine.Particles;
using VerticesEngine.Input;
using VerticesEngine.Input.Events;
using VerticesEngine.Profile;
using VerticesEngine.Screens.Async;
using VerticesEngine.Serilization;
using VerticesEngine.UI;
using VerticesEngine.UI.Dialogs;
using VerticesEngine.UI.Events;
using VerticesEngine.UI.Menus;
using VerticesEngine.UI.MessageBoxs;
using VerticesEngine.Utilities;
using VerticesEngine.Graphics;
using System.Collections;
using System.Diagnostics;
using VerticesEngine.ContentManagement;
using VerticesEngine.Entities;
using System.Linq;
using System.Text.RegularExpressions;
using VerticesEngine.UI.Controls;


#endregion

namespace VerticesEngine
{
    public struct ImportedFileInfo
    {
        public string guid;
        public string ExternalFilePath;
        public Texture2D Icon;
        public vxMesh Model;
    }

    public partial class vxGameplayScene3D
    {
        private Texture2D LoadImportedFileThumbnail(string guid)
        {
            var icon = vxInternalAssets.Textures.DefaultDiffuse;

            try
            {
                var iconPath = Path.Combine(vxIO.PathToImportFolder, guid + "_icon.png");
                if (File.Exists(iconPath))
                {
                    icon = vxIO.LoadImage(iconPath, false);
                    if (icon == null)
                    {
                        vxConsole.WriteError($"Missing icon for guid: {guid}");
                        icon = vxInternalAssets.Textures.DefaultDiffuse;
                    }
                }
            }
            catch { }

            return icon;
        }

        /// <summary>
        /// Imports a model into the game, registers it and sets up it's guid
        /// </summary>
        /// <param name="isEmbedded"></param>
        protected virtual void RegisterImportedModel(string guid, string filepath, vxMesh model)
        {
            ImportedFileInfo newFile;
            newFile.guid = guid;
            newFile.Model = model;
            newFile.ExternalFilePath = filepath;
            newFile.Icon = LoadImportedFileThumbnail(guid);

            // this is a new file, let's add it to the 
            importedFiles.Add(guid, newFile);
            importedGuidLookup.Add(filepath, guid);

            AddNewImportedItemSandboxButton(guid);
        }

        protected virtual void AddNewImportedItemSandboxButton(string guid)
        {
            // now add it to the tab page if it exists
            if (m_importedEntitiesScrollPage != null)
            {
                var import = importedFiles[guid];
                var newBtn = new vxSandboxImportedItemButton(import.Icon, new FileInfo(import.ExternalFilePath).Name, guid,
                    Vector2.Zero, SandboxItemButtonSize, SandboxItemButtonSize);
                
                newBtn.Clicked += delegate{
                    OnNewItemAdded(guid);
                };

                m_importedEntitiesScrollPage.AddItem(newBtn);
            }
        }

        internal void DeleteImportedEntity(string guid)
        {
            var itemPath = Path.Combine(vxIO.PathToImportFolder, guid);
            if(Directory.Exists(itemPath))
            {
                Directory.Delete(itemPath,true);

                if (File.Exists(itemPath + "_icon.png"))
                    File.Delete(itemPath + "_icon.png");

                var extPath = importedFiles[guid].ExternalFilePath;

                if(importedGuidLookup.ContainsKey(extPath))
                    importedGuidLookup.Remove(extPath);

                if (importedFiles.ContainsKey(guid))
                    importedFiles.Remove(guid);

                // now loop through all entities that match this guid and delete them
                for (int e = 0; e < Entities.Count; e++)
                {
                    if (e >= 0 && e < Entities.Count)
                    {
                        var entity = Entities[e];
                        if (entity is vxImportedEntity3D)
                        {
                            if (entity.CastAs<vxImportedEntity3D>().ImportedModelGuid == guid)
                            {
                                entity.CastAs<vxImportedEntity3D>().Dispose();
                                e--;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Refreshes an imported file with the original path for the sepcified guid
        /// </summary>
        /// <param name="guid"></param>
        public void RefreshImportedFile(string guid, string externalPath)
        {
            if (importedGuidLookup.ContainsKey(externalPath))
            {
                if (File.Exists(externalPath) == false)
                {
                    vxMessageBox.Show("External File missing", "External File missing \n" + externalPath);
                    vxConsole.WriteError("External File missing \n" + externalPath);
                    return;
                }
                // get the file info for this guid
                var importedFileInfo = importedFiles[guid];

                FileInfo fileToRefresh = new FileInfo(externalPath);

                // overwright the previous folder
                vxIO.CopyDirectory(Path.GetDirectoryName(externalPath), Path.Combine(vxIO.PathToImportFolder, guid), "");

                var importResult = vxMeshHelper.Import(Path.Combine(vxIO.PathToImportFolder, guid, fileToRefresh.Name));
                if (importResult.ImportResultStatus == vxImportResultStatus.Success)
                {
                    // set the model
                    importedFileInfo.Model = importResult.ImportedModel;

                    // regenerate the icon
                    importedFileInfo.Icon = this.GenerateImportedItemIcon(guid);
                    importedFiles[guid] = importedFileInfo;
                }

                // now loop through all imported entities and refresh their models
                foreach(var entity in Entities)
                {
                    if(entity is vxImportedEntity3D)
                    {
                        if(entity.CastAs<vxImportedEntity3D>().ImportedModelGuid == guid)
                        {
                            entity.CastAs<vxImportedEntity3D>().InitImportedEntity(guid);
                        }
                    }
                }

            }
            else
            {
                vxMessageBox.Show("Error Refreshing Imported File", "External File guid mismatch\n" + guid);
                vxConsole.WriteError("Guid is not registered " + guid);

            }
        }

        /// <summary>
        /// Imports the file toolbar item clicked.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public virtual void ImportFileToolbarItem_Clicked(object sender, vxUIControlClickEventArgs e)
        {
            var FileExplorerDialog = new vxFileExplorerDialog(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

            vxSceneManager.AddScene(FileExplorerDialog, ControllingPlayer);

            FileExplorerDialog.Accepted += delegate
            {

                FileInfo importedFile = new FileInfo(FileExplorerDialog.SelectedItem);

                var result = vxMeshHelper.Import(FileExplorerDialog.SelectedItem);


                // imported files will have 3 identifiers stored.
                // guid - this is the main id for an imported mesh
                // original external path - this is the main external location of a mesh so that it can be updated
                // imported path - this is the local imported path of where this file is stored when it's brought into the snadbox file

                if (result.ImportResultStatus == vxImportResultStatus.Success)
                {
                    string importGuid = Guid.NewGuid().ToString();

                    // we already have this path, so let's replace the currently loaded file
                    if (importedGuidLookup.ContainsKey(importedFile.FullName))
                    {
                        // get the guid for this model path
                        importGuid = importedGuidLookup[importedFile.FullName];

                        // get the file info for this guid
                        var importedFileInfo = importedFiles[importGuid];

                        // set the model
                        importedFileInfo.Model = result.ImportedModel;

                        // regenerate the icon
                        importedFileInfo.Icon = this.GenerateImportedItemIcon(importGuid);
                        importedFiles[importGuid] = importedFileInfo;
                    }
                    else
                    {
                        RegisterImportedModel(importGuid, importedFile.FullName, result.ImportedModel);

                        var importedFileInfo = importedFiles[importGuid];

                        vxIO.CopyDirectory(importedFile.DirectoryName, Path.Combine(vxIO.PathToImportFolder, importGuid), "");

                        importedFileInfo.Icon = this.GenerateImportedItemIcon(importGuid);
                        importedFiles[importGuid] = importedFileInfo;
                    }

                    var entity = new vxImportedEntity3D(this);

                    entity.InitImportedEntity(importGuid);
                }
                else
                {
                    string errorList = "";
                    foreach (var error in result.Errors)
                        errorList += "\n- " + error;

                    vxMessageBox.Show("Error Importing File", $"Error Importing File '{FileExplorerDialog.SelectedItem}'\n{errorList}");
                }
            };
        }


        // Process all files in the directory passed in, recurse on any directories
        // that are found, and process the files they contain.
        public static void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        // Insert logic for processing found files here.
        public static void ProcessFile(string path)
        {
            Console.WriteLine("Processed file '{0}'.", path);
        }

        /// <summary>
        /// Imported models which are 
        /// </summary>
        internal Dictionary<string, ImportedFileInfo> importedFiles = new Dictionary<string, ImportedFileInfo>();

        // file 
        internal Dictionary<string, string> importedGuidLookup = new Dictionary<string, string>();

        //internal Dictionary<string, vxImportedEntity3D> importedEntities = new Dictionary<string, vxImportedEntity3D>();

    }
}
