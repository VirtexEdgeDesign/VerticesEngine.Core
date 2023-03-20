using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using VerticesEngine.Commands;
using VerticesEngine.Diagnostics;
using VerticesEngine.Graphics;
using VerticesEngine.Input;

namespace VerticesEngine.Editor.Entities
{

    /// <summary>
    /// Axis Object for editing Sandbox Entity Position in the Sandbox Enviroment.
    /// </summary>
    public class vxGizmoHotKeyTranslationEntity : vxGizmoTransformationBaseEntity
    {
        VertexPositionColor[] Vertices;

        BasicEffect _quadEffect;

        const GizmoAxis DefaultAxis = GizmoAxis.None;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:VerticesEngine.Entities.Util.vxGizmoAxisTranslationEntity"/> class.
        /// </summary>
        /// <param name="scene">Scene.</param>
        /// <param name="Gimbal">Gimbal.</param>
        /// <param name="AxisDirections">Axis directions.</param>
        public vxGizmoHotKeyTranslationEntity(vxGizmo Gimbal)
            : base(Gimbal.Scene, Gimbal, DefaultAxis)
        {

            Vertices = new VertexPositionColor[4];

            // Set the position and texture coordinate for each
            // vertex
            float size = 1000;
            Vertices[0].Position = new Vector3(0, 0, -size);
            Vertices[1].Position = new Vector3(0, 0, 0);
            Vertices[2].Position = new Vector3(0, 0, 0);
            Vertices[3].Position = new Vector3(0, 0, size);

            for (int v = 0; v < Vertices.Length; v++)
                Vertices[v].Color = Color.White;

            _quadEffect = new BasicEffect(vxGraphics.GraphicsDevice);
            _quadEffect.VertexColorEnabled = true;
            _quadEffect.AmbientLightColor = Vector3.One;
            _quadEffect.DiffuseColor = Vector3.One;
        }

        protected override vxMesh OnLoadModel() { return vxInternalAssets.Models.UnitSphere; }


        public bool IsInGrabMode
        {
            get { return m_isInGrabMode; }
            set
            {
                m_isInGrabMode = value;

                if(m_isInGrabMode)
                {

                }
                else
                {
                    // always reset the axis on exit
                    AxisDirections = DefaultAxis;
                    OnGrabCanceled();
                }
            }
        }
        public bool m_isInGrabMode = false;


        protected internal override void PostUpdate()
        {
            base.PostUpdate();

            IsVisible = false;// IsInGrabMode;
            EditorHandleScale = 0.0001f;
            // TODO: Fix
            //Transform.Scale = Matrix.CreateScale(EditorHandleScale * vxGizmo.ScreenSpaceZoomFactor / (Gizmo.scale)) *
            //    Matrix.CreateWorld(Gizmo.Position, MainAxis, PerpendicularAxis);
        }


        public override void RenderOverlayMesh(vxCamera3D Camera)
        {
            //_quadEffect.AmbientLightColor = PlainColor.ToVector3();
            //_quadEffect.DiffuseColor = PlainColor.ToVector3();


            //_quadEffect.World = Matrix.CreateScale(RenderScale * 1 / EditorHandleScale) * Transform.Matrix4x4Transform;
            //_quadEffect.View = Camera.View;
            //_quadEffect.Projection = Camera.Projection;
            //_quadEffect.CurrentTechnique.Passes[0].Apply();

            //vxGraphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, Vertices, 0, 2);
        }


        protected void OnGrabFinished()
        {
            m_isFirstSelectionFrame = true;
            if (m_isFirstLoopSinceSelected)
            {
                m_isFirstLoopSinceSelected = false;
                if (vxInput.IsNewMouseButtonRelease(MouseButtons.LeftButton))
                {
                    
                }
            }
        }

        protected void OnGrabCanceled()
        {
            m_isFirstSelectionFrame = true;
            if (m_isFirstLoopSinceSelected)
            {
                m_isFirstLoopSinceSelected = false;

                for (int i = 0; i < Scene.SelectedItems.Count; i++)
                {
                    var entity = Scene.SelectedItems[i];
                    entity.Transform = prevTransforms[i];
                }
            }
        }

        protected override void UpdateAxis()
        {
            switch (AxisDirections)
            {
                case GizmoAxis.X:
                    MainAxis = Gizmo.Transform.Forward;
                    PerpendicularAxis = Gizmo.Transform.Up;
                    break;
                case GizmoAxis.Y:
                case GizmoAxis.None:
                    MainAxis = Gizmo.Transform.Up;
                    PerpendicularAxis = Gizmo.Transform.Backward;
                    break;
                case GizmoAxis.Z:
                    MainAxis = Gizmo.Transform.Right;
                    PerpendicularAxis = Gizmo.Transform.Up;
                    break;
            }

        }

        protected internal override void Update()
        {
            if (Scene == null)
                return;

            if (vxInput.IsNewKeyPress(Keys.G))
            {
                if (IsInGrabMode == false)
                {
                    IsInGrabMode = true;
                    vxConsole.WriteLine("Is In Grab Mode");
                }
            }

            //else if (vxInput.IsNewKeyPress(Keys.Escape))
            //{
            //    if (IsInGrabMode)
            //    {
            //        IsInGrabMode = false;
            //        vxConsole.WriteLine("Exiting Grab Mode");
            //        OnGrabCanceled();
            //    }
            //}

            if (IsInGrabMode)
            {
                // calculate the camera plane based off of the initial center pos
                if(vxInput.IsNewKeyPress(Keys.X))
                {
                    AxisDirections = GizmoAxis.X;
                }
                if (vxInput.IsNewKeyPress(Keys.Y))
                {
                    AxisDirections = GizmoAxis.Y;
                }
                if (vxInput.IsNewKeyPress(Keys.Z))
                {
                    AxisDirections = GizmoAxis.Z;
                }
                if (vxInput.IsNewKeyPress(Keys.G))
                {
                    AxisDirections = GizmoAxis.None;
                }

                PlainColor = GetAxisColour();
            }


            if (Scene.SandboxCurrentState == vxEnumSandboxStatus.EditMode)
            {

                base.Update();


                //Handle if Selected
                if (IsInGrabMode && SandboxCamera != null)
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
                    else if (AxisDirections == GizmoAxis.None)
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

                    delta = Vector3.Transform(delta, _rotationMatrix);

                    for (int i = 0; i < Scene.SelectedItems.Count; i++)
                    {
                        var entity = Scene.SelectedItems[i];
                        entity.Transform.Position = prevTransforms[i].Position + (delta);
                    }

                    if(vxInput.IsNewMouseButtonPress(MouseButtons.LeftButton))
                    {
                        _isFirstSelection = true;


                        m_isFirstSelectionFrame = true;
                        if (m_isFirstLoopSinceSelected)
                        {
                            m_isFirstLoopSinceSelected = false;
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

                        IsInGrabMode = false;
                    }

                }
                else
                {
                    _isFirstSelection = true;
                }
            }
        }
    }
}