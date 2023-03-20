using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using VerticesEngine.ContentManagement;
using VerticesEngine.Graphics;

namespace VerticesEngine.Input
{
    /// <summary>
    /// An enum of all available mouse buttons.
    /// </summary>
    public enum MouseButtons
    {
        LeftButton,
        MiddleButton,
        RightButton,
        ExtraButton1,
        ExtraButton2
    }

    public enum KeyboardTypes
    {
        QWERTY,
        AZERTY,
        CUSTOM
    }


    /// <summary>
    /// What is the input type? Is it it a Controller, Keyboard, Touchscreen? This is useful for displaying
    /// button prompts
    /// </summary>
    public enum InputType
    {
        /// <summary>
        /// The main player is using a Controller for input.
        /// </summary>
        Controller,

        /// <summary>
        /// The main player is using a keyboard for input.
        /// </summary>
        Keyboard,

        /// <summary>
        /// The main player is using a touch screen for input. Mobile will default to this.
        /// </summary>
        TouchScreen,

        /// <summary>
        /// TODO: The main player is using a virtual controller for input. This can be set in Mobile.
        /// </summary>
        VirtualController,
    }

    /// <summary>
    /// The central Input Manager Class for the Vertices Engine. This handles all types of input, from Keyboard, Mouse, GamePad
    /// touch and gesture support.
    /// </summary>
    public static partial class vxInput
    {
        /// <summary>
        /// Gets or sets the cursor position.
        /// </summary>
        /// <value>The cursor.</value>
        public static Vector2 Cursor
        {
            get { return _cursor; }
            set
            {
                _cursor = value;

                //if (InputType == InputType.Keyboard)
                {
                    // set mouse position
                    Mouse.SetPosition((int)_cursor.X, (int)_cursor.Y);
                }
            }
        }
        static Vector2 _cursor;

        static public Vector2 PreviousCursor
        {
            get { return _previousCursor; }
            set { _previousCursor = value; }
        }
        static Vector2 _previousCursor;

        private static List<GestureSample> _gestures = new List<GestureSample>();


        /// <summary>
        /// Gets or sets a value indicating whether the custom cusor in <see cref="T:VerticesEngine.Input.vxvxInput"/> is
        /// visible.
        /// </summary>
        /// <value><c>true</c> if is cusor visible; otherwise, <c>false</c>.</value>
        public static bool IsCursorVisible
        {
            get {

                // first we need to check if the game is fully initialised
                if (!vxEngine.Game.IsGameContentLoaded)
                    return false;

                // next, are we on mobile? if so do we have a gamepad
                if (vxEngine.PlatformType == vxPlatformHardwareType.Mobile)
                {
                    if(InputType == InputType.Controller)
                    {
                        return _isCursorVisible;
                    }
                    else
                    {
                        return false;
                    }
                }
                
                return _isCursorVisible; }
            set
            {
                _isCursorVisible = value;
            }
        }
        private static bool _isCursorVisible = false;

        /// <summary>
        /// Gets or sets the cursor sprite.
        /// </summary>
        /// <value>The cursor sprite.</value>
        public static Texture2D CursorSprite;
        public static Texture2D CursorSpriteClicked;


        public static Texture2D CursorControllerSprite;
        public static Texture2D CursorControllerSpriteClicked;

        /// <summary>
        /// The cursor sprite scale.
        /// </summary>
        public static float CursorSpriteScale = 1;

        /// <summary>
        /// The cursors pixel. This is mainly used for GUI interesection.
        /// </summary>
        public static Rectangle CursorPixel = new Rectangle(0, 0, 1, 1);

        /// <summary>
        /// Gets or sets the cursor rotation.
        /// </summary>
        /// <value>The cursor rotation.</value>
        public static float CursorSpriteRotation = 0;

        /// <summary>
        /// The cursor sprite colour.
        /// </summary>
        public static Color CursorSpriteColour = Color.White;

        /// <summary>
        /// The cursor sprite colour on click.
        /// </summary>
        public static Color CursorSpriteColourOnClick = Color.White;


