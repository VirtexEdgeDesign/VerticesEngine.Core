using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using VerticesEngine.UI.Themes;

namespace VerticesEngine.Graphics
{
    public enum ReflectionType
    {
        None,
        CubeMaps,
        SSR
    }

    /// <summary>
    /// A material is what is used to draw a mesh. It can expose common shader parameters along with allowing for expansion
    /// </summary>
    public class vxMaterial : IDisposable
    {
        public bool ShouldDispose = true;

        /// <summary>
        /// The Current Render Technique for thie Model
        /// </summary>
        public string RenderTechnique = "Technique_Main";

        #region Shaders

        public vxShader Shader
        {
            get { return _shader; }
        }
        vxShader _shader;

        protected EffectParameterCollection Parameters
        {
            get { return _shader.Parameters; }
        }

        /// <summary>
        /// Which pass should this material be drawn in?
        /// </summary>
        public string MaterialRenderPass;

        /// <summary>
        /// The utility effect.
        /// </summary>
        public vxUtilityEffect UtilityEffect;

        #endregion


        /// <summary>
        /// Sets the world.
        /// </summary>
        /// <value>The world.</value>
        public Matrix World
        {
            set
            {
                //SetEffectParameter("World", value);
                //SetEffectParameter("world", value);
                //SetEffectParameter("VX_MATRIX_WORLD", value);
                worldEffectParam?.SetValue(value);
            }
        }
        protected EffectParameter worldEffectParam;

        /// <summary>
        /// The World * View * Projection Cumulative Matrix Value.
        /// </summary>
        public Matrix WVP
        {
            set
            {
                //SetEffectParameter("wvp", value);
                //SetEffectParameter("WorldViewProj", value);
                //SetEffectParameter("VX_MATRIX_WVP", value);

                wvpEffectParam?.SetValue(value);
            }
        }
        protected  EffectParameter wvpEffectParam;

        public Matrix WorldInverseTranspose
        {
            set
            {
                //SetEffectParameter("WorldInverseTranspose", value);
                //SetEffectParameter("VX_MATRIX_W_INV_T", value);
                worldInvTEffectParam?.SetValue(value);
            }
        }
        protected EffectParameter worldInvTEffectParam;

        public Matrix View
        {
            set
            {
                //SetEffectParameter("View", value);
                //SetEffectParameter("VX_CAMERA_VIEW", value);

                cameraViewEffectParam?.SetValue(value);
            }
        }
        protected EffectParameter cameraViewEffectParam;

        public Matrix Projection
        {
            set
            {
                //SetEffectParameter("Projection", value);
                //SetEffectParameter("VX_CAMERA_PROJ", value);

                cameraProjEffectParam?.SetValue(value);
            }
        }
        protected EffectParameter cameraProjEffectParam;


        public Vector3 CameraPosition
        {
            set
            {
                //SetEffectParameter("VX_CAMERA_POS", value);

                cameraPosEffectParam?.SetValue(value);
            }
        }
        protected EffectParameter cameraPosEffectParam;

        /// <summary>
        /// Model Alpha Value for Transparency
        /// </summary>
        [vxShowInInspector("Alpha Value", vxInspectorCategory.GraphicalProperies, "The Alpha value to fade the Entity by (if it's supported).")]
        public float Alpha
        {
            set
            {
                m_alphaValue = value;
                SetEffectParameter("AlphaValue", value);
                SetEffectParameter("Alpha", value);
            }
            get { return m_alphaValue; }
        }
        private float m_alphaValue = 1;

        #region -- Texture Properties --

        /// <summary>
        /// Gets the Main Texture for this material. This will return null if no texture is set.
        /// </summary>
        public Texture2D Texture
        {
            get { return _texture; }
            set
            {
                _texture = value;
                if (_shader.Parameters[shaderkey_mainTexture] != null)
                    _shader.Parameters[shaderkey_mainTexture].SetValue(_texture);

                UtilityEffect.DiffuseTexture = _texture;
            }
        }
        private Texture2D _texture;

