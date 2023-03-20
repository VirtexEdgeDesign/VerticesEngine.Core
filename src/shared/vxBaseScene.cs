#region File Description
//-----------------------------------------------------------------------------
// vxGameBaseScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
using VerticesEngine.Input;


#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.UI;
using VerticesEngine.Graphics;
using VerticesEngine.Audio;
using System.Diagnostics;
#endregion

namespace VerticesEngine
{
    /// <summary>
    /// Enum describes the screen transition state.
    /// </summary>
    public enum ScreenState
    {
		/// <summary>
		/// Transitioning on.
		/// </summary>
        TransitionOn,

		/// <summary>
		/// Screen is active.
		/// </summary>
        Active,

		/// <summary>
		/// Transitioning off.
		/// </summary>
        TransitionOff,

		/// <summary>
		/// The screen is hidden.
		/// </summary>
        Hidden,
    }


    /// <summary>
    /// A scene is a single layer that has update and draw logic, and which
    /// can be combined with other layers to build up a complex game and menu system.
    /// For instance the main menu, the options menu, the "are you sure you
    /// want to quit" message box, and the main game itself are all implemented
    /// as scenes.
    /// </summary>
    public abstract class vxBaseScene : IDisposable
    {
        #region Properties


        /// <summary>
        /// Should we remove this scene completely?!
        /// </summary>
        internal bool CanSceneBeRemovedCompletely = false;

        /// <summary>
        /// Normally when one screen is brought up over the top of another,
        /// the first screen will transition off to make room for the new
        /// one. This property indicates whether the screen is only a small
        /// popup, in which case screens underneath it do not need to bother
        /// transitioning off.
        /// </summary>
        public bool IsPopup
        {
            get { return _isPopup; }
            protected set { _isPopup = value; }
        }
        private bool _isPopup = false;

        /// <summary>
        /// Has this scene been removed?
        /// </summary>
        public bool IsRemoved
        {
            get { return _isRemoved; }
            protected internal set { _isRemoved = value; }
        }
        private bool _isRemoved = false;

        /// <summary>
        /// Indicates how long the screen takes to
        /// transition on when it is activated.
        /// </summary>
        public TimeSpan TransitionOnTime
        {
            get { return transitionOnTime; }
            protected set { transitionOnTime = value; }
        }

        TimeSpan transitionOnTime = TimeSpan.Zero;


        /// <summary>
        /// Indicates how long the screen takes to
        /// transition off when it is deactivated.
        /// </summary>
        public TimeSpan TransitionOffTime
        {
            get { return _transitionOffTime; }
            protected set { _transitionOffTime = value; }
        }

        TimeSpan _transitionOffTime = TimeSpan.Zero;


        /// <summary>
        /// Gets the current position of the screen transition, ranging
        /// from zero (fully active, no transition) to one (transitioned
        /// fully off to nothing).
        /// </summary>
        public float TransitionPosition
        {
            get { return _transitionPosition; }
            protected set { _transitionPosition = value; }
        }

        float _transitionPosition = 1;


        /// <summary>
        /// Gets the current alpha of the screen transition, ranging
        /// from 1 (fully active, no transition) to 0 (transitioned
        /// fully off to nothing).
        /// </summary>
        public float TransitionAlpha
        {
            get { return 1f - TransitionPosition; }
        }


        /// <summary>
        /// Gets the current screen transition state.
        /// </summary>
        public ScreenState ScreenState
        {
            get { return _screenState; }
            protected set { _screenState = value; }
        }

        ScreenState _screenState = ScreenState.TransitionOn;


        /// <summary>
        /// There are two possible reasons why a screen might be transitioning
        /// off. It could be temporarily going away to make room for another
        /// screen that is on top of it, or it could be going away for good.
        /// This property indicates whether the screen is exiting for real:
        /// if set, the screen will automatically remove itself as soon as the
        /// transition finishes.
        /// </summary>
        public bool IsExiting
        {
            get { return _isExiting; }
            protected internal set { _isExiting = value; }
        }

        bool _isExiting = false;


        /// <summary>
        /// Checks whether this screen is active and can respond to user input.
        /// </summary>
        public bool IsActive
        {
            get
            {
                return !otherScreenHasFocus &&
                       (_screenState == ScreenState.TransitionOn ||
                        _screenState == ScreenState.Active);
            }
        }

