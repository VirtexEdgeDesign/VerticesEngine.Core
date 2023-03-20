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
    public class vxGizmoPanTranslationEntity : vxGizmoTransformationBaseEntity
    {
        VertexPositionColor[] Vertices;

        BasicEffect _quadEffect;


        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:VerticesEngine.Entities.Util.vxGizmoAxisTranslationEntity"/> class.
        /// </summary>
        /// <param name="scene">Scene.</param>
        /// <param name="Gimbal">Gimbal.</param>
        /// <param name="AxisDirections">Axis directions.</param>
        public vxGizmoPanTranslationEntity(vxGizmo Gimbal, GizmoAxis AxisDirections)
            : base(Gimbal.Scene, Gimbal, AxisDirections)
        {
            EditorHandleScale = 0.33f;

            Vertices = new VertexPositionColor[4];

            // Set the position and texture coordinate for each
            // vertex
            Vertices[0].Position = new Vector3(0, 0, 0);
            Vertices[1].Position = new Vector3(0, 0, -1);
            Vertices[2].Position = new Vector3(1, 0, -1);
            Vertices[3].Position = new Vector3(1, 0, 0);

            for (int v = 0; v < Vertices.Length; v++)
                Vertices[v].Color = Color.White;

            _quadEffect = new BasicEffect(vxGraphics.GraphicsDevice);
            _quadEffect.VertexColorEnabled = true;
            _quadEffect.AmbientLightColor = Vector3.One;
            _quadEffect.DiffuseColor = Vector3.One;

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

        public override void RenderOverlayMesh(vxCamera3D Camera)
        {
            if (Scene.SandboxCurrentState == vxEnumSandboxStatus.EditMode)
            {
                base.RenderOverlayMesh(Camera);

                _quadEffect.AmbientLightColor = PlainColor.ToVector3();
                _quadEffect.DiffuseColor = PlainColor.ToVector3();

                _quadEffect.World = Transform.Matrix4x4Transform;
                _quadEffect.View = Camera.View;
                _quadEffect.Projection = Camera.Projection;
                _quadEffect.CurrentTechnique.Passes[0].Apply();

                vxGraphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, Vertices, 0, 3);
            }
        }

        protected override vxMesh OnLoadModel() { return vxInternalAssets.Models.UnitPlanePan; }

        protected override float GetGizmoTypeRenderScale()
        {
            return 25;
        }

        protected override void UpdateAxis()
        {
            base.UpdateAxis();
            switch (AxisDirections)
            {
                case GizmoAxis.YZ:
                    MainAxis = Gizmo.Transform.Up;
                    PerpendicularAxis = Gizmo.Transform.Right;
                    break;
                case GizmoAxis.ZX:
                    MainAxis = Gizmo.Transform.Forward;
                    PerpendicularAxis = Gizmo.Transform.Up;
                    break;
                case GizmoAxis.XY:
                    MainAxis = Gizmo.Transform.Up;
                    PerpendicularAxis = Gizmo.Transform.Backward;
                    break;
            }
        }

        protected internal override void Update()
        {
            //TODO: Why is this null?
            if (Scene == null)
                return;

            if (Scene.SandboxCurrentState == vxEnumSandboxStatus.EditMode)
            {
                PlainColor = GetAxisColour();

                EditorEntityMaterial.SetEffectParameter("NormalColour", PlainColor.ToVector3());
                EditorEntityMaterial.SetEffectParameter("Alpha", 0.2f);

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
                    Matrix _rotationMatrix = Matrix.CreateFromQuaternion(Quaternion.CreateFromRotationMatrix(Gizmo.Transform.Matrix4x4Transform));

                    // now let's work on the rotation seperate from the world transofmr
                    Matrix transform = Matrix.Invert(_rotationMatrix);
                    ray.Position = Vector3.Transform(ray.Position, transform);
                    ray.Direction = Vector3.TransformNormal(ray.Direction, transform);

                    Vector3 delta = Vector3.Zero;

                    switch (AxisDirections)
                    {
                        // A plane with it's normal in the 'Y' direction
                        case GizmoAxis.ZX:
                            {
                                Plane plane = new Plane(-Vector3.Up, Vector3.Transform(this.Position, Matrix.Invert(_rotationMatrix)).Y);

                                float? intersection = ray.Intersects(plane);
                                if (intersection.HasValue)
                                {
                                    _intersectPosition = (ray.Position + (ray.Direction * intersection.Value));
                                    if (_lastIntersectionPosition != Vector3.Zero && _isFirstSelection == false)
                                    {
                                        var _tDelta = _intersectPosition - _lastIntersectionPosition;

                                        delta = new Vector3(_tDelta.X, 0, _tDelta.Z);
                                    }


                                    if (_isFirstSelection)
                                    {
                                        _isFirstSelection = false;
                                        _lastIntersectionPosition = _intersectPosition;
                                    }
                                }
                            }
                            break;
                        case GizmoAxis.YZ:
                            {
                                Plane plane = new Plane(Vector3.Left, Vector3.Transform(this.Position, Matrix.Invert(_rotationMatrix)).X);

                                float? intersection = ray.Intersects(plane);
                                if (intersection.HasValue)
                                {
                                    _lastIntersectionPosition = _intersectPosition;
                                    _intersectPosition = (ray.Position + (ray.Direction * intersection.Value));
                                    if (_lastIntersectionPosition != Vector3.Zero && _isFirstSelection == false)
                                    {
                                        var _tDelta = _intersectPosition - _lastIntersectionPosition;

                                        delta = new Vector3(0, _tDelta.Y, _tDelta.Z);
                                    }


                                    if (_isFirstSelection)
                                    {
                                        _lastIntersectionPosition = _intersectPosition;
                                        _isFirstSelection = false;
                                    }
                                }
                            }
                            break;
                        case GizmoAxis.XY:
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

                                        delta = new Vector3(_tDelta.X, _tDelta.Y, 0);
                                    }


                                    if (_isFirstSelection)
                                    {
                                        _lastIntersectionPosition = _intersectPosition;
                                        _isFirstSelection = false;
                                    }
                                }
                            }
                            break;
                    }

                    delta = Vector3.Transform(delta, _rotationMatrix);

                    for (int i = 0; i < Scene.SelectedItems.Count; i++)
                    {
                        var entity = Scene.SelectedItems[i];
                        entity.Position = entity.Position + (delta);
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