        protected string shaderkey_mainTexture = "Texture";
        //protected string shaderkey_mainTexture = "_MainTex";

        [vxShowInInspector(vxInspectorCategory.GraphicalProperies)]
        public Color DiffuseColor
        {
            get { return _diffuseColor; }
            set
            {
                _diffuseColor = value;
                SetEffectParameter("DiffuseColor", _diffuseColor);
            }
        }
        private Color _diffuseColor = Color.White;

        /// <summary>
        /// Off Set for Textures Used On the Model. Uses the 'VX_UV0_OFFSET' shader param
        /// </summary>
        public Vector2 TextureUVOffset
        {
            get { return _textureUVOffset; }
            set
            {
                _textureUVOffset = value;
                UtilityEffect.UVOffset = _textureUVOffset;
                textureUVOffsetEffectParam.SetValue(value);
            }
        }
        Vector2 _textureUVOffset = Vector2.Zero;

        protected EffectParameter textureUVOffsetEffectParam;

        /// <summary>
        /// A UV Factor to keep repeating UV Coordinates the same during plane scaling.
        /// </summary>
        [vxShowInInspector(vxInspectorCategory.GraphicalProperies)]
        public Vector2 UVFactor
        {
            get { return _uvFactor; }
            set
            {
                _uvFactor = value;
                SetEffectParameter("UVFactor", value);
                UtilityEffect.UVFactor = _uvFactor;
            }
        }
        Vector2 _uvFactor = Vector2.One;

        /// <summary>
        /// Toggles whether or not the main diffuse texture is shown.
        /// </summary>
        [vxShowInInspector(vxInspectorCategory.GraphicalProperies)]
        public bool IsTextureEnabled
        {
            set
            {
                _textureEnabled = value;
                SetEffectParameter("IsTextureEnabled", value);
            }
            get { return _textureEnabled; }
        }
        bool _textureEnabled;

        #region - Surface Condition Mapping -


        [vxShowInInspector(vxInspectorCategory.GraphicalProperies)]
        public bool IsNormalMapEnabled
        {
            get { return _isNormalMapEnabled; }
            set
            {
                _isNormalMapEnabled = value;

                SetEffectParameter("IsNormalMapEnabled", value);

                UtilityEffect.IsNormalMapEnabled = value;
            }
        }
        private bool _isNormalMapEnabled = true;

        /// <summary>
        /// Gets the Main Texture for this material. This will return null if no texture is set.
        /// </summary>
        public Texture2D NormalMap
        {
            get { return _normalMap; }
            set
            {
                _normalMap = value;
                if (_shader.Parameters[shaderkey_normalMap] != null)
                    _shader.Parameters[shaderkey_normalMap].SetValue(_normalMap);
                UtilityEffect.NormalMap = _normalMap;
            }
        }
        private Texture2D _normalMap;

        protected string shaderkey_normalMap = "NormalMap";


        /// <summary>
        /// Gets or Set's the RMA map for this material. There is a 4th channel used here, The Alpha channel is used for emissivity as well.
        /// </summary>
        public Texture2D RMAMap
        {
            get { return _rmaMap; }
            set
            {
                _rmaMap = value;
                if (_shader.Parameters[shaderkey_rmaMap] != null)
                    _shader.Parameters[shaderkey_rmaMap].SetValue(_rmaMap);
                UtilityEffect.SurfaceMap = _rmaMap;
            }
        }
        private Texture2D _rmaMap;

        protected string shaderkey_rmaMap = "maps_RMA";

        #endregion


        public Texture2D AlphaMaskTexture
        {
            get { return _alphaMaskTexture; }
            set
            {
                _alphaMaskTexture = value;
                SetEffectParameter("AlphaMaskTexture", value);
            }
        }
        Texture2D _alphaMaskTexture;



