using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace VerticesEngine.Input
{
    public static partial class vxInput
    {
        public static class MouseSettings
        {
            [vxInputSettingsAttribute("Mouse.Sensitivity")]
            public static float Sensitivity = 1.0f;

            [vxInputSettingsAttribute("Mouse.IsXInverted")]
            public static bool IsXInverted = false;


            [vxInputSettingsAttribute("Mouse.IsYInverted")]
            public static bool IsYInverted = false;
        }

        public static MouseState MouseState;

        public static MouseState PreviousMouseState { get; private set; }

        /// <summary>
        /// Gets the Change in Scroll wheel position since the last update
        /// </summary>
        public static int ScrollWheelDelta { get; private set; }

        private static int PreviousScrollWheel;


        /// <summary>
        /// The initial mouse down position.
        /// </summary>
        public static Point MouseDownPosition;

        /// <summary>
        /// The mouse click position.
        /// </summary>
        public static Vector2 MouseClickPos = new Vector2();

        public static bool IsNewLeftMouseClick
        {
            get { return (IsNewMouseButtonPress(MouseButtons.LeftButton) && !DoBoxSelect); }
        }

        static void InitMouseState()
        {
            PreviousScrollWheel = MouseState.ScrollWheelValue;

            MouseState = new MouseState();
            PreviousMouseState = new MouseState();
        }

        static void UpdateMouseState()
        {
            PreviousMouseState = MouseState;
            MouseState = Mouse.GetState();

            ScrollWheelDelta = MouseState.ScrollWheelValue - PreviousScrollWheel;
            PreviousScrollWheel = MouseState.ScrollWheelValue;

            if (IsNewMouseButtonPress(MouseButtons.LeftButton) || MouseState.LeftButton == ButtonState.Released)
            {
                MouseDownPosition = MouseState.Position;
            }

            if (!DidGamepadMoved && m_inputType == InputType.Keyboard)
            {
                _cursor = new Vector2(MouseState.X, MouseState.Y) * _cursorScalar;// - _manager.Game.Window.Position.ToVector2();
            }
        }

        #region - Utility Methods -

        public static bool IsMouseButtonDown()
        {
            return (vxInput.MouseState.LeftButton == ButtonState.Pressed ||
                vxInput.MouseState.MiddleButton == ButtonState.Pressed ||
                vxInput.MouseState.RightButton == ButtonState.Pressed);
        }

        /// <summary>
        ///   Helper for checking if a mouse button was newly pressed during this update.
        /// </summary>
        public static bool IsNewMouseButtonPress(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.LeftButton:
                    return (MouseState.LeftButton == ButtonState.Pressed && PreviousMouseState.LeftButton == ButtonState.Released);
                case MouseButtons.RightButton:
                    return (MouseState.RightButton == ButtonState.Pressed && PreviousMouseState.RightButton == ButtonState.Released);
                case MouseButtons.MiddleButton:
                    return (MouseState.MiddleButton == ButtonState.Pressed && PreviousMouseState.MiddleButton == ButtonState.Released);
                case MouseButtons.ExtraButton1:
                    return (MouseState.XButton1 == ButtonState.Pressed && PreviousMouseState.XButton1 == ButtonState.Released);
                case MouseButtons.ExtraButton2:
                    return (MouseState.XButton2 == ButtonState.Pressed && PreviousMouseState.XButton2 == ButtonState.Released);
                default:
                    return false;
            }
        }


        /// <summary>
        /// Checks if the requested mouse button is released.
        /// </summary>
        /// <param name="button">The button.</param>
        public static bool IsNewMouseButtonRelease(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.LeftButton:
                    return (PreviousMouseState.LeftButton == ButtonState.Pressed && MouseState.LeftButton == ButtonState.Released);
                case MouseButtons.RightButton:
                    return (PreviousMouseState.RightButton == ButtonState.Pressed && MouseState.RightButton == ButtonState.Released);
                case MouseButtons.MiddleButton:
                    return (PreviousMouseState.MiddleButton == ButtonState.Pressed && MouseState.MiddleButton == ButtonState.Released);
                case MouseButtons.ExtraButton1:
                    return (PreviousMouseState.XButton1 == ButtonState.Pressed && MouseState.XButton1 == ButtonState.Released);
                case MouseButtons.ExtraButton2:
                    return (PreviousMouseState.XButton2 == ButtonState.Pressed && MouseState.XButton2 == ButtonState.Released);
                default:
                    return false;
            }
        }

        /// <summary>
        /// Ises the mouse button pressed.
        /// </summary>
        /// <returns><c>true</c>, if mouse button pressed was ised, <c>false</c> otherwise.</returns>
        /// <param name="button">Button.</param>
        public static bool IsMouseButtonPressed(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.LeftButton:
                    return (MouseState.LeftButton == ButtonState.Pressed);
                case MouseButtons.RightButton:
                    return (MouseState.RightButton == ButtonState.Pressed);
                case MouseButtons.MiddleButton:
                    return (MouseState.MiddleButton == ButtonState.Pressed);
                case MouseButtons.ExtraButton1:
                    return (MouseState.XButton1 == ButtonState.Pressed);
                case MouseButtons.ExtraButton2:
                    return (MouseState.XButton2 == ButtonState.Pressed);
                default:
                    return false;
            }
        }

        #endregion
    }
}
