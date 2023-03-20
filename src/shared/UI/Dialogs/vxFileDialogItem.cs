using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Utilities;
using VerticesEngine;
using VerticesEngine.UI.Events;
using VerticesEngine.UI.Themes;
using System.IO;
using System.ComponentModel;
using System.Xml.Serialization;
using VerticesEngine.Serilization;
using Microsoft.Xna.Framework.Content;
using VerticesEngine.Graphics;

namespace VerticesEngine.UI.Dialogs
{
	/// <summary>
	/// File Chooser Dialor Item.
	/// </summary>
	public class vxFileDialogItem : vxScrollPanelItem
	{        
        public string Description = "";

        /// <summary>
        /// The name of the file.
        /// </summary>
        public string FileName
        {
            get { return FileInfo.Name; }
        }

        /// <summary>
        /// The file path.
        /// </summary>
        public string FilePath
        {
            get { return FileInfo.FullName; }
        }

        public string LastUsed
        {
            get { return FileInfo.LastAccessTime.ToString(); }
        }

        /// <summary>
        /// The file path.
        /// </summary>
        public string FileSize
        {
            get
            {
                float size = FileInfo.Length / 1024.0f / 1024.0f;

                return Math.Round((float)size, 3).ToString() + " MB";
            }
        }

        public vxFileInfo MetaData
        {
            get { return _metaData; }
        }
        vxFileInfo _metaData = new vxFileInfo();

        /// <summary>
        /// Gets the file version.
        /// </summary>
        /// <value>The file version.</value>
        public string FileVersion
        {
            get { return _fileVersion; }
        }
        string _fileVersion = "";

		/// <summary>
		/// The art provider.
		/// </summary>
		public new vxFileDialoglItemArtProvider ArtProvider;


        public FileInfo FileInfo;


        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Dialogs.vxFileDialogItem"/> class.
        /// </summary>
        /// <param name="FileInfo">File info.</param>
		public vxFileDialogItem(FileInfo FileInfo) : base(FileInfo.Name, Vector2.Zero, null, 0)
		{
			Padding = new Vector2(4);

            this.FileInfo = FileInfo;

			Text = FileName;

            this.Position = Position;
			OriginalPosition = Position;


            Height = vxLayout.GetScaledHeight(72);
            if (vxEngine.PlatformType == vxPlatformHardwareType.Mobile)
            {
                Height = vxLayout.GetScaledHeight(96);
            }
            Width = 3000;

			IsTogglable = true;

            Theme = new vxUIControlTheme(
                new vxColourTheme(new Color(0.15f, 0.15f, 0.15f, 0.5f), Color.DarkOrange),
                new vxColourTheme(Color.LightGray));

			ArtProvider = (vxFileDialoglItemArtProvider)vxUITheme.ArtProviderForFileDialogItem.Clone();
		}

		public override void ThisSelect()
		{
			ToggleState = true;
		}
		public override void UnSelect()
		{
			ToggleState = false;
		}

		public override void Draw()
		{
			// Now draw this GUI item using it's ArtProvider.
            this.ArtProvider.Draw(this);
		}


        int index = 0;
        vxOpenSandboxFileDialog openFileDialog;
        public virtual void Process(vxOpenSandboxFileDialog openFileDialog, int index)
        {
            BckgrndWrkr = new BackgroundWorker();

            BckgrndWrkr.DoWork += OnAsyncFileDetailsLoad;
            BckgrndWrkr.RunWorkerCompleted += HandleRunWorkerCompletedEventHandler;
            BckgrndWrkr.WorkerSupportsCancellation = true;
            this.index = index;
            this.openFileDialog = openFileDialog;
            BckgrndWrkr.RunWorkerAsync(FilePath);

        }

        BackgroundWorker BckgrndWrkr;




        public void CancelProcessing()
        {
            if (BckgrndWrkr != null)
            {
                BckgrndWrkr.CancelAsync();
                //Console.WriteLine("Canceling Process");
            }
        }

        public bool IsCancelingProcessing()
        {
            if (BckgrndWrkr != null)
                return BckgrndWrkr.CancellationPending;
            else
                return false;
        }

        struct FileInfoResult
        {
            public Texture2D Thumbnail;
            public Texture2D Screenshot;
            public vxFileInfo fileMetaData;
        }

        /// <summary>
        /// An async method called to load file details.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public virtual void OnAsyncFileDetailsLoad(object sender, DoWorkEventArgs e)
        {
            string cachePath = Path.Combine(vxIO.PathToCacheFolder, "sandbox_thumbnails", Path.GetFileNameWithoutExtension(FilePath));
            if (!Directory.Exists(cachePath))
            {
                // Decompress The Directory
                vxIO.DecompressToDirectory(FilePath, cachePath, null, false);
            }

            // First load the file info
            var fileInfo = new vxFileInfo();
            fileInfo.Load(cachePath);

            Texture2D thumbnail;
            FileInfoResult FileInfoResult = new FileInfoResult();
            FileInfoResult.fileMetaData = fileInfo;
            /*
            File.Copy(cachePath + "/img.png", cachePath + "/img.xnb", true);
            ContentManager Content = new ContentManager(vxEngine.CurrentGame.Services, cachePath);
            var texture = Content.Load<Texture2D>("img");
            */

            if (File.Exists(cachePath + "/thumbnail.png"))
            using (var fileStream = new FileStream(cachePath + "/thumbnail.png", FileMode.Open))
            {
                thumbnail = Texture2D.FromStream(vxGraphics.GraphicsDevice, fileStream);
                    FileInfoResult.Thumbnail = thumbnail;
                }

            Texture2D screenshot;
            try
            {
                using (var fileStream = new FileStream(cachePath + "/img.png", FileMode.Open))
                {
                    
                    screenshot = Texture2D.FromStream(vxGraphics.GraphicsDevice, fileStream);
                    FileInfoResult.Screenshot = screenshot;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("EROR: "+ex);
                e.Result = FileInfoResult;
            }
            

            e.Result = FileInfoResult;
        }

        public Texture2D Screenshot;
        public virtual void HandleRunWorkerCompletedEventHandler(object sender, RunWorkerCompletedEventArgs e)
        {
            if(e.Cancelled)
            {
                Console.WriteLine("Canceled");
            }
            if(e.Error != null)
            {
                Console.WriteLine("Error With :" + FilePath);
                Console.WriteLine("     "+e.Error.Message);
                //ButtonImage = DefaultTexture;
            }
            else if (e.Result != null)
            {
                if (e.Result is FileInfoResult)
                {
                    var finfo = (FileInfoResult)e.Result;
                    _fileVersion = "v."+finfo.fileMetaData.Version.ToString();
                    _metaData = finfo.fileMetaData;
                    Text = finfo.fileMetaData.SandboxFileInfo.Title;
                    ButtonImage = finfo.Thumbnail;
                    Screenshot = finfo.Screenshot;
                }
            }
            index++;
            if(openFileDialog.IsActive)
                openFileDialog.ProcessItemsAsync(index);
        }
	}
}
