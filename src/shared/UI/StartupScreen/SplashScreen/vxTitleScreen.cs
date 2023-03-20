#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using VerticesEngine.ContentManagement;
using VerticesEngine.Graphics;
using VerticesEngine.Input;
using VerticesEngine.Screens.Async;

#endregion

namespace VerticesEngine.UI.StartupScreen
{
    /// <summary>
    /// This is the Vertices Engine Title Screen which is the first screen when the Engine is launched.
    /// </summary>
    public class vxTitleScreen : vxBaseScene
    {
        #region Fields


        public static bool IsDarkStart = false;

        public static Color LightCol = new Color(1f, 1f, 1f, 1f);
        public static Color DarkCol = new Color(0.15f, 0.15f, 0.15f, 1);

        internal static float FogStart = 0;

        internal static float FogEnd = 12;

        /// <summary>
        /// The title screen camera view
        /// </summary>
        public Matrix CameraView
        {
            get { return _view; }
        }
        public Matrix _view = Matrix.CreateLookAt(new Vector3(0, 0, 4), new Vector3(0, 0, 0), new Vector3(0, 1, 0));

        /// <summary>
        /// The title screen camera projection
        /// </summary>
        public Matrix CameraProjection
        {
            get { return _projection; }
        }
        private Matrix _projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 1280f / 720f, 0.01f, 10000f);

        public vxMesh SphereModel;

        List<TitleScreenPlexusTri> Tris = new List<TitleScreenPlexusTri>();

        ContentManager content;

        SpriteFont TitleFont;

        Texture2D Logo;

        Texture2D EngineTitle;

        Texture2D Splitter;

        Texture2D BuiltWithTexture;

        float pauseAlpha;

        float UpdateCount = 0;

        KeyboardState CurrentKeyboardState = new KeyboardState();
        KeyboardState PreviousKeyboardState = new KeyboardState();

        #endregion

        #region Initialization

#if DEBUG
        float UpdateTime = 0.25f;
#else
        float UpdateTime = 5;
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="VerticesEngine.Screens.TitleScreen"/> class.
        /// </summary>
		public vxTitleScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(1.5);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VerticesEngine.Screens.TitleScreen"/> class.
        /// </summary>
        /// <param name="UpdateTime">Update time.</param>
        internal vxTitleScreen(int UpdateTime) : this()
        {
            this.UpdateTime = UpdateTime;
        }


        /// <summary>
        /// Loads graphics content for this screen. The background texture is quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, wheras if we
        /// used the shared ContentManager provided by the Game class, the content
        /// would remain loaded forever.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(vxEngine.Game.Services, "Content");

            TitleFont = vxContentManager.Instance.Load<SpriteFont>("vxengine/fonts/font_splash_24");
            SphereModel = vxContentManager.Instance.LoadMesh("vxengine/models/titleScreen/sphere/sphere");

            
            Logo = vxContentManager.Instance.Load<Texture2D>("vxengine/models/titleScreen/logo/vrtx/vrtx_title" + (IsDarkStart ? "_dark" : ""));
            EngineTitle = vxContentManager.Instance.Load<Texture2D>("vxengine/models/titleScreen/logo/vrtc/vrtc_title" + (IsDarkStart ? "_dark" : ""));
            Splitter = vxContentManager.Instance.Load<Texture2D>("vxengine/models/titleScreen/logo/spliiter" + (IsDarkStart ? "_dark" : ""));
            BuiltWithTexture = vxContentManager.Instance.Load<Texture2D>("vxengine/models/titleScreen/logo/vrtc/built_with" + (IsDarkStart ? "_dark" : ""));
            
            try
            {
                vxLoadAssetsScreen.SplashScreen = vxEngine.Game.Content.Load<Texture2D>("SplashScreen");
                vxConsole.WriteLine("Loading Custom Splash Screen...");
            }
            catch
            {
                //vxConsole.WriteLine("Custom Splash Screen Not Fount...");
            }

            Vector3[] Positions ={
                new Vector3(0,0,0),
                new Vector3(-2,0,-3),
                new Vector3(1,1,-2),
                new Vector3(1,-2,-4),
                new Vector3(-1.5f,1,2),
                new Vector3(2,2,-5),
                new Vector3(-2,1,-5),
                new Vector3(-1,-2,-6),
                new Vector3(-4,-2,-3)
            };

