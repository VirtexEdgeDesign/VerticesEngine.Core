using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VerticesEngine;
using VerticesEngine.Input.Events;
using VerticesEngine.UI.Menus;
using VerticesEngine.Utilities;
using Microsoft.Xna.Framework.Audio;
using VerticesEngine.UI.Themes;

namespace VerticesEngine.UI.Controls
{
	/// <summary>
	/// Basic Button GUI Control.
	/// </summary>
	public class vxMenuEntry : vxUIControl
    {
		/// <summary>
		/// The Parent Menu Screen for this Menu Entry.
		/// </summary>
		public vxMenuBaseScreen ParentScreen;

        /// <summary>
        /// The icon location.
        /// </summary>
        public Rectangle IconLocation = Rectangle.Empty;

        /// <summary>
        /// Gets or sets the texture for this Menu Entry Background.
        /// </summary>
        /// <value>The texture.</value>
        public Texture2D Texture;

        /// <summary>
        /// Event raised when the menu entry is selected.
        /// </summary>
        public event EventHandler<PlayerIndexEventArgs> Selected;


        /// <summary>
        /// The given Art Provider of the Menu Entry. 
        /// </summary>
        public vxMenuItemArtProvider ArtProvider { get; internal set; }

        public override string ToString()
        {
            return "MenuEntry {" + this.Text + "}";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxMenuEntry"/> class.
        /// </summary>
        /// <param name="ParentScreen">Parent screen.</param>
        /// <param name="LocalisationKey">Localisation key.</param>
        public vxMenuEntry(vxMenuBaseScreen ParentScreen, string LocalisationKey)
            : this(ParentScreen, LocalisationKey, Rectangle.Empty)
        {

        }


        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxMenuEntry"/> class.
        /// </summary>
        /// <param name="ParentScreen">Parent screen.</param>
        /// <param name="LocalisationKey">Localisation key.</param>
        /// <param name="icon">Icon.</param>
        public vxMenuEntry(vxMenuBaseScreen ParentScreen, string LocalisationKey, Rectangle IconLocation) : base(Vector2.Zero)
        {
            this.LocalisationKey = LocalisationKey;

			//Set Font from Global Engine Font
			this.Font = vxUITheme.Fonts.Size24;

			//Set Parten Screen
			this.ParentScreen = ParentScreen;

			//Text
            this.Text = vxLocalizer.GetText(LocalisationKey);

            this.IconLocation = IconLocation;

			//Engine
			//this.Engine = Engine;

			//Get Settings
			//this.//Color_Normal = vxUITheme.ArtProviderForMenuScreenItems.Theme.Background.NormalColour;
            //this.Color_Highlight = vxUITheme.ArtProviderForMenuScreenItems.Theme.Background.HoverColour;

            Width = (int)(vxUITheme.Fonts.Size24.MeasureString(this.Text).X + vxUITheme.ArtProviderForMenuScreenItems.Padding.X * 2);
            Height = (int)(vxUITheme.Fonts.Size24.MeasureString(this.Text).Y + vxUITheme.ArtProviderForMenuScreenItems.Padding.Y * 2);

            //Set up Bounding Rectangle
            Bounds = new Rectangle(
				(int)(Position.X - vxUITheme.ArtProviderForMenuScreenItems.Padding.X/2), 
				(int)(Position.Y - vxUITheme.ArtProviderForMenuScreenItems.Padding.Y/2), 
				(int)(this.Font.MeasureString(Text).X + 2 * vxUITheme.ArtProviderForMenuScreenItems.Padding.X), 
				(int)(this.Font.MeasureString(Text).Y + 2 * vxUITheme.ArtProviderForMenuScreenItems.Padding.Y));

			Texture = vxUITheme.ArtProviderForMenuScreenItems.BackgroundImage;

            this.OnInitialHover += MenuEntry_OnInitialHover;

            this.ArtProvider = (vxMenuItemArtProvider)vxUITheme.ArtProviderForMenuScreenItems.Clone();
        }

        private void MenuEntry_OnInitialHover(object sender, EventArgs e)
		{
#if !NO_DRIVER_OPENAL
			//If Previous Selection = False and Current is True, then Create Highlite Sound Instsance

            PlaySound(vxUITheme.SoundEffects.MenuHover, MenuHoverVolume);
#endif
        }

        public static float MenuHoverVolume = 0.3f;
        public static float MenuConfirmVolume = 0.3f;
        

        /// <summary>
        /// Method for raising the Selected event.
        /// </summary>
        protected internal virtual void OnSelectEntry(PlayerIndex playerIndex)
		{
			try
			{
				if (Selected != null)
					Selected(this, new PlayerIndexEventArgs(playerIndex));
#if !NO_DRIVER_OPENAL
				PlaySound(vxUITheme.SoundEffects.MenuConfirm, 0.3f);
#endif

			}
			catch(Exception ex)
            {
                vxConsole.WriteError("Error Playing Menu Click");
                vxConsole.WriteException(this, ex);
            }
		}
        

        public virtual void SetArtProvider(vxMenuItemArtProvider NewArtProvider)
        {
            this.ArtProvider = (vxMenuItemArtProvider)NewArtProvider.Clone();
        }

        public override void Draw ()
		{
            Width = (int)(vxUITheme.Fonts.Size24.MeasureString(this.Text).X + vxUITheme.ArtProviderForMenuScreenItems.Padding.X * 2);
            Height = (int)(vxUITheme.Fonts.Size24.MeasureString(this.Text).Y + vxUITheme.ArtProviderForMenuScreenItems.Padding.Y * 2);

            this.ArtProvider.Draw(this);
		}
    }
}