        public float AlphaMaskCutoff
        {
            get { return _alphaMaskCutoff; }
            set
            {
                _alphaMaskCutoff = value;
                SetEffectParameter("AlphaMaskCutoff", value);
            }
        }
        float _alphaMaskCutoff = 0;

        #endregion

        #region -- Lighting --


        /// <summary>
        /// Should this material be drawn to the derffered back buffers
        /// </summary>
        [vxShowInInspector(vxInspectorCategory.GraphicalProperies)]
        public bool IsDefferedRenderingEnabled
        {
            get { return _doDepthMapping; }
            set
            {
                _doDepthMapping = value;

                SetEffectParameter("DoDepthMapping", value);
            }
        }
        private bool _doDepthMapping = true;

        /// <summary>
        /// If this is a transparent item but we still want to do distortion
        /// </summary>
        public bool IsTransparentDefferedRenderingEnabled
        {
            get { return _isTransparentDefEnabled; }
            set
            {
                _isTransparentDefEnabled = value;
            }
        }
        private bool _isTransparentDefEnabled = false;


        #region  - Specular -


        /// <summary>
        /// SpecularIntensity of the Shader
        /// </summary>
        [vxShowInInspector(vxInspectorCategory.GraphicalProperies)]
        public float SpecularIntensity
        {
            set
            {
                _specularIntensity = value;
                SetEffectParameter("SpecularIntensity", value);
            }
            get { return _specularIntensity; }
        }
        float _specularIntensity = 8;

        /// <summary>
        /// SpecularIntensity of the Shader
        /// </summary>
        [vxShowInInspector(vxInspectorCategory.GraphicalProperies)]
        public float SpecularPower
        {
            set
            {
                _specularPower = value;
                SetEffectParameter("SpecularPower", value);
            }
            get { return _specularPower; }
        }
        float _specularPower = 1;
        #endregion


        /// <summary>
        /// The Light Direction which is Shining on this Object
        /// </summary>
        [vxShowInInspector(vxInspectorCategory.GraphicalProperies)]
        public Vector3 LightDirection
        {
            get { return _lightDirection; }
            set
            {
                _lightDirection = value;
                SetEffectParameter("LightDirection", value);
                SetEffectParameter("DirLight0Direction", value);
                SetEffectParameter("ToonLightDirection", value);
                SetEffectParameter("VX_MAINLIGHTDIR", value);
                UtilityEffect.LightDirection = _lightDirection;
            }
        }
        Vector3 _lightDirection;


        /// <summary>
        /// The Light Colour to be used in the Model Shader.
        /// </summary>
        [vxShowInInspector(vxInspectorCategory.GraphicalProperies)]
        public Color LightColor
        {
            get { return _lightColor; }
            set
            {
                _lightColor = value;
                SetEffectParameter("LightColor", value.ToVector4());
            }
        }
        Color _lightColor;

        /// <summary>
        /// The Ambient Light Colour for this Models Shader.
        /// </summary>
        [vxShowInInspector(vxInspectorCategory.GraphicalProperies)]
        public Color AmbientLightColor
        {
            get { return _ambientLightColor; }
            set
            {
                _ambientLightColor = value;
                SetEffectParameter("AmbientLight", value.ToVector4());
                UtilityEffect.AmbientLightColor = _ambientLightColor;
            }
        }
        Color _ambientLightColor;


        /// <summary>
        /// Gets or sets the ambient light intensity.
        /// </summary>
        /// <value>The ambient light intensity.</value>
        [vxShowInInspector(vxInspectorCategory.GraphicalProperies)]
        public float AmbientLightIntensity
        {
            get { return _ambientLightIntensity; }
            set
            {
                _ambientLightIntensity = value;
                SetEffectParameter("AmbientIntensity", value);
                UtilityEffect.AmbientLightColor = _ambientLightColor;
            }
        }
        float _ambientLightIntensity;

        public float DiffuseIntensity
        {
            set
            {
                _diffuseIntensity = value;

                SetEffectParameter("DiffuseIntensity", _diffuseIntensity);

            }
            get { return _diffuseIntensity; }
        }
        float _diffuseIntensity = 1;

