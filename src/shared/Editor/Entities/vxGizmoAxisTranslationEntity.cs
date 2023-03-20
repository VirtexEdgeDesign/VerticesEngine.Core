using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Commands;
using VerticesEngine.Diagnostics;
using VerticesEngine.Graphics;
using VerticesEngine.Input;

namespace VerticesEngine.Editor.Entities
{

    /// <summary>
    /// Axis Object for editing Sandbox Entity Position in the Sandbox Enviroment.
    /// </summary>
    public class vxGizmoAxisTranslationEntity : vxGizmoTransformationBaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:VerticesEngine.Entities.Util.vxGizmoAxisTranslationEntity"/> class.
        /// </summary>
        /// <param name="scene">Scene.</param>
        /// <param name="Gimbal">Gimbal.</param>
        /// <param name="AxisDirections">Axis directions.</param>
        public vxGizmoAxisTranslationEntity(vxGizmo Gimbal, GizmoAxis AxisDirections)
            : base(Gimbal.Scene, Gimbal, AxisDirections)
        {

        }

        protected override vxMesh OnLoadModel() { return vxInternalAssets.Models.UnitArrow; }

        protected override void UpdateAxis()
        {
            base.UpdateAxis();
            switch (AxisDirections)
            {
                case GizmoAxis.X:
                    MainAxis = Gizmo.Transform.Forward;
                    PerpendicularAxis = Gizmo.Transform.Up;
                    break;
                case GizmoAxis.Y:
                    MainAxis = Gizmo.Transform.Up;
                    PerpendicularAxis = Gizmo.Transform.Backward;
                    break;
                case GizmoAxis.Z:
                    MainAxis = Gizmo.Transform.Right;
                    PerpendicularAxis = Gizmo.Transform.Up;
                    break;
            }

            vxConsole.WriteToScreen($"Axis: {AxisDirections}", $"MainAxis: {MainAxis}", GetAxisColour());
        }

        protected internal override void Update()
        {
            //TODO: Why is this null?
            if (Scene == null)
                return;

            if (Scene.SandboxCurrentState == vxEnumSandboxStatus.EditMode)
            {

                base.Update();

                _lastIntersectionPosition = _intersectPosition;

                //Handle if Selected
                if (SelectionState == vxSelectionState.Selected && SandboxCamera != null)
                {
                    Ray ray = ConvertMouseToRay(vxInput.Cursor, SandboxCamera);

                    OnGizmoSelection();

                    if (FirstCheck)
                    {
                        FirstCheck = false;

                        SetStartPosition(Gizmo.Position);
                    }

                    // Let's get the transform of the gizmo currently
                    Matrix _rotationMatrix = Matrix.CreateFromQuaternion(Gizmo.Transform.Rotation);

                    // now let's work on the rotation seperate from the world transofmr
                    Matrix transform = Matrix.Invert(_rotationMatrix);
                    ray.Position = Vector3.Transform(ray.Position, transform);
                    ray.Direction = Vector3.TransformNormal(ray.Direction, transform);

                    Vector3 delta = Vector3.Zero;


                    // get the plane for this axis
                    if (AxisDirections == GizmoAxis.X || AxisDirections == GizmoAxis.Z)
                    {
                        Plane plane = new Plane(-Vector3.Up, Vector3.Transform(this.Position, Matrix.Invert(_rotationMatrix)).Y);

                        float? intersection = ray.Intersects(plane);
                        if (intersection.HasValue)
                        {
                            _intersectPosition = (ray.Position + (ray.Direction * intersection.Value));
                            if (_lastIntersectionPosition != Vector3.Zero && _isFirstSelection == false)
                            {
                                var _tDelta = _intersectPosition - _lastIntersectionPosition;

                                delta = AxisDirections == GizmoAxis.Z ? new Vector3(_tDelta.X, 0, 0) : new Vector3(0, 0, _tDelta.Z);
                            }


                            if (_isFirstSelection)
                            {
                                _isFirstSelection = false;
                                _lastIntersectionPosition = _intersectPosition;
                            }
                        }
                    }
                    else if (AxisDirections == GizmoAxis.Y)
                    {
                        Plane plane = new Plane(Vector3.Forward, Vector3.Transform(this.Position, Matrix.Invert(_rotationMatrix)).Z);

                        float? intersection = ray.Intersects(plane);
                        if (intersection.HasValue)
                        {
                            _lastIntersectionPosition = _intersectPosition;
                            _intersectPosition = (ray.Position + (ray.Direction * intersection.Value));
                            if (_lastIntersectionPosition != Vector3.Zero && _isFirstSelection == false)
                            {
                                var _tDelta = _intersectPosition - _lastIntersectionPosition;

                                delta = new Vector3(0, _tDelta.Y, 0);
                            }


                            if (_isFirstSelection)
                            {
                                _lastIntersectionPosition = _intersectPosition;
                                _isFirstSelection = false;
                            }
                        }
                    }

                    delta = Vector3.Transform(delta, _rotationMatrix);

                    for (int i = 0; i < Scene.SelectedItems.Count; i++)
                    {
                        var entity = Scene.SelectedItems[i];
                        entity.Transform.Position = entity.Transform.Position + (delta);
                    }

                }
                else
                {
                    _isFirstSelection = true;
                    OnGizmoNotSelection();
                }
            }
        }
    }
}