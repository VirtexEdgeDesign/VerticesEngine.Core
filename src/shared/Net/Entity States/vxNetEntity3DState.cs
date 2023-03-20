using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Storage;

namespace VerticesEngine.Net
{

    /// <summary>
    /// This holds all data needed to update the state of a multiplayer entity.
    /// </summary>
    public class vxNetEntity3DState
    {
        /// <summary>
        /// Returns the 
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// Rotation
        /// </summary>
        public float Rotation;

        /// <summary>
        /// The Current Velocity of the Entity. This isn't overly useful if the player is accelerating. 
        /// </summary>
        public Vector3 Velocity;

        /// <summary>
        /// Gets or Sets the World Orientation of this entity.
        /// </summary>
        public Matrix World;


        /// <summary>
        ///
        /// </summary>
        public bool IsThrustDown = false;
        public bool IsForwardDown = false;
        public bool IsBackDown = false;
        public bool IsLeftDown = false;
        public bool IsRightDown = false;

        public float TurnAmount = 0;

        public float ThrustAmount = 0;


        public vxNetEntity3DState()
        {

        }
    }
}