        #region - Emission -

        [vxShowInInspector(vxInspectorCategory.GraphicalProperies)]
        public bool IsEmissionEnabled
        {
            get { return _isEmissionEnabled; }
            set
            {
                _isEmissionEnabled = value;

                SetEffectParameter("DoEmissiveMapping", value);
            }
        }
        private bool _isEmissionEnabled = true;

        /// <summary>
        /// The Emission Map. 
        /// </summary>
        public Texture2D EmissionMap
        {
            get { return _emissionMap; }
            set
            {
                _emissionMap = value;
                _isEmissionEnabled = !(value == null);
                UtilityEffect.EmissiveMap = _emissionMap;
                SetEffectParameter("EmissiveMap", value);

            }
        }
        Texture2D _emissionMap;

        //[vxShowInInspector(vxInspectorCategory.GraphicalProperies, "The amount of emissive bloom (glow) from the Emissivity map.")]
        //public float EmissiveIntensity
        //{
        //    get { return _glowIntensity; }
        //    set
        //    {
        //        _glowIntensity = value;
        //        UtilityEffect.GlowIntensity = _glowIntensity;
        //        SetEffectParameter("GlowIntensity", _glowIntensity);
        //    }
        //}
        //private float _glowIntensity = 1;

        /// <summary>
        /// Emissive Colour for use in Highlighting a Model.
        /// </summary>
        [vxShowInInspector(vxInspectorCategory.GraphicalProperies)]
        public Color EmissiveColour
        {
            set
            {
                _emissiveColour = value;
                SetEffectParameter("EmissiveColour", value.ToVector4());
                UtilityEffect.EmissiveColour = value;
            }
            get { return _emissiveColour; }
        }
        Color _emissiveColour = Color.White;

        [vxShowInInspector(vxInspectorCategory.GraphicalProperies)]
        public float EmissiveIntensity
        {
            set
            {
                _emissiveIntensity = value;
                SetEffectParameter("GlowIntensity", _emissiveIntensity);
                SetEffectParameter("EmissiveIntensity", _emissiveIntensity);
            }
            get { return _emissiveIntensity; }
        }
        float _emissiveIntensity = 1;
        #endregion

        #region - Reflections -

        /// <summary>
        /// Texture which is applied as the Reflection Texture.
        /// NOTE: This must be added to the Main Model Shader.
        /// </summary>
        public TextureCube ReflectionTextureCube
        {
            set
            {
                _reflectionTexture = value;
                SetEffectParameter("ReflectionTexture", value);
            }
            get { return _reflectionTexture; }
        }
        TextureCube _reflectionTexture;



        [vxShowInInspector(vxInspectorCategory.GraphicalProperies)]
        public ReflectionType ReflectionType
        {
            get { return _reflectionType; }
            set
            {
                _reflectionType = value;

                switch (_reflectionType)
                {
                    case ReflectionType.None:
                        IsSSREnabled = false;
                        IsBasicReflectionsEnabled = false;
                        break;
                    case ReflectionType.CubeMaps:
                        IsSSREnabled = false;
                        IsBasicReflectionsEnabled = true;
                        break;
                    case ReflectionType.SSR:
                        IsSSREnabled = true;
                        IsBasicReflectionsEnabled = true;
                        break;
                }
            }
        }
        ReflectionType _reflectionType = ReflectionType.CubeMaps;


        public bool IsBasicReflectionsEnabled
        {
            get { return _doReflections; }
            set
            {
                _doReflections = value;
                //UtilityEffect.Parameters["DoReflections"].SetValue(_doReflections);
            }
        }
        bool _doReflections = true;


        /// <summary>
        /// Is SSR enabled for this entity
        /// </summary>
        public bool IsSSREnabled
        {
            get { return _doSSR; }
            set
            {
                _doSSR = value;
                UtilityEffect.DoSSR = value;
            }
        }
        bool _doSSR = true;


