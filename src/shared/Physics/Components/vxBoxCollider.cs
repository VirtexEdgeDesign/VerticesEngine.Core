using Microsoft.Xna.Framework;

namespace VerticesEngine.Physics
{
    /// <summary>
    /// A 3D Box Physics Collider interface
    /// </summary>
    public interface vxBoxCollider : vxPhysicsCollider3D
    {
        public float Width { get; set; }

        public float Height { get; set; }

        public float Length { get; set; }

        public void SetSize(Vector3 size);
    }
}
