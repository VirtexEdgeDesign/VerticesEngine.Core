using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VerticesEngine;
using VerticesEngine.Graphics;
using VerticesEngine.Physics;
using VerticesEngine.UI;
using VerticesEngine.UI.Controls;

namespace VerticesEngine
{
    public enum EntityType
    {
        BaseEntity,
        Joint,
        Particle
    }

    [Flags]
    public enum SandboxOptions
    {

        /// <summary>
        /// The entity is deletable in the sandbox. This is useful for certain items such as node define points
        /// </summary>
        Delete = 1,

        /// <summary>
        /// This entity is saveable
        /// </summary>
        Save = 2,

        /// <summary>
        /// Should this entity be exported?
        /// </summary>
        Export = 4

    }

    /// <summary>
    /// Base Entity in the Virtex vxEngine which controls all Rendering and Provides
    /// position and world matrix updates to the other required entities.
    /// </summary>
    public class vxEntity : vxGameObject, ICloneable
    {
        /// <summary>
        /// Gets the current scene of the game
        /// </summary>
		public vxGameplaySceneBase CurrentScene
        {
			get { return m_currentScene; }
        }
        private vxGameplaySceneBase m_currentScene;

        /// <summary>
        /// List of Components attached to this Entity
        /// </summary>
        protected List<vxComponent> Components
        {
            get { return m_components; }
        }
        private List<vxComponent> m_components = new List<vxComponent>();


        /// <summary>
        /// Options for this entity in the sandbox. I.e. is it saveable, is it cullable, etc...
        /// </summary>
        public SandboxOptions EntitySandboxOptions;

        protected void AddSandboxOption(SandboxOptions option)
        {
            EntitySandboxOptions |= option;
        }

        protected void RemoveSandboxOption(SandboxOptions option)
        {
            EntitySandboxOptions &= ~option;
        }

        public bool HasSandboxOption(SandboxOptions option)
        {
            return EntitySandboxOptions.HasFlag(option);
        }

        /// <summary>
        /// Whether or not too keep Updating the current Entity
        /// </summary>
        public bool KeepUpdating = true;

		/// <summary>
		/// Should it be Rendered in Debug
		/// </summary>
		public bool RenderEvenInDebug = false;


        /// <summary>
        /// The transform for this entity
        /// </summary>
        public vxTransform Transform
        {
            get { return m_transform; }
            set
            {
                // if we're setting the transform, we don't want to set the reference, we just want to set the internal structs
                m_transform.Scale = value.Scale;
                m_transform.Rotation = value.Rotation;
                m_transform.Position = value.Position;
            }
        }
        private vxTransform m_transform = new vxTransform();

        /// <summary>
        /// The Bounding Sphere which is used to do frustrum culling.
        /// </summary>
        public BoundingSphere BoundingShape
        {
            get { return m_boundingSphere; }
        }
        protected BoundingSphere m_boundingSphere = new BoundingSphere();


        /// <summary>
        /// Should this entity be checked  for culling. Items like the Sky box shouldn't ever be.
        /// </summary>
        public bool IsEntityCullable
        {
            get { return EntityRenderer.IsCullable; }
            set { EntityRenderer.IsCullable = value; }
        }


        private bool m_isFirstUpdate = true;

        public vxEntityRenderer EntityRenderer
        {
            get { return m_entityRenderer; }
        }
        private vxEntityRenderer m_entityRenderer;

        /// <summary>
		/// Initializes a new instance of the <see cref="VerticesEngine.Entities.vxEntity"/> class. The Base Entity Object for the Engine.
        /// </summary>
        /// <param name="scene">The current Scene for this entity to be added to.</param>
        public vxEntity(vxGameplaySceneBase scene)
        {
            if (scene != null)
                m_currentScene = scene;
            else
                m_currentScene = vxEngine.Instance.CurrentScene;

            AddSandboxOption(SandboxOptions.Delete);
            AddSandboxOption(SandboxOptions.Save);
            AddSandboxOption(SandboxOptions.Export);

            m_entityRenderer = CreateRenderer();
        }

        protected virtual vxEntityRenderer CreateRenderer()
        {
            return AddComponent<vxEntityRenderer>();
        }

