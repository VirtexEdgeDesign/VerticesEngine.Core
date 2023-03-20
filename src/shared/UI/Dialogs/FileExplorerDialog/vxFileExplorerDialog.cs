using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using VerticesEngine.Graphics;
using VerticesEngine.Input.Events;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Events;
using VerticesEngine.UI.MessageBoxs;

namespace VerticesEngine.UI.Dialogs
{
    /// <summary>
    /// Open File Dialog
    /// </summary>
    public class vxFileExplorerDialog : vxDialogBase
    {
        #region Fields

        public vxScrollPanel ScrollPanel;
        public vxScrollPanel SideScrollPanel;

        public string Path
        {
            get { return _path; }   
            set {
                string prevPath = _path;

                try
                {
                    _path = value;
                    RefreshDirectoryItems();
                }
                catch(Exception ex)
                {
                    _path = prevPath;
                    RefreshDirectoryItems();
                    vxMessageBox.Show("Can't Use Directory", "Can't enter this directory: \n" + ex.Message);
                }
            }
        }

        string _path="";
        #endregion

        #region Events

        public new event EventHandler<PlayerIndexEventArgs> Accepted;
        public new event EventHandler<PlayerIndexEventArgs> Cancelled;

        #endregion

        #region Initialization


        vxFileExplorerURLBar m_explorerNavBar;



        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Dialogs.vxOpenFileDialog"/> class.
        /// </summary>
        /// <param name="Engine">Engine.</param>
        /// <param name="startpath">Path.</param>
        public vxFileExplorerDialog(string startpath)
			: base("File Explorer", vxEnumButtonTypes.OkCancel)
        {
            // Set the path without triggering the refresh
            this._path = startpath;

        }

        /// <summary>
        /// Loads graphics content for this screen. This uses the shared ContentManager
        /// provided by the Game class, so the content will remain loaded forever.
        /// Whenever a subsequent MessageBoxScreen tries to load this same content,
        /// it will just get back another reference to the already loaded data.
        /// </summary>
        public override void LoadContent()
        {
			base.LoadContent();
            int sideWidth = 200;
            ScrollPanel = new vxScrollPanel(new Vector2(ArtProvider.GUIBounds.X + ArtProvider.Padding.X + sideWidth, 
                            ArtProvider.GUIBounds.Y + ArtProvider.Padding.Y + OKButton.Bounds.Height) + ArtProvider.PosOffset,
                                            ArtProvider.GUIBounds.Width - (int)ArtProvider.Padding.X * 2 - sideWidth,
				ArtProvider.GUIBounds.Height - OKButton.Bounds.Height * 2 - (int)ArtProvider.Padding.Y * 4);
            ScrollPanel.Padding = new Vector2(1);

            InternalGUIManager.Add(ScrollPanel);

            sideWidth -= 5;
            SideScrollPanel=new vxScrollPanel(new Vector2(ArtProvider.GUIBounds.X + ArtProvider.Padding.X,
                            ArtProvider.GUIBounds.Y + ArtProvider.Padding.Y + OKButton.Bounds.Height) + ArtProvider.PosOffset,
                                            sideWidth,
                ArtProvider.GUIBounds.Height - OKButton.Bounds.Height * 2 - (int)ArtProvider.Padding.Y * 4);
            SideScrollPanel.Padding = new Vector2(1);

            InternalGUIManager.Add(SideScrollPanel);


            AddSpecialFolderItem(Environment.SpecialFolder.Desktop);
            AddSpecialFolderItem(Environment.SpecialFolder.MyDocuments);
            AddSpecialFolderItem(Environment.SpecialFolder.MyPictures);
            //AddSpecialFolderItem(Environment.SpecialFolder.MyVideos);
            AddSpecialFolderItem(Environment.SpecialFolder.MyMusic);

            var navBarLoc = new Vector2(ScrollPanel.Position.X, ArtProvider.GUIBounds.Y);
            m_explorerNavBar = new vxFileExplorerURLBar(this, navBarLoc, ScrollPanel.Width);

            InternalGUIManager.Add(m_explorerNavBar);
            Path = _path;
		}

