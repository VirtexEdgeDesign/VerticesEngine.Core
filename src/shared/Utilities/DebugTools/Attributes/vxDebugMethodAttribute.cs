using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.Diagnostics
{
    /// <summary>
    /// Tag a static method with this attribute for it to be exposed by the in game console.
    /// Note the parameters are method(), method(string[] args), method(vxEngine Engine) & method(vxEngine Engine, string[] args)
    /// </summary>
    public class vxDebugMethodAttribute : Attribute
    {
        public string cmd { get; private set; }
        public string description { get; private set; }

        public vxDebugMethodAttribute(string cmd, string description)
        {
            this.cmd = cmd;
            this.description = description;
        }
    }
}
