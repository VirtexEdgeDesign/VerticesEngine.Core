using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine;
using VerticesEngine.ContentManagement;
using VerticesEngine.Entities;

namespace VerticesEngine.Graphics
{
    public class vxWaterMaterial : vxMaterial
    {

        [vxShowInInspector("Water Properties")]
        public Color ShallowColor
        {
            get { return _shallowColour; }
            set
            {
                _deepColour = value;
                SetEffectParameter("vShallowColor", _shallowColour);
            }
        }
        Color _shallowColour = Color.DeepSkyBlue * 0.5f;

        [vxShowInInspector("Water Properties")]
        public Color DeepColour
        {
            get { return _deepColour; }
            set
            {
                _deepColour = value;
                SetEffectParameter("vDeepColor", _deepColour);
            }
        }
        Color _deepColour = Color.DeepSkyBlue * 1.2f;

        [vxShowInInspector("Water Properties")]
        public Texture2D BumpMap
        {
            get { return _bumpMap; }
            set
            {
                _bumpMap = value;
                SetEffectParameter("BumpMap", _bumpMap);
            }
        }
        Texture2D _bumpMap;// = Color.DeepSkyBlue * 1.2f;



        [vxShowInInspector("Water Properties")]
        public float WaterAmount
        {
            get { return _waterAmount; }
            set
            {
                _waterAmount = value;
                SetEffectParameter("fWaterAmount", _waterAmount);
            }
        }
        float _waterAmount;// = Color.DeepSkyBlue * 1.2f;

        public RenderTarget2D AuxDepthMap;


        public vxWaterMaterial() : base(new vxShader(vxInternalAssets.Shaders.WaterReflectionShader))
        {
            RenderTechnique = "Technique_Water";
        }

        public override void Initalise()
        {
            base.Initalise();

            this.MaterialRenderPass = vxRenderPipeline.Passes.TransparencyPass;

            //BumpMap = vxContentManager.Instance.Load<Texture2D>("vxengine/models/water/waterbump");
            BumpMap = vxInternalAssets.Textures.WaterBumpMap;// vxContentManager.Instance.Load<Texture2D>("vxengine/models/water/waterbump");

            UVFactor = new Vector2(0.5f, 0.5f);
            Shader.Parameters["ReflectionCube"].SetValue(vxSkyBox.Instance.CubeMap);
            Shader.Parameters["fFresnelPower"].SetValue(20.0f);
            Shader.Parameters["vDeepColor"].SetValue(Color.DeepSkyBlue.ToVector4() * 0.25f);
            Shader.Parameters["vShallowColor"].SetValue(Color.DeepSkyBlue.ToVector4());
            Shader.Parameters["fWaterAmount"].SetValue(0.0125f);

            // Load the Distortion Map
            //DistortionMap = vxInternalAssets.Textures.RandomValues;
            DistortionMap = vxInternalAssets.Textures.WaterDistortionMap;//vxContentManager.Instance.Load<Texture2D>("vxengine/models/water/water_plane_dm");
            IsShadowCaster = false;

            Shader.Parameters["EmissiveColour"].SetValue(Color.Transparent.ToVector4());

            DistortionScale = 0.25f;
            IsSSREnabled = true;
            //DoAuxDepth = false;
            //DoSSRReflection;
            IsDistortionEnabled = true;

            LightDirection = Vector3.Normalize(Vector3.One);

            ShadowBrightness = 0.51f;
        }

        
    }
}
