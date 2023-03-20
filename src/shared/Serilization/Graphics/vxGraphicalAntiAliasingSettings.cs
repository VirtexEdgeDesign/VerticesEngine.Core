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
using VerticesEngine.Graphics;

namespace VerticesEngine.Serilization
{
	/// <summary>
	/// This holds the Serializable data for the Anti Aliasing Settings.
	/// </summary>
	public class vxGraphicalAntiAliasingSettings
	{
        //[vxGraphicalSettingsAttribute("AntiAliasType")]
		//public static vxEnumAntiAliasType Type = vxEnumAntiAliasType.FXAA;

		//[XmlElement("FXAA")]
		//public vxGraphicalFXAASettings FXAASettings;

		//[XmlElement("TXAA")]
		//public vxGraphicalTXAASettings TXAASettings;

		public vxGraphicalAntiAliasingSettings()
		{
			//FXAASettings = new vxGraphicalFXAASettings();
			//TXAASettings = new vxGraphicalTXAASettings();
		}
    }
}
