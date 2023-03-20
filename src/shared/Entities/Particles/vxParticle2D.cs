using Microsoft.Xna.Framework;
using VerticesEngine.Graphics;
using VerticesEngine;

namespace VerticesEngine.Particles
{
    public enum vxEnumParticleLayer
	{
		Front,
		Behind
	}

	/// <summary>
	/// 2D Particle Object for use in the vxParticleSystem2D Manager Class.
	/// </summary>
    public class vxParticle2D : vxEntity2D, vxIParticle
    {

		/// <summary>
		/// Boolean of whether to keep the Particle Alive or not.
		/// </summary>
        public bool IsAlive
        {
            get { return _isAlive; }
            set
            {
                if (_isAlive == true && value == false)
                {
                    OnParticleDespawned();
                }
                _isAlive = value;
            }
        }
        private bool _isAlive = false;

        /// <summary>
        /// Is the particle infront or behind the scene
        /// </summary>
        public vxEnumParticleLayer ParticleLayer
        {
            get { return _particleLayer; }
            set { _particleLayer = value; }
        }
        public vxEnumParticleLayer _particleLayer = vxEnumParticleLayer.Front;

        public Rectangle Source;

        /// <summary>
        /// The index of this particle in the pool.
        /// </summary>
        public int index = 0;
		
        /// <summary>
		/// Initializes a new instance of the <see cref="T:VerticesEngine.Particles.vxParticle2D"/> class.
		/// </summary>
		/// <param name="Engine">Engine.</param>
		/// <param name="texture">Texture.</param>
		/// <param name="StartPosition">Start position.</param>
        public vxParticle2D(vxGameplayScene2D Scene, Rectangle Source, int index) : base(Scene, Source, Vector2.Zero)
        {
			// Remove the Entity from the main Entity List
            Scene.Entities.Remove(this);

            this.index = index;

            // Now add it to the main list
            _isAlive = false;

            this.Source = Source;

            Origin = new Vector2(Source.Width / 2f, Source.Height / 2f);
        }

        public vxIParticle Spawn(vxGameObject emitter)
        {
            OnParticleSpawned(emitter);
            return this;
        }

        public void UpdateParticle()
        {
            Update();
        }


        /// <summary>
        /// Activates the particle in the pool as if it was new.
        /// </summary>
        /// <param name="emitter">The Emiting Entity.</param>
        public virtual vxIParticle OnParticleSpawned(vxGameObject emitter)
        {
            _isAlive = true;
            return this;
        }

        /// <summary>
        /// Called when the particle is despawned
        /// </summary>
        /// <returns></returns>
        protected virtual vxIParticle OnParticleDespawned()
        {
            return this;
        }


        public vxIParticle Despawn()
        {
            OnParticleDespawned();
            return this;
        }



        protected override void OnDisposed()
        {
			if (PhysicCollider != null)
			{
				PhysicCollider.Dispose();
			}
            base.OnDisposed();
		}
    }
}