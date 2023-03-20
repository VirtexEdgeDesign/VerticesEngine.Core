namespace VerticesEngine.Net.Messages
{
    /// <summary>
    /// This message is used during the discovery phase to glean basic server information.
    /// </summary>
    public struct vxNetmsgRemovePlayer : vxINetworkMessage
    {

        /// <summary>
        /// The Server Name
        /// </summary>
        public vxNetPlayerInfo PlayerInfo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerinfo"></param>
        public vxNetmsgRemovePlayer(vxNetPlayerInfo playerinfo)
        {
            this.PlayerInfo = playerinfo;
        }

        /// <summary>
        /// Decoding Constructor to be used by client.
        /// </summary>
        /// <param name="im"></param>
        public vxNetmsgRemovePlayer(vxINetMessageIncoming im)
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
                return vxNetworkMessageTypes.PlayerDisconnected;
            }
        }

        public void DecodeMsg(vxINetMessageIncoming im)
        {
            PlayerInfo = new vxNetPlayerInfo(
                im.ReadString(),
                im.ReadString(),
                -1,
                vxEnumNetPlayerStatus.None);
            string dummy = im.ReadString();
        }

        public void EncodeMsg(vxINetMessageOutgoing om)
        {
            om.Write(this.PlayerInfo.ID);
            om.Write(this.PlayerInfo.UserName);
            om.Write(this.PlayerInfo.Status.ToString());
        }
    }
}
