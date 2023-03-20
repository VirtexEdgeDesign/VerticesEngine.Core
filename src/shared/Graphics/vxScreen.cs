using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VerticesEngine.Diagnostics;
using VerticesEngine.Graphics;
using VerticesEngine.UI;
using VerticesEngine.Utilities;

namespace VerticesEngine
{
    /// <summary>
    /// The Engine Graphical Settings Manager which holds all of settings for engine graphics such as
    /// resolution, fullscreen, vsync as well as other Depth of Field toggle or Cascade Shadow qaulity.
    /// </summary>
    public static class vxScreen
    {
        /// <summary>
        /// Gets the graphics device manager.
        /// </summary>
        /// <value>The graphics device manager.</value>
        static GraphicsDeviceManager GraphicsDeviceManager;


        /// <summary>
        /// Gets or sets the resolution from 'Engine.Settings.Graphics.Screen'. NOTE: Apply needs to be called to apply these settings.
        /// </summary>
        /// <value>The resolution.</value>
        public static Point Resolution
        {
            get { return _resolution; }
        }
#if DEBUG
        static Point _resolution = new Point(1376, 760);
#else  
        static Point _resolution = new Point(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
#endif

        /// <summary>
        /// Screen Width. You must call <see cref="RefreshGraphics()"/> to apply this change.
        /// </summary>
        [vxGraphicalSettings("ResolutionX")]
        public static int Width
        {
            get { return _resolution.X; }
            set { _resolution = new Point(value, _resolution.Y); }
        }

        /// <summary>
        /// Screen Height. You must call <see cref="RefreshGraphics()"/> to apply this change.
        /// </summary>
        [vxGraphicalSettings("ResolutionY")]
        public static int Height
        {
            get { return _resolution.Y; }
            set { _resolution = new Point(_resolution.X, value); }
        }

        /// <summary>
        /// Gets or sets a value from 'Engine.Settings.Graphics.Screen' indicating whether this instance is full screen.
        /// NOTE: Apply needs to be called to apply these settings.
        /// </summary>
        /// <value><c>true</c> if this instance is full screen; otherwise, <c>false</c>.</value>
        [vxGraphicalSettings("FullScreenMode")]
        public static vxFullScreenMode FullScreenMode
        {
            get { return _fullScreenMode; }
            set
            {
                _fullScreenMode = value;
            }
        }

#if DEBUG
        private static vxFullScreenMode _fullScreenMode = vxFullScreenMode.Windowed;
#else
        private static vxFullScreenMode _fullScreenMode = vxFullScreenMode.Fullscreen;
#endif


        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:VerticesEngine.Graphics.vxGraphicsSettingsManager"/> is
        /// VS ync on.
        /// </summary>
        /// <value><c>true</c> if is VS ync on; otherwise, <c>false</c>.</value>
        [vxGraphicalSettings("IsVSyncOn")]
        public static bool IsVSyncOn
        {
            get { return _isVSyncOn; }
            set { _isVSyncOn = value; }
        }
        static bool _isVSyncOn = true;

        /// <summary>
        /// The currently configured Safe Aera
        /// </summary>
        public static Viewport SafeArea
        {
            get { return _safeArea; }
        }
        private  static Viewport _safeArea = new Viewport(0,0,1,1);

        private static Vector4 _margin;
        

        /// <summary>
        /// The viewport for the screen. Note this can be scaled and stretched to match DPI and window settings.
        /// This can be different than <see cref="SafeArea"/>
        /// </summary>
        public static Viewport Viewport
        {
            get { return _viewport; }
        }
        private static Viewport _viewport = new Viewport(0,0,1,1);
        
        /// <summary>
        /// Initialise the screen subsystem.
        /// </summary>
        internal static void Init()
        {
            vxConsole.WriteLine("Starting Graphics Settings Manager...");

            GraphicsDeviceManager = vxEngine.Game.Services.GetService(typeof(IGraphicsDeviceService)) as GraphicsDeviceManager;

            vxDebug.CommandUI.RegisterCommand(
                "graref",              // Name of command
                "Refresh the Grapahics Settings",     // Description of command
                delegate (IDebugCommandHost host, string command, IList<string> args)
                {
                    RefreshGraphics();
                });
#if __MOBILE__
            FullScreenMode = vxFullScreenMode.Fullscreen;
            SetResolution(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
#endif
        }

        /// <summary>
        /// Set's the Window Position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void SetWindowPos(int x, int y)
        {
#if !__MOBILE__
            if (vxEngine.PlatformOS == vxPlatformOS.Windows)
            {
                vxEngine.Game.Window.Position = new Point(x, y);
            }
#endif
        }
        
        
        /// <summary>
        /// Sets the resolution
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void SetResolution(int x, int y)
        {
            SetResolution(new Point(x, y));
        }

        /// <summary>
        /// Sets the resolution
        /// </summary>
        /// <param name="point"></param>
        public static void SetResolution(Point point)
        {
            _resolution = point;
        }

        /// <summary>
        /// Sets the Safe Area margin
        /// </summary>
        /// <param name="margin"></param>
        public static void SetSafeArea(Vector4 margin)
        {
            _margin = margin;
            
            _safeArea = new Viewport(
                _viewport.X + (int)_margin.X, 
                _viewport.Y +(int)_margin.Y,
                _viewport.Width -(int) _margin.Z, 
                _viewport.Height - (int)_margin.W);
        }

        /// <summary>
        /// Sets the Safe Area margin
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="w"></param>
        public static void SetSafeArea(int x, int y, int z, int w)
        {
            SetSafeArea(new Vector4(x, y, z, w));
        }


        /// <summary>
        /// Applies the Current Graphics Settings list Resolution and Fullscreen.
        /// </summary>
        public static void RefreshGraphics()
        {
            // Don't set resolution for Mobile.
#if __MOBILE__

            // Set the Window Flags here
#if __ANDROID__
            Microsoft.Xna.Framework.Game.Activity.Window.AddFlags(Android.Views.WindowManagerFlags.Fullscreen);
            Microsoft.Xna.Framework.Game.Activity.Window.ClearFlags(Android.Views.WindowManagerFlags.ForceNotFullscreen);
#endif

            GraphicsDeviceManager.PreferredBackBufferWidth = vxScreen.Width;
            GraphicsDeviceManager.PreferredBackBufferHeight = vxScreen.Height;

            // Set FullScreen Value
            GraphicsDeviceManager.IsFullScreen = true;
            FullScreenMode = vxFullScreenMode.Fullscreen;
            _viewport = new Viewport(0, 0, _resolution.X, _resolution.Y);
#else
            _viewport = new Viewport(0, 0, _resolution.X, _resolution.Y);

            // Set Resolution
            // *****************************
            GraphicsDeviceManager.PreferredBackBufferWidth = Resolution.X;
            GraphicsDeviceManager.PreferredBackBufferHeight = Resolution.Y;
            
            if (_fullScreenMode== vxFullScreenMode.Fullscreen)
            {
                GraphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                GraphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }
            
            // Set FullScreen Value
            GraphicsDeviceManager.HardwareModeSwitch = true;
            GraphicsDeviceManager.IsFullScreen = (FullScreenMode == vxFullScreenMode.Fullscreen);
            
            // VSync Values
            vxEngine.Game.IsFixedTimeStep = _isVSyncOn;
            GraphicsDeviceManager.SynchronizeWithVerticalRetrace = _isVSyncOn;
#endif
            GraphicsDeviceManager.GraphicsDevice.Viewport = new Viewport(0, 0, _resolution.X, _resolution.Y);
            SetSafeArea(_margin);
            //_safeArea = vxLayout.GetRect(_margin.X, _margin.Y, _resolution.X - _margin.Z, _resolution.Y - _margin.W);
            
            vxDebug.Log(new
            {
                resolution = _resolution,
                FullScreenMode = _fullScreenMode,
                isVSyncOn = _isVSyncOn,
                safeArea = _safeArea,
            });

            // Refresh the Graphics
            GraphicsDeviceManager.ApplyChanges();

            if (_fullScreenMode== vxFullScreenMode.Fullscreen)
            {
                GraphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                GraphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }
#if !__MOBILE__
            vxEngine.Game.Window.IsBorderless = (_fullScreenMode == vxFullScreenMode.Borderless);
#endif
            if (_fullScreenMode == vxFullScreenMode.Borderless)
            {
                SetWindowPos(0,0);
            }
            else if(_fullScreenMode == vxFullScreenMode.Windowed)
            {
#if !__MOBILE__
                if (vxEngine.Game.Window.Position.Y < 32)
                    SetWindowPos(0, 32);
#endif
            }


            // Now tell all scenes to reset
            vxEngine.Instance.OnGraphicsRefresh();

            OnScreenResChanged?.Invoke();
        }

        /// <summary>
        /// Called when ever the Graphics Settings are refreshed and applied.
        /// </summary>
        public static event Action OnScreenResChanged = () => { };

        /// <summary>
        /// Our we currently taking a screenshot while rendering? This is useful for setting specific
        /// render settings if we are taking a screenshot.
        /// </summary>
        public static bool IsTakingScreenshot
        {
            get { return m_isTakingScreenshot; }
        }
        private static bool m_isTakingScreenshot = false;

        
        #region -- Util Functions --
        
        /// <summary>
        /// Takes a screenshot.
        /// </summary>
        public static Texture2D TakeScreenshot(bool forceFrameDraw = false, bool suppressUI = false, bool suppressSandboxItems = true)
        {
            if (forceFrameDraw)
            {
                m_isTakingScreenshot = true;
                vxEngine.Instance.CurrentScene.IsUIVisibilitySuppressed = suppressUI;
                var originalSandboxState = vxEngine.Instance.CurrentScene.SandboxCurrentState = vxEnumSandboxStatus.EditMode;
                if (suppressSandboxItems)
                {
                    vxEngine.Instance.CurrentScene.SandboxCurrentState = vxEnumSandboxStatus.Running;
                }
                vxEngine.Instance.Draw();
                vxEngine.Instance.CurrentScene.IsUIVisibilitySuppressed = false;
                vxEngine.Instance.CurrentScene.SandboxCurrentState = originalSandboxState;

                m_isTakingScreenshot = false;
            }
            return vxGraphics.FinalBackBuffer.Resize(vxGraphics.FinalBackBuffer.Width, vxGraphics.FinalBackBuffer.Height);
        }
        
        #endregion
        
        #region -- Debug Functions --
        
        static void DebugSettingChange(string name, object setting)
        {
            vxConsole.WriteLine(string.Format("     {0} : {1}", name, setting));
        }

        public static void DebugDump(string area)
        {
            Console.WriteLine("VIEWPORT DUMP: " + area);

            var GraphicsDeviceManager = vxEngine.Game.Services.GetService(typeof(IGraphicsDeviceService)) as GraphicsDeviceManager;
            
            Console.WriteLine("DefaultAdapter");
            Console.WriteLine(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width + "x" + GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

            Console.WriteLine("CurrentDisplayMode");
            Console.WriteLine(vxGraphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width + "x" + vxGraphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height);


            PresentationParameters pp = GraphicsDeviceManager.GraphicsDevice.PresentationParameters;
            
            Console.WriteLine("BackBufferWidth");
            Console.WriteLine(pp.BackBufferWidth + "x" + pp.BackBufferHeight);


            var w = GraphicsDeviceManager.PreferredBackBufferWidth;
            var h = GraphicsDeviceManager.PreferredBackBufferHeight;

            Console.WriteLine("PreferredBackBuffer");
            Console.WriteLine(w + "x" + h);

            var xB = GraphicsDeviceManager.GraphicsDevice.Viewport.Bounds.Width;
            var yB = GraphicsDeviceManager.GraphicsDevice.Viewport.Bounds.Height;
            Console.WriteLine("Viewport");
            Console.WriteLine(xB + "x" + yB);
        }

        #endregion
    }
}

