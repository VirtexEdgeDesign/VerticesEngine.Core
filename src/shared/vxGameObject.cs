using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using VerticesEngine.Graphics;
using VerticesEngine.UI.Controls;

namespace VerticesEngine
{
    /// <summary>
    /// This item can be selected in the sandbox
    /// </summary>
    public interface vxISelectable
    {
        string GetTitle();

        Texture2D GetIcon(int w, int h);
    }

    /// <summary>
    /// General Inspector Categories
    /// </summary>
    public enum vxInspectorCategory
    {
        BasicProperties,
        EntityProperties,
        EntityTransformProperties,
        ModelProperties,
        GraphicalProperies,
    }


    public enum vxInspectorShaderCategory
    {
        AntiAliasing,
        BlurShader,
        Bloom,
        MotionBlur,
        DeferredRenderer,
        DepthOfField,
        Distortion,
        EdgeDetection,
        GodRays,
        Lighting,
        ShadowMapping,
        ScreenSpaceAmbientOcclusion,
        ScreenSpaceReflections

    }

    /// <summary>
    /// This is the base class for all items in the Vertices Engine. It allows access to basic variables such as the 
    /// Engine and the GraphicsDevice.
    /// </summary>
    public class vxGameObject : IDisposable, vxISelectable
    {
        /// <summary>
        /// Is this Game Object currently visible? Note that an Object can be Enabled, but not visible
        /// </summary>
        public bool IsVisible
        {
            get { return m_isVisible; }
            set
            {
                if (m_isVisible != value)
                {
                    m_isVisible = value;
                    OnVisibilityChanged();
                }
            }
        }
        private bool m_isVisible = true;

        /// <summary>
        /// Called when an Objects Visibility Changes
        /// </summary>
        protected virtual void OnVisibilityChanged() { }

        /// <summary>
        /// Is this Game Object Enabled currently
        /// </summary>
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
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

