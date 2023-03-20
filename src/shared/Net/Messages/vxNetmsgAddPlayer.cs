using VerticesEngine.Profile;

namespace VerticesEngine.Net.Messages
{
    /// <summary>
    /// This message is sent when a player has connected to a server
    /// </summary>
    public struct vxNetmsgAddPlayer : vxINetworkMessage
    {

        /// <summary>
        /// The Server Name
        /// </summary>
        public vxNetPlayerInfo PlayerInfo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerinfo"></param>
        public vxNetmsgAddPlayer(vxNetPlayerInfo playerinfo)
        {
            this.PlayerInfo = playerinfo;
        }

        /// <summary>
        /// Decoding Constructor to be used by client.
        /// </summary>
        /// <param name="im"></param>
        public vxNetmsgAddPlayer(vxINetMessageIncoming im)
        {
            PlayerInfo = new vxNetPlayerInfo();
            this.DecodeMsg(im);
        }

        /// <summary>
        /// The Message Type
        /// </summary>
        public vxNetworkMessageTypes MessageType
        {
            get
            {
                return vxNetworkMessageTypes.PlayerConnected;
            }
        }

         public void DecodeMsg(vxINetMessageIncoming im)
        {
            PlayerInfo = new vxNetPlayerInfo(
                im.ReadString(),
                im.ReadString(),
                im.ReadInt32(),
                vxEnumNetPlayerStatus.InServerLobbyNotReady,
                (vxPlatformType)im.ReadByte(),
                im.ReadString());
        }

        public void EncodeMsg(vxINetMessageOutgoing om)
        {
            om.Write(this.PlayerInfo.ID);
            om.Write(this.PlayerInfo.UserName);
            //om.Write(this.PlayerInfo.Status.ToString());
            om.Write(PlayerInfo.PlayerIndex);
            om.Write((byte)(this.PlayerInfo.Platform));
            om.Write((this.PlayerInfo.PlatformPlayerID));
        }
    }
}