        protected override void OnVisibilityChanged()
        {
            if (EntityRenderer != null)
                EntityRenderer.IsEnabled = this.IsVisible;

            base.OnVisibilityChanged();
        }

        /// <summary>
        /// Applies the materials to the a corresponding mesh in the mesh renderer
        /// </summary>
        protected virtual vxMaterial OnMapMaterialToMesh(vxModelMesh mesh)
        {
            return new vxMaterial(new vxShader(vxInternalAssets.Shaders.MainShader));
        }


        /// <summary>
        /// Clones this Entity.
        /// </summary>
        /// <returns>A Clone copy of this object</returns>
        public virtual object Clone()
        {
            return MemberwiseClone();
        }


        protected virtual void OnFirstUpdate()
        {

        }

        /// <summary>
        /// Update the Entity.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected internal virtual void Update()
        {
			if(m_isFirstUpdate==true)
            {
                m_isFirstUpdate = false;
                OnFirstUpdate();
            }

            // update each of the internal components
            for (int c = 0; c < m_components.Count; c++)
            {
                if (m_components[c].IsEnabled)
                {
                    m_components[c].Update();
                }
            }
        }

        protected internal virtual void PostUpdate() {

            for (int c = 0; c < m_components.Count; c++)
            {
                if (m_components[c].IsEnabled)
                {
                    m_components[c].PostUpdate();
                }
            }
        }

        protected virtual void MarkForDisposal()
        {
            CurrentScene.AddForDisposal(this);
        }



        /// <summary>
        /// Called when this entity passes the cull test
        /// </summary>
        /// <param name="Camera"></param>
        protected internal virtual void OnWillDraw(vxCamera Camera)
        {
            m_transform.RenderPassData.WVP = m_transform.Matrix4x4Transform * Camera.ViewProjection;
            m_transform.RenderPassData.WorldInvT = Matrix.Transpose(Matrix.Invert(m_transform.Matrix4x4Transform));
            m_transform.RenderPassData.CameraPos = Camera.Position;
        }


        protected override void OnDisposed()
        {
            for (int c = 0; c < Components.Count; c++)
            {
                m_components[c].Dispose();
            }
            m_components.Clear();

            try
            {
                if (vxGameObject.NameRegister.Contains(this.Id))
                    vxGameObject.NameRegister.Remove(this.Id);
            }
            catch (Exception ex)
            {
                vxConsole.WriteException(this, ex);
            }

            //First Remove From Entities List
            if (m_currentScene.Entities.Contains(this))
                m_currentScene.Entities.Remove(this);

            // remove scene reference
            m_currentScene = null;

            m_currentScene = null;


            base.OnDisposed();

            m_entityRenderer = null;

            m_transform = null;
        }

        public virtual void OnNewItemAdded(string itmekey) { }

        #region -- Component Management --

        /// <summary>
        /// Add's a component to this entitiy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T AddComponent<T>() where T: vxComponent
        {
            // create a new component
            var component = (T)Activator.CreateInstance(typeof(T));

            // add it to the list
            Components.Add(component);

            // now initialise it
            component.InternalInitialise(this);

            // set enabled to true always
            component.IsEnabled = true;

            return component;
        }


        /// <summary>
        /// Get's a component from the collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetComponent<T>() where T : vxComponent
        {
            for(int c = 0; c < Components.Count; c++)
            {
                if(Components[c].GetType() == typeof(T))
                {
                    return Components[c] as T;
                }
            }

            // if nothing is return, then return null if nothing
            return null;
        }

        internal void OnEntityCollisionStart(vxEntity entityA, vxEntity entityB, vxBEPUPhysicsBaseCollider collider)
        {
            for (int c = 0; c < entityA.Components.Count; c++)
            {
                if (entityA.Components[c].IsEnabled)
                {
                    entityA.Components[c].OnCollisionStart(entityB, collider);
                }
            }

            for (int c = 0; c < entityB.Components.Count; c++)
            {
                if (entityB.Components[c].IsEnabled)
                {
                    entityB.Components[c].OnCollisionStart(entityA, collider);
                }
            }
        }


