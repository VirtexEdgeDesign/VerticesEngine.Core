namespace VerticesEngine.Physics
{
    /// <summary>
    /// A 3D Capsule Physics Collider interface
    /// </summary>
    public interface vxCapsuleCollider : vxPhysicsCollider3D
    {
        /// <summary>
        /// The radius of the capsule collider
        /// </summary>
        public float Radius { get; set; }

        /// <summary>
        /// The length of the capsule collider
        /// </summary>
        public float Length { get; set; }
    }
}
