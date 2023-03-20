using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace VerticesEngine.Plugins
{
    /// <summary>
    /// Plugin Metadata which holds id, name and type. This lets a player view plugin info in a dropdown without loading any further info about it
    /// </summary>
    [Serializable]
    public class vxPluginMetadata
    {
        [XmlAttribute]
        public string id = "";

        [XmlAttribute]
        public string name = "";

        [XmlAttribute]
        public vxPluginType PluginType = vxPluginType.Mod;

        public vxPluginMetadata()
        {
            id = "";
            name = "";
            PluginType = vxPluginType.Mod;
        }

        public vxPluginMetadata(vxIPlugin plugin)
        {
            id = plugin.ID;
            name = plugin.Name;
            PluginType = plugin.PluginType;
        }
    }
}
