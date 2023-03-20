namespace VerticesEngine.Net.UI
{

    /// <summary>
    /// The sever connection busy dialog. This is used for connecting to a multiplayer server.
    /// </summary>
    internal class vxMultiplayerLANConnectBusyDialog : vxMultiplayerBaseConnectBusyDialog
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.Net.UI.vxMultiplayerLANConnectBusyDialog"/> class.
        /// </summary>
        public vxMultiplayerLANConnectBusyDialog(string serverName, string ipAddress, int port)
            : base(serverName, ipAddress, port)
        {

        }
    }
}
