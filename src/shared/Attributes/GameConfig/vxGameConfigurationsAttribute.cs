using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine
{
    /// <summary>
    /// This holds all configuration options for the game such as Name as well as is there Player Profile support, Network support etc...
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
    public class vxGameConfigurationsAttribute : Attribute
    {
        /// <summary>
        /// What is the Game's name
        /// </summary>
        public string GameName;

        /// <summary>
        /// Is this a 2D or 3D game?
        /// </summary>
        public vxGameEnviromentType GameType;

        /// <summary>
        /// Should the game be played in Portrait or Landscape mode? Mainly for Mobile games
        /// </summary>
        public vxOrientationType MainOrientation;

        /// <summary>
        /// Configuration Option flags for the game to operate under
        /// </summary>
        public vxGameConfigFlags ConfigOptions;

        /// <summary>
        /// This is the main game config which holds any and all information about this game.
        /// </summary>
        public vxGameConfigurationsAttribute()
        {

        }
    }
}