        /// <summary>
        /// Does the cursor rotate
        /// </summary>
        public static bool DoCursorSpriteRotation = false;

#if WINDOWS_PHONE
		private VirtualStick _phoneStick;
		private VirtualButton _phoneA;
		private VirtualButton _phoneB;
#endif


        private static Viewport _viewport;

        public static Vector2 DragDistance
        {
            get { return _dragDistance; }
        }
        static Vector2 _dragDistance = Vector2.Zero;

        public static bool IsCursorMoved { get; private set; }

        public static bool IsCursorValid { get; private set; }


        static int upCount = 0;

        // 
        public static bool IsInit = false;

        /// <summary>
        /// Is the Mouse Dragging. If yes, then don't let the GUI take input.
        /// </summary>
        public static bool IsDragging = false;

        static Vector2 InitialDownLoc = new Vector2();

        /// <summary>
        /// Whether or not the engine is initialised enough to draw the cursor. The default is this isn't set to true
        /// until after the global LoadAssets screen is called.
        /// </summary>
        public static bool IsCusorInitialised = false;

        public static bool DoBoxSelect = false;

        /// <summary>
        /// What is the current input type for the main player. 
        /// 
        /// On Desktop this is controlled by whether a key press or mouse movement is detected. When a Controller button is pressed then
        /// the input type will automatically switch over to controller.
        /// 
        /// On Mobile this will default to <see cref="InputType.TouchScreen"/>, which will hide the cursor from view for mobile.
        /// </summary>
        public static InputType InputType
        {
            get { return m_inputType; }
        }
#if __MOBILE__
        private static InputType m_inputType = InputType.TouchScreen;
#else
        private static InputType m_inputType = InputType.Keyboard;
#endif

        /// <summary>
        /// Controller buttons
        /// </summary>
        static Array _cntrlButtons;

        #region - Initialization -

        /// <summary>
        ///   Constructs a new input state.
        /// </summary>
        internal static void Init()
        {
            _viewport = vxGraphics.GraphicsDevice.Viewport;

            // initialise cursor in middle of screen
            _cursor = _viewport.Bounds.Center.ToVector2();
            _previousCursor = _cursor;

            InitKeyboardState();
            InitGamePads();
            InitMouseState();
            InitVirtualGamePad();
            InitTouchPanelState();


            // Handle Cursor Abilities
            IsCursorVisible = false;
            IsCursorMoved = false;

#if __MOBILE__
            IsCursorValid = false;
#else
            IsCursorValid = true;
#endif


            CursorSprite = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/gui/cursor/Cursor");
            CursorSpriteClicked = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/gui/cursor/Cursor");

            CursorControllerSprite = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/gui/cursor/Cursor");
            CursorControllerSpriteClicked = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/gui/cursor/Cursor");

            IsCursorVisible = true;

            _cntrlButtons = Enum.GetValues(typeof(Buttons));
        }



        /// <summary>
        /// Input is Reinitialised at the start of each scene.
        /// </summary>
        public static void InitScene()
        {
            IsInit = false;
            upCount = 0;
        }

        #endregion


        public static bool WrapCursor = false;

        //public static event Action OnInputTypeChanged = { };

