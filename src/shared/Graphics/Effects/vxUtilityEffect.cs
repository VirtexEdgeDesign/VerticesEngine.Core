using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine;

namespace VerticesEngine.Graphics
{
	public class vxUtilityEffect : vxShader
	{
		/// <summary>
		/// Sets the world.
		/// </summary>
		/// <value>The world.</value>
		public Matrix World
		{
			set { worldEffectParam.SetValue(value); }
		}
		
		EffectParameter worldEffectParam;

		/// <summary>
		/// The World * View * Projection Cumulative Matrix Value.
		/// </summary>
		public Matrix WVP
		{
			set { wvpEffectParam.SetValue(value); }
		}
		EffectParameter wvpEffectParam;


		/// <summary>
		/// An offset for Texture Coordinates.
		/// </summary>
		public Vector2 UVOffset
		{
			set
			{
				txtrOffsetEffectParam.SetValue(value);
			}
		}
		EffectParameter txtrOffsetEffectParam;

		internal Color IndexEncodedColour
        {
			set
            {
				indexColourEffectParam.SetValue(value.ToVector4());
            }
        }
		private EffectParameter indexColourEffectParam;

		internal bool IsTransparent
		{
			set
			{
				isTransparentEffectParam.SetValue(value);
			}
		}
		private EffectParameter isTransparentEffectParam;

		/// <summary>
		/// An offset for Distortion Coordinates.
		/// </summary>
		public Vector2 DistortionUVOffset
		{
			set
			{
				if (Parameters["DistUVOffset"] != null) Parameters["DistUVOffset"].SetValue(value);
			}
		}



		/// <summary>
		/// A UV Factor to keep repeating UV Coordinates the same during plane scaling.
		/// </summary>
		public Vector2 UVFactor
		{
			set
			{
				if (Parameters["UVFactor"] != null) Parameters["UVFactor"].SetValue(value);
			}
		}

		#region Lighting 

		public Color DiffuseLight
		{
			set
			{
				if (Parameters["DiffuseLight"] != null)
					Parameters["DiffuseLight"].SetValue(value.ToVector3());
			}
		}


		public float SpecularIntensity
		{
			set
			{
				if (Parameters["SpecularIntensity"] != null) Parameters["SpecularIntensity"].SetValue(value);
			}
		}

		public float SpecularPower
		{
			set
			{
				if (Parameters["SpecularPower"] != null) Parameters["SpecularPower"].SetValue(value);
			}
		}



		/// <summary>
		/// Sets the light direction.
		/// </summary>
		/// <value>The light direction.</value>
		public Vector3 LightDirection
		{
			set
			{
				lightDir0EffectParam?.SetValue(value);
			}
		}
		EffectParameter lightDir0EffectParam;

		#endregion

		#region Colours

		public Color AmbientLightColor
		{
			set
			{
				if (Parameters["AmbientLight"] != null)
					Parameters["AmbientLight"].SetValue(value.ToVector4());
			}
		}

		public Color EmissiveColour
		{
			set
			{
				if (Parameters["EmissiveColour"] != null)
					Parameters["EmissiveColour"].SetValue(value.ToVector4());
			}
		}

		public Color SelectionColour
		{
			set
			{
				if (Parameters["SelectionColour"] != null)
					Parameters["SelectionColour"].SetValue(value.ToVector4());
			}
		}

		#endregion

		#region Textures and Maps


		/// <summary>
		/// Gets or sets the diffuse texture. Note this is not needed by the Utility effect but
        /// kept for information purposes.
		/// </summary>
		/// <value>The diffuse texture.</value>
		public Texture2D DiffuseTexture
		{
			get { return _diffusetexture; }
			set
			{
				_diffusetexture = value;
				if (Parameters["Texture"] != null)
					Parameters["Texture"].SetValue(value);
				//Parameters["Texture"].SetValue(value);
			}
		}
		Texture2D _diffusetexture;



		/// <summary>
		/// Normal Map for this mesh.
		/// </summary>
		public Texture2D NormalMap
		{
			get { return _normalMap; }
			set
			{
				_normalMap = value;
				if (Parameters["NormalMap"] != null)
					Parameters["NormalMap"].SetValue(value);
			}
		}
		Texture2D _normalMap;


