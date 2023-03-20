
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using VerticesEngine.Net.Messages;

namespace VerticesEngine.Net.Events
{
    /// <summary>
    /// This event is fired whenever a discovery request is recieved from a client.
    /// </summary>
    public class vxNetServerEventDiscoverySignalRequest : EventArgs
    {
        public IPEndPoint IPEndPoint
        {
            get { return m_IPEndPoint; }
        }
        IPEndPoint m_IPEndPoint;


        /// <summary>
        /// The address of where the Discovery Signal originates from.
        /// </summary>
        public string Address
        {
            get { return m_IPEndPoint.Address.ToString(); }
        }

        /// <summary>
        /// The port of where the Discovery Signal originates from.
        /// </summary>
        public int Port
        {
            get { return m_IPEndPoint.Port; }
        }

        /// <summary>
        /// Should a response be sent for this IP? This is a handy way to block requests from blacklisted IPs.
        /// </summary>
        public bool SendResponse
        {
            get { return m_sendResponse; }
            set { m_sendResponse = value; }
        }
        private bool m_sendResponse = true;

        /// <summary>
        /// The address of where the Discovery Signal originates from.
        /// </summary>
        public string Response
        {
            get { return m_response; }
            set { m_response = value; }
        }
        private string m_response = "Hello From The Server!";

        /// <summary>
        /// Constructor.
        /// </summary>
        public vxNetServerEventDiscoverySignalRequest(IPEndPoint ipendpoint)
        {
            this.m_IPEndPoint = ipendpoint;
        }
    }


    /// <summary>
    /// This event is fired whenever a client connects to this server.
    /// </summary>
    public class vxNetServerEventClientConnected : EventArgs
    {
        /// <summary>
        /// ID of the Client that has been added.
        /// </summary>
        public long ID;

        /// <summary>
        /// Constructor.
        /// </summary>
        public vxNetServerEventClientConnected(long id)
        {
            this.ID = id;
        }
    }


    /// <summary>
    /// This event is fired whenever a client connects to this server.
    /// </summary>
    public class vxNetServerEventPlayerJoined : EventArgs
    {
        /// <summary>
        /// ID of the Client that has been added.
        /// </summary>
        public vxNetmsgAddPlayer Player;

        /// <summary>
        /// Constructor.
        /// </summary>
        public vxNetServerEventPlayerJoined(vxNetmsgAddPlayer player)
        {
            this.Player = player;
        }
    }




    /// <summary>
    /// This event is fired whenever a client disconnects to this server.
    /// </summary>
    public class vxNetServerEventClientDisconnected : EventArgs
    {
        /// <summary>
        /// ID of the Client that has been added.
        /// </summary>
        public string ID;

        /// <summary>
        /// Constructor.
        /// </summary>
        public vxNetServerEventClientDisconnected(string id)
        {
            this.ID = id;
        }
    }


    /// <summary>
    /// This event is fired on the client side whenever a player needs to be updated with information from the server.
    /// </summary>
    public class vxNetServerEventPlayerStatusUpdate : EventArgs
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
        public vxNetServerEventPlayerStatusUpdate(vxNetPlayerInfo player)
        {
            m_playerToUpdate = player;
        }
    }

    /// <summary>
    /// This event is fired on the client side whenever a player needs to be updated with information from the server.
    /// </summary>
    public class vxNetServerEventPlayerStateUpdate : EventArgs
    {
        public string PlayerID
        {
            get { return m_playerId; }
        }
        private string m_playerId = string.Empty;

        /// <summary>
        /// The player that needs updating.
        /// </summary>
        public vxNetEntityState PlayerToUpdate
        {
            get { return m_playerToUpdate; }
        }
        vxNetEntityState m_playerToUpdate;

        /// <summary>
        /// Constructor.
        /// </summary>
        public vxNetServerEventPlayerStateUpdate(string playerId, vxNetEntityState entityState)
        {
            m_playerId = playerId;
            m_playerToUpdate = entityState;
        }
    }
}
