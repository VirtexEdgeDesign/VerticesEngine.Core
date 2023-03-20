using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.CollisionRuleManagement;
using BEPUutilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VerticesEngine.Graphics;

namespace VerticesEngine.Physics
{
    /// <summary>
    /// Interface definition for a 3D Static Mesh Physics Collider.
    /// </summary>
    public interface vxStaticMeshCollider
    {
        vxPhysicsBodyType ColliderType { get; }
        public Matrix WorldTransform { get; }
        public float StaticFriction { get; set; }
        public float KineticFriction { get; set; }
        public float Bounciness { get; set; }
        public bool IsTrigger { get; set; }
    }
}
