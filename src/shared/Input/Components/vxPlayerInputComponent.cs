using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.Input
{
    /// <summary>
    /// This component handles per-player input. A player may be player 2, but they could be the one using
    /// the keyboard, or a different game pad, this way it allows the player index to map to a specific input device
    /// </summary>
    public class vxPlayerInputComponent : vxComponent
    {
        /// <summary>
        /// Which player input should this correspond to?
        /// </summary>
        public int GamePadIndex = -1;


        public event EventHandler<PlayerGamePadDisconnectEventArgs> OnGamepadDisconnect;

        public GamePadState GamePadState
        {
            get { return vxInput.GamePadStates[GamePadIndex]; }
        }

        protected internal override void PreUpdate()
        {
            if (GamePadState.IsConnected == false)
            {
                OnGamepadDisconnect(this, new PlayerGamePadDisconnectEventArgs(Microsoft.Xna.Framework.PlayerIndex.One));
            }
        }
    }
}
