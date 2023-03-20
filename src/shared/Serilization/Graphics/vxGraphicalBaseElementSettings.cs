using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using VerticesEngine.Settings;
/*
namespace VerticesEngine.Serilization
{
    /// <summary>
    /// The Serializeable Base Setting Element for the Graphical Settings.
    /// </summary>
    public class vxGraphicalBaseElementSettings
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

        /// <summary>
        /// Graphical Quality for this Setting
        /// </summary>
        [XmlAttribute("Quality")]
        public vxEnumQuality Quality
        {
            get { return _quality; }
            set
            {
                _quality = value;
                if (QualityChanged != null)
                    QualityChanged(this, new EventArgs());
            }
        }
        vxEnumQuality _quality;


        /// <summary>
        /// Fired when Quality Changes.
        /// </summary>
        public event EventHandler<EventArgs> QualityChanged;


        public vxGraphicalBaseElementSettings()
        {
            Quality = vxEnumQuality.Medium;
            IsEnabled = true;
        }
    }
}
*/
