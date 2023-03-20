using BEPUphysics.Entities;
using BEPUphysics.Entities.Prefabs;
using Microsoft.Xna.Framework;

namespace VerticesEngine.Physics.BEPUWrapper
{
    public class vxBEPUPhysicsBoxCollider : vxBEPUPhysicsBaseCollider, vxBoxCollider
    {
        public override vxPhysicsBodyType ColliderType
        {
            get { return vxPhysicsBodyType.Box; }
        }

        public float Width
        {
            get { return _width; }
            set
            {
                _width = value;
                collider.Width = _width;
                RefreshDebugMesh();
            }
        }
        private float _width = 1;

        public float Height
        {
            get { return _height; }
            set
            {
                _height = value;
                collider.Height = _height;
                RefreshDebugMesh();
            }
        }
        private float _height = 1;

        public float Length
        {
            get { return _length; }
            set
            {
                _length = value;
                collider.Length = _length;
                RefreshDebugMesh();
            }
        }
        private float _length = 4;

        private Box collider
        {
            get { return (Box)BEPUCollider; }
        }

        protected override Entity InitColliderEntity()
        {
            return new Box(PairedEntity.Position, _width, _height, _length);
        }


        protected override void OnMassChanged(float mass)
        {
            collider.Mass = mass;
            base.OnMassChanged(mass);
        }

        public void SetSize(Vector3 size)
        {
            _width = size.X;
            collider.Width = _width;
            _height = size.Y;
            collider.Height = _height;
            _length = size.Z;
            collider.Length = _length;
            RefreshDebugMesh();
        }
    }
}
