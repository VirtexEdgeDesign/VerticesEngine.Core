#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Input.Events;
using VerticesEngine;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Dialogs;
using VerticesEngine.Profile;
using System;
using VerticesEngine.UI.MessageBoxs;
using VerticesEngine.Monetization.Purchases;


#endregion

namespace VerticesEngine.UI.Menus
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class vxPurchaseMenuScreen : vxMenuBaseScreen
    {

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
        public vxPurchaseMenuScreen()
            : base("Purchases")
        {

        }

        vxMenuEntry clearPurchMenuEntry;

        public override void LoadContent()
        {
            base.LoadContent();

            clearPurchMenuEntry = new vxMenuEntry(this, "Clear All Purchases");
            clearPurchMenuEntry.Selected += ClearPurchMenuEntry_Selected;

          var backMenuEntry = new vxMenuEntry(this, "Back");
            backMenuEntry.Selected += backMenuEntry_Selected;


            // Add entries to the menu.
            AddMenuItem(clearPurchMenuEntry);
            AddMenuItem(backMenuEntry);
        }

        private void ClearPurchMenuEntry_Selected(object sender, PlayerIndexEventArgs e)
        {
            var msgBox = vxMessageBox.Show("Clear Purchases?", "This will consume all\navailable purchases.\nPlease Confirm", vxEnumButtonTypes.OkCancel);

            msgBox.Accepted += delegate {
                vxInAppProductManager.Instance.ConsumeAllPurchases();
            };            
        }


        void backMenuEntry_Selected(object sender, PlayerIndexEventArgs e)
        {
            ExitScreen();
        }


    }
}