            for (int i = 0; i < Positions.Length; i++)
                Tris.Add(new TitleScreenPlexusTri(this, Positions[i]));
        }


        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();

            Tris.Clear();
            
            vxSceneManager.AddScene(vxEngine.Game.OnShowInitScreen());
        }


        #endregion

        #region Update and Draw



        bool MainEntryFired = false;
        /// <summary>
        /// Updates the background screen. Unlike most screens, this should not
        /// transition off even if it has been covered by another screen: it is
        /// supposed to be covered, after all! This overload forces the
        /// coveredByOtherScreen parameter to false in order to stop the base
        /// Update method wanting to transition off.
        /// </summary>
        protected internal override void Update()
        {
            #region Fade and Base Update

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            #endregion

            _projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(60), vxScreen.Width / vxScreen.Height, 0.01f, 10000f);

            CurrentKeyboardState = Keyboard.GetState();

            UpdateCount += vxTime.DeltaTime;

            if (MainEntryFired == false)
            {
                if (UpdateCount > UpdateTime || CurrentKeyboardState.IsKeyDown(Keys.Enter) ||
                    vxEngine.Game.IsGameContentLoaded || vxInput.IsNewTouchPressed() == true)
                {
                    MainEntryFired = true;
                    ExitScreen();
                }
            }

            FogEnd = 12 * Math.Min(UpdateCount, 4) / 4;


            foreach (TitleScreenPlexusTri tri in Tris)
                tri.Update();

            PreviousKeyboardState = CurrentKeyboardState;

            base.Update();
        }


        /// <summary>
        /// Draws the background screen.
        /// </summary>
        /// 
        public override void Draw()
        {
            vxSpriteBatch spriteBatch = vxGraphics.SpriteBatch;
            Viewport viewport = vxGraphics.GraphicsDevice.Viewport;

            vxGraphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            vxGraphics.GraphicsDevice.Clear(ClearOptions.Target, (IsDarkStart ? DarkCol : Color.White), 0, 0);

            foreach (TitleScreenPlexusTri tri in Tris)
            {
                tri.Draw();
            }

            //Draw SpriteBatch
            vxEngine.Game.GraphicsDevice.SamplerStates[1] = SamplerState.AnisotropicClamp;

            spriteBatch.Begin("Title Screen");

            //Draw Version Information
            vxEngine.Instance.DrawVersionInfo(Color.DimGray, TransitionAlpha);

            float scale = 2;

            float scalefactor = Math.Min(UpdateCount, 2) / 2;


            Rectangle VrtxLogo = new Rectangle(
            viewport.Width / 2 - Logo.Width / 2,
            viewport.Height * 1 / 3 - Logo.Height / 2,
                Logo.Width, Logo.Height);


            // Draw the Engine Title
            spriteBatch.Draw(Logo, VrtxLogo, Color.White * TransitionAlpha * (Math.Min(UpdateCount, 1) / 1));

            //spriteBatch.DrawString(TitleFont, "Virtex Edge Design", new Vector2(vxScreen.Width / 2, vxScreen.Height / 3),
            //    IsDarkStart ? LightCol : DarkCol, 1, horizontalJustification: vxHorizontalJustification.Center);

            //spriteBatch.Draw(Logo,
            // new Vector2(
            //	 viewport.Width / 2,
            //	 viewport.Height / 2 - 1.5f * EngineTitle.Bounds.Height / scale - 8), null,
            // Color.White * TransitionAlpha * (Math.Min(UpdateCount, 1) / 1) * 0.5f,
            //0,
            // new Vector2(BuiltWithTexture.Width / 2, BuiltWithTexture.Height / 2),
            //Vector2.One / scale, SpriteEffects.None, 0);

            // //Draw the Engine Title
            //spriteBatch.Draw(EngineTitle,
            //new Vector2(
            //             viewport.Width / 2,
            // viewport.Height / 2 - EngineTitle.Bounds.Height/scale-2), null,
            //Color.White * TransitionAlpha * (Math.Min(UpdateCount, 2) / 2),
            //           0,
            //new Vector2(EngineTitle.Width / 2, EngineTitle.Height / 2),
            //Vector2.One/scale, SpriteEffects.None,0);

            float BuiltWithHeight = viewport.Height -
                                            EngineTitle.Bounds.Height / scale -
                                            BuiltWithTexture.Bounds.Height / scale - 25;


            //Draw the Engine Title
            spriteBatch.Draw(BuiltWithTexture,
                             new Vector2(
                                 viewport.Width / 2,
                                 BuiltWithHeight), null,
                             Color.White * TransitionAlpha * (Math.Min(UpdateCount, 1) / 1) * 0.5f,
                            0,
                             new Vector2(BuiltWithTexture.Width / 2, BuiltWithTexture.Height / 2),
                            Vector2.One / scale, SpriteEffects.None, 0);


            // Draw the Splitter
            spriteBatch.Draw(Splitter,
                             new Vector2(
                                 viewport.Width / 2,
                                 BuiltWithHeight + BuiltWithTexture.Bounds.Height / scale - 4), null,
                             Color.White * TransitionAlpha * (Math.Min(UpdateCount, 4) / 2 - 1) * 0.5f,
                            0,
                             new Vector2(Splitter.Width / 2, Splitter.Height / 2),
                             new Vector2((Math.Min(UpdateCount, 4) / 2 - 1), 1) / scale, SpriteEffects.None, 0);


            scale = 2.75f;
            //Draw the Logo
            spriteBatch.Draw(EngineTitle,
                             new Vector2(
                                 viewport.Width / 2,
                                 BuiltWithHeight + BuiltWithTexture.Bounds.Height / scale), null,
                             Color.White * TransitionAlpha * (Math.Min(UpdateCount, 4) / 2 - 1),
                            0,
                             new Vector2(EngineTitle.Width / 2, 0),
                            Vector2.One / scale, SpriteEffects.None, 0);

            spriteBatch.End();

            #region Transition Code
            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                vxSceneManager.FadeBackBufferToBlack(alpha);
            }
            #endregion
        }

        #endregion
    }
}
