#if VIRTICES_OLD
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

//Virtex vxEngine Declaration
using VerticesEngine;
using BEPUphysics.UpdateableSystems;
using VerticesEngine.Scenes.Sandbox.Entities;
using VerticesEngine.Scenes.Sandbox;
using VerticesEngine.Utilities;
using VerticesEngine.Diagnostics;
using VerticesEngine.Entities.Sandbox3D;
//using VerticesEngine.Entities.Sandbox3D.Util;
using VerticesEngine.Scenes.Sandbox3D;
using VerticesEngine.Cameras;

namespace VerticesEngine.Entities
{
	public enum vxEnumWaterReflectionQuality
	{
		Ultra,
		High,
		Medium,
		Low,
		Simple,
		None
	}

    public class vxOldWaterEntity : vxEntity3D
    {
        public Plane WrknPlane;

        public Texture2D waterBumpMap;

        public Texture2D waterDistortionMap;

        public Vector2 WaterFlowOffset = new Vector2(0, 0);

        public float xWaveLength = 0.25f;

        public float xWaveHeight = 0.15f;

        public FluidVolume fluidVolume;

        public Vector3 WaterScale = Vector3.One;

        vxScaleCube scPosition;
        vxScaleCube scLeft;
        vxScaleCube scRight;
        vxScaleCube scForward;
        vxScaleCube scBack;

        public static vxEntity3DRegistrationInfo Info
        {
            get
            {
                return new vxEntity3DRegistrationInfo(true,
                    typeof(vxWaterEntity).ToString(),
                "Water",
                "Models/water_plane/water_plane",
                    delegate (vxEngine Engine)
                    {
                        return new vxWaterEntity(Engine, Vector3.Zero, new Vector3(25, 5, 25));
                    });
            }
        }

        /// <summary>
        /// Creates a New Instance of the Base Ship Class
        /// </summary>
        /// <param name="AssetPath"></param>
        public vxOldWaterEntity(vxEngine Engine, Vector3 StartPosition, Vector3 WaterScale)
            : base(Engine, vxInternalAssets.Models.WaterPlane, StartPosition)
        {
            Engine.Current3DSceneBase.waterItems.Add(this);

            waterBumpMap = vxInternalAssets.Textures.Texture_WaterWaves;
            waterDistortionMap = vxInternalAssets.Textures.Texture_WaterDistort;

            //Render even in debug mode
            RenderEvenInDebug = true;

            this.WaterScale = WaterScale;

            //WrknPlane = new Plane(0, 1, 0, StartPosition.Y);
            Position = StartPosition;




            //Set up Scale Cubes
            scPosition = new vxScaleCube(Engine, StartPosition);
            scPosition.Moved += ScPosition_Moved;

            scLeft = new vxScaleCube(Engine, StartPosition + Vector3.Backward * WaterScale.Z);
            scLeft.Moved += Sc_Moved;
            scRight = new vxScaleCube(Engine, StartPosition - Vector3.Backward * WaterScale.Z);
            scRight.Moved += Sc_Moved;

            scForward = new vxScaleCube(Engine, StartPosition + Vector3.Right * WaterScale.X);
            scForward.Moved += Sc_Moved;
            scBack = new vxScaleCube(Engine, StartPosition - Vector3.Right * WaterScale.X);
            scBack.Moved += Sc_Moved;


            //World = Matrix.CreateScale(WaterScale);
            //World *= Matrix.CreateTranslation(Position);

            //
            //Physics Body
            //
            var tris = new List<Vector3[]>();
            //Remember, the triangles composing the surface need to be coplanar with the surface.  In this case, this means they have the same height.
            tris.Add(new[]
                         {
                            new Vector3(Position.X - WaterScale.X, Position.Y, Position.Z- WaterScale.Z),
                            new Vector3(Position.X + WaterScale.X, Position.Y, Position.Z- WaterScale.Z),
                            new Vector3(Position.X - WaterScale.X, Position.Y, Position.Z + WaterScale.Z),
                         });
            tris.Add(new[]
                         {
                            new Vector3(Position.X - WaterScale.X, Position.Y, Position.Z+ WaterScale.Z),
                            new Vector3(Position.X + WaterScale.X, Position.Y, Position.Z- WaterScale.Z),
                            new Vector3(Position.X + WaterScale.X, Position.Y, Position.Z + WaterScale.Z),
                         });
            fluidVolume = new FluidVolume(Vector3.Up, -9.81f, tris, WaterScale.Y, density, 0.9f, .4f);

            Current3DScene.BEPUPhyicsSpace.Add(fluidVolume);
        }


