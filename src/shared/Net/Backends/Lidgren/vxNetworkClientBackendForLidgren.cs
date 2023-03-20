
using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using VerticesEngine.Graphics;
using VerticesEngine.Net;
using VerticesEngine.Net.Events;
using VerticesEngine.Net.Messages;

namespace VerticesEngine.Net.BackendForLidgren
{
    /// <summary>
    /// The general network client manager which uses Lidgren as the back end.
    /// </summary>
    internal class vxNetworkClientBackendForLidgren : vxINetworkClientBackend
    {
        #region Constants and Fields

        /// <summary>
        /// The is disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return m_isDisposed; }
        }
        private bool m_isDisposed;

        private SendOrPostCallback m_clientCallBackLoop;


        /// <summary>
        /// The NetPeer Server object
        /// </summary>
        private NetClient m_lidgrenNetClient;

        private vxNetworkClient m_netClientManager;


        #endregion



        public vxNetworkClientBackendForLidgren(vxNetworkClient netClientManager)
        {
            m_netClientManager = netClientManager;

            NetUtility.SetNetUtilPlatform(vxNetworkManager.Config.LidgrenNetUIPlatform);

            //Why? Bc Linux hates me.
            if (SynchronizationContext.Current == null)
				SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());


            //PlayerManager = new vxNetPlayerManager(this.Engine);
            var config = new NetPeerConfiguration(vxNetworkManager.Config.GameName)
            {
#if DEBUG // let's not ever ship with this on by accident :D
                //SimulatedMinimumLatency = 0.2f,
                // SimulatedLoss = 0.1f
#endif
            };

            config.EnableMessageType(NetIncomingMessageType.WarningMessage);
            config.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            config.EnableMessageType(NetIncomingMessageType.ErrorMessage);
            config.EnableMessageType(NetIncomingMessageType.Error);
            config.EnableMessageType(NetIncomingMessageType.DebugMessage);
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            //config.EnableMessageType(NetIncomingMessageType.ConnectionLatencyUpdated);

            m_lidgrenNetClient = new NetClient(config);