        /// <summary>
        /// Gets or sets the surface map for this mesh. The Surface Map uses the following RGBA channels as:
        /// R: Specular Power,
        /// G: Specular Intensity,
        /// B: Reflection Map Value,
        /// A: Emissivity.
        /// </summary>
        /// <value>The surface map.</value>
		public Texture2D SurfaceMap
		{
            get { return _surfaceMap; }
			set
			{
				_surfaceMap = value;
                if (Parameters["SurfaceMap"] != null)
                    Parameters["SurfaceMap"].SetValue(value);
			}
		}
		Texture2D _surfaceMap;

        //public float GlowIntensity
        //{
        //    get { return _glowIntensity; }
        //    set
        //    {
        //        _glowIntensity = value;
        //        if (Parameters["GlowIntensity"] != null)
        //            Parameters["GlowIntensity"].SetValue(value);
        //    }
        //}
        //private float _glowIntensity = 1;



        public bool IsNormalMapEnabled
        {
            set { Parameters["DoNormalMapping"].SetValue(value); }
        }

        public bool DoSSR
        {
            get;
            set;
            //set { Parameters["DoSSRefection"].SetValue(value); }
        }

        
        public bool ReflectionIntensity
        {
            set { Parameters["ReflectionIntensity"].SetValue(value); }
        }



		/// <summary>
		/// Specular Map. Note, setting this will set the same specular map for all meshes.
		/// </summary>
		public RenderTarget2D ShadowMap
		{
			get { return _shadowMap; }
			set
			{
				_shadowMap = value;
				shadowMapEffectParam.SetValue(value);
			}
		}
		RenderTarget2D _shadowMap;
		EffectParameter shadowMapEffectParam;


        /// <summary>
        /// Alpha Mask
        /// </summary>
        public Texture2D AlphaMaskTexture
        {
            get { return _alphaMaskTexture; }
            set
            {
                _alphaMaskTexture = value;
                if (Parameters["AlphaMaskTexture"] != null)
                    Parameters["AlphaMaskTexture"].SetValue(value);
            }
        }
        Texture2D _alphaMaskTexture;


        /// <summary>
        /// Alpha Mask Cutoff
        /// </summary>
        public float AlphaMaskCutoff
        {
            get { return _alphaMaskCutoff; }
            set
            {
                _alphaMaskCutoff = value;
                if (Parameters["AlphaMaskCutoff"] != null)
                    Parameters["AlphaMaskCutoff"].SetValue(value);
            }
        }
        float _alphaMaskCutoff = 0;


		public Texture2D EmissiveMap
		{
			get { return _emissiveMap; }
			set
			{
				_emissiveMap = value;
				if (Parameters["EmissiveMap"] != null)
					Parameters["EmissiveMap"].SetValue(_emissiveMap);
			}
		}
		Texture2D _emissiveMap;

		#endregion

		#region Distortion Code

		public float DistortionScale
		{
			set
			{
				if (Parameters["DistortionScale"] != null) Parameters["DistortionScale"].SetValue(value);
			}
		}

		public Texture2D DistortionMap
		{
			get { return _distortionMap; }
			set
			{
				_distortionMap = value;
                if (Parameters["DistortionMap"] != null)
                    Parameters["DistortionMap"].SetValue(_distortionMap);
			}
		}
		Texture2D _distortionMap;

		#endregion

		#region Shadows

		/// <summary>
		/// Sets the shadow transform.
		/// </summary>
		/// <value>The shadow transform.</value>
		public Matrix[] ShadowTransform
		{
			set
			{
				shadowTransformEffectParam.SetValue(value);
			}
		}
		EffectParameter shadowTransformEffectParam;

		/// <summary>
		/// Sets the tile bounds.
		/// </summary>
		/// <value>The tile bounds.</value>
		public Vector4[] TileBounds
		{
			set
			{
				if (value != null)
					tileBoundsEffectParam.SetValue(value);
			}
		}
		EffectParameter tileBoundsEffectParam;


		/// <summary>
		/// Sets a value indicating whether this <see cref="T:VerticesEngine.Graphics.vxUtilityEffect"/> do shadow.
		/// </summary>
		/// <value><c>true</c> if do shadow; otherwise, <c>false</c>.</value>
		public bool IsShadowsEnabled
		{
			set { isShadowsEnabledEffectParam.SetValue(value); }
		}
		EffectParameter isShadowsEnabledEffectParam;



		/// <summary>
		/// Sets the shadow brightness. 0 being the Darkest and 1 being no shadow.
		/// </summary>
		/// <value>The shadow brightness.</value>
		public float ShadowBrightness
		{
            set {
				shadowBrightnessEnabledEffectParam.SetValue(value); 
            }
		}
		EffectParameter shadowBrightnessEnabledEffectParam;

