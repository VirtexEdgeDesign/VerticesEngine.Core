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
public class vxGraphicalBaseQualitySetting
    {
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


        public vxGraphicalBaseQualitySetting()
        {
            Quality = vxEnumQuality.Medium;
        }
    }
}
