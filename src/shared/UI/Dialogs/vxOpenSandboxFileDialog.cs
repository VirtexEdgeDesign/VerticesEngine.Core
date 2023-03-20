using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using VerticesEngine.Graphics;
using VerticesEngine.Input.Events;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Events;
using VerticesEngine.UI.MessageBoxs;
using VerticesEngine.UI.Themes;
using VerticesEngine.Utilities;

namespace VerticesEngine.UI.Dialogs
{
    public class vxSandboxFileSelectedEventArgs : EventArgs
    {

        /// <summary>
        /// Gets the index of the player who triggered this event.
        /// </summary>
        public FileInfo SelectedFile
        {
            get { return _selectedFile; }
        }

        FileInfo _selectedFile;

        /// <summary>
        /// Constructor.
        /// </summary>
        public vxSandboxFileSelectedEventArgs(string SelectedFilePath)
        {
            _selectedFile = new FileInfo(SelectedFilePath);
        }

    }

    /// <summary>
    /// New file dialog item delegate.
    /// </summary>
    public delegate vxFileDialogItem NewFileDialogItemDelegate(vxEngine Engine, string filePath);

    /// <summary>
    /// Open File Dialog
    /// </summary>
    public class vxOpenSandboxFileDialog : vxDialogBase
    {
        #region Fields

        public vxScrollPanel ScrollPanel;

        string FileExtentionFilter;


        /// <summary>
        /// Main text for the Message box
        /// </summary>
        public string Path;


        #endregion

        #region Events

        public new event EventHandler<vxSandboxFileSelectedEventArgs> Accepted;
        public new event EventHandler<vxUIControlClickEventArgs> Cancelled;

        #endregion

        #region Initialization

        //public string FileName = "";

        vxButtonControl DeleteFileBtn;

        //public bool IsItemSelected = false;

        /// <summary>
        /// Gets the selected file.
        /// </summary>
        /// <value>The selected file.</value>
        public FileInfo SelectedFile
        {
            get { return new FileInfo(SelectedItem); }
        }


        public vxImage Image;


        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Dialogs.vxOpenSandboxFileDialog"/> class.
        /// </summary>
        /// <param name="Engine">Engine.</param>
        /// <param name="FileExtentionFilter">File extention filter.</param>
        public vxOpenSandboxFileDialog(string FileExtentionFilter) : this(FileExtentionFilter, null)
        {

        }

        public vxOpenSandboxFileDialog(string FileExtentionFilter, NewFileDialogItemDelegate OnNewFileDialogItem)
            : base("Open a Sandbox File", vxEnumButtonTypes.OkCancel)
        {
            this.Path = vxIO.PathToSandbox;
            this.FileExtentionFilter = FileExtentionFilter;

        }


        //NewFileDialogItemDelegate OnNewFileDialogItem;

        /// <summary>
        /// The selected item.
        /// </summary>
        public string SelectedItem = "";


        public vxLabel GetLabel()
        {
            var label = new vxLabel("", Vector2.Zero);
            label.Theme.Text = new vxColourTheme(Color.Black);
            label.IsShadowVisible = true;
            label.ShadowColour = Color.Black * 0.5f;
            InternalGUIManager.Add(label);
            label.Font = vxUITheme.Fonts.Size12;
            return label;
        }

        public static Texture2D InitialTexture;

        /// <summary>
        /// Loads graphics content for this screen. This uses the shared ContentManager
        /// provided by the Game class, so the content will remain loaded forever.
        /// Whenever a subsequent MessageBoxScreen tries to load this same content,
        /// it will just get back another reference to the already loaded data.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            // Set up the Preview Image
            Image = new vxImage(InitialTexture,
                                new Vector2(
                                    ArtProvider.GUIBounds.X + ArtProvider.Padding.X,
                                    ArtProvider.GUIBounds.Y + ArtProvider.Padding.Y),
                                (int)(ArtProvider.GUIBounds.Width / 2 - ArtProvider.Padding.X * 2),
                                (int)(ArtProvider.GUIBounds.Height / 2 - ArtProvider.Padding.Y));
            Image.DoBorder = true;

