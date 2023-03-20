using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Graphics;
using VerticesEngine.Plugins.Mods;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Themes;

namespace VerticesEngine.UI.Dialogs
{
    /// <summary>
    /// File Chooser Dialor Item.
    /// </summary>
    public class vxModDialoglItem : vxScrollPanelItem
	{
		/// <summary>
		/// The art provider.
		/// </summary>
		public new vxModDialoglItemArtProvider ArtProvider;

        /// <summary>
        /// The file info.
        /// </summary>
        public FileInfo FileInfo;

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

        public string Directory
        {
            get { return FileInfo.Directory.FullName; }
        }

        public string Description
        {
            get { return config.Description; }
        }


        vxModItemStatus ModItem;

        vxModConfig config;

        public vxToggleImageButton EnabledButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Dialogs.vxModDialoglItem"/> class.
        /// </summary>
        /// <param name="ModItem">Mod item.</param>
		public vxModDialoglItem(ref vxModItemStatus ModItem) : base("", Vector2.Zero, null, 0)
		{
            this.ModItem = ModItem;
            
            Padding = new Vector2(4);
            FileInfo = new FileInfo(ModItem.Path);
            config = vxModConfig.Load(Directory);

            Text = config.Name;
			
            this.Position = Position;
			OriginalPosition = Position;

			Height = 128;
			Width = 30000;

            var imgPath = Path.Combine(Directory, "preview.png");
            if(File.Exists(imgPath))
            {
                using (var fileStream = new FileStream(imgPath, FileMode.Open))
                {
                    ButtonImage = Texture2D.FromStream(vxGraphics.GraphicsDevice, fileStream);
                }
            }
            else
            {
                ButtonImage = vxInternalAssets.Textures.Blank;
            }

            Theme = new vxUIControlTheme(
                new vxColourTheme(new Color(0.15f, 0.15f, 0.15f, 0.5f), Color.DarkOrange),
                new vxColourTheme(Color.LightGray));

            EnabledButton = new vxToggleImageButton(vxUITheme.SpriteSheetLoc.ToggleOn, vxUITheme.SpriteSheetLoc.ToggleOff, Vector2.Zero);

            EnabledButton.Width = 48;
            EnabledButton.Height = 48;

            EnabledButton.Clicked += EnabledButton_Clicked;

            ArtProvider = (vxModDialoglItemArtProvider)vxUITheme.ArtProviderForModDialogItem.Clone();
            IsEnabled = ModItem.IsEnabled;
		}


        void EnabledButton_Clicked(object sender, Events.vxUIControlClickEventArgs e)
        {
            IsEnabled = !IsEnabled;
            this.ModItem.IsEnabled = IsEnabled;
        }


        protected internal override void Update()
        {
            base.Update();

            EnabledButton.Position = new Vector2(
                Bounds.Right - EnabledButton.Width - ArtProvider.Padding.X * 2,
                Position.Y + Bounds.Height / 2 - EnabledButton.Height / 2);

            EnabledButton.Update();
        }

        public override void Draw()
		{
			// Now draw this GUI item using it's ArtProvider.
            this.ArtProvider.Draw(this);

            EnabledButton.Draw();
		}
	}
}
