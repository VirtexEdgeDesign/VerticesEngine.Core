using System;
using VerticesEngine.Net.Messages;

namespace VerticesEngine.Net
{
    public interface INetworkSessionManager
    {

        /// <summary>
        /// Initialises The Network Manager
        /// </summary>
        void Initialise();

        /// <summary>
        /// The disconnect.
        /// </summary>
        void Disconnect();

        void Start(vxNetworkBackend networkBackend);

        /// <summary>
        /// Reset this instance.
        /// </summary>
        void Reset();

        void SendMessage(vxINetworkMessage gameMessage);

        void StartQuickGame();

        void InviteFriend();

        void OnPlayerInvite();

        void OnPlayerJoined();

        void OnPlayerDisconnect();

        void OnAllPlayersReady();

        void OnMessageReceived(object msg);

    }
}
