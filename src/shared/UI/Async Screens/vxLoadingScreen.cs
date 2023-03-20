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
using VerticesEngine.Input;
using VerticesEngine.UI.Themes;
using VerticesEngine.Graphics;
using VerticesEngine.Utilities;
using System.Collections;
using VerticesEngine.UI;
using System.Collections.Generic;
#endregion

namespace VerticesEngine.Screens.Async
{
    /// <summary>
    /// The loading screen coordinates transitions between the menu system and the
    /// game itself. Normally one screen will transition off at the same time as
    /// the next screen is transitioning on, but for larger transitions that can
    /// take a longer time to load their data, we want the menu system to be entirely
    /// gone before we start loading the game. This is done as follows:
    /// 
    /// - Tell all the existing screens to transition off.
    /// - Activate a loading screen, which will transition on at the same time.
    /// - The loading screen watches the state of the previous screens.
    /// - When it sees they have finished transitioning off, it activates the real
    ///   next screen, which may take a long time to load its data. The loading
    ///   screen will be the only thing displayed while this load is taking place.
    /// </summary>
    public class vxLoadingScreen : vxBaseScene
    {
        #region Fields

        /// <summary>
        /// Is the loading async, i.e. should we spread it over a number of frames or should we load it all at once?
        /// </summary>
        public bool IsLoadingAsync
        {
            get { return _isLoadingAsync; }
        }
        private bool _isLoadingAsync = false;

        /// <summary>
        /// The loaded percentage from 0 to 1
        /// </summary>
        public float LoadedPercentage
        {
            get { return loadedPerc; }
        }
        private static float loadedPerc = 0;

        public static void SetLoadPercentage(float perc)
        {
            loadedPerc = perc / 100f;
        }

        bool otherScreensAreGone;

        public vxBaseScene[] screensToLoad;

        bool hasInitialyLoaded = false;

        bool isContentFinishedLoading = false;

        IEnumerator loadingSceneEnumerator;
        IEnumerator loadingEnumerator;

        /// <summary>
        /// The loading speed which controls how many coroutine loops are fired per frame. This can speed up level load times
        /// with the trade off of a potentially choppier visual load.
        /// </summary>
        public static int LoadingSpeed = 4;

        #endregion

        #region Initialization



        /// <summary>
        /// The constructor is private: loading screens should
        /// be activated via the static Load method instead.
        /// </summary>
        public vxLoadingScreen(vxBaseScene[] screensToLoad, bool isLoadingAsync = true)
        {
            _isLoadingAsync = isLoadingAsync;

            this.screensToLoad = screensToLoad;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            loadedPerc = 0;

            vxUITheme.LoadingScreenRenderer.Init(this);

        }


        #endregion

        #region Update and Draw

        Stopwatch stopwatch = new Stopwatch();

        /// <summary>
        /// Should we run any and all corouines? This is useful for turning them off so that we can update the loadign screen smoothly
        /// during an expensive or stuturing load
        /// </summary>
        public bool IsCoroutinesEnabled = true;

        /// <summary>
        /// Updates the loading screen.
        /// </summary>
        protected internal override void Update()
        {
            base.Update();

            // If all the previous screens have finished transitioning
            // off, it is time to actually perform the load.
            if (otherScreensAreGone)
            {
                if (hasInitialyLoaded == false)
                {
                    stopwatch.Start();
                    // Perform the load operation.
                    foreach (vxBaseScene screen in screensToLoad)
                    {
                        if (screen != null)
                        {
                            vxSceneManager.AddScene(screen, ControllingPlayer);

                            // get internal scene content loaded in vertices
                            loadingSceneEnumerator = screen.LoadSceneContentAsync();

                            // now call the general load content method
                            loadingEnumerator = screen.LoadContentAsync();
                            // since we're doing a content load then we may not be fully done yet
                            screen.IsContentLoaded = false;
                        }
                    }
                    // Once the load has finished, we use ResetElapsedTime to tell
                    // the  game timing mechanism that we have just finished a very
                    // long frame, and that it should not try to catch up.
                    vxTime.ResetElapsedTime();
                    hasInitialyLoaded = true;
                    vxEngine.Game.IsFixedTimeStep = false;
                    vxGraphics.DeviceManager.SynchronizeWithVerticalRetrace = false;
                }
                else if(isContentFinishedLoading == false && IsCoroutinesEnabled)
                {
                    for (int loadCnt = 0; loadCnt < LoadingSpeed; loadCnt++)
                    {
                        if (loadingSceneEnumerator.MoveNext())
                        {
                            // first load scene content
                        }
                        else if (loadingEnumerator.MoveNext())
                        {
                            // now load other content
                        }
                        else
                        {
                            screensToLoad[0].IsContentLoaded = true;
                            isContentFinishedLoading = true;
                        }
                    }
                }

                // we're done loading, so let's exit this scene
                if(isContentFinishedLoading)
                {
                    stopwatch.Stop();
                    //Console.WriteLine("Scene Loaded..." + stopwatch.Elapsed);
                    ExitScreen();
                }
            }
            vxUITheme.LoadingScreenRenderer.Update(this);
            vxTime.ResetElapsedTime();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            vxUITheme.LoadingScreenRenderer.OnExit();
        }

        /// <summary>
        /// Draws the loading screen.
        /// </summary>
        public override void Draw()
        {
            vxEngine.Game.GraphicsDevice.SetRenderTarget(vxGraphics.FinalBackBuffer);

            vxInput.IsCursorVisible = false;

            // If we are the only active screen, that means all the previous screens
            // must have finished transitioning off. We check for this in the Draw
            // method, rather than in Update, because it isn't enough just for the
            // screens to be gone: in order for the transition to look good we must
            // have actually drawn a frame without them before we perform the load.
            if ((ScreenState == ScreenState.Active) &&
                (vxSceneManager.GetScreens().Length == 1))
            {
                otherScreensAreGone = true;
            }

            vxUITheme.LoadingScreenRenderer.Draw(this);

        }


        #endregion
    }
}
