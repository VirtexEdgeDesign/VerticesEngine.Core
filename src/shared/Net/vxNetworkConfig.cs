using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using VerticesEngine.Net.Messages;

namespace VerticesEngine.Net
{
    /// <summary>
    /// A network config interface which allows per-game customization and configuration of the networking code
    /// </summary>
    public interface vxINetworkConfig
    {
        /// <summary>
        /// The game name for this config
        /// </summary>
        string GameName { get; }
        
        /// <summary>
        /// The App Id used to identify this network backend
        /// </summary>
        string AppId { get; }
        
        /// <summary>
        /// The initial port to look for when searching for servers on a host
        /// </summary>
        int ServerLANDefaultPort { get; }

        /// <summary>
        /// The server port range to search when looking for servers
        /// </summary>
        int ServerLANPortRange { get; }

        /// <summary>
        /// The minimum number of players before a level/session can begin
        /// </summary>
        int MinNumberOfPlayers { get; }

        /// <summary>
        /// The maximum number of players per server.
        /// </summary>
        int MaxNumberOfPlayers { get; }

        /// <summary>
        /// The net ui platform for the lidgren networking library. This allows us to swap between different behaviour such as windows vs linux,
        /// This essentially toggles functionality originally split up using the __CONSTRAINED__ compile flag. It allows of a single binary.
        /// </summary>
        NetUtiPlatform LidgrenNetUIPlatform { get; }

        /// <summary>
        /// Are we a dedicated server that's running?
        /// </summary>
        bool IsDedicatedServer { get; }

        /// <summary>
        /// The URL to send logs and events to
        /// </summary>
        string EventURL { get; }

        string AppGuid { get; }

        vxNetmsgPlayerMetaData GetPlayerMetaData(vxINetMessageIncoming im);

        vxNetmsgLevelMetaData GetLevelMetaData(vxINetMessageIncoming im);

        vxNetmsgLevelEvent GetLevelEventMessage(vxINetMessageIncoming im);
    }
}
