
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VerticesEngine;
using VerticesEngine.Net.BackendForLidgren;
using VerticesEngine.Net.Events;
using VerticesEngine.Net.Messages;

namespace VerticesEngine.Net
{
    /// <summary>
    /// The network server which is used when a player hosts a game session
    /// </summary>
    public class vxNetworkServer : IDisposable
    {
        vxINetworkServerBackend m_serverBackend;

        /// <summary>
        /// The Server Name
        /// </summary>
        public string ServerName
        {
            get
            {
                return m_serverBackend?.ServerName;
            }
        }
        /// <summary>
        /// Gets the number of players.
        /// </summary>
        /// <value>The number of players.</value>
        public int NumberOfPlayers
        {
            get { return m_serverPlayerManager.Players.Count; }
        }

        /// <summary>
        /// Gets the max number of players.
        /// </summary>
        /// <value>The max number of players.</value>
        public int MaxNumberOfPlayers
        {
            get
            {
                return vxNetworkManager.Config.MaxNumberOfPlayers;
            }
        }

        /// <summary>
        /// Is the server full?
        /// </summary>
        public bool IsServerFull
        {
            get
            {
                return (m_serverPlayerManager.Players.Count >= MaxNumberOfPlayers);
            }
        }

#region Constants and Fields


        /// <summary>
        /// Gets the network backend.
        /// </summary>
        /// <value>The network backend.</value>
        public vxNetworkBackend NetworkBackend
        {
            get { return m_networkBackend; }
        }
        vxNetworkBackend m_networkBackend = vxNetworkBackend.CrossPlatform;

        /// <summary>
        /// Gets the session status.
        /// </summary>
        /// <value>The session status.</value>
        public vxEnumNetSessionState SessionStatus
        {
            get
            {
                return m_sessionStatus;
            }
        }
        vxEnumNetSessionState m_sessionStatus = vxEnumNetSessionState.UNKNOWN;

        private vxNetPlayerManager m_serverPlayerManager;

        public int Port
        {
            get { return m_serverBackend.Port; }
        }

        public bool IsAcceptingIncomingConnections
        {
            get { return m_serverBackend.IsAcceptingIncomingConnections; }
            set { m_serverBackend.IsAcceptingIncomingConnections = value; }
        }

#endregion



        internal vxNetworkServer()
        {
            m_serverPlayerManager = new vxNetPlayerManager();
        }

        /// <summary>
        /// A version code used for checking if the client is up to date enough to connect with us
        /// </summary>
        public Version ServerVersion
        {
            get { return m_serverVersion; }
        }
        private Version m_serverVersion;

        /// <summary>
        /// Initialise the Network Backend for a given type
        /// </summary>
        /// <param name="networkBackend"></param>
        /// <param name="serverName"></param>
        /// <param name="maxPlayers"></param>
        /// <param name="port"></param>
        /// <param name="callback"></param>
        public void Initialise(vxNetworkBackend networkBackend, string serverName, string version = "1.0.0.0", int port = 14242,  Action callback = null)
        {
            m_networkBackend = networkBackend;

            m_serverVersion = new Version(version);

            // we're starting a new network session, so let's get rid of the previous session if available
            if (m_serverBackend != null)
            {
                m_serverBackend.Dispose();
            }

            switch (m_networkBackend)
            {
                case vxNetworkBackend.CrossPlatform:
                    m_serverBackend = new vxNetworkServerBackendForLidgren(this, serverName, port);
                    break;
                case vxNetworkBackend.SteamP2P:
                    // TODO: Implement Steam Backend
                    //m_serverBackend = new vxNetworkSteamServerWrapper(this, serverName, port);
                    break;
                case vxNetworkBackend.Android:
                    // TODO: Implement Android Backend for Google Play store support
                    break;
                case vxNetworkBackend.iOS:
                    m_serverBackend = new vxNetworkServerBackendForLidgren(this, serverName, port);
                    break;
            }
            m_serverBackend.Initialise();

            callback?.Invoke();
        }

        /// <summary>
        /// Starts the server, this opens the server to connection requests and places it in the lobby state
        /// </summary>
        public void Start()
        {
            vxNetworkManager.Client.SetPlayerNetworkRole(vxEnumNetworkPlayerRole.Server);

            vxConsole.NetLog("Starting Server...");

            m_sessionStatus = vxEnumNetSessionState.InLobby;
            m_serverBackend.Start();
            IsAcceptingIncomingConnections = true;

            vxConsole.NetLog("Done");
        }

