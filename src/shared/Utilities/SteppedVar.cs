using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.Utilities
{
    public class SteppedVar<T>
    {
        public T Value;

        public T RequestedValue;

        public void Step()
        {
            //Value = vxMathHelper.Smooth(Value, RequestedValue, 4);
        }
    }
}
