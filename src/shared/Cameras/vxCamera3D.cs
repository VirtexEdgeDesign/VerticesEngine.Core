using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using VerticesEngine.Graphics;
using VerticesEngine.Input;
using VerticesEngine;
using VerticesEngine.Utilities;

namespace VerticesEngine
{
    /// <summary>
    /// Whether or not to use the default free-flight camera controls.
    /// Set to false when using vehicles or character controllers.
    /// </summary>
    public enum vxCameraType
	{
		Freeroam,
		CharacterFPS,
		ChaseCamera,
		Orbit,
		OnRails,

		/// <summary>
		/// The Scene Editor camera controll
		/// </summary>
        SceneEditor,

		/// <summary>
		/// An external camera controller. Set this if you're going to attach your own camera controller to it
		/// </summary>
		External
	}

    public enum vxCameraProjectionType
    {
        Perspective,
        Orthographic
    }

	/// <summary>
	/// Simple Camera class.
	/// </summary>
    public class vxCamera3D : vxCamera
	{
		/// <summary>
		/// Focal Distance Used During Depth of Field Calculations.
		/// </summary>
		public float FocalDistance
		{
			get { return _focalDistance; }
			set { _focalDistance = value; }
		}
		float _focalDistance = 40;


		/// <summary>
		/// Focal Width Used in the Depth of Field Calculations.
		/// </summary>
		public float FocalWidth
		{
			get { return _focalWidth; }
			set { _focalWidth = value; }
		}
		float _focalWidth = 75;


		/// <summary>
		/// Velocity of camera.
		/// </summary>
		public Vector3 Velocity
		{
			get { return _velocity; }
			set { _velocity = value; }
		}
		private Vector3 _velocity;


		/// <summary>
		/// Gets or sets the yaw rotation of the camera.
		/// </summary>
		public float Yaw
		{
			get { return _yaw; }
			set { _yaw = MathHelper.WrapAngle(value); }
		}
		private float _yaw;

		/// <summary>
		/// Gets or sets the pitch rotation of the camera.
		/// </summary>
		public float Pitch
		{
			get { return _pitch; }
			set
			{
				_pitch = value;
				if (_pitch > MathHelper.PiOver2 * .99f)
					_pitch = MathHelper.PiOver2 * .99f;
				else if (_pitch < -MathHelper.PiOver2 * .99f)
					_pitch = -MathHelper.PiOver2 * .99f;
			}
		}
		private float _pitch;


		//float _zoom = -15;

        #region -- Components --

        private vxCameraOrbitController OrbitController;
        private vxCameraFreeRoamController FreeRoamController;
        private vxCameraSceneEditorController SceneEditor;
        private vxCameraFpsController FpsController;
        private vxCameraChaseController ChaseController;

        #endregion

        public vxCamera3D(vxGameplaySceneBase sceneBase) : base(sceneBase)
        {

        }

		//public vxCamera3D(vxEngine Engine, CameraType CameraType, Vector3 position, float pitch, float yaw, Matrix projectionMatrix)
		public vxCamera3D(vxGameplaySceneBase sceneBase, vxCameraType CameraType,
						  Vector3 Position,
						  float Pitch = 0, float Yaw = 0,
						  float NearPlane = 0.1f, float FarPlane = 1000,
						 float FieldOfView = MathHelper.PiOver4) : base(sceneBase)
        {
            // add controllers
            OrbitController = AddComponent<vxCameraOrbitController>();
            FreeRoamController = AddComponent<vxCameraFreeRoamController>();
            FpsController = AddComponent<vxCameraFpsController>();
            SceneEditor = AddComponent<vxCameraSceneEditorController>();
            ChaseController = AddComponent<vxCameraChaseController>();


			// What type of Camera is it, Orbiting, FPS, Chase etc...
			this.CameraType = CameraType;

			// Set Position and Orientation
			this.Position = Position;
			this.Yaw = Yaw;
			this.Pitch = Pitch;

			// Now set Optional Items
			_fieldOfView = FieldOfView;
			_nearPlane = NearPlane;
			_farPlane = FarPlane;

            // Where is the Camera's Viewport on the Screen.
            Viewport = vxGraphics.GraphicsDevice.Viewport;
            _aspectRatio = Viewport.AspectRatio;


			CalculateProjectionMatrix();
        }

		public void DrawDebugOutput()
		{
			string output = string.Format("Viewport: {0}\nAspect Ratio:{1}", Viewport.ToString(), Viewport.AspectRatio);

            vxGraphics.SpriteBatch.Begin("Camera Debug");
            vxGraphics.SpriteBatch.DrawString(vxInternalAssets.Fonts.DebugFont, output, Vector2.Zero, Color.Black);
            vxGraphics.SpriteBatch.DrawString(vxInternalAssets.Fonts.DebugFont, output, Vector2.Zero - Vector2.One, Color.White);
            vxGraphics.SpriteBatch.End();
		}


        protected override void OnCameraTypeChanged()
        {
            // disable all camera controllers
            OrbitController.IsEnabled = CameraType == vxCameraType.Orbit;
            FreeRoamController.IsEnabled = CameraType == vxCameraType.Freeroam;
            FpsController.IsEnabled = CameraType == vxCameraType.CharacterFPS;
            SceneEditor.IsEnabled = CameraType == vxCameraType.SceneEditor;
            ChaseController.IsEnabled = CameraType == vxCameraType.ChaseCamera;
        }
	}
}