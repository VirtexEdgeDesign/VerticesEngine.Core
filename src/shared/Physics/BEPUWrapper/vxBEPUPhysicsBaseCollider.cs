using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.CollisionRuleManagement;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.Physics
{

    public abstract class vxBEPUPhysicsBaseCollider : vxComponent, vxPhysicsCollider3D
    {
        /// <summary>
        /// What type of collider is this
        /// </summary>
        public virtual vxPhysicsBodyType ColliderType
        {
            get { return vxPhysicsBodyType.Sphere; }
        }

        /// <summary>
        /// The movement type of this collider
        /// </summary>
        public virtual vxPhysicsColliderMovementType MovementType
        {
            get { return _movementType; }
            set { _movementType = value; }
        }
        private vxPhysicsColliderMovementType _movementType = vxPhysicsColliderMovementType.Kinamatic;

        public vxEntity3D PairedEntity
        {
            get { return (vxEntity3D)this.Entity; }
        }

        protected BEPUphysics.Entities.Entity BEPUCollider;

        public float Mass
        {
            get { return BEPUCollider.Mass; }
            set
            {
                BEPUCollider.Mass = value;
            }
        }

        #region Properties

        public bool IsAffectedByGravity
        {
            get { return BEPUCollider.IsAffectedByGravity; }
            set { BEPUCollider.IsAffectedByGravity = value; }
        }

        public Vector3 LinearVelocity
        {
            get { return BEPUCollider.LinearVelocity; }
            set { BEPUCollider.LinearVelocity = value; }
        }

        public Vector3 AngularVelocity
        {
            get { return BEPUCollider.AngularVelocity; }
            set { BEPUCollider.AngularVelocity = value; }
        }

        public void Clear()
        {
            BEPUCollider.LinearVelocity = Vector3.Zero;
            BEPUCollider.AngularVelocity = Vector3.Zero;
        }


        public Matrix WorldTransform
        {
            get { return BEPUCollider.WorldTransform; }
            set { 
                if(BEPUCollider != null)
                BEPUCollider.WorldTransform = value; }
        }


        public float StaticFriction
        {
            get { return BEPUCollider.Material.StaticFriction; }
            set { BEPUCollider.Material.StaticFriction = value; }
        }
        public float KineticFriction
        {
            get { return BEPUCollider.Material.KineticFriction; }
            set { BEPUCollider.Material.KineticFriction = value; }
        }
        public float Bounciness
        {
            get { return BEPUCollider.Material.Bounciness; }
            set { BEPUCollider.Material.Bounciness = value; }
        }

        public bool IsTrigger
        {
            get { return _isTrigger; }
            set
            {
                _isTrigger = value;

                BEPUCollider.CollisionInformation.CollisionRules.Personal = _isTrigger == false ? CollisionRule.Normal : CollisionRule.NoSolver;
            }
        }

        public float LinearDamping
        {
            get { return BEPUCollider.LinearDamping; }
            set { BEPUCollider.LinearDamping = value; }
        }

        public BoundingBox BoundingBox
        {
            get { return BEPUCollider.CollisionInformation.BoundingBox; }
        }

        public Vector3 Position
        {
            get { return BEPUCollider.Position; }
            set { BEPUCollider.Position = value; }
        }

        private bool _isTrigger = false;

        #endregion

        public bool IsDebugViewEnabled
        {
            get { return m_isDebugViewEnabled; }
            set
            {
                m_isDebugViewEnabled = value;
                if (m_isDebugViewEnabled)
                    PairedEntity.Scene.PhysicsDebugViewer.Add(BEPUCollider);
                else
                    PairedEntity.Scene.PhysicsDebugViewer.Remove(BEPUCollider);
            }
        }
        private bool m_isDebugViewEnabled = true;

        /// <summary>
        /// This is called during Initialise which should return back the collider entity for this physics body
        /// </summary>
        /// <returns></returns>
        protected virtual BEPUphysics.Entities.Entity InitColliderEntity()
        {
            return null;
        }

        protected internal override void OnEnabled()
        {
            base.OnEnabled();
            try
            {
                PairedEntity.Scene.PhyicsSimulation.Add(BEPUCollider);
                if (m_isDebugViewEnabled)
                    PairedEntity.Scene.PhysicsDebugViewer.Add(BEPUCollider);

            }
            catch { }
        }

        protected internal override void OnDisabled()
        {
            base.OnDisabled();
            try
            {
                PairedEntity.Scene.PhyicsSimulation.Remove(BEPUCollider);
                if (m_isDebugViewEnabled)
                    PairedEntity.Scene.PhysicsDebugViewer.Remove(BEPUCollider);
            }
            catch { }
        }

        protected override void Initialise()
        {
            base.Initialise();

            BEPUCollider = InitColliderEntity();

            BEPUCollider.CollisionInformation.Events.DetectingInitialCollision += OnDetectingInitialCollision;
            BEPUCollider.CollisionInformation.Events.CollisionEnded += OnCollisionEnded;
            BEPUCollider.CollisionInformation.Tag = PairedEntity;

            PairedEntity.Scene.PhyicsSimulation.Add(BEPUCollider);
            PairedEntity.Scene.PhysicsDebugViewer.Add(BEPUCollider);
        }

        protected override void OnDisposed()
        {
            if (BEPUCollider != null)
            {
                BEPUCollider.CollisionInformation.Events.DetectingInitialCollision -= OnDetectingInitialCollision;
                BEPUCollider.CollisionInformation.Events.CollisionEnded -= OnCollisionEnded;

                BEPUCollider.Space?.Remove(BEPUCollider);
                PairedEntity.Scene.PhysicsDebugViewer.Remove(BEPUCollider);
                BEPUCollider.CollisionInformation.Tag = null;
                BEPUCollider = null;
            }
            base.OnDisposed();
        }

        protected internal override void OnTransformChanged()
        {

        }

        protected internal override void PostUpdate()
        {
            base.PostUpdate();

            switch (_movementType)
            {
                // if we're static or dynamic, then we control teh meshes transform
                case vxPhysicsColliderMovementType.Static:
                case vxPhysicsColliderMovementType.Dynamic:
                    PairedEntity.Transform = this.WorldTransform.ToTransform();
                    break;

                // otherwise, the entity controls the physics meshes location
                case vxPhysicsColliderMovementType.Kinamatic:
                    this.WorldTransform = PairedEntity.Transform.Matrix4x4Transform;
                    break;

            }
        }

        protected virtual void RefreshDebugMesh()
        {
            PairedEntity.Scene.PhysicsDebugViewer.Remove(BEPUCollider);
            PairedEntity.Scene.PhysicsDebugViewer.Add(BEPUCollider);
        }

        protected virtual void OnMassChanged(float mass) { }

        #region -- Events --

        void CheckTagOnError(object tag, string collisionType)
        {
            if (tag is vxEntity)
                return;
            //else
            //vxConsole.WriteError(collisionType + " collision with non vxEntity of type " + tag.GetType().ToString());

        }

        public object ExtraCollisionInfo;
        protected virtual void OnDetectingInitialCollision(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
        {
            ExtraCollisionInfo = pair;
            // only send the event if we've hit another entity, if its a random tag then report it as an error but continue on
            if (sender.Tag is vxEntity && other.Tag is vxEntity)
            {
                if (_isTrigger)
                    Entity.OnEntityTriggerEnter((vxEntity)sender.Tag, (vxEntity)other.Tag, this);
                else
                    Entity.OnEntityCollisionStart((vxEntity)sender.Tag, (vxEntity)other.Tag, this);
            }
            else
            {
                CheckTagOnError(sender.Tag, "Entered");
                CheckTagOnError(other.Tag, "Entered");
            }
        }

        protected virtual void OnCollisionEnded(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
        {
            if (sender.Tag is vxEntity && other.Tag is vxEntity)
            {
                if (_isTrigger)
                    Entity.OnEntityTriggerExit((vxEntity)sender.Tag, (vxEntity)other.Tag, this);
                else
                    Entity.OnEntityCollisionEnd((vxEntity)sender.Tag, (vxEntity)other.Tag, this);
            }
            else
            {
                CheckTagOnError(sender.Tag, "Left");
                CheckTagOnError(other.Tag, "Left");
            }
        }

        public void ApplyImpulse(Vector3 position, Vector3 impulse)
        {
            BEPUCollider.ApplyImpulse(position, impulse * vxTime.FramerateFactor);
        }

        #endregion
    }
}
