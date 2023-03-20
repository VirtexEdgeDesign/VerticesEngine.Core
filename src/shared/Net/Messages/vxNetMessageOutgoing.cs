using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.Net.Messages
{
    public interface vxINetMessageOutgoing
    {
        void WriteTime();
        void WriteAllFields(object source);
        void Write(byte source);
        void Write(byte[] bytes);
        void Write(string source);
        void Write(int source);
        void Write(double source);
        void Write(float source);
        void Write(long source);
        void Write(Vector2 vector2);
        void Write(Vector3 vector3);
        void Write(Quaternion vector3);
        void Write(bool boolean);
    }
}