        float density = 1.05f;

        public override void OnWorldMatrixChange(object sender, EventArgs e)
        {
            base.OnWorldMatrixChange(sender, e);

            //Set the position
            scPosition.Position = this.Position + new Vector3(0, 0, 0);
            ResetScaleCubes();
        }

        private void ScPosition_Moved(object sender, EventArgs e)
        {
            this.Position = scPosition.World.Translation;
            ResetScaleCubes();
        }

        private void Sc_Moved(object sender, EventArgs e)
        {
            //Set the Main Position as the average of the top scale cubes
            this.Position = new Vector3((scForward.Position.X + scBack.Position.X) / 2,
                scPosition.Position.Y,
                (scLeft.Position.Z + scRight.Position.Z) / 2);

            //Set the position
            scPosition.Position = this.Position + new Vector3(0, 0, 0);

            //Set the water scale
            WaterScale = new Vector3(
                (scForward.Position.X - scBack.Position.X) / 2,
                WaterScale.Y,
                (scLeft.Position.Z - scRight.Position.Z) / 2);

            //Now reset all items
            ResetScaleCubes();
        }

        void ResetScaleCubes()
        {
            if (scLeft.SelectionState != vxEnumSelectionState.Selected)
                scLeft.Position = this.Position + Vector3.Backward * WaterScale.Z;
            if (scRight.SelectionState != vxEnumSelectionState.Selected)
                scRight.Position = this.Position - Vector3.Backward * WaterScale.Z;

            if (scForward.SelectionState != vxEnumSelectionState.Selected)
                scForward.Position = this.Position + Vector3.Right * WaterScale.X;
            if (scBack.SelectionState != vxEnumSelectionState.Selected)
                scBack.Position = this.Position - Vector3.Right * WaterScale.X;



            //
            //Reset the Physics Body
            //
            Current3DScene.BEPUPhyicsSpace.Remove(fluidVolume);
            var tris = new List<Vector3[]>();
            //Remember, the triangles composing the surface need to be coplanar with the surface.  In this case, this means they have the same height.
            tris.Add(new[]
                         {
                            new Vector3(Position.X - WaterScale.X, Position.Y, Position.Z- WaterScale.Z),
                            new Vector3(Position.X + WaterScale.X, Position.Y, Position.Z- WaterScale.Z),
                            new Vector3(Position.X - WaterScale.X, Position.Y, Position.Z + WaterScale.Z),
                         });
            tris.Add(new[]
                         {
                            new Vector3(Position.X - WaterScale.X, Position.Y, Position.Z+ WaterScale.Z),
                            new Vector3(Position.X + WaterScale.X, Position.Y, Position.Z- WaterScale.Z),
                            new Vector3(Position.X + WaterScale.X, Position.Y, Position.Z + WaterScale.Z),
                         });
            fluidVolume = new FluidVolume(Vector3.Up, -9.81f, tris, WaterScale.Y, density, 0.9f, .4f);

            Current3DScene.BEPUPhyicsSpace.Add(fluidVolume);
        }

        public override void InitShaders()
		{
			base.InitShaders();

			if (this.Model != null)
			{
				if (this.Model.ModelMain != null)
				{
					foreach (var part in this.Model.ModelMain.Meshes.SelectMany(m => m.MeshParts))
					{
						//if (part.Effect.Parameters["fFresnelBias"] != null)
						part.Effect.Parameters["fFresnelBias"].SetValue(0.025f);
						part.Effect.Parameters["fFresnelPower"].SetValue(8.0f);
						part.Effect.Parameters["fHDRMultiplier"].SetValue(0.45f);
						part.Effect.Parameters["vDeepColor"].SetValue(new Vector4(0.0f, 0.20f, 0.4150f, 1.0f));
						part.Effect.Parameters["vShallowColor"].SetValue(new Vector4(0.35f, 0.55f, 0.55f, 1.0f));
						//part.Effect.Parameters["fReflectionAmount"].SetValue(0.5f);
						part.Effect.Parameters["fWaterAmount"].SetValue(0.5f);

                        if (part.Effect.Parameters["PoissonKernel"] != null)
                            part.Effect.Parameters["PoissonKernel"].SetValue(Engine.Renderer.PoissonKernel);

                        if (part.Effect.Parameters["RandomTexture3D"] != null)
                            part.Effect.Parameters["RandomTexture3D"].SetValue(Engine.Renderer.RandomTexture3D);
                        if (part.Effect.Parameters["RandomTexture2D"] != null)
                            part.Effect.Parameters["RandomTexture2D"].SetValue(Engine.Renderer.RandomTexture2D);

                    }
				}
			}
		}

