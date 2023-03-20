using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using VerticesEngine.Graphics;
using VerticesEngine.Screens.Async;
using VerticesEngine.UI.StartupScreen;

namespace VerticesEngine
{
    /// <summary>
    /// The Scene Manager which handles Scene Loading and drawing
    /// </summary>
    public static class vxSceneManager
    {
        /// <summary>
        /// Screen List
        /// </summary>
        public static List<vxBaseScene> SceneCollection = new List<vxBaseScene>();

        /// <summary>
        /// Screens to Update List
        /// </summary>
        static List<vxBaseScene> scenesToUpdate = new List<vxBaseScene>();


        /// <summary>
        /// The color to fade the back buffer to.
        /// </summary>
        public static Color FadeToBackBufferColor = Color.Black;

        /// <summary>
        /// The color of the loading screen background.
        /// </summary>
        public static Color LoadingScreenBackColor = Color.Black;

        /// <summary>
        /// The color of the loading screen text.
        /// </summary>
        public static Color LoadingScreenTextColor = Color.White;

        /// <summary>
        /// If true, the manager prints out a list of all the screens
        /// each time it is updated. This can be useful for making sure
        /// everything is being added and removed at the right times.
        /// </summary>
        public static bool TraceEnabled;

        internal static void Init()
        {

        }

        internal static void LoadContent()
        {
            // Tell each of the screens to load their content.
            foreach (vxBaseScene screen in SceneCollection)
            {
                screen.LoadContent();
            }

        }

        internal static void UnloadContent()
        {
            // Tell each of the screens to unload their content.
            for(int s = 0; s < SceneCollection.Count; s++)
            {
                SceneCollection[s].UnloadContent();
            }
        }


        internal static void Update()
        {
            // Make a copy of the master screen list, to avoid confusion if
            // the process of updating one screen adds or removes others.
            scenesToUpdate.Clear();

            for (int i = 0; i < SceneCollection.Count; i++)
            {
                vxBaseScene screen = SceneCollection[i];
                scenesToUpdate.Add(screen);

                if (screen.CanSceneBeRemovedCompletely == true)
                    screen.Dispose();
            }

            bool otherScreenHasFocus = false;// !vxEngine.CurrentGame.IsActive;
            bool coveredByOtherScreen = false;

            // Loop as long as there are screens waiting to be updated.
            while (scenesToUpdate.Count > 0)
            {

                // Pop the topmost screen off the waiting list.
                vxBaseScene screen = scenesToUpdate[scenesToUpdate.Count - 1];

                scenesToUpdate.RemoveAt(scenesToUpdate.Count - 1);

                // Update the screen.
                screen.coveredByOtherScreen = coveredByOtherScreen;
                screen.otherScreenHasFocus = otherScreenHasFocus;

                if (screen.IsContentLoaded && screen.IsRemoved == false)
                {
                    screen.Update();

                    if (screen.ScreenState == ScreenState.TransitionOn ||
                        screen.ScreenState == ScreenState.Active)
                    {
                        // If this is the first active screen we came across,
                        // give it a chance to handle input.
                        if (!otherScreenHasFocus)
                        {
                            screen.HandleInput();

                            otherScreenHasFocus = true;
                        }

                        // If this is an active non-popup, inform any subsequent
                        // screens that they are covered by it.
                        if (!screen.IsPopup)
                            coveredByOtherScreen = true;
                    }
                }
            }

            if (scenesToUnload.Count > 0)
            {
                foreach (var scene in scenesToUnload)
                {
                    // If we have a graphics device, tell the screen to unload content.
                    if (vxEngine.Instance.IsEngineInitialised)
                    {
                        scene.UnloadContent();
                        scene.Dispose();
                    }
                }
                scenesToUnload.Clear();
            }

            // Print debug trace?
            if (TraceEnabled)
                TraceScreens();
        }

        /// <summary>
        /// Scenes which needs to be unloaded
        /// </summary>
        static List<vxBaseScene> scenesToUnload = new List<vxBaseScene>();

        internal static void Draw()
        {
            for (int s = 0; s < SceneCollection.Count; s++)
            {
                if (SceneCollection[s].ScreenState == ScreenState.Hidden || SceneCollection[s].IsContentLoaded == false)
                    continue;

                SceneCollection[s].Draw();
            }
        }


        #region Public Methods

        /// <summary>
        /// Adds a new screen of type <see cref="T"/> to the screen manager.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void AddScene<T>() where T : vxBaseScene
        {
            var sceneInstance = System.Activator.CreateInstance<T>();
            AddScene(sceneInstance);
        }
        
