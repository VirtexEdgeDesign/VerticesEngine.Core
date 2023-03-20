using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VerticesEngine.Graphics
{
	public class vxShadowEffect : vxShader
	{
		public EffectParameter ShadowViewProjection;

		public EffectParameter DepthBias;

		public EffectParameter World;

		public vxShadowEffect(Effect effect) : base(effect)
		{
			ShadowViewProjection = Parameters["ViewProjection_Sdw"];
			DepthBias = Parameters["DepthBias_Sdw"];
			World = Parameters["VX_MATRIX_WORLD"];
		}


        /// <summary>
        /// The shadow split colors.
        /// </summary>
        public static Color[] ShadowSplitColors =
        {
            new Color(255, 0, 0, 255),
            new Color(0, 255, 0, 255),
            new Color(0, 0, 255, 255),
            new Color(160, 32, 240, 255)
        };

        /// <summary>
        /// The shadow depth bias.
        /// </summary>
        public static float[,] ShadowDepthBias =
        {
            { 2.5f, 0.00009f },
            { 2.5f, 0.00009f },
            { 2.5f, 0.0009f },
            { 2.5f, 0.0009f }
        };


        /// <summary>
        /// Gets the poisson kernel.
        /// </summary>
        /// <value>An array of Two Dimensional Vectors (Vector2's) that define the poisson kernel.</value>
        /// <example>
        /// It returns the following values.
        /// <code>
        ///     public static IEnumerable<Vector2> poissonKernel()
        ///         {
        ///             return new[]
        ///             {
        ///             new Vector2(-0.326212f, -0.405810f),
        ///             new Vector2(-0.840144f, -0.073580f),
        ///             new Vector2(-0.695914f,  0.457137f),
        ///             new Vector2(-0.203345f,  0.620716f),
        ///             new Vector2( 0.962340f, -0.194983f), 
        ///             new Vector2( 0.473434f, -0.480026f),
        ///             new Vector2( 0.519456f,  0.767022f), 
        ///             new Vector2( 0.185461f, -0.893124f),
        ///             new Vector2( 0.507431f,  0.064425f), 
        ///             new Vector2( 0.896420f,  0.412458f),
        ///             new Vector2(-0.321940f, -0.932615f),
        ///             new Vector2(-0.791559f, -0.597710f)
        ///             };
        ///     }
        /// </code></example>
        public static Vector2[] PoissonKernel
        {
            get
            {
                return poissonKernel()
                    .Select(v => v)
                    .OrderBy(v => v.Length())
                    .ToArray();
            }
        }
        public static IEnumerable<Vector2> poissonKernel()
        {
            return new[]
            {
                     new Vector2(-0.326212f, -0.405810f),
                     new Vector2(-0.840144f, -0.073580f),
                     new Vector2(-0.695914f,  0.457137f),
                     new Vector2(-0.203345f,  0.620716f),
                     new Vector2( 0.962340f, -0.194983f),
                     //new Vector2( 0.473434f, -0.480026f),
                     //new Vector2( 0.519456f,  0.767022f),
                     //new Vector2( 0.185461f, -0.893124f),
                     //new Vector2( 0.507431f,  0.064425f),
                     //new Vector2( 0.896420f,  0.412458f),
                     //new Vector2(-0.321940f, -0.932615f),
                     //new Vector2(-0.791559f, -0.597710f)
            };
        }

        /// <summary>
        /// The Poisson Kernel Scale
        /// </summary>
        public static float[] PoissonKernelScale
        {
            get
            {
                return new[]
                {
                   0.751f, 1.10f, 1.2f, 1.3f, 0.0f
               };
            }
        }
    }
}
