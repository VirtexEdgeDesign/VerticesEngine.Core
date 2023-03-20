using Microsoft.Xna.Framework;
using System;
using System.ComponentModel;
using VerticesEngine.Graphics;
using VerticesEngine.UI.MessageBoxs;

namespace VerticesEngine
{
    /// <summary>
    /// The save busy screen.
    /// </summary>
    public class vxSaveBusyScreen : vxMessageBox
    {

        vxGameplaySceneBase Scene;

        public BackgroundWorker SaveFileAsyncWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.vxSaveBusyScreen"/> class.
        /// </summary>
        public vxSaveBusyScreen(vxGameplaySceneBase Scene)
            : base("Saving", "Saving File", UI.vxEnumButtonTypes.None)
        {
            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);

            this.Scene = Scene;


            SaveFileAsyncWriter = new BackgroundWorker();
            SaveFileAsyncWriter.WorkerReportsProgress = true;
            SaveFileAsyncWriter.WorkerSupportsCancellation = true;
            SaveFileAsyncWriter.DoWork += OnAsyncWriteSaveFile;
            SaveFileAsyncWriter.ProgressChanged += OnAsyncSaveFileWriter_ProgressChanged;
            SaveFileAsyncWriter.RunWorkerCompleted += OnAsyncSaveFileWriter_RunWorkerCompleted;
        }

        int Inc = 0;

        protected internal override void Update()
        {
            if (Inc == 25)
            {
                vxConsole.WriteIODebug("============================================");
                vxConsole.WriteIODebug("Saving File : '" + Scene.FileName + "'");
                StartSave();
            }
            string SavingText = "Saving File ";

            Inc++;

            SavingText += new string('.', (int)(Inc / 10) % 5);

            Message = SavingText;
            //Bounds.Width = 300;
            base.Update();

        }

        public void StartSave()
        {
            Scene.SaveSupportFiles();
            SaveFileAsyncWriter.RunWorkerAsync(Scene);
        }

        float percent = 0;
        public virtual void OnAsyncSaveFileWriter_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            percent = e.ProgressPercentage / 100.0f;
            Console.Write(".");
        }



        /// <summary>
        /// The Async File Save Writer, override this to provide your own custom saving.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public virtual void OnAsyncWriteSaveFile(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

        }


        /// <summary>
        /// Saves the file and returns the file path.
        /// </summary>
        /// <returns>The file.</returns>
        /// <param name="CurrentScene">Current scene.</param>
        /// <param name="IsWorkshop">If set to <c>true</c> is workshop.</param>
        public virtual string SaveFile(vxGameplaySceneBase CurrentScene, bool IsWorkshop = false)
        {
            return string.Empty;
        }


        public override void Draw()
        {
            base.Draw();

            vxGraphics.SpriteBatch.Begin("Save Screen");

            vxGraphics.SpriteBatch.Draw(vxInternalAssets.Textures.Blank,
                                    new Rectangle(0, 0, (int)(vxGraphics.GraphicsDevice.Viewport.Width * percent), 2),
                                    Color.DeepSkyBlue);

            vxGraphics.SpriteBatch.End();
        }

        protected virtual void OnFinished(bool success)
        {

        }

        void OnAsyncSaveFileWriter_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bool success = false;
            if (e.Error != null)
            {
                vxConsole.WriteLine("ERROR: " + e.Error.Message);
                vxConsole.WriteLine("StackTrace: " + e.Error.StackTrace);
                //ButtonImage = DefaultTexture;
            }
            else if (e.Result != null)
            {
                //Console.WriteLine();
                if (e.Result is bool)
                    success = (bool)e.Result;
            }
            OnFinished(success);
            vxConsole.WriteIODebug("Finished Save! Success: " + success);
            vxConsole.WriteIODebug("============================================");
            ExitScreen();
            Scene.IsDumping = false;
        }
    }
}