            m_clientCallBackLoop = new SendOrPostCallback(ClientMsgCallback);
            m_lidgrenNetClient.RegisterReceivedCallback(m_clientCallBackLoop);
        }

        public void Initialise()
        {
            m_lidgrenNetClient.Start();
        }

        public string UniqueIdentifier
        {
            get
            {
                return "id"+m_lidgrenNetClient.UniqueIdentifier.ToString();
            }
        }

        #region Public Methods and Operators

        /// <summary>
        /// The connect.
        /// </summary>
        public void Connect(string Address, int Port)
        {
            this.m_lidgrenNetClient.Connect(new IPEndPoint(NetUtility.Resolve(Address), Port));
        }

        /// <summary>
        /// Connect with a hail message
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="Port"></param>
        public void Connect(string Address, int Port, vxINetMessageOutgoing hail)
        {
            this.m_lidgrenNetClient.Connect(new IPEndPoint(NetUtility.Resolve(Address), Port),  ToBackendType(hail));
        }

        NetOutgoingMessage ToBackendType(vxINetMessageOutgoing om)
        {
            return ((vxNetMessageOutgoingLidgren)om).dataWriter;
        }

        /// <summary>
        /// The disconnect.
        /// </summary>
        public void Disconnect()
        {            
            m_lidgrenNetClient.Disconnect("Client Disconnected");
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }


        public vxINetMessageOutgoing CreateMessage()
        {
            return new vxNetMessageOutgoingLidgren(m_lidgrenNetClient.CreateMessage());
        }

        public vxINetMessageIncoming ReadMessage()
        {
            return new vxNetMessageIncomingLidgren(m_lidgrenNetClient.ReadMessage());
        }

        public void Recycle(vxINetMessageIncoming im)
        {
            var lidgrenMsg = (vxNetMessageIncomingLidgren)im;
            this.m_lidgrenNetClient.Recycle(lidgrenMsg.im);
        }

        /// <summary>
        /// The send message.
        /// </summary>
        /// <param name="gameMessage">
        /// The game message.
        /// </param>
        public void SendMessage(vxINetworkMessage gameMessage)
        {
            var om = new vxNetMessageOutgoingLidgren(this.m_lidgrenNetClient.CreateMessage());
            om.Write((byte)gameMessage.MessageType);
            gameMessage.EncodeMsg(om);
            
            this.m_lidgrenNetClient.SendMessage(om.dataWriter, NetDeliveryMethod.ReliableUnordered);
        }


        /// <summary>
        /// Emit a discovery signal
        /// </summary>
        public void SendLocalDiscoverySignal(int port)
        {
            m_lidgrenNetClient.DiscoverLocalPeers(port);
        }
        //int port;
        public void SendDiscoverySignal(string ip, int port)
        {
            //this.port = port;
            m_lidgrenNetClient.DiscoverKnownPeer(ip, port);
        }
        
        //private async void SendLidgrenSignalAsync()
        //{
        //    m_lidgrenNetClient.DiscoverLocalPeers(port);
        //    //await Task.Run(delegate
        //    //{
        //    //    m_lidgrenNetClient.DiscoverLocalPeers(port);
        //    //});
        //}

        /// <summary>
        /// The dispose.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        private void Dispose(bool disposing)
        {
            if (!this.m_isDisposed)
            {
                if (disposing)
                {
                    this.Disconnect();
                }

                this.m_isDisposed = true;
            }

            m_lidgrenNetClient.Disconnect(vxNetDisconnectedEventReason.ClientDisconected.ToString());
            m_lidgrenNetClient.Shutdown(vxNetDisconnectedEventReason.ClientDisconected.ToString());
        }



        int local = -50;
        int Req = -50;
        public void DebugDraw()
        {
            if (m_lidgrenNetClient != null)
            {
                Req = 5;
                local = vxMathHelper.Smooth(local, Req, 8);
                string output = string.Format(
                    "NETWORK DEBUG INFO: | User Roll: {3} | Client Name: {0} | Port: {1} | Broadcast Address: {2} | Status: {4}",
                    m_lidgrenNetClient.Configuration.AppIdentifier,
                    m_lidgrenNetClient.Configuration.Port.ToString(),
                    m_lidgrenNetClient.Configuration.BroadcastAddress,
                    m_netClientManager.PlayerNetworkRole.ToString(),
                    m_lidgrenNetClient.Status.ToString());

                int pad = 3;
            
            vxGraphics.SpriteBatch.Draw(vxInternalAssets.Textures.Blank, new Rectangle(0, local + 0, 1000, (int)vxInternalAssets.Fonts.DebugFont.MeasureString(output).Y + 2 * pad), Color.Black * 0.75f);
            vxGraphics.SpriteBatch.DrawString(vxInternalAssets.Fonts.DebugFont, output, new Vector2(pad, local + pad), Color.White);
            }
        }

        public double GetCurrentNetTime()
        {
            return NetTime.Now;
        }

        #endregion

        #region Callback Loop

        /// <summary>
        /// Method for Receiving Messages kept in a Global Scope for the Engine.
        /// </summary>
        /// <param name="peer">Peer.</param>
        void ClientMsgCallback(object peer)
        {
            NetIncomingMessage im;

            while ((im = m_lidgrenNetClient.ReadMessage()) != null)
            {
                var vxim = new vxNetMessageIncomingLidgren(im);
                //vxConsole.WriteNetworkLine(im.MessageType);
                switch (im.MessageType)
                {
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        //LogClient("DEBUG: " + im.ReadString());
                        break;
                    /**************************************************************/
                    //DiscoveryResponse
                    /**************************************************************/
                    case NetIncomingMessageType.DiscoveryResponse:

                        vxConsole.NetLog("     ------- Server found at: " + im.SenderEndPoint);
                        //Console.w im.SenderEndPoint.Address
                        var serverDisco = new vxNetMsgServerInfo(vxim);

                        // check the server version against our game version
                        if(m_netClientManager.ClientVersion < serverDisco.ServerVersion)
                        {
                            vxDebug.LogNet($"SERVER IS UPDATED, WE SHOULD UPDATE THE CLIENT. SERVER: {serverDisco.ServerVersion} CLIENT: {m_netClientManager.ClientVersion}");
                        }

                        //Fire the RecieveDiscoverySignalResponse Event by passing down the decoded Network Message
                        m_netClientManager.OnDiscoverySignalResponseRecieved(serverDisco);
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        switch ((NetConnectionStatus)im.ReadByte())
                        {
                            case NetConnectionStatus.Connected:
                                {
                                    vxConsole.NetLog(string.Format("Client Connected to {0}", im.SenderEndPoint));

                                    //Fire the Connected Event
                                    m_netClientManager.OnServerConnected();

                                    m_netClientManager.PlayerState.SetID(UniqueIdentifier);
                                }
                                break;
                            case NetConnectionStatus.Disconnected:
                                {
                                    vxConsole.NetLog(string.Format("Client Disconnected from {0}", im.SenderEndPoint));
                                    //Console.WriteLine();
                                    string reasonText = im.ReadString();

                                    vxNetDisconnectedEventReason reason = vxNetDisconnectedEventReason.Unknown;
                                    if (Enum.TryParse<vxNetDisconnectedEventReason>(reasonText, out var res))
                                    {
                                        reason = res;
                                    }
                                    //Fire the Connected Event
                                    m_netClientManager.OnServerDisconnected(reason);
                                }
                                break;
                            default:
                                vxConsole.NetLog(im.ReadString());
                                break;
                        }
                        break;
                    case NetIncomingMessageType.Data:
                        var gameMessageType = (vxNetworkMessageTypes)im.ReadByte();
                        //vxConsole.NetLog(gameMessageType.ToString());
                        switch (gameMessageType)
                        {
                            case vxNetworkMessageTypes.UpdatePlayersList:

                                //Get the Message Containing the Updated Player List
                                var updatedPlayerList = new vxNetmsgUpdatePlayerList(vxim);

                                //Now Loop through each player in the list
                                foreach (vxNetPlayerInfo serverPlayer in updatedPlayerList.Players)
                                {
                                    //First Check if the Server Player is in the clients list. If not, then add the server player to the clients list.
                                    if (m_netClientManager.PlayerManager.Contains(serverPlayer))
                                    {
                                        m_netClientManager.PlayerManager.Players[serverPlayer.ID] = serverPlayer;
                                    }
                                    else
                                    {
                                        //Add Player to Player manager
                                        m_netClientManager.PlayerManager.Add(serverPlayer);

                                        //Now Fire the Event Handler
                                        m_netClientManager.OnOtherPlayerConnected(serverPlayer);
                                    }

                                    // our player index may have changed, we should update it
                                    if(serverPlayer.ID == m_netClientManager.UniqueID)
                                    {
                                        vxNetworkManager.Client.PlayerState.PlayerIndex = serverPlayer.PlayerIndex;
                                    }
                                }
                                m_netClientManager.OnPlayerListUpdated();
                                break;


                            case vxNetworkMessageTypes.PlayerDisconnected:

                                //For what ever reason, a player has disconnected, so we need to remove it from the player list
                                var rmvMsg = new vxNetmsgRemovePlayer(vxim);

                                //Fire the Disconnected Event
                                m_netClientManager.OnOtherPlayerDisconnected(rmvMsg.PlayerInfo);

                                break;

                                // we've recieved a message from the server that there's a new host player, we should check if its us
                            case vxNetworkMessageTypes.SetPlayerAsHost:
                                var newHostIDMsg = new vxNetmsgSetPlayerAsHost(vxim);
                                m_netClientManager.OnPlayerHostUpdated(newHostIDMsg.HostID);
                                break;

                            /**************************************************************/
                            //Handle Player Meta Data Update
                            /**************************************************************/
                            case vxNetworkMessageTypes.PlayerMetaData:
                                //Decode the list
                                var playerMetaData = vxNetworkManager.Config.GetPlayerMetaData(vxim);

                                // fire off the event
                                m_netClientManager.OnPlayerMetaDataRecieved(playerMetaData);

                                break;

                            case vxNetworkMessageTypes.LevelMetaData:
                                //Decode the list
                                var levelMetaData = vxNetworkManager.Config.GetLevelMetaData(vxim);
                                m_netClientManager.IsSessionLocal = levelMetaData.IsDedicated;
                                // fire off the event
                                m_netClientManager.OnLevelMetaDataRecieved(levelMetaData);

                                break;

                            case vxNetworkMessageTypes.UpdatePlayerLobbyStatus:

                                //Decode the list
                                var updatePlayerState = new vxNetmsgUpdatePlayerLobbyStatus(vxim);

                                //Update the internal server list
                                m_netClientManager.OnPlayerStatusUpdated(updatePlayerState.PlayerInfo);

                                break;

                            case vxNetworkMessageTypes.SessionStateChanged:
                                var newSessionStatus = new vxNetmsgUpdateSessionState(vxim);

                                var sessionTimeDelay = (float)(NetTime.Now - im.SenderConnection.GetLocalTime(newSessionStatus.MessageTime));
                                //Console.WriteLine("SESSION CHANGE DELAY: " + sessionTimeDelay);
                                //Set the new Session Status
                                m_netClientManager.OnNetSessionStateChanged(newSessionStatus, sessionTimeDelay);
                                break;


                            case vxNetworkMessageTypes.UpdatePlayerEntityState:

                                //First decode the message
                                var updatePlayerEntityState = new vxNetmsgUpdatePlayerEntityState(vxim);

                                ucnt++;
                                //if(vxConsole.IsVerboseErrorLoggingEnabled)
                                {
                                    //var timeDelay = (float)(NetTime.Now - im.SenderConnection.GetLocalTime(updatePlayerEntityState.MessageTime));
                                    var relayTime = NetTime.Now - updatePlayerEntityState.sentTime;
                                    var flightTime = updatePlayerEntityState.totalFlightTime;
                                    //var jumpCount = updatePlayerEntityState.relays;
                                    //var timeDelay = (float)(NetTime.Now - updatePlayerEntityState.MessageTime);
                                    //vxConsole.NetLog("\t"+ucnt + ":timeDelay:\t" + timeDelay);
                                    //vxConsole.NetLog("\t" + ucnt + ":relayTime:\t" + relayTime);
                                    //vxConsole.NetLog("\t" + ucnt + ":flightTime:\t" + flightTime);
                                    //vxConsole.NetLog("\t" + ucnt + ":jumpCount:\t" + jumpCount);
                                }
                                m_netClientManager.OnPlayerEntityStateUpdated(updatePlayerEntityState, (float)updatePlayerEntityState.totalFlightTime * 1000.0f);
                                
                                break;

                            case vxNetworkMessageTypes.LevelEvent:

                                var lvlEventMsg = vxNetworkManager.Config.GetLevelEventMessage(vxim);
                                m_netClientManager.OnLevelEvent(lvlEventMsg);
                                break;

                            case vxNetworkMessageTypes.Other:
                                vxConsole.WriteWarning("CLIENT", "WARNING");
                                break;
                        }
                        break;
                }

                Recycle(vxim);
            }
        }
        static int ucnt = 0;
        #endregion

    }
}