        public void Shutdown()
        {
            m_serverBackend?.Shutdown();

            // if we're shutting down the server then we should reset the players role just incase they try to join
            // another server as a client
            vxNetworkManager.Client.SetPlayerNetworkRole(vxEnumNetworkPlayerRole.Client);

        }

        public void Dispose()
        {
            try
            {
                if (m_serverBackend != null)
                {
                    m_serverBackend.Shutdown();
                    // give the server a moment to send all the shutdown signals, and then dispose of it
                    System.Threading.Thread.Sleep(500);
                    m_serverBackend.Dispose();
                }
            }
            catch (Exception ex)
            {
                vxConsole.WriteException(ex);
            }
        }

#region Events

        /// <summary>
        /// This event fires when ever a Discovery Signal is Recieved
        /// </summary>
        public event EventHandler<vxNetServerEventDiscoverySignalRequest> DiscoverySignalRequestRecieved;

        internal void OnDiscoverySignalRequestRecieved(vxNetServerEventDiscoverySignalRequest discoveryRequestEvent)
        {
            if (DiscoverySignalRequestRecieved != null)
                DiscoverySignalRequestRecieved(this, discoveryRequestEvent);
        }

        /// <summary>
        /// This event fires whenever a new client connects.
        /// </summary>
        public event EventHandler<vxNetServerEventClientConnected> ClientConnected;
        internal void OnClientConnected(long playerUniqueNetID)
        {
            // a client has connected
            var msg = new vxNetServerEventClientConnected(playerUniqueNetID);
            if (ClientConnected != null)
                ClientConnected(this, msg);
        }

        /// <summary>
        /// This event fires whenever a new client disconnects.
        /// </summary>
        public event EventHandler<vxNetServerEventClientDisconnected> ClientDisconnected;
        internal void OnClientDisconnected(long playerUniqueNetID)
        {
            //Send a message to all clients to remove this client from their list.
            var id = "id" + playerUniqueNetID.ToString();

            if (m_serverPlayerManager.Players.ContainsKey(id))
            {
                var rmvMsg = new vxNetmsgRemovePlayer(m_serverPlayerManager.Players[id]);

                if (ClientDisconnected != null)
                    ClientDisconnected(this, new vxNetServerEventClientDisconnected(id));


                //Finally remove the player from the server's list
                if (m_serverPlayerManager.Players.ContainsKey(id))
                    m_serverPlayerManager.Players.Remove(id);

                //Send the message to all clients.
                m_serverBackend.SendMessage(rmvMsg);
            }

            // check if we have any players left
            if(m_serverPlayerManager.Players.Count > 0)
            {
                // we still have some players, but did we lose our host?
                if(hostPlayerID == id)
                {
                    hostPlayerID = string.Empty;
                    vxConsole.NetLog(" >> HOST LOST, CHOSING NEW HOST", ConsoleColor.Magenta);
                    CheckPlayerHostStatus();
                }
            }
            else // we have no more players, lets reset the server back to a lobby state
            {
                hostPlayerID = string.Empty;
                vxConsole.NetLog(" >> NO MORE PLAYERS, RESETTING SERVER STATE", ConsoleColor.DarkYellow);
                OnSessionStateChanged(new vxNetmsgUpdateSessionState(vxEnumNetSessionState.InLobby));
            }
        }

        /// <summary>
        /// This is the ID of the current host player. This generally is the first player in the player list.
        /// If the host player leaves the game then it will choose the next index 0 player in the player list.
        /// </summary>
        public string hostPlayerID = string.Empty;

        public event EventHandler<vxNetServerEventPlayerJoined> PlayerJoinedConnected;
        internal void OnPlayerJoinedConnected(vxNetmsgAddPlayer newPlayer)
        {
            // a player has joined, lets add them to the list
            var msg = new vxNetServerEventPlayerJoined(newPlayer);

            if (PlayerJoinedConnected != null)
                PlayerJoinedConnected(this, msg);

            //Add the new player info to the Server List
            m_serverPlayerManager.Add(newPlayer.PlayerInfo);
            newPlayer.PlayerInfo.Status = vxEnumNetPlayerStatus.InServerLobbyNotReady;

            //Now Update all clients with the Player list using the server Player List
            var newPlayerList = new vxNetmsgUpdatePlayerList(this.m_serverPlayerManager);
            m_serverBackend.SendMessage(newPlayerList);

            // Check for host id
            CheckPlayerHostStatus();
        }


