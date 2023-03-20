
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using VerticesEngine;
using VerticesEngine.Net.Events;
using VerticesEngine.Net.Messages;

namespace VerticesEngine.Net.BackendForLidgren
{
    internal class vxNetworkServerBackendForLidgren : vxINetworkServerBackend
    {
        public NetPeerStatus ServerStatus
        {
            get { return m_lidgrenNetServer.Status; }
        }

        public string ServerName
        {
            get { return _serverName; }
        }
        private string _serverName = "VerticesServer";


        public int Port => port;
        private int port = 0;

        private NetServer m_lidgrenNetServer;

        private SendOrPostCallback m_serverCallBackLoop;

        vxNetworkServer m_netServerManager;

        public bool IsAcceptingIncomingConnections { 
            get => m_lidgrenNetServer.Configuration.AcceptIncomingConnections; 
            set => m_lidgrenNetServer.Configuration.AcceptIncomingConnections=value; 
        }

        

        public vxNetworkServerBackendForLidgren(vxNetworkServer netServerManager, string serverName, int port)
        {
            m_netServerManager = netServerManager;

            this._serverName = serverName;

            this.port = port;

            NetUtility.SetNetUtilPlatform(vxNetworkManager.Config.LidgrenNetUIPlatform);

            //Why? Bc Linux hates me.
            if (SynchronizationContext.Current == null)
				SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
        }

        /// <summary>
        /// The connect.
        /// </summary>
        public virtual void Initialise()
        {
            
            var config = new NetPeerConfiguration(vxNetworkManager.Config.GameName)
            {
                Port = port,
            };
            config.EnableMessageType(NetIncomingMessageType.WarningMessage);
            config.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            config.EnableMessageType(NetIncomingMessageType.ErrorMessage);
            config.EnableMessageType(NetIncomingMessageType.Error);
            config.EnableMessageType(NetIncomingMessageType.DebugMessage);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            m_lidgrenNetServer = new NetServer(config);


            m_serverCallBackLoop = new SendOrPostCallback(ServerMsgCallBack);
            m_lidgrenNetServer.RegisterReceivedCallback(m_serverCallBackLoop);
        }
        #region Public Methods and Operators

        public void Start()
        {
            this.m_lidgrenNetServer.Start();
        }


