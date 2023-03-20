using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Text;
using BEPUphysics.UpdateableSystems.ForceFields;
using VerticesEngine.Diagnostics;
using VerticesEngine.Graphics;
using VerticesEngine.Input;
using VerticesEngine;

namespace VerticesEngine
{
    /// <summary>
    /// Handles Camera Free Roam Controls
    /// </summary>
    public class vxCameraSceneEditorController : vxComponent
    {
        private vxCamera3D _camera;


        /// <summary>
        /// Gets or sets the speed at which the camera moves for freem roaming.
        /// </summary>
        public float Speed
        {
            get { return _speed; }
            set { _speed = Math.Max(value, 0);
            }
        }
        private float _speed = 10;

        protected override void Initialise()
        {
            base.Initialise();

            _camera = (vxCamera3D)Entity;
        }


        /// <summary>
        /// Check if we should wrap the mouse
        /// </summary>
        void CheckMouseOutOfBoundsWrap()
        {
            if(vxInput.IsMouseButtonDown())
            {
                var borderPadding = 10;
                var newCursor = vxInput.Cursor - vxInput.PreviousCursor;
                // Handle Top
                if (vxInput.Cursor.Y <= vxGraphics.GraphicsDevice.Viewport.Bounds.Top + borderPadding)
                {
                    // get the current difference
                    vxInput.Cursor = new Vector2(vxInput.Cursor.X, vxGraphics.GraphicsDevice.Viewport.Bounds.Bottom - borderPadding) + newCursor;
                    vxInput.PreviousCursor = vxInput.Cursor;
                }

                // Handle Right
                if (vxInput.Cursor.Y >= vxGraphics.GraphicsDevice.Viewport.Bounds.Bottom - borderPadding)
                {
                    vxInput.Cursor = new Vector2(vxInput.Cursor.X, vxGraphics.GraphicsDevice.Viewport.Bounds.Top + borderPadding) + newCursor;
                    vxInput.PreviousCursor = vxInput.Cursor;
                }

                // Handle Left
                if (vxInput.Cursor.X <= vxGraphics.GraphicsDevice.Viewport.Bounds.Left  + borderPadding)
                {
                    vxInput.Cursor = new Vector2(vxGraphics.GraphicsDevice.Viewport.Bounds.Right - borderPadding, (int)vxInput.Cursor.Y) + newCursor;
                    vxInput.PreviousCursor = vxInput.Cursor;
                }

                // Handle Right
                if (vxInput.Cursor.X >= vxGraphics.GraphicsDevice.Viewport.Bounds.Right - borderPadding)
                {
                    vxInput.Cursor = new Vector2(0 + borderPadding, (int)vxInput.Cursor.Y) + newCursor;
                    vxInput.PreviousCursor = vxInput.Cursor;
                }
            }
        }


        protected internal override void Update()
        {
            if (vxEngine.Instance.CurrentScene.IsActive)
            {
                //Only move around if the camera has control over its own position.
                float dt = vxTime.DeltaTime;

                float distance = Speed * dt / 4;

                CheckMouseOutOfBoundsWrap();

                if (vxEngine.Instance.CurrentScene.UIManager.HasFocus == false)
                {
                    #region Panning

                    if (vxInput.MouseState.MiddleButton == ButtonState.Pressed)
                    {

                        var panScale = Math.Max(movementMultiplier() / 50, 3);

                        _camera.Position -= _camera.WorldMatrix.Up * ((int)vxInput.PreviousCursor.Y - vxInput.Cursor.Y) * dt * panScale;
                        _camera.Position += _camera.WorldMatrix.Right * ((int)vxInput.PreviousCursor.X - vxInput.Cursor.X) * dt * panScale;

                    }

                    #endregion

                    #region Rotation

                    if (vxInput.MouseState.RightButton == ButtonState.Pressed && vxInput.IsKeyDown(Keys.LeftShift) == false)
                    {
                        _camera.ReqYaw += ((int)vxInput.PreviousCursor.X - vxInput.Cursor.X) * dt * .12f;
                        _camera.ReqPitch += ((int)vxInput.PreviousCursor.Y - vxInput.Cursor.Y) * dt * .12f;
                    }

                    #endregion

                    #region Zoom 

                    if (!vxInput.IsKeyDown(Keys.LeftShift) && !vxInput.IsKeyDown(Keys.LeftControl))
                    {
                        if (vxInput.ScrollWheelDelta != 0)
                        {
                            _camera.Position += _camera.WorldMatrix.Forward * vxInput.ScrollWheelDelta * 0.125f;
                        }
                    }
                    #endregion
                }
                _camera.Yaw = vxMathHelper.Smooth(_camera.Yaw, _camera.ReqYaw, 2);
                _camera.Pitch = vxMathHelper.Smooth(_camera.Pitch, _camera.ReqPitch, 2);


                _camera.WorldMatrix = Matrix.CreateFromAxisAngle(Vector3.Right, _camera.Pitch) * Matrix.CreateFromAxisAngle(Vector3.Up, _camera.Yaw);
                               
                Speed = Math.Max(0, Speed);

                _camera.WorldMatrix = _camera.WorldMatrix * Matrix.CreateTranslation(_camera.Position);
                _camera.View = Matrix.Invert(_camera.WorldMatrix);
            }
            

            if (vxInput.IsKeyDown(Keys.F) && isSettingSmoothPosition == false)
            {
                FrameToSelectedObject();
            }
        }

