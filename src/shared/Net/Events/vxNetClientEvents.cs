using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using VerticesEngine.Net.Messages;
using Microsoft.Xna.Framework;

namespace VerticesEngine.Net.Events
{
    /// <summary>
    /// This event is fired whenever a discovery response is recieved from a server.
    /// </summary>
    public class vxNetClientEventDiscoverySignalResponse : EventArgs
    {
        public vxNetMsgServerInfo NetMsgServerInfo
        {
            get { return m_vxNetMsgServerInfo; }
        }
        vxNetMsgServerInfo m_vxNetMsgServerInfo;

        /// <summary>
        /// The address of where the Discovery Signal originates from.
        /// </summary>
        public string Address
        {
            get { return m_vxNetMsgServerInfo.ServerIP.ToString(); }
        }

        /// <summary>
        /// The port of where the Discovery Signal originates from.
        /// </summary>
        public int Port
        {
            get { return Convert.ToInt32(m_vxNetMsgServerInfo.ServerPort); }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public vxNetClientEventDiscoverySignalResponse(vxNetMsgServerInfo NetMsgServerInfo)
        {
            this.m_vxNetMsgServerInfo = NetMsgServerInfo;
        }
    }


    /// <summary>
    /// This event is fired whenever this player connects to the server.
    /// </summary>
    public class vxNetClientEventConnected : EventArgs
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public vxNetClientEventConnected()
        {
            
        }
    }

    /// <summary>
    /// This event is fired whenever this player disconnects from the server.
    /// </summary>
    public class vxNetClientEventDisconnected : EventArgs
    {
        public vxNetDisconnectedEventReason Reason;

        public string AdditionalInfo = string.Empty;

        /// <summary>
        /// Constructor.
        /// </summary>
        public vxNetClientEventDisconnected(vxNetDisconnectedEventReason Reason, string AdditionalInfo = "")
        {
            this.Reason = Reason;
            this.AdditionalInfo = AdditionalInfo;
        }
    }


    /// <summary>
    /// This event is fired on the client side whenever a new player connects to the server.
    /// </summary>
    public class vxNetClientEventPlayerConnected : EventArgs
    {
        /// <summary>
        /// Information pertaining to the New Connected Player.
        /// </summary>
        public vxNetPlayerInfo ConnectedPlayer
        {
            get { return m_connectedPlayer; }
        }
        vxNetPlayerInfo m_connectedPlayer;

        /// <summary>
        /// Constructor.
        /// </summary>
        public vxNetClientEventPlayerConnected(vxNetPlayerInfo player)
        {
            m_connectedPlayer = player;
        }
    }
    

    /// <summary>
    /// This event is fired on the client side whenever a player disconnects from the server.
    /// </summary>
    public class vxNetClientEventPlayerDisconnected : EventArgs
    {
        /// <summary>
        /// A copy of information pertaining to the disconnected player. The player is still in the PlayerManager until after
        /// this Event is fired.
        /// </summary>
        public vxNetPlayerInfo DisconnectedPlayer
        {
            get { return m_disconnectedPlayer; }
        }
        vxNetPlayerInfo m_disconnectedPlayer;

        /// <summary>
        /// Constructor.
        /// </summary>
        public vxNetClientEventPlayerDisconnected(vxNetPlayerInfo player)
        {
            m_disconnectedPlayer = player;
        }
    }


    /// <summary>
    /// This event is fired on the client side whenever a player needs to be updated with information from the server.
    /// </summary>
    public class vxNetClientEventPlayerStatusUpdate : EventArgs
    {
        /// <summary>
        /// The player that needs updating.
        /// </summary>
        public vxNetPlayerInfo PlayerToUpdate
        {
            get { return m_playerToUpdate; }
        }
        vxNetPlayerInfo m_playerToUpdate;

        /// <summary>
        /// Constructor.
        /// </summary>
        public vxNetClientEventPlayerStatusUpdate(vxNetPlayerInfo player)
        {
            m_playerToUpdate = player;
        }
    }


    /// <summary>
    /// This event is fired whenever the server changes the net session state (such as moving from Lobby to Playing in game)
    /// </summary>
    public class vxNetClientEventSessionStatusUpdated : EventArgs
    {
        /// <summary>
        /// ID of the Client that has been added.
        /// </summary>
        public vxEnumNetSessionState NewSessionState
        {
            get { return msg.SessionState; }
        }

        /// <summary>
        /// The start time to use
        /// </summary>
        public float MessageTimeDelay;

        public vxEnumNetSessionState PreviousSessionStatus;

        private vxNetmsgUpdateSessionState msg;

        /// <summary>
        /// Constructor.
        /// </summary>
        public vxNetClientEventSessionStatusUpdated(vxEnumNetSessionState oldStatus, vxNetmsgUpdateSessionState msg, float MessageTimeDelay)
        {
            PreviousSessionStatus = oldStatus;
            this.msg = msg;
            this.MessageTimeDelay = MessageTimeDelay;
        }
    }



    public enum NetSessionState
    { 
        /// <summary>
        /// Tells the player to go to the lobby
        /// </summary>
        Lobby,

        /// <summary>
        /// Tells the clients to shift into playing mode
        /// </summary>
        Playing,

        /// <summary>
        /// Tells the clients to load the next level
        /// </summary>
        NextLevel,

        /// <summary>
        /// Tells the clients we've reached the post game
        /// </summary>
        PostGame
    }


    /// <summary>
    /// This event is fired when the server requests that all clients go to a given state, Lobby, PlayGame, Post Game, and back to Lobby
    /// </summary>
    public class vxNetClientEvent_NetSessionStateChanged : EventArgs
    {
        /// <summary>
        /// The player that needs updating.
        /// </summary>
        public NetSessionState State
        {
            get { return _state; }
        }
        NetSessionState _state;

        /// <summary>
        /// Constructor.
        /// </summary>
        public vxNetClientEvent_NetSessionStateChanged(NetSessionState state)
        {
            _state = state;
        }
    }
}
