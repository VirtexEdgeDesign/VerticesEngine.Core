using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Utilities;
using VerticesEngine;
using VerticesEngine.UI.Events;
using VerticesEngine.UI.Themes;
using System.IO;

namespace VerticesEngine.UI.Dialogs
{
	/// <summary>
	/// File Chooser Dialor Item.
	/// </summary>
	public class vxFileExplorerDirectoryItem : vxScrollPanelItem
	{
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
                
                //if (size > 1000)
                //    return size / 1000 + "kB";

                //return FileInfo.Length.ToString(); 
            }
        }

		/// <summary>
		/// The art provider.
		/// </summary>
        public new vxFileExplorerItemArtProvider ArtProvider;

        public FileInfo FileInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Dialogs.vxFileExplorerItem"/> class.
        /// </summary>
        /// <param name="Engine">Engine.</param>
        /// <param name="FileInfo">File info.</param>
        /// <param name="buttonImage">Button image.</param>
        /// <param name="ElementIndex">Element index.</param>
        public vxFileExplorerDirectoryItem(FileInfo FileInfo, Texture2D buttonImage, int ElementIndex) :
		base(FileInfo.Name, Vector2.Zero, buttonImage, ElementIndex)
		{
			Padding = new Vector2(4);

            this.FileInfo = FileInfo;

			Text = FileName;

			Index = ElementIndex;
			ButtonImage = buttonImage;
			Bounds = new Rectangle(0, 0, 64, 64);

            Font = vxInternalAssets.Fonts.ViewerFont;
            ArtProvider = (vxFileExplorerItemArtProvider)vxUITheme.ArtProviderForFileExplorerItem.Clone();
            Height = (int)(Font.MeasureString(Text).Y + ArtProvider.Padding.Y * 2);
			Width = 3000;

			IsTogglable = true;

            Theme = new vxUIControlTheme(
                new vxColourTheme(new Color(0.15f, 0.15f, 0.15f, 0.5f), Color.DarkOrange),
                new vxColourTheme(Color.LightGray));


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

			//base.Draw();
		}
	}
}
