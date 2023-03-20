using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Utilities;
using VerticesEngine;
using VerticesEngine.UI.Dialogs;
using VerticesEngine.UI.Themes;
using VerticesEngine.Graphics;
using System.IO;
using VerticesEngine.UI.MessageBoxs;

namespace VerticesEngine.UI.Controls
{
    /// <summary>
    ///  <see cref="VerticesEngine.UI.Controls.vxManageImportedEntityUIItem"/> control which allows for spinning through options
    /// </summary>
	public class vxManageImportedEntityUIItem : vxScrollPanelItem
    {
        /// <summary>
        /// Refresh button
        /// </summary>
        private  vxButtonImageControl RefreshButton
        {
            get { return m_refreshButton; }
        }
        vxButtonImageControl m_refreshButton;

        private vxButtonImageControl RemoveItemButton
        {
            get { return m_removeItemButton; }
        }
        vxButtonImageControl m_removeItemButton;

        /// <summary>
        /// File info for the current 
        /// </summary>
        private FileInfo m_fileInfo;

        /// <summary>
        /// The imported file that this ui item represents
        /// </summary>
        private ImportedFileInfo m_importedFileInfo;


        private Vector2 m_titlePos = Vector2.Zero;

        private Vector2 m_filePathTxtPos = Vector2.Zero;

        private readonly bool m_isLocalFileAvailable = false;

        private string m_filePathTxt = "<file_path>";

        private Texture2D m_icon;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxManageImportedEntityUIItem"/> class.
        /// </summary>
        /// <param name="UIManager">GUIM anager.</param>
        /// <param name="importedFileInfo">Title.</param>
        public vxManageImportedEntityUIItem(vxManageImportedEntitiesDialog dialog, ImportedFileInfo importedFileInfo) : base("", Vector2.Zero, null, 0)
        {
            m_importedFileInfo = importedFileInfo;
            m_icon = importedFileInfo.Icon;

            m_fileInfo = new FileInfo(importedFileInfo.ExternalFilePath);
            if(File.Exists(importedFileInfo.ExternalFilePath))
            {
                m_isLocalFileAvailable = true;
                m_fileInfo = new FileInfo(importedFileInfo.ExternalFilePath);

                Text = m_fileInfo.Name;
            }
            else
            {
                Text = "FILE NOT FOUND";
            }
            m_filePathTxt = "File Path: " + importedFileInfo.ExternalFilePath;

            Height = vxLayout.GetScaledHeight(64);

            //Set GUI Stuff.
            int ofst = vxLayout.GetScaledSize(8);

            int btnSize = Height - ofst * 2;

            m_refreshButton = new vxButtonImageControl(vxUITheme.SpriteSheetLoc.Refresh, Vector2.Zero)
            {
                Width = btnSize,
                Height = btnSize
            };
            m_refreshButton.Clicked += delegate {

                if (m_isLocalFileAvailable)
                {
                    var refreshItemMsgBx = vxMessageBox.Show("Refresh Local File", "Would you like to refresh the imported file with the original?", vxEnumButtonTypes.OkCancel);
                    refreshItemMsgBx.Accepted += delegate
                    {
                        vxConsole.WriteLine($"Refreshing imported guid {importedFileInfo.guid} at location '{importedFileInfo.ExternalFilePath}'");
                        vxEngine.Instance.GetCurrentScene<vxGameplayScene3D>().RefreshImportedFile(importedFileInfo.guid, importedFileInfo.ExternalFilePath);
                    };
                }
                else
                {
                    vxMessageBox.Show("File Not Found", "Can't find the original imported file at:\n" + m_importedFileInfo.ExternalFilePath);
                }
            
            };

            m_removeItemButton = new vxButtonImageControl(vxUITheme.SpriteSheetLoc.Delete, Vector2.Zero)
            {
                Width = btnSize,
                Height = btnSize
            };
            m_removeItemButton.Clicked += delegate {

                if (m_isLocalFileAvailable)
                {
                    var removeItemMsgBx = vxMessageBox.Show("Remove Imported File", "Would you like to remove the imported file from this level?"+
                        "\n\nImportant:" +
                        "\n     - This will delete all instances of the object currently." +
                        "\n     - This cannot be undone!"
                        , vxEnumButtonTypes.OkCancel);
                    removeItemMsgBx.Accepted += delegate
                    {
                        try
                        {
                            vxEngine.Instance.GetCurrentScene<vxGameplayScene3D>().DeleteImportedEntity(importedFileInfo.guid);
                            dialog.ExitScreen();

                            var manageImportedEntitiesDialog = new vxManageImportedEntitiesDialog(dialog.Level);
                            vxSceneManager.AddScene(manageImportedEntitiesDialog);
                        }
                        catch(Exception ex)
                        {
                            vxConsole.WriteException("Error Deleting Imported File", ex);
                            vxMessageBox.Show("Could not delete file", "Could not delete imported file for guid :\n" + importedFileInfo.guid);
                        }
                    };
                }
                else
                {
                    vxMessageBox.Show("File Not Found", "Can't find the original imported file at:\n" + m_importedFileInfo.ExternalFilePath);
                }
            };


            // Set the Padding and Sizes
            this.Font = vxUITheme.Fonts.Size24;

            Bounds = new Rectangle(0, 0, (int)(vxLayout.Scale.X * 64), Height);
            Padding = new Vector2((int)(vxLayout.Scale.X * 16), (Bounds.Height / 2 - m_refreshButton.Bounds.Height / 2));

            Theme.Background.HoverColour = Color.DarkOrange;
        }



