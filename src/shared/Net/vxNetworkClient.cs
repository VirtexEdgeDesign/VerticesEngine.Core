using Microsoft.Xna.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using VerticesEngine.Net.BackendForLidgren;
using VerticesEngine.Net.Events;
using VerticesEngine.Net.Messages;
using VerticesEngine.Profile;
using VerticesEngine.UI.Controls;

namespace VerticesEngine.Net
{
    /// <summary>
    /// This holds all events and is a wrapper around the currently running network backend
    /// </summary>
    public class vxNetworkClient : IDisposable
    {
        /// <summary>
        /// The interface for the network backend. This is the wrapper to work with Steam, Lidgren Android etc...
        /// </summary>
        vxINetworkClientBackend m_netClientBackendWrapper;


        #region -- Fields and Properties --

        /// <summary>
        /// Player Info
        /// </summary>
        public vxNetPlayerInfo PlayerState;

        public string UniqueID
        {
            get { return PlayerState.ID; }
        }


        public Version ClientVersion
        {
            get { return m_clientVersion; }
        }
        private Version m_clientVersion;


        /// <summary>
        /// The player manager
        /// </summary>
        public vxNetPlayerManager PlayerManager { get; internal set; }

        public double GetCurrentNetTime()
        {
            if (m_netClientBackendWrapper != null)
                return m_netClientBackendWrapper.GetCurrentNetTime();

            // TODO: Hack to get around server implementation
            return Lidgren.Network.NetTime.Now;
        }

        /// <summary>
        /// The current session status
        /// </summary>
        public vxEnumNetSessionState SessionStatus
        {
            get { return m_sessionStatus; }
            set { m_sessionStatus = value; }
        }
        public vxEnumNetSessionState m_sessionStatus = vxEnumNetSessionState.InLobby;

        /// <summary>
        /// Is this session a local LAN session or Dedicated Server session
        /// </summary>
        public bool IsSessionLocal
        {
            get { return _isSessionLocal; }
            internal set { _isSessionLocal = value; }
        }
        private bool _isSessionLocal = false;


        public vxINetGameplayScene CurrentNetGame
        {
            get { return m_currentScene; }
        }
        private vxINetGameplayScene m_currentScene;