        /// <summary>
        ///   Reads the latest state of the keyboard and gamepad and mouse/touchpad.
        /// </summary>
        public static void Update()
        {
            // Save Previous Input States
            _previousCursor = Cursor;

            UpdateGamePadState();
            UpdateMouseState();
            UpdatKeyboardState();
            UpdateVirtualGamePad();
            UpdateTouchPanelState();

            CheckInputTypeChange();

            if (WrapCursor && IsMainInputDown())
            {
                var borderPadding = 0;
                var newCursor = vxInput.Cursor - vxInput.PreviousCursor;
                // Handle Top
                if (vxInput.Cursor.Y <= vxGraphics.GraphicsDevice.Viewport.Bounds.Top + borderPadding)
                {
                    // get the current difference
                    vxInput.Cursor = new Vector2(vxInput.Cursor.X, vxGraphics.GraphicsDevice.Viewport.Bounds.Bottom - borderPadding) + newCursor;
                    vxInput.PreviousCursor = vxInput.Cursor;
                }

                // Handle Right
                if (vxInput.Cursor.Y >= vxGraphics.GraphicsDevice.Viewport.Bounds.Bottom - borderPadding)
                {
                    vxInput.Cursor = new Vector2(vxInput.Cursor.X, vxGraphics.GraphicsDevice.Viewport.Bounds.Top + borderPadding) + newCursor;
                    vxInput.PreviousCursor = vxInput.Cursor;
                }

                // Handle Left
                if (vxInput.Cursor.X <= vxGraphics.GraphicsDevice.Viewport.Bounds.Left + borderPadding)
                {
                    vxInput.Cursor = new Vector2(vxGraphics.GraphicsDevice.Viewport.Bounds.Right - borderPadding, (int)vxInput.Cursor.Y) + newCursor;
                    vxInput.PreviousCursor = vxInput.Cursor;
                }

                // Handle Right
                if (vxInput.Cursor.X >= vxGraphics.GraphicsDevice.Viewport.Bounds.Right - borderPadding)
                {
                    vxInput.Cursor = new Vector2(0 + borderPadding, (int)vxInput.Cursor.Y) + newCursor;
                    vxInput.PreviousCursor = vxInput.Cursor;
                }
            }
            else
            {
                // Clamp The Cursor Position
                _cursor = new Vector2(
                    MathHelper.Clamp(_cursor.X, 0f, vxScreen.Width),
                    MathHelper.Clamp(_cursor.Y, 0f, vxScreen.Height)
                );
            }

            if (IsCursorValid && _previousCursor != Cursor)
                IsCursorMoved = true;
            else
                IsCursorMoved = false;


            IsCursorValid = _viewport.Bounds.Contains(MouseState.X, MouseState.Y);
#if __MOBILE__
            IsCursorValid = MouseState.LeftButton == ButtonState.Pressed;
#endif
            if (DoCursorSpriteRotation)
                CursorSpriteRotation += vxTime.DeltaTime;

            CursorPixel.Location = Cursor.ToPoint();


            // Is it the First Down?
            if (IsNewMainInputDown())
            {
                InitialDownLoc = Cursor;
                IsDragging = false;
            }


            if (IsMainInputDown() && IsDragging == false)
            {
                float dif = (Cursor - InitialDownLoc).Length();
                if (dif > 5)
                    IsDragging = true;
            }

            if (upCount > 30)
            {
                IsInit = true;
            }
            else
            {
                upCount++;
            }

            _dragDistance = (Cursor - InitialDownLoc);
        }

        static Vector2 _cursorScalar = Vector2.One;

        /// <summary>
        /// Draw this instance.
        /// </summary>
        internal static void Draw()
        {
            if (IsCursorVisible && IsCusorInitialised == true)
            {
                Texture2D ActiveCursorSprite= (IsMainInputDown() ? CursorSpriteClicked : CursorSprite);

                if (vxInput.InputType == InputType.Controller)
                    ActiveCursorSprite = (IsMainInputDown() ? CursorControllerSpriteClicked : CursorControllerSprite);

                if (MouseState.LeftButton == ButtonState.Pressed && DoBoxSelect)
                {
                    Rectangle MouseBoxSelection = new Rectangle(
                    MouseDownPosition,
                        MouseState.Position - MouseDownPosition);

                    vxGraphics.SpriteBatch.Draw(
                    vxInternalAssets.Textures.Blank,
                    MouseBoxSelection,
                        Color.DeepSkyBlue * 0.5f);
                }

                Vector2 factor = new Vector2(
                    (float)vxScreen.Width / (float)vxGraphics.GraphicsDevice.Viewport.Width,
                    (float)vxScreen.Height / (float)vxGraphics.GraphicsDevice.Viewport.Height);

                _cursorScalar = factor;
                //if (factor.X > 1 && factor.Y > 1)
                //    factor = Vector2.One;
                
                vxGraphics.SpriteBatch.Draw(ActiveCursorSprite, Cursor / factor, null, IsMainInputDown() ? CursorSpriteColourOnClick : CursorSpriteColour,
                    CursorSpriteRotation, new Vector2(ActiveCursorSprite.Width / 2, ActiveCursorSprite.Height / 2),
                    CursorSpriteScale,
                    SpriteEffects.None,
                    0f);

            }
#if WINDOWS_PHONE
			if (_handleVirtualStick)
			{
			_manager.SpriteBatch.Begin();
			_phoneA.Draw(_manager.SpriteBatch);
			_phoneB.Draw(_manager.SpriteBatch);
			_phoneStick.Draw(_manager.SpriteBatch);
			_manager.SpriteBatch.End();
			}
#endif
        }