            imageWidth = (int)(ArtProvider.GUIBounds.Width / 2 - ArtProvider.Padding.X * 2);

            InternalGUIManager.Add(Image);




            ScrollPanel = new vxScrollPanel(new Vector2(
                ArtProvider.GUIBounds.X + ArtProvider.GUIBounds.Width / 2 + ArtProvider.Padding.X * 3,
                ArtProvider.GUIBounds.Y + ArtProvider.Padding.Y),
                                            (ArtProvider.GUIBounds.Width / 2 - (int)ArtProvider.Padding.X * 2),
                                            ArtProvider.GUIBounds.Height - OKButton.Bounds.Height - (int)ArtProvider.Padding.Y * 4);


            InternalGUIManager.Add(ScrollPanel);




            DeleteFileBtn = new vxButtonControl("Delete File", new Vector2(ArtProvider.GUIBounds.X + ArtProvider.Padding.X * 2, OKButton.Position.Y));

            InternalGUIManager.Add(DeleteFileBtn);

            DeleteFileBtn.Clicked += DeleteFileBtn_Clicked;
            DeleteFileBtn.IsEnabled = false;

            RefreshDirectoryItems();

            OKButton.IsEnabled = false;


            FileNameLabel = GetLabel();
            FileNameLabel.Scale = 1.15f;
            //FileNameLabel.Font = vxUITheme.Fonts.Size36;
            FilSizeLabel = GetLabel();
            FileLastUsedLabel = GetLabel();
        }

        protected internal override void Update()
        {
            base.Update();

            DeleteFileBtn.Position = new Vector2(DeleteFileBtn.Position.X, OKButton.Position.Y);
        }

        vxLabel FileNameLabel;
        vxLabel FilSizeLabel;
        vxLabel FileLastUsedLabel;


        public void RefreshDirectoryItems()
        {
            SelectedItem = "";

            // reset the Scroll Panel
            ScrollPanel.Clear();
            ScrollPanel.ScrollBar.TravelPosition = 0;

            // get all files with the specified file extention filter
            foreach (string file in Directory.GetFiles(vxIO.PathToSandbox, "*." + FileExtentionFilter))
                AddScrollItem(file);

            // Then finalise the scroll panel
            ScrollPanel.UpdateItemPositions();

            ProcessItemsAsync(0);
        }

        void AddScrollItem(string file)
        {
            // Get a new File Dialog Item
            //var fileDialogButton = (OnNewFileDialogItem != null) ? OnNewFileDialogItem(Engine, file) : new vxFileDialogItem(new FileInfo(file));
            var fileDialogButton = new vxFileDialogItem(new FileInfo(file));

            // Hookup Click Events
            fileDialogButton.Clicked += OnItemClicked;
            fileDialogButton.DoubleClicked += delegate
            {
                OKButton.Select();
            };

            //Set Button Width
            fileDialogButton.Width = vxGraphics.GraphicsDevice.Viewport.Width - (4 * (int)this.ArtProvider.Padding.X);
            if (fileDialogButton.FileInfo.Name.GetFileNameFromPath() != "")
                ScrollPanel.AddItem(fileDialogButton);
        }

