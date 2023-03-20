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
    public enum vxEnumReflectionType
    {
        None,
        CubeMaps,
        SSR,
    }

	/// <summary>
	/// This holds the Serializable data of a Cascade Shadow Mapping Settings.
	/// </summary>
	public class vxGraphicalReflectionSettings : vxGraphicalBaseQualitySetting
	{
		[XmlAttribute("ReflectionType")]
		public vxEnumReflectionType ReflectionType = vxEnumReflectionType.CubeMaps;


		[XmlElement("ScreenSpaceReflections")]
		public vxGraphicalSSRSettings SSRSettings;


		public vxGraphicalReflectionSettings()
		{
			SSRSettings = new vxGraphicalSSRSettings();
		}
    }
}
