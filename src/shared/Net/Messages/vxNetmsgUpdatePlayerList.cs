
using System;
using System.Collections.Generic;

namespace VerticesEngine.Net.Messages
{
    /// <summary>
    /// This message is used during the discovery phase to glean basic server information.
    /// </summary>
    public struct vxNetmsgUpdatePlayerList : vxINetworkMessage
    {

        /// <summary>
        /// The Server Name
        /// </summary>
        public List<vxNetPlayerInfo> Players;


        /// <summary>
        /// The Message Type
        /// </summary>
        public vxNetworkMessageTypes MessageType
        {
            get
            {
                return vxNetworkMessageTypes.UpdatePlayersList;
            }
        }

        /// <summary>
        /// Creates a new player list. This message is only dispatched by the server
        /// </summary>
        /// <param name="playerManager"></param>
        public vxNetmsgUpdatePlayerList(vxNetPlayerManager playerManager)
        {
            Players = new List<vxNetPlayerInfo>();

            //Translate the Dictionary into A list of players.
            foreach (KeyValuePair<string, vxNetPlayerInfo> entry in playerManager.Players)
            {
                Players.Add(entry.Value);
            }
        }
        /// <summary>
        /// Decoding Constructor to be used by client.
        /// </summary>
        /// <param name="im"></param>
        public vxNetmsgUpdatePlayerList(vxINetMessageIncoming im)
        {
            Players = new List<vxNetPlayerInfo>();
            this.DecodeMsg(im);
        }

        public void DecodeMsg(vxINetMessageIncoming im)
        {
            int Count = im.ReadInt32();

            for (int i = 0; i < Count; i++)
            {
                Players.Add(new vxNetPlayerInfo(
                    im.ReadString(),
                im.ReadString(),
                im.ReadInt32(),
                (vxEnumNetPlayerStatus)im.ReadByte(),
                (vxPlatformType)im.ReadByte(),
                im.ReadString()));
            }
        }

        public void EncodeMsg(vxINetMessageOutgoing om)
        {
            //First Write the number of elements
            om.Write(Players.Count);

            for(int p = 0; p < Players.Count; p++)
            {
                var plyr = Players[p];
                om.Write(plyr.ID);
                om.Write(plyr.UserName);
                om.Write(p); // we can encode this here bc this is only encoded and sent by a server
                om.Write((byte)plyr.Status);
                om.Write((byte)plyr.Platform);
                om.Write(plyr.PlatformPlayerID);
            }
        }
    }
}
