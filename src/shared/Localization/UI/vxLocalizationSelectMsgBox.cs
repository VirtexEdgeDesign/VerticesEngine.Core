using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VerticesEngine.UI;
using VerticesEngine.UI.Events;
using VerticesEngine.UI.MessageBoxs;

namespace VerticesEngine.Localization.UI
{
    /// <summary>
    /// Displays an about box with info about this game and the engine
    /// </summary>
    public class vxLocalizationSelectMsgBox : vxMessageBox
    {
        /// <summary>
        /// Shows an About Box
        /// </summary>
        public static vxLocalizationSelectMsgBox Show()
        {
            var aboutBox = new vxLocalizationSelectMsgBox();
            vxSceneManager.AddScene(aboutBox);
            return aboutBox;
        }


        vxLocComboBox m_locDropDown;

        private vxLocalizationSelectMsgBox() : base("", vxLocalizer.GetText(vxLocKeys.Localization_SetLanguage), vxEnumButtonTypes.OkCancel)
        {
            IsPopup = true;

            Message = vxLocalizer.GetText(vxLocKeys.Localization_SetLanguagePrompt) +"\n";
        }


        public override void LoadContent()
        {
            base.LoadContent();

            OKButton.Text = "OK";

            // Add the Localization Drop Down
            m_locDropDown = new vxLocComboBox();

            // now loop through all available lanugages
            foreach(var language in vxLocalizer.SupportedLangagues)
            {
                m_locDropDown.AddItem(language.Key);
            }

            InternalGUIManager.Add(m_locDropDown);

            //Message = ArtProvider.Font.WrapString(Message, vxScreen.Width / 2);
        }
        protected override void OnFirstDraw()
        {
            base.OnFirstDraw();

            m_locDropDown.Width = ArtProvider.FormBounds.Width - 2 * (int)ArtProvider.Padding.X;
            m_locDropDown.Position = new Vector2(ArtProvider.FormBounds.Center.X - m_locDropDown.Width/2, ArtProvider.TextPosition.Y + ArtProvider.TextSize.Y + Padding.Y);
            
            ArtProvider.ButtonBuffer = m_locDropDown.Height * 3;
        }

        public override void OnOKButtonClicked(object sender, vxUIControlClickEventArgs e)
        {
            base.OnOKButtonClicked(sender, e);

            foreach (var local in vxLocalizer.SupportedLangagues)
            {
                if(local.Value == m_locDropDown.SelectedItem.Text)
                {
                    vxLocalizer.SetLocalization(local.Key);
                }
            }
        }
    }
}
