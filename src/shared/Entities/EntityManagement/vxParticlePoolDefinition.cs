using System;

namespace VerticesEngine.Particles
{
    public class vxParticlePoolDefinition
    {
        public Type Type { get; private set; }

        public string Name { get; private set; }

        public int PoolSize { get; private set; }

        public virtual string Key { get; private set; }

        public vxParticlePoolDefinition(Type type, string name, string key, int PoolSize)
        {
            this.Type = type;
            this.Name = name;
            this.Key = key;
            this.PoolSize = PoolSize;
        }
    }
}
