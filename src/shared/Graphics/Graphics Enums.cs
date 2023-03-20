using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VerticesEngine.Graphics
{
	/// <summary>
	/// What is the full screen mode of the current screen.
	/// </summary>
	public enum vxFullScreenMode
	{
		Windowed,
		Borderless,
		Fullscreen
	}


	/// <summary>
	/// Distortion techniques.
	/// </summary>
	public enum DistortionTechniques
	{
		/// <summary>
		/// Distortion casused by a distortion map.
		/// </summary>
		DisplacementMapped,

		/// <summary>
		/// The heat haze distortion.
		/// </summary>
		HeatHaze,

		/// <summary>
		/// Pull In Distortion.
		/// </summary>
		PullIn,

		/// <summary>
		/// No Displacement or Distortion.
		/// </summary>
		ZeroDisplacement,
	}

}
