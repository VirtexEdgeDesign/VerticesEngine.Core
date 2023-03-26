#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using VerticesEngine.Audio;
using VerticesEngine.ContentManagement;
using VerticesEngine.Diagnostics;
using VerticesEngine.Graphics;
using VerticesEngine.Input;
using VerticesEngine.Monetization.Ads;
using VerticesEngine.Net;
using VerticesEngine.Plugins;
using VerticesEngine.Profile;
using VerticesEngine.UI;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Themes;
using VerticesEngine.Utilities;
using VerticesEngine.Workshop;


//Android Libraries
#if __ANDROID__
using Android.Views;
using Android.Content;
#endif

#endregion

namespace VerticesEngine
{
    /// <summary>
    /// The vxEngine is a component which manages one or more vxGameBaseScreen
    /// instances. It maintains a stack of screens, calls their Update and Draw
    /// methods at the appropriate times, and automatically routes input to the
    /// topmost active screen.
    /// </summary>
    public sealed class vxEngine : IDisposable
    {
        /// <summary>
        /// Returns the instance of the Vertices Engine
        /// </summary>
        public static vxEngine Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new vxEngine();
                }

                return _instance;
            }
        }

        private static vxEngine _instance;

        /// <summary>
        /// The reference to the current game that is being run by the engine.
        /// </summary>
        public static vxGame Game
        {
            get { return _game; }
        }
        private static vxGame _game;


        #region -- Public Fields --


        /// <summary>
        /// Gets the type of the build config. This is set internally but can be changed by launching with specific launch options.
        /// </summary>
        /// <value>The type of the build config.</value>
        public static vxBuildType BuildType
        {
            get { return _buildConfigType; }
        }
        static private vxBuildType _buildConfigType;


        /// <summary>
        /// Gets the OS type of the platform.
        /// </summary>
        /// <value>The type of the platorm.</value>
        public static vxPlatformOS PlatformOS
        {
            get { return _platformOS; }
        }
        static vxPlatformOS _platformOS;


        /// <summary>
        /// Specifies whether this game is running on Desktop, Console, Mobile or Web.
        /// </summary>
        public static vxPlatformHardwareType PlatformType
        {
            get { return _platformType; }
        }
        static vxPlatformHardwareType _platformType;

        /// <summary>
        /// Which platform is this version meant to released on, i.e. Steam, ItchIO, Android etc...
        /// </summary>
        public static vxPlatformType ReleasePlatformType
        {
            get
            {
                return vxPlatform.Platform;
            }
        }


        /// <summary>
        /// Gets the type graphical backend that's being used (i.e. OpenGL, DirectX etc...)
        /// </summary>
        public static vxGraphicalBackend GraphicalBackend
        {
            get
            {
                //Set Location of Content Specific too Platform
#if VRTC_PLTFRM_XNA
                m_graphicalBackend = vxGraphicalBackend.XNA;
#elif VRTC_PLTFRM_GL
                m_graphicalBackend = vxGraphicalBackend.OpenGL;
#elif VRTC_PLTFRM_DX
                m_graphicalBackend = vxGraphicalBackend.DirectX;
#elif __ANDROID__
                m_graphicalBackend = vxGraphicalBackend.Android;
#elif __IOS__
                m_graphicalBackend = vxGraphicalBackend.iOS;
#else
                throw new Exception("Unsupported Graphical Backend");
#endif
                //m_graphicalBackend = vxGraphicalBackend.OpenGL;

                return m_graphicalBackend;
            }
        }

        private static vxGraphicalBackend m_graphicalBackend = vxGraphicalBackend.OpenGL;


        #endregion

        #region -- Public Properties --


        /// <summary>
        /// Whether or not the engine has been initliased or not.
        /// </summary>
        internal bool IsEngineInitialised = false;


        /// <summary>
        /// Gets the Vertices Engine version. Note this is different than the game version.
        /// </summary>
        /// <value>The vxEngine version.</value>
        public static string EngineVersion
        {
            get { return _engineVersion; }
        }
        private static string _engineVersion = "v. 0.0.0.0";

        /// <summary>
        /// The cmd line arguments.
        /// </summary>
        public readonly string[] CMDLineArgs;


        /// <summary>
        /// Gets the CMDL ine arguments as string.
        /// </summary>
        /// <returns>The CMDL ine arguments as string.</returns>
        public string CMDLineArgsToString()
        {
            string[] args = CMDLineArgs;

            string argoutput = "";
            for (int argIndex = 1; argIndex < args.Length; argIndex++)
            {
                argoutput += args[argIndex] + " ";
            }
            return argoutput;
        }


        /// <summary>
        /// The Current Gameplay Screen that is being run.
        /// </summary>
        public vxGameplaySceneBase CurrentScene;


        /// <summary>
        /// Gets or sets the vxUITheme use by this game.
        /// </summary>
        public vxUITheme GUITheme;
        


        #endregion

        #region Initialization

        /// <summary>
        /// Constructs a new screen manager component.
        /// </summary>
        private vxEngine()
        {
            if (m_isProcessConfigured == false)
                ConfigureProcess();
            // Get the CMD line args for this game.
            CMDLineArgs = System.Environment.GetCommandLineArgs();

            HasLicense = LicenseCheck();

            try
            {
                vxSystemInfo.Init();
            }catch(Exception ex)
            {
                vxConsole.WriteException(ex);
            }

        }

        private static bool m_isProcessConfigured = false;
        /// <summary>
        /// Configures the process for this game. This needs to be called from your Program.cs file and before the vxGame is instantiated.
        /// </summary>
        public static void ConfigureProcess()
        {
            m_isProcessConfigured = true;
#if __ANDROID__
            _platformOS = vxPlatformOS.Android;
            _platformType = vxPlatformHardwareType.Mobile;
#elif __IOS__
            _platformOS = vxPlatformOS.iOS;
            _platformType = vxPlatformHardwareType.Mobile;
#else

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                _platformOS = vxPlatformOS.Windows;
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                _platformOS = vxPlatformOS.OSX;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                _platformOS = vxPlatformOS.Linux;

            _platformType = vxPlatformHardwareType.Desktop;
#endif

#if !__MOBILE__
            try
            {
                if (_platformOS == vxPlatformOS.Windows)
                {
                    if (Environment.OSVersion.Version >= new Version(6, 3, 0)) // win 8.1 added support for per monitor dpi
                    {
                        if (Environment.OSVersion.Version >= new Version(10, 0, 15063)) // win 10 creators update added support for per monitor v2
                        {
                            vxNativeMethods.SetProcessDpiAwarenessContext((int)vxNativeMethods.DPI_AWARENESS_CONTEXT.DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2);
                        }
                        else vxNativeMethods.SetProcessDpiAwareness(vxNativeMethods.PROCESS_DPI_AWARENESS.Process_Per_Monitor_DPI_Aware);
                    }
                    else
                    {
                        vxNativeMethods.SetProcessDPIAware();
                    }

                    vxNativeMethods.DisableProcessWindowsGhosting();
                }
            }
            catch (Exception ex)
            {
                if (ex != null)
                    vxConsole.DumpException(ex);
            }
#endif
        }

        internal void Init(vxGame Game)
        { 
            _game = Game;

            _game.IsMouseVisible = true;

            //Get the vxEngine Version through Reflection
            _engineVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            
            vxPlatform.InitProfile();

            vxConsole.Init();

            vxRandom.Init(DateTime.Now.Millisecond);

            vxIO.Init();

            vxAudioManager.Init();

            vxPlatform.Initialise();

            // init the workshop
            vxWorkshop.Init();

            // Initialise the Ad Manager
            vxAdManager.Init();


            IsEngineInitialised = true;
        }

        private readonly bool HasLicense = false;
        private bool LicenseCheck()
        {
            return true;
            // TO Decode
            //int numPrev = 0;
            //string result = "";
            //foreach (var s in str)
            //{
            //    char ch = (char)(256 + numPrev - int.Parse(s));

            //    numPrev = (int)(ch);

            //    result += ch;
            //}

            //string n = Game.Config.GameName;

            //string r = "";

            //int prevMin = 0;
            //foreach (char c in n)
            //{
            //    int num = (256 + prevMin - (int)c);
            //    prevMin = (int)c;
            //    string output = num.ToString();
            //    r += output;

            //}

            //if (r == "" || n == "")
            //    return false;

            //if (Game.CheckEngineLicense() == r)
            //    return true;
            //else
            //    return false;
        }



        /// <summary>
        /// Sets the build config.
        /// </summary>
        /// <param name="configType">Config type.</param>
        internal void SetBuildType(vxBuildType configType)
        {
            vxDebug.LogEngine(new
            {
                engineConfig=configType
            });

            _buildConfigType = configType;
        }

        private Texture2D Logo;

        /// <summary>
        /// Load your graphics content.
        /// </summary>
        internal void LoadContent()
        {
#if DEBUG
            _buildConfigType = vxBuildType.Debug;
#else
            _buildConfigType = vxBuildType.Release;
#endif


            // Initialise the Engine Speciality Content Manager.
            vxInternalContentManager.Instance.Init();

            // Setup and Load Settings at the start
            vxSettings.Init();

            // now lets check if this is a new version
            Version GameAssemblyVersion = new Version(_game.Version);

            var zeroVersion = new Version(0,0,0,0);
            var serialisedVersion = vxSettings.GameVersion.ToSystemVersion();
            if (GameAssemblyVersion > serialisedVersion && serialisedVersion != zeroVersion)
            {
                // we have a later version, but we want to check that either the Major, Minor or Build have increased, not just the revision
                if(GameAssemblyVersion.Major > serialisedVersion.Major || GameAssemblyVersion.Minor > serialisedVersion.Minor || GameAssemblyVersion.Build > serialisedVersion.Build)
                    _game.IsGameUpdated = true;
            }
            vxSettings.GameVersion = new Serilization.vxSerializableVersion(GameAssemblyVersion);

            // Setup the Debug Systems as the Singletons and Class after will use it
            #region Initialise Debug Systems


            // Initialize the debug system
            vxDebug.Init();

            // Setup the Profiler
            vxProfiler.Init();

            vxProfiler.RegisterMark(vxProfiler.Tags.FRAME_DRAW, Color.Red);
            vxProfiler.RegisterMark(vxProfiler.Tags.FRAME_UPDATE, Color.Yellow);
            vxProfiler.RegisterMark(vxProfiler.Tags.PHYSICS_UPDATE, Color.LimeGreen);
            vxProfiler.RegisterMark(vxProfiler.Tags.UI_DRAW, Color.DeepSkyBlue);

            _game.InitDebugCommands();

            //vxSystemProfiler.Init();
            #endregion

            vxGraphics.Init();

            // Setup the Screen Manager
            vxScreen.Init();

            // Setup the Input Manager
            vxInput.Init();

            // Start the Plugin Manager
            vxPluginManager.Init();

            // Setup the Scene Manager
            vxSceneManager.Init();

            vxAudioManager.Init();


            GUITheme = new vxUITheme();
            GUITheme.SetDefaultTheme();
            Logo = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/logo/logo_72");

            // Setup the Network Manager
            vxNetworkManager.Init(Game.GetNetworkConfig());

            // initialise the localizer
            vxLocalizer.Init();
            
            // TODO: Add Language handling from Player Profile APIs

            //Once all items are Managers and Engine classes are intitalised, then apply any and all passed cmd line arguments
            ApplyCommandLineArgs();

            //Set Initial Graphics Settings
            vxScreen.RefreshGraphics();
            
            // Call this once the Game and graphics Device Object have been created.
            if (_game.HasProfileSupport)
                vxPlatform.Player.InitialisePlayerInfo();

            vxSceneManager.LoadContent();

            vxGraphics.InitMainBuffer();
            vxSettings.Save();
        }



        internal void OnGraphicsRefresh()
        {
            vxLayout.SetLayoutScale(vxGraphics.GraphicsDevice.Viewport.Bounds.Size, vxLayout.IdealScreenSize);
            vxSceneManager.OnGraphicsRefresh();
        }




        /// <summary>
        /// Unload your graphics content.
        /// </summary>
        internal void UnloadContent()
        {
            vxIO.ClearTempDirectory();
            
            string cachePath = Path.Combine(vxIO.PathToCacheFolder, "sandbox_thumbnails");
            try
            {
                if (Directory.Exists(cachePath))
                {
                    Directory.Delete(cachePath, true);
                }
            }
            catch { }

                vxSceneManager.UnloadContent();
            vxAudioManager.Dispose();
        }

        public void Dispose()
        {
            vxNetworkManager.Dispose();

            vxConsole.DumpLog();

            foreach(var proc in processes)
            {
                //proc.CloseMainWindow();
            }
        }

        #endregion

        #region Update and Draw

        public bool Pause = false;

        public bool AlwaysRun = true;

        /// <summary>
        /// Allows each screen to run logic.
        /// </summary>
        internal void Update()
        {
            vxConsole.InGameDebugLines.Clear();

            vxProfiler.Update();

            vxAdManager.Update();

            vxProfiler.BeginMark(vxProfiler.Tags.FRAME_UPDATE);

            // Reset Batch Caller
            vxGraphics.StartNewFrame();

            vxCoroutineManager.Instance.Update();

            // update all player profiles
            vxPlatform.Update();

            if (_game.IsActive && Pause == false || AlwaysRun)
            {
                vxNotificationManager.Update();

                //If the Debug Console is Open, Then don't update or take input
                if (!vxDebug.CommandUI.Focused)
                {
                    // Read the keyboard and gamepad.
                    vxInput.Update();

                    // Now update all scenes 
                    vxSceneManager.Update();
                }
            }

            // update audio manager
            vxAudioManager.Update();

            // Stop measuring time for "Update".
            vxProfiler.EndMark(vxProfiler.Tags.FRAME_UPDATE);

            vxDebug.UpdateTools();
        }



        /// <summary>
        /// Tells each screen to draw itself.
        /// </summary>
        internal void Draw()
        {
            // Start measuring time for "Draw".
            vxProfiler.BeginMark(vxProfiler.Tags.FRAME_DRAW);
            
            // draw all scenes
            vxSceneManager.Draw();

            // finalise the Frame
            vxGraphics.Finalise();

            // draw any notifications on top
            vxNotificationManager.Draw();
            
            DrawDebugFlag();

            // Draw required textures for the Input Manager
            vxInput.Draw();

            if (vxEngine.BuildType == vxBuildType.Debug)
            {
                vxConsole.WriteToScreen("ClearCount: ", vxGraphics.GraphicsDevice.Metrics.ClearCount);
                vxConsole.WriteToScreen("DrawCount: ", vxGraphics.GraphicsDevice.Metrics.DrawCount);
                vxConsole.WriteToScreen("PrimitiveCount: ", vxGraphics.GraphicsDevice.Metrics.PrimitiveCount);
                vxConsole.WriteToScreen("VertexShaderCount: ", vxGraphics.GraphicsDevice.Metrics.VertexShaderCount);
                vxConsole.WriteToScreen("PixelShaderCount: ", vxGraphics.GraphicsDevice.Metrics.PixelShaderCount);
            }

            if (HasLicense == false)
            {
                Vector2 pos = new Vector2(48, vxGraphics.GraphicsDevice.Viewport.Height - 48);
                string txt = "VERTICES ENGINE \nv." + EngineVersion + "\n" + GraphicalBackend;

                // Draw the Trial Text
                vxGraphics.SpriteBatch.DrawString(vxInternalAssets.Fonts.ViewerFont, txt, pos + Vector2.One, Color.Black);
                vxGraphics.SpriteBatch.DrawString(vxInternalAssets.Fonts.ViewerFont, txt, pos, Color.White);
                var rect = new Rectangle(
                5, vxGraphics.GraphicsDevice.Viewport.Height - 48, 72 / 2, 72 / 2);
                rect.Location += new Point(1);
                vxGraphics.SpriteBatch.Draw(Logo, rect, Color.Black * 0.5f);
                rect.Location -= new Point(1);
                vxGraphics.SpriteBatch.Draw(Logo, rect, Color.White);
            }

            vxGraphics.SpriteBatch.End();

            vxProfiler.EndMark(vxProfiler.Tags.FRAME_DRAW);

            // Now draw the Time Ruler Graph outside of the Sprite Batch
            vxDebug.Draw();
            vxConsole.InGameDebugLines.Clear();
        }


        #endregion

        #region Utilty Functions

        /// <summary>
        /// Is this running in batch mode, i.e. will there be no user interaction? 
        /// </summary>
        public static bool IsBatchMode = false;

        /// <summary>
        /// Returns the current scene 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetCurrentScene<T>() where T : vxGameplaySceneBase
        {
            return (T)CurrentScene;
        }


        /// <summary>
        /// Hook this up when the game crashes to catch issues
        /// </summary>
        /// <param name="ex"></param>
        public static void OnCrash(Exception ex)
        {
            Environment.ExitCode = ex.HResult;
            vxConsole.WriteException(ex);
            vxConsole.DumpLogsOnClose = true;

            Game.OnGameCrash(ex);

            var url = Game.GetDashboardURL() + "/logevent";

            var request = System.Net.WebRequest.Create(url);
            request.Method = "POST";

            var user = new
            {
                app = Game.GetAppGuid(),
                version = vxEngine.Game.Version,
                engine = vxEngine.EngineVersion,
                os = vxEngine.PlatformOS.ToString(),
                platform = vxEngine.ReleasePlatformType.ToString(),
                config = vxEngine.BuildType.ToString(),
                type = "fatal",
                code = ex.HResult,
                msg = ex.Message,
                innerMsg = ex.InnerException != null ? ex.InnerException.Message : "none",
                stack = ex.StackTrace
            };


            request.ContentType = "application/json";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                var json = JsonConvert.SerializeObject(user);

                streamWriter.Write(json);
            }

            try
            {
                var httpResponse = (System.Net.HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    vxConsole.WriteLine("log result: " + result);
                }
            }
            catch (Exception ex2)
            {
                vxConsole.WriteException(ex2);
            }
        }

        #endregion

        #region Debug Functions

        internal void RegisterCommand(string command, string description, DebugCommandExecute callback)
        {
            vxDebug.CommandUI.RegisterCommand(command, description, callback);
        }

        /// <summary>
        /// Applies any command line arguments in the .
        /// </summary>
        private void ApplyCommandLineArgs()
        {
            //Get Command Line Arguments that have been passed to the main game
            string[] args = System.Environment.GetCommandLineArgs();

            string argoutput = "Applying Command Line Arguments: ";

            for (int argIndex = 1; argIndex < args.Length; argIndex++)
            {
                argoutput += args[argIndex] + " ";
            }

            //Parse Command Line Arguments Here
            for (int argIndex = 0; argIndex < args.Length; argIndex++)
            {

                try
                {
                    TryToApplyCMDArg(args, argIndex);
                }
                catch (Exception ex)
                {
                    vxConsole.WriteLine(string.Format(" >> cmd line error: {0} <<", ex.Message));
                }

            }
        }

        /// <summary>
        /// Holds all process started by this game
        /// </summary>
        List<System.Diagnostics.Process> processes = new List<System.Diagnostics.Process>();

        /// <summary>
        /// Tries to apply CMD argument.
        /// </summary>
        /// <param name="args">Arguments.</param>
        /// <param name="argIndex">Argument index.</param>
        private void TryToApplyCMDArg(string[] args, int argIndex)
        {
            //Get Argument
            string arg = args[argIndex];

            switch (arg)
            {

                //Sets the Build Conifg too Debug. This for debugging release builds in the wild
                case "-dev":
                case "-debug":
                case "-console":
                    SetBuildType(vxBuildType.Debug);
                    break;

                //Set Resolution Width
                case "-w":
                case "-width":
                    vxDebug.CommandUI.ExecuteCommand(string.Format("width {0}", args[argIndex + 1]));
                    break;
                //Set Resolution Height
                case "-h":
                case "-height":
                    vxDebug.CommandUI.ExecuteCommand(string.Format("height {0}", args[argIndex + 1]));
                    break;

#if !__MOBILE__
                case "-netdev":

                    string newArgs = "";
                    foreach (var sysArg in System.Environment.GetCommandLineArgs())
                    {
                        if (sysArg != "-netdev")
                        {
                            newArgs += " " + sysArg;
                        }
                    }

                    newArgs += "-wpos 256 128";

                    if (vxEngine.PlatformOS == vxPlatformOS.Windows)
                    {
                        var proc = System.Diagnostics.Process.Start(Game.GetProcessName() + ".exe", newArgs);
                        processes.Add(proc);
                    }
                    else
                    {
                        var proc = System.Diagnostics.Process.Start(Game.GetProcessName(), newArgs);
                        processes.Add(proc);
                    }

                    _game.Window.Position = new Point(1800, 128);
                    _game.Window.Title += "NET DEV PLAYER 1 - ";

                    break;
#endif

                case "-wpos":
#if !__MOBILE__
                    _game.Window.Position = new Point(int.Parse(args[argIndex + 1]), int.Parse(args[argIndex + 2]));
#endif
                    break;
                case "-sw":
                case "-startwindowed":
                case "-window":
                case "-windowed":
                    vxDebug.CommandUI.ExecuteCommand(string.Format("windowed"));
                    break;

                //Set Fullsreen
                case "-full":
                case "-fullscreen":
                    vxDebug.CommandUI.ExecuteCommand(string.Format("fullscreen"));
                    break;

                case "-fps":
                    vxDebug.CommandUI.ExecuteCommand(string.Format("fps"));
                    break;
                case "-tr":
                    vxDebug.CommandUI.ExecuteCommand(string.Format("tr"));
                    break;

                case "-v":
                    vxDebug.IsDebugOutputVerbose = true;
                    break;

                // only load mods if it's in debug/dev mode
                case "-mod":
                    if (BuildType == vxBuildType.Debug)
                    {
                        try
                        {
                            DevModPaths.Add(args[argIndex + 1]);
                            //PluginManager.LoadAssembly(args[argIndex + 1]);
                        }
                        catch (Exception ex)
                        {
                            vxConsole.WriteException(this, ex);
                        }
                    }
                    break;
            }
        }

        List<string> DevModPaths = new List<string>();
        internal string[] LoadDevMods()
        {
            //DevModPaths.Add(@"C:\Users\rtroe\Documents\GitHub\chaoticworkshop-modkit\ChaoticWorkshopModKit\bin\Debug\ChaoticWorkshop.Mods.Template.dll");
            return DevModPaths.ToArray();
        }

        public void DrawDebugFlag()
        {
            //enginevtext += "\n"+GameName + " v." + GameVersion;
            //enginevtext += "\nVertices Engine v." + EngineVersion + " - (" + PlatformType + ")";
            if (DevModPaths.Count > 0)
            {
                SpriteFont engineFont = vxInternalAssets.Fonts.ViewerFont;

                // Engine Version Text
                string enginevtext = _game.Name + " - Dev Mode\n========================\n";

                enginevtext += "Loaded Development DLL's:\n";

                foreach (var dll in DevModPaths)
                    enginevtext += "  " + dll + "\n";

                // Text Size + Padding and offset from screen edges
                Vector2 EngineTextSize = engineFont.MeasureString(enginevtext);
                int padding = 5;

                vxGraphics.SpriteBatch.DrawString(vxInternalAssets.Fonts.ViewerFont,
                                       enginevtext,
                                       (padding + 1) * Vector2.One,
                                       Color.Black);

                vxGraphics.SpriteBatch.DrawString(vxInternalAssets.Fonts.ViewerFont,
                                       enginevtext,
                                       (padding) * Vector2.One,
                                       Color.Lime);
            }
        }

        /// <summary>
        /// Draws the version info in the corner of the screen.
        /// </summary>
        /// <param name="color">Color to draw the text with.</param>
        /// <param name="alpha">Alpha to draw with, usually the TransitionAlpha value for the current screen.</param>
        public void DrawVersionInfo(Color color, float alpha)
        {
            float engineTextAlpha = 0.75f;

            SpriteFont engineFont = vxInternalAssets.Fonts.ViewerFont;

            // Engine Version Text
            string enginevtext = string.Format("[{0} {1} Build] - v. {2} - Engine v." + EngineVersion, PlatformOS, _buildConfigType, _game.Version);

            // Text Size + Padding and offset from screen edges
            Vector2 EngineTextSize = engineFont.MeasureString(enginevtext);
            int padding = 5;
            int offset = 5;

            // Set the text position
            Vector2 EngineTextPosition = new Vector2(offset + padding, vxGraphics.GraphicsDevice.Viewport.Height - EngineTextSize.Y - (offset + padding));

            vxGraphics.SpriteBatch.DrawString(engineFont,
                                   enginevtext,
                                   EngineTextPosition,
                                   color * engineTextAlpha * alpha);
        }


        void DrawIPContent(Color color, float alpha)
        {
            SpriteFont engineFont = vxInternalAssets.Fonts.ViewerFont;

            // Engine Version Text
            string enginevtext = "Engine v." + EngineVersion + " - (" + PlatformOS + ")";

            // Text Size + Padding and offset from screen edges
            Vector2 EngineTextSize = engineFont.MeasureString(enginevtext);
            int padding = 5;
            int offset = 5;

            // Set the text position
            Vector2 EngineTextPosition = new Vector2(offset + padding, vxGraphics.GraphicsDevice.Viewport.Height - EngineTextSize.Y - (offset + padding));

            vxGraphics.SpriteBatch.DrawString(vxInternalAssets.Fonts.ViewerFont, enginevtext, EngineTextPosition, color * 0.75f * alpha);
        }

        #endregion

#if __ANDROID__

        public Android.App.Activity Activity;

        public void OnStart()
        {
            try
            {
                //game.Engine.PlayerProfile
                if (Game != null)
                    ((vxGooglePlayServicePlatformWrapper)vxPlatform.Player).Start();
            }
            catch (Exception ex)
            {
                vxCrashHandler.Thwrow(ex);
            }
        }

        public void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            try
            {
                // call any in app billing calls
                //InAppBillingImplementation.HandleActivityResult(requestCode, resultCode, data);
                

                // fire any activity results
                if (Game != null)
                    ((vxGooglePlayServicePlatformWrapper)vxPlatform.Player).OnActivityResult(requestCode, resultCode, data);


            }
            catch (Exception ex)
            {
                vxCrashHandler.Thwrow(ex);
            }
        }

        public void OnStop()
        {
            try
            {
                if (Game != null)
                    ((vxGooglePlayServicePlatformWrapper)vxPlatform.Player).Stop();
            }
            catch (Exception ex)
            {
                vxCrashHandler.Thwrow(ex);
            }
        }
#endif
    }
}