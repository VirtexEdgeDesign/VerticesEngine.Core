using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine;

namespace VerticesEngine.Graphics
{
	public class vxEditorTempEntityEffect : vxShader
	{
		/// <summary>
		/// The World * View * Projection Cumulative Matrix Value.
		/// </summary>
		public Matrix WVP
		{
			set { if (Parameters["LineThickness"] != null) Parameters["VX_MATRIX_WVP"].SetValue(value); }
		}


		public Color NormalColour
		{
			set { if (Parameters["LineThickness"] != null) Parameters["NormalColour"].SetValue(value.ToVector4()); }
		}

		public float Alpha
		{
			set { if (Parameters["LineThickness"] != null) Parameters["Alpha"].SetValue(value); }
		}


        public float LineThickness
        {
            set { if(Parameters["LineThickness"] != null) Parameters["LineThickness"].SetValue(value); }
        }


        public vxEditorTempEntityEffect(Effect effect) : base(effect)
		{
			NormalColour = Color.Cyan;
			Alpha = 0.5f;
			LineThickness = 0.03f;
        }
	}
}
