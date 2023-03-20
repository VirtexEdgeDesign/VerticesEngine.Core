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
using VerticesEngine.Profile;
using VerticesEngine.UI.MessageBoxs;
using VerticesEngine.UI.Themes;
using VerticesEngine.InitSteps;
using VerticesEngine.Plugins;

#endregion

namespace VerticesEngine.UI.StartupScreen
{
    /// <summary>
    /// The Init Screen which calls all required setup screens on start of launch 
    /// </summary>
    public class vxInitializationScreen : vxBaseScene
    {
        #region Fields



        /// <summary>
        /// The background texture used for the Init screen which should be located in your main game content at 'ui/screen_init/init_background'
        /// </summary>
        protected Texture2D backgroundTexture;



        protected ContentManager content;


        /// <summary>
        /// The delay between stages to trigger
        /// </summary>
        #if DEBUG
        float m_delay = 0;
#else
        float m_delay = 1;
#endif
        /// <summary>
        /// The current delay time before the next stage will trigger
        /// </summary>
        float m_currentDelay = 0;


        float m_pauseAlpha;

        bool MainEntryFired = false;

#endregion

#region Initialization

        /// <summary>
        /// The list of initialization steps for the game to go through when starting up
        /// </summary>
        Queue<vxIInitializationStep> steps = new Queue<vxIInitializationStep>();

        protected vxIInitializationStep activeStep = null;

       public static List<vxIInitializationStep> InitSteps = new List<vxIInitializationStep>();

        /// <summary>
        /// Initializes a new instance of the <see cref="VerticesEngine.UI.StartupScreen.vxInitializationScreen"/> class.
        /// </summary>
		public vxInitializationScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            //vxEngine.Game.InitializationStage = GameInitializationStage.NotifyOfPermissions;
            AddStep(new PermissionsRequestStep());
            AddStep(new SignInUserInitStep());
            AddStep(new LoadPlayerProfileStep());
            AddStep(new LoadGlobalContentStep());

            // now add in all of the content pack loading
            AddStep(new LoadContentPackStep(vxPluginManager.CoreGameModule));

            foreach (var dlc in vxPluginManager.DLCPacks.Values)
                AddStep(new LoadContentPackStep(dlc, vxPluginType.DLC));

            foreach (var mod in vxPluginManager.ModPacks.Values)
                AddStep(new LoadContentPackStep(mod, vxPluginType.Mod));

            vxEngine.Game.OnInitStepsSetup();
        }

        public static void AddStep<T>() where T : vxIInitializationStep
        {
            var newStep = System.Activator.CreateInstance<T>();
            AddStep(newStep);
            // System.Reflection.ConstructorInfo ctor = typeof(T).GetConstructor(Type.EmptyTypes);
            // if (ctor == null)
            // {
            //     var res =(vxIInitializationStep)ctor.Invoke(null);
            //     AddStep(res);
            // }
        }
        
        public static void AddStep<T>(int index) where T : vxIInitializationStep
        {
            var newStep = System.Activator.CreateInstance<T>();
            AddStep(newStep, index);
            // System.Reflection.ConstructorInfo ctor = typeof(T).GetConstructor(Type.EmptyTypes);
            // if (ctor == null)
            // {
            //     var res =(vxIInitializationStep)ctor.Invoke(null);
            //     AddStep(res);
            // }
        }

        public static void AddStep(vxIInitializationStep step)
        {
            InitSteps.Add(step);
        }

        public static void AddStep(vxIInitializationStep step, int index)
        {
            InitSteps.Insert(index, step);
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

            try
            {
                backgroundTexture = content.Load<Texture2D>("ui/screen_init/init_background");
            }
            catch
            {
                backgroundTexture = vxInternalAssets.Textures.Blank;
            }

            IsContentLoaded = true;

            foreach (var step in InitSteps)
                steps.Enqueue(step);

            // get the next item in the queue
            activeStep = steps.Dequeue();
            activeStep?.Start();
        }


        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


#endregion

#region Update and Draw


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
                m_pauseAlpha = Math.Min(m_pauseAlpha + 1f / 32, 1);
            else
                m_pauseAlpha = Math.Max(m_pauseAlpha - 1f / 32, 0);

#endregion


            base.Update();

            if (activeStep != null || vxEngine.Game.InitializationStage == GameInitializationStage.Running)
            {
                // check if the active step is complete, if it is, then get the next one
                if(activeStep.IsComplete && m_currentDelay < 0)
                {   
                    activeStep = steps.Count > 0 ? steps.Dequeue() : null;
                    activeStep?.Start();
                    Next();
                }
                else
                {
                    activeStep.Update();
                    vxTime.ResetElapsedTime();
                }
            }
            else
            {
                if (MainEntryFired == false)
                {
                    MainEntryFired = true;
                    // if we've run out of steps, then let's start it up!
                    vxEngine.Game.OnGameStart();
                    vxEngine.Game.IsGameContentLoaded = true;
                }
            }


            if (IsActive && m_currentDelay >= 0)
                m_currentDelay -= vxTime.DeltaTime;

        }


        public void Next()
        {
            m_currentDelay = m_delay;
        }

        int Inc = 0;
        protected bool IsDotAnimEnabled = true;


        /// <summary>
        /// Draws the background screen.
        /// </summary>
        /// 
        public override void Draw()
        {
            base.Draw();

            Inc++;
            vxGraphics.GraphicsDevice.Clear(Color.Black);

            vxGraphics.SpriteBatch.Begin("InitScreen");

            vxGraphics.SpriteBatch.Draw(backgroundTexture, vxGraphics.FinalViewport.Bounds, Color.White*.75f);

            if(activeStep!= null)
            {
                var font = vxUITheme.Fonts.Size12;
                var textSize = font.MeasureString(activeStep.Status);

                var textPos = new Vector2(vxScreen.Width / 2, vxScreen.Height *(1f-1f/6f)) - textSize / 2;

                string txt = activeStep?.Status + (IsDotAnimEnabled == true ? new string('.', (int)(Inc / 15) % 5) : "");
                //Console.WriteLine(">>Draw" + vxTime.DeltaTime);
                vxGraphics.SpriteBatch.DrawString(font, txt, textPos+Vector2.One*4, Color.Black*0.75f);
                vxGraphics.SpriteBatch.DrawString(font, txt, textPos, Color.White);
            }

            vxEngine.Instance.DrawVersionInfo(Color.White, TransitionAlpha);

            vxGraphics.SpriteBatch.End();



#region Transition Code
            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || m_pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, m_pauseAlpha / 2);

                vxSceneManager.FadeBackBufferToBlack(alpha);
            }
#endregion
        }

#endregion
    }
}
