using Microsoft.Xna.Framework;
using System.Collections.Generic;
using VerticesEngine.Commands;
using VerticesEngine.Graphics;
using VerticesEngine.Input;

namespace VerticesEngine.Editor.Entities
{
    /// <summary>
    /// Axis Object for editing Sandbox Entity Position in the Sandbox Enviroment.
    /// </summary>
    public class vxGizmoAxisRotationEntity : vxGizmoTransformationBaseEntity
    {
        float rotationAngle = 0;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:VerticesEngine.Entities.Util.vxGizmoAxisRotationEntity"/> class.
        /// </summary>
        /// <param name="scene">Scene.</param>
        /// <param name="gizmo">Gimbal.</param>
        /// <param name="AxisDirections">Axis directions.</param>
        public vxGizmoAxisRotationEntity(vxGizmo gizmo, GizmoAxis AxisDirections)
            : base(gizmo.Scene, gizmo, AxisDirections)
        {
            
            Transform.Scale = Vector3.One * 12;
            EditorHandleScale = 1.25f;
        }

        protected override vxMesh OnLoadModel() { return vxInternalAssets.Models.UnitTorus; }

        protected override void UpdateAxis()
        {
            base.UpdateAxis();

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

        protected override float GetGizmoTypeRenderScale()
        {
            return 7.5f;
        }

        protected internal override void Update()
        {
            base.Update();

            //Handle if Selected
            if (SelectionState == vxSelectionState.Selected)
            {
                OnGizmoSelection();

                rotationAngle = (vxInput.Cursor.Y - vxInput.PreviousCursor.Y) / 100;

                Matrix rot = Matrix.Identity;

                MainAxis.Normalize();

                Vector3 rotAxis = Vector3.Zero;
                Quaternion rotQ = Quaternion.Identity;
                if (Gizmo.TransformationType == TransformationType.Global)
                {

                    switch (AxisDirections)
                    {
                        case GizmoAxis.X:
                            rot = Matrix.CreateRotationX(rotationAngle);
                            break;
                        case GizmoAxis.Y:
                            rot = Matrix.CreateRotationY(rotationAngle);
                            break;
                        case GizmoAxis.Z:
                            rot = Matrix.CreateRotationZ(rotationAngle);
                            break;
                    }
                    for (int i = 0; i < Scene.SelectedItems.Count; i++)
                    {
                        var entity = Scene.SelectedItems[i];

                        entity.Transform.Rotation = Quaternion.CreateFromRotationMatrix(rot) * entity.Transform.Rotation;
                    }
                }
                else if (Gizmo.TransformationType == TransformationType.Local)
                {
                    for (int i = 0; i < Scene.SelectedItems.Count; i++)
                    {
                        var entity = Scene.SelectedItems[i];

                        switch (AxisDirections)
                        {
                            case GizmoAxis.X:
                                rotAxis = Vector3.UnitX;
                                break;
                            case GizmoAxis.Y:
                                rotAxis = Vector3.UnitY;
                                break;
                            case GizmoAxis.Z:
                                rotAxis = Vector3.UnitZ;
                                break;
                        }
                        rotAxis.Normalize();

                        rotQ = Quaternion.CreateFromAxisAngle(rotAxis, rotationAngle);
                        entity.Transform.Rotation *= rotQ;
                    }
                }
            }
            else
            {
                OnGizmoNotSelection();
            }
        }
    }
}