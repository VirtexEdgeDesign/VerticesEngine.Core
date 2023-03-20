using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace VerticesEngine.Input
{
    public static partial class vxInput
    {
        public static class GamePadSettings
        {
            //Controls
            [vxInputSettingsAttribute("GamePad.Sensitivity")]
            public static float Sensitivity = 1.0f;

            [vxInputSettingsAttribute("GamePad.IsXInverted")]
            public static bool IsXInverted = false;


            [vxInputSettingsAttribute("GamePad.IsYInverted")]
            public static bool IsYInverted = false;
        }

        public const int NumberOfGamePads = 4;

        /// <summary>
        /// A collection of game pad states based off of how many players are allowed for this specific game.
        /// </summary>
        public static List<GamePadState> GamePadStates = new List<GamePadState>();

        /// <summary>
        /// Previous gamepad states
        /// </summary>
        public static List<GamePadState> PreviousGamePadStates = new List<GamePadState>();


        #region - Properties -

        /// <summary>
        /// Did the game pad move this frame?
        /// </summary>
        static bool DidGamepadMoved
        {
            get { return GamePadState.IsConnected && GamePadState.ThumbSticks.Left != Vector2.Zero; }
        }

        /// <summary>
        /// The player index who is currently controlling the cursor
        /// </summary>
        public static PlayerIndex PlayerControllingCursor
        {
            get { return (PlayerIndex)_playerControllingCursor; }
        }
        private static int _playerControllingCursor = 0;

        /// <summary>
        /// Gets the state of the game pad for the currently conrtolling game pad
        /// </summary>
        /// <value>The state of the game pad.</value>
        public static GamePadState GamePadState
        {
            get { return GamePadStates[_playerControllingCursor]; }
        }

        /// <summary>
        /// Gets the state of the previous game pad state for Player One.
        /// </summary>
        /// <value>The state of the previous game pad.</value>
        public static GamePadState PreviousGamePadState
        {
            get { return PreviousGamePadStates[_playerControllingCursor]; }
        }

        #endregion

        static void InitGamePads()
        {

            for (int i = 0; i < NumberOfGamePads; i++)
            {
                GamePadStates.Add(new GamePadState());
                PreviousGamePadStates.Add(new GamePadState());
            }
        }

        static void UpdateGamePadState()
        {
            for (int i = 0; i < NumberOfGamePads; i++)
                PreviousGamePadStates[i] = GamePadStates[i];

            for (int i = 0; i < NumberOfGamePads; i++)
                GamePadStates[i] = GamePad.GetState((PlayerIndex)i);


            if (DidGamepadMoved)
            {
                Vector2 temp = GamePadState.ThumbSticks.Left;
                Cursor += temp * new Vector2(700f, -700f) * vxTime.DeltaTime;
            }
        }


        #region - Utility Methods -

        /// <summary>
        /// Sets which player is controlling the cursor currently. This is useful for multiplayer setup
        /// </summary>
        /// <param name="playerIndex"></param>
        public static void SetCursorControllerPlayer(PlayerIndex playerIndex)
        {
            _playerControllingCursor = (int)playerIndex;
        }


        /// <summary>
        /// Resets the controlling player index to Player One
        /// </summary>
        public static void ResetCursorControllingPlayer()
        {
            _playerControllingCursor = 0;
        }



        /// <summary>
        /// Returns the boolean of whether of GamePad Button has been newly Pressed. The default is for Player One, but you can pass the PlayerIndex as an 
        /// optional Variable.
        /// </summary>
        /// <returns><c>true</c>, if new button release was used, <c>false</c> otherwise.</returns>
        /// <param name="button">Button to check if it's newly been pressed.</param>
        /// <param name="PlayerIndex">Player index.</param>
        public static bool IsNewButtonPressed(Buttons button)
        {
            return IsNewButtonPressed(button, (PlayerIndex)_playerControllingCursor);
        }

        public static bool IsNewButtonPressed(Buttons button, PlayerIndex PlayerIndex)
        {
            return (GamePadStates[(int)PlayerIndex].IsButtonDown(button) && PreviousGamePadStates[(int)PlayerIndex].IsButtonUp(button));
        }

        /// <summary>
        /// Returns the boolean of whether of GamePad Button has been newly Released. The default is for Player One, but you can pass the PlayerIndex as an 
        /// optional Variable.
        /// </summary>
        /// <returns><c>true</c>, if new button release was used, <c>false</c> otherwise.</returns>
        /// <param name="button">Button to check if it's newly been pressed.</param>
        /// <param name="PlayerIndex">Player index.</param>
        public static bool IsNewButtonReleased(Buttons button)
        {
            return IsNewButtonReleased(button, (PlayerIndex)_playerControllingCursor);
        }

        public static bool IsNewButtonReleased(Buttons button, PlayerIndex PlayerIndex)
        {
            return (PreviousGamePadStates[(int)PlayerIndex].IsButtonDown(button) && GamePadStates[(int)PlayerIndex].IsButtonUp(button));
        }

        /// <summary>
        /// Returns the boolean of whether of GamePad Button is Pressed. The default is for Player One, but you can pass the PlayerIndex as an 
        /// optional Variable.
        /// </summary>
        /// <returns><c>true</c>, if the button press was used, <c>false</c> otherwise.</returns>
        /// <param name="button">Button to check if it's pressed.</param>
        /// <param name="PlayerIndex">Player index.</param>
        public static bool IsButtonPressed(Buttons button)
        {
            return IsButtonPressed(button, _playerControllingCursor);
        }

        public static bool IsButtonPressed(Buttons button, PlayerIndex PlayerIndex)
        {
            return IsButtonPressed(button, (int)PlayerIndex);
        }

        public static bool IsButtonPressed(Buttons button, int PlayerIndex)
        {
            return GamePadStates[PlayerIndex].IsButtonDown(button);
        }

        /// <summary>
        /// Returns the boolean of whether of GamePad Button is Released. The default is for Player One, but you can pass the PlayerIndex as an 
        /// optional Variable.
        /// </summary>
        /// <returns><c>true</c>, if the button release was used, <c>false</c> otherwise.</returns>
        /// <param name="button">Button to check if it's released.</param>
        /// <param name="PlayerIndex">Player index.</param>
        public static bool IsButtonReleased(Buttons button)
        {
            return IsButtonReleased(button, _playerControllingCursor);
        }

        public static bool IsButtonReleased(Buttons button, PlayerIndex PlayerIndex)
        {
            return IsButtonReleased(button, (int)PlayerIndex);
        }

        public static bool IsButtonReleased(Buttons button, int PlayerIndex)
        {
            if (PlayerIndex < GamePadStates.Count)
                return GamePadStates[PlayerIndex].IsButtonUp(button);
            else return false;
        }


        public static bool IsNewLeftThumbstickUp()
        {
            return (Math.Abs(GamePadState.ThumbSticks.Left.Y) > Math.Abs(GamePadState.ThumbSticks.Left.X) &&
                GamePadState.ThumbSticks.Left.Y > 0) &&
                (PreviousGamePadStates[0].ThumbSticks.Left.Y <= 0);
        }
        public static bool IsNewLeftThumbstickDown()
        {
            return (Math.Abs(GamePadState.ThumbSticks.Left.Y) > Math.Abs(GamePadState.ThumbSticks.Left.X) &&
                GamePadState.ThumbSticks.Left.Y < 0) &&
                (PreviousGamePadStates[0].ThumbSticks.Left.Y >= 0);
        }
        public static bool IsNewLeftThumbstickLeft()
        {
            return (Math.Abs(GamePadState.ThumbSticks.Left.Y) < Math.Abs(GamePadState.ThumbSticks.Left.X) &&
                GamePadState.ThumbSticks.Left.X < 0) && (PreviousGamePadStates[0].ThumbSticks.Left.X == 0);
        }
        public static bool IsNewLeftThumbstickRight()
        {
            return (Math.Abs(GamePadState.ThumbSticks.Left.Y) < Math.Abs(GamePadState.ThumbSticks.Left.X) &&
                GamePadState.ThumbSticks.Left.X > 0) && (PreviousGamePadStates[0].ThumbSticks.Left.X == 0);
        }


        #endregion
    }
}
