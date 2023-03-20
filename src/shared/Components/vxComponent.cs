using System;
using System.Collections.Generic;
using System.Text;
using VerticesEngine.Physics;

namespace VerticesEngine
{
    /// <summary>
    /// Component to be attached to an entitiy which allows for containerized functionality 
    /// </summary>
    public abstract class vxComponent : IDisposable
    {
        /// <summary>
        /// Is this enabled
        /// </summary>
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                // check if it's changed.
                if (_isEnabled != value)
                {
                    _isEnabled = value;

                    if (_isEnabled)
                    {
                        OnEnabled();
                    }
                    else
                    {
                        OnDisabled();
                    }
                }
            }
        }
        private bool _isEnabled = true;

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }
        private string m_name = string.Empty;

        internal bool IsSelected
        {
            get { return Entity.SelectionState == vxSelectionState.Selected; }
        }

        /// <summary>
        /// The entitiy which owns this component
        /// </summary>
        protected vxEntity Entity
        {
            get { return _entity; }
        }
        private vxEntity _entity;

        protected internal vxComponent() { }

        /// <summary>
        /// Internal initialise
        /// </summary>
        /// <param name="entity"></param>
        internal void InternalInitialise(vxEntity entity)
        {
            _entity = entity;
            m_name = entity.Id + $".{this.GetType().Name}";
            Initialise();
        }

        /// <summary>
        /// Called on Initialise
        /// </summary>
        protected virtual void Initialise() { }


        public bool IsDisposed
        {
            get { return m_isDisposed; }
        }
        private bool m_isDisposed = false;

        /// <summary>
        /// Called when disposed
        /// </summary>
        public void Dispose() {
            if (m_isDisposed == false)
            {
                m_isDisposed = true;
                OnDisposed();
            }
            _entity = null;
        }

        protected virtual void OnDisposed() { }

        /// <summary>
        /// Called on Component Enabled
        /// </summary>
        protected internal virtual void OnEnabled(){ }


        /// <summary>
        /// Called on Component Disabled
        /// </summary>
        protected internal virtual void OnDisabled() { }

        /// <summary>
        /// Called before the update loop starts. This should be used for early polling of input state and
        /// setup of key values that will be used in the update thread
        /// </summary>
        protected internal virtual void PreUpdate() { }

        /// <summary>
        /// Update loop for this component
        /// </summary>
        protected internal virtual void Update() { }

        /// <summary>
        /// Called after the update loop that allows for any last minute checks and code before entering the
        /// render loop
        /// </summary>
        protected internal virtual void PostUpdate() { }

        /// <summary>
        /// Called when ever the owning entities transform is changed
        /// </summary>
        protected internal virtual void OnTransformChanged() { }

        /// <summary>
        /// Called when a collision starts
        /// </summary>
        protected internal virtual void OnCollisionStart(vxEntity otherEntity, vxBEPUPhysicsBaseCollider collider) { }

        /// <summary>
        /// Called when a collisioin ends
        /// </summary>
        protected internal virtual void OnCollisionEnd(vxEntity otherEntity, vxBEPUPhysicsBaseCollider collider) { }

        /// <summary>
        /// Called when a collision starts
        /// </summary>
        protected internal virtual void OnTriggerEnter(vxEntity otherEntity, vxBEPUPhysicsBaseCollider collider) { }

        /// <summary>
        /// Called when a collisioin ends
        /// </summary>
        protected internal virtual void OnTriggerExit(vxEntity otherEntity, vxBEPUPhysicsBaseCollider collider) { }
    }
}