		/// <summary>
		/// Sets the number samples for shadow edge blending.
		/// </summary>
		/// <value>The number samples.</value>
		public int NumberOfShadowBlendSamples
		{
			set {
				numberOfShadowBlendSamplesEffectParam?.SetValue(value); }
		}
		EffectParameter numberOfShadowBlendSamplesEffectParam;


		/// <summary>
		/// Gets or sets the random texture2 d.
		/// </summary>
		/// <value>The random texture2 d.</value>
		public Texture2D RandomTexture2D
		{
			get { return _randomTexture2D; }
			set
			{
				_randomTexture2D = value;
				if (Parameters["RandomTexture2D"] != null)
					Parameters["RandomTexture2D"].SetValue(_randomTexture2D);
			}
		}
		Texture2D _randomTexture2D;


        /// <summary>
        /// Gets or sets the poisson kernel.
        /// </summary>
        /// <value>The poisson kernel.</value>
		public Vector2[] PoissonKernel
		{
			get { return _poissonKernel; }
			set
			{
				_poissonKernel = value;
				if (Parameters["pk"] != null)
					Parameters["pk"].SetValue(_poissonKernel);
            }
		}
		Vector2[] _poissonKernel;

        /// <summary>
        /// Gets or sets the poisson kernel scale.
        /// </summary>
        /// <value>The poisson kernel scale.</value>
		public float[] PoissonKernelScale
        {
            get { return _poissonKernelScale; }
            set
            {
                _poissonKernelScale = value;
                if (Parameters["PoissonKernelScale"] != null)
                    Parameters["PoissonKernelScale"].SetValue(_poissonKernelScale);
            }
        }
        float[] _poissonKernelScale;

        /// <summary>
        /// The number of blends to be done when blurring the shadow edge
        /// </summary>
        [vxShowInInspector(vxInspectorCategory.GraphicalProperies)]
        public float BlendSampleCount
        {
            get { return _blendSampleCount; }
            set
            {
                _blendSampleCount = value;

                if (Parameters["sampleCount"] != null)
                    Parameters["sampleCount"].SetValue(_blendSampleCount);
            }
        }
        float _blendSampleCount;




        [vxShowInInspector(vxInspectorCategory.GraphicalProperies)]
        public int ShadowBlurStart
        {
            get { return _shadowBlurStart; }
            set
            {
                _shadowBlurStart = value;
                if (Parameters["ShadowBlurStart"] != null)
                    Parameters["ShadowBlurStart"].SetValue(_shadowBlurStart);
            }
        }
        int _shadowBlurStart = 4;



        public Color[] DebugShadowSplitColors
        {
            get { return _debugShadowSplitColors; }
            set
            {
                _debugShadowSplitColors = value;
                List<Vector4> colours = new List<Vector4>();
                foreach(var color in value)
                {
                    colours.Add(color.ToVector4());
                }
                Parameters["SplitColors"].SetValue(colours.ToArray());
            }
        }
        Color[] _debugShadowSplitColors;


		#endregion


		internal EffectTechnique PrepPassTechnique;

		internal EffectTechnique DataMaskTechnique;

		public vxUtilityEffect(Effect effect) : base(effect)
		{
			Init();
		}

		void Init()
		{
			PrepPassTechnique = Techniques["Technique_PrepPass"];
			DataMaskTechnique = Techniques["Technique_DataMaskPass"];
			
			
			worldEffectParam = Parameters["VX_MATRIX_WORLD"];
			wvpEffectParam = Parameters["VX_MATRIX_WVP"];
			txtrOffsetEffectParam = Parameters["VX_UV0_OFFSET"];
			indexColourEffectParam = Parameters["IndexEncodedColour"];
			isTransparentEffectParam = Parameters["isAlpha"];

			shadowMapEffectParam = Parameters["ShadowMap"];
			shadowTransformEffectParam = Parameters["ShadowTransform"];
			tileBoundsEffectParam = Parameters["TileBounds"];
			isShadowsEnabledEffectParam = Parameters["DoShadow"];
			shadowBrightnessEnabledEffectParam = Parameters["ShadowBrightness"];
			numberOfShadowBlendSamplesEffectParam = Parameters["numSamples"];

			lightDir0EffectParam = Parameters["LightDirection"];
		}
	}
}
