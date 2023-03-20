using Lidgren.Network;
using Microsoft.Xna.Framework;
using VerticesEngine.Net.Messages;

namespace VerticesEngine.Net.BackendForLidgren
{
    internal struct vxNetMessageOutgoingLidgren : vxINetMessageOutgoing
    {
        /// <summary>
        /// The underlying Lidren Message which handles byte data writing
        /// </summary>
        internal NetOutgoingMessage dataWriter;

        public vxNetMessageOutgoingLidgren(NetOutgoingMessage om)
        {
            this.dataWriter = om;
        }

        public void WriteTime()
        {
            dataWriter.WriteTime(true);
        }

        public void Write(byte source)
        {
            dataWriter.Write(source);
        }
        public void Write(byte[] bytes)
        {
            dataWriter.Write(bytes);
        }

        public void Write(string source)
        {
            dataWriter.Write(source);
        }
        public void Write(int source)
        {
            dataWriter.Write(source);
        }

        public void Write(double source)
        {
            dataWriter.Write(source);
        }

        public void Write(float source)
        {
            dataWriter.Write(source);
        }

        public void Write(long source)
        {
            dataWriter.Write(source);
        }
        public void Write(bool source)
        {
            dataWriter.Write(source);
        }

        public void Write(Vector2 source)
        {
            dataWriter.Write(source);
        }

        public void Write(Vector3 source)
        {
            dataWriter.Write(source);
        }

        public void Write(Quaternion source)
        {
            dataWriter.WriteRotation(source, 24);
        }

        public void WriteAllFields(object source)
        {
            dataWriter.WriteAllFields(source);
        }
    }
}
