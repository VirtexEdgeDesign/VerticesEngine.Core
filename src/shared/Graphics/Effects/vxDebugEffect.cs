using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine;

namespace VerticesEngine.Graphics
{
	public class vxDebugEffect : vxShader
	{

		public Matrix World
		{
			set { Parameters["VX_MATRIX_WORLD"].SetValue(value); }
		}

		/// <summary>
		/// The World * View * Projection Cumulative Matrix Value.
		/// </summary>
		public Matrix WVP
		{
			set { Parameters["VX_MATRIX_WVP"].SetValue(value); }
		}

		/// <summary>
		/// Sets a value indicating whether this <see cref="T:VerticesEngine.Graphics.vxUtilityEffect"/> do texture.
		/// </summary>
		/// <value><c>true</c> if do texture; otherwise, <c>false</c>.</value>
		public bool DoTexture
		{
			set { Parameters["DoTexture"].SetValue(value); }
		}


		/// <summary>
		/// Gets or sets the diffuse texture.
		/// </summary>
		/// <value>The diffuse texture.</value>
		public Texture2D DiffuseTexture
		{
			get { return _diffusetexture; }
			set
			{
				_diffusetexture = value;
				Parameters["Texture"].SetValue(value);
			}
		}
		Texture2D _diffusetexture;


        public Vector3 LightDirection
        {
            set { Parameters["LightDirection"].SetValue(value); }
        }
		#region DebugParameters


		public bool DoDebugWireFrame
		{
			set { Parameters["DoWireFrame"].SetValue(value); }
		}


		public Color WireColour
		{
			set { Parameters["WireColour"].SetValue(value.ToVector4()); }
		}

		#endregion


		public bool ShadowDebug
		{
			set { if(Parameters["ShadowDebug"] != null) Parameters["ShadowDebug"].SetValue(value); }
		}

		public vxDebugEffect(Effect effect) : base(effect)
		{


		}
	}
}
