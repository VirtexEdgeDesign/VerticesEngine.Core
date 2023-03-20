using System;
using VerticesEngine.UI;
using VerticesEngine.UI.MessageBoxs;

namespace VerticesEngine.Net.UI
{
    /// <summary>
    /// The sever creation busy dialog. 
    /// </summary>
    internal class vxMultiplayerLANCreateServerBusyDialog : vxMultiplayerBaseCreateServerBusyDialog
    {
        protected override vxNetworkBackend NetworkBackend => vxNetworkBackend.CrossPlatform;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.Net.UI.vxMultiplayerServerConnectBusyDialog"/> class.
        /// </summary>
        public vxMultiplayerLANCreateServerBusyDialog(string serverName) : base(serverName)
        {

        }
    }
}
