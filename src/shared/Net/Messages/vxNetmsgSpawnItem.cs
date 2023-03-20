
using Microsoft.Xna.Framework.Graphics;

namespace VerticesEngine.Net.Messages
{
    /// <summary>
    /// Called when an item is spawned in the level. This requires the game to handle the logic of this
    /// </summary>
    public abstract class vxNetmsgSpawnItem : vxINetworkMessage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerinfo"></param>
        public vxNetmsgSpawnItem()
        {

        }

        /// <summary>
        /// Decoding Constructor to be used by client.
        /// </summary>
        /// <param name="im"></param>
        public vxNetmsgSpawnItem(vxINetMessageIncoming im)
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
                return vxNetworkMessageTypes.SpawnItem;
            }
        }

        public void EncodeMsg(vxINetMessageOutgoing om)
        {
            OnMsgEncoded(om);
        }

        protected virtual void OnMsgEncoded(vxINetMessageOutgoing om) { }

        public void DecodeMsg(vxINetMessageIncoming im)
        {
            OnMsgDencoded(im);
        }

        protected virtual void OnMsgDencoded(vxINetMessageIncoming im) { }
    }
}