        void AddSpecialFolderItem(Environment.SpecialFolder folder)
        {
            //Create a New File Dialog Button
            vxFileExplorerItem fileDialogButton = new vxFileExplorerItem(new FileInfo(Environment.GetFolderPath(folder)), 0, true);

            fileDialogButton.Clicked += delegate{
                foreach (var item in SideScrollPanel.Items)
                    item.ToggleState = false;
                fileDialogButton.ToggleState = true;
            };
            fileDialogButton.DoubleClicked += delegate {
                foreach (var item in SideScrollPanel.Items)
                    item.ToggleState = false;

                Path = Environment.GetFolderPath(folder);
            };

            //Set Button Width
            fileDialogButton.Width = vxGraphics.GraphicsDevice.Viewport.Width - (4 * (int)this.ArtProvider.Padding.X);


            if (fileDialogButton.FileInfo.Name.GetFileNameFromPath() != "")
                SideScrollPanel.AddItem(fileDialogButton);
        }


        #endregion

        #region Handle Input


        protected override void OnOKButtonClicked(object sender, vxUIControlClickEventArgs e)
        {
            if (SelectedItem != "")
            {
                // Raise the accepted event, then exit the message box.
                if (Accepted != null)
                    Accepted(this, new PlayerIndexEventArgs(ControllingPlayer.Value));

                ExitScreen();
            }
        }

        protected override void OnCancelButtonClicked(object sender, vxUIControlClickEventArgs e)
        {
            // Raise the cancelled event, then exit the message box.
            if (Cancelled != null)
                Cancelled(this, new PlayerIndexEventArgs(ControllingPlayer.Value));

            ExitScreen();
        }

        
        #endregion

        void AddScrollItem(string file, int index, bool IsDirectory)
        {
            //Create a New File Dialog Button
            vxFileExplorerItem fileDialogButton = new vxFileExplorerItem(new FileInfo(file), index, IsDirectory);

            fileDialogButton.Clicked += GetHighlitedItem;
            fileDialogButton.DoubleClicked += delegate {

                SelectedItem = System.IO.Path.Combine(Path, fileDialogButton.Text);
                // Go into the next directory
                if(fileDialogButton.IsDirectory)
                    Path = System.IO.Path.Combine(Path, fileDialogButton.Text);
                else
                    OKButton.Select();
            };

            //Set Button Width
            fileDialogButton.Width = vxGraphics.GraphicsDevice.Viewport.Width - (4 * (int)this.ArtProvider.Padding.X);
            //Console.WriteLine(fileDialogButton.FileInfo.Name.GetFileNameFromPath());
            if(fileDialogButton.FileInfo.Name.GetFileNameFromPath() != "")
                ScrollPanel.AddItem(fileDialogButton);
        }
        public string SelectedItem = "";

        public void RefreshDirectoryItems()
        {
            SelectedItem = "";
            ScrollPanel.Clear();
            //List_Items.Clear();

            ScrollPanel.ScrollBar.TravelPosition = 0;

            string[] dirPaths = Directory.GetDirectories(Path);

            List<vxFileExplorerItem> List_Temp_Items = new List<vxFileExplorerItem>();

            int index = 0;
            foreach (string file in dirPaths)
            {
                AddScrollItem(file, index, true);
                index++;
            }


            string[] filePaths = Directory.GetFiles(Path);
            foreach (string file in filePaths)
            {
                AddScrollItem(file, index, false);
                index++;
            }
            ScrollPanel.UpdateItemPositions();

            m_explorerNavBar.FilePath = Path;
        }


		public void GetHighlitedItem(object sender, vxUIControlClickEventArgs e)
        {
            foreach (var item in ScrollPanel.Items)
                item.ToggleState = false;

            e.GUIitem.ToggleState = true;

            SelectedItem = System.IO.Path.Combine(Path, e.GUIitem.Text);
        }
    }
}
