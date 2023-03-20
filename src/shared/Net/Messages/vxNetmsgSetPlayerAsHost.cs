namespace VerticesEngine.Net.Messages
{
    /// <summary>
    /// This message is sent when a player is selected as the host by the server
    /// </summary>
    public struct vxNetmsgSetPlayerAsHost : vxINetworkMessage
    {

        /// <summary>
        /// The Server Name
        /// </summary>
        public string HostID;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerinfo"></param>
        public vxNetmsgSetPlayerAsHost(vxNetPlayerInfo playerinfo)
        {
            this.HostID = playerinfo.ID;
        }

        /// <summary>
        /// Decoding Constructor to be used by client.
        /// </summary>
        /// <param name="im"></param>
        public vxNetmsgSetPlayerAsHost(vxINetMessageIncoming im)
        {
            HostID = string.Empty;
            this.DecodeMsg(im);
        }

        /// <summary>
        /// The Message Type
        /// </summary>
        public vxNetworkMessageTypes MessageType
        {
            get
            {
                return vxNetworkMessageTypes.SetPlayerAsHost;
            }
        }

         public void DecodeMsg(vxINetMessageIncoming im)
        {
            HostID = im.ReadString();
        }

        public void EncodeMsg(vxINetMessageOutgoing om)
        {
            om.Write(HostID);
        }
    }
}
