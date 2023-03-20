using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.Plugins.Mods
{

    public struct vxModItemStatus
    {
        /// <summary>
        /// The name of this mod
        /// </summary>
        public string Name;

        /// <summary>
        /// The path to this mod
        /// </summary>
        public string Path;

        /// <summary>
        /// Is this mod enabled?
        /// </summary>
        public bool IsEnabled;
    }
}
