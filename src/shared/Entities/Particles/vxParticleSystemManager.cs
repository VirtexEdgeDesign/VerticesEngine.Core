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
    /// A scene's particle system which holds all particle pools
    /// </summary>
    public class vxParticleSystemManager : vxGameObject
    {
        /// <summary>
        /// Particle Pools
        /// </summary>
        internal Dictionary<string, vxParticlePool> ParticlePools = new Dictionary<string, vxParticlePool>();

        public vxParticleSystemManager()
        {
			
        }

        /// <summary>
        /// Add's a particle pool to this system
        /// </summary>
        /// <param name="particlePool"></param>
        public void AddPool(vxParticlePool particlePool)
        {
            ParticlePools.Add(particlePool.Key.ToString(), particlePool);
        }

        /// <summary>
        /// Initialises' a new particle with the givien particle pool key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="emitter"></param>
        public virtual vxIParticle SpawnParticle(object key, vxGameObject emitter)
        {
            if (ParticlePools.ContainsKey(key.ToString()))
                return ParticlePools[key.ToString()].SpawnParticle(emitter);
            else
                return null;
        }

        protected override void OnDisposed()
        {
            foreach (var pool in ParticlePools)
                pool.Value.Dispose();

            ParticlePools.Clear();

            base.OnDisposed();
        }

        public void Update()
        {
            foreach (var poolKeyPair in ParticlePools)
                poolKeyPair.Value.Update();
        }

		/// <summary>
		/// Draws the particles for the specified render pass
		/// </summary>
        public void DrawParticles(vxCamera camera, string renderPass)
        {
            foreach (var poolKeyPair in ParticlePools)
                poolKeyPair.Value.DrawActivePartciles(camera, renderPass);
        }
    }
}