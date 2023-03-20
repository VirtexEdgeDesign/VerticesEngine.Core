using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Utilities;
using VerticesEngine;
using VerticesEngine.UI.Events;
using VerticesEngine.UI.Themes;
using System.IO;
using VerticesEngine.ContentManagement;

namespace VerticesEngine.UI.Dialogs
{
	/// <summary>
	/// File Chooser Dialor Item.
	/// </summary>
	public class vxFileExplorerItem : vxScrollPanelItem
	{

        public Texture2D IconTexture;


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
            get { return FileInfo.Directory.Name; }
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
            get {
                float size = FileInfo.Length;

                int sizeInt = (int)size / 1000;

                string result = ((float)sizeInt / 1000 + " MB");

                return result;
            }
        }

		/// <summary>
		/// The art provider.
		/// </summary>
        public new vxFileExplorerItemArtProvider ArtProvider;

        /// <summary>
        /// The file info.
        /// </summary>
        public FileInfo FileInfo;

        /// <summary>
        /// The is directory.
        /// </summary>
        public bool IsDirectory = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Dialogs.vxFileExplorerItem"/> class.
        /// </summary>
        /// <param name="FileInfo">File info.</param>
        /// <param name="ElementIndex">Element index.</param>
        /// <param name="IsDirectory">If set to <c>true</c> is directory.</param>
		public vxFileExplorerItem(FileInfo FileInfo, int ElementIndex, bool IsDirectory = false) :
		base(FileInfo.Name,
			 Vector2.Zero,
			 null,
			 ElementIndex)
		{
			Padding = new Vector2(4);

            this.FileInfo = FileInfo;
            this.IsDirectory = IsDirectory;
			Text = FileName;

			Index = ElementIndex;

			Bounds = new Rectangle(0, 0, 64, 64);

            Font = vxInternalAssets.Fonts.ViewerFont;
            ArtProvider = (vxFileExplorerItemArtProvider)vxUITheme.ArtProviderForFileExplorerItem.Clone();
            Height = (int)(Font.MeasureString(Text).Y + ArtProvider.Padding.Y * 2);
			Width = 3000;

			IsTogglable = true;


            Theme = new vxUIControlTheme(
                new vxColourTheme(new Color(0.15f, 0.15f, 0.15f, 0.5f), Color.DarkOrange),
                new vxColourTheme(Color.LightGray));

            Texture2D folderIcon = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/rbn/main/open_16");

            Texture2D icon = DefaultTexture;

            switch(FileInfo.Extension)
            {
                case ".zip":
                    icon = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/gui/icons/zip");
                    break;

                case ".png":
                case ".jpeg":
                case ".jpg":
                    icon = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/gui/icons/picture");
                    break;

                case ".obj":
                case ".fbx":
                case ".stl":
                    icon = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/gui/icons/model");
                    break;

                default:
                    icon = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/gui/icons/document");
                    break;
            }

            ButtonImage = IsDirectory ? folderIcon : icon;
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
	}
}