        /// <summary>
        /// Adds a new screen to the screen manager.
        /// </summary>
        public static void AddScene(vxBaseScene screen)
        {
            AddScene(screen, PlayerIndex.One);
        }

        /// <summary>
        /// Adds a new screen to the screen manager.
        /// </summary>
        public static void AddScene(vxBaseScene screen, PlayerIndex? controllingPlayer)
        {
            //screen.IsInitialised = true;
            screen.ControllingPlayer = controllingPlayer;

            screen.IsExiting = false;

            // If we have a graphics device, tell the screen to load content.
            if (vxEngine.Instance.IsEngineInitialised)
            {
                screen.LoadContent();
                screen.IsContentLoaded = true;
            }

            SceneCollection.Add(screen);
        }

        public static void LoadScene<T>() where T : vxBaseScene
        {
            var sceneToLoad = System.Activator.CreateInstance<T>();
            LoadScene(sceneToLoad);
        }

        /// <summary>
        /// Loads a scene using an async method where the loading is spread out over a series of frames
        /// </summary>
        /// <param name="screensToLoad"></param>
        public static void LoadScene(params vxBaseScene[] screensToLoad)
        {
            // Tell all the current screens to transition off.
            foreach (vxBaseScene screen in vxSceneManager.GetScreens())
                screen.ExitScreen();

            // Create and activate the loading screen.
            vxLoadingScreen loadingScreen = new vxLoadingScreen(screensToLoad, true);


            vxSceneManager.AddScene(loadingScreen, PlayerIndex.One);
        }

        /// <summary>
        /// Tells the game to exit whatever it is doing now and to go to the Main Menu 
        /// </summary>
        public static void GoToMainMenu()
        {
            vxEngine.Game.OnGameStart();
        }

        /// <summary>
        /// Loads a scene syncronously
        /// </summary>
        /// <param name="screensToLoad"></param>
        public static void LoadSceneSyncronously(params vxBaseScene[] screensToLoad)
        {
            // Tell all the current screens to transition off.
            foreach (vxBaseScene screen in vxSceneManager.GetScreens())
                screen.ExitScreen();

            // Create and activate the loading screen.
            vxLoadingScreen loadingScreen = new vxLoadingScreen(screensToLoad, false);


            vxSceneManager.AddScene(loadingScreen, PlayerIndex.One);
        }

        /// <summary>
        /// Removes a screen from the screen manager. You should normally
        /// use vxGameBaseScreen.ExitScreen instead of calling this directly, so
        /// the screen can gradually transition off rather than just being
        /// instantly removed.
        /// </summary>
        public static void RemoveScene(vxBaseScene screen)
        {
            screen.IsRemoved = true;
            SceneCollection.Remove(screen);
            scenesToUpdate.Remove(screen);
            scenesToUnload.Add(screen);

            // if there is a screen still in the manager, update TouchPanel
            // to respond to gestures that screen is interested in.
            if (SceneCollection.Count > 0)
            {
            }
        }


        /// <summary>
        /// Expose an array holding all the screens. We return a copy rather
        /// than the real master list, because screens should only ever be added
        /// or removed using the AddScreen and RemoveScreen methods.
        /// </summary>
        public static vxBaseScene[] GetScreens()
        {
            return SceneCollection.ToArray();
        }


        /// <summary>
        /// Helper draws a translucent black fullscreen sprite, used for fading
        /// screens in and out, and for darkening the background behind popups.
        /// </summary>
        public static void FadeBackBufferToBlack(float alpha)
        {
            Viewport viewport = vxGraphics.GraphicsDevice.Viewport;

            vxGraphics.SpriteBatch.Begin("UI.FadeBackBuffer");

            vxGraphics.SpriteBatch.Draw(vxInternalAssets.Textures.Blank, viewport.Bounds, FadeToBackBufferColor * alpha);

            vxGraphics.SpriteBatch.End();
        }


        #endregion

        #region Utility Functions

        internal static void OnGraphicsRefresh()
        {
            // Now tell all scenes to reset
            foreach (var scene in GetScreens())
            {
                scene.OnGraphicsRefresh();
            }
        }

        internal static void OnLocalizationChanged()
        {
            // Now tell all scenes to reset their localization settings
            foreach (var scene in GetScreens())
            {
                scene.OnLocalizationChanged();
            }
        }

        /// <summary>
        /// Prints a list of all the screens, for debugging.
        /// </summary>
        static void TraceScreens()
        {
            List<string> screenNames = new List<string>();

            foreach (vxBaseScene screen in SceneCollection)
                screenNames.Add(screen.GetType().Name);
        }

        #endregion
    }
}
