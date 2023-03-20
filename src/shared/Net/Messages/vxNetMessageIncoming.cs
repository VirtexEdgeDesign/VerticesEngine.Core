using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace VerticesEngine.Net.Messages
{
    /// <summary>
    /// This is the bases for an incoming message. This can act as a wrapper for Steam, Lidgen or Android messages
    /// </summary>
    public interface vxINetMessageIncoming
    {
        // TODO: Update to not use Lidgren-specific do info here, maybe a GetSender() / GetSenderPort()
        IPEndPoint SenderEndPoint { get; } 

        void ReadAllFields(object target);

        byte ReadByte();

        double ReadTime();

        byte[] ReadByteArray(int length);

        string ReadString();

        short ReadInt16();

        int ReadInt32();

        long ReadInt64();

        double ReadDouble();

        float ReadFloat();

        bool ReadBoolean();

        Vector2 ReadVector2();

        Vector3 ReadVector3();

        Quaternion ReadQuaternion();
    }
}
