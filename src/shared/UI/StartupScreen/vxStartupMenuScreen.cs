#region File Description
//-----------------------------------------------------------------------------
// LoadingScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;
using System.Diagnostics;
using VerticesEngine;
using VerticesEngine.UI.Themes;
using VerticesEngine.Graphics;
using VerticesEngine.UI.MessageBoxs;
using VerticesEngine.UI;
using VerticesEngine.Utilities;
#endregion

namespace VerticesEngine.Screens.Async
{
    /// <summary>
    /// The Init Screen which calls all required setup screens on start of launch
    /// </summary>
    public class vxStartupMenuScreen : vxBaseScene
    {
        #region Initialization

        /// <summary>
        /// the final main menu screen to startup
        /// </summary>
        private vxBaseScene mainMenuScreen;

        /// <summary>
        /// The delay between dialogs
        /// </summary>
        float delay = 15;

        /// <summary>
        /// The currenty delay
        /// </summary>
        float currentDelay = 0;

        public vxStartupMenuScreen(vxBaseScene mainMenuScreen)
        {
            vxEngine.Game.InitializationStage = GameInitializationStage.CheckIfUpdated;
            this.mainMenuScreen = mainMenuScreen;
            Next();
        }

        #endregion

        #region Update and Draw


        public void Next()
        {
            currentDelay = delay;
        }

        /// <summary>
        /// Updates the loading screen.
        /// </summary>
        protected internal override void Update()
        {
            base.Update();


            if (IsActive && currentDelay > 0)
                currentDelay--;



            // now check if we've updated
            if (vxEngine.Game.InitializationStage == GameInitializationStage.CheckIfUpdated && currentDelay == 0)
            {
                vxEngine.Game.InitializationStage = GameInitializationStage.Waiting;
                
                // Check if version is greater or not
                if(vxEngine.Game.IsGameUpdated)
                {
                    vxEngine.Game.OnShowUpdateInfoScreen();
                    vxEngine.Game.IsGameUpdated = false;
                }

                vxEngine.Game.InitializationStage = GameInitializationStage.GameSpecificChecks;
                Next();
            }


            // Call any game specific startup screens
            if (vxEngine.Game.InitializationStage == GameInitializationStage.GameSpecificChecks && currentDelay == 0 && otherScreenHasFocus == false)
            {
                vxEngine.Game.InitializationStage = GameInitializationStage.ReadyToRun;
                vxEngine.Game.OnShowGameSpecificStartUpScreens();
                Next();
            }


            // Finally show the main menu
            if (vxEngine.Game.InitializationStage == GameInitializationStage.ReadyToRun && currentDelay == 0 && otherScreenHasFocus == false)
            {
                // at this point we should be up and running, so we can load the players profile
                //vxEngine.Game.OnLoadPlayerProfile();

                // make sure we have sandbox
                vxIO.EnsureDirExists(vxIO.PathToSandbox);
                vxEngine.Game.InitializationStage = GameInitializationStage.Running;
                Next();
                vxSceneManager.AddScene(mainMenuScreen);
            }

        }

        public override void Draw()
        {
            base.Draw();
        }

        #endregion
    }
}
