using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.UI.Controls
{
    /// <summary>
    /// A Canvas is used for Camera UI overlays
    /// </summary>
    public class vxCanvas : IDisposable
    {
        protected bool isInitialised = false;

        /// <summary>
        /// The owning camera for this HUD
        /// </summary>
        public vxCamera Camera { get; private set; }

        vxUIManager m_uiManager;

        protected List<vxUIControl> Controls
        {
            get { return m_uiManager.Items; }
        }

        protected float Transparency
        {
            get { return m_uiManager.Alpha; }
            set { m_uiManager.Alpha = value; }
        }

        public vxCanvas()
        {
            m_uiManager = new vxUIManager();
        }

        public void Init(vxCamera Camera)
        {
            this.Camera = Camera;

            isInitialised = true;
        }

        /// <summary>
        /// Adds a UI Control to this canvas
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T AddUIControl<T> () where T : vxUIControl
        {
            var uiControl = (T)Activator.CreateInstance(typeof(T));

            m_uiManager.Add(uiControl);

            return uiControl;
        }


        public virtual void Update()
        {
            m_uiManager.Update();
        }

        public virtual void Draw()
        {            
            m_uiManager.DrawByOwner();
        }

        public void Dispose()
        {
            OnDisposed();
            Camera = null;
        }
        protected  virtual void OnDisposed()
        {

        }
    }
}
