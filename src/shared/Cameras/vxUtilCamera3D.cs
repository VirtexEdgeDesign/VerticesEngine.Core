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
    public class vxUtilCameraEntity : vxEntity3D
    {
        public vxUtilCamera3D Camera;
        public vxUtilCameraEntity(vxGameplayScene3D Scene) : base(Scene, Vector3.Zero)
        {
            Camera = new vxUtilCamera3D(Scene, vxCameraType.Orbit, Vector3.Zero);

            EntityRenderer.IsRenderedForUtilCamera = false;
            RemoveSandboxOption(SandboxOptions.Save);
        }

        public void UpdateCamera()
        {
            Update();
        }


        protected override vxMesh OnLoadModel()
        {
            return vxInternalAssets.Models.UnitSphere;
        }

        protected override void OnWorldTransformChanged()
        {
            base.OnWorldTransformChanged();

            Camera.Transform = this.Transform;

            Camera.View = Matrix.Invert(Transform.Matrix4x4Transform);
            //IsVisible = false;
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();

            if (Camera != null)
            {
                Camera.Dispose();
                Camera = null;
            }
        }
    }
    /// <summary>
    /// A utility camera
    /// </summary>
    public class vxUtilCamera3D : vxCamera3D 
	{
        public bool IsUtilCameraActive = false;

        protected override bool IsUtilCamera
        {
            get { return true; }
        }


        public vxUtilCamera3D(vxGameplaySceneBase sceneBase, vxCameraType CameraType, Vector3 Position,
						  float Pitch = 0, float Yaw = 0, float NearPlane = 0.1f, float FarPlane = 1000, float FieldOfView = MathHelper.PiOver4) : 
			base(sceneBase, CameraType, Position, Pitch, Yaw, NearPlane, FarPlane, FieldOfView)
        {
			sceneBase.UtilCameras.Add(this);
            RemoveSandboxOption(SandboxOptions.Save);
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();

			if(CurrentScene != null && CurrentScene.UtilCameras.Contains(this))
            {
                CurrentScene.UtilCameras.Remove(this);
            }

            IsUtilCameraActive = false;
        }  
    }
}