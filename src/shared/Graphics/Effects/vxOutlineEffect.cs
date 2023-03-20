using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine;

namespace VerticesEngine.Graphics
{
	public class vxOutlineEffect : vxShader
	{
		/// <summary>
		/// Sets the selection colour.
		/// </summary>
		/// <value>The selection colour.</value>
		public Color SelectionColour
		{
			set { Parameters["SelectionColour"].SetValue(value.ToVector4()); }
		}

        /// <summary>
        /// The World * View * Projection Cumulative Matrix Value.
        /// </summary>
        public Matrix WVP
        {
            set { Parameters["VX_MATRIX_WVP"].SetValue(value); }
        }

		/// <summary>
		/// Sets the line thickness.
		/// </summary>
		/// <value>The line thickness.</value>
		public float LineThickness
		{
            set { Parameters["LineThickness"].SetValue(value); }
		}

		public vxOutlineEffect(Effect effect) : base(effect)
		{
			

		}
	}
}
