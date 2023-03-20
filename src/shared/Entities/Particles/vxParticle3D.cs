using Microsoft.Xna.Framework;
using VerticesEngine.Graphics;

namespace VerticesEngine.Particles
{
    /// <summary>
    /// 3D Particle Object for use in the vxParticleSystem3D Manager Class.
    /// </summary>
    public class vxParticle3D : vxEntity3D, vxIParticle
    {
        /// <summary>
        /// Boolean of whether to keep the Particle Alive or not.
        /// </summary>
        public bool IsAlive { get; set; }

        /// <summary>
        /// Is the particle infront or behind the scene
        /// </summary>
        public vxEnumParticleLayer ParticleLayer
        {
            get { return _particleLayer; }
            set { _particleLayer = value; }
        }
        public vxEnumParticleLayer _particleLayer = vxEnumParticleLayer.Front;

        /// <summary>
        /// Initializes a new instance of the <see cref="VerticesEngine.Particles.vxParticle3D"/> class.
        /// </summary>
        /// <param name="Engine">The Vertices Engine Reference.</param>
        /// <param name="model">Model.</param>
        /// <param name="StartPosition">Start position.</param>
        public vxParticle3D(vxGameplayScene3D scene, vxMesh model) : base(scene, model, Vector3.Zero)
        {
            Scene.Entities.Remove(this);
        }

        protected override bool HasId()
        {
            return false;
        }

        protected override vxEntityRenderer CreateRenderer()
        {
            return AddComponent<vxParticleRenderer>();
        }

        /// <summary>
        /// Disposes the Particle
        /// </summary>
        protected override void OnDisposed()
        {
            base.OnDisposed();
        }

        public vxIParticle Spawn(vxGameObject emitter)
        {
            this.IsAlive = true;

            this.IsEnabled = true;
            this.IsVisible = true;
            OnParticleSpawned(emitter);
            return this;
        }

        protected virtual void OnParticleSpawned(vxGameObject emitter)
        {            
        }


        public vxIParticle Despawn()
        {
            this.IsAlive = false;
            this.IsVisible = false;

            //this.IsEnabled = false;
            OnParticleDespawned();
            return this;
        }

        protected virtual void OnParticleDespawned()
        {

        }

        public void UpdateParticle()
        {
            Update();
        }

    }
}