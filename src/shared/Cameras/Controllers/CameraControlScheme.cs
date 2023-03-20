using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VerticesEngine.Controllers
{
    /// <summary>
    /// Superclass of implementations which control the behavior of a camera.
    /// </summary>
    public abstract class CameraControlScheme
    {
        /// <summary>
        /// Gets the game associated with the camera.
        /// </summary>
        public vxEngine Engine { get; private set; }

        /// <summary>
        /// Gets the camera controlled by this control scheme.
        /// </summary>
        public vxCamera3D Camera { get; private set; }

		protected CameraControlScheme(vxCamera3D camera, vxEngine Engine)
        {
            Camera = camera;
            this.Engine = Engine;
        }

        /// <summary>
        /// Updates the camera state according to the control scheme.
        /// </summary>
        /// <param name="dt">Time elapsed since previous frame.</param>
        public virtual void Update(float dt)
        {
            /*
            //Only turn if the mouse is controlled by the game.
            if (Engine._input.ShowCursor)
            {
                Camera.Yaw += ((int)Engine.Mouse_ClickPos.X - Engine.CurrentGameplayScreen.mouseInput.X) * dt * .12f;
                Camera.Pitch += ((int)Engine.Mouse_ClickPos.Y - Engine.CurrentGameplayScreen.mouseInput.Y) * dt * .12f;
            }
             */
        }
    }
}
