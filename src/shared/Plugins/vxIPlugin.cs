using System;
using System.Collections.Generic;
using System.Text;
using VerticesEngine;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;

namespace VerticesEngine.Plugins
{
    public enum vxPluginType
    {
        DLC = 0,

        Mod = 1
    }

    /// <summary>
    /// The Plugin Interface for creating DLC and Mod packs for Vertices Engine Games. DLC and Mods are treated very similar
    /// for ease of use and production.
    /// </summary>
    public interface vxIPlugin
    {
        /// <summary>
        /// Plugin Readable Name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Plugin Description
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        string ID { get; }
        bool IsLoaded { get; }

        /// <summary>
        /// Gets the type of the plugin.
        /// </summary>
        /// <value>The type of the plugin.</value>
        vxPluginType PluginType { get; }

        /// <summary>
        /// The main sprite sheet for this content pack.
        /// </summary>
        Texture2D MainSpriteSheet { get; }

        /// <summary>
        /// Initialises this Plugin/DLC pack
        /// </summary>
        /// <param name="Engine"></param>
        void Initialise();


        /// <summary>
        /// Loads the Assets for the this Content Pack. This is called as a coroutine so as to not block the main thread
        /// </summary>
        IEnumerator LoadContent();

    }
}
