using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using VerticesEngine;

namespace VerticesEngine.Graphics
{
    public class vxEntityRenderer : vxComponent
    {
        /// <summary>
        /// Is this entity cullable by the camera? Or should it always render?
        /// </summary>
        public bool IsCullable
        {
            get { return m_isCullable; }
            set { m_isCullable = value; }
        }
        private bool m_isCullable = true;

        public bool IsRenderedThisFrame
        {
            get { return m_isRenderedThisFrame; }
            internal set { m_isRenderedThisFrame = value; }
        }
        private bool m_isRenderedThisFrame;

        protected internal RenderPassTransformData RenderPassData
        {
            get { return renderPassTransformData; }
        }
        private RenderPassTransformData renderPassTransformData = new RenderPassTransformData();

        /// <summary>
        /// Should this renderer be run for a Util Camera
        /// </summary>
        public bool IsRenderedForUtilCamera = true;

        /// <summary>
        /// Called once when created
        /// </summary>
        protected override void Initialise()
        {
            base.Initialise();
        }

        /// <summary>
        /// Called each frame 
        /// </summary>
        protected internal override void Update()
        {
            base.Update();
        }


        /// <summary>
        /// When the component or owning entity is disposed
        /// </summary>
        protected override void OnDisposed()
        {
            base.OnDisposed();
        }

        protected internal virtual void OnWillDraw(vxCamera Camera)
        {
            Entity.OnWillDraw(Camera);
            RenderPassData.World = Entity.Transform.Matrix4x4Transform;
            RenderPassData.WVP = Entity.Transform.Matrix4x4Transform * Camera.ViewProjection;
            RenderPassData.WorldInvT = Matrix.Transpose(Matrix.Invert(Entity.Transform.Matrix4x4Transform));
            RenderPassData.CameraPos = Camera.Position;
            m_isRenderedThisFrame = true;
        }

        public virtual void Draw(vxCamera Camera, string renderpass)
        {

        }
    }
}