        /// <summary>
        /// The disconnect.
        /// </summary>
        public void Shutdown()
        {
            if(m_lidgrenNetServer != null)
                m_lidgrenNetServer.Shutdown(vxNetDisconnectedEventReason.ServerShutdown.ToString());
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// The read message.
        /// </summary>

        public vxINetMessageOutgoing CreateMessage()
        {
            return new vxNetMessageOutgoingLidgren(m_lidgrenNetServer.CreateMessage());
        }

        public vxINetMessageIncoming ReadMessage()
        {
            return new vxNetMessageIncomingLidgren(m_lidgrenNetServer.ReadMessage());
        }

        /// <summary>
        /// The recycle.
        /// </summary>
        /// <param name="im">
        /// The im.
        /// </param>
        public void Recycle(NetIncomingMessage im)
        {
            this.m_lidgrenNetServer.Recycle(im);
        }

        /// <summary>
        /// The send message.
        /// </summary>
        /// <param name="gameMessage">
        /// The game message.
        /// </param>
        public void SendMessage(vxINetworkMessage gameMessage)
        {
            var om = CreateMessage();
            om.Write((byte)gameMessage.MessageType);
            gameMessage.EncodeMsg(om);
            
            var lidgrenMsg = (vxNetMessageOutgoingLidgren)om;            
            this.m_lidgrenNetServer.SendToAll(lidgrenMsg.dataWriter, NetDeliveryMethod.ReliableUnordered);
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        private void Dispose(bool disposing)
        {
            Shutdown();
        }

        public double GetCurrentNetTime()
        {
            return NetTime.Now;
        }


        public void DebugDraw()
        {
            
        }

        #endregion

        #region Callback Loop

        private string GetPlayerID(NetIncomingMessage im)
        {
            return "id" + im.SenderConnection.RemoteUniqueIdentifier;
        }

        int discoCount = 0; // a simple counter for counting the number of pings we've receievd

        private void ServerMsgCallBack(object peer)
        {
            NetIncomingMessage im;
            while ((im = m_lidgrenNetServer.ReadMessage()) != null)
            {
                var vxim = new vxNetMessageIncomingLidgren(im);
                //vxConsole.WriteLine(m_netServerManager.SessionStatus + ": " + im.MessageType);
                switch (im.MessageType)
                {
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        //LogServer(im.ReadString());
                        break;
                    case NetIncomingMessageType.ConnectionLatencyUpdated:
                        Console.WriteLine(im.ReadFloat());
                        //this.PlayerManager.Entites [im.SenderConnection.RemoteUniqueIdentifier].Ping = im.ReadFloat();

                        break;

                    /**************************************************************/
                    //Handle Discovery
                    /**************************************************************/
                    case NetIncomingMessageType.DiscoveryRequest:

                        // We still want to respond to other players that we're here, we may just be busy
                        // We won't accept any connection requests unless we're in the lobby
                        //if (IsAcceptingIncomingConnections)
                        {
                            //vxConsole.NetLog(string.Format("     ------- Discovery Request Recieved from: {0}", im.SenderEndPoint));
                            vxConsole.NetLog(string.Format(" >-< Discovery Request Recieved : No. {0}", discoCount++));
                            vxNetServerEventDiscoverySignalRequest discoveryRequestEvent = new vxNetServerEventDiscoverySignalRequest(im.SenderEndPoint);

                            m_netServerManager.OnDiscoverySignalRequestRecieved(discoveryRequestEvent);

                            if (discoveryRequestEvent.SendResponse == true)
                            {
                                // Create a response and write some example data to it
                                var response = new vxNetMessageOutgoingLidgren(this.m_lidgrenNetServer.CreateMessage());

                                //Send Back Connection Information, the client will still need to Authenticate with a Secret though                            
                                var resp = new vxNetMsgServerInfo(
                                    m_lidgrenNetServer.Configuration.AppIdentifier,
                                    _serverName,
                                    m_netServerManager.ServerVersion,
                                    vxNetworkManager.Config.IsDedicatedServer,
                                    m_lidgrenNetServer.Configuration.BroadcastAddress.ToString(),
                                    m_lidgrenNetServer.Configuration.Port.ToString(),
                                    m_netServerManager.NumberOfPlayers,
                                    m_netServerManager.MaxNumberOfPlayers,
                                    m_netServerManager.SessionStatus);

                                resp.EncodeMsg(response);
                                
                                // Send the response to the sender of the request
                                m_lidgrenNetServer.SendDiscoveryResponse(response.dataWriter, im.SenderEndPoint);
                            }
                            else
                            {
                                //The discovery response was blocked in the event. Notify on the server why but do not respond to the client.
                                //vxConsole.NetLog(string.Format("\n--------------------------\nDISCOVERY REQUEST BLOCKED\n IPEndpoint: '{0}'\nREASON: '{1}' \n--------------------------\n", im.SenderEndPoint, discoveryRequestEvent.Response));
                                vxConsole.NetLog(string.Format("\n--------------------------\nDISCOVERY REQUEST BLOCKED\n IPEndpoint: '{0}'\nREASON: '{1}' \n--------------------------\n", "", discoveryRequestEvent.Response));
                            }
                        }

                        break;



                    /**************************************************************/
                    //Handle Connection Approval
                    /**************************************************************/
                    case NetIncomingMessageType.ConnectionApproval:
                        
                        string s = im.ReadString();
                        if (s == "secret" && m_netServerManager.IsServerFull == false && IsAcceptingIncomingConnections == true)
                        {
                            NetOutgoingMessage approve = m_lidgrenNetServer.CreateMessage();
                            im.SenderConnection.Approve(approve);
                            //vxConsole.NetLog(string.Format("{0} Connection Approved", im.SenderEndPoint.Port));
                            vxConsole.NetLog(string.Format("{0} Connection Approved", ""));
                        }
                        else if (IsAcceptingIncomingConnections == false)
                        {
                            im.SenderConnection.Deny("Server Currently Running Game");
                            //vxConsole.NetLog(string.Format("{0} Connection DENIED! - Server Currently Running Game", im.SenderEndPoint));
                            vxConsole.NetLog(string.Format("{0} Connection DENIED! - Server Currently Running Game", ""));
                        }
                        else if (m_netServerManager.IsServerFull)
                        {
                            im.SenderConnection.Deny("Server Is Full");
                            //vxConsole.NetLog(string.Format("{0} Connection DENIED! - Server Is Full", im.SenderEndPoint));
                            vxConsole.NetLog(string.Format("{0} Connection DENIED! - Server Is Full", ""));
                        }
                        else
                        {
                            im.SenderConnection.Deny();

                            vxConsole.NetLog(string.Format("{0} Connection DENIED! - Unknown Reason", ""));
                        }
                        break;


                    case NetIncomingMessageType.StatusChanged:
                        var id = GetPlayerID(im);
                        switch ((NetConnectionStatus)im.ReadByte())
                        {
                            case NetConnectionStatus.Connected:
                                vxConsole.NetLog(string.Format("Client {0} Connected", m_netServerManager.NumberOfPlayers));
                                m_netServerManager.OnClientConnected(im.SenderConnection.RemoteUniqueIdentifier);
                                break;
                            case NetConnectionStatus.Disconnected:
                                vxConsole.NetLog(string.Format("Client {0} Disconnected", m_netServerManager.NumberOfPlayers));
                                m_netServerManager.OnClientDisconnected(im.SenderConnection.RemoteUniqueIdentifier);
                                break;
                            default:
                                vxConsole.NetLog(im.ReadString());
                                break;
                        }
                        break;
                    case NetIncomingMessageType.Data:
                        var gameMessageType = (vxNetworkMessageTypes)im.ReadByte();
                        switch (gameMessageType)
                        {

                            /**************************************************************/
                            //Handle New Player Connection
                            /**************************************************************/
                            case vxNetworkMessageTypes.PlayerConnected:
                                var newPlayer = new vxNetmsgAddPlayer(new vxNetMessageIncomingLidgren(im));
                                newPlayer.PlayerInfo.SetID("id" + im.SenderConnection.RemoteUniqueIdentifier.ToString());
                                m_netServerManager.OnPlayerJoinedConnected(newPlayer);
                                break;

                            /**************************************************************/
                            //Handle Player Lobby Status Update
                            /**************************************************************/
                            case vxNetworkMessageTypes.UpdatePlayerLobbyStatus:
                                //Decode the list
                                var updatePlayerState = new vxNetmsgUpdatePlayerLobbyStatus(vxim);
                                m_netServerManager.OnUpdatePlayerStatus(updatePlayerState);
                                break;

                            /**************************************************************/
                            //Handle Player Meta Data Update
                            /**************************************************************/
                            case vxNetworkMessageTypes.PlayerMetaData:
                                //Decode the list
                                var playerMetaData = vxNetworkManager.Config.GetPlayerMetaData(vxim);

                                SendMessage(playerMetaData);

                                break;

                            case vxNetworkMessageTypes.LevelMetaData:
                                //Decode the list
                                var levelMetaData = vxNetworkManager.Config.GetLevelMetaData(vxim);

                                SendMessage(levelMetaData);

                                break;

                            /**************************************************************/
                            //Handles an Update to a Player's Entity State
                            /**************************************************************/
                            case vxNetworkMessageTypes.UpdatePlayerEntityState:
                                //First decode the message
                                var updatePlayerEntityState = new vxNetmsgUpdatePlayerEntityState(vxim);
                                m_netServerManager.OnUpdatePlayerEntityState(updatePlayerEntityState);
                                break;

                            // we've recieved a session state changed message, likely from the host
                            case vxNetworkMessageTypes.SessionStateChanged:
                                //First decode the message
                                var sessionStateChangedMsg = new vxNetmsgUpdateSessionState(vxim);
                                m_netServerManager.OnSessionStateChanged(sessionStateChangedMsg);
                                break;

                            /**************************************************************/
                            //Handles Spawning and Removing of Items
                            /**************************************************************/
                            case vxNetworkMessageTypes.LevelEvent:
                                var spawnItem = vxNetworkManager.Config.GetLevelEventMessage(vxim);

                                SendMessage(spawnItem);

                                break;

                            case vxNetworkMessageTypes.Other:

                                break;

                        }
                        break;


                    default:
                        break;
                }

                m_lidgrenNetServer.Recycle(im);
            }
        }

        #endregion

    }
}
