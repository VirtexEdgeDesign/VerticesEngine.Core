
using Microsoft.Xna.Framework.Graphics;
using System;

namespace VerticesEngine.Net.Messages
{
    /// <summary>
    /// This message is used to communicate Player Meta Data, it must be inherited and handeled by the 
    /// running game to encode and decode it
    /// </summary>
    public abstract class vxNetmsgPlayerMetaData : vxINetworkMessage
    {
        /// <summary>
        /// The players id
        /// </summary>
        public string id;

        /// <summary>
        /// The players name
        /// </summary>
        public string PlayerName;
        public vxNetmsgPlayerMetaData()
        {
            id = "1234";
            PlayerName = "test mname";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerinfo"></param>
        public vxNetmsgPlayerMetaData(string id, string name)
        {
            this.id = id;
            PlayerName = name;
        }

        /// <summary>
        /// Decoding Constructor to be used by client.
        /// </summary>
        /// <param name="im"></param>
        public vxNetmsgPlayerMetaData(vxINetMessageIncoming im)
        {
            this.DecodeMsg(im);
            Console.WriteLine("vxINetMessageIncoming " + id);
        }

        /// <summary>
        /// The Message Type
        /// </summary>
        public vxNetworkMessageTypes MessageType
        {
            get
            {
                return vxNetworkMessageTypes.PlayerMetaData;
            }
        }

        public void EncodeMsg(vxINetMessageOutgoing om)
        {
            OnMsgEncoded(om);
        }

        protected virtual void OnMsgEncoded(vxINetMessageOutgoing om)
        {
            om.Write(id);
            om.Write(PlayerName);
        }

        public void DecodeMsg(vxINetMessageIncoming im)
        {
            OnMsgDencoded(im);
        }

        protected virtual void OnMsgDencoded(vxINetMessageIncoming im)
        {
            id = im.ReadString();
            PlayerName = im.ReadString();
        }
    }
}
