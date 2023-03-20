#region File Description
//-----------------------------------------------------------------------------
// PauseMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
using VerticesEngine.Input.Events;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.MessageBoxs;


#endregion

#region Using Statements
using VerticesEngine.Input;
using VerticesEngine.UI.StartupScreen;
#endregion

namespace VerticesEngine.UI.Menus
{
    /// <summary>
    /// The pause menu comes up over the top of the game,
    /// giving the player options to resume or quit.
    /// </summary>
    public class vxPauseMenuScreen : vxMenuBaseScreen
    {
        #region Initialization

        vxMenuEntry resumeGameMenuEntry;
        vxMenuEntry SettingsMenuEntry;
        vxMenuEntry quitGameMenuEntry;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Menus.vxPauseMenuScreen"/> class.
        /// </summary>
        public vxPauseMenuScreen()
            : base(vxLocKeys.Pause)
        {
            vxInput.IsCursorVisible = true;
        }

		public override void LoadContent ()
		{
			base.LoadContent ();

            // Create our menu entries.
            resumeGameMenuEntry = new vxMenuEntry(this, vxLocKeys.Resume);
            SettingsMenuEntry = new vxMenuEntry(this, vxLocKeys.Settings);
            quitGameMenuEntry = new vxMenuEntry(this, vxLocKeys.Exit);
            // Hook up menu event handlers.
            resumeGameMenuEntry.Selected += OnCancel;
			SettingsMenuEntry.Selected += new System.EventHandler<PlayerIndexEventArgs>(MenuEntry_Graphics_Selected);
			quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

			// Add entries to the menu.
			AddMenuItem(resumeGameMenuEntry);
            AddMenuItem(SettingsMenuEntry);
			AddMenuItem(quitGameMenuEntry);
		}

        void MenuEntry_Graphics_Selected(object sender, PlayerIndexEventArgs e)
        {
            vxSceneManager.AddScene(new vxSettingsMenuScreen(), e.PlayerIndex);
        }


        #endregion

        #region Handle Input

        public override void Draw()
        {
            base.Draw();

            vxInput.IsCursorVisible = true;
        }

        /// <summary>
        /// Event handler for when the Quit Game menu entry is selected.
        /// </summary>
        void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            string message = vxLocalizer.GetText(vxLocKeys.QuitConfirm);
            vxMessageBox confirmQuitMessageBox = new vxMessageBox(message, vxLocalizer.GetText(vxLocKeys.Pause));

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            vxSceneManager.AddScene(confirmQuitMessageBox, ControllingPlayer);
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to quit" message box. This uses the loading screen to
        /// transition from the game back to the main menu screen.
        /// </summary>
        void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            vxSceneManager.GoToMainMenu();
        }


        #endregion
    }
}