        public void SetCurrentNetGame(vxINetGameplayScene gameplayScene)
        {
            m_currentScene = gameplayScene;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:VerticesEngine.Net.vxNetworkSessionManager"/>
        /// 's match is running.
        /// </summary>
        /// <value><c>true</c> if is match running; otherwise, <c>false</c>.</value>
        public bool IsMatchRunning { get; internal set; }

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
        /// Are we the host for this net session? This is set only by the server. We can be a host (i.e. authority) but 
        /// not the server when using dedicated servers
        /// </summary>
        public bool IsHost
        {
            get { return _isHost; }
        }
        private bool _isHost = false;

        /// <summary>
        /// The ID of the current host. This is mainly just for debugging purposes
        /// </summary>
        public string HostID
        {
            get { return _hostID; }
        }
        private string _hostID = string.Empty;
        /// <summary>
        /// Gets the player network role.
        /// </summary>
        /// <returns>The player network role.</returns>
        public vxEnumNetworkPlayerRole PlayerNetworkRole
        {
            get { return m_playerNetworkRole; }
        }


        /// <summary>
        /// Sets the player network role.
        /// </summary>
        /// <param name="role">Role.</param>
        internal void SetPlayerNetworkRole(vxEnumNetworkPlayerRole role)
        {
            m_playerNetworkRole = role;
        }
        vxEnumNetworkPlayerRole m_playerNetworkRole = vxEnumNetworkPlayerRole.Client;

        #endregion

        public void Start(vxNetworkBackend networkBackend)
        {
            // first reset the Network Manager
            Reset();

            // Now set the new one
            m_networkBackend = networkBackend;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.Net.vxNetworkClient"/> class.
        /// </summary>
        internal vxNetworkClient()
        {
            PlayerManager = new vxNetPlayerManager();
            IsMatchRunning = false;
        }

        void InitBackend()
        {
            switch (m_networkBackend)
            {
                case vxNetworkBackend.CrossPlatform:
                    m_netClientBackendWrapper = new vxNetworkClientBackendForLidgren(this);
                    break;
                case vxNetworkBackend.SteamP2P:
                    // TODO: Implement Steam Backend
                    break;
                case vxNetworkBackend.Android:
                    // TODO: Implement Android Backend
                    break;
                case vxNetworkBackend.iOS:
                    // TODO: Implement a better way
                    m_netClientBackendWrapper = new vxNetworkClientBackendForLidgren(this);
                    break;
            }
            m_netClientBackendWrapper.Initialise();
        }

        public bool IsInitialised = false;

        internal async void Initialise(vxNetworkBackend networkBackend)
        {
            m_clientVersion = new Version(vxEngine.Game.Version);

            //Task.Run()
            m_networkBackend = networkBackend;
            // we're starting a new network session, so let's get rid of the previous session if available
            if (m_netClientBackendWrapper != null)
            {
                if (m_netClientBackendWrapper != null)
                    m_netClientBackendWrapper.Dispose();
            }

            await Task.Run(InitBackend);

            IsInitialised = true;

            string id = Guid.NewGuid().ToString();

            // initialise the player state here
            PlayerState = new vxNetPlayerInfo(id.ToString(), vxPlatform.Player.Name, -1, vxEnumNetPlayerStatus.None);
        }


        /// <summary>
        /// The disconnect.
        /// </summary>
        public void Disconnect()
        {
            // Clear all Players
            PlayerManager.Players.Clear();

            switch (m_networkBackend)
            {
                case vxNetworkBackend.CrossPlatform:
                    m_netClientBackendWrapper?.Disconnect();
                    break;
            }

            // Shutdown the server
            if (PlayerNetworkRole == vxEnumNetworkPlayerRole.Server)
            {
                // wait for any client side disconnect code to run.
                Thread.Sleep(200);

                vxNetworkManager.Server.Shutdown();
            }
        }


        public void Dispose()
        {
            try
            {
                // let's try to disconnect
                Disconnect();
                Thread.Sleep(200);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            if (m_netClientBackendWrapper != null)
                m_netClientBackendWrapper.Dispose();
        }

        /// <summary>
        /// Sends out a discovery signal for all local servers/games running. This is usually used for LAN games.
        /// </summary>
        /// <param name="port"></param>
        public void SendLocalDiscoverySignal(int port)
        {
            m_netClientBackendWrapper.SendLocalDiscoverySignal(port);
        }


        public void SendDiscoverySignal(string ip, int port)
        {
            m_netClientBackendWrapper.SendDiscoverySignal(ip, port);
        }

        public vxINetMessageOutgoing CreateMessage()
        {
            return m_netClientBackendWrapper.CreateMessage();
        }

        public void Connect(string Address, int Port)
        {
            m_netClientBackendWrapper.Connect(Address, Port);
        }

        /// <summary>
        /// Connect with a hail message
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="Port"></param>
        public void Connect(string Address, int Port, vxINetMessageOutgoing hail)
        {
            m_netClientBackendWrapper.Connect(Address, Port, hail);
        }

        public void SendMessage(vxINetworkMessage gameMessage)
        {
            switch (m_networkBackend)
            {
                case vxNetworkBackend.CrossPlatform:
                    if (m_netClientBackendWrapper != null)
                        m_netClientBackendWrapper.SendMessage(gameMessage);
                    break;
            }
        }


        //public void InviteFriend()
        //{
        //    // TODO: Implement For Steam
        //}

        //public void OnAllPlayersReady()
        //{

        //}

        //public void OnPlayerDisconnect()
        //{

        //}

        //public void OnPlayerInvite()
        //{
        //    // TODO: Implement For Steam
        //}

        //public void OnPlayerJoined()
        //{

        //}

        //public void StartGame()
        //{

        //}

        //public void StartQuickGame()
        //{

        //}


        //public void OnMessageReceived(object msg)
        //{

        //}

        public void Draw()
        {
            if (m_netClientBackendWrapper != null)
                m_netClientBackendWrapper.DebugDraw();
        }


        #region -- Net Events --

        /// <summary>
        /// This event is fired on the client side whenever a new player connects to the server.
        /// </summary>
        public event EventHandler<vxNetClientEventDiscoverySignalResponse> DiscoverySignalResponseRecieved;

        internal void OnDiscoverySignalResponseRecieved(vxNetMsgServerInfo info)
        {
            if (DiscoverySignalResponseRecieved != null)
                DiscoverySignalResponseRecieved(this, new vxNetClientEventDiscoverySignalResponse(info));

        }

        /// <summary>
        /// This event is fired whenever this player connects to the server.
        /// </summary>
        public event EventHandler<vxNetClientEventConnected> Connected;

        internal void OnServerConnected()
        {
            // we should clear the player list
            this.PlayerManager.Players.Clear();
            _isHost = false;
            _hostID = string.Empty;

            if (Connected != null)
                Connected(this, new vxNetClientEventConnected());
        }



        /// <summary>
        /// This event is fired whenever this player disconnects from the server.
        /// </summary>
        public event EventHandler<vxNetClientEventDisconnected> Disconnected;

        internal void OnServerDisconnected(vxNetDisconnectedEventReason Reason, string AdditionalInfo = "")
        {
            if (Disconnected != null)
                Disconnected(this, new vxNetClientEventDisconnected(Reason, AdditionalInfo));

            m_currentScene?.OnNetServerDisconnected(Reason);

            // we should clear the player list
            this.PlayerManager.Players.Clear();
        }


        /// <summary>
        /// This event is fired on the client side whenever a new player connects to the server.
        /// </summary>
        public event EventHandler<vxNetClientEventPlayerConnected> OtherPlayerConnected;

        internal void OnOtherPlayerConnected(vxNetPlayerInfo newPlayerInfo)
        {
            vxNotificationManager.Show($"{newPlayerInfo.UserName} has joined the game", Color.DeepSkyBlue);

            if (OtherPlayerConnected != null)
                OtherPlayerConnected(this, new vxNetClientEventPlayerConnected(newPlayerInfo));
        }


        /// <summary>
        /// This event is fired when the client receieves the player list
        /// </summary>
        public event Action PlayerListUpdated = () => { };

        internal void OnPlayerListUpdated()
        {
            PlayerListUpdated?.Invoke();
        }

        /// <summary>
        /// This event is fired when the server tells all clients there's a new host.
        /// </summary>
        public event Action<string> PlayerHostUpdated = (id) => {};

        internal void OnPlayerHostUpdated(string newHostID)
        {
            // do we have the ID of the new host?
            if(this.UniqueID == newHostID)
            {
                vxConsole.NetLog(" >> WE HAVE BEEN CHOSEN! " + this.PlayerState.UserName);
                _isHost = true;
                PlayerHostUpdated?.Invoke(newHostID);

                vxNotificationManager.Show("We are now the Session Host", Color.DarkOrange);
            }
            else
            {
                vxConsole.NetLog(" >> " + PlayerState.UserName + "not our game...");
                _isHost = false;
            }
            _hostID = newHostID;
            vxConsole.NetLog("Host ID is " + _hostID);
        }

        /// <summary>
        /// This event is fired on the client side whenever a player disconnects from the server.
        /// </summary>
        public event EventHandler<vxNetClientEventPlayerDisconnected> OtherPlayerDisconnected;

        internal void OnOtherPlayerDisconnected(vxNetPlayerInfo playerInfo)
        {
            vxConsole.NetLog("Player Disconnected - id:" + playerInfo.ID);

            vxNotificationManager.Show($"{playerInfo.UserName} has left the game", Color.Magenta);

            if (OtherPlayerDisconnected != null)
                OtherPlayerDisconnected(this, new vxNetClientEventPlayerDisconnected(playerInfo));


            // Remove the player from the server's list
            if (PlayerManager.Players.ContainsKey(playerInfo.ID))
                PlayerManager.Players.Remove(playerInfo.ID);


            m_currentScene?.OnNetPlayerDisconnected(playerInfo);
        }

        /// <summary>
        /// When ever new information of a player is recieved.
        /// </summary>
        public event EventHandler<vxNetClientEventPlayerStatusUpdate> UpdatedPlayerInfoRecieved;

        internal void OnPlayerStatusUpdated(vxNetPlayerInfo playerInfo)
        {
            // Update The Player info
            if (PlayerManager.Players.ContainsKey(playerInfo.ID))
                PlayerManager.Players[playerInfo.ID] = playerInfo;

            if (UpdatedPlayerInfoRecieved != null)
                UpdatedPlayerInfoRecieved(this, new vxNetClientEventPlayerStatusUpdate(playerInfo));


            m_currentScene?.OnNetPlayerStatusUpdated(playerInfo);
        }

        /// <summary>
        /// When ever new player metadate is recieved.
        /// </summary>
        public event Action<vxNetmsgPlayerMetaData> UpdatedPlayerMetaDataRecieved;

        internal void OnPlayerMetaDataRecieved(vxNetmsgPlayerMetaData playerInfo)
        {
            // Update The Player info
            if (UpdatedPlayerMetaDataRecieved != null)
                UpdatedPlayerMetaDataRecieved(playerInfo);
        }

        /// <summary>
        /// When ever new player metadate is recieved.
        /// </summary>
        public event Action<vxNetmsgLevelMetaData> UpdatedLevelMetaDataRecieved;

        internal void OnLevelMetaDataRecieved(vxNetmsgLevelMetaData levelInfo)
        {
            // Update the level info
            if (UpdatedLevelMetaDataRecieved != null)
                UpdatedLevelMetaDataRecieved(levelInfo);
        }

        public event Action<vxNetmsgLevelEvent> LevelEventRecieved;

        internal void OnLevelEvent(vxNetmsgLevelEvent item)
        {
            if (LevelEventRecieved != null)
                LevelEventRecieved(item);

            m_currentScene?.OnNetLevelEvent(item);
        }

        /// <summary>
        /// When the server updates the session status.
        /// </summary>
        public event EventHandler<vxNetClientEventSessionStatusUpdated> NetSessionStatusChanged;

        internal void OnNetSessionStateChanged(vxNetmsgUpdateSessionState newSessionState, float timeDelay)
        {
            // let's fire any connected events
            if (NetSessionStatusChanged != null)
                NetSessionStatusChanged(this, new vxNetClientEventSessionStatusUpdated(SessionStatus, newSessionState, timeDelay));

            // Lets now update the current game
            var oldSessionState = SessionStatus;
            m_sessionStatus = newSessionState.SessionState;

            // only fire the session state change if the state has actually changed
            if (oldSessionState != newSessionState.SessionState)
            {
                m_currentScene?.OnNetSessionStateChanged(oldSessionState, newSessionState.SessionState, timeDelay);
            }
        }

        /// <summary>
        /// This event fires when an updated Entity State is recieved from the Server.
        /// </summary>
        public event Action<vxNetmsgUpdatePlayerEntityState, float> UpdatePlayerEntityState;

        internal void OnPlayerEntityStateUpdated(vxNetmsgUpdatePlayerEntityState updatePlayerEntityState, float timeDelay)
        {
            if (PlayerManager.TryGetPlayerInfo(updatePlayerEntityState.PlayerID, out var playerInfo))
            {
                if (this.PlayerState.ID != updatePlayerEntityState.PlayerID)
                {
                    //Then Update the Player in the client List
                    playerInfo.EntityState = updatePlayerEntityState.EntityState;
                    PlayerManager.Players[updatePlayerEntityState.PlayerID] = playerInfo;

                    //Then fire any connected events
                    if (UpdatePlayerEntityState != null)
                        UpdatePlayerEntityState(updatePlayerEntityState, timeDelay);


                    m_currentScene?.OnNetPlayerEntityStateUpdated(updatePlayerEntityState, timeDelay);
                }
            }
        }

        #endregion


        public void Reset()
        {

        }
    }
}