using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using VerticesEngine.Commands;
using VerticesEngine.Graphics;
using VerticesEngine.Input;

namespace VerticesEngine.Editor.Entities
{

    /// <summary>
    /// The base class for gimbal transformation entities such as Translation Arrows, Rotators and Panning Sqaures.
    /// </summary>
    public class vxGizmoTransformationBaseEntity : vxEditorEntity
    {
        /// <summary>
        /// The axis that this gizmo entity controls
        /// </summary>
        public GizmoAxis AxisDirections
        {
            get { return m_axis; }
            protected set { m_axis = value; }
        }
        private GizmoAxis m_axis;

        /// <summary>
        /// A reference to the owning fizmo
        /// </summary>
        public vxGizmo Gizmo
        {
            get { return m_gizmo; }
        }
        private vxGizmo m_gizmo;

        /// <summary>
        /// The main axis for this handle
        /// </summary>
        protected Vector3 MainAxis = Vector3.Zero;

        /// <summary>
        /// The perpendicular axis for this handle
        /// </summary>
        protected Vector3 PerpendicularAxis = Vector3.Zero;

        /// <summary>
        /// The scale for this handle
        /// </summary>
        protected float EditorHandleScale = 1;

        /// <summary>
        /// A reference to the Sandbox Camera
        /// </summary>
        protected vxCamera SandboxCamera;

        protected bool m_isFirstSelectionFrame = true;
        protected bool m_isFirstLoopSinceSelected = false;

        protected List<vxTransform> prevTransforms = new List<vxTransform>();

        protected bool FirstCheck = true;

        protected Vector3 _intersectPosition = Vector3.Zero;
        protected Vector3 _lastIntersectionPosition = Vector3.Zero;
        protected bool _isFirstSelection = true;

        public vxGizmoTransformationBaseEntity(vxGameplayScene3D scene, vxGizmo Gizmo, GizmoAxis AxisDirections) : base(scene, vxEntityCategory.Axis)
        {
            m_gizmo = Gizmo;

            Transform.Scale = Vector3.One * 7;

            m_axis = AxisDirections;

            Transform.Scale = Vector3.One;

            PlainColor = GetAxisColour();

            EditorEntityMaterial.IsShadowCaster = false;
            EditorEntityMaterial.SetEffectParameter("NormalColour", PlainColor.ToVector3());
        }

        protected Vector3 GetAxisScale()
        {
            Vector3 scale = Vector3.One;
            switch (AxisDirections)
            {
                case GizmoAxis.X:
                    scale = new Vector3(25, 2, 2);
                    break;
                case GizmoAxis.Y:
                    scale = new Vector3(2, 25, 2);
                    break;
                case GizmoAxis.Z:
                    scale = new Vector3(2, 2, 25);
                    break;
            }
            return scale;
        }


        protected Color GetAxisColour()
        {
            Color resultColour = Color.White;
            switch (m_axis)
            {
                case GizmoAxis.X:
                case GizmoAxis.YZ:
                    resultColour = new Color(225, 10, 10);
                    break;
                case GizmoAxis.Y:
                case GizmoAxis.ZX:
                    resultColour = new Color(10, 225, 10);
                    break;
                case GizmoAxis.Z:
                case GizmoAxis.XY:
                    resultColour = new Color(10, 10, 225);
                    break;
            }

            return resultColour;
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();
            m_gizmo = null;
        }

        protected internal override void Update()
        {
            base.Update();

            if (Scene.SandboxCurrentState == vxEnumSandboxStatus.EditMode)
            {
                UpdateAxis();
            }
        }

        protected internal override void PostUpdate()
        {
            base.PostUpdate();

            IsVisible = Scene.SandboxCurrentState == vxEnumSandboxStatus.EditMode;

            //Transform.Scale = Vector3.One * EditorHandleScale * vxGizmo.ScreenSpaceZoomFactor / (Gizmo.scale);
            //WorldTransform = Matrix.CreateScale(EditorHandleScale * vxGizmo.ScreenSpaceZoomFactor / (Gizmo.scale)) *
            //    Matrix.CreateWorld(Gizmo.Position, MainAxis, PerpendicularAxis);
            Position = Gizmo.Position;
            Transform.Rotation = Quaternion.CreateFromRotationMatrix(Matrix.CreateWorld(Gizmo.Position, MainAxis, PerpendicularAxis));
        }


        protected Ray ConvertMouseToRay(Vector2 mousePosition, vxCamera Camera)
        {
            Vector3 nearPoint = new Vector3(mousePosition, 0);
            Vector3 farPoint = new Vector3(mousePosition, 1);

            nearPoint = vxGraphics.GraphicsDevice.Viewport.Unproject(nearPoint,
                                                     Camera.Projection,
                                                     Camera.View,
                                                     Matrix.Identity);

            farPoint = vxGraphics.GraphicsDevice.Viewport.Unproject(farPoint,
                                                    Camera.Projection,
                                                    Camera.View,
                                                    Matrix.Identity);

            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            return new Ray(nearPoint, direction);
        }

        protected virtual float GetGizmoTypeRenderScale()
        {
            return 10;
        }

        protected virtual void UpdateAxis()
        {
            switch (AxisDirections)
            {
                case GizmoAxis.X:
                    MainAxis = Gizmo.Transform.Right;
                    PerpendicularAxis = Gizmo.Transform.Up;
                    break;
                case GizmoAxis.Y:
                    MainAxis = Gizmo.Transform.Up;
                    PerpendicularAxis = Gizmo.Transform.Backward;
                    break;
                case GizmoAxis.Z:
                    MainAxis = Gizmo.Transform.Backward;
                    PerpendicularAxis = Gizmo.Transform.Up;
                    break;
            }
        }

        protected internal override void OnWillDraw(vxCamera Camera)
        {
            SandboxCamera = Camera;
            RenderScale = Math.Abs(Vector3.Subtract(Position, Camera.Position).Length()) / GetGizmoTypeRenderScale();

            Transform.Scale = Vector3.One * RenderScale;

            base.OnWillDraw(Camera);
        }

        protected void OnGizmoSelection()
        {
            m_isFirstLoopSinceSelected = true;

            if (m_isFirstSelectionFrame)
            {
                m_isFirstSelectionFrame = false;
                prevTransforms.Clear();
                for (int i = 0; i < Scene.SelectedItems.Count; i++)
                {
                    prevTransforms.Add(Scene.SelectedItems[i].Transform.ToCopy());
                }
            }
        }

        protected void OnGizmoNotSelection()
        {
            m_isFirstSelectionFrame = true;
            if (m_isFirstLoopSinceSelected)
            {
                m_isFirstLoopSinceSelected = false;
                if (vxInput.IsNewMouseButtonRelease(MouseButtons.LeftButton))
                {
                    List<vxTransform> NewTransforms = new List<vxTransform>();
                    for (int i = 0; i < Scene.SelectedItems.Count; i++)
                    {
                        NewTransforms.Add(Scene.SelectedItems[i].Transform);
                    }

                    if (NewTransforms.Count > 0 && prevTransforms.Count > 0)
                    {
                        if (NewTransforms[0] != prevTransforms[0])
                        {
                            // then create an entry in the command manager of the delta which applies it
                            Scene.CommandManager.Add(
                                new vxCMDTransformChanged(Scene, Scene.SelectedItems, NewTransforms, prevTransforms));
                        }
                    }
                }
            }
        }
    }
}