                    InternalOnEnabledChanged(_isEnabled);
                }
            }
        }
        private bool _isEnabled = true;

        /// <summary>
        /// Called on Component Enabled
        /// </summary>
        public virtual void OnEnabled() { }


        /// <summary>
        /// Called on Component Disabled
        /// </summary>
        public virtual void OnDisabled() { }

        /// <summary>
        /// A unqiue string id which allows references to items to be serialised between sessions
        /// </summary>
        [vxShowInInspectorAttribute("Element ID", vxInspectorCategory.BasicProperties, "The Elements ID in the system.", isReadOnly:true, isDebugOnly:true)]
		public string Id
        {
            get { return _id; }
            set {

                if (_id != value)
                {
                    //vxConsole.WriteVerboseLine($"New ID OLD {_id}, NEW {value}");

                    if (HasId())
                    {
                        if (vxGameObject.NameRegister.Contains(this.Id))
                            vxGameObject.NameRegister.Remove(this.Id);
                        _id = value;
                        vxGameObject.NameRegister.Add(this.Id);
                    }
                }
            }
        }
        string _id = "";



        protected virtual bool HasId()
        {
            return true;
        }

        /// <summary>
        /// Gets the default texture.
        /// </summary>
        /// <value>The default texture.</value>
        public Texture2D DefaultTexture
        {
            get { return vxInternalAssets.Textures.Blank; }
        }

        #region - Sandbox State Fields and Properties -

        /// <summary>
        /// State of the Entity which is triggered by the simulation.
        /// </summary>
        public vxEnumSandboxStatus SandboxState
        {
            get { return vxEngine.Instance.CurrentScene.SandboxCurrentState;  }
        }


        /// <summary>
		/// Gets or sets the selection state.
		/// </summary>
		/// <value>The state of the selection.</value>
		public vxSelectionState SelectionState
        {
            get { return _selectionState; }
            set
            {
                _previousSelectionState = _selectionState;
                _selectionState = value;
                OnSelectionStateChange();
            }
        }
        vxSelectionState _selectionState;

        /// <summary>
        /// Get's the previous selection state
        /// </summary>
        protected vxSelectionState PreviousSelectionState
        {
            get { return _previousSelectionState; }
        }
        vxSelectionState _previousSelectionState;

        public bool OnlySelectInSandbox = false;

        /// <summary>
        /// Called when Selected
        /// </summary>
        public virtual void OnSelected() { }


        /// <summary>
        /// Called when Unselected
        /// </summary>
        public virtual void OnUnSelected() { }

        /// <summary>
        /// Called when the selection state changes.
        /// </summary>
        protected virtual void OnSelectionStateChange()
        {
            // If the Level is in Edit mode, then set up Selection
            if (SandboxState == vxEnumSandboxStatus.Running && OnlySelectInSandbox)
            {
                SelectionState = vxSelectionState.None;
            }
            else
            {
                if (SelectionState == vxSelectionState.Selected &&
                    PreviousSelectionState != vxSelectionState.Selected)
                {
                    OnSelected();
                    // Raise the 'SelectionStateSelected' event.
                    if (Selected != null)
                        Selected(this, new EventArgs());
                }

                if (PreviousSelectionState == vxSelectionState.Selected &&
                    SelectionState != vxSelectionState.Selected)
                {
                    OnUnSelected();
                    // Raise the 'SelectionStateUnSelected' event.
                    if (UnSelected != null)
                        UnSelected(this, new EventArgs());
                }

                _previousSelectionState = SelectionState;
            }
        }


        /// <summary>
        /// Event Fired when the Items Selection stat Changes too Hovered
        /// </summary>
        public event EventHandler<EventArgs> Selected;

        /// <summary>
        /// Event Fired when the Items Selection stat Changes too unselected (or unhovered)
        /// </summary>
        public event EventHandler<EventArgs> UnSelected;

        #endregion


        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <returns>The title.</returns>
        public virtual string GetTitle()
        {
            return this._id;
        }

        internal void GetProperties(vxPropertiesControl propertyControl)
        {
            // first get all Attributes for this Item
            propertyControl.AddPropertiesFromType(GetType());
            
        }

        /// <summary>
        /// Gets the icon for this game object. Override this to provide per-entity cusomtization.
        /// </summary>
        /// <returns>The icon.</returns>
        public virtual Texture2D GetIcon(int w, int h)
        {
            return DefaultTexture;
        }

        protected virtual string GetIdPrefix()
        {
            return this.GetType().ToString();
        }

        /// <summary>
        /// Creates a new vxGameObject
        /// </summary>
        /// <param name="Engine"></param>
        public vxGameObject()
        {
			string key = GetIdPrefix();

			// break down key for id
			if (key.Contains("."))
                _id = key.Substring(key.LastIndexOf(".") + 1) + ".";

			int i = 0;

            // set the ID. 
            if (HasId())
            {
                while (vxGameObject.NameRegister.Contains(_id + i))
                    i++;

                _id += i;
                vxGameObject.NameRegister.Add(Id);
            }
            else
            {
                //Console.WriteLine(this.GetType());
            }
        }


        public bool IsDisposed
        {
            get { return m_isDisposed; }
        }
        private bool m_isDisposed = false;

        /// <summary>
        /// Dispposes this Object
        /// </summary>
		public void Dispose(){
            if (m_isDisposed == false)
            {
                m_isDisposed = true;
                vxConsole.WriteVerboseLine($"Disposing {this.Id}-{this.GetType()}");
                OnDisposed();
            }
        }


        /// <summary>
        /// Called when the entity is disposed
        /// </summary>
        protected virtual void OnDisposed() { m_isDisposed = true; }


        /// <summary>
        /// Called when there is a reset or refresh of Graphic settings such as resolution or setting.
        /// </summary>
        public virtual void OnGraphicsRefresh() { }


        #region -- Internal Engine Methods --


        /// <summary>
        /// The name register.
        /// </summary>
        public static List<string> NameRegister = new List<string>();


        /// <summary>
        /// Called internally by the engine when enabled changes
        /// </summary>
        /// <param name="value"></param>
        internal virtual void InternalOnEnabledChanged(bool value) { }

        #endregion
    }
}
