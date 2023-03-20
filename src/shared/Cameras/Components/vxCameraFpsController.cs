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
    /// Handles Camera FPS Controls
    /// </summary>
    public class vxCameraFpsController : vxComponent
    {
        private vxCamera3D Camera;

        private Point _mouseCenterPosition;

        protected override void Initialise()
        {
            base.Initialise();

            Camera = (vxCamera3D)Entity;

            _mouseCenterPosition = new Point(Camera.Viewport.Width / 2, Camera.Viewport.Height / 2);
        }

        protected internal override void OnEnabled()
        {
            base.OnEnabled();
            CenterMouseForFPS();
        }

        protected internal override void Update()
        {
            //Only move around if the camera has control over its own position.
            float dt = vxTime.DeltaTime;

            // Set Yaw and Pitch based on delta with previous frames cursor positions
            Camera.Yaw += (_mouseCenterPosition.X - vxInput.Cursor.X) * dt * .12f;
            Camera.Pitch += (_mouseCenterPosition.Y - vxInput.Cursor.Y) * dt * .12f;

            // Set Wolrd Matrices
            Camera.WorldMatrix = Matrix.CreateFromAxisAngle(Vector3.Right, Camera.Pitch) * Matrix.CreateFromAxisAngle(Vector3.Up, Camera.Yaw);
            Camera.WorldMatrix = Camera.WorldMatrix * Matrix.CreateTranslation(Camera.Position);
            Camera.View = Matrix.Invert(Camera.WorldMatrix);

            // Finally, recenter the mouse
            CenterMouseForFPS();
        }


        /// <summary>
        /// Centers the mouse for FPS.
        /// </summary>
        public void CenterMouseForFPS()
        {
            vxInput.Cursor = _mouseCenterPosition.ToVector2();
        }

    }
}
