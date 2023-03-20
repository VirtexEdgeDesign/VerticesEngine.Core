/**
 * @file
 * @brief This is the foundational class of the entire engine. You will need to create a class which inherit from <see cref="vxGame"/> and provide the proper overrides for configuration
 *
 * You will need to provide a <see cref="VerticesEngine.vxGameConfigurationsAttribute"/> which provides configuration data for the engine to 
 * use your game instance. See the <see cref="VerticesEngine.vxGame"/> documentations for a breakdown of how to provide it.
 * 
 */

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using VerticesEngine.Diagnostics;
using VerticesEngine.Graphics;
using VerticesEngine.Input;
using VerticesEngine.Monetization.Ads;
using VerticesEngine.Plugins;
using VerticesEngine.Profile;
using VerticesEngine.Screens.Async;
using VerticesEngine.UI;
using VerticesEngine.UI.Menus;
using VerticesEngine.UI.MessageBoxs;
using VerticesEngine.UI.StartupScreen;
using VerticesEngine.Utilities;
using VerticesEngine.Workshop;


#if __ANDROID__
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Content.PM;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Gms.Ads;
#endif

#endregion

namespace VerticesEngine
{

    /// <summary>
    /// This is the foundational class of the entire engine. You will need to create a class which inherit from <see cref="vxGame"/> and 
    /// provide the proper overrides for configuration.
    /// </summary>
    /// <remarks>
    /// You will need to provide a <see cref="VerticesEngine.vxGameConfigurationsAttribute"/> which provides configuration data for the engine to 
    /// use your game instance.
    /// </remarks>
    /// <code>
    ///     [vxGameConfigurations(GameName = "My Game Name", 
    ///     GameType = vxGameEnviromentType.ThreeDimensional, MainOrientation = vxOrientationType.Landscape,
    ///     ConfigOptions = 
    ///         vxGameConfigFlags.AudioSettings | 
    ///         vxGameConfigFlags.GraphicsSettings | 
    ///         vxGameConfigFlags.ControlsSettings | 
    ///         vxGameConfigFlags.LeaderboardsSupport |
    ///         vxGameConfigFlags.LanguageSettings
    ///         #if __STEAM__ || __ITCHIO__
    ///         | vxGameConfigFlags.PlayerProfileSupport 
    ///         #endif
    ///         )]
    ///         public MyGame : vxGame
    ///         {
    ///             ...
    ///         }
    /// </code>
    public abstract class vxGame : Game
    {
        #region Fields

        /// <summary>
        /// The game config object. This is set at launch and can not be changed
        /// once it's been initialised.
        /// </summary>
        vxGameConfigurationsAttribute gameConfig;

        /// <summary>
        /// Gets the name of this game.
        /// </summary>
        /// <value>The name of the game.</value>
        public string Name
        {
            get { return gameConfig.GameName; }
        }

        /// <summary>
        /// Gets the game version.
        /// </summary>
        /// <returns>The game version.</returns>
        public virtual string Version
        {
            get
            {
                return _version;
            }
        }
        private string _version;

        /// <summary>
        /// The App ID which is used by a large number of different platforms
        /// </summary>
        public string AppID
        {
            get { return GetAppID(); }
        }

        /// <summary>
        /// Is this game in demo mode?
        /// </summary>
        public virtual bool IsDemo
        {
            get { return false; }
        }


