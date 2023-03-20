using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using VerticesEngine.Settings;

public enum vxEnumTextureQuality
{
	Ultra,
	High,
	Medium,
	Low
}

namespace VerticesEngine.Serilization
{
	/// <summary>
	/// This holds the Serializable data of a Texture Settings.
	/// </summary>
	public class vxGraphicalTexturesSettings : ICloneable
	{
		/// <summary>
		/// Graphical Quality for this Setting
		/// </summary>
		[XmlAttribute("Quality")]
		public vxEnumTextureQuality Quality
		{
			get { return _quality; }
			set
			{
				IsDirty = true;

				_quality = value;
				if (QualityChanged != null)
					QualityChanged(this, new EventArgs());
			}
		}
		vxEnumTextureQuality _quality;

		/// <summary>
		/// Whether or not the Setting has been changed.
		/// </summary>
		public bool IsDirty = false;


		/// <summary>
		/// Fired when Quality Changes.
		/// </summary>
		public event EventHandler<EventArgs> QualityChanged;

		public vxGraphicalTexturesSettings()
		{

		}

		public object Clone()
		{
			return MemberwiseClone();
		}
	}
}
