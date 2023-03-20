using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VerticesEngine.Net
{
    /// <summary>
    /// This holds all data needed to update the state of a multiplayer entity.
    /// </summary>
    public class vxNetEntityState
    {
        public Vector3 Position;
        public Quaternion Orientation;
        public Vector3 Velocity;
        public bool IsRightDown;
        public bool IsLeftDown;
        public bool IsThrustDown;
        public float TurnAmount;
        public float ThrustAmount;
    }
}
