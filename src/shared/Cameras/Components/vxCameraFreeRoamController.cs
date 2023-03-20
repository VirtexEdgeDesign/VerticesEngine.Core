using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using VerticesEngine.Input;
using VerticesEngine;

namespace VerticesEngine
{
    /// <summary>
    /// Handles Camera Free Roam Controls
    /// </summary>
    public class vxCameraFreeRoamController : vxComponent
    {
        private vxCamera3D Camera;


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

            Camera = (vxCamera3D)Entity;
        }

        protected internal override void Update()
        {
            //Only move around if the camera has control over its own position.
            float dt = vxTime.DeltaTime;

            float distance = Speed * dt / 4;
            if (vxInput.IsNewMouseButtonPress(MouseButtons.MiddleButton))
            {
                vxInput.MouseClickPos = vxInput.Cursor;//new Vector2(vxInput.Cursor.X, vxInput.Cursor.Y);
            }

            if (vxInput.MouseState.MiddleButton == ButtonState.Pressed)
            {
                Camera.ReqYaw += ((int)vxInput.MouseClickPos.X - vxInput.Cursor.X) * dt * .12f;
                Camera.ReqPitch += ((int)vxInput.MouseClickPos.Y - vxInput.Cursor.Y) * dt * .12f;
                Mouse.SetPosition((int)vxInput.MouseClickPos.X, (int)vxInput.MouseClickPos.Y);

            }
            //Camera.Yaw = Camera.ReqYaw;
            //Camera.Pitch = Camera.ReqPitch;

            Camera.Yaw = vxMathHelper.Smooth(Camera.Yaw, Camera.ReqYaw, 4);
            Camera.Pitch = vxMathHelper.Smooth(Camera.Pitch, Camera.ReqPitch, 4);


            Camera.WorldMatrix = Matrix.CreateFromAxisAngle(Vector3.Right, Camera.Pitch) * Matrix.CreateFromAxisAngle(Vector3.Up, Camera.Yaw);

            if (vxInput.KeyboardState.IsKeyDown(Keys.W))
                MoveForward(distance);
            if (vxInput.KeyboardState.IsKeyDown(Keys.S))
                MoveForward(-distance);
            if (vxInput.KeyboardState.IsKeyDown(Keys.A))
                MoveRight(-distance);
            if (vxInput.KeyboardState.IsKeyDown(Keys.D))
                MoveRight(distance);
            if (vxInput.KeyboardState.IsKeyDown(Keys.Q))
                Speed += 10;
            if (vxInput.KeyboardState.IsKeyDown(Keys.Z))
                Speed -= 10;


            Speed = Math.Max(0, Speed);

            Camera.WorldMatrix = Camera.WorldMatrix * Matrix.CreateTranslation(Camera.Position);
            Camera.View = Matrix.Invert(Camera.WorldMatrix);
        }


        /// <summary>
        /// Moves the camera forward.
        /// </summary>
        /// <param name="distance">Distance to move.</param>
        public void MoveForward(float distance)
        {
            Camera.Position += Camera.WorldMatrix.Forward * distance;
        }

        /// <summary>
        /// Moves the camera to the right.
        /// </summary>
        /// <param name="distance">Distance to move.</param>
        public void MoveRight(float distance)
        {
            Camera.Position += Camera.WorldMatrix.Right * distance;
        }

        /// <summary>
        /// Moves the camera up.
        /// </summary>
        /// <param name="distance">Distanec to move.</param>
        public void MoveUp(float distance)
        {
            Camera.Position += new Vector3(0, distance, 0);
        }
    }
}
