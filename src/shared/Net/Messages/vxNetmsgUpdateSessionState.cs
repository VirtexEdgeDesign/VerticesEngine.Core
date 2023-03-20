namespace VerticesEngine.Net.Messages
{

    /// <summary>
    /// This message is used during the discovery phase to glean basic server information.
    /// </summary>
    public struct vxNetmsgUpdateSessionState : vxINetworkMessage
    {
        /// <summary>
        /// What is the current status of the game
        /// </summary>
        public vxEnumNetSessionState SessionState;

        /// <summary>
        /// The amount of time until the game will start. This is only filled in when SessionStatus is set to 'Ready'.
        /// </summary>
        public double MessageTime;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerinfo"></param>
        public vxNetmsgUpdateSessionState(vxEnumNetSessionState status)
        {
            SessionState = status;
            MessageTime = vxNetworkManager.GetCurrentNetTime();
        }

        /// <summary>
        /// Decoding Constructor to be used by client.
        /// </summary>
        /// <param name="im"></param>
        public vxNetmsgUpdateSessionState(vxINetMessageIncoming im)
        {
            SessionState = vxEnumNetSessionState.UNKNOWN;
            MessageTime = double.MaxValue;
            DecodeMsg(im);
        }

        /// <summary>
        /// The Message Type
        /// </summary>
        public vxNetworkMessageTypes MessageType
        {
            get
            {
                return vxNetworkMessageTypes.SessionStateChanged;
            }
        }

        public void DecodeMsg(vxINetMessageIncoming im)
        {
            SessionState = (vxEnumNetSessionState)im.ReadInt32();
            MessageTime = im.ReadDouble();
        }

        public void EncodeMsg(vxINetMessageOutgoing om)
        {
            om.Write((int)SessionState);
            om.Write(MessageTime);
        }
    }
}
