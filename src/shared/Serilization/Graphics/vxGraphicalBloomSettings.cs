using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using VerticesEngine;
using VerticesEngine.Diagnostics;
using VerticesEngine.Settings;

namespace VerticesEngine.Serilization
{
	/// <summary>
	/// This holds the Serializable data of a Cascade Shadow Mapping Settings.
	/// </summary>
	public class vxGraphicalBloomSettings : vxGraphicalBaseQualitySetting
	{
		//[XmlAttribute("Samples")]
		//public int Samples = 15;
        
		public vxGraphicalBloomSettings()
		{

		}
    }
}
