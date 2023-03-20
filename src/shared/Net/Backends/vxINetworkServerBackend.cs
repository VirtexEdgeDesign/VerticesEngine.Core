using System;
using VerticesEngine.Net.Messages;

namespace VerticesEngine.Net
{
    /// <summary>
    /// Provides an API interface for a common network interface to a specific backend implmenetation (such as SteamWorks API, Android API, Lidgren etc...)
    /// </summary>
    public interface vxINetworkServerBackend : IDisposable
    {
        string ServerName { get; }
        int Port { get; }
        bool IsAcceptingIncomingConnections { get; set; }

        /// <summary>
        /// Initialises The Network Manager
        /// </summary>
        void Initialise();

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
        /// Starts the server
        /// </summary>
        void Start();

        /// <summary>
        /// Shutsdown the server
        /// </summary>
        void Shutdown();

        /// <summary>
        /// The read message.
        /// </summary>
        vxINetMessageIncoming ReadMessage();

        /// <summary>
        /// The recycle.
        /// </summary>
        /// <param name="im">
        /// The im.
        /// </param>
        //void Recycle(vxINetMessageIncoming im);

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