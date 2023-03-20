using BEPUphysics.Entities.Prefabs;
using BEPUphysics.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.Physics.BEPUWrapper
{
    public class vxBEPUPhysicsCapsuleCollider : vxBEPUPhysicsBaseCollider, vxCapsuleCollider
    {
        public override vxPhysicsBodyType ColliderType
        {
            get { return vxPhysicsBodyType.Capsule; }
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
            }
        }
        private float _radius = 0.5f;

        public float Length
        {
            get { return _length; }
            set
            {
                _length = value;
                collider.Radius = _length;
            }
        }
        private float _length = 4;

        private Capsule collider
        {
            get { return (Capsule)BEPUCollider; }
        }

        protected override Entity InitColliderEntity()
        {
            return new Capsule(PairedEntity.Position, _length, _radius);
        }


        protected override void OnMassChanged(float mass)
        {
            collider.Mass = mass;
            base.OnMassChanged(mass);
        }
    }
}
