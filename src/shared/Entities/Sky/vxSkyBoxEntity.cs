using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using VerticesEngine.ContentManagement;
using VerticesEngine.Graphics;

namespace VerticesEngine.Entities
{
    public class vxSkyBox : vxEntity3D
    {
        public static vxSkyBox Instance
        {
            get { return _instance; }
        }
        private static vxSkyBox _instance;


        /// <summary>
        /// The material that the skybox will use to render
        /// </summary>
        vxSkyBoxMaterial m_skyBoxMaterial = new vxSkyBoxMaterial();

        /// <summary>
        /// Is the sky rotating
        /// </summary>
        private bool m_isRotating = false;

        private float m_rotation = 0;

        Matrix m_rotationMatrix;

        public bool IsSunEnabled = true;

        /// <summary>
        /// The size of the cube, used so that we can resize the box
        /// for different sized environments.
        /// </summary>
        private float m_cubeSize = 150f;


        /// <summary>
        /// Gets or sets the skybox texture cube.
        /// </summary>
        /// <value>The skybox texture cube.</value>
        public TextureCube CubeMap
		{
			set
			{
				_skyboxTextureCube = value;
                m_skyBoxMaterial.SetEffectParameter("SkyBoxTexture", _skyboxTextureCube);
                //m_skyBoxMaterial.SetEffectParameter("SkyBoxTexture", (Texture2D)null);


                //if (Model != null)
                //{
                //    m_skyBoxMaterial.SetEffectParameter("SkyBoxTexture", _skyboxTextureCube);

                //    if (Model.ModelMain != null)
                //        foreach (var part in Model.ModelMain.Meshes.SelectMany(m => m.MeshParts))
                //            if (part.Effect.Parameters["SkyBoxTexture"] != null)
                //                part.Effect.Parameters["SkyBoxTexture"].SetValue(_skyboxTextureCube);
                //}
            }
			get { return _skyboxTextureCube; }
		}
		TextureCube _skyboxTextureCube;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.Entities.vxSkyBoxEntity"/> class.
        /// </summary>
        internal vxSkyBox()
        {
            _instance = this;

            IsEntityCullable = false;

            RemoveSandboxOption(SandboxOptions.Save);
            RemoveSandboxOption(SandboxOptions.Export);
            RemoveSandboxOption(SandboxOptions.Delete);

            //MeshRenderer.IsShadowCaster = false;

            IsMotionBlurEnabled = false;

            //foreach (var mat in MeshRenderer.Materials)
            //    mat.IsDefferedRenderingEnabled = false;
        }

        protected override vxMaterial OnMapMaterialToMesh(vxModelMesh mesh)
        {
            //m_skyBoxMaterial.IsDefferedRenderingEnabled = false;
            
            return m_skyBoxMaterial;
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();

            m_skyBoxMaterial.Dispose();
            m_skyBoxMaterial = null;
        }

        protected internal override void Update()
        {
            base.Update();
            
            m_rotation += 0.1f;
            m_rotationMatrix = m_isRotating ? Matrix.CreateRotationY(m_rotation) : Matrix.Identity;
        }

        protected override vxMesh OnLoadModel()
        {
            return vxContentManager.Instance.LoadMesh("vxengine/shaders/Skybox/cube");
        }

        protected override vxEntityRenderer CreateRenderer()
        {
            return AddComponent<vxSkyboxRenderer>();
        }


        float rot = 0;
        protected internal override void OnWillDraw(vxCamera Camera)
        {
            rot += vxTime.DeltaTime * 10;
            m_cubeSize = Camera.FarPlane * 0.55f;
            //_worldTransform = Matrix.CreateScale(m_cubeSize) * Matrix.CreateTranslation(Camera.Position);// * Matrix.CreateRotationX(rot) * Matrix.CreateRotationY(rot);
            Transform.Scale = m_cubeSize * Vector3.One;
            Transform.Position = Camera.Position;

            base.OnWillDraw(Camera);
            m_skyBoxMaterial.SetEffectParameter("_flipX", CurrentScene.SandBoxFile.Enviroment.SkyBox.FlipX);
            m_skyBoxMaterial.SetEffectParameter("_flipY", CurrentScene.SandBoxFile.Enviroment.SkyBox.FlipY);

            m_skyBoxMaterial.SetEffectParameter("_SkyColor1", CurrentScene.SandBoxFile.Enviroment.SkyBox.SkyColour1);
            m_skyBoxMaterial.SetEffectParameter("_SkyExponent1", CurrentScene.SandBoxFile.Enviroment.SkyBox.SkyExp1);
            m_skyBoxMaterial.SetEffectParameter("_SkyColorStrength1", CurrentScene.SandBoxFile.Enviroment.SkyBox.SkyColourStrength1);

            m_skyBoxMaterial.SetEffectParameter("_SkyColor2", CurrentScene.SandBoxFile.Enviroment.SkyBox.SkyColour2);
            m_skyBoxMaterial.SetEffectParameter("_SkyExponent2", CurrentScene.SandBoxFile.Enviroment.SkyBox.SkyExp2);
            m_skyBoxMaterial.SetEffectParameter("_SkyColorStrength2", CurrentScene.SandBoxFile.Enviroment.SkyBox.SkyColourStrength2);

            m_skyBoxMaterial.SetEffectParameter("_SkyColor3", CurrentScene.SandBoxFile.Enviroment.SkyBox.SkyColour3);
            m_skyBoxMaterial.SetEffectParameter("_SkyColorStrength3", CurrentScene.SandBoxFile.Enviroment.SkyBox.SkyColourStrength3);

            m_skyBoxMaterial.SetEffectParameter("_SkyIntensity", CurrentScene.SandBoxFile.Enviroment.SkyBox.SkyIntensity);
            //m_skyBoxMaterial.SetEffectParameter("_SunColor", CurrentScene.SandBoxFile.Enviroment.SkyBox.SunColor);
            //m_skyBoxMaterial.SetEffectParameter("_SunIntensity", CurrentScene.SandBoxFile.Enviroment.SkyBox.SunIntensity);
            //m_skyBoxMaterial.SetEffectParameter("_SunAlpha", CurrentScene.SandBoxFile.Enviroment.SkyBox.SunAlpha);
            //m_skyBoxMaterial.SetEffectParameter("_SunBeta", CurrentScene.SandBoxFile.Enviroment.SkyBox.SunBeta);
            //m_skyBoxMaterial.SetEffectParameter("_SunVector", CurrentScene.SandBoxFile.Enviroment.SkyBox.SunVector);
            // set transforms
            m_skyBoxMaterial.World = Transform.Matrix4x4Transform;
            m_skyBoxMaterial.WorldInverseTranspose = Transform.RenderPassData.WorldInvT;
            m_skyBoxMaterial.WVP = Transform.RenderPassData.WVP;

            // set camera values
            m_skyBoxMaterial.View = Camera.View;
            m_skyBoxMaterial.Projection = Camera.Projection;
            m_skyBoxMaterial.CameraPosition = Camera.Position;
            m_skyBoxMaterial.LightDirection = Scene.LightPositions;
        }
    }
}
