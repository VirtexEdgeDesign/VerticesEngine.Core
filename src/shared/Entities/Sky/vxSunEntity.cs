
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VerticesEngine.Graphics;
using VerticesEngine.Input;
using VerticesEngine.Utilities;

namespace VerticesEngine.Entities
{
    /// <summary>
    /// The Sun entity holds the current enviroment lighting position in both world and screen space
    /// </summary>
    public class vxSunEntity : vxEntity3D
    {
        /// <summary>
        /// The sun texture for this entitiy
        /// </summary>
        public Texture2D SunTexture = vxInternalAssets.Textures.Texture_Sun_Glow;

        /// <summary>
        /// This World Position of the "Sun"
        /// </summary>
        public Vector3 SunWorldPosition
        {
            get { return m_sunWorldPosition; }
        }
        private Vector3 m_sunWorldPosition = Vector3.One * 10000;

        public float SunFarScale = 100000;


        public float RotationX
        {
            get
            {
                return m_rotationX;
            }

            set
            {
                m_rotationX = value;
                UpdateSunWorld();
            }
        }
        private float m_rotationX = 0.75f;

        public float RotationY
        {
            get
            {
                return m_rotationY;
            }

            set
            {
                m_rotationY = value;
                UpdateSunWorld();
            }
        }
        private float m_rotationY = 0.6f;

        public float RotationZ
        {
            get
            {
                return m_rotationZ;
            }

            set
            {
                m_rotationZ = value;
                UpdateSunWorld();
            }
        }
        private float m_rotationZ = 0.6f;

        public float SunSize
        {
            get
            {
                return m_sunSize;
            }

            set
            {
                m_sunSize = value;
            }
        }
        private float m_sunSize = 4.0f;

        public Vector3 LightDirection
        {
            get { return Vector3.Normalize(SunWorldPosition); }
        }


        public static vxSunEntity Instance
        {
            get { return _instance; }
        }
        private static vxSunEntity _instance;

        /// <summary>
        /// SnapBox for allowing tracks to snap together
        /// </summary>
        public vxSunEntity()
        {
            _instance = this;

            IsEntityCullable = false;

            RemoveSandboxOption(SandboxOptions.Save);
            RemoveSandboxOption(SandboxOptions.Export);
            RemoveSandboxOption(SandboxOptions.Delete);

            //IsShadowCaster = false;

            IsMotionBlurEnabled = false;
        }

        // update the rotation
        protected internal override void Update()
        {
            base.Update();
            
            m_rotationX = CurrentScene.SandBoxFile.Enviroment.SkyBox.SunRotX;
            m_rotationY = CurrentScene.SandBoxFile.Enviroment.SkyBox.SunRotY;
            m_rotationZ = CurrentScene.SandBoxFile.Enviroment.SkyBox.SunRotZ;
            SunSize = CurrentScene.SandBoxFile.Enviroment.SkyBox.SunSize;

            UpdateSunWorld();
        }

        public Vector3 ScreenSpacePosition;

        /// <summary>
        /// Returns the screen space position for the current world
        /// </summary>
        /// <returns></returns>
        public Vector3 GetScreenSpacePosition(vxCamera camera)
        {
            ScreenSpacePosition = vxGraphics.GraphicsDevice.Viewport.Project(
                       SunWorldPosition,
                       camera.Projection,
                       camera.View, Matrix.Identity);

            return ScreenSpacePosition;
        }


        protected override vxMesh OnLoadModel()
        {
            return null;
        }

        private void UpdateSunWorld()
        {
            //TODO: Implement Euler angles
            //WorldTransform = Matrix.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(RotationY, RotationZ, RotationX));
            Transform.Rotation = Quaternion.CreateFromYawPitchRoll(RotationY, RotationZ, RotationX);

            m_sunWorldPosition = Vector3.Transform(new Vector3(0, 0, SunFarScale), Transform.Matrix4x4Transform);

            Scene.LightPositions = new Vector3(SunWorldPosition.X, -SunWorldPosition.Y, -SunWorldPosition.Z);
        }
    }
}