        static void SetInputType(InputType inputType)
        {
            m_inputType = inputType;

            IsCursorVisible = inputType == InputType.Keyboard;
            vxConsole.InternalWriteLine(new
            {
                msg = "Input Type Changed",
                type = inputType
            });
        }

        /// <summary>
        /// Check whether the player has changed from keyboard to controller (or vice versa).
        /// </summary>
        private static void CheckInputTypeChange()
        {
            if (m_inputType == InputType.Keyboard || m_inputType == InputType.TouchScreen)
            {
                // loop through all buttons
                foreach (Buttons button in _cntrlButtons)
                {
                    if (vxInput.GamePadState.IsButtonDown(button))
                        SetInputType(InputType.Controller);
                }
            }
            else if (m_inputType == InputType.Controller)
            {
                // if any button is clicked, then let's set to keyboard mode
                if (KeyboardState.GetPressedKeyCount() > 0)
                {
                    SetInputType(InputType.Keyboard);
                }
                // fallback to 
                else if(TouchCollection.Count > 0)
                {
                    m_inputType = InputType.TouchScreen;
                }
            }
        }


        /// <summary>
        /// Checks for a "menu select" input action such as Space or Enter Key, A or Start buttons, Mouse Click or Touch Release.
        /// </summary>
        public static bool IsMenuSelect()
        {
            return IsNewKeyPress(Keys.Space) || IsNewKeyPress(Keys.Enter) || IsNewButtonPressed(Buttons.A) || IsNewButtonPressed(Buttons.Start) || IsNewMouseButtonPress(MouseButtons.LeftButton) || IsTouchReleased();
        }


        /// <summary>
        /// Checks for a "menu select" press such as Space or Enter Key, A or Start buttons, Mouse Click or Touch Release.
        /// </summary>
        public static bool IsMenuPressed()
        {
            return KeyboardState.IsKeyDown(Keys.Space) || KeyboardState.IsKeyDown(Keys.Enter) || GamePadState.IsButtonDown(Buttons.A) || GamePadState.IsButtonDown(Buttons.Start) || MouseState.LeftButton == ButtonState.Pressed || IsTouchReleased();
        }

        /// <summary>
        /// Checks for a "menu select" release such as Space or Enter Key, A or Start buttons, Mouse Click or Touch Release.
        /// </summary>
        public static bool IsMenuReleased()
        {
            return IsNewKeyRelease(Keys.Space) || IsNewKeyRelease(Keys.Enter) || IsNewButtonReleased(Buttons.A) || IsNewButtonReleased(Buttons.Start) || IsNewMouseButtonRelease(MouseButtons.LeftButton);
        }


        /// <summary>
        /// Cross Platform Main Input Check. Checks for New Left Click Down, New Button A Press or New Touch Pressed.
        /// </summary>
        /// <returns><c>true</c>, if new main input down was ised, <c>false</c> otherwise.</returns>
        public static bool IsNewMainInputDown()
        {
            return (IsNewButtonPressed(Buttons.A) ||
                    IsNewMouseButtonPress(MouseButtons.LeftButton)) ||
                    IsNewTouchPressed();
        }


