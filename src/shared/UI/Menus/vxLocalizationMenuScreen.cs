using Microsoft.Xna.Framework;
using VerticesEngine.Input.Events;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.MessageBoxs;

namespace VerticesEngine.UI.Menus
{
    /// <summary>
    /// A basic menu for choosing a small set of languages
    /// </summary>
    public class vxLocalizationMenuScreen : vxMenuBaseScreen
    {
        bool IsShownOnLaunch = false;

        string tempLang = "";

        vxMenuEntry backMenuEntry;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Dialogs.vxLocalizationMenuScreen"/> class.
        /// </summary>
		public vxLocalizationMenuScreen(bool IsShownOnLaunch)
            : base(vxLocKeys.Localization_SetLanguage)
        {
            this.IsShownOnLaunch = IsShownOnLaunch;
		}


		public override void LoadContent()
		{
			base.LoadContent();

            foreach (var language in vxLocalizer.SupportedLangagues)
			{
				vxMenuEntry tempLanguageMenuEntry = new vxMenuEntry(this, language.Value);
                tempLanguageMenuEntry.Clicked += delegate
                {
                    vxMenuEntry menuentry = (vxMenuEntry)tempLanguageMenuEntry;

                    vxMessageBox confirmMsgBox = new vxMessageBox(string.Format(vxLocalizer.GetText(vxLocKeys.Localization_SetLanguageConfirmation), menuentry.Text), "Set Language");
                    confirmMsgBox.Accepted += ConfirmMsgBox_Accepted;
                    vxSceneManager.AddScene(confirmMsgBox);
                    tempLang = language.Key;
                };
                AddMenuItem(tempLanguageMenuEntry);
            }

            backMenuEntry = new vxMenuEntry(this, vxLocKeys.Back);
            backMenuEntry.Selected += new System.EventHandler<PlayerIndexEventArgs>(cancelMenuEntry_Selected);
            AddMenuItem(backMenuEntry);
        }

        public override void UnloadContent()
        {
            // if the initialise step is waiting, then kick it to the next level
            if (vxEngine.Game.InitializationStage == GameInitializationStage.Waiting)
            {
                vxEngine.Game.InitializationStage = GameInitializationStage.CheckIfUpdated;
            }
            base.UnloadContent();
        }

        #region Events


        void ConfirmMsgBox_Accepted(object sender, PlayerIndexEventArgs e)
        {
            vxLocalizer.SetLocalization(tempLang);
            vxSceneManager.AddScene(new vxLocalizationUpdateScreen(tempLang));

            this.OnCancel(PlayerIndex.One);
        }

        void cancelMenuEntry_Selected(object sender, PlayerIndexEventArgs e)
        {
            ExitScreen();
        }

        #endregion
    }
}
