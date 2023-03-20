using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.Net
{
    public partial class vxNetBuffer
    {
        public byte[] Data;

        public vxNetBuffer(int bufferSize)
        {
            Data = new byte[bufferSize];
        }
    }
}
