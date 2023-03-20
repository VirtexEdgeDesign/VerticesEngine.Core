using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using VerticesEngine.Graphics;
using VerticesEngine.Utilities;
using VerticesEngine.Serilization;

namespace VerticesEngine.Settings
{
	/// <summary>
	/// Serializable Settings.
	/// </summary>
	public class vxGraphicalSettings : ICloneable
	{
		[XmlElement("Screen")]
		//public vxGraphicalViewSettings Screen;

		[XmlElement("Textures")]
		public vxGraphicalTexturesSettings Textures;

		[XmlElement("AntiAliasing")]
		public vxGraphicalAntiAliasingSettings AntiAliasing;

		[XmlElement("Bloom")]
		public vxGraphicalBloomSettings Bloom;

		[XmlElement("Blur")]
		public vxGraphicalBlurSettings Blur;

        [XmlElement("MotionBlur")]
        public vxGraphicalMotionBlurSettings MotionBlur;

		[XmlElement("CrepuscularRays")]
		public vxGraphicalGodRaysSettings GodRays;

		[XmlElement("DefferredLighting")]
		public vxGraphicalDefferredLightingSettings DefferredLighting;

		[XmlElement("DepthOfField")]
		public vxGraphicalDepthOfFieldSettings DepthOfField;

		[XmlElement("EdgeDetection")]
		public vxGraphicalEdgeDetectionSettings EdgeDetection;

		[XmlElement("Reflections")]
		public vxGraphicalReflectionSettings Reflections;

		[XmlElement("Shadows")]
		public vxGraphicalShadowSettings Shadows;

		[XmlElement("SSAO")]
		public vxGraphicalSSAOSettings SSAO;

		/// <summary>
		/// The Base Constructor
		/// </summary>
		public vxGraphicalSettings()
		{
			//Screen = new vxGraphicalViewSettings();
			Textures = new vxGraphicalTexturesSettings();
			AntiAliasing = new vxGraphicalAntiAliasingSettings();
			Bloom = new vxGraphicalBloomSettings();
			Blur = new vxGraphicalBlurSettings();
            MotionBlur = new vxGraphicalMotionBlurSettings();
			DefferredLighting = new vxGraphicalDefferredLightingSettings();
			DepthOfField = new vxGraphicalDepthOfFieldSettings();
			EdgeDetection = new vxGraphicalEdgeDetectionSettings();
			GodRays = new vxGraphicalGodRaysSettings();
			Reflections = new vxGraphicalReflectionSettings();
			Shadows = new vxGraphicalShadowSettings();
			SSAO = new vxGraphicalSSAOSettings();
		}

		public object Clone()
		{
			vxGraphicalSettings clonedSettings = (vxGraphicalSettings)MemberwiseClone();
			//clonedSettings.Screen = (vxGraphicalViewSettings)Screen.Clone();
			clonedSettings.Textures = (vxGraphicalTexturesSettings)Textures.Clone();
			
			return clonedSettings;
		}
	}
}
