using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VerticesEngine.Profile;

namespace VerticesEngine.Net
{


    /// <summary>
    /// This is a base class which holds basic information for a player in the server/client system.
    /// </summary>
    public struct vxNetPlayerInfo
    {
        /// <summary>
        /// Gets the ID of this Net Player. 
        /// </summary>
        public string ID;

        /// <summary>
        /// Gets the Username of this player
        /// </summary>
        public string UserName;

        /// <summary>
        /// The current Entity State
        /// </summary>
        public vxNetEntityState EntityState;

        /// <summary>
        /// The index of the player. Provided by the server
        /// </summary>
        public int PlayerIndex;

        /// <summary>
        /// An Enumerator that holds where in the "Ready" phase the player is
        /// </summary>
        public vxEnumNetPlayerStatus Status;


        /// <summary>
        /// The platform type, are we on steam, Itchio? This is to handle cross-platform
        /// player intertaction 
        /// </summary>
        public vxPlatformType Platform;

        /// <summary>
        /// Whats the players platform id
        /// </summary>
        public string PlatformPlayerID;


        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.Net.vxNetPlayerInfo"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="username">Username.</param>
        /// <param name="netplayerstatus">Netplayerstatus.</param>
        public vxNetPlayerInfo(string id, string username, int playerIndex, vxEnumNetPlayerStatus netplayerstatus)
        {
            this.ID = id;
            this.UserName = username;
            PlayerIndex = playerIndex;
            this.Status = netplayerstatus;
            EntityState = new vxNetEntityState();
            Platform = vxPlatformType.Steam;
            PlatformPlayerID=vxPlatform.Player?.Id;
        }

        public vxNetPlayerInfo(string id, string username, int playerIndex, vxEnumNetPlayerStatus netplayerstatus, vxPlatformType platform, string platformPlayerId)
        {
            this.ID = id;
            this.UserName = username;
            PlayerIndex = playerIndex;
            this.Status = netplayerstatus;
            EntityState = new vxNetEntityState();
            Platform = platform;
            PlatformPlayerID = platformPlayerId;
        }

        internal void SetID(string id)
        {
            ID = id;
        }
    }
}
