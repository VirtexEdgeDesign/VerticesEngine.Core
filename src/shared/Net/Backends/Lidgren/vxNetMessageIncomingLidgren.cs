using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using VerticesEngine.Net.Messages;

namespace VerticesEngine.Net.BackendForLidgren
{
    /// <summary>
    /// Converts a Lidgren message to a generic Net Message
    /// </summary>
    internal struct vxNetMessageIncomingLidgren : vxINetMessageIncoming
    {
        /// <summary>
        /// The underlying lidgren net code
        /// </summary>
        internal NetIncomingMessage im;

        public vxNetMessageIncomingLidgren(NetIncomingMessage im)
        {
            this.im = im;            
        }

        public IPEndPoint SenderEndPoint => im.SenderEndPoint;

        public double ReadTime()
        {
            return im.ReadTime(true);
        }

        public void ReadAllFields(object target)
        {
            im.ReadAllFields(target);
        }

        public bool ReadBoolean()
        {
            return im.ReadBoolean();
        }

        public byte ReadByte()
        {
            return im.ReadByte();
        }

        public byte[] ReadByteArray(int length)
        {
            return im.ReadBytes(length);
        }

        public double ReadDouble()
        {            
            return im.ReadDouble();
        }

        public float ReadFloat()
        {
            return im.ReadFloat();
        }

        public short ReadInt16()
        {
            return im.ReadInt16();
        }

        public int ReadInt32()
        {
            return im.ReadInt32();
        }

        public long ReadInt64()
        {
            return im.ReadInt64();
        }

        public string ReadString()
        {
            return im.ReadString();
        }

        public Vector2 ReadVector2()
        {
            return im.ReadVector2();
        }

        public Vector3 ReadVector3()
        {
            return im.ReadVector3();
        }

        public Quaternion ReadQuaternion()
        {
            return im.ReadRotation(24);
        }
    }
}