        /// <summary>
        /// The overall Reflection Amount to be applied in the Shader.
        /// </summary>
        [vxShowInInspector(vxInspectorCategory.GraphicalProperies)]
        public float ReflectionIntensity
        {
            set
            {
                _reflectionAmount = value;
                SetEffectParameter("ReflectionIntensity", _reflectionAmount);
            }
            get { return _reflectionAmount; }
        }
        float _reflectionAmount = 1;

        #endregion

        #region - Fresnel -

        [vxShowInInspector(vxInspectorCategory.GraphicalProperies)]
        public float FresnelBias
        {
            set
            {
                _fresnelBias = value;
                SetEffectParameter("fFresnelBias", _fresnelBias);
            }
            get { return _fresnelBias; }
        }
        float _fresnelBias = 0.025f;


        [vxShowInInspector(vxInspectorCategory.GraphicalProperies)]
        public float FresnelPower
        {
            set
            {
                _fresnelPower = value;
                SetEffectParameter("fFresnelPower", _fresnelPower);
            }
            get { return _fresnelPower; }
        }
        float _fresnelPower = 6.0f;

        #endregion

        #endregion

        #region -- Shadows --

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:VerticesEngine.Entities.vxEntity3D"/> should do shadow map.
        /// </summary>
        /// <value><c>true</c> if do shadow map; otherwise, <c>false</c>.</value>
        [vxShowInInspector(vxInspectorCategory.GraphicalProperies)]
        public bool IsShadowCaster
        {
            get { return _isShadowCaster; }
            set
            {
                _isShadowCaster = value;
                UtilityEffect.IsShadowsEnabled = _isShadowCaster;
            }
        }
        private bool _isShadowCaster = false;



        /// <summary>
        /// Gets or sets the shadow brightness.
        /// </summary>
        /// <value>The shadow brightness.</value>
        [vxShowInInspector(vxInspectorCategory.GraphicalProperies)]
        public float ShadowBrightness
        {
            get { return _shadowBrightness; }
            set
            {
                _shadowBrightness = value;
                UtilityEffect.ShadowBrightness = _shadowBrightness;
            }
        }
        float _shadowBrightness = 0.25f;


        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:VerticesEngine.Entities.vxEntity3D"/> render
        /// shadow split index.
        /// </summary>
        /// <value><c>true</c> if render shadow split index; otherwise, <c>false</c>.</value>
        //public bool renderShadowSplitIndex
        //{
        //    set
        //    {
        //        _renderShadowSplitIndex = value; //UpdateRenderTechnique();
        //        if (Model != null)
        //        {
        //            foreach (var mesh in Model.Meshes)
        //                mesh.DebugEffect.ShadowDebug = _renderShadowSplitIndex;
        //        }
        //    }
        //    get { return _renderShadowSplitIndex; }
        //}
        //bool _renderShadowSplitIndex;



        /*
        [vxGraphicalPropertyAttribute(Title = "Shadow MaxShadowLoops")]
        public int MaxShadowLoops
        {
            get { return _maxShadowLoops; }
            set
            {
                _maxShadowLoops = value;

                if (Model != null)
                {
                    foreach (var mesh in Model.Meshes)
                        mesh.UtilityEffect.Parameters["MaxShadowLoops"].SetValue(_maxShadowLoops);
                }

            }
        }
        int _maxShadowLoops = 4;
        */




        /// <summary>
        /// The Poisson Kernel to be used for Shadow Mapping Edge Blending
        /// </summary>
        //public Vector2[] PoissonKernel
        //{
        //    get { return _poissonKernel; }
        //    set
        //    {
        //        _poissonKernel = value;
        //        if (Model != null)
        //        {
        //            foreach (var mesh in Model.Meshes)
        //                mesh.UtilityEffect.PoissonKernel = _poissonKernel;
        //        }
        //    }
        //}
        //Vector2[] _poissonKernel;



