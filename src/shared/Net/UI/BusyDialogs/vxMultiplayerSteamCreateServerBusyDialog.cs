using System;
using VerticesEngine.UI;
using VerticesEngine.UI.MessageBoxs;

namespace VerticesEngine.Net.UI
{
    /// <summary>
    /// The sever creation busy dialog for creating Steam P2P Sessions
    /// </summary>
    internal class vxMultiplayerSteamCreateServerBusyDialog : vxMultiplayerBaseCreateServerBusyDialog
    {

        protected override vxNetworkBackend NetworkBackend => vxNetworkBackend.SteamP2P;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.Net.UI.vxMultiplayerSteamCreateServerBusyDialog"/> class.
        /// </summary>
        public vxMultiplayerSteamCreateServerBusyDialog(string serverName) : base(serverName)
        {

        }
    }
}
