#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Input.Events;
using VerticesEngine;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Dialogs;


#endregion

namespace VerticesEngine.UI.Menus
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class vxControlsMenuScreen : vxMenuBaseScreen
    {
        #region Fields

//        vxSliderMenuEntry MouseInvertedMenuEntry;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public vxControlsMenuScreen()
            : base(vxLocKeys.Settings_Controls)
        {

        }

        public override void LoadContent()
        {
            base.LoadContent();

            var keyboardMenuEntry = new vxMenuEntry(this, vxLocKeys.Settings_Keyboard_Title);
			var gamepadMenuEntry = new vxMenuEntry(this, vxLocKeys.Settings_Gamepad_Title);
			var mouseMenuEntry = new vxMenuEntry(this, vxLocKeys.Settings_Mouse_Title);

			var backMenuEntry = new vxMenuEntry(this, vxLocKeys.Back);

			//Accept and Cancel
			keyboardMenuEntry.Selected += delegate {
				vxSceneManager.AddScene(new vxKeyboardSettingsDialog(), PlayerIndex.One);	
			};
			backMenuEntry.Selected += new System.EventHandler<PlayerIndexEventArgs>(backMenuEntry_Selected);


			// Add entries to the menu.
			AddMenuItem(keyboardMenuEntry);
			AddMenuItem(gamepadMenuEntry);
			AddMenuItem(mouseMenuEntry);
			AddMenuItem(backMenuEntry);
        }


        void backMenuEntry_Selected(object sender, PlayerIndexEventArgs e)
        {
            ExitScreen();
        }

        #endregion



    }
}