        void CheckPlayerHostStatus()
        {
            if (m_serverPlayerManager.Players.Count > 0 && hostPlayerID == string.Empty)
            {
                //Console.WriteLine("hostPlayerID " + hostPlayerID);
                // get the 'first' entry
                var first = m_serverPlayerManager.Players.First();
                //Console.WriteLine("first " + first.Value.ID);
                if (hostPlayerID != first.Value.ID)
                {
                    hostPlayerID = first.Value.ID;
                    vxConsole.NetLog(">> SELECTING NEW HOST PLAYER: " + first.Value.ID + " " + first.Value.UserName);
                    vxNetmsgSetPlayerAsHost newHost = new vxNetmsgSetPlayerAsHost(first.Value);
                    m_serverBackend.SendMessage(newHost);
                }
            }
        }


        /// <summary>
        /// This event updates the player status within the server.
        /// </summary>
        public event EventHandler<vxNetServerEventPlayerStatusUpdate> UpdatePlayerStatus;
        internal void OnUpdatePlayerStatus(vxNetmsgUpdatePlayerLobbyStatus updatePlayerState)
        {
            //Update the internal server list
            m_serverPlayerManager[updatePlayerState.PlayerInfo.ID] = updatePlayerState.PlayerInfo;

            if (UpdatePlayerStatus != null)
                UpdatePlayerStatus(this, new vxNetServerEventPlayerStatusUpdate(updatePlayerState.PlayerInfo));

            //Now resend it back to all clients
            m_serverBackend.SendMessage(updatePlayerState);


            // are we loading the next level, then we should wait until each and every player reports that they're ready
            if (SessionStatus == vxEnumNetSessionState.LoadingNextLevel)
            {
                bool startServerGame = true;

                // Now Loop through all players
                //lock (PlayerManager.Players)
                {
                    try
                    {
                        foreach (var player in m_serverPlayerManager.Players)
                        {
                            if (player.Value.Status != vxEnumNetPlayerStatus.ReadyToPlay)
                            {
                                startServerGame = false;
                            }
                        }

                        if (startServerGame)
                        {
                            //m_sessionStatus = vxEnumNetSessionState.PlayingGame;
                            //m_serverBackend.SendMessage(new vxNetmsgUpdateSessionState(m_sessionStatus));

                            OnSessionStateChanged(new vxNetmsgUpdateSessionState(vxEnumNetSessionState.PlayingGame));
                        }
                    }
                    catch(Exception ex)
                    {
                        vxConsole.WriteException(ex);
                    }
                }
            }
        }

        /// <summary>
        /// This Event fires when ever the server recieves updated information for an Entity State 
        /// from a client.
        /// </summary>
        public event EventHandler<vxNetmsgUpdatePlayerEntityState> UpdatePlayerEntityState;
        internal void OnUpdatePlayerEntityState(vxNetmsgUpdatePlayerEntityState updatePlayerEntityState)
        {
            //Relay the updated state too all clients.
            m_serverBackend.SendMessage(updatePlayerEntityState);

            // now let's update our own state
            if (m_serverPlayerManager.TryGetPlayerInfo(updatePlayerEntityState.PlayerID, out var playerInfo))
            {
                //Then Update the Player in the Server List
                playerInfo.EntityState = updatePlayerEntityState.EntityState;
                m_serverPlayerManager[updatePlayerEntityState.PlayerID] = playerInfo;

                //Then fire any tied events in the server
                if (UpdatePlayerEntityState != null)
                    UpdatePlayerEntityState(this, updatePlayerEntityState);
            }
        }

        internal void OnUpdatePlayerMetaData(vxNetmsgPlayerMetaData playerMetaData)
        {
            // relay the message to all clients
            m_serverBackend.SendMessage(playerMetaData);
        }

        public event Action<vxEnumNetSessionState> SessionStateChanged = (state) => { };

        internal void OnSessionStateChanged(vxNetmsgUpdateSessionState sessionStateChangedMsg)
        {
            m_sessionStatus = sessionStateChangedMsg.SessionState;

            vxConsole.NetLog("SERVER SHIFTING TO NEW STATE: " + m_sessionStatus, ConsoleColor.Green);

            // we only accept connections IFF we're in the lobby state
            IsAcceptingIncomingConnections = m_sessionStatus == vxEnumNetSessionState.InLobby;

            // relay the message to all clients
            m_serverBackend.SendMessage(sessionStateChangedMsg);

            // now invoke all server events
            SessionStateChanged?.Invoke(m_sessionStatus);
        }



#endregion
    }
}
