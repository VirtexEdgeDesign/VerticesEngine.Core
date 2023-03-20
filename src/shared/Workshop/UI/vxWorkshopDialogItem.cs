using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Net;
using VerticesEngine.Graphics;
using VerticesEngine.UI;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Dialogs;
using VerticesEngine.UI.Themes;
using VerticesEngine.Utilities;

namespace VerticesEngine.Workshop.UI
{
    /// <summary>
    /// Workshop dialog item.
    /// </summary>
    public class vxWorkshopDialogItem : vxScrollPanelItem
    {
        /// <summary>
        /// Item reference from search results
        /// </summary>
        public vxIWorkshopItem Item
        {
            get { return _item; }
        }
        private vxIWorkshopItem _item;

        public string Author
        {
            get { return Item.Author; }
        }


        /// <summary>
        /// File description
        /// </summary>
        public string Description
        {
            get { return Item.Description; }
        }

        /// <summary>
        /// File size
        /// </summary>
        public string FileSize
        {
            get
            {
                float size = Item.Size / 1024.0f / 1024.0f;

                return Math.Round((float)size, 3).ToString()+ " MB";
            }
        }

        public bool IsProcessed = false;

		/// <summary>
		/// The art provider.
		/// </summary>
		public new vxWorkshopDialogItemArtProvider ArtProvider;

        private vxButtonImageControl buttonImageControl;

        private Rectangle downloadLoc = new Rectangle(0, 0, 72, 72);// new Rectangle(925, 674, 32, 32);
        private Rectangle downloadedLoc = new Rectangle(0, 0, 72, 72);// new Rectangle(893, 674, 32, 32);

        private int index = 0;
        private vxWorkshopSearchResultDialog searchResultsDialog;
        private string imgFilePath;


        /// <summary>
        /// Initializes a new instance of the <see cref="vxWorkshopDialogItem"/> class.
        /// </summary>
        /// <param name="item">Workshop Item</param>
        public vxWorkshopDialogItem(vxIWorkshopItem item) : base(item.Title, Vector2.Zero, null, 0)
		{
            _item = item;

            Padding = new Vector2(4);

            this.Position = Position;
			OriginalPosition = Position;

			Height = 128;
			Width = 4096;

			IsTogglable = true;

            Theme = new vxUIControlTheme(
                new vxColourTheme(new Color(0.15f, 0.15f, 0.15f, 0.5f), Color.DarkOrange, Color.DeepSkyBlue, new Color(0.15f, 0.15f, 0.15f, 0.15f)),
                new vxColourTheme(Color.LightGray, Color.LightGray, Color.LightGray, Color.DimGray));
            
            ArtProvider = (vxWorkshopDialogItemArtProvider)vxUITheme.ArtProviderForWorkshopDialogItem.Clone();

            buttonImageControl = new vxButtonImageControl(downloadLoc, Position);

            buttonImageControl.IsTogglable = true;
                buttonImageControl.Theme.Background = new vxColourTheme(Color.Gray, Color.White,Color.White);
            
            buttonImageControl.Clicked += delegate {

                vxConsole.WriteLine("Downloading "+Item.Title+"...");


            };
        }

        protected internal override void Update()
        {
            base.Update();


            buttonImageControl.Position = new Vector2(Bounds.Right - 64, Bounds.Center.Y - 32);
            buttonImageControl.Update();
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
            this.ArtProvider.DrawUIControl(this);
		}


        public virtual void Process(vxWorkshopSearchResultDialog searchResultsDialog, int index)
        {
            this.searchResultsDialog = searchResultsDialog;
            this.index = index;

            imgFilePath = System.IO.Path.Combine(vxIO.PathToTempFolder, Item.Id + ".png");

            if (!File.Exists(imgFilePath))
            {
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadFileCompleted += OnFiledDownloadCompleted;
                    wc.DownloadFileAsync(new Uri(Item.PreviewImageURL), imgFilePath);
                }
            }
            else
            {
                OnProcessFinished();
            }
        }



        private void OnFiledDownloadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                Console.WriteLine("The download has been cancelled");
                return;
            }

            if (e.Error != null) // We have an error! Retry a few times, then abort.
            {
                Console.WriteLine("An error ocurred while trying to download file");

                OnProcessFinished();
                return;
            }

            using (var fileStream = new FileStream(imgFilePath, FileMode.Open))
            {
                Item.PreviewImage = Texture2D.FromStream(vxGraphics.GraphicsDevice, fileStream);
                fileStream.Dispose();
            }
            
            OnProcessFinished();
            vxConsole.WriteIODebug($"File '{imgFilePath}' succesfully downloaded");

        }

        private void Wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Console.WriteLine(e.ProgressPercentage + "% | " + e.BytesReceived + " bytes out of " + e.TotalBytesToReceive + " bytes retrieven.");
        }

        void OnProcessFinished()
        {
            IsProcessed = true;
            if (Item.PreviewImage != null)
                ButtonImage = Item.PreviewImage;

            index++;
            searchResultsDialog.ProcessItemsAsync(index);

        }

	}
}
