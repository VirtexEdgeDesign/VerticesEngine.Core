using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using VerticesEngine.Graphics;

namespace VerticesEngine.Editor.Entities
{
    /// <summary>
    /// Which direction is this item scaling?
    /// </summary>
    public enum ScaleDirection
    {
        X,
        Y,
        Z
    }

    /// <summary>
    /// Scaling entity for helping re-size items like volumes
    /// </summary>
    public abstract class vxResizingGizmoHandle : vxEditorEntity
    {
        private Vector3 prePos;

        protected BasicEffect BasicEffect;

        protected float ScreenSpaceZoomFactor = 1;

        public ScaleDirection ScaleDirection
        {
            get { return _scaleDirection; }
            set
            {
                _scaleDirection = value;

                if (_scaleDirection == ScaleDirection.X)
                    rotMatrix = Matrix.CreateRotationX(vxMathHelper.DegToRad(90));

                if (_scaleDirection == ScaleDirection.Y)
                    rotMatrix = Matrix.CreateRotationY(vxMathHelper.DegToRad(90));

                if (_scaleDirection == ScaleDirection.Z)
                    rotMatrix = Matrix.CreateRotationZ(vxMathHelper.DegToRad(90));
            }
        }
        private ScaleDirection _scaleDirection;

        protected Matrix rotMatrix = Matrix.Identity;

        /// <summary>
        /// The Cube is moved
        /// </summary>
        public event EventHandler<EventArgs> Moved;

        public vxResizingGizmoHandle(vxGameplayScene3D scene, Vector3 StartPosition, vxEntity3D Parent) : base(scene, vxEntityCategory.Entity)
        {
            BasicEffect = new BasicEffect(vxGraphics.GraphicsDevice);
            BasicEffect.DiffuseColor = Color.Magenta.ToVector3();
        }

        protected override vxMesh OnLoadModel()
        {
            return vxInternalAssets.Models.UnitCylinder;
        }

        public override void OnSelected()
        {
            base.OnSelected();
        }


        protected internal override void Update()
        {
            base.Update();

            if (Math.Abs(Vector3.Subtract(Position, prePos).Length()) > 0.0005f)
            {
                // constrain the movement
                if (SelectionState == vxSelectionState.Selected)
                {
                    // Raise the 'Moved' event.
                    if (Moved != null)
                        Moved(this, new EventArgs());
                }
            }

            prePos = Position;

            //_worldTransform = GetTransform();
            // TODO: Fix ^
        }

        protected virtual Matrix GetTransform()
        {
            return Matrix.CreateScale(ScreenSpaceZoomFactor / 100 * new Vector3(1, 10, 1)) * rotMatrix * Matrix.CreateTranslation(Position);
        }

        protected internal override void OnSandboxStatusChanged(bool IsRunning)
        {
            base.OnSandboxStatusChanged(IsRunning);

            this.IsEnabled = !IsRunning;
        }


        public override void RenderOverlayMesh(vxCamera3D Camera)
        {
            if (Scene != null && Scene.SandboxCurrentState == vxEnumSandboxStatus.EditMode && Model != null)
            {
                ScreenSpaceZoomFactor = Math.Abs(Vector3.Subtract(Position, Camera.Position).Length());

                foreach (vxModelMesh mesh in Model.Meshes)
                {
                    BasicEffect.DiffuseColor = (SelectionState == vxSelectionState.Selected ? Color.DeepSkyBlue : Color.DarkOrange).ToVector3();
                    //BasicEffect.Texture = vxInternalAssets.Textures.DefaultDiffuse;
                    //BasicEffect.TextureEnabled = true;
                    BasicEffect.World = Transform.Matrix4x4Transform;
                    BasicEffect.View = Camera.View;
                    BasicEffect.Projection = Camera.Projection;
                    mesh.Draw(BasicEffect);
                }
            }
        }

    }
}
