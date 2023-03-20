using System;
using VerticesEngine.Net.Messages;

namespace VerticesEngine.Net
{
    /// <summary>
    /// Provides an API interface for a common network interface to a specific backend implmenetation (such as SteamWorks API, Android API, Lidgren etc...)
    /// </summary>
    public interface vxINetworkClientBackend : IDisposable
    {
        /// <summary>
        /// Initialises The Network Manager
        /// </summary>
        void Initialise();

        /// <summary>
        /// Sends a discovery signal to find available servers. Note not all implementations need to specifiy a port
        /// </summary>
        /// <param name="port"></param>
        void SendLocalDiscoverySignal(int port);

        /// <summary>
        /// Sends a discovery signal to find available servers. Note not all implementations need to specifiy a port
        /// </summary>
        /// <param name="port"></param>
        void SendDiscoverySignal(string ip, int port);


        /// <summary>
        /// Connect to the specified address and port with a given hail message
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="Port"></param>
        void Connect(string Address, int Port);

        /// <summary>
        /// Connect to the specified address and port with a given hail message
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="Port"></param>
        /// <param name="hail"></param>
        void Connect(string Address, int Port, vxINetMessageOutgoing hail);

        /// <summary>
        /// The create message.
        /// </summary>
        /// <returns>
        /// </returns>
        vxINetMessageOutgoing CreateMessage();

        /// <summary>
        /// Returns the current Net Time for this implmentation
        /// </summary>
        /// <returns></returns>
        double GetCurrentNetTime();


        /// <summary>
        /// The disconnect.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// The read message.
        /// </summary>
        /// <returns>
        /// </returns>
        vxINetMessageIncoming ReadMessage();

        /// <summary>
        /// The recycle.
        /// </summary>
        /// <param name="im">
        /// The im.
        /// </param>
        void Recycle(vxINetMessageIncoming im);

        /// <summary>
        /// The send message.
        /// </summary>
        /// <param name="gameMessage">
        /// The game message.
        /// </param>
        void SendMessage(vxINetworkMessage gameMessage);

        void DebugDraw();
    }
}