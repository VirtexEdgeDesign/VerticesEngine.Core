#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Input.Events;
using VerticesEngine;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Dialogs;
using VerticesEngine.Profile;
using System;
using VerticesEngine.Monetization.Purchases;


#endregion

namespace VerticesEngine.UI.Menus
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class vxProfileMenuScreen : vxMenuBaseScreen
    {
        #region Initialization

        /// <summary>
        /// Gets the profile.
        /// </summary>
        /// <value>The profile.</value>
        vxIPlayerProfile Profile
        {
            get { return vxPlatform.Player; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public vxProfileMenuScreen()
            : base("Profile")
        {

        }
        vxMenuEntry signOutMenuEntry;
        vxMenuEntry ViewLeaderboardsMenuEntry;
        vxMenuEntry ViewAchievementsMenuEntry;
        vxMenuEntry RestorePurchasesMenuEntry;

        public override void LoadContent()
        {
            base.LoadContent();

            ViewLeaderboardsMenuEntry = new vxMenuEntry(this, vxLocKeys.Profile_ViewLeaderboards);
            ViewAchievementsMenuEntry = new vxMenuEntry(this, vxLocKeys.Profile_ViewAchievements);
            RestorePurchasesMenuEntry = new vxMenuEntry(this, vxLocKeys.Profile_RestorePurchases);
            signOutMenuEntry = new vxMenuEntry(this, vxLocKeys.Network_User_SignIn);

            if (vxPlatform.Player.IsSignedIn)
                signOutMenuEntry.SetLocalisedText(vxLocKeys.Network_User_SignOut);

            var backMenuEntry = new vxMenuEntry(this, vxLocKeys.Back);

            RestorePurchasesMenuEntry.Selected += delegate
            {
                vxNotificationManager.Show("Restoring Purchases", Color.Magenta);
                vxInAppProductManager.Instance.RestorePurchases();
            };

            // View Leaderboards
            ViewLeaderboardsMenuEntry.Selected += delegate
            {
                vxPlatform.Player.ViewAllLeaderboards();
            };

            // View Achievements
            ViewAchievementsMenuEntry.Selected += delegate
            {
                vxPlatform.Player.ViewAchievments();
            };

            signOutMenuEntry.Selected += delegate
            {

                if (vxPlatform.Player.IsSignedIn)
                    vxPlatform.Player.SignOut();
                else
                    vxPlatform.Player.SignIn();
            };

            backMenuEntry.Selected += new System.EventHandler<PlayerIndexEventArgs>(backMenuEntry_Selected);


            // Add entries to the menu.
            if (vxEngine.Game.HasAchievements)
                AddMenuItem(ViewAchievementsMenuEntry);

            if (vxEngine.Game.HasLeaderboards)
                AddMenuItem(ViewLeaderboardsMenuEntry);

            if (vxEngine.Game.HasInAppPurchases)
                AddMenuItem(RestorePurchasesMenuEntry);
            
#if !__IOS__
            AddMenuItem(signOutMenuEntry);
#endif
			AddMenuItem(backMenuEntry);
        }


        void backMenuEntry_Selected(object sender, PlayerIndexEventArgs e)
        {
            ExitScreen();
        }

#endregion

        public override void Draw()
        {

            if (vxPlatform.Player.IsSignedIn)
            {
                signOutMenuEntry.Text = "Sign Out";
                ViewAchievementsMenuEntry.IsEnabled = true;
                ViewLeaderboardsMenuEntry.IsEnabled = true;
            }
            else
            {
                signOutMenuEntry.Text = "Sign In";
                ViewAchievementsMenuEntry.IsEnabled = false;
                ViewLeaderboardsMenuEntry.IsEnabled = false;
            }

            base.Draw();
        }

    }
}
