#region Using Statements
using System;
using System.Threading;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine;
using VerticesEngine.Utilities;
using VerticesEngine.Plugins;
using VerticesEngine.UI.Themes;
using VerticesEngine.Graphics;
using System.Collections.Generic;
using VerticesEngine.UI;
using System.Collections;
using VerticesEngine.Input;

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
    internal class vxLoadAssetsScreen : vxBaseScene
    {
        /// <summary>
        /// The loading text.
        /// </summary>
        public static string LoadingText = "";

        #region Fields

        bool otherScreensAreGone;

        vxBaseScene[] screensToLoad;

        float loadAnimationTimer;

        /// <summary>
        /// The loading speed which controls how many coroutine loops are fired per frame. This can speed up level load times
        /// with the trade off of a potentially choppier visual load.
        /// </summary>
        public static int LoadingSpeed = 4;

        #endregion

        #region Initialization

        struct SceneLoadInfo
        {
            public vxBaseScene scene;
            public IEnumerator loadingSceneEnumerator;
            public IEnumerator loadingEnumerator;
        }

        List<SceneLoadInfo> scenes = new List<SceneLoadInfo>();

        /// <summary>
        /// The constructor is private: loading screens should
        /// be activated via the static Load method instead.
        /// </summary>
        private vxLoadAssetsScreen(vxBaseScene[] screensToLoad)
        {
            this.screensToLoad = screensToLoad;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
        }


        /// <summary>
        /// Activates the loading screen.
        /// </summary>
        public static void Load(params vxBaseScene[] screensToLoad)
        {
            vxInput.IsCursorVisible = false;

            // Tell all the current screens to transition off.
            foreach (var screen in vxSceneManager.GetScreens())
                screen.ExitScreen();

            // Create and activate the loading screen.
            vxLoadAssetsScreen loadingScreen = new vxLoadAssetsScreen(screensToLoad);
            vxSceneManager.AddScene(loadingScreen);
        }


        #endregion

        #region Update and Draw

        /// <summary>
        /// Gets or sets the splash screen.
        /// </summary>
        /// <value>The splash screen.</value>
        public static Texture2D SplashScreen;

        bool isStartScreensLoaded = false;
        bool isContentFinishedLoading = false;
        int sceneLoadIndex = 0;

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
                if (isStartScreensLoaded == false)
                {
                    foreach (vxBaseScene screen in screensToLoad)
                    {
                        if (screen != null)
                        {
                            vxSceneManager.AddScene(screen, ControllingPlayer);

                            SceneLoadInfo loadInfo;
                            loadInfo.scene = screen;
                            // get internal scene content loaded in vertices
                            loadInfo.loadingSceneEnumerator = screen.LoadSceneContentAsync();

                            // now call the general load content method
                            loadInfo.loadingEnumerator = screen.LoadContentAsync();

                            // since we're doing a content load then we may not be fully done yet
                            screen.IsContentLoaded = false;
                            scenes.Add(loadInfo);
                        }
                    }
                    vxTime.ResetElapsedTime();
                    isStartScreensLoaded = true;
                    vxEngine.Game.IsFixedTimeStep = false;
                    vxGraphics.DeviceManager.SynchronizeWithVerticalRetrace = false;
                }
                else if (isContentFinishedLoading == false)
                {
                    // how many load counts per frame do we want? this can speed up the main menu load
                    for (int loadCnt = 0; loadCnt < LoadingSpeed; loadCnt++)
                    {
                        if (sceneLoadIndex < scenes.Count)
                        {
                            if (scenes[sceneLoadIndex].loadingSceneEnumerator.MoveNext())
                            {
                                // first load scene content
                            }
                            else if (scenes[sceneLoadIndex].loadingEnumerator.MoveNext())
                            {
                                // now load other content
                            }
                            else
                            {
                                scenes[sceneLoadIndex].scene.IsContentLoaded = true;
                                sceneLoadIndex++;
                            }
                        }
                        else
                        {
                            isContentFinishedLoading = true;
                        }
                    }

                    vxInput.IsCursorVisible = false;
                }

                // we're done loading, so let's exit this scene
                else if (isContentFinishedLoading)
                {
                    vxEngine.Game.IsGameContentLoaded = true;

                    vxScreen.RefreshGraphics();

                    vxConsole.WriteIODebug("Global Assets Loaded");
                    vxConsole.InternalWriteLine("Starting Game");
                    ExitScreen();
                    vxInput.IsCursorVisible = true;
                }

                // Once the load has finished, we use ResetElapsedTime to tell
                // the  game timing mechanism that we have just finished a very
                // long frame, and that it should not try to catch up.
                vxTime.ResetElapsedTime();
            }
        }


        /// <summary>
        /// Draws the loading screen.
        /// </summary>
        public override void Draw()
        {
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

            vxEngine.Game.GraphicsDevice.SetRenderTarget(vxGraphics.FinalBackBuffer);

            // The gameplay screen takes a while to load, so we display a loading
            // message while that is going on, but the menus load very quickly, and
            // it would look silly if we flashed this up for just a fraction of a
            // second while returning from the game to the menus. This parameter
            // tells us how long the loading is going to take, so we know whether
            // to bother drawing the message.
            if (true)
            {

                SpriteFont font = vxUITheme.Fonts.Size20;

                string message = vxLocalizer.GetText(vxLocKeys.Loading);

                // Center the text in the viewport.
                Viewport viewport = vxGraphics.GraphicsDevice.Viewport;
                Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
                Vector2 textSize = font.MeasureString(message + "...");
                Vector2 textPosition = (viewportSize - textSize) * 95 / 100;


                // Animate the number of dots after our "Loading..." message.
                loadAnimationTimer += vxTime.DeltaTime;

                int dotCount = (int)(loadAnimationTimer * 5) % 5;

                message += new string('.', dotCount);


                // Draw the text.
                vxGraphics.SpriteBatch.Begin("Loading Assets Screen");

                if (SplashScreen != null && vxEngine.Game.IsGameContentLoaded == false)
                {
                    // fit the splash screen within the viewport while keeping it's aspect ratio
                    var splashHeight = viewport.Height;
                    var splashWidth = viewport.Height * SplashScreen.GetAspectRatio();

                    var splashX = viewport.Bounds.Center.X - splashWidth / 2;
                    var splashY = 0;

                    var splashRect = vxLayout.GetRect(splashX, splashY, splashWidth, splashHeight);

                    vxGraphics.SpriteBatch.Draw(SplashScreen, splashRect, Color.White * TransitionAlpha);
                }

                vxGraphics.SpriteBatch.DrawString(font, message, textPosition, Color.White);

                vxGraphics.SpriteBatch.DrawString(vxUITheme.Fonts.Size12, LoadingText, new Vector2(10), Color.Gray * 0.25f);
                vxEngine.Instance.DrawVersionInfo(Color.White, TransitionAlpha);

                vxGraphics.SpriteBatch.End();
            }
        }


        #endregion

    }
}
