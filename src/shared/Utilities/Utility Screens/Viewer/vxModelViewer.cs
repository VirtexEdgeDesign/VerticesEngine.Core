using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using VerticesEngine.Diagnostics;
using VerticesEngine.Editor.Entities;
using VerticesEngine.Entities;
using VerticesEngine.Input;
using VerticesEngine.Util;
using VerticesEngine.Utilities;

namespace VerticesEngine.Screens
{
    /// <summary>
    /// This is a basic model view class which allows for basic viewing and editing shader values
    /// of models as well as is inhertiable to add extra functions to it.
    /// </summary>
    public class vxModelViewer : vxGameplayScene3D
    {
        public vxEntity3D Model;

        public vxModelViewer():base(vxStartGameMode.Editor)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

        }

        protected virtual vxEntity3D LoadModel()
        {
            return null;    
        }

		protected override Texture2D GenerateSandboxItemIcon(vxSandboxEntityRegistrationInfo EntityDescription)
		{
			var Icon = vxInternalAssets.Textures.DefaultDiffuse;

			string folderPath = vxIO.PathToCacheFolder + "/SandboxIcons";
			if (Directory.Exists(folderPath) == false)
				Directory.CreateDirectory(folderPath);

			string pngFilePath = Path.Combine(folderPath, EntityDescription.Type.Name + "_icon.png");

			string xnbFilePath = Path.Combine("txtrs", "sandbox", "entity_icons", EntityDescription.Type.Name + "_icon");

			// check if it exists in the game content
			if (File.Exists(Path.Combine(vxEngine.Game.Content.RootDirectory, EntityDescription.FilePath + "_icon.xnb")))
			{
				Icon = vxEngine.Game.Content.Load<Texture2D>(EntityDescription.FilePath + "_icon");
			}

			else if (File.Exists(Path.Combine(vxEngine.Game.Content.RootDirectory, xnbFilePath + ".xnb")))
			{
				Icon = vxEngine.Game.Content.Load<Texture2D>(xnbFilePath);
			}
			else
            {
				Icon = base.GenerateSandboxItemIcon(EntityDescription);
            }
			return Icon;
		}

        protected override void OnFirstUpdate()
        {
            base.OnFirstUpdate();


			vxGizmo.Instance.IsEnabled = true;
			vxGizmo.Instance.IsVisible = true;
			vxWorkingPlane.Instance.IsEnabled = true;
		}

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            // Initialise the Cameera
            foreach (var camera in Cameras)
            {
                vxCamera3D Camera = (vxCamera3D)camera;
                Camera.CameraType = vxCameraType.SceneEditor;
                Camera.OrbitTarget = Vector3.Zero;
                Camera.OrbitZoom = 750;
                Camera.ReqYaw = MathHelper.PiOver4;
                Camera.ReqPitch = -MathHelper.PiOver4 * 2 / 3;


				Camera.Position = Vector3.One * 25;
				Camera.Yaw = MathHelper.PiOver4;
				Camera.Pitch = -MathHelper.PiOver4 * 2 / 3;

				float scale = 2 * Camera.FieldOfView / MathHelper.PiOver4;
			}

			
            //Plane = new vxEntity3D(this, vxInternalAssets.Models.ViewerPlane, Vector3.Zero);

            Model = LoadModel();

            // Now move the plane down by the radius of the Models bounding sphere + a buffer
            //float deltaY = Model.BoundingShape.Radius;
            //Plane.World = Plane.World * Matrix.CreateTranslation(new Vector3(0, -deltaY, 0));

            vxInput.IsCursorVisible = true;
			vxSkyBox.Instance.IsVisible = true;

        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        float angle = 0;
        /// <summary>
        /// If the model requires special modfcation's each loop, you can override this method to provide it there.
        /// </summary>
        /// <param name="gameTime"></param>
        protected virtual void UpdateModel()
        {
            //Model.WorldTransform = Matrix.CreateScale(1.0f) * Matrix.CreateRotationY(angle);

        }


        /// <summary>
        /// Updates Main Gameplay Loop code here, this is affected by whether or not the scene is paused.
        /// </summary>
        protected internal override void UpdateScene()
        {
            angle += 0.01f;

            if (Model != null)
                UpdateModel();

            base.UpdateScene();
            IsGUIVisible = true;



            // Initialise the Cameera
            foreach (var camera in Cameras)
            {
                vxCamera3D Camera = (vxCamera3D)camera;
                Camera.CanTakeInput = !UIManager.HasFocus;

                int size = 5;
                vxDebug.DrawLine(Camera.OrbitTarget + Vector3.Right, Camera.OrbitTarget + Vector3.Right * size, Color.Red);
                vxDebug.DrawLine(Camera.OrbitTarget + Vector3.Up, Camera.OrbitTarget + Vector3.Up * size, Color.Lime);
                vxDebug.DrawLine(Camera.OrbitTarget + Vector3.Backward, Camera.OrbitTarget + Vector3.Backward * size, Color.Blue);
            }
        }


        public override void DrawScene()
        {
            base.DrawScene();
			
            IsGUIVisible = true;
			vxInput.IsCursorVisible = true;
        }

    }
}