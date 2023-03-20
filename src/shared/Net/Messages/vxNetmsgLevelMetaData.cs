
using Microsoft.Xna.Framework.Graphics;
using System;

namespace VerticesEngine.Net.Messages
{
    /// <summary>
    /// What does this net level come from?
    /// </summary>
    public enum NetLevelType
    {
        /// <summary>
        /// The level belongs to the base game
        /// </summary>
        BaseGame = 0,

        /// <summary>
        /// The level belongs to a given DLC
        /// </summary>
        DLC = 1,

        /// <summary>
        /// The level is a custom player-made level
        /// </summary>
        Custom = 2
    }

    /// <summary>
    /// This message is used to communicate Level Meta Data, it must be inherited and handeled by the 
    /// running game to encode and decode it
    /// </summary>
    public abstract class vxNetmsgLevelMetaData : vxINetworkMessage
    {
        /// <summary>
        /// An ID For a given level
        /// </summary>
        public string id;

        /// <summary>
        /// A Name for a given level, this can eb useful to give some information while 
        /// </summary>
        public string Name;

        /// <summary>
        /// The Content path to the given level
        /// </summary>
        public string Path;


        /// <summary>
        /// Is this server a dedicated server?
        /// </summary>
        public bool IsDedicated;

        /// <summary>
        /// Where does this level live? In the base game? DLC? Custom?
        /// </summary>
        public NetLevelType NetLevelType = NetLevelType.BaseGame;

        public vxNetmsgLevelMetaData() {
            id = "1234";
            Name = "test-level";
            Path = "Somewhere";
            NetLevelType = NetLevelType.BaseGame;
            IsDedicated = vxNetworkManager.Config.IsDedicatedServer;
        }

        /// <summary>
        /// Creates a Level Meta Data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Name"></param>
        /// <param name="Path"></param>
        /// <param name="NetLevelType"></param>
        public vxNetmsgLevelMetaData(string id, string Name, string Path, NetLevelType NetLevelType)
        {
            this.id = id;
            this.Name = Name;
            this.Path = Path;
            this.NetLevelType = NetLevelType;
            IsDedicated = vxNetworkManager.Config.IsDedicatedServer;
        }

        /// <summary>
        /// Decoding Constructor to be used by client.
        /// </summary>
        /// <param name="im"></param>
        public vxNetmsgLevelMetaData(vxINetMessageIncoming im)
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
                return vxNetworkMessageTypes.LevelMetaData;
            }
        }

        public void EncodeMsg(vxINetMessageOutgoing om)
        {
            OnMsgEncoded(om);
        }

        protected virtual void OnMsgEncoded(vxINetMessageOutgoing om) {

            om.Write(id);
            om.Write(Name);
            om.Write(Path);
            om.Write(vxNetworkManager.Config.IsDedicatedServer);
            om.Write((int)NetLevelType);
        }

        public void DecodeMsg(vxINetMessageIncoming im)
        {
            OnMsgDencoded(im);
        }

        protected virtual void OnMsgDencoded(vxINetMessageIncoming im)
        {
            id = im.ReadString();
            Name = im.ReadString();
            Path = im.ReadString();
            IsDedicated = im.ReadBoolean();
            NetLevelType = (NetLevelType)im.ReadInt32();
        }
    }
}