        //public override void RenderMask(vxCamera3D Camera) { }
        public override void RenderMesh(string RenderTechnique, vxCamera3D Camera) { }

        public override void RenderReflectionMap(vxCamera3D Camera)
        {
            IsEntityDrawable = true;
            base.RenderReflectionMap(Camera);
        }

        public override void PreSave()
        {
            base.PreSave();

            UserDefinedData02 = string.Format("{0};{1};{2}", WaterScale.X, WaterScale.Y, WaterScale.Z);
        }

        public override void PostLoad()
        {
            string[] vars = UserDefinedData02.Split(';');

            if(vars.Length > 2)
                WaterScale = new Vector3(float.Parse(vars[0]), float.Parse(vars[1]), float.Parse(vars[2]));

            ResetScaleCubes();

            base.PostLoad();
        }

        public override void DisposeEntity()
        {
            if (scPosition != null)
                scPosition.DisposeEntity();

            scLeft.DisposeEntity();
            scRight.DisposeEntity();
            scForward.DisposeEntity();
            scBack.DisposeEntity();
            
            CurrentSandboxLevel.waterItems.Remove(this);

            Current3DScene.BEPUPhyicsSpace.Remove(fluidVolume);
            base.DisposeEntity();
        }

		int modelscalar = 1;
        /// <summary>
        /// Applies a simple rotation to the ship and animates position based
        /// on simple linear motion physics.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
#if !VRTC_PLTFRM_XNA
			modelscalar = 200;
#endif
            World = Matrix.Identity;
            World *= Matrix.CreateRotationX(-MathHelper.PiOver2);
            World *= Matrix.CreateScale(WaterScale / modelscalar);
            World *= Matrix.CreateTranslation(Position);

            //WrknPlane.D = 0;// World.Translation.Y;
            
            base.Update(gameTime);
        }



        public void DrawWater(vxCamera3D Camera)
        {
            //vxDebugShapeRenderer.AddBoundingBox(fluidVolume.BoundingBox, Color.Blue);
            WaterFlowOffset += Vector2.UnitX / 3000;
            if (Model.ModelMain != null)
            {
                // Look up the bone transform matrices.
                Matrix[] transforms = new Matrix[Model.ModelMain.Bones.Count];

                Model.ModelMain.CopyAbsoluteBoneTransformsTo(transforms);

                // Draw the model.
                foreach (ModelMesh mesh in Model.ModelMain.Meshes)
                {
                    foreach (Effect effect in mesh.Effects)
                    {
                        // Specify which effect technique to use.
                        effect.CurrentTechnique = effect.Techniques["Water"];

                        effect.Parameters["World"].SetValue(World);
                        effect.Parameters["View"].SetValue(Camera.View);
                        effect.Parameters["Projection"].SetValue(Camera.Projection);
                        //effect.Parameters["ReflectionView"].SetValue(reflectionViewMatrix);
                        //effect.Parameters["ReflectionMap"].SetValue(reflectionMap);
                        effect.Parameters["xWaterBumpMap"].SetValue(waterBumpMap);
                        effect.Parameters["xWaveLength"].SetValue(xWaveLength);
                        effect.Parameters["xWaveHeight"].SetValue(xWaveHeight);
                        effect.Parameters["WaterFlowOffset"].SetValue(WaterFlowOffset);

                        effect.Parameters["xCamPos"].SetValue(Camera.Position);
                        effect.Parameters["xLightDirection"].SetValue(Engine.Renderer.lightPosition);
                    }
                    mesh.Draw();
                }
            }
        }
        /*
        /// <summary>
    /// Draws the Models to the Distortion Target
    /// </summary>
    public override void DrawModelDistortion(vxEngine Engine, GameTime gameTime)
    {
            // draw the distorter
            Matrix worldView = World * Camera.View;

            // make sure the depth buffering is on, so only parts of the scene
            // behind the distortion effect are affected
            Engine.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    effect.CurrentTechnique =
                        effect.Techniques["Distortion"];
                    effect.Parameters["WorldView"].SetValue(worldView);
                    effect.Parameters["WorldViewProjection"].SetValue(worldView * Camera.Projection);
                    effect.Parameters["DisplacementMap"].SetValue(waterDistortionMap);
                    effect.Parameters["offset"].SetValue(0);

                    effect.Parameters["DistortionScale"].SetValue(DistortionScale);
                    effect.Parameters["Time"].SetValue((float)gameTime.TotalGameTime.TotalSeconds);
                }
                mesh.Draw();
            }
       
    } */
    }
}
#endif