        internal void OnEntityCollisionEnd(vxEntity entityA, vxEntity entityB, vxBEPUPhysicsBaseCollider collider)
        {
            for (int c = 0; c < entityA.Components.Count; c++)
            {
                if (entityA.Components[c].IsEnabled)
                {
                    entityA.Components[c].OnCollisionEnd(entityB, collider);
                }
            }

            for (int c = 0; c < entityB.Components.Count; c++)
            {
                if (entityB.Components[c].IsEnabled)
                {
                    entityB.Components[c].OnCollisionEnd(entityA, collider);
                }
            }
        }


        internal void OnEntityTriggerEnter(vxEntity entityA, vxEntity entityB, vxBEPUPhysicsBaseCollider collider)
        {
            for (int c = 0; c < entityA.Components.Count; c++)
            {
                entityA.Components[c].OnTriggerEnter(entityB, collider);
            }

            for (int c = 0; c < entityB.Components.Count; c++)
            {
                entityB.Components[c].OnTriggerEnter(entityA, collider);
            }
        }


        internal void OnEntityTriggerExit(vxEntity entityA, vxEntity entityB, vxBEPUPhysicsBaseCollider collider)
        {
            for (int c = 0; c < entityA.Components.Count; c++)
            {
                entityA.Components[c].OnTriggerExit(entityB, collider);
            }

            for (int c = 0; c < entityB.Components.Count; c++)
            {
                entityB.Components[c].OnTriggerExit(entityA, collider);
            }
        }

        #endregion


        #region -- Sandbox Code --


        protected internal virtual void OnSandboxStatusChanged(bool IsRunning) { }



        public virtual void GetPropertyInfo(vxPropertiesControl propertyControl) { }

        /// <summary>
        /// Sandbox data which holds the vxSerializable attribute
        /// </summary>
        internal string SandboxData;

        /// <summary>
        /// A method which allows for certain opperations to be preformed just before the entity is saved to a file.
        /// </summary>
        public virtual void OnBeforeEntitySerialize() { }

        /// <summary>
        /// A method which allows for certain opperations to be preformed after the entity is loaded from a file.
        /// </summary>
        public virtual void OnAfterEntityDeserialized() { }

        /// <summary>
        /// Are we currently deserailizing
        /// </summary>
        protected internal bool IsDeserializing = false;

        internal void GetSerialisableSandboxData()
        {

            var serialiedAttributes = this.GetType().GetCustomAttributes(typeof(vxSerialiseAttribute), true);

            this.SandboxData = string.Empty;

            List<PropertyInfo> propertiesInfo = GetType().GetProperties().Where(
                p => p.GetCustomAttributes(typeof(vxSerialiseAttribute), true).Any()).ToList();

            foreach (var prop in propertiesInfo)
            {
                this.SandboxData += string.Format("{0}:{1}\n", prop.Name, Newtonsoft.Json.JsonConvert.SerializeObject(prop.GetValue(this)));
            }
        }


        internal void SetSerialisedSandboxData()
        {
            if (this.SandboxData == null)
                return;


            List<PropertyInfo> propertiesInfo = GetType().GetProperties().Where(
                p => p.GetCustomAttributes(typeof(vxSerialiseAttribute), true).Any()).ToList();

            var serialisedFields = this.SandboxData.Split('\n');

            foreach (var field in serialisedFields)
            {
                if (field.Contains(":"))
                {
                    var fieldEntriesSplitLoc = field.IndexOf(':');
                    var name = field.Substring(0, fieldEntriesSplitLoc);
                    var valu = field.Substring(fieldEntriesSplitLoc + 1);

                    foreach (var prop in propertiesInfo)
                    {
                        if (prop.Name == name)
                        {
                            try
                            {
                                var val = Newtonsoft.Json.JsonConvert.DeserializeObject(valu, prop.GetValue(this).GetType());
                                prop.SetValue(this, val);
                            }
                            catch (Exception ex)
                            {
                                vxConsole.WriteException(this, ex);
                            }
                        }
                    }
                }
            }
        }



        #endregion

        #region -- Internal Engine Methods --

        /// <summary>
        /// Called internally by the engine when enabled changes
        /// </summary>
        /// <param name="value"></param>
        internal override void InternalOnEnabledChanged(bool value)
        {
            for (int c = 0; c < Components.Count; c++)
            {
                Components[c].IsEnabled = value;
            }
        }

        #endregion

        #region -- Utilities --

        public T CastAs<T>() where T : vxEntity
        {
            return (T)this;
        }

        #endregion
    }
}