        public bool otherScreenHasFocus
        {
            get { return _otherScreenHasFocus; }
            internal set { _otherScreenHasFocus = value; }
        }
        private bool _otherScreenHasFocus;




        /// <summary>
        /// Gets a value indicating whether this <see cref="T:VerticesEngine.vxGameBaseScreen"/> is first loop.
        /// </summary>
        /// <value><c>true</c> if is first loop; otherwise, <c>false</c>.</value>
        public bool IsFirstLoop
        {
            get { return _isFirstLoop; }
        }
        bool _isFirstLoop = true;


        
        /// <summary>
        /// Returns whether we're currently loading a file or not.
        /// </summary>
        public bool IsLoadingFile
        {
            get { return _IsLoadingFile; }
            internal set { _IsLoadingFile = value; }
        }
        bool _IsLoadingFile = false;

        /// <summary>
        /// Gets the index of the player who is currently controlling this screen,
        /// or null if it is accepting input from any player. This is used to lock
        /// the game to a specific player profile. The main menu responds to input
        /// from any connected gamepad, but whichever player makes a selection from
        /// this menu is given control over all subsequent screens, so other gamepads
        /// are inactive until the controlling player returns to the main menu.
        /// </summary>
        public PlayerIndex? ControllingPlayer
        {
            get { return _controllingPlayer == null ? PlayerIndex.One : _controllingPlayer; }
            internal set { _controllingPlayer = value; }
        }

        PlayerIndex? _controllingPlayer;


        /// <summary>
        /// this holds the start up time from the time of LoadContent called, to the first update
        /// </summary>
        Stopwatch startupStopWatch = new Stopwatch();

        protected bool IsLoadingTimeMeasured = false;

        #endregion

        #region Initialization


        //public bool IsInitialised
        //{
        //    get { return isInitialised; }
        //    set { isInitialised = value; }
        //}
        //bool isInitialised = false;

        public vxBaseScene()
        {

        }


        /// <summary>
        /// Load graphics content for the screen.
        /// </summary>
		public virtual void LoadContent()
        {
            startupStopWatch.Start();

            vxInput.InitScene();
        }

        /// <summary>
        /// Has all the content been loaded yet?
        /// </summary>
        public bool IsContentLoaded
        {
            get { return _isContentLoaded; }
            set { _isContentLoaded = value; }
        }
        private bool _isContentLoaded = false;

        protected internal bool IsLoadContentAsyncEnabled = true;
        protected internal bool IsLoadSceneContentAsyncEnabled = true;

        /// <summary>
        /// Loads content as part of a Coroutine allowing the game to update frames during the loading process
        /// </summary>
        /// <returns></returns>
        public virtual System.Collections.IEnumerator LoadContentAsync()
        {
            yield return null;
        }

        /// <summary>
        /// Loads scene content. This is only internal to the engine
        /// </summary>
        /// <returns></returns>
        internal virtual System.Collections.IEnumerator LoadSceneContentAsync()
        {
            yield return null;
        }


        /// <summary>
        /// Unload content for the screen.
        /// </summary>
        public virtual void UnloadContent()
        {

        }


        #endregion

        #region Update and Draw

        /// <summary>
        /// Called at the start of this scenes first update/tick.
        /// </summary>
        protected virtual void OnFirstUpdate()
        {

        }


        /// <summary>
        /// This sets the text for this scene. This is handy if you're changing the language during gameplay.
        /// </summary>
        protected internal virtual void OnLocalizationChanged()
        {

        }

        public bool coveredByOtherScreen
        {
            get { return _coveredByOtherScreen; }
            set { _coveredByOtherScreen = value; }
        }
        private bool _coveredByOtherScreen;

        protected bool HideIfCovered = true;

        private void OnSceneLoaded()
        {
            if (IsLoadingTimeMeasured)
            {
                vxDebug.LogIO(new
                {
                    scene = this.GetType(),
                    loadTime = startupStopWatch.Elapsed
                });
            }
            else
            {
                vxDebug.LogEngine(new
                {
                    scene = this.GetType()
                });
            }
        }

        private void OnSceneRemoved()
        {
            vxDebug.LogEngine(new
            {
                scene = this.GetType()
            });
        }

