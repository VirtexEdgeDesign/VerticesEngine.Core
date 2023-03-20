
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace VerticesEngine.Net.Messages
{
    /// <summary>
    /// This message is used during the discovery phase to glean basic server information.
    /// </summary>
    public struct vxNetmsgUpdatePlayerEntityState : vxINetworkMessage
    {
        /// <summary>
        /// The player id to update
        /// </summary>
        public string PlayerID;

        /// <summary>
        /// The current Entity State
        /// </summary>
        public vxNetEntityState EntityState;

        /// <summary>
        /// The time at which the message was sent. This is to help with interpolating the actual position after lag.
        /// </summary>
        public double sentTime;

        /// <summary>
        /// The summation of all delta times while being sent over the wire. This is the summation of each delta that this message is relayed through.
        /// </summary>
        public double totalFlightTime;

        /// <summary>
        /// How many jumps have we done
        /// </summary>
        //public int relays;

        /// <summary>
        /// The initial constructor of this entity update method.
        /// </summary>
        /// <param name="PlayerState"></param>
        public vxNetmsgUpdatePlayerEntityState(vxNetPlayerInfo playerInfo)
        {
            PlayerID = playerInfo.ID;
            sentTime = 0;
            totalFlightTime = 0;
            //relays = 0;
            EntityState = playerInfo.EntityState;
        }

        /// <summary>
        /// The message is decoded by a recipient. This can be a server or client.
        /// </summary>
        /// <param name="im"></param>
        public vxNetmsgUpdatePlayerEntityState(vxINetMessageIncoming im)
        {
            PlayerID = string.Empty;
            //MessageTime = 0;
            sentTime = 0;
            totalFlightTime = 0;
            EntityState = new vxNetEntityState();
            //relays = 0;
            DecodeMsg(im);
        }

        /// <summary>
        /// The Message Type
        /// </summary>
        public vxNetworkMessageTypes MessageType
        {
            get
            {
                return vxNetworkMessageTypes.UpdatePlayerEntityState;
            }
        }

         public void DecodeMsg(vxINetMessageIncoming im)
        {
            //player ID
            sentTime = im.ReadTime();
            PlayerID = im.ReadString();

            // each time we decode this message it means we've arrived at a point. This might get relayed though (like a server relaying an original client
            // message to all other clients, so we should sum up the delta time for each relay to get the total flight time by the end.
            totalFlightTime = im.ReadDouble() + (NetTime.Now - sentTime);

            //get the current physical orientations and locations
            EntityState.Position = new Vector3(im.ReadFloat(), im.ReadFloat(), im.ReadFloat());
            EntityState.Velocity = new Vector3(im.ReadFloat(), im.ReadFloat(), im.ReadFloat());

            EntityState.Orientation = im.ReadQuaternion();

            //What is the current control status
            EntityState.IsLeftDown = im.ReadBoolean();
            EntityState.IsRightDown = im.ReadBoolean();
            EntityState.IsThrustDown = im.ReadBoolean();

            //how much should these controls be applied
            EntityState.ThrustAmount = im.ReadFloat();
            EntityState.TurnAmount = im.ReadFloat();

            //relays = im.ReadInt32() + 1;
        }

        public void EncodeMsg(vxINetMessageOutgoing om)
        {
            //player ID
            om.WriteTime();
            om.Write(PlayerID);
            om.Write(totalFlightTime);

            //get the current physical orientations and locations
            om.Write(EntityState.Position.X);
            om.Write(EntityState.Position.Y);
            om.Write(EntityState.Position.Z);

            om.Write(EntityState.Velocity.X);
            om.Write(EntityState.Velocity.Y);
            om.Write(EntityState.Velocity.Z);

            om.Write(EntityState.Orientation);

            //What is the current control status
            om.Write(EntityState.IsLeftDown);
            om.Write(EntityState.IsRightDown);
            om.Write(EntityState.IsThrustDown);

            //how much should these controls be applied
            om.Write(EntityState.ThrustAmount);
            om.Write(EntityState.TurnAmount);

            //om.Write(relays);
        }
    }
}
