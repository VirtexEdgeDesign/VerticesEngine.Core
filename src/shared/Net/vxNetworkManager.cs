using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.Net
{
    public static class vxNetworkManager
    {
        /// <summary>
        /// The network client which handles all messaging between the server
        /// </summary>
        public static vxNetworkClient Client
        {
            get { return m_client; }
        }
        private static vxNetworkClient m_client;


        /// <summary>
        /// The network server for if the player is hosting a game
        /// </summary>
        public static vxNetworkServer Server
        {
            get { return m_server; }
        }
        private static vxNetworkServer m_server;

        /// <summary>
        /// Is this player the current host of the game
        /// </summary>
        public static bool IsHost
        {
            get { return m_client.IsHost; }
            //get { return PlayerNetworkRole == vxEnumNetworkPlayerRole.Server; }
        }

        /// <summary>
        /// The ID of the current host. This is mainly just for debugging purposes
        /// </summary>
        public static string HostID
        {
            get { return m_client.HostID; }
        }

        /// <summary>
        /// What is this players Network Role
        /// </summary>
        public static vxEnumNetworkPlayerRole PlayerNetworkRole
        {
            get { return Client.PlayerNetworkRole; }
        }

        /// <summary>
        /// Net Identifier, is it a server, if so what server name, is it a client, if so then what client. 
        /// This is useful when multiple clients and/or servers are run through the same console.
        /// </summary>
        public static string NetID
        {
            get
            {
                if(PlayerNetworkRole == vxEnumNetworkPlayerRole.Server)
                {
                    return $"{PlayerNetworkRole}:{m_server.ServerName}";
                }
                else // we're a client
                {
                    return $"{PlayerNetworkRole}:{m_client.PlayerState.UserName}";
                }
            }
        }

        internal static vxINetworkConfig Config;

        /// <summary>
        /// Initialises the Network Manager
        /// </summary>
        /// <param name="config"></param>
        public static void Init(vxINetworkConfig config)
        {
            Config = config;

            m_client = new vxNetworkClient();

            m_server = new vxNetworkServer();
        }

        public static void Dispose()
        {
            if (Server != null)
            {
                Server.Dispose();
            }
            
            System.Threading.Thread.Sleep(200);
            
            if (Client != null)
                Client.Dispose();
        }

        public static double GetCurrentNetTime()
        {
            if(m_client != null)
                return m_client.GetCurrentNetTime();

            // TODO: Hack to get around server implementation
            return Lidgren.Network.NetTime.Now;
        }
    }
}
