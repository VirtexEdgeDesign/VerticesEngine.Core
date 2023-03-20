using BEPUphysics.Entities;
using BEPUphysics.Entities.Prefabs;

namespace VerticesEngine.Physics
{
    /// <summary>
    /// A 3D Sphere Physics Collider interface
    /// </summary>
    public interface vxSphereCollider : vxPhysicsCollider3D
    {
        /// <summary>
        /// The radius of the sphere collider
        /// </summary>
        public float Radius { get; set; }
    }
}