        /// <summary>
        /// Gets the random texture2 d.
        /// </summary>
        /// <value>The random texture2 d.</value>
        //public Texture2D RandomTexture2D
        //{
        //    get { return _randomTexture2D; }
        //    set
        //    {
        //        _randomTexture2D = value;
        //        if (Model != null)
        //        {
        //            foreach (var mesh in Model.Meshes)
        //                mesh.UtilityEffect.RandomTexture2D = _randomTexture2D;
        //        }
        //    }
        //}
        //Texture2D _randomTexture2D;

        #endregion

        #region -- Distortion --


        [vxShowInInspector(vxInspectorCategory.GraphicalProperies)]
        public bool IsDistortionEnabled
        {
            get { return _doDistortionMapping; }
            set
            {
                _doDistortionMapping = value;
                SetEffectParameter("DoDistortionMapping", _doDistortionMapping);
            }
        }
        bool _doDistortionMapping = false;

        /// <summary>
        /// A global amount controlling the amount of distortion this entity causes.
        /// </summary>
        [vxShowInInspector(vxInspectorCategory.GraphicalProperies)]
        public float DistortionScale
        {
            get { return _distortionScale; }
            set
            {
                _distortionScale = value;
                UtilityEffect.DistortionScale = _distortionScale;
            }
        }
        float _distortionScale = 0.25f;

        /// <summary>
        /// The Distortion Map. 
        /// </summary>
        public Texture2D DistortionMap
        {
            get { return _distortionMap; }
            set
            {
                _distortionMap = value;
                UtilityEffect.DistortionMap = _distortionMap;

                IsDistortionEnabled = !(value == null);
            }
        }
        Texture2D _distortionMap;


        /// <summary>
        /// Off Set for Textures Used On the Model
        /// </summary>
        public Vector2 DistortionUVOffset
        {
            get { return _distortionUVOffset; }
            set
            {
                _distortionUVOffset = value;
                UtilityEffect.DistortionUVOffset = _distortionUVOffset;
            }
        }
        Vector2 _distortionUVOffset = Vector2.Zero;

        #endregion

        #region -- Utility Properties --

        /// <summary>
        /// Is this entity used in a auxiliary depth calucaltions
        /// </summary>
        public bool IsAuxDepthCalculated
        {
            get { return _doAuxDepth; }
            set
            {
                _doAuxDepth = value;
                SetEffectParameter("DoAuxDepth", _doAuxDepth);
            }
        }
        private bool _doAuxDepth = true;

        #endregion

        public vxMaterial() : this(new vxShader(vxInternalAssets.Shaders.MainShader))
        {

        }

        /// <summary>
        /// Creates a new material using the specified effect
        /// </summary>
        /// <param name="shader"></param>
        public vxMaterial(Effect effect) : this(new vxShader(effect))
        {

        }

