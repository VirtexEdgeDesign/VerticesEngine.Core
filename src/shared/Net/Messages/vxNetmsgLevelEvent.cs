
using Microsoft.Xna.Framework.Graphics;

namespace VerticesEngine.Net.Messages
{
    /// <summary>
    /// Message which specifies level event info such as when an item is spawned in the level. This requires the game to handle the implementation
    /// </summary>
    public abstract class vxNetmsgLevelEvent : vxINetworkMessage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerinfo"></param>
        public vxNetmsgLevelEvent()
        {

        }

        /// <summary>
        /// Decoding Constructor to be used by client.
        /// </summary>
        /// <param name="im"></param>
        public vxNetmsgLevelEvent(vxINetMessageIncoming im)
        {
            this.DecodeMsg(im);
        }

        /// <summary>
        /// The Message Type
        /// </summary>
        public vxNetworkMessageTypes MessageType
        {
            get
            {
                return vxNetworkMessageTypes.LevelEvent;
            }
        }

        public void EncodeMsg(vxINetMessageOutgoing om)
        {
            OnMsgEncoded(om);
        }

        protected abstract void OnMsgEncoded(vxINetMessageOutgoing om);

        public void DecodeMsg(vxINetMessageIncoming im)
        {
            OnMsgDencoded(im);
        }

        protected abstract void OnMsgDencoded(vxINetMessageIncoming im);
    }
}
