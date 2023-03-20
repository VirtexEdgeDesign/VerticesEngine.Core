﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.Particles
{
    /// <summary>
    /// This registers a class as a particle type for use in Vertice. You specify the Name and Pool size and the engine 
    /// sets up the rest. To spawn them you only need to call <see cref="vxParticleSystem.Instance.Spawn(emitter);"/>
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
    public class vxRegisterAsParticleSystemAttribute : Attribute
    {
        /// <summary>
        /// Entity Name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The pool size for this particle type
        /// </summary>
        public int PoolSize { get; private set; }

        public vxRegisterAsParticleSystemAttribute(string Name, int PoolSize)
        {
            this.Name = Name;
            this.PoolSize = PoolSize;
        }
    }
}