        protected internal override void Update()
        {
            base.Update();

            // Left Justified
            m_titlePos = new Vector2(Bounds.Left + Padding.X, Position.Y + Height / 2);


            // Set the Position walking left from the right side
            m_refreshButton.Position = new Vector2(Bounds.Right - (Padding.X + m_refreshButton.Width)*2, Position.Y + Padding.Y);
            m_refreshButton.Update();

            m_removeItemButton.Position = new Vector2(Bounds.Right - Padding.X - m_refreshButton.Width, Position.Y + Padding.Y);
            m_removeItemButton.Update();
        }


        /// <summary>
        /// Draws the GUI Item
        /// </summary>
        public override void Draw()
        {
            var imgSize = Height - 4;

            m_titlePos = new Vector2(Bounds.Left + Padding.X + Height, Position.Y + Padding.Y * 2);

            //base.Draw();
            vxGraphics.SpriteBatch.Draw(DefaultTexture, Bounds.GetBorder(1), Color.Black);
            vxGraphics.SpriteBatch.Draw(DefaultTexture, Bounds, Theme.Background.Color);

            // draw icon
            var imgRect = vxLayout.GetRect(Bounds.Location + new Point(2), imgSize);
            SpriteBatch.Draw(vxInternalAssets.Textures.Gradient, imgRect, Color.Black);
            SpriteBatch.Draw(m_icon, imgRect.GetBorder(-1), m_isLocalFileAvailable ? Color.White : Color.Red);


            // draw file name
            var titleFont = vxUITheme.Fonts.Size24;
            SpriteBatch.DrawString(titleFont, Text, m_titlePos + Vector2.One * 2, (HasFocus ? Color.Black * 0.75f : Color.Gray * 0.5f), vxLayout.ScaleAvg, vxHorizontalJustification.Left, vxVerticalJustification.Middle);
            SpriteBatch.DrawString(titleFont, Text, m_titlePos, Color.White, vxLayout.ScaleAvg, vxHorizontalJustification.Left, vxVerticalJustification.Middle);

            // draw file guid
            var detailFont = vxInternalAssets.Fonts.ViewerFont;
            int hPadding = 4;
            float fade = 0.5f;
            m_filePathTxtPos = new Vector2(Bounds.Left + Padding.X + Height, Bounds.Bottom - detailFont.LineSpacing * 1 - hPadding);
            SpriteBatch.DrawString(detailFont, m_importedFileInfo.guid, m_filePathTxtPos, Color.White * fade);

            // draw file path
            m_filePathTxtPos = new Vector2(Bounds.Left + Bounds.Width / 3, Bounds.Bottom - detailFont.LineSpacing * 1 - hPadding);
            SpriteBatch.DrawString(detailFont, m_filePathTxt, m_filePathTxtPos, Color.White * fade);

            // draw file size
            m_filePathTxtPos = new Vector2(Bounds.Left + Bounds.Width * 3 / 4, Bounds.Bottom - detailFont.LineSpacing * 1 - hPadding);
            if (m_fileInfo != null)
            {
                SpriteBatch.DrawString(detailFont, (m_fileInfo.Length / (1024.0f*1024.0f)).ToString("##0.00") + " MB", m_filePathTxtPos, Color.White * fade);
            }
            else
            {
                SpriteBatch.DrawString(detailFont, "[File Not Found]", m_filePathTxtPos, Color.Red);
            }

            //m_refreshButton.Position = new Vector2(Bounds.Right - Padding.X - m_refreshButton.Width, Position.Y + Padding.Y);
            m_removeItemButton.Draw();
            m_refreshButton.Draw();
        }
    }
}
