using Microsoft.Xna.Framework;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Events;

namespace VerticesEngine.UI.Dialogs
{
    /// <summary>
    /// A popup message box screen, used to display "are you sure?"
    /// confirmation messages.
    /// </summary>
    public class vxLocalizationDialog : vxDialogBase
    {

        /// <summary>
        /// The Graphics Settings Dialog
        /// </summary>
        public vxLocalizationDialog()
			: base("Localization", vxEnumButtonTypes.OkApplyCancel)
        {

        }

        public override void LoadContent()
        {
            base.LoadContent();


            //Full Screen
            /*****************************************************************************************************/
            vxComboBox combo = new vxComboBox(vxLocalizer.CurrentLanguage,
                                              new Vector2(this.ArtProvider.GUIBounds.X, this.ArtProvider.GUIBounds.Y));
            
            InternalGUIManager.Add(combo);
			
            //Add in languages
            foreach (var language in vxLocalizer.SupportedLangagues)
            {
                combo.AddItem(language.Value);
            }

            combo.SelectionChanged += delegate (object sender, vxComboBoxSelectionChangedEventArgs e) {

                foreach (var language in vxLocalizer.SupportedLangagues)
                {
                    if (e.SelectedItem.Text == language.Value)
                    {
                        vxLocalizer.SetLocalization(language.Key);

                        vxConsole.InternalWriteLine("Setting Language to: " + language.Key + " - " + language.Value);
                    }
                }
            };
        }

        protected override void OnApplyButtonClicked(object sender, vxUIControlClickEventArgs e)
		{
            SetSettings();

        }

        protected override void OnOKButtonClicked(object sender, vxUIControlClickEventArgs e)
        {
            SetSettings();
            ExitScreen();
        }

        void SetSettings()
        {
            vxConsole.WriteLine("Setting Language Keys");


            foreach(vxBaseScene screen in vxSceneManager.GetScreens())
            {
                screen.OnLocalizationChanged();
            }

            //Save Settings
            vxSettings.Save();
        }
    }
}
