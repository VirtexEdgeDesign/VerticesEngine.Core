using System;
using VerticesEngine;
using VerticesEngine.Utilities;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using VerticesEngine.UI.Controls;
using VerticesEngine.Graphics;
using VerticesEngine.Workshop.UI;

namespace VerticesEngine.UI.Themes
{
    /// <summary>
    /// Vx GUI theme.
    /// </summary>
    public class vxUITheme
    {
        static string PathTooFiles = "Gui/DfltThm/";

        /// <summary>
        /// The GUI sprite sheet.
        /// </summary>
        public static Texture2D SpriteSheet;

        /*******************************************/
        //					MASTER VALUES
        /*******************************************/

        /// <summary>
        /// The font pack. Note that this is not used by the engine internally and
        /// must be intitalised outside of it.
        /// </summary>
        public static vxFontPack Fonts;

        /// <summary>
        /// Gets or sets the default padding for all GUI items
        /// </summary>
        /// <value>The padding.</value>
        public Vector2 Padding = new Vector2(10, 10);

        //Misc
        //public Texture2D SplitterTexture;

        /*******************************************/
        //				ART PROVIDERS
        /*******************************************/
        public static vxButtonArtProvider ArtProviderForButtons;
        public static vxLabelArtProvider ArtProviderForLabels;
        public static vxTabPageTabArtProvider ArtProviderForTabs;
        public static vxTextboxArtProvider ArtProviderForTextboxes;
        public static vxScrollPanelArtProvider ArtProviderForScrollPanel;
        public static vxScrollPanelItemArtProvider ArtProviderForScrollPanelItem;
        public static vxScrollBarArtProvider ArtProviderForScrollBars;
        public static vxFileDialoglItemArtProvider ArtProviderForFileDialogItem;
        public static vxModDialoglItemArtProvider ArtProviderForModDialogItem;
        public static vxWorkshopDialogItemArtProvider ArtProviderForWorkshopDialogItem;
        public static vxFileExplorerItemArtProvider ArtProviderForFileExplorerItem;
        public static vxMenuScreenArtProvider ArtProviderForMenuScreen;
        public static vxMenuItemArtProvider ArtProviderForMenuScreenItems;
        public static vxMessageBoxArtProvider ArtProviderForMessageBoxes;
        public static vxSliderArtProvider ArtProviderForSlider;
        public static vxDialogArtProvider ArtProviderForDialogs;
        public static vxToolbarArtProvider ArtProviderForToolbars;
        public static vxSlidePageTabArtProvider ArtProviderForSlidePageTab;
        public static vxSpinnerControlArtProvider ArtProviderForSpinners;
        public static vxToolTipArtProvider ArtProviderForToolTips;
        public static vxLoadingScreenRenderer LoadingScreenRenderer;

        /*******************************************/
        //					LABEL
        /*******************************************/
        //public Color vxLabelColorNormal = Color.White;

        /// <summary>
        /// The Sprite Sheet Locations for Common GUI Items such as Arrows
        /// </summary>
        public static class SpriteSheetLoc
        {
            public static Rectangle BlankLoc;
            public static Rectangle ArrowBtnBack;
            public static Rectangle ArrowBtnFwd;
            public static Rectangle ToggleOn;
            public static Rectangle ToggleOff;
            public static Rectangle Refresh;
            public static Rectangle Delete;
        }




        /*******************************************/
        //					Sound Effects
        /*******************************************/
#if !NO_DRIVER_OPENAL

        public static class SoundEffects
        {
            public static SoundEffect MenuHover;
            public static SoundEffect MenuConfirm;
            public static SoundEffect MenuCancel;
        }

#endif

        public static vxUIControlTheme SelectedItemTheme;

        public static vxUIControlTheme UnselectedItemTheme;

        public vxUIControlTheme DropDownItemTheme;

