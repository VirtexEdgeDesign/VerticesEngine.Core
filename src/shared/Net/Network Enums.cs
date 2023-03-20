using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VerticesEngine.Net
{

    /// <summary>
    /// What is the players role in Networked games.
    /// </summary>
    public enum vxEnumNetworkPlayerRole
    {
        /// <summary>
        /// The player is the server for the game.
        /// </summary>
        Server,

        /// <summary>
        /// The player is a client for the game.
        /// </summary>
        Client,
    }

    /// <summary>
    /// Which network backend are we using
    /// </summary>
    public enum vxNetworkBackend
    {
        /// <summary>
        /// The network backend being used is Cross Platform which is Lidgren
        /// </summary>
        CrossPlatform = 0,

        /// <summary>
        /// We're using the Android subsystem for networking
        /// </summary>
        Android = 1,

        /// <summary>
        /// //TODO
        /// </summary>
        iOS = 2,

        /// <summary>
        /// This uses the SteamWorks P2P API as the network backend
        /// </summary>
        SteamP2P = 3
    }

    public enum vxNetConnectionStatus
    {
        None,
        InitiatedConnect,
        ReceivedInitiation,
        RespondedAwaitingApproval,
        RespondedConnect,
        Connected,
        Disconnecting,
        Disconnected
    }


    public enum vxNetDisconnectedEventReason
    {
        /// <summary>
        /// The client has disconnected
        /// </summary>
        ClientDisconected,

        /// <summary>
        /// The server has shutdown
        /// </summary>
        ServerShutdown,

        /// <summary>
        /// An unknown disconnect reason
        /// </summary>
        Unknown
    }

    public enum vxEnumNetSessionState
    {
        /// <summary>
        /// The current server state is unknown and likely invalid. It may be starting up
        /// </summary>
        UNKNOWN,

        /// <summary>
        /// We're currently in lobby waiting for the 
        /// </summary>
        InLobby,

        /// <summary>
        /// We're pre-game waiting for players
        /// </summary>
        LoadingNextLevel,

        /// <summary>
        /// We're currently playing the game
        /// </summary>
        PlayingGame,

        /// <summary>
        /// We're in a post-game state where scores are shown and the next step can be chosen
        /// </summary>
        PostGame,
    }

    /// <summary>
    /// Connection status for networked games.
    /// </summary>
    public enum vxEnumNetworkConnectionStatus
    {
        /// <summary>
        /// Currently Connected.
        /// </summary>
        Running,

        /// <summary>
        /// Connection has Stopped.
        /// </summary>
        Stopped,

        /// <summary>
        /// The connection has timed out.
        /// </summary>
        TimedOut
    }


    /// <summary>
    /// Generic Message Types that can be sent by the Vertices Engine.
    /// </summary>
    public enum vxNetworkMessageTypes
    {
        /// <summary>
        /// Sends Server Info back to a client. Usually done during the discovery signal handshake.
        /// </summary>
        ServerInfo,

        /// <summary>
        /// The server shutdown.
        /// </summary>
        ServerShutdown,

        /// <summary>
        /// A basic status check while waiting in the lobby
        /// </summary>
        PlayerLobbyStatusRequest,

        /// <summary>
        /// A player has connected to the server.
        /// </summary>
        PlayerConnected,

        /// <summary>
        /// Updated which player is the 'host' player which acts as the session authority
        /// </summary>
        SetPlayerAsHost,

        /// <summary>
        /// A player's meta data is received such as load outs, stats etc...
        /// </summary>
        PlayerMetaData,

        /// <summary>
        /// A level's meta data to update clients which level will be played.
        /// </summary>
        LevelMetaData,

        /// <summary>
        /// A player has disconnected from the server.
        /// </summary>
        PlayerDisconnected,

        /// <summary>
        /// Updates the status of the session.
        /// </summary>
        SessionStateChanged,

        /// <summary>
        /// An Item has been added to the server.
        /// </summary>
        LevelEvent,

        /// <summary>
        /// Updates the Player list
        /// </summary>
        UpdatePlayersList,

        /// <summary>
        /// The update player state.
        /// </summary>
        UpdatePlayerLobbyStatus,

        /// <summary>
        /// Updates the State of a player. This is fired both during the heart beat as well as when ever a player presses a key, it updates
        /// it's state with the server, and the server updates all clients with the new information.
        /// </summary>
        UpdatePlayerEntityState,

        /// <summary>
        /// A Chat message is recieved.
        /// </summary>
        ChatMsgRecieved,

        /// <summary>
        /// A different type of Network Message that isn't covered by the defaults. You can handle how this is handeled 
        /// in the vxINetworkMessage inherited class Encoding and Decodings.
        /// </summary>
        Other,
    }

    /// <summary>
    /// What is the players role in Networked games.
    /// </summary>
    public enum vxEnumEntityController
    {
        /// <summary>
        /// The player is the server for the game.
        /// </summary>
        LocalPlayer,

        /// <summary>
        /// The player is a client for the game.
        /// </summary>
        NetworkedPlayer,
    }



    public enum vxEnumNetPlayerStatus
    {
        /// <summary>
        /// The player has no status, and needs to be set before doing anything else.
        /// </summary>
        None,

        /// <summary>
        /// The player is unconnected and searching for a server.
        /// </summary>
        SearchingForServer,

        /// <summary>
        /// The player is in the lobby, but not ready to start the session.
        /// </summary>
        InServerLobbyNotReady,

        /// <summary>
        /// The player is in the lobby, and ready to start the session.
        /// </summary>
        InServerLobbyReady,

        /// <summary>
        /// The player is transitioning into the game session. This is usually used to keep track of which players
        /// in the session have loaded the level, and which ones are still loading.
        /// </summary>
        TransitioningToGame,

        /// <summary>
        /// The player is ready to play 
        /// </summary>
        ReadyToPlay,

        /// <summary>
        /// The player is playing currently. This can be used for whether to accept new invites or not.
        /// </summary>
        Playing,

        /// <summary>
        /// The Player is transitioning back to the lobby.
        /// </summary>
        TransitioningToLobby
    }

}
