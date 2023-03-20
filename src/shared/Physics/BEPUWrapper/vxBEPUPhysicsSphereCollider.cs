using BEPUphysics.Entities;
using BEPUphysics.Entities.Prefabs;

namespace VerticesEngine.Physics.BEPUWrapper
{
    public class vxBEPUPhysicsSphereCollider : vxBEPUPhysicsBaseCollider, vxSphereCollider
    {
        public override vxPhysicsBodyType ColliderType
        {
            get { return vxPhysicsBodyType.Sphere; }
        }

        /// <summary>
        /// Sphere collider radius
        /// </summary>
        public float Radius
        {
            get { return _radius; }
            set
            {
                _radius = value;
                collider.Radius = _radius;
                RefreshDebugMesh();
            }
        }
        private float _radius = 1;

        private Sphere collider
        {
            get { return (Sphere)BEPUCollider; }
        }

        protected override Entity InitColliderEntity()
        {
            return new Sphere(PairedEntity.Position, _radius);
        }


        protected override void OnMassChanged(float mass)
        {
            collider.Mass = mass;
            base.OnMassChanged(mass);
        }
    }
}