        /// <summary>
        /// Allows the screen to run logic, such as updating the transition position.
        /// Unlike HandleInput, this method is called regardless of whether the screen
        /// is active, hidden, or in the middle of a transition.
        /// </summary>
        protected internal virtual void Update()
        {
            if (_isFirstLoop)
            {
                _isFirstLoop = false;
                OnFirstUpdate();

                startupStopWatch.Stop();
                OnSceneLoaded();
            }

            if (_isExiting)
            {
                // If the screen is going away to die, it should transition off.
                _screenState = ScreenState.TransitionOff;

                if (!UpdateTransition(_transitionOffTime, 1))
                {
                    OnSceneRemoved();
                    // When the transition finishes, remove the screen.
                    vxSceneManager.RemoveScene(this);
                }
            }
            else if (coveredByOtherScreen && HideIfCovered)
            {
                // If the screen is covered by another, it should transition off.
                if (UpdateTransition(_transitionOffTime, 1))
                {
                    // Still busy transitioning.
                    _screenState = ScreenState.TransitionOff;
                }
                else
                {
                    // Transition finished!
                    //screenState = ScreenState.Hidden;
                }
            }
            else if(_screenState != ScreenState.Active)
            {
                // Otherwise the screen should transition on and become active.
                if (UpdateTransition(transitionOnTime, -1))
                {
                    // Still busy transitioning.
                    _screenState = ScreenState.TransitionOn;
                }
                else
                {
                    // Transition finished!
                    _screenState = ScreenState.Active;
                }
            }
        }


        /// <summary>
        /// Helper for updating the screen transition position.
        /// </summary>
        bool UpdateTransition(TimeSpan time, int direction)
        {
            // How much should we move by?
            float transitionDelta;

            if (time == TimeSpan.Zero)
                transitionDelta = 1;
            else
                transitionDelta = (float)((vxTime.DeltaTime * 1000)/ time.TotalMilliseconds);

            // Update the transition position.
            _transitionPosition += transitionDelta * direction;

            // Did we reach the end of the transition?
            if (((direction < 0) && (_transitionPosition <= 0)) ||
                ((direction > 0) && (_transitionPosition >= 1)))
            {
                _transitionPosition = MathHelper.Clamp(_transitionPosition, 0, 1);
                return false;
            }

            // Otherwise we are still busy transitioning.
            return true;
        }


        /// <summary>
        /// Allows the screen to handle user input. Unlike Update, this method
        /// is only called when the screen is active, and not when some other
        /// screen has taken the focus.
        /// </summary>
        protected internal virtual void HandleInput() { }

        bool _isFirstDraw = true;

        /// <summary>
        /// This is called when the screen is drawn
        /// </summary>
        public virtual void Draw() 
        { 

            if (_isFirstDraw)
            {
                _isFirstDraw = false;
                OnFirstDraw();
            }
        }

        /// <summary>
        /// Called on the first draw of this scene.
        /// </summary>
        protected virtual void OnFirstDraw() { }


        #endregion

        #region Public Methods

        /// <summary>
        /// Called when there is a reset or refresh of Graphic settings such as resolution or setting.
        /// </summary>
        protected internal virtual void OnGraphicsRefresh() {

            vxGraphics.InitMainBuffer();
        }



		public virtual bool PlaySound(vxBaseScene sender, SoundEffect SoundEffect, float Volume = 1, float Pitch = 0)
		{
			try
			{
				// Max the Volume at 1.
				Volume = MathHelper.Min(Volume, 1);

				// Create the Instance of the Sound Effect.
				SoundEffectInstance sndEfctInstnc = SoundEffect.CreateInstance();

				// Set the Volume
				sndEfctInstnc.Volume = Volume * vxAudioManager.SoundEffectVolume;

				// Set the Pitch
				sndEfctInstnc.Pitch = Pitch;

				// Play It
				sndEfctInstnc.Play();

				return true;
			}
			catch 
			{
				//vxConsole.WriteException(this,new vxSoundEffectException(sender, SoundEffect, ex));
				return false;
			}
		}

        /// <summary>
        /// Tells the screen to go away. Unlike ScreenManager.RemoveScreen, which
        /// instantly kills the screen, this method respects the transition timings
        /// and will give the screen a chance to gradually transition off.
        /// </summary>
        public void ExitScreen()
        {
            if (TransitionOffTime == TimeSpan.Zero)
            {
                // If the screen has a zero transition time, remove it immediately.
                vxSceneManager.RemoveScene(this);
            }
            else
            {
                // Otherwise flag that it should transition off and then exit.
                _isExiting = true;
            }
        }

        public virtual void Dispose()
        {
            
        }


        #endregion
    }
}
