using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using VerticesEngine;
using VerticesEngine.Graphics;
using VerticesEngine.Profile;
using VerticesEngine.UI;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Dialogs;
using VerticesEngine.UI.Events;
using VerticesEngine.UI.MessageBoxs;
using VerticesEngine.UI.Themes;
using VerticesEngine.Utilities;
using VerticesEngine.Workshop.Events;

namespace VerticesEngine.Workshop
{
    public struct WorkshopItemUploadResult
    {
        /// <summary>
        /// Was the upload successfull
        /// </summary>
        public bool IsSucessfull;

        /// <summary>
        /// Info around the upload
        /// </summary>
        public string Info;
        public WorkshopItemUploadResult(bool IsSucessfull, string Info)
        {
            this.IsSucessfull = IsSucessfull;
            this.Info = Info;
        }
    }

    /// <summary>
    /// Upload worker message dialog
    /// </summary>
    public class vxWorkshopUploadWorker : vxMessageBox
    {
        /// <summary>
        /// Threaded background worker
        /// </summary>
        private BackgroundWorker filesaveWorker;

        /// <summary>
        /// The current level
        /// </summary>
        private vxGameplaySceneBase level;

        private string msg = "Uploading";

        private bool isFinishedUploading = false;


        public vxWorkshopUploadWorker(vxGameplaySceneBase level) :
        base("Publishing", "Publishing", VerticesEngine.UI.vxEnumButtonTypes.OkCancel)
        {
            this.level = level;

            // clear the temp folder
            vxIO.ClearTempDirectory();
            
            filesaveWorker = new BackgroundWorker();
            filesaveWorker.WorkerReportsProgress = true;
            filesaveWorker.WorkerSupportsCancellation = true;

            filesaveWorker.DoWork += OnDoWorkEventHandler;
            filesaveWorker.RunWorkerCompleted += OnRunWorkerCompleted;

            msg = "Preparing '" + level.Title + "'";

        }

        public override void LoadContent()
        {
            base.LoadContent();

            OKButton.Text = "Cancel";

            CancelButton.IsEnabled = false;
            CancelButton.Text = "View Level";

            vxWorkshop.Instance.ItemPublished += OnItemPublished;
        }

        public override void UnloadContent()
        {
            level = null;
            base.UnloadContent();
        }

        int updateIncrement = 0;
        protected internal override void Update()
        {
            if (updateIncrement == 20)
            {
                msg = "Saving File...";
            }
            else if (updateIncrement == 25)
            {
                vxConsole.WriteIODebug("============================================");
                vxConsole.WriteIODebug("UploadingFile(" + level.FileName + ")");


                // Now clear it just incase it exists but has files in it already
                //vxIO.ClearTempDirectory();

                level.SaveSupportFiles();
                if (File.Exists(Path.Combine(vxIO.PathToTempFolder, "preview.jpg")) == false)
                {
                    string imgPath = Path.Combine(vxIO.PathToTempFolder, "img.png");

                    var previewImg = vxIO.LoadImage(imgPath, false);
                    var h = 720;
                    var w = previewImg.Width * h / previewImg.Height;
                    previewImg = previewImg.Resize(w, h);
                    previewImg.SaveToDisk(Path.Combine(vxIO.PathToTempFolder, "preview.jpg"), vxExtensions.ImageType.JPG);
                }
                filesaveWorker.RunWorkerAsync(level);
            }


            if (isFinishedUploading == false)
            {
                updateIncrement++;

                Message = msg + new string('.', (int)(updateIncrement / 30) % 3) + "\n" + new string(' ', 40);
            }

            base.Update();

        }



        public override void OnOKButtonClicked(object sender, vxUIControlClickEventArgs e)
        {
            if (filesaveWorker.IsBusy)
                filesaveWorker.CancelAsync();
            else
                ExitScreen();
        }

        public override void OnCancelButtonClicked(object sender, vxUIControlClickEventArgs e)
        {
            if (isFinishedUploading)
            {
                vxPlatform.Player.OpenURL(vxWorkshop.Instance.GetWorkshopItemURL(level.WorkshopID));
            }
        }


        private void OnDoWorkEventHandler(object sender, DoWorkEventArgs e)
        {
            try
            {
                vxConsole.WriteLine("Saving Workshop file... ");
                var level = (vxGameplaySceneBase)e.Argument;

                // Save the file to a temp directory
                string imgPath = Path.Combine(vxIO.PathToTempFolder, "preview.jpg");
                
                // load img and then resize it, then save it
                var saveScreen = level.GetAsyncSaveScreen();
                var filePath = saveScreen.SaveFile(level, true);
                var uploadPath = System.IO.Path.GetDirectoryName(filePath);

                vxConsole.WriteLine("File Saved To: " + filePath);

                vxConsole.WriteLine("Uploading files... ");
                string rev = level.SandBoxFile.FileReversion.ToString();

                vxWorkshop.Instance.Publish(
                    level.Title, 
                    level.Description, 
                    imgPath,
                    filePath,
                    new string[] { "tracks","sandbox" },
                    level.SandBoxFile.workshopId, 
                    "initial upload");

                level.GetFileInfo();

                e.Result = new WorkshopItemUploadResult(true, "Publishing Started!");
            }
            catch (Exception ex)
            {
                e.Result = new WorkshopItemUploadResult(false, ex.Message);
            }
        }


        private void OnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // get result
            if (e.Result is WorkshopItemUploadResult)
            {
                var results = (WorkshopItemUploadResult)e.Result;

                if (results.IsSucessfull)
                {
                    // Set the text based on if we're publishing or updating
                    if (level.WorkshopID == string.Empty || level.WorkshopID == "0")
                        msg = "Publishing '" + level.Title + "'...";
                    else
                        msg = "Updating '" + level.Title + "'...";
                }
                else
                {
                    Message = "There was a problem uploading '" + level.Title + "'!\n";
                    string errorText = "Error Details:\n" + results.Info;
                    Message += "\n" + ArtProvider.Font.WrapString(errorText,
                                                                       (int)ArtProvider.Font.MeasureString(Message).X);
                    isFinishedUploading = true;
                }
            }
        }

        

        private void OnItemPublished(object sender, vxWorkshopItemPublishedEventArgs e)
        {
            if (e.IsUploadSuccessful)
            {
                // Now capture the returned app id
                level.SandBoxFile.workshopId = e.ItemID;
                level.WorkshopID = e.ItemID;

                // and finally re-save the file so that the Workshop ID is captured
                level.SaveFile(false);

                OKButton.Text = "OK";
                isFinishedUploading = true;
                CancelButton.IsEnabled = true;

                Message = "'" + level.Title + "' has been Published to the\nWorkshop!\n\n";
            }
            else
            {
                isFinishedUploading = true;

                Message = "There was a problem uploading '" + level.Title + "'!\n";
                string errorText = "Error Details:\n" + e.Info;
                Message += "\n" + ArtProvider.Font.WrapString(errorText,
                                                                   (int)ArtProvider.Font.MeasureString(Message).X);
            }

            vxWorkshop.Instance.ItemPublished -= OnItemPublished;
        }
    }
}
//#endif