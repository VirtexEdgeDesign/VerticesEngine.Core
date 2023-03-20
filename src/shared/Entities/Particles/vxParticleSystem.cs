/**
 * @file
 * @author rtroe (C) Virtex Edge Design
 * @brief The particle system instance
 *
 */

using System;

namespace VerticesEngine.Particles
{
    /// <summary>
    /// The particle system instance
    /// </summary>
    public class vxParticleSystem
    {
        /// <summary>
        /// The particle system singleton
        /// </summary>
        public static vxParticleSystem Instance
        {
            get { return _instance; }
        }
        private static vxParticleSystem _instance = new vxParticleSystem();


        /// <summary>
        /// Creates a pool of particle type 'T' of size 'n'
        /// </summary>
        /// <example> 
        /// How to use the Particle System Create Pool method. This should eb used in the 'LoadParticlePool()' method in your Scene.
        /// <code>
        /// 
        /// // Create a pool of '' with 100 elements in it
        /// vxParticleSystem.Instance.CreatePool<ParticleBlastCloud>(100);
        /// 
        /// </code>
        /// </example>
        /// <typeparam name="T">The type of particle. It must implement the <see cref="vxIParticle" /> interface</typeparam>
        /// <param name="poolSize">The number of particles in this pool size</param>
        public void CreatePool<T>(int poolSize) where T : vxIParticle
        {
            var newPool = new vxParticlePool(typeof(T), poolSize);
            for (int i = 0; i < poolSize; i++)
            {
                vxIParticle particle;

                System.Reflection.ConstructorInfo ctor = typeof(T).GetConstructor(new[] { typeof(vxGameplayScene3D) });

                // if there isn't this constructor, then there should be one with just the scene
                if (ctor == null)
                {
                    throw new Exception("Missing contructor for " + typeof(T).ToString());
                }
                else
                {
                    particle = (vxIParticle)ctor.Invoke(new object[] { vxEngine.Instance.CurrentScene });
                    newPool.Pool.Add(particle);
                }
            }

            vxEngine.Instance.CurrentScene.ParticleSystem.AddPool(newPool);
        }

        /// <summary>
        /// Spawns a particle of type T
        /// </summary>
        /// <example> 
        /// How to use the Spawn method.
        /// <code>
        /// 
        /// // Spawns a new particle of type 'ParticleBlasCloudFire' using the <see cref="vxGameObject"/> emitter.
        /// vxParticleSystem.Instance.Spawn<ParticleBlastCloudFire>(emitter);
        /// 
        /// </code>
        /// </example>
        /// <typeparam name="T">Spawns a of particle. It must implement the <see cref="vxIParticle" /> interface</typeparam>
        /// <param name="emitter"></param>
        /// <returns></returns>
        public T Spawn<T>(vxGameObject emitter) where T : vxIParticle
        {
            return (T)vxEngine.Instance.CurrentScene.ParticleSystem.SpawnParticle(typeof(T).Name, emitter);
        }
    }
}
