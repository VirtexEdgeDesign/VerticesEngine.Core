
using Lidgren.Network;
using System;

namespace VerticesEngine.Net.Messages
{
    /// <summary>
    /// This message is used during the discovery phase to glean basic server information.
    /// </summary>
    public struct vxNetMsgServerInfo : vxINetworkMessage
    {
        /// <summary>
        /// The Server Name
        /// </summary>
        public string AppName { get; private set; }

        /// <summary>
        /// The Server Name
        /// </summary>
        public string ServerName { get; private set; }

        /// <summary>
        /// Is this server a dedicated server?
        /// </summary>
        public bool IsDedicated { get; private set; }

        /// <summary>
        /// Server version
        /// </summary>
        public Version ServerVersion { get; private set; }

        /// <summary>
        /// The Server's IP
        /// </summary>
        public string ServerIP { get; private set; }


        /// <summary>
        /// The Server's Port
        /// </summary>
        public string ServerPort { get; private set; }

        /// <summary>
        /// Gets the number of players.
        /// </summary>
        /// <value>The number of players.</value>
        public int NumberOfPlayers { get; private set; }

        /// <summary>
        /// Gets the max number of players.
        /// </summary>
        /// <value>The max number of players.</value>
        public int MaxNumberOfPlayers { get; private set; }

        public vxEnumNetSessionState SessionState { get; private set; }

        /// <summary>
        /// The message time to recieve this server info
        /// </summary>
        public double Ping { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.Net.Messages.vxNetMsgServerInfo"/> class.
        /// </summary>
        /// <param name="ServerName">Server name.</param>
        /// <param name="ServerIP">Server ip.</param>
        /// <param name="ServerPort">Server port.</param>
        /// <param name="NumberOfPlayers">Number of players.</param>
        /// <param name="MaxNumberOfPlayers">Max number of players.</param>
        public vxNetMsgServerInfo(string AppName, string ServerName, Version ServerVersion, bool IsDedicated, string ServerIP, string ServerPort, int NumberOfPlayers, int MaxNumberOfPlayers, vxEnumNetSessionState SessionState)
        {
            this.AppName = AppName;
            this.ServerName = ServerName;
            this.ServerVersion = ServerVersion;
            this.IsDedicated = IsDedicated;
            this.ServerIP = ServerIP;
            this.ServerPort = ServerPort;
            this.NumberOfPlayers = NumberOfPlayers;
            this.MaxNumberOfPlayers = MaxNumberOfPlayers;
            this.SessionState = SessionState;
            this.Ping = 0;
        }

        /// <summary>
        /// Decoding Constructor to be used by client.
        /// </summary>
        /// <param name="im"></param>
        public vxNetMsgServerInfo(vxINetMessageIncoming im)
        {
            this.AppName = string.Empty;
            this.ServerName = string.Empty;
            this.ServerVersion = new Version();
            this.IsDedicated = false;
            this.ServerIP = string.Empty;
            this.ServerPort = string.Empty;
            this.NumberOfPlayers = 1;
            this.MaxNumberOfPlayers = 4;
            this.SessionState = vxEnumNetSessionState.InLobby;
            this.Ping = 0;
            this.DecodeMsg(im);
        }

        /// <summary>
        /// The Message Type
        /// </summary>
        public vxNetworkMessageTypes MessageType
        {
            get
            {
                return vxNetworkMessageTypes.ServerInfo;
            }
        }

        public void DecodeMsg(vxINetMessageIncoming im)
        {
            this.AppName = im.ReadString();
            this.ServerName = im.ReadString();
            this.ServerVersion = new Version(im.ReadString());
            this.IsDedicated = im.ReadBoolean();
            this.ServerIP = im.SenderEndPoint.Address.ToString();//im.ReadString();
            //vxConsole.WriteLine(""+im.ReadString());
            var ip = (""+im.ReadString());
            this.ServerPort = im.SenderEndPoint.Port.ToString();//im.ReadString();
            var port = ("" +im.ReadString());

            this.NumberOfPlayers = im.ReadInt32();
            this.MaxNumberOfPlayers = im.ReadInt32();
            this.SessionState = (vxEnumNetSessionState)im.ReadInt32();
            this.Ping = TimeSpan.FromTicks(DateTime.UtcNow.Ticks - im.ReadInt64()).TotalSeconds;
            //Ping = im.SenderConnection.AverageRoundtripTime;
        }

        public void EncodeMsg(vxINetMessageOutgoing om)
        {
            om.Write(this.AppName);
            om.Write(this.ServerName);
            om.Write(this.ServerVersion.ToString());
            om.Write(this.IsDedicated);
            om.Write(this.ServerIP);
            om.Write(this.ServerPort);
            om.Write(this.NumberOfPlayers);
            om.Write(this.MaxNumberOfPlayers);
            om.Write((int)this.SessionState);

            // get the current UTC ticks
            om.Write(DateTime.UtcNow.Ticks);
        }
    }
}
