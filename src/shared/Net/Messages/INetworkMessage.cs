namespace VerticesEngine.Net.Messages
{
    /// <summary>
    /// The Network Message Intergace for exchanging data between
    /// server and clients.
    /// </summary>
    public interface vxINetworkMessage
    {
        /// <summary>
        /// Gets MessageType.
        /// </summary>
        vxNetworkMessageTypes MessageType { get; }

        /// <summary>
        /// Decodes the Incoming Message
        /// </summary>
        /// <param name="im"></param>
        void DecodeMsg(vxINetMessageIncoming im);

        /// <summary>
        /// Encode's the Network Message to be sent out.
        /// </summary>
        /// <param name="om"></param>
        void EncodeMsg(vxINetMessageOutgoing om);
    }
}