#region File Description
//-----------------------------------------------------------------------------
// PlayerIndexEventArgs.cs
//
// XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using VerticesEngine.UI;
#endregion

namespace VerticesEngine.Input
{
    /// <summary>
    /// Custom event argument which includes the index of the player who
    /// triggered the event. This is used by the vxMenuEntry.Selected event.
    /// </summary>
    public class PlayerGamePadDisconnectEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public PlayerGamePadDisconnectEventArgs(PlayerIndex playerIndex)
        {
            this.playerIndex = playerIndex;
        }


        /// <summary>
        /// Gets the index of the player who triggered this event.
        /// </summary>
        public PlayerIndex PlayerIndex
        {
            get { return playerIndex; }
        }

        PlayerIndex playerIndex;
    }
}