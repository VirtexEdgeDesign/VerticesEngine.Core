
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Input.Events;
using VerticesEngine;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Dialogs.Utilities;
using VerticesEngine.Diagnostics;
using VerticesEngine.UI.Menus;
using VerticesEngine.Monetization.Ads;


#if __ANDROID__
using Android.App;
using Android.Content;
using Android.Views;
using Android.Views.InputMethods;
#endif



namespace VerticesEngine.UI.Dialogs
{
    /// <summary>
    /// This is the Debug Settings Screen
    /// </summary>
    public class vxDebugMenuScreen : vxMenuBaseScreen
    {
        #region Fields
        vxMenuEntry displayDebugHUDMenuEntry;
        vxMenuEntry showInterstitialAd;
        vxMenuEntry showRewardAd;
        vxMenuEntry showAndroidDialog;
        vxMenuEntry testCrashHandling;
        vxMenuEntry cancelMenuEntry;
        
        #endregion



        /// <summary>
        /// Constructor.
        /// </summary>
        public vxDebugMenuScreen() : base("Debug Methods")
        {
           
        }

        public override void LoadContent()
        {
            base.LoadContent();

            displayDebugHUDMenuEntry = new vxMenuEntry(this, "Debug Settings");
            displayDebugHUDMenuEntry.Selected += DisplayDebugHUDMenuEntry_Selected;
            AddMenuItem(displayDebugHUDMenuEntry);
            
            showInterstitialAd = new vxMenuEntry(this, "Show Init Ad");
            showInterstitialAd.Selected += ShowInterstitialAd_Selected; ;
            

            showRewardAd = new vxMenuEntry(this, "Show Reward Ad");
            showRewardAd.Selected += ShowRewardAd_Selected; ; ;

            showAndroidDialog = new vxMenuEntry(this, "Show Dialog");
            showAndroidDialog.Selected += displayDebugHUDMenuEntry_Selected;
            
            if (vxEngine.PlatformType == vxPlatformHardwareType.Mobile)
            {
                AddMenuItem(showRewardAd);
                AddMenuItem(showInterstitialAd);
                AddMenuItem(showAndroidDialog);
            }
            

            testCrashHandling = new vxMenuEntry(this, "Test Crash");
            testCrashHandling.Selected += TestCrashHandling_Selected;
            AddMenuItem(testCrashHandling);

            cancelMenuEntry = new vxMenuEntry(this, vxLocKeys.Back);
            cancelMenuEntry.Selected += new System.EventHandler<PlayerIndexEventArgs>(cancelMenuEntry_Selected);
            AddMenuItem(cancelMenuEntry);
        }

        private bool isCrashing = false;

        private void TestCrashHandling_Selected(object sender, PlayerIndexEventArgs e)
        {
            isCrashing = true;
        }

        private void DisplayDebugHUDMenuEntry_Selected(object sender, PlayerIndexEventArgs e)
        {
            vxSceneManager.AddScene(new vxDebugSettingsDialog(), e.PlayerIndex);
        }

        private void ShowInterstitialAd_Selected(object sender, PlayerIndexEventArgs e)
        {
            vxConsole.WriteLine("Showing INiT Ad");
            vxAdManager.ShowInterstitial();
        }

        private void ShowRewardAd_Selected(object sender, PlayerIndexEventArgs e)
        {
            vxConsole.WriteLine("Showing Reward Ad");
            vxAdManager.ShowRewardAd();
        }

        void cancelMenuEntry_Selected(object sender, PlayerIndexEventArgs e)
        {
            ExitScreen();
        }

        protected internal override void Update()
        {
            base.Update();

            if(isCrashing)
            {
                throw new System.Exception("Test Debug Crash");
            }
        }
        
        
        void displayDebugHUDMenuEntry_Selected(object sender, PlayerIndexEventArgs e)
        {

#if __ANDROID__L
            
            /*
            InputMethodManager imm = (InputMethodManager)Engine.Activity.GetSystemService(Activity.INPUT_METHOD_SERVICE);
            //Find the currently focused view, so we can grab the correct window token from it.
            View view = activity.getCurrentFocus();
            //If no view currently has focus, create a new one, just so we can grab a window token from it
            if (view == null)
            {
                view = new View(activity);
            }
            imm.hideSoftInputFromWindow(view.getWindowToken(), 0);
            */
            var pView = vxEngine.Game.Services.GetService<View>();
            var inputMethodManager = Engine.Activity.GetSystemService(Context.InputMethodService) as InputMethodManager;
            inputMethodManager.ShowSoftInput(pView, ShowFlags.Forced);
            inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);
#endif

        }
    }
}
