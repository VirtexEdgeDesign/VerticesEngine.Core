using Microsoft.Xna.Framework.Input;

namespace VerticesEngine.Input
{
    public static partial class vxInput
    {
        public static class KeyboardSettings
        {
            [vxInputSettingsAttribute("Key.Movement.Forwards")]
            public static Keys Forward = Keys.W;

            [vxInputSettingsAttribute("Key.Movement.Backwards")]
            public static Keys Backwards = Keys.S;

            [vxInputSettingsAttribute("Key.Movement.Left")]
            public static Keys Left = Keys.A;

            [vxInputSettingsAttribute("Key.Movement.Right")]
            public static Keys Right = Keys.D;

            [vxInputSettingsAttribute("Key.Movement.Jump")]
            public static Keys Jump = Keys.Space;

            [vxInputSettingsAttribute("Key.Movement.Crouch")]
            public static Keys Crouch = Keys.LeftControl;
        }

        public static vxKeyBindings KeyBindings;

        public static KeyboardState KeyboardState { get; private set; }

        public static KeyboardState PreviousKeyboardState { get; private set; }

        static void InitKeyboardState()
        {
            KeyBindings = new vxKeyBindings();

            KeyboardState = new KeyboardState();
            PreviousKeyboardState = new KeyboardState();
        }

        static void UpdatKeyboardState()
        {
            PreviousKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();
        }

        #region - Utility Methods -

        /// <summary>
        ///   Helper for checking if a key was newly pressed during this update.
        /// </summary>
        public static bool IsNewKeyPress(Keys key)
        {
            return (KeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyUp(key));
        }

        public static bool IsNewKeyRelease(Keys key)
        {
            return (PreviousKeyboardState.IsKeyDown(key) && KeyboardState.IsKeyUp(key));
        }

        /// <summary>
        /// Is the specified key currently down?
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>A boolean value indicating whether the specified key is down or not</returns>
        public static bool IsKeyDown(Keys key)
        {
            return (KeyboardState.IsKeyDown(key));
        }

        #endregion
    }
}
