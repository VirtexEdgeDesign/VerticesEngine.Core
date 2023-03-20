using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VerticesEngine;
using VerticesEngine.Graphics;

namespace VerticesEngine.Particles
{

    /// <summary>
    /// A particle set which holds all of the particle pool.
    /// </summary>
    public class vxParticlePool : IDisposable 
    {

        public List<vxIParticle> Pool;// = new List<vxIParticle>();

        public string Key;

        /// <summary>
        /// The size of this particle pool
        /// </summary>
        public int Size
        {
            get { return Pool.Capacity; }
        }

        //public vxEnumParticleLayer ParticleLayer = vxEnumParticleLayer.Behind;


        //public vxParticlePool(int size) 
        //{
        //    this.Key = typeof(T).Name;
        //}

        /// <summary>
        /// A particle set which holds all of the particle pool.
        /// </summary>
        public vxParticlePool(Type type, int capacity)
        {
            this.Key = type.Name.ToString();
            Pool = new List<vxIParticle>(capacity);
        }

        public void Update()
        {
            for (int i = 0; i < Pool.Count; i++)
            {
                if (Pool[i].IsAlive)
                    Pool[i].UpdateParticle();
            }
        }

        int initIndex = 0;

        /// <summary>
        /// Inits a new particle in the particle pool.
        /// </summary>
        /// <param name="key">Key.</param>
        internal virtual vxIParticle SpawnParticle(vxGameObject emitter)
        {
            initIndex = (initIndex + 1) % Pool.Count;
            return Pool[initIndex].Spawn(emitter);
        }



        /// <summary>
        /// Draws the active particles in this pool
        /// </summary>
        public void DrawActivePartciles(vxCamera camera, string renderPass)
        {
            for (int p = Pool.Count - 1; p >= 0; p--)
            {
                if (Pool[p].IsAlive == true)
                {
                    ((vxEntity)Pool[p]).EntityRenderer.Draw(camera, renderPass);
                }
            }

        }

        public void Dispose()
        {
            foreach (IDisposable particle in Pool)
            {
                particle.Dispose();
            }
            Pool.Clear();
        }
    }
}
