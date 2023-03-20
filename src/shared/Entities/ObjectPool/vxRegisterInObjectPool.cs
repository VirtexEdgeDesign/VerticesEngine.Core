using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.Entities
{
    /// <summary>
    /// This registers a class as a particle type for use in Vertice. You specify the Name and Pool size and the engine 
    /// sets up the rest. To spawn them you only need to call <see cref="vxParticleSystem.Instance.Spawn(emitter);"/>
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
    public class vxRegisterObjectPoolAttribute : Attribute
    {
        /// <summary>
        /// The pool size for this particle type
        /// </summary>
        public int PoolSize { get; private set; }

        public vxRegisterObjectPoolAttribute(int PoolSize)
        {
            this.PoolSize = PoolSize;
        }
    }
}
