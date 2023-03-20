
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace VerticesEngine.Net.Messages
{
    /// <summary>
    /// This message is used during the discovery phase to glean basic server information.
    /// </summary>
    public struct vxNetmsgServerShutdown : vxINetworkMessage
    {
        public string reason;

        /// <summary>
        /// Initialization Constructor to be used on Server Side.
        /// </summary>
        /// <param name="ServerName"></param>
        /// <param name="ServerIP"></param>
        /// <param name="ServerPort"></param>
        public vxNetmsgServerShutdown(string reason)
        {
            this.reason = reason;
        }

        /// <summary>
        /// Decoding Constructor to be used by client.
        /// </summary>
        /// <param name="im"></param>
        public vxNetmsgServerShutdown(vxINetMessageIncoming im)
        {
            this.reason = "temp reason";
            this.DecodeMsg(im);
        }

        /// <summary>
        /// The Message Type
        /// </summary>
        public vxNetworkMessageTypes MessageType
        {
            get
            {
                return vxNetworkMessageTypes.ServerShutdown;
            }
        }

        public void DecodeMsg(vxINetMessageIncoming im)
        {
            reason = im.ReadString();
        }

        public void EncodeMsg(vxINetMessageOutgoing om)
        {
            om.Write(reason);
        }
    }
}