        /// <summary>
        /// Smooths the camera to frame the target object as a coroutine
        /// </summary>
        /// <returns></returns>
        public IEnumerator SmoothToFramePosition()
        {
            float smoothDT = smoothResponseTime - currentSmoothTime;

            while (smoothDT > 0)
            {
                Vector3 requiredDistance = targetPosition - _camera.Position;

                // the speed at which we need to go in the amount of time we have left
                Vector3 smoothVelocity = requiredDistance / smoothDT;

                _camera.Position += smoothVelocity * vxTime.DeltaTime;

                currentSmoothTime += vxTime.DeltaTime;
                smoothDT = smoothResponseTime - currentSmoothTime;
                yield return smoothDT;
            }
            isSettingSmoothPosition = false;
                yield return null;
        }

        float movementMultiplier()
        {
            return Math.Max(1, (currentCenter - _camera.Position).Length());
        }

        Vector3 currentCenter = Vector3.Zero;

        // are we currently setting a smooth position?
        private bool isSettingSmoothPosition = false;

        // what is the time in which we should get there in?
        private float smoothResponseTime = 0.25f;

        private float currentSmoothTime = 0;

        private Vector3 targetPosition;

        public void SetPositionSmooth(Vector3 position)
        {
            isSettingSmoothPosition = true;
            currentSmoothTime = 0;
            targetPosition = position;
            vxCoroutineManager.Instance.StartCoroutine(SmoothToFramePosition());
        }


        public void FrameToSelectedObject()
        {
            var Scene = ((vxGameplayScene3D)vxEngine.Instance.CurrentScene);
            
            if (Scene.SelectedItems.Count > 0)
            {
                BoundingSphere sphere = Scene.SelectedItems[0].BoundingShape;

                // merge the bounding spheres for all selected items
                for(int s = 1; s < Scene.SelectedItems.Count; s++)
                {
                    sphere = BoundingSphere.CreateMerged(sphere, Scene.SelectedItems[s].BoundingShape);
                }

                // get the view vector as the current camera position - the current piece position and normalise that vector
                Vector3 viewVector = _camera.WorldMatrix.Forward;

                viewVector.Normalize();
                currentCenter = sphere.Center;
                
                SetPositionSmooth(sphere.Center - viewVector * Math.Max(sphere.Radius, 10) * 1.5f);
            }
        }

        /// <summary>
        /// Moves the camera forward.
        /// </summary>
        /// <param name="distance">Distance to move.</param>
        public void MoveForward(float distance)
        {
            _camera.Position += _camera.WorldMatrix.Forward * distance;
        }

        /// <summary>
        /// Moves the camera to the right.
        /// </summary>
        /// <param name="distance">Distance to move.</param>
        public void MoveRight(float distance)
        {
            _camera.Position += _camera.WorldMatrix.Right * distance;
        }

        /// <summary>
        /// Moves the camera up.
        /// </summary>
        /// <param name="distance">Distance to move.</param>
        public void MoveUp(float distance)
        {
            _camera.Position += new Vector3(0, distance, 0);
        }
    }
}