        /// <summary>
        /// Cross Platform Main Input Check. Checks for Left Click Down, Button A Press or Touch Pressed.
        /// </summary>
        /// <returns><c>true</c>, if main input down was ised, <c>false</c> otherwise.</returns>
        public static bool IsMainInputDown()
        {
            return (IsButtonPressed(Buttons.A) ||
                    MouseState.LeftButton == ButtonState.Pressed ||
                    IsTouchPressed());
        }

        /// <summary>
        /// Cross Platform Main Input Check Up. Checks for New Left Click Release, New Button A Release or New Touch Release.
        /// </summary>
        /// <returns><c>true</c>, if new main input down was ised, <c>false</c> otherwise.</returns>
        public static bool IsNewMainInputUp()
        {
            return (IsNewButtonReleased(Buttons.A) ||
                     IsNewMouseButtonRelease(MouseButtons.LeftButton)) ||
                    IsNewTouchReleased();
        }


        public static bool IsMainInputUp()
        {
            return (IsButtonReleased(Buttons.A) ||
                    MouseState.LeftButton == ButtonState.Released ||
                    IsTouchReleased());
        }

        /// <summary>
        /// Checks for a "menu up" input action.
        /// The controllingPlayer parameter specifies which player to read
        /// input for. If this is null, it will accept input from any player.
        /// </summary>
        public static bool IsMenuUp()
        {
            return IsNewKeyPress(Keys.Up) ||
                   IsNewButtonPressed(Buttons.DPadUp) ||
                   IsNewButtonPressed(Buttons.LeftThumbstickUp);
        }


        /// <summary>
        /// Checks for a "menu down" input action.
        /// The controllingPlayer parameter specifies which player to read
        /// input for. If this is null, it will accept input from any player.
        /// </summary>
        public static bool IsMenuDown()
        {
            return IsNewKeyPress(Keys.Down) ||
                   IsNewButtonPressed(Buttons.DPadDown) ||
                   IsNewButtonPressed(Buttons.LeftThumbstickDown);
        }

        /// <summary>
        /// Checks for a "pause the game" input action.
        /// The controllingPlayer parameter specifies which player to read
        /// input for. If this is null, it will accept input from any player.
        /// </summary>
        public static bool IsPauseGame()
        {
            return IsNewKeyPress(Keys.Escape) ||
                   IsNewButtonPressed(Buttons.Back) ||
                   IsNewButtonPressed(Buttons.Start);
        }

        /// <summary>
        ///   Checks for a "menu cancel" input action.
        /// </summary>
        public static bool IsMenuCancel()
        {
            return IsNewKeyPress(Keys.Escape) || IsNewButtonPressed(Buttons.Back) || IsNewButtonPressed(Buttons.B);
        }

        public static bool IsNewMainNavDown()
        {
            return (IsNewButtonPressed(Buttons.DPadDown) || IsNewKeyPress(Keys.Down));
        }

        public static bool IsNewMainNavUp()
        {
            return (IsNewButtonPressed(Buttons.DPadUp) || IsNewKeyPress(Keys.Up));
        }

        public static bool IsNewMainNavLeft()
        {
            return (IsNewButtonPressed(Buttons.DPadLeft) || IsNewButtonPressed(Buttons.LeftShoulder) || IsNewKeyPress(Keys.Left));
        }

        public static bool IsNewMainNavRight()
        {
            return (IsNewButtonPressed(Buttons.DPadRight) || IsNewButtonPressed(Buttons.RightShoulder) || IsNewKeyPress(Keys.Right));
        }



        public static bool IsNewMainSlideLeft()
        {
            return (IsNewButtonPressed(Buttons.DPadLeft) || IsNewButtonPressed(Buttons.LeftShoulder) || IsNewKeyPress(Keys.Left) || IsNewKeyPress(Keys.Q));
        }

        public static bool IsNewMainSlideRight()
        {
            return (IsNewButtonPressed(Buttons.DPadRight) || IsNewButtonPressed(Buttons.RightShoulder) || IsNewKeyPress(Keys.Right) || IsNewKeyPress(Keys.E));
        }
    }
}

