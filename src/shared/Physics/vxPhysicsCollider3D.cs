using Microsoft.Xna.Framework;

namespace VerticesEngine.Physics
{
    public enum vxPhysicsColliderMovementType
    {
        /// <summary>
        /// The physics body never moves and controls the entities transform
        /// </summary>
        Static,

        /// <summary>
        /// The physics body can move and controls the entities transform
        /// </summary>
        Dynamic,

        /// <summary>
        /// The physics body has it's transform set by the entities transform
        /// </summary>
        Kinamatic
    }

    /// <summary>
    /// An easy to use enum which will tell other components what type of collider this is from a high level
    /// </summary>
    public enum vxPhysicsBodyType
    {
        Box,
        Sphere,
        Capsule,
        StaticMesh,
        MeshCollider
    }

    /// <summary>
    /// Interface definition for a 3D Physics Collider.
    /// </summary>
    public interface vxPhysicsCollider3D
    {
        vxPhysicsBodyType ColliderType { get; }

        /// <summary>
        /// The movement type of this collider
        /// </summary>
        vxPhysicsColliderMovementType MovementType { get; set; }

        float Mass { get; set; }

        bool IsAffectedByGravity { get; set; }

        Vector3 LinearVelocity { get; set; }

        Vector3 AngularVelocity { get; set; }
        public Matrix WorldTransform { get; set; }
        public float StaticFriction { get; set; }
        public float KineticFriction { get; set; }
        public float Bounciness { get; set; }
        public bool IsTrigger { get; set; }

        public float LinearDamping { get; set; }

        public BoundingBox BoundingBox { get; }

        public Vector3 Position { get; set; }

        public bool IsDebugViewEnabled { get; set; }

        void ApplyImpulse(Vector3 position, Vector3 impulse);
        void Clear();
    }
}
