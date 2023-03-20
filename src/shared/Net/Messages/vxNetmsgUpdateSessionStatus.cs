namespace VerticesEngine.Net.Messages
{

    /// <summary>
    /// This message is used during the discovery phase to glean basic server information.
    /// </summary>
    public struct vxNetmsgUpdateSessionStatus : vxINetworkMessage
    {
        /// <summary>
        /// What is the current status of the game
        /// </summary>
        public vxEnumNetSessionState SessionStatus;

        /// <summary>
        /// The amount of time until the game will start. This is only filled in when SessionStatus is set to 'Ready'.
        /// </summary>
        public float StartTime;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerinfo"></param>
        public vxNetmsgUpdateSessionStatus(vxEnumNetSessionState status, float startTime)
        {
            SessionStatus = status;
            StartTime = startTime;
        }
        
        /// <summary>
        /// Decoding Constructor to be used by client.
        /// </summary>
        /// <param name="im"></param>
        public vxNetmsgUpdateSessionStatus(vxINetMessageIncoming im)
        {
            SessionStatus = vxEnumNetSessionState.WaitingForPlayers;
            StartTime = float.MaxValue;
            DecodeMsg(im);
        }

        /// <summary>
        /// The Message Type
        /// </summary>
        public vxNetworkMessageTypes MessageType
        {
            get
            {
                return vxNetworkMessageTypes.SessionStatus;
            }
        }

        public void DecodeMsg(vxINetMessageIncoming im)
        {
            SessionStatus = (vxEnumNetSessionState)im.ReadInt32();
            StartTime = im.ReadFloat();
        }

        public void EncodeMsg(vxINetMessageOutgoing om)
        {
            om.Write((int)SessionStatus);
            om.Write(StartTime);
        }
    }
}
