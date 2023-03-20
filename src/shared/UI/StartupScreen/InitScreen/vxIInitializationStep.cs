using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine
{
    /// <summary>
    /// A initialization step 
    /// </summary>
    public interface vxIInitializationStep
    {
        bool IsComplete { get; }

        string Status { get; }

        void Start();

        void Update();
    }
}
