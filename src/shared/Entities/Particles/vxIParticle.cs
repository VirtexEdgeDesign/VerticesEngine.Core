using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VerticesEngine;
using VerticesEngine.Graphics;

namespace VerticesEngine.Particles
{
    public interface vxIParticle
    {
        /// <summary>
        /// Boolean of whether to keep the Particle Alive or not.
        /// </summary>
        bool IsAlive { get; set; }

        /// <summary>
        /// Is the particle infront or behind the scene
        /// </summary>
        vxEnumParticleLayer ParticleLayer { get; set; }

        vxIParticle Spawn(vxGameObject emitter);

        vxIParticle Despawn();

        void UpdateParticle();

        //vxEntityRenderer Renderer { get; }

        //void DrawParticle(vxCamera Camera, string renderpass);
        //void Draw(vxCamera Camera, string renderpass);
    }
}
