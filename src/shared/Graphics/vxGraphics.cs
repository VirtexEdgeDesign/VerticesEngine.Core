using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VerticesEngine.Graphics
{
    /// <summary>
    /// Graphical Variables like the Main Sprite Sheet and Getting New Render Textures
    /// </summary>
    public static class vxGraphics
    {
        private static RasterizerState _wireframeRasterizerState;
        private static RasterizerState _solidRasterizerState;

        public static GraphicsDeviceManager DeviceManager
        {
            get { return vxEngine.Game.GraphicsDeviceManager; }
        }

        /// <summary>
        /// The current Engine Graphics Device
        /// </summary>
        public static GraphicsDevice GraphicsDevice
        {
            get { return vxEngine.Game.GraphicsDevice; }
        }

        /// <summary>
        /// The Current Presentation Parameters
        /// </summary>
        public static PresentationParameters PresentationParameters
        {
            get { return vxEngine.Game.GraphicsDevice.PresentationParameters; }
        }



        /// <summary>
        /// The main render target whithout any GUI and Overlay Items.
        /// </summary>
        public static RenderTarget2D FinalBackBuffer
        {
            get
            {
                return _finalBackBuffer;
            }
        }
        private static RenderTarget2D _finalBackBuffer;

        
        /// <summary>
        /// The final viewport size, i.e. the actual device or screen resolution.
        /// </summary>
        public static Viewport FinalViewport
        {
            get { return _finalViewport; }
        }
        private static Viewport _finalViewport = new Viewport(0, 0, 1200, 720);
        
        /// <summary>
        /// Line Batch Manager which draw's a number of 2D Lines to the screen.
        /// </summary>
        /// <value>The line batch.</value>
        public static vxLineBatch LineBatch
        {
            get { return _lineBatch; }
        }
        private static vxLineBatch _lineBatch;


        /// <summary>
        /// A default SpriteBatch shared by all the screens. This saves
        /// each screen having to bother creating their own local instance.
        /// </summary>
        public static vxSpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
        }
        private static vxSpriteBatch _spriteBatch;

        /// <summary>
        /// The global sprite sheet which all draw calls should reference for effeciency.
        /// </summary>
        public static Texture2D MainSpriteSheet;

        /// <summary>
        /// Utilities for graphics
        /// </summary>
        public static class Util
        {
            /// <summary>
            /// A Wireframe shader which is useful for highlighting meshes in the editor
            /// </summary>
            public static vxDebugEffect WireframeShader;

            public static vxEditorTempEntityEffect EditorTempEntityShader;
        }

        /// <summary>
        /// Initialiases any graphical objects
        /// </summary>
        internal static void Init()
        {
            _lineBatch = new vxLineBatch(GraphicsDevice);
            _spriteBatch = new vxSpriteBatch(GraphicsDevice);
            _wireframeRasterizerState = new RasterizerState() { FillMode = FillMode.WireFrame, };
            _solidRasterizerState = new RasterizerState() { FillMode = FillMode.Solid, };
        }

        internal static void StartNewFrame()
        {
            _spriteBatch.StartNewFrame();
        }

        /// <summary>
        /// Inits the main rendertarget which is the final screen which is drawn.
        /// </summary>
        public static void InitMainBuffer()
        {
#if __IOS__
            _finalBackBuffer = new RenderTarget2D(GraphicsDevice, vxScreen.Width, vxScreen.Height, false, SurfaceFormat.Color, DepthFormat.Depth24);

            _finalViewport = new Viewport(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
#else

            _finalBackBuffer = new RenderTarget2D(GraphicsDevice, vxScreen.Width, vxScreen.Height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);

            if (vxScreen.FullScreenMode == vxFullScreenMode.Fullscreen)
            {
                _finalViewport = new Viewport(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            }
            else
            {
                _finalViewport = new Viewport(0, 0, vxScreen.Width, vxScreen.Height);
            }
#endif

            }

        public static void SetRenderTarget(RenderTarget2D renderTarget)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
        }


        /// <summary>
        /// Finalises the Frame 
        /// </summary>
        public static void Finalise()
        {
            // Get the Final Scene
            vxEngine.Game.GraphicsDevice.SetRenderTarget(null);
            vxGraphics.SpriteBatch.Begin("Engine.DrawScenesToBackBuffer");
            vxGraphics.SpriteBatch.Draw(FinalBackBuffer, FinalViewport.Bounds, Color.White);
        }



        /// <summary>
        /// Sets the rasterizer state which allows for switching between Wireframe and Solid
        /// </summary>
        /// <param name="fillMode"></param>
        public static void SetRasterizerState(FillMode fillMode)
        {
            GraphicsDevice.RasterizerState = fillMode == FillMode.WireFrame ? _wireframeRasterizerState : _solidRasterizerState;
        }


        /// <summary>
        /// Creates a new render target using the current resolution
        /// </summary>
        /// <returns></returns>
        public static RenderTarget2D GetNewRenderTarget()
        {
            return GetNewRenderTarget(1);
        }

        public static RenderTarget2D GetNewRenderTarget(float scale)
        {
            return GetNewRenderTarget((int)(PresentationParameters.BackBufferWidth * scale), (int)(PresentationParameters.BackBufferHeight * scale));
        }

        /// <summary>
        /// Creates a new render target using the current resolution with the specified width and height
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="mipmap"></param>
        /// <returns></returns>
        public static RenderTarget2D GetNewRenderTarget(int width, int height, bool mipmap = false)
        {
            return new RenderTarget2D(GraphicsDevice, width, height, mipmap, PresentationParameters.BackBufferFormat, PresentationParameters.DepthStencilFormat);
        }

    }
}
