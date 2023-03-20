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
    /// Handles Camera Orbit Controls
    /// </summary>
    public class vxCameraOrbitController : vxComponent
    {
        private vxCamera3D Camera;

        protected override void Initialise()
        {
            base.Initialise();

            Camera = (vxCamera3D)Entity;
        }

        protected internal override void Update()
        {
            if (vxInput.IsNewMouseButtonPress(MouseButtons.RightButton))
            {
                vxInput.MouseClickPos = vxInput.Cursor;//new Vector2(vxInput.Cursor.X, vxInput.Cursor.Y);
            }

            if (vxInput.MouseState.RightButton == ButtonState.Pressed)
            {
                
                // Get the time difference
                float dt = vxTime.DeltaTime;

                // Get the Requi
                if (Camera.CanTakeInput)
                {
                    Camera.ReqYaw += ((int)vxInput.MouseClickPos.X - vxInput.Cursor.X) * dt * .12f;
                    Camera.ReqPitch += ((int)vxInput.MouseClickPos.Y - vxInput.Cursor.Y) * dt * .12f;
                    Mouse.SetPosition((int)vxInput.MouseClickPos.X, (int)vxInput.MouseClickPos.Y);
                }
                Camera.Yaw = Camera.ReqYaw;
                Camera.Pitch = Camera.ReqPitch;
            }


            if (vxEngine.Instance.CurrentScene.UIManager.HasFocus == false)
                Camera.OrbitZoom += vxInput.ScrollWheelDelta;


            Camera.OrbitZoom = Math.Max(Camera.OrbitZoom, 15);

            Camera.Yaw = vxMathHelper.Smooth(Camera.Yaw, Camera.ReqYaw, 8);
            Camera.Pitch = vxMathHelper.Smooth(Camera.Pitch, Camera.ReqPitch, 8);

            Camera.Zoom = vxMathHelper.Smooth(Camera.Zoom, Camera.OrbitZoom, 14);

            Camera.WorldMatrix = Matrix.CreateTranslation(new Vector3(0, 0, (Camera.Zoom) / 200));

            Camera.WorldMatrix *= Matrix.CreateFromAxisAngle(Vector3.Right, Camera.Pitch) * Matrix.CreateFromAxisAngle(Vector3.Up, Camera.Yaw);

            Camera.WorldMatrix *= Matrix.CreateTranslation(Camera.OrbitTarget);

            if (Camera.ProjectionType == vxCameraProjectionType.Orthographic)
                Camera.CalculateProjectionMatrix();

            Camera.View = Matrix.Invert(Camera.WorldMatrix);
            Camera.Position = Camera.WorldMatrix.Translation;
        }
    }
}
