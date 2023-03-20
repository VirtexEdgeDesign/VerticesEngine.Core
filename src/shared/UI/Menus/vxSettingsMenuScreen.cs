#region Using Statements
using Microsoft.Xna.Framework;
using VerticesEngine.Input.Events;
using VerticesEngine.Localization.UI;
using VerticesEngine.Monetization.Purchases;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Dialogs;
using VerticesEngine.UI.MessageBoxs;


#endregion

namespace VerticesEngine.UI.Menus
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    public class vxSettingsMenuScreen : vxMenuBaseScreen
    {
        #region Initialization

        vxMenuEntry ControlsMenuEntry;
        vxMenuEntry LocalizationMenuEntry;
        vxMenuEntry ProfileMenuEntry;
        vxMenuEntry GraphicsMenuEntry;
        vxMenuEntry AudioMenuEntry;
        vxMenuEntry CreditsMenuEntry;
        vxMenuEntry restorePurchasesMenuEntry;
        
        vxMenuEntry displayDebugMethodsMenuEntry;
        vxMenuEntry AboutMsgBoxMenuEntry;
        vxMenuEntry cancelMenuEntry;

        public vxSettingsMenuScreen() : this(ControllerNavFlow.Vertical)
        {

        }

        public vxSettingsMenuScreen(ControllerNavFlow navFlow) : base(vxLocKeys.Settings, navFlow)
        {

        }


        public override void LoadContent()
        {
            base.LoadContent();

            // Create our menu entries.
            ProfileMenuEntry = new vxMenuEntry(this, vxLocKeys.Profile);
            ControlsMenuEntry = new vxMenuEntry(this, vxLocKeys.Settings_Controls);
            LocalizationMenuEntry = new vxMenuEntry(this, vxLocKeys.Settings_Localisation);
            GraphicsMenuEntry = new vxMenuEntry(this, vxLocKeys.Settings_Graphics);
            AudioMenuEntry = new vxMenuEntry(this, vxLocKeys.Settings_Audio);
            CreditsMenuEntry = new vxMenuEntry(this, vxLocKeys.Credits);
            restorePurchasesMenuEntry = new vxMenuEntry(this, vxLocKeys.Profile_RestorePurchases);
            
            displayDebugMethodsMenuEntry = new vxMenuEntry(this, vxLocKeys.Debug);

            AboutMsgBoxMenuEntry = new vxMenuEntry(this, vxLocKeys.Sandbox_About);

            cancelMenuEntry = new vxMenuEntry(this, vxLocKeys.Back);

            // Hook up menu event handlers.
            ProfileMenuEntry.Selected += ProfileMenuEntry_Selected;
            ControlsMenuEntry.Selected += new System.EventHandler<PlayerIndexEventArgs>(ControlsMenuEntry_Selected);
            GraphicsMenuEntry.Selected += new System.EventHandler<PlayerIndexEventArgs>(GraphicsMenuEntry_Selected);
            LocalizationMenuEntry.Selected += LocalizationMenuEntry_Selected;
            AudioMenuEntry.Selected += new System.EventHandler<PlayerIndexEventArgs>(AudioMenuEntry_Selected);
            
            displayDebugMethodsMenuEntry.Selected += DisplayDebugMethodsMenuEntry_Selected; ;
            CreditsMenuEntry.Selected += CreditsMenuEntry_Selected;
            restorePurchasesMenuEntry.Selected += RestorePurchasesMenuEntry_Selected;
            AboutMsgBoxMenuEntry.Selected += AboutMsgBoxMenuEntry_Selected;
            //Back
            cancelMenuEntry.Selected += new System.EventHandler<PlayerIndexEventArgs>(cancelMenuEntry_Selected);


            // Add entries to the menu.

            // TODO: Re-ad
            if (vxEngine.Game.HasProfileSupport)
                AddMenuItem(ProfileMenuEntry);

            if (vxEngine.Game.HasInAppPurchases)
                AddMenuItem(restorePurchasesMenuEntry);

            // TODO: Re-ad
            //AddMenuItem(CreditsMenuEntry);

            if (vxEngine.Game.HasControlsSettings)
                AddMenuItem(ControlsMenuEntry);

            if (vxEngine.Game.HasLanguageSettings)
                AddMenuItem(LocalizationMenuEntry);

            if (vxEngine.Game.HasGraphicsSettings)
                AddMenuItem(GraphicsMenuEntry);

            if (vxEngine.Game.HasAudioSettings)
                AddMenuItem(AudioMenuEntry);

            if (vxEngine.Game.IsDebugMenuAvailable)
            {
                AddMenuItem(displayDebugMethodsMenuEntry);
            }


            // Add any extra settings the player wants
            vxEngine.Game.AddSettingsScreens(this);

            AddMenuItem(AboutMsgBoxMenuEntry);

            AddMenuItem(cancelMenuEntry);
        }


        #endregion

        #region Events

        void ProfileMenuEntry_Selected(object sender, PlayerIndexEventArgs e)
        {
            vxEngine.Game.OnShowProfileSettings();
        }

        void ControlsMenuEntry_Selected(object sender, PlayerIndexEventArgs e)
        {
            vxEngine.Game.OnShowControlsSettings();
        }

        void GraphicsMenuEntry_Selected(object sender, PlayerIndexEventArgs e)
        {
            vxSceneManager.AddScene(new vxGraphicSettingsDialog(), e.PlayerIndex);
        }

        private void LocalizationMenuEntry_Selected(object sender, PlayerIndexEventArgs e)
        {
            vxLocalizationSelectMsgBox.Show();
            //vxSceneManager.AddScene(new vxLocalizationMenuScreen(false), e.PlayerIndex);
        }

        void AudioMenuEntry_Selected(object sender, PlayerIndexEventArgs e)
        {
            vxSceneManager.AddScene(new vxAudioMenuScreen(), e.PlayerIndex);
        }
        private void DisplayDebugMethodsMenuEntry_Selected(object sender, PlayerIndexEventArgs e)
        {
            vxSceneManager.AddScene(new vxDebugMenuScreen(), e.PlayerIndex);
        }

        private void RestorePurchasesMenuEntry_Selected(object sender, PlayerIndexEventArgs e)
        {
            vxNotificationManager.Show("Restoring Purchases...", Color.Magenta);
            vxInAppProductManager.Instance.RestorePurchases();
        }

        private void AboutMsgBoxMenuEntry_Selected(object sender, PlayerIndexEventArgs e)
        {
            vxAboutBox.Show();
        }

        void CreditsMenuEntry_Selected(object sender, PlayerIndexEventArgs e)
        {
            vxEngine.Game.OnShowCreditsPage();
        }

        void cancelMenuEntry_Selected(object sender, PlayerIndexEventArgs e)
        {
            ExitScreen();
        }

        #endregion
    }
}
