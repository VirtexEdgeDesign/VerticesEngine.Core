using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine
{
    /// <summary>
    /// Serialises an objects field or property to be saved in the sandbox
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class vxSerialiseAttribute : Attribute
    {
        public vxSerialiseAttribute()
        {

        }
    }

}
