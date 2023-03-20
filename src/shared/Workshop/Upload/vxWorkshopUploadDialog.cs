/**
 * @file
 * @brief Opens a Workshop Upload Dialog which allows the user to upload custom levels to a hosting solution like Steam Workshop or Firebase
 *
 * Here typically goes a more extensive explanation of what the header
 * defines. Doxygens tags are words preceeded by either a backslash @\
 * or by an at symbol @@.
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Graphics;
using VerticesEngine.UI;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Dialogs;
using VerticesEngine.UI.Events;
using VerticesEngine.UI.Themes;

namespace VerticesEngine.Workshop
{
    /// <summary>
    /// Opens a Workshop Upload Dialog which allows the user to upload custom levels to a hosting solution like Steam Workshop or Firebase
    /// </summary>
    public class vxWorkshopUploadDialog : vxDialogBase
    {
        private vxGameplaySceneBase Level;

        private vxImage levelScreenshot;

        private Texture2D previewImage;

        public vxWorkshopUploadDialog(vxGameplaySceneBase Level)
        : base("Upload to Workshop", vxEnumButtonTypes.OkCancel)
        {
            this.Level = Level;
            this.previewImage = Level.PreviewImage;
        }

        public override Vector2 GetBoundarySize()
        {
            return new Vector2(vxGraphics.GraphicsDevice.Viewport.Width, vxGraphics.GraphicsDevice.Viewport.Height);
        }


        public override void LoadContent()
        {
            base.LoadContent();

            Vector2 Padding = new Vector2(20);

            Vector2 LeftStart = ArtProvider.GUIBounds.Location.ToVector2() + Padding;

            // Handle the Level Screenshot
            levelScreenshot = new vxImage(this.previewImage, ArtProvider.GUIBounds.Location.ToVector2() + Padding);

            levelScreenshot.Width = (int)(ArtProvider.GUIBounds.Width / 2 - Padding.X * 2);
            levelScreenshot.Height = levelScreenshot.Width * 9 / 16;
            InternalGUIManager.Add(levelScreenshot);


            LeftStart += Vector2.UnitY * (levelScreenshot.Height + 24);

            // Handle the Level Title
            vxLabel LevelTitleLabel = new vxLabel("Steam Workshop Upload", LeftStart);
            LevelTitleLabel.Font = vxUITheme.Fonts.Size36;
            InternalGUIManager.Add(LevelTitleLabel);



            var descFont = vxUITheme.Fonts.Size12;
            var descTxt = descFont.WrapString("Upload and update your creation and sandbox files here to Steam Workshop", ArtProvider.GUIBounds.Width / 2);

            // Handle the Level Desciprtion
            vxLabel LevelDescripLabel = new vxLabel(descTxt, LeftStart + new Vector2(0, 30));
            
            InternalGUIManager.Add(LevelDescripLabel);

            this.OKButton.Text = "Upload";


            Vector2 RightTextCol = ArtProvider.GUIBounds.Location.ToVector2() +
                                            new Vector2(ArtProvider.GUIBounds.Width / 2, 24);


            vxScrollPanel levelSettingsScrollPanel = new vxScrollPanel(RightTextCol + new Vector2(0, 24), ArtProvider.GUIBounds.Width / 2, ArtProvider.GUIBounds.Height - 128);

            InternalGUIManager.Add(levelSettingsScrollPanel);

            InternalGUIManager.Add(new vxLabel("Track Settings", RightTextCol));



            // Handle the Track Theme
            // *****************************************************************************************************

            //Full Screen
            // *****************************************************************************************************
            levelSettingsScrollPanel.AddItem(new vxSettingsReadOnlyGUIItem(InternalGUIManager, "Title", Level.Title));
            levelSettingsScrollPanel.AddItem(new vxSettingsReadOnlyGUIItem(InternalGUIManager, "Author", Level.PlayerProfile.Name));
            levelSettingsScrollPanel.AddItem(new vxSettingsReadOnlyGUIItem(InternalGUIManager, "Revision", Level.SandBoxFile.FileReversion.ToString()));
            //levelSettingsScrollPanel.AddItem(new vxSettingsReadOnlyGUIItem(InternalGUIManager, "Workshop ID", Level.WorkshopID.ToString()));

            var idFont = vxUITheme.Fonts.Size12;
            var idTxt = "ID:"+Level.WorkshopID?.ToString();
            
            // Handle the Level Desciprtion            
            InternalGUIManager.Add(new vxLabel(idTxt, new Vector2(ArtProvider.GUIBounds.X, ArtProvider.GUIBounds.Bottom - idFont.LineSpacing)));
        }

        /// <inheritdoc/>
        protected override void OnOKButtonClicked(object sender, vxUIControlClickEventArgs e)
        {
            vxSceneManager.AddScene(new vxWorkshopUploadWorker(Level));

            base.OnOKButtonClicked(sender, e);
        }
    }
}