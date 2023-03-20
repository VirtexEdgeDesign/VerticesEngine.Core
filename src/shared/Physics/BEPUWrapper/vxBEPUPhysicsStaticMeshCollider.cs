using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.CollisionRuleManagement;
using BEPUutilities;
using Microsoft.Xna.Framework;
using VerticesEngine.Graphics;

namespace VerticesEngine.Physics.BEPUWrapper
{
    public class vxBEPUPhysicsStaticMeshCollider : vxComponent, vxStaticMeshCollider
    {
        public virtual vxPhysicsBodyType ColliderType
        {
            get { return vxPhysicsBodyType.StaticMesh; }
        }

        public vxEntity3D PairedEntity
        {
            get { return (vxEntity3D)this.Entity; }
        }

        /// <summary>
        /// The BEPU static mesh
        /// </summary>
        private StaticMesh BEPUStaticMesh;

        public Matrix WorldTransform
        {
            get { return BEPUStaticMesh.WorldTransform.Matrix; }
            //set { BEPUStaticMesh.WorldTransform = AffineTransform. value; }
        }

        public object Tag
        {
            get { return BEPUStaticMesh.Tag; }
            set { BEPUStaticMesh.Tag = value; }
        }

        //public Matrix LocalWorldTransform = Matrix.CreateRotationZ(MathHelper.PiOver2);

        public float StaticFriction
        {
            get { return BEPUStaticMesh.Material.StaticFriction; }
            set { BEPUStaticMesh.Material.StaticFriction = value; }
        }
        public float KineticFriction
        {
            get { return BEPUStaticMesh.Material.KineticFriction; }
            set { BEPUStaticMesh.Material.KineticFriction = value; }
        }
        public float Bounciness
        {
            get { return BEPUStaticMesh.Material.Bounciness; }
            set { BEPUStaticMesh.Material.Bounciness = value; }
        }

        public bool IsTrigger
        {
            get { return m_isTrigger; }
            set
            {
                m_isTrigger = value;

                if (BEPUStaticMesh != null)
                    BEPUStaticMesh.CollisionRules.Personal = (m_isTrigger == false ? CollisionRule.Normal : CollisionRule.NoSolver);
            }
        }
        private bool m_isTrigger = false;

        public vxMesh Mesh
        {
            get { return m_physMesh; }
            set
            {
                m_physMesh = value;
                RefreshMesh();
            }
        }
        private vxMesh m_physMesh;

        public Vector3 Scale
        {
            get { return m_scale; }
            set
            {
                m_scale = value;
                OnTransformChanged();
            }
        }
        private Vector3 m_scale = Vector3.One;

        protected override void Initialise()
        {
            base.Initialise();

            BEPUStaticMesh = InitialiseStaticMesh();
        }

        //private int refreshCount = 0;
        /// <summary>
        /// This refreshes the static mesh currently in the scene
        /// </summary>
        public void RefreshMesh()
        {
            //vxConsole.WriteLine("RefreshMesh " + this.Name + ": cnt: "+refreshCount ++);
            if (BEPUStaticMesh != null)
            {
                try
                {
                    PairedEntity.Scene.PhysicsDebugViewer.Remove(BEPUStaticMesh);
                    PairedEntity.Scene.PhyicsSimulation.Remove(BEPUStaticMesh);

                    //vxConsole.WriteLine("PhyicsSimulation.Remove " + this.Name);
                }
                catch
                {
                    vxConsole.WriteLine("Can't remove static mesh for " + this.Name);
                }
            }
            BEPUStaticMesh = InitialiseStaticMesh();
        }


        protected internal override void OnEnabled()
        {
            if (BEPUStaticMesh != null && isActive == false)
            {
                try
                {

                    PairedEntity.Scene.PhyicsSimulation.Add(BEPUStaticMesh);
                    PairedEntity.Scene.PhysicsDebugViewer.Add(BEPUStaticMesh);

                    isActive = true;
                }
                catch { vxConsole.WriteLine("Can't Add static mesh OnEnabled for " + this.PairedEntity.Id); }
            }
        }

        protected internal override void OnDisabled()
        {
            if (BEPUStaticMesh != null && isActive == true)
            {
                try
                {
                    PairedEntity.Scene.PhyicsSimulation.Remove(BEPUStaticMesh);
                    PairedEntity.Scene.PhysicsDebugViewer.Remove(BEPUStaticMesh);

                    isActive = false;
                }
                catch { vxConsole.WriteLine("Can't Remove static mesh OnDisabled for " + this.PairedEntity.Id); }
            }
        }

        bool isActive = false;

        protected virtual StaticMesh InitialiseStaticMesh()
        {
            //vxConsole.WriteLine("InitialiseStaticMesh " + this.Name);

            if (m_physMesh == null)
                m_physMesh = PairedEntity.Model;

            if (m_physMesh != null && m_physMesh.Meshes.Count > 0)
            {
                vxMeshHelper.GetVerticesAndIndicesFromModel(m_physMesh, out Vector3[] MeshVertices, out int[] MeshIndices);

                var staticMesh = new StaticMesh(MeshVertices, MeshIndices,
                    new AffineTransform((0.01f * PairedEntity.Transform.Scale), PairedEntity.Transform.Rotation, PairedEntity.Position));

                PairedEntity.Scene.PhyicsSimulation.Add(staticMesh);
                PairedEntity.Scene.PhysicsDebugViewer.Add(staticMesh);

                isActive = true;

                staticMesh.Tag = PairedEntity;
                return staticMesh;
            }
            else
            {
                return null;
            }
        }

        protected internal override void OnTransformChanged()
        {
            if (BEPUStaticMesh != null)
            {
                PairedEntity.Scene.PhysicsDebugViewer.Remove(BEPUStaticMesh);

                BEPUStaticMesh.WorldTransform = new AffineTransform(0.01f * PairedEntity.Transform.Scale, PairedEntity.Transform.Rotation, PairedEntity.Position);

                PairedEntity.Scene.PhysicsDebugViewer.Add(BEPUStaticMesh);
            }
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();
            if (BEPUStaticMesh != null)
            {
                //Console.WriteLine("Removing " + PairedEntity.Id);
                BEPUStaticMesh.Space?.Remove(BEPUStaticMesh);
                PairedEntity.Scene.PhysicsDebugViewer.Remove(BEPUStaticMesh);

                BEPUStaticMesh = null;

                isActive = false;
            }
        }
    }
}