        /// <summary>
        /// Creates a new material using the specified shader
        /// </summary>
        /// <param name="shader"></param>
        public vxMaterial(vxShader shader) : base()
        {
            _shader = shader;
            UtilityEffect = new vxUtilityEffect(vxInternalAssets.Shaders.PrepPassShader);

            Initalise();
        }
        protected virtual void InitEffectParams()
        {
            worldEffectParam = Parameters["VX_MATRIX_WORLD"];
            wvpEffectParam = Parameters["VX_MATRIX_WVP"];
            worldInvTEffectParam = Parameters["VX_MATRIX_W_INV_T"];
            cameraViewEffectParam = Parameters["VX_CAMERA_VIEW"];
            cameraProjEffectParam = Parameters["VX_CAMERA_PROJ"];
            cameraPosEffectParam = Parameters["VX_CAMERA_POS"];

            textureUVOffsetEffectParam = Parameters["VX_UV0_OFFSET"];
        }
        public virtual void Initalise()
        {
            InitEffectParams();

            // set initial render pass
            MaterialRenderPass = vxRenderPipeline.Passes.OpaquePass;

            IsTextureEnabled = true;
            IsNormalMapEnabled = true;
            IsDefferedRenderingEnabled = true;
            Alpha = 1;

            // Set Lighting Values
            AmbientLightColor = Color.White;
            AmbientLightIntensity = 0.51f;
            SpecularIntensity = 0;
            SpecularPower = 1;
            LightDirection = Vector3.Normalize(new Vector3(100, 130, 0));
            LightColor = new Color(0.8f, 0.8f, 0.8f, 1.0f);
            DiffuseIntensity = 1;

            // Emission
            IsEmissionEnabled = true;
            EmissiveIntensity = 1;
            EmissiveColour = Color.White;
            EmissiveIntensity = 1f;

            // Reflections
            ReflectionType = ReflectionType.CubeMaps;
            ReflectionIntensity = 1;
            FresnelPower = 2;
            FresnelBias = 0.025f;
            DistortionScale = 0.0125f;
            // Some Items, which don't need Reflections, are added before the scene is actually
            // fully instantiated, therefore, a check to see if the Sky Box exists yet is needed.
            //if (Scene.SkyBox != null)
            //ReflectionTextureCube = Scene.SkyBox.SkyboxTextureCube;

            // Utility Shader
            UtilityEffect.UVFactor = new Vector2(1, 1);
            UtilityEffect.DiffuseLight = new Color(0.5f, 0.5f, 0.5f, 1);
            IsShadowCaster = true;
            ShadowBrightness = 0.355f;
            UtilityEffect.PoissonKernel = vxShadowEffect.PoissonKernel;
            UtilityEffect.ShadowBlurStart = 4;
            UtilityEffect.NumberOfShadowBlendSamples = 4;
            UtilityEffect.BlendSampleCount = 8.0f;
            UtilityEffect.PoissonKernelScale = vxShadowEffect.PoissonKernelScale;
            //UtilityEffect.TileBounds = vxShadowEffect.ShadowSplitTileBounds;
            UtilityEffect.RandomTexture2D = vxInternalAssets.Textures.RandomValues;
            UtilityEffect.DebugShadowSplitColors = vxShadowEffect.ShadowSplitColors;
            IsAuxDepthCalculated = true;

            // Debug Shader
            //DebugEffect.WireColour = new Color(0.1f, 0.1f, 0.1f, 1);
        }

        public void Dispose()
        {
            OnDisposed();
        }

        protected virtual void OnDisposed()
        {
            //base.OnDisposed();

            if (_shader == null)
                return;

            //if(Texture != null)
            //    Texture.Dispose();
            Texture = null;

            //if (NormalMap != null)
            //    NormalMap.Dispose();
            NormalMap = null;
            
            //if (UtilityEffect.SurfaceMap != null)
            //    UtilityEffect.SurfaceMap.Dispose();
            UtilityEffect.SurfaceMap = null;
            RMAMap = null;

            //if (UtilityEffect.ShadowMap != null)
            //    UtilityEffect.ShadowMap.Dispose();
            UtilityEffect.ShadowMap = null;


            //if (DistortionMap != null)
            //    DistortionMap.Dispose();
            DistortionMap = null;

            // dispose all textures
            //foreach (var prop in this.GetType().GetFields())
            //{
            //    if(prop.FieldType == typeof(Texture2D))
            //    {
            //        Console.WriteLine(prop.DeclaringType);
            //    }

            //}

            //UtilityEffect.Dispose();
            _shader.Dispose();
            UtilityEffect.Dispose();
            _shader = null;
            UtilityEffect = null;
        }

        public void SetPass()
        {
            _shader.CurrentTechnique = _shader.Techniques[RenderTechnique];
        }


        #region -- Utility Methods --