        protected virtual string GetAppID()
        {
            throw new Exception("App ID Is not set for this game");
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="VerticesEngine.vxGame"/> has been updated since last launch
        /// Check this whether to show a custom update screen.
        /// @note This will use the version saved in the Game.ini file.
        /// </summary>
        /// <returns><c>true</c> if has game updated; otherwise, <c>false</c>.</returns>
        public bool IsGameUpdated
        {
            get { return _isGameUpdated; }
            internal set { _isGameUpdated = value; }
        }
        bool _isGameUpdated = false;

        public GameInitializationStage InitializationStage = GameInitializationStage.PrimaryStartup;


        #endregion

        #region -- Game Configuration Properties --

        /// <summary>
        /// The type of the game.
        /// </summary>
        public readonly vxGameEnviromentType GameType;

        /// <summary>
        /// The main orientation.
        /// </summary>
        public readonly vxOrientationType MainOrientation = vxOrientationType.Landscape;

        /// <summary>
        /// The has controls settings.
        /// </summary>
        public bool HasControlsSettings
        {
            get { return gameConfig.ConfigOptions.HasFlag(vxGameConfigFlags.ControlsSettings); }
        }

        /// <summary>
        /// The has graphics settings.
        /// </summary>
        public bool HasGraphicsSettings
        {
            get { return gameConfig.ConfigOptions.HasFlag(vxGameConfigFlags.GraphicsSettings); }
        }

        /// <summary>
        /// The has audio settings.
        /// </summary>
        public bool HasAudioSettings
        {
            get { return gameConfig.ConfigOptions.HasFlag(vxGameConfigFlags.AudioSettings); }
        }

        /// <summary>
        /// The has language settings.
        /// </summary>
        public bool HasLanguageSettings
        {
            get { return gameConfig.ConfigOptions.HasFlag(vxGameConfigFlags.LanguageSettings); }
        }

        /// <summary>
        /// Does this game require the network code to be setup at start.
        /// </summary>
        public bool HasNetworkCapabilities
        {
            get { return gameConfig.ConfigOptions.HasFlag(vxGameConfigFlags.NetworkEnabled); }
        }

        public bool HasProfileSupport
        {
            get { return gameConfig.ConfigOptions.HasFlag(vxGameConfigFlags.PlayerProfileSupport); }
        }

        public bool HasLeaderboards
        {
            get { return gameConfig.ConfigOptions.HasFlag(vxGameConfigFlags.LeaderboardsSupport); }
        }

        public bool HasAchievements
        {
            get { return gameConfig.ConfigOptions.HasFlag(vxGameConfigFlags.AchievementsSupport); }
        }

        /// <summary>
        /// Does this game have in app purchases
        /// </summary>
        public bool HasInAppPurchases
        {
            get { return gameConfig.ConfigOptions.HasFlag(vxGameConfigFlags.InAppPurchases); }
        }

        /// <summary>
        /// Is the debug menu available?
        /// </summary>
        public virtual bool IsDebugMenuAvailable
        {
            get { return (vxEngine.BuildType == vxBuildType.Debug); }
        }

        /// <summary>
        /// The Sanbox Icon path
        /// </summary>
        public virtual string SandboxIconPath
        {
            get { return Path.Combine(vxIO.PathToCacheFolder, "SandboxIcons"); }
        }

        /// <summary>
        /// Called during the vxGame constructor during startup when the ad manager is initialised.
        /// You should set the Ad Manager Test mode and test devices in this method.
        /// </summary>
        protected internal virtual vxIAdProvider OnInitAdProvider()
        {

#if __ANDROID__
            return new vxAdProviderAndroid();
#elif __IOS__
            return new vxAdProviderIOS();
#else
            return new vxGenericAdProvider();
#endif

        }

        #endregion

        #region -- Private Fields --


        /// <summary>
        /// Tells whether or not the Game has loaded it's Specific Assets.
        /// </summary>
        internal bool IsGameContentLoaded = false;


        /// <summary>
        /// The graphics device manager.
        /// </summary>
        internal GraphicsDeviceManager GraphicsDeviceManager;


        #endregion

        #region Initialization

        /// <summary>
        /// Get's the running process name for this game
        /// </summary>
        /// <returns></returns>
        public virtual string GetProcessName()
        {
            return System.Diagnostics.Process.GetCurrentProcess().ProcessName;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="VerticesEngine.vxGame"/> class.
        /// </summary>
        protected vxGame()
        {
            // get the game assembly
            var gameType = System.Reflection.Assembly.GetAssembly(this.GetType());

            // set the game version
            _version = gameType.GetName().Version.ToString();

            gameConfig = (vxGameConfigurationsAttribute)Attribute.GetCustomAttribute(this.GetType(), typeof(vxGameConfigurationsAttribute));
            if (gameConfig == null)
            {
                throw new vxGameAttributeException(typeof(vxGameConfigurationsAttribute));
            }
            else
            {
                this.MainOrientation = gameConfig.MainOrientation;
                this.GameType = gameConfig.GameType;
            }

            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            GraphicsDeviceManager.GraphicsProfile = GraphicsProfile.HiDef;

            // Set the Games Root Directory
            Content = new ContentManager(this.Services)
            {
                RootDirectory = "Content"
            };

            // Create the engine component.
            vxEngine.Instance.Init(this);
        }


        /// <summary>
        /// Initialise any and all overlays. This is often used for adding Admob views in iOS and Android
        /// as well as for handling Steam Overlay code.
        /// </summary>
        protected virtual void InitOverlays()
        {

        }

        protected internal virtual Net.vxINetworkConfig GetNetworkConfig()
        {
            return null;
            throw new Exception("No vxINetworkConfig has been provided");
        }

        /// <summary>
        /// Has a command line argument been passed through?
        /// </summary>
        /// <param name="cmdarg"></param>
        /// <returns></returns>
        public bool IsCmdArgPassed(string cmdarg)
        {
            foreach (var arg in vxEngine.Instance.CMDLineArgs)
            {
                if (cmdarg == arg)
                    return true;
            }

            return false;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// Loads graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            // Load Engine 
            vxEngine.Instance.LoadContent();

            InitialiseRenderPasses();

            vxPluginManager.LoadPlugins();

            // setup InApp Products
            InitInAppProducts();

            // Now Initialise any other Overlays
            InitOverlays();

            // Load GUI Content
            OnLoadUIAssets();

            vxSceneManager.AddScene(new vxTitleScreen(), null);
        }

        protected virtual string GetEngineLicense()
        {
            return "0000-0000-0000-0000-0000";
        }

        internal string CheckEngineLicense()
        {
            return GetEngineLicense();
        }

        /// <summary>
        /// Loads Global Content Specific for the Game. This is called by 'vxLoadAssetsScreen.Load(...'
        /// in the 'MainEntryPoint()' method.
        /// </summary>
        protected internal virtual IEnumerator LoadGlobalContent()
        {
            // Let the Inpurt Manager Draw
            vxInput.IsCusorInitialised = true;
            yield return null;
        }

        /// <summary>
        /// Setups the In App Products. This is where you'll want to Add Products and
        /// Restore Purchases as well
        /// </summary>
        /// <remarks>
        /// Below is some code
        /// </remarks>
        /// <code>
        /// 
        /// // Register an available product with the game
        /// vxInAppProductManager.Instance.AddProduct(InAppProductTypes.RemoveAdsItem, new vxInAppProduct("Go Ad Free",
        ///      vxInAppProductType.NonConsumable,
        ///      new vxPlatformString("", "sku_key_remove_ads")));
        ///
        /// // You'll want to restore purchases after
        /// vxInAppProductManager.Instance.RestorePurchases();
        /// 
        /// </code>
        protected virtual void InitInAppProducts()
        {

        }

        protected internal virtual vxIPlugin GetCoreGamePlugin()
        {
            throw new Exception("No Main Content Pack Provided.");
        }

        /// <summary>
        /// Initialises the players platform profile. Override this method to provide your own platform player profile
        /// </summary>
        /// <returns></returns>
        protected internal virtual vxIPlayerProfile InitPlayerPlatformProfile()
        {
            return new vxPlayerProfileGenericWrapper();
        }

        /// <summary>
        /// Override this to add more options to the settings screens.
        /// </summary>
        public virtual void AddSettingsScreens(vxSettingsMenuScreen SettingsMenuScreen)
        {

        }


        


        /// <summary>
        /// Registers all render passes for a given scene
        /// </summary>
        /// <param name="renderer"></param>
        internal void InitialiseRenderPasses()
        {
            var renderer = vxRenderPipeline.Instance;

            if (this.gameConfig.GameType == vxGameEnviromentType.TwoDimensional)
            {
                renderer.AddRenderFeature<vxMainScene2DRenderPass>();
                renderer.AddRenderFeature<vxDistortionPostProcess2D>();
#if !__MOBILE__
                renderer.AddRenderFeature<vxSceneBlurPostProcess>();
#endif
            }
            else
            {
                renderer.AddRenderFeature<vxMainScene3DRenderPass>();
                renderer.AddRenderFeature<vxCascadeShadowRenderPass>();
                renderer.AddRenderFeature<vxGBufferRenderingPass>();

                // post processes
                renderer.AddRenderFeature<vxSceneLightRenderingPass>();
                renderer.AddRenderFeature<vxEdgeDetectPostProcess>();
                renderer.AddRenderFeature<vxSelectedItemEdgePostProcess>();
                renderer.AddRenderFeature<vxSunLightPostProcess>();
                renderer.AddRenderFeature<vxDistortionPostProcess>();
#if !__MOBILE__
                renderer.AddRenderFeature<vxBlurEffectsPostProcess>();
                renderer.AddRenderFeature<vxCameraMotionBlurPostProcess>();
#endif
                renderer.AddRenderFeature<vxFogPostProcess>();
#if !__MOBILE__
                renderer.AddRenderFeature<vxSceneBlurPostProcess>();
                renderer.AddRenderFeature<vxAntiAliasPostProcess>();
#endif
                renderer.AddRenderFeature<vxDebugRenderPass>();
            }

            OnRenderPipelineInitialised(renderer);

            renderer.Initialise();
        }

        /// <summary>
        /// Called when the render pipeline has been initialised. Use this method to register your own <see cref="vxRenderPass"/>s.
        /// </summary>
        /// <remarks>
        /// The stock effects are added to the renderer at this step. If you'd like to modify or add your own <see cref="vxRenderPass"/> you 
        /// can do so here by calling the following:
        /// <code>
        /// // adds a scene blur post processa the end of the render pipeline
        ///  renderer.AddRenderFeature<vxSceneBlurPostProcess>();
        ///  </code>
        /// </remarks>
        /// <param name="renderPipeline"></param>
        protected virtual void OnRenderPipelineInitialised(vxRenderPipeline renderPipeline)
        {

        }

        /// <summary>
        /// Called before the window is first shown, this is where any and all main UI assets should be loaded.
        /// </summary>
        protected internal virtual void OnLoadUIAssets()
        {
            vxConsole.WriteLine("Loading GUI");
        }



        /// <summary>
        /// This loads any localised content. This is called if the localization settings are changed
        /// and will re-load any and all relavant content
        /// </summary>
        protected internal virtual void LoadLocalisedContent(string regionKey)
        {
            // Let the Inpurt Manager Draw
            vxInput.IsCusorInitialised = true;
        }


        /// <summary>
        /// Override this Method to have your DLC content loaded here.
        /// </summary>
        protected internal virtual string[] GetDLCPaths()
        {
            return new string[0];
        }



        /// <summary>
        /// This is the Main Entry point for the game external to the Engine. Override this and call StartGame(...)
        /// </summary>
        protected internal virtual void OnGameStart()
        {
            throw new Exception("OnGameStart must be overriden to call StartGame(...);");
        }

        /// <summary>
        /// Loads the Player Profile Data
        /// </summary>
        protected internal virtual IEnumerator OnLoadPlayerProfile()
        {
            vxConsole.WriteWarning(this.ToString(), "No Player Profile has been loaded...");
            yield return null;
        }

        /// <summary>
        /// Starts the game with the specefied Backgound Scene and Start menu. Override if you want anything more than just the
        /// basic 'background screen with menu over top.'
        /// </summary>
        public void OnGameStart(vxBaseScene BackgroundScene, vxBaseScene StartMenuScreen)
        {
            // if the game hasn't been initialised yet, then load the init screen, if not, go straight into the main menu
            if (InitializationStage < GameInitializationStage.Running)
            {
                vxLoadAssetsScreen.Load(BackgroundScene, new vxStartupMenuScreen(StartMenuScreen));
            }
            else
            {
                vxLoadAssetsScreen.Load(BackgroundScene, StartMenuScreen);
            }
        }


        public virtual void InitDebugCommands()
        {

#if DEBUG
            // dumps or packs the current level
            // ******************************************************************
            vxDebug.CommandUI.RegisterCommand(
                "sbx",              // Name of command
                "Sandbox File Commands.",     // Description of command
                delegate (IDebugCommandHost host, string command, IList<string> args)
                {
                    if (args.Count == 0)
                        args.Add("-help");

                    switch (args[0])
                    {
                        case "-dump":
                            vxEngine.Instance.CurrentScene.DumpFile();
                            break;
                        case "-pack":
                            vxEngine.Instance.CurrentScene.PackFile();
                            break;
                        case "-help":
                            host.Echo("");
                            host.Echo("Sandbox File Command Help");
                            host.Echo("==============================================================================");
                            host.Echo("     -dump           Dumps the current sandbox components to it's uncompressed parts.");
                            host.Echo("     -pack           Packs dumped files into a Vertices 'sbx' file.");
                            host.Echo("");
                            break;
                    }
                });

#endif
        }

#endregion

                #region Game Loop


        private float accumulatedTime = 0;
        private float timeStep = 1 / 60f;
        protected override void Update(GameTime gameTime)
        {
#if !DEBUG && CRASHHANDLER_ENABLED
            try
            {
                if (vxCrashHandler.IsInitialised == false)
                {
#endif

            if (IsFixedTimeStep)
                IsFixedTimeStep = false;

            // First thing is to update the time
            vxTime.Update(gameTime);

            base.Update(gameTime);

            // The game is optimised for 60 fps, but if we're out side of that
            // then we want to skip a few frames or make up for a few frames
            if (vxTime.IsFixed)
            {
                accumulatedTime += vxTime.ActualTotalGameTime;

                while (accumulatedTime >= timeStep)
                {
                    accumulatedTime -= timeStep;

                    vxEngine.Instance.Update();
                }
            }
            else
            {
                vxEngine.Instance.Update();
            }
#if !DEBUG && CRASHHANDLER_ENABLED
                }
            }
            catch (Exception ex)
            {
                vxCrashHandler.Thwrow(ex);
            }
#endif
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
#if !DEBUG && CRASHHANDLER_ENABLED
            if (vxCrashHandler.IsInitialised)
            {
                vxCrashHandler.Draw(gameTime);
            }
            else
            {
                try
                {
#endif
            GraphicsDeviceManager.GraphicsDevice.Clear(Color.Black);

            // The real drawing happens inside the engine component.
            base.Draw(gameTime);

            // Draw the Engine
            vxEngine.Instance.Draw();
#if !DEBUG && CRASHHANDLER_ENABLED

                }
                catch (Exception ex)
                {
                    vxCrashHandler.Thwrow(ex);
                }
            }
#endif
        }


                #endregion

        /// <summary>
        /// This method is called when the user clicks on the 'get mod sdk' button.
        /// </summary>
        public virtual void OnGetModSDK()
        {

        }

        /// <summary>
        /// This is called when the user wants to upload a mod
        /// </summary>
        public virtual void OnUploadMod()
        {

        }

        /// <summary>
        /// Initialises the Workshop Wrapper. Should this use the default Steam Workhop or Firebase Workshop or the
        /// empty <see cref="Workshop.Providers.Generic.vxGenericWorkshopWrapper"/>
        /// </summary>
        /// <returns></returns>
        protected internal virtual vxIWorkshopWrapper OnInitWorkshopWrapper()
        {
            return new Workshop.Providers.Generic.vxGenericWorkshopWrapper();
        }

        /// <summary>
        /// This is called when a workshop item is opened
        /// </summary>
        /// <param name="item">The workshop item to open</param>
        public virtual void OnWorkshopItemOpen(vxIWorkshopItem item)
        {

        }


                #region Misc Default Screens

        /// <summary>
        /// This shows the init screen which signs in the user, prompts for permissions etc...
        /// 
        /// <example> 
        /// You can add your own Init Steps by overrideing this call and adding to the InitializationStep list.
        /// <code>
        /// 
        /// // create a class which inherits from vxInitializationStep
        /// public class MyInitializationStep : MyInitializationStep
        /// {
        /// ...
        /// }
        /// 
        /// // Then override this method to add more
        /// vxInitializationScreen.InitializationStep.Add(new MyInitializationStep());
        /// 
        /// </code>
        /// </example>
        /// </summary>
        /// <returns></returns>
        protected internal virtual vxInitializationScreen OnShowInitScreen()
        {
            return new vxInitializationScreen();
        }

        /// <summary>
        /// Called when the engine sets up the init steps at the very start of the game. 
        /// <example> 
        /// You can add your own Init Steps implementing the <see cref="vxIInitializationStep"/> interface and adding to the InitializationStep list.
        /// <code>
        /// 
        /// internal class MetricSignInStep : vxIInitializationStep
        /// {
        /// ...
        /// }
        /// 
        /// 
        /// // setup init steps
        /// vxInitializationScreen.AddStep(new MetricRacer.InitSteps.MetricSignInStep(), 2);
        /// 
        /// </code>
        /// </example>
        /// </summary>
        protected internal virtual void OnInitStepsSetup()
        {

        }

        /// <summary>
        /// Gets the localization screen. Override this to implement your own custom Localization screen.
        /// </summary>
        /// <returns>The localization screen.</returns>
        protected internal virtual vxBaseScene OnShowLocalizationScreen()
        {
            return new vxLocalizationMenuScreen(true);
        }

        /// <summary>
        /// Called then the controls screen are shown
        /// </summary>
        protected internal virtual void OnShowControlsSettings()
        {
            vxSceneManager.AddScene(new vxControlsMenuScreen(), PlayerIndex.One);
        }

        /// <summary>
        /// Called when the player profile screen is shown
        /// </summary>
        protected internal virtual void OnShowProfileSettings()
        {
            vxSceneManager.AddScene(new vxProfileMenuScreen(), PlayerIndex.One);
        }

        /// <summary>
        /// Opens the credits page for this game in the web browser.
        /// </summary>
        protected internal virtual void OnShowCreditsPage()
        {


        }

        /// <summary>
        /// Opens the review page for this games platform in the web browser.
        /// </summary>
        protected internal virtual void OnShowReviewPrompt()
        {

        }

        /// <summary>
        /// This method is called when the game has been updated. Override this for a more interesting output then
        /// just a basic notification
        /// </summary>
        protected internal virtual void OnShowUpdateInfoScreen()
        {
            vxMessageBox.Show("Game Updated!", $"{this.Name} has been updated\nto Version {Version}!");
        }


        /// <summary>
        /// Are there any screens which the game would like to show at startup? This is called
        /// after all other initialization screens.
        /// 
        /// If you override this method, you should set the 'InitializationStage = GameInitializationStage.Waiting'
        /// and then once you're ready to continue to the main screen, you can call 'InitializationStage = GameInitializationStage.ReadyToRun'
        /// </summary>
        protected internal virtual void OnShowGameSpecificStartUpScreens()
        {

        }

                #endregion


                #region Permission Methods

        /// <summary>
        /// Check permissions here
        /// </summary>
        /// <returns></returns>
        protected internal virtual bool IsPermissionsRequestRequired()
        {
            return false;
        }

        /// <summary>
        /// A dialog screen which is shown to the users when requesting permissions. This is usually
        /// used on a mobile platform such as Android
        /// </summary>
        protected internal virtual vxMessageBox OnShowPermissionRequestMessage()
        {
            return vxMessageBox.Show("Permissions", "Request Permissions Here", vxEnumButtonTypes.OkCancel);
        }

        /// <summary>
        /// Called at the start to get basic permissions for mobile apps. External Storage Read/Write is called here,
        /// but override if you want to add any more.
        /// </summary>
        protected internal virtual void RequestPermissions()
        {
#if __ANDROID__
            AndroidX.Core.App.ActivityCompat.RequestPermissions(Game.Activity, new String[] { Android.Manifest.Permission.GetAccounts,
            Android.Manifest.Permission.ReadExternalStorage ,
            Android.Manifest.Permission.WriteExternalStorage }, 0);
#endif
        }

                #endregion


        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);
            vxConsole.InternalWriteLine("Exiting ...");
        }
        protected override void UnloadContent()
        {
            base.UnloadContent();

            vxEngine.Instance.UnloadContent();

            vxConsole.InternalWriteLine("Unloading Content...");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(true);

            vxConsole.InternalWriteLine("Disposing...");

            vxEngine.Instance.Dispose();

#if !__MOBILE__
            System.Diagnostics.ProcessThreadCollection currentThreads = System.Diagnostics.Process.GetCurrentProcess().Threads;
            foreach (System.Diagnostics.ProcessThread thread in currentThreads)
            {
                //Console.WriteLine(thread.Id);
                //    var p =System.Diagnostics.Process.GetProcessById(thread.Id);

                //   Console.WriteLine(p.ProcessName);
                //    p.Kill();
                thread.Dispose();
                //thread.
                //thread.IsBackground = true;
            }

            // why doesn't it always cleanly exit?
            System.Diagnostics.Process.GetCurrentProcess().Kill();
            System.Environment.Exit(Environment.ExitCode);
#endif

        }



#if __ANDROID__


        protected virtual void OnStart()
        {

        }

        protected virtual void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {

        }

        protected virtual void OnStop()
        {

        }
#endif

        /// <summary>
        /// Called when the game crashes, this is provided for app specific 
        /// </summary>
        /// <param name="ex"></param>
        protected internal virtual void OnGameCrash(Exception ex) { }

        /// <summary>
        /// This is the URL to the dashboard to send stats and crash messages to.
        /// </summary>
        /// <returns></returns>
        protected internal virtual string GetDashboardURL()
        {
            throw new Exception("Dashboard URL Not Specified");
        }

        /// <summary>
        /// An internal guid used for identifying an app 
        /// </summary>
        /// <returns></returns>
        public virtual string GetAppGuid()
        {
            throw new Exception("App GUID Not Specified");
        }

                #region -- Debug Methods --

        [vxDebugMethod("whoami", "Returns the current profile User Display Name if logged in")]
        static void WhoAmI(vxEngine engine)
        {
            if (vxPlatform.Player.IsSignedIn)
                vxConsole.WriteLine(vxPlatform.Player.Name);
            else
                vxConsole.WriteLine("Player Not Signed In...");
        }



                #endregion

    }
}

