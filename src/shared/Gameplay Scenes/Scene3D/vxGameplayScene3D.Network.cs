
using VerticesEngine.Net;


namespace VerticesEngine
{
    public partial class vxGameplayScene3D : vxGameplaySceneBase
    {
#region Network Code

       // private NetServer server;

        //private NetClient s_client;
        public vxEnumNetworkConnectionStatus ConnectionStatus;


        public vxEnumNetworkPlayerRole PlayerNetworkRole = vxEnumNetworkPlayerRole.Client;
        //public vxGameType GameType = vxGameType.Local;

        //private string IPAddress = "";
        //private int Port = 14242;

#endregion

//        /// <summary>
//        /// Starts the Scene as a Networked Game.
//        /// </summary>
//        /// <param name="IPAddress">IP Address too Connect Too.</param>
//        /// <param name="Port">Port too Connect Too</param>
//        /// <param name="PlayerNetworkRole">Players Network Role (i.e. Client or Server).</param>
//        public vxScene3D(
//            string IPAddress,
//            int Port,
//            vxEnumNetworkPlayerRole PlayerNetworkRole
//            ) : this()
//        {
//            //Set Game Type as Networked for this Constructor
//            this.GameType = VerticesEngine.vxEnumGameType.Networked;

//            //Set the Player Role
//            this.PlayerNetworkRole = PlayerNetworkRole;

//            //Set Connection Data
//            this.IPAddress = IPAddress;
//            this.Port = Port;
//        }

//        public void InitialiseNetwork()
//        {
//            /***************************************************/
//            //              Start Network Code
//            /***************************************************/
//            if (GameType == vxEnumGameType.Networked)
//            {
//                //Set Connection Name
//                string connectionName = vxEngine.CurrentGameName + "_Server_" + IPAddress;

//                //Set up as a Server
//                if (PlayerNetworkRole == vxEnumNetworkPlayerRole.Server)
//                {
//                    vxConsole.WriteNetworkLine("Setting Up Server Upder: " + connectionName);

//                    //Set up Configuration
//                    var config = new NetPeerConfiguration(connectionName);
//                    config.Port = 14242;

//                    //Set up and start Server
//                    server = new NetServer(config);
//                    server.Start();
//                }

//                if (PlayerNetworkRole == vxEnumNetworkPlayerRole.Client)
//                {
//                    ConnectionStatus = vxEnumNetworkConnectionStatus.Stopped;
//                    var config = new NetPeerConfiguration(connectionName);
//                    s_client = new NetClient(config);

//                    s_client.Start();
//                    NetOutgoingMessage hail = s_client.CreateMessage("Hail Message");
//                    s_client.Connect(IPAddress, Port, hail);
//                }

//            }
//        }

//        public void DeinitialiseNetwork()
//        {
//            if (server != null)
//                server.Shutdown("Requested by server");

//            if (s_client != null)
//                s_client.Disconnect("Requested by user");
//        }

//        public void UpdateNetwork()
//        {
//#region Process Network Messages

//#region Process Client Network Messages

//            NetIncomingMessage im;
//            if (s_client != null)
//            {
//                while ((im = s_client.ReadMessage()) != null)
//                {
//                    // handle incoming message
//                    switch (im.MessageType)
//                    {
//                        case NetIncomingMessageType.DebugMessage:
//                        case NetIncomingMessageType.ErrorMessage:
//                        case NetIncomingMessageType.WarningMessage:
//                        case NetIncomingMessageType.VerboseDebugMessage:
//                            string text = im.ReadString();
//                            vxConsole.WriteNetworkLine(im.MessageType + " : " + text);
//                            break;
//                        case NetIncomingMessageType.StatusChanged:
//                            NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();

//                            if (status == NetConnectionStatus.Connected)
//                                ConnectionStatus = vxEnumNetworkConnectionStatus.Running;
//                            else
//                                ConnectionStatus = vxEnumNetworkConnectionStatus.Stopped;

//                            if (status == NetConnectionStatus.Disconnected)
//                            {
//                                ConnectionStatus = vxEnumNetworkConnectionStatus.Stopped;
//                            }

//                            string reason = im.ReadString();
//                            vxConsole.WriteNetworkLine(status.ToString() + ": " + reason);

//                            break;
//                        case NetIncomingMessageType.Data:

//                            // incoming chat message from a client
//                            string chat = im.ReadString();

//                            //Split the Text By Carriage Return
//                            string[] result = chat.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

//                            switch (result[0])
//                            {
//                                case "vrtx_serverList":
//                                    vxConsole.WriteNetworkLine("Server List Recieved");

//                                    break;
//                            }
//                            break;
//                        default:
//                            vxConsole.WriteNetworkLine("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes");
//                            break;
//                    }
//                    s_client.Recycle(im);
//                }
//            }
//#endregion

//#region Process Server Network Messages

//            NetIncomingMessage msg;
//            if (server != null)
//            {
//                while ((msg = server.ReadMessage()) != null)
//                {
//                    switch (msg.MessageType)
//                    {
//                        case NetIncomingMessageType.VerboseDebugMessage:
//                        case NetIncomingMessageType.DebugMessage:
//                        case NetIncomingMessageType.WarningMessage:
//                        case NetIncomingMessageType.ErrorMessage:
//                            vxConsole.WriteNetworkLine(msg.MessageType + " : " + msg.ReadString());
//                            break;
//                        case NetIncomingMessageType.StatusChanged:
//                            NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();

//                            string reason = msg.ReadString();
//                            vxConsole.WriteNetworkLine(NetUtility.ToHexString(msg.SenderConnection.RemoteUniqueIdentifier) + " " + status + ": " + reason);

//                            if (status == NetConnectionStatus.Connected)
//                            {
//                                string hail = msg.SenderConnection.RemoteHailMessage.ReadString();
//                                vxConsole.WriteNetworkLine("Remote hail: " + hail);

//                                if (hail == "vrtx_hail_msg")
//                                {
//                                    vxConsole.WriteNetworkLine("Connection Accepted");
//                                }
//                                else
//                                    vxConsole.WriteNetworkLine("Hail Message Rejected");

//                            }

//                            UpdateConnectionsList();
//                            break;
//                        case NetIncomingMessageType.Data:
//                            // incoming chat message from a client
//                            string chat = msg.ReadString();

//                            //Split the Text By Carriage Return
//                            string[] result = chat.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

//                            switch (result[0])
//                            {
//                                case "vrtx_request_newServer":

//                                    break;

//                                default:
//                                    vxConsole.WriteNetworkLine("Broadcasting '" + chat + "'");

//                                    // broadcast this to all connections, except sender
//                                    List<NetConnection> all = server.Connections; // get copy
//                                    all.Remove(msg.SenderConnection);

//                                    if (all.Count > 0)
//                                    {
//                                        NetOutgoingMessage om2 = server.CreateMessage();
//                                        om2.Write(NetUtility.ToHexString(msg.SenderConnection.RemoteUniqueIdentifier) + " said: " + chat);
//                                        server.SendMessage(om2, all, NetDeliveryMethod.ReliableOrdered, 0);
//                                    }
//                                    break;
//                            }
//                            break;
//                        default:
//                            vxConsole.WriteNetworkLine("Unhandled type: " + msg.MessageType + " " + msg.LengthBytes + " bytes " + msg.DeliveryMethod + "|" + msg.SequenceChannel);
//                            break;
//                    }
//                    server.Recycle(msg);
//                }
//            }
//#endregion

//#endregion
//        }

//        private void UpdateConnectionsList()
//        {
//            foreach (NetConnection conn in server.Connections)
//            {
//                string str = NetUtility.ToHexString(conn.RemoteUniqueIdentifier) + " from " + conn.RemoteEndPoint.ToString() + " [" + conn.Status + "]";
//                vxConsole.WriteNetworkLine(str);
//            }
//        }

    }
}