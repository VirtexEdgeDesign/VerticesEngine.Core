using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using VerticesEngine.Settings;

namespace VerticesEngine.Serilization
{
    /// <summary>
    /// The Serializeable Boolean Setting Element for the Graphical Settings.
    /// </summary>
    public class vxGraphicalBaseBooleanSetting
    {
        /// <summary>
        /// Is this Graphical Element Enabled
        /// </summary>
        [XmlAttribute("IsEnabled")]
        public bool IsEnabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                if (EnabledStateChanged != null)
                    EnabledStateChanged(this, new EventArgs());
            }
        }
        bool _enabled;

        /// <summary>
        /// Fired when Enabled Changes.
        /// </summary>
        public event EventHandler<EventArgs> EnabledStateChanged;
        
        public vxGraphicalBaseBooleanSetting()
        {
            IsEnabled = true;
        }
    }
}