        public static string FontRootPath = "Fonts";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Themes.vxUITheme"/> class.
        /// </summary>
        /// <param name="Engine">Engine.</param>
        public vxUITheme()
        {
            string regionKey = vxSettings.Language != string.Empty ? vxSettings.Language : "en";
            try
            {
                vxUITheme.Fonts = new VerticesEngine.UI.vxFontPack(FontRootPath, regionKey);
            }
            catch
            {
                vxSettings.Language = "en";
                vxUITheme.Fonts = new VerticesEngine.UI.vxFontPack(FontRootPath, vxSettings.Language);
            }
            Padding = vxLayout.Scale * Padding;

            SpriteSheet = vxInternalAssets.Textures.Blank;

            //SetDefaultTheme();
            //Fonts.Font = vxInternalAssets.Fonts.MenuFont;
            //Fonts.FontTitle = vxInternalAssets.Fonts.MenuTitleFont;
            //Fonts.FontSmall = vxInternalAssets.Fonts.ViewerFont;



            // Sound Effects
            /*******************************************/
            SoundEffects.MenuHover = vxInternalAssets.SoundEffects.MenuClick; //Engine.InternalContentManager.Load<SoundEffect>("Gui/DfltThm/vxUITheme/SndFx/Menu/Click/Menu_Click");
            SoundEffects.MenuConfirm = vxInternalAssets.SoundEffects.MenuConfirm;// Engine.InternalContentManager.Load<SoundEffect>("Gui/DfltThm/vxUITheme/SndFx/Menu/MenuConfirm");
            SoundEffects.MenuCancel = vxInternalAssets.SoundEffects.MenuError;//Engine.InternalContentManager.Load<SoundEffect>("Gui/DfltThm/vxUITheme/SndFx/Menu/MenuError");


            Color bckgrndCol = new Color(50, 50, 50, 255);
            Color foregrndCol = Color.DeepSkyBlue;

            SelectedItemTheme = new vxUIControlTheme(
            new vxColourTheme(foregrndCol, foregrndCol, foregrndCol),
                new vxColourTheme(bckgrndCol, bckgrndCol, bckgrndCol));


            UnselectedItemTheme = new vxUIControlTheme(
            new vxColourTheme(bckgrndCol, bckgrndCol * 1.75f, bckgrndCol),
            new vxColourTheme(foregrndCol, foregrndCol * 1.75f, foregrndCol));


            Color dDIThmBack = Color.Black * 0.75f;
            DropDownItemTheme = new vxUIControlTheme(
            new vxColourTheme(dDIThmBack, Color.DarkOrange, dDIThmBack),
            new vxColourTheme(foregrndCol, Color.Black, foregrndCol));
        }


        public void SetDefaultTheme()
        {
            //Initialise Art Providers
            ArtProviderForButtons = new vxButtonArtProvider();
            ArtProviderForLabels = new vxLabelArtProvider();
            ArtProviderForTabs = new vxTabPageTabArtProvider();
            ArtProviderForTextboxes = new vxTextboxArtProvider();
            ArtProviderForScrollPanel = new vxScrollPanelArtProvider();
            ArtProviderForScrollPanelItem = new vxScrollPanelItemArtProvider();
            ArtProviderForScrollBars = new vxScrollBarArtProvider();
            ArtProviderForFileDialogItem = new vxFileDialoglItemArtProvider();
            ArtProviderForModDialogItem = new vxModDialoglItemArtProvider();
            ArtProviderForWorkshopDialogItem = new vxWorkshopDialogItemArtProvider();
            ArtProviderForFileExplorerItem = new vxFileExplorerItemArtProvider();
            ArtProviderForMenuScreen = new vxMenuScreenArtProvider();
            ArtProviderForMenuScreenItems = new vxMenuItemArtProvider();
            ArtProviderForMessageBoxes = new vxMessageBoxArtProvider();
            ArtProviderForSlider = new vxSliderArtProvider();
            ArtProviderForDialogs = new vxDialogArtProvider();
            ArtProviderForToolbars = new vxToolbarArtProvider();
            ArtProviderForSlidePageTab = new vxSlidePageTabArtProvider();
            ArtProviderForSpinners = new vxSpinnerControlArtProvider();
            ArtProviderForToolTips = new vxToolTipArtProvider();
            LoadingScreenRenderer = new vxLoadingScreenRenderer();
        }

        /// <summary>
        /// Loads the gui sprite sheet.
        /// </summary>
        /// <param name="path">Path.</param>
        public static void LoadSpriteSheet(bool IsXNB = true, string path = "Textures/Gui/GUISpriteSheet")
        {
            // is the sprite sheet a compiled XNB?
            if (IsXNB)
            {
                SpriteSheet = vxEngine.Game.Content.Load<Texture2D>(path);
            }
            // sometimes the Spritesheet may be supplied as a 'png' file to cutdown on file space as a 1024x1024 image can be
            // ~300 kb as a png and 4,000 kb as an 'xnb'.
            else
            {
#if __ANDROID__
                Stream fileStream = Game.Activity.Assets.Open("Content/" + path + ".png");
                SpriteSheet = Texture2D.FromStream(vxGraphics.GraphicsDevice, fileStream);
#else
                using (var fileStream = new System.IO.FileStream("Content/" + path + ".png", System.IO.FileMode.Open))
                {
                    SpriteSheet = Texture2D.FromStream(vxGraphics.GraphicsDevice, fileStream);
                }
#endif
			}
        }


        /// <summary>
        /// Loads the texture.
        /// </summary>
        /// <returns>The texture.</returns>
        /// <param name="path">Path.</param>
        public static Texture2D LoadTexture(ContentManager contentManager, string path)
		{
			try{
                return contentManager.Load<Texture2D>(PathTooFiles + path);
			}
			catch(Exception ex){
				vxConsole.WriteException ("vxUITheme",ex);

				return vxInternalAssets.Textures.DefaultDiffuse;
			}
		}
	}

}