        int imageWidth = 0;
        vxFileDialogItem SelectedDialogItem;
        public void OnItemClicked(object sender, vxUIControlClickEventArgs e)
        {
            OKButton.IsEnabled = true;
            DeleteFileBtn.IsEnabled = true;

            foreach (var item in ScrollPanel.Items)
                item.ToggleState = false;

            e.GUIitem.ToggleState = true;

            if (e.GUIitem is vxFileDialogItem)
            {
                // Get the GUI Item
                SelectedDialogItem = ((vxFileDialogItem)e.GUIitem);

                // Get the Selected Item Path
                SelectedItem = System.IO.Path.Combine(Path, SelectedDialogItem.FileName);

                // No fet the screenshot
                Texture2D scrnsht = SelectedDialogItem.Screenshot;
                Image.Texture = scrnsht;
                Image.Width = imageWidth;
                if(scrnsht != null)
                Image.Height = (int)(imageWidth * (float)scrnsht.Height / (float)scrnsht.Width);

                // Now update all of the labels
                FileNameLabel.Text = SelectedDialogItem.Text;
                FileNameLabel.Position = new Vector2(Image.Bounds.Left + ArtProvider.Padding.X, Image.Bounds.Bottom + ArtProvider.Padding.Y); ;

                FilSizeLabel.Text = SelectedDialogItem.FileSize;
                FilSizeLabel.Position = new Vector2(Image.Bounds.Right - FilSizeLabel.Bounds.Width,
                                                    Image.Bounds.Bottom + FileNameLabel.Bounds.Height + ArtProvider.Padding.Y); ;


                FileLastUsedLabel.Text = SelectedDialogItem.FileInfo.LastWriteTime.ToString();
                FileLastUsedLabel.Position = new Vector2(Image.Bounds.Left + ArtProvider.Padding.X,
                                                    Image.Bounds.Bottom + FileNameLabel.Bounds.Height + ArtProvider.Padding.Y); ;

            }
        }

        vxFileDialogItem ItemToDelete;
        void DeleteFileBtn_Clicked(object sender, vxUIControlClickEventArgs e)
        {
            ItemToDelete = SelectedDialogItem;
            string message = "Are you sure you want to delete \nthe file:  '" + ItemToDelete.FileName + "'";

            vxMessageBox confirmDeleteMessageBox = new vxMessageBox(message, "Delete File?");
            confirmDeleteMessageBox.Accepted += ConfirmDeleteMessageBoxAccepted;
            vxSceneManager.AddScene(confirmDeleteMessageBox, PlayerIndex.One);
        }



        public void ProcessItemsAsync(int index)
        {
            if (ScrollPanel.Items.Count > 0 && index < ScrollPanel.Items.Count)
            {
                vxConsole.WriteVerboseLine("PARSING: " + index);
                var item = ScrollPanel.Items[index];
                if (item is vxFileDialogItem)
                {
                    currentProcessingItem = ((vxFileDialogItem)item);
                    currentProcessingItem.Process(this, index);
                }
            }
        }

        vxFileDialogItem currentProcessingItem;

        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ConfirmDeleteMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            try
            {
                File.Delete(ItemToDelete.FilePath);
            }
            catch (Exception ex)
            {
                vxMessageBox error = new vxMessageBox("Error Deleting File", "Error", vxEnumButtonTypes.Ok);
                vxSceneManager.AddScene(error, PlayerIndex.One);
                vxConsole.WriteException(this, ex);
            }

            // Now clear the lists to be re-added too.
            ///ScrollPanel.Clear();
            ScrollPanel.Items.Remove(ItemToDelete);
            ItemToDelete = null;
            DeleteFileBtn.IsEnabled = false;
            OKButton.IsEnabled = false;
            SelectedItem = "";

            // Then finalise the scroll panel
            RefreshDirectoryItems();
        }

        public override void UnloadContent()
        {
            //foreach (vxFileDialogItem fd in List_Items)
            //fd.ButtonImage.Dispose();
            foreach(var item in ScrollPanel.Items)
            { 
                if (item is vxFileDialogItem)
                {
                    ((vxFileDialogItem)item).CancelProcessing();
                }
            }

            base.UnloadContent();
        }

        #endregion

        #region Handle Input


        protected override void OnOKButtonClicked(object sender, vxUIControlClickEventArgs e)
        {
            if (SelectedItem != "")
            {
                if (currentProcessingItem != null)
                {
                    currentProcessingItem.CancelProcessing();
                }
                
                // Raise the accepted event, then exit the message box.
                if (Accepted != null)
                    Accepted(this, new vxSandboxFileSelectedEventArgs(SelectedItem));

                ExitScreen();
            }
        }



        protected override void OnCancelButtonClicked(object sender, vxUIControlClickEventArgs e)
        {
            // Raise the cancelled event, then exit the message box.
            if (Cancelled != null)
                Cancelled(this, e);

            ExitScreen();
        }


        #endregion


    }
}