        /// <summary>
        /// Copies over material properties into this material
        /// </summary>
        /// <param name="material"></param>
        public void CopyMaterial(vxMaterial material)
        {
            //foreach (var param in material.Shader.Parameters)
            //{
            //    try
            //    {
            //        if (param.ParameterType == EffectParameterType.Texture)
            //            SetEffectParameter(param.Name, param.GetValueTexture2D());
            //        if (param.ParameterType == EffectParameterType.Texture2D)
            //            SetEffectParameter(param.Name, param.GetValueTexture2D());
            //        if (param.ParameterType == EffectParameterType.Single)
            //            SetEffectParameter(param.Name, param.GetValueSingle());
            //    }
            //    catch { }
            //}

            this.Texture = material.Texture;
            this.RMAMap = material.RMAMap;
            this.NormalMap = material.NormalMap;

            this.UtilityEffect = material.UtilityEffect;
            //this.DebugEffect = material.DebugEffect;
        }


        public void SetEffectParameter(string param, float value)
        {

            if (_shader.Parameters[param] != null)
                _shader.Parameters[param].SetValue(value);

            if (UtilityEffect.Parameters[param] != null)
                UtilityEffect.Parameters[param].SetValue(value);
        }

        public void SetEffectParameter(string param, float[] values)
        {
            if (_shader.Parameters[param] != null)
                _shader.Parameters[param].SetValue(values);

            if (UtilityEffect.Parameters[param] != null)
                UtilityEffect.Parameters[param].SetValue(values);
        }

        public void SetEffectParameter(string param, bool value)
        {
            if (_shader.Parameters[param] != null)
                _shader.Parameters[param].SetValue(value);

            if (UtilityEffect.Parameters[param] != null)
                UtilityEffect.Parameters[param].SetValue(value);
        }

        public void SetEffectParameter(string param, Vector2 value)
        {
            if (_shader.Parameters[param] != null)
                _shader.Parameters[param].SetValue(value);

            if (UtilityEffect.Parameters[param] != null)
                UtilityEffect.Parameters[param].SetValue(value);
        }

        public void SetEffectParameter(string param, Vector3 value)
        {
            if (_shader.Parameters[param] != null)
                _shader.Parameters[param].SetValue(value);

            if (UtilityEffect.Parameters[param] != null)
                UtilityEffect.Parameters[param].SetValue(value);
        }

        public void SetEffectParameter(string param, Vector4 value)
        {
            if (_shader.Parameters[param] != null)
                _shader.Parameters[param].SetValue(value);

            if (UtilityEffect.Parameters[param] != null)
                UtilityEffect.Parameters[param].SetValue(value);
        }

        public void SetEffectParameter(string param, Matrix value)
        {
            if (_shader.Parameters[param] != null)
                _shader.Parameters[param].SetValue(value);

            if (UtilityEffect.Parameters[param] != null)
                UtilityEffect.Parameters[param].SetValue(value);
        }

        public void SetEffectParameter(string param, Color value)
        {
            Vector4 colourVec4 = value.ToVector4();

            if (_shader.Parameters[param] != null)
                _shader.Parameters[param].SetValue(colourVec4);

            if (UtilityEffect.Parameters[param] != null)
                UtilityEffect.Parameters[param].SetValue(colourVec4);
        }

        public void SetEffectParameter(string param, Texture2D value)
        {
            if (_shader.Parameters[param] != null)
                _shader.Parameters[param].SetValue(value);

            if (UtilityEffect.Parameters[param] != null)
                UtilityEffect.Parameters[param].SetValue(value);
        }

        public void SetEffectParameter(string param, TextureCube value)
        {
            if (_shader.Parameters[param] != null)
                _shader.Parameters[param].SetValue(value);

            if (UtilityEffect.Parameters[param] != null)
                UtilityEffect.Parameters[param].SetValue(value);
        }

        public vxMaterial Clone()
        {
            vxMaterial other = (vxMaterial)this.MemberwiseClone();
            other._shader = new vxShader(this.Shader);
            other.UtilityEffect = new vxUtilityEffect(vxInternalAssets.Shaders.PrepPassShader);

            other.Initalise();
            return other;
        }

        #endregion
    }
}
