using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Profile;

namespace VerticesEngine.Net.Messages
{
    /// <summary>
    /// This message is used during the discovery phase to glean basic server information.
    /// </summary>
    public struct vxNetmsgUpdatePlayerLobbyStatus : vxINetworkMessage
    {

        /// <summary>
        /// The Server Name
        /// </summary>
        public vxNetPlayerInfo PlayerInfo;

        /// <summary>
        /// Initialization Constructor to be used on Server Side.
        /// </summary>
        /// <param name="ServerName"></param>
        /// <param name="ServerIP"></param>
        /// <param name="ServerPort"></param>
        public vxNetmsgUpdatePlayerLobbyStatus(vxNetPlayerInfo playerinfo)
        {
            this.PlayerInfo = playerinfo;
        }

        /// <summary>
        /// Decoding Constructor to be used by client.
        /// </summary>
        /// <param name="im"></param>
        public vxNetmsgUpdatePlayerLobbyStatus(vxINetMessageIncoming im)
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
                return vxNetworkMessageTypes.UpdatePlayerLobbyStatus;
            }
        }

        public void DecodeMsg(vxINetMessageIncoming im)
        {
            PlayerInfo = new vxNetPlayerInfo(
                    im.ReadString(),
                im.ReadString(),
                im.ReadInt32(),
                (vxEnumNetPlayerStatus)im.ReadByte(),
                (vxPlatformType)im.ReadByte(),
                im.ReadString());
        }

        public void EncodeMsg(vxINetMessageOutgoing om)
        {
            om.Write(this.PlayerInfo.ID);
            om.Write(this.PlayerInfo.UserName);
            om.Write(PlayerInfo.PlayerIndex);
            om.Write((byte)PlayerInfo.Status);
            om.Write((byte)PlayerInfo.Platform);
            om.Write(PlayerInfo.PlatformPlayerID);
        }
    }
}
