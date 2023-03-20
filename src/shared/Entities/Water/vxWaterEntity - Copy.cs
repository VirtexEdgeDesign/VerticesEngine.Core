
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Linq;

//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.Input;

////Virtex vxEngine Declaration
//using VerticesEngine;
//using BEPUphysics.UpdateableSystems;
//using VerticesEngine.Utilities;
//using VerticesEngine.Diagnostics;
//using VerticesEngine.Cameras;
//using BEPUphysics.Entities;
//using BEPUphysics.Entities.Prefabs;
//using BEPUphysics.BroadPhaseEntries;
//using BEPUutilities;
//using VerticesEngine.Entities.Util;
//using VerticesEngine.Scenes;
//using VerticesEngine.Graphics;

//namespace VerticesEngine.Entities
//{
//    [vxSandbox3DItem("Water", "Engine Items", "Basic")]
//    public class vxWaterEntity : vxEntity3D
//    {
//        Matrix preMat = Matrix.Identity;

//        [vxShowInInspector("Water Properties")]
//        public Vector3 WaterScale
//        {
//            get { return _waterScale; }
//            set
//            {
//                _waterScale = value;
//                OnScaleCubeMoved(this, null);
//            }
//        }
//        public Vector3 _waterScale = Vector3.One;




//        /// <summary>
//        /// The Physics Collider Skin
//        /// </summary>
//        //public Box entity;
//       // Terrain terrin;
//       // float[,] HeightData;

//        FluidVolume fluidVolume;


//        // Scaling Cubes
//        //vxScaleCube scPosition;
//        vxScaleCube scLeft;
//        vxScaleCube scRight;
//        vxScaleCube scForward;
//        vxScaleCube scBack;

//        //public static vxEntityRegistrationInfo Info
//        //{
//        //    get
//        //    {
//        //        return new vxEntityRegistrationInfo(true,
//        //            typeof(vxWaterEntity).ToString(),
//        //        "Water",
//        //        "Models/water/water_plane",
//        //                                            delegate (vxGameplayScene3D scene)
//        //            {
//        //                return new vxWaterEntity(scene, Vector3.Zero, new Vector3(100, 5, 100));
//        //            });
//        //    }
//        //}

//        float ModelScale = 250;

//        /// <summary>
//        /// Creates a New Instance of a Water Entity
//        /// </summary>
//        /// <param name="Engine"></param>
//        /// <param name="StartPosition"></param>
//        /// <param name="WaterScale"></param>
//        public vxWaterEntity(vxGameplayScene3D scene) : base(scene, vxInternalAssets.Models.WaterPlane, Vector3.Zero)
//        {
//            //Scene.WaterEntities.Add(this);

//            //int dim = 3;
//            /*
//            HeightData = new float[dim, dim];

//            for (int i = 0; i < dim; i++)
//                for (int j = 0; j < dim; j++)
//                {
//                    HeightData[i, j] = 0;
//                }
//                */

//            //Render even in debug mode
//            RenderEvenInDebug = true;

//            // Set Initial Position
//            //Position = StartPosition;

//            // Load the Distortion Map
//            //DistortionMap = vxInternalAssets.LoadInternalTexture2D(vxWaterEntity.Info.FilePath + "_dm");

//            // Set the Water Scale
//            this._waterScale = new Vector3(100, 5, 100);// WaterScale / (ModelScale * 2);

//            // Finally Set up the Physics Bodies
//            SetUpPhysics();

//            Tag = this;

//            //Set up Scale Cubes
//            SetupCubes();

//        }

//        void SetupCubes()
//        {
//            scLeft = new vxScaleCube(Scene, StartPosition + Vector3.Backward * _waterScale.Z, this);
//            scLeft.Moved += OnScaleCubeMoved;
//            scRight = new vxScaleCube(Scene, StartPosition - Vector3.Backward * _waterScale.Z, this);
//            scRight.Moved += OnScaleCubeMoved;

//            scForward = new vxScaleCube(Scene, StartPosition + Vector3.Right * _waterScale.X, this);
//            scForward.Moved += OnScaleCubeMoved;
//            scBack = new vxScaleCube(Scene, StartPosition - Vector3.Right * _waterScale.X, this);
//            scBack.Moved += OnScaleCubeMoved;

//            Scene.EditorEntities.Add(scLeft);
//            Scene.EditorEntities.Add(scRight);
//            Scene.EditorEntities.Add(scForward);
//            Scene.EditorEntities.Add(scBack);
//        }

//        public override void OnAdded()
//        {
//            base.OnAdded();

//            // Setup ID's
//            scLeft.Id = this.Id + ".Left";
//            scRight.Id = this.Id + ".Right";
//            scForward.Id = this.Id + ".Forward";
//            scBack.Id = this.Id + ".Back";
//        }

//        void DestroyCubes()
//        {
//            scLeft.Dispose();
//            scRight.Dispose();
//            scForward.Dispose();
//            scBack.Dispose();
//        }

//        /// <summary>
//        /// Called whenever the skybox is changed.
//        /// </summary>
//        public virtual void OnSkyboxChange()
//        {
//            InitShaders();
//        }






//        public override void InitShaders()
//        {
//            base.InitShaders();

//            if (this.Model != null)
//            {
//                // TODO: Create a Water Material

//                //foreach (vxModelMesh mesh in Model.Meshes)
//                //{
//                //    mesh.MainEffect.CurrentTechnique = mesh.MainEffect.Techniques[RenderTechnique];

//                //    //mesh.MainEffect.Parameters["BumpMap"].SetValue(

//                //    mesh.MainEffect.Parameters["ReflectionCube"].SetValue(Scene.SkyBox.SkyboxTextureCube);
//                //    mesh.MainEffect.Parameters["fFresnelPower"].SetValue(8.0f);
//                //    mesh.MainEffect.Parameters["vDeepColor"].SetValue(Color.DeepSkyBlue.ToVector4() * 0.5f);
//                //    mesh.MainEffect.Parameters["vShallowColor"].SetValue(Color.DeepSkyBlue.ToVector4() * 1.2f);
//                //    mesh.MainEffect.Parameters["fWaterAmount"].SetValue(0.5f);
//                //    DistortionScale = 0.25f;
//                //    DoSSR = true;
//                //    //DoSSRReflection;
//                //    IsDistortionEnabled = true;
//                //}

//                /*
//                if (this.Model.ModelMain != null)
//                {
//                    foreach (var part in this.Model.ModelMain.Meshes.SelectMany(m => m.MeshParts))
//                    {
//                        //if (part.Effect.Parameters["NormalMap"] != null)
//                        part.Effect.Parameters["BumpMap"].SetValue(
//                            vxInternalAssets.Load<Texture2D>("Models/water/waterbump"));

//                        part.Effect.Parameters["ReflectionCube"].SetValue(Scene.SkyBox.SkyboxTextureCube);

//                        part.Effect.Parameters["fFresnelPower"].SetValue(8.0f);
//                        //part.Effect.Parameters["fHDRMultiplier"].SetValue(0.45f);
//                        part.Effect.Parameters["vDeepColor"].SetValue(Color.DeepSkyBlue.ToVector4() * 0.5f);
//                        //part.Effect.Parameters["vDeepColor"].SetValue(new Vector4(0.0f, 0.20f, 0.4150f, 1.0f));
//                        //part.Effect.Parameters["vShallowColor"].SetValue(new Vector4(0.35f, 0.55f, 0.55f, 1.0f));
//                        part.Effect.Parameters["vShallowColor"].SetValue(Color.DeepSkyBlue.ToVector4() * 1.2f);
//                        //part.Effect.Parameters["fReflectionAmount"].SetValue(0.5f);
//                        part.Effect.Parameters["fWaterAmount"].SetValue(0.5f);
//                        DistortionScale = 0.25f;
//                    }
//                }
//                */
//            }
//        }

//        /// <summary>
//        /// Sets up the Physics Entities
//        /// </summary>
//        public virtual void SetUpPhysics()
//        {
//            // Terrain
//            // ------------------
//            /*
//            if (terrin == null)
//                terrin = new Terrain(HeightData, new BEPUutilities.AffineTransform(
//                    _waterScale, Quaternion.Identity, StartPosition));

//            terrin.CollisionRules.Personal = BEPUphysics.CollisionRuleManagement.CollisionRule.NoSolver;
//            */
//            //Scene.PhyicsSimulation.Add(terrin);
//            //Scene.PhysicsDebugViewer.Add(terrin);


//            // Fluid Volume
//            // ------------------

//            var tris = new List<Vector3[]>();
//            //Remember, the triangles composing the surface need to be coplanar with the surface.  In this case, this means they have the same height.
//            tris.Add(new[]
//                         {
//                new Vector3(Position.X - _waterScale.X*ModelScale, Position.Y, Position.Z- _waterScale.Z*ModelScale),
//                new Vector3(Position.X + _waterScale.X*ModelScale, Position.Y, Position.Z- _waterScale.Z*ModelScale),
//                new Vector3(Position.X - _waterScale.X*ModelScale, Position.Y, Position.Z + _waterScale.Z*ModelScale),
//                         });
//            tris.Add(new[]
//                         {
//                new Vector3(Position.X - _waterScale.X*ModelScale, Position.Y, Position.Z+ _waterScale.Z*ModelScale),
//                new Vector3(Position.X + _waterScale.X*ModelScale, Position.Y, Position.Z- _waterScale.Z*ModelScale),
//                new Vector3(Position.X + _waterScale.X*ModelScale, Position.Y, Position.Z + _waterScale.Z*ModelScale),
//                         });

//            fluidVolume = new FluidVolume(Vector3.Up, -9.81f, tris, _waterScale.Y * ModelScale, density, 0.9f, .4f);
//            //fluidVolume.Tag = Index;

//            Scene.PhyicsSimulation.Add(fluidVolume);
//            Scene.PhysicsDebugViewer.Add(fluidVolume);

//        }
//        float density = 20;

//        public override void OnSandboxStatusChanged(bool IsRunning)
//        {
//            base.OnSandboxStatusChanged(IsRunning);

//            ResetScaleCubes();
//        }

//        public override void OnFirstUpdate(GameTime gameTime)
//        {
//            base.OnFirstUpdate(gameTime);
//            //ResetScaleCubes();
//            OnWorldTransformChanged();
//        }

//        private void OnScaleCubeMoved(object sender, EventArgs e)
//        {
//            //Set the Main Position as the average of the top scale cubes
//            Vector3 NewPosition = new Vector3(
//    (scForward.Position.X + scBack.Position.X) / 2,
//    Position.Y,
//    (scLeft.Position.Z + scRight.Position.Z) / 2);

//            Position += (NewPosition - Position) * 2 / 3;


//            //Set the water scale
//            _waterScale = new Vector3(
//                (scForward.Position.X - scBack.Position.X) / (2 * ModelScale),
//                _waterScale.Y,
//                (scLeft.Position.Z - scRight.Position.Z) / (2 * ModelScale));



//            //Now reset all items
//            ResetScaleCubes();
//        }

//        Vector3 OutOfSightOffset = Vector3.Zero;
//        void ResetScaleCubes()
//        {
//            if (scLeft.SelectionState != vxSelectionState.Selected)
//                scLeft.Position = Position + Vector3.Backward * _waterScale.Z * ModelScale + OutOfSightOffset;
//            if (scRight.SelectionState != vxSelectionState.Selected)
//                scRight.Position = Position - Vector3.Backward * _waterScale.Z * ModelScale + OutOfSightOffset;

//            if (scForward.SelectionState != vxSelectionState.Selected)
//                scForward.Position = Position + Vector3.Right * _waterScale.X * ModelScale + OutOfSightOffset;
//            if (scBack.SelectionState != vxSelectionState.Selected)
//                scBack.Position = Position - Vector3.Right * _waterScale.X * ModelScale + OutOfSightOffset;

//            /*
//            //Reset the Physics Body
//            Scene.PhysicsDebugViewer.Remove(terrin);
//            Scene.PhyicsSimulation.Remove(terrin);

//            terrin.WorldTransform = new AffineTransform(
//                new Vector3(_waterScale.X * ModelScale, 1, _waterScale.Z * ModelScale),
//                Quaternion.Identity,
//                Position - new Vector3(_waterScale.X * ModelScale, 0, _waterScale.Z * ModelScale));

//            BoundingShape = BoundingSphere.CreateFromBoundingBox(terrin.BoundingBox);
//            //BoundingShape.Radius = WaterScale.Length() * ModelScale;

//            Scene.PhyicsSimulation.Add(terrin);
//            Scene.PhysicsDebugViewer.Add(terrin);
//            */



//            //Current3DScene.BEPUDebugDrawer.Remove(fluidVolume);
//            //Current3DScene.BEPUPhyicsSpace.Remove(fluidVolume);
//            //var tris = new List<Vector3[]>();
//            ////Remember, the triangles composing the surface need to be coplanar with the surface.  In this case, this means they have the same height.
//            //tris.Add(new[]
//            //             {
//            //                new Vector3(Position.X - WaterScale.X* ModelScale, Position.Y, Position.Z- WaterScale.Z* ModelScale),
//            //                new Vector3(Position.X + WaterScale.X* ModelScale, Position.Y, Position.Z- WaterScale.Z* ModelScale),
//            //                new Vector3(Position.X - WaterScale.X* ModelScale, Position.Y, Position.Z + WaterScale.Z* ModelScale),
//            //             });
//            //tris.Add(new[]
//            //             {
//            //                new Vector3(Position.X - WaterScale.X* ModelScale, Position.Y, Position.Z+ WaterScale.Z* ModelScale),
//            //                new Vector3(Position.X + WaterScale.X* ModelScale, Position.Y, Position.Z- WaterScale.Z* ModelScale),
//            //                new Vector3(Position.X + WaterScale.X* ModelScale, Position.Y, Position.Z + WaterScale.Z* ModelScale),
//            //             });
//            //fluidVolume = new FluidVolume(Vector3.Up, -9.81f, tris, 100, density, 0.9f, .4f);

//            //Current3DScene.BEPUPhyicsSpace.Add(fluidVolume);
//            //Current3DScene.BEPUDebugDrawer.Add(fluidVolume);

//            Scale = new Vector3(_waterScale.X * 2, 1, _waterScale.Z * 2);

//            //UVFactor = new Vector2(_waterScale.X, _waterScale.Z);
//        }

//        public override void OnWorldTransformChanged()
//        {
//            base.OnWorldTransformChanged();

//            //entity.WorldTransform = World;
//            //entity.WorldTransform = Matrix.CreateRotationX(MathHelper.PiOver2) * _world * Matrix.CreateTranslation(-Vector3.Up / 2);
//            /*
//            terrin.WorldTransform = new AffineTransform(
//                new Vector3(_waterScale.X * ModelScale, 1, _waterScale.Z * ModelScale),
//                Quaternion.Identity, Position);
//                */
//            ResetScaleCubes();
//        }

//        public override void Draw(vxCamera Camera, string renderpass)
//        {
//            if (renderpass == vxRenderer.Passes.TransparencyPass)
//            {
//                // Get Base Scale
//                BaseScale = MathHelper.Clamp(
//                (((vxCamera3D)Camera).Position - Position).Length(), 500, 100000) *
//                                  Vector3.One / (ModelScale) * _waterScale.Length() * 2;

//            //base.Draw(Camera, renderpass);
//                foreach (vxModelMesh mesh in Model.Meshes)
//                {
//                    if (renderpass == mesh.Material.MaterialRenderPass)
//                    {
//                        mesh.Material.World = WorldTransform;
//                        mesh.Material.WorldInverseTranspose = Matrix.Transpose(Matrix.Invert(WorldTransform));
//                        mesh.Material.WVP = WorldTransform * Camera.ViewProjection;
//                        mesh.Material.View = Camera.View;
//                        mesh.Material.Projection = projection;
//                        mesh.Draw();
//                    }
//                }
//            }
//        }

//        Vector3 BaseScale;
//        public override void Update(GameTime gameTime)
//        {
//            base.Update(gameTime);


//            // Now set the Scale Cube Sizes
//            float scale = 0.8f;
//            if (scLeft != null)
//            {
//                scLeft.Scale = BaseScale + Vector3.UnitX * _waterScale.X * ModelScale * scale;
//                scRight.Scale = BaseScale + Vector3.UnitX * _waterScale.X * ModelScale * scale;
//                scForward.Scale = BaseScale + Vector3.UnitZ * _waterScale.Z * ModelScale * scale;
//                scBack.Scale = BaseScale + Vector3.UnitZ * _waterScale.Z * ModelScale * scale;
//            }
//            //TextureUVOffset += Vector2.One * 0.001f;
//        }

//        /// <summary>
//        /// Renders the mesh.
//        /// </summary>
//        /// <param name="Camera">Camera.</param>
//        //public override void RenderMesh(vxCamera3D Camera)
//        //{
//        //    if (SelectionState == vxEnumSelectionState.None)
//        //        SelectionColour = Color.Transparent;
//        //}


//        //public override void RenderToShadowMap(int ShadowMapIndex) { }


//        public override void PreSave()
//        {
//            base.PreSave();

//            UserDefinedData02 = string.Format("{0};{1};{2}", _waterScale.X, _waterScale.Y, _waterScale.Z);
//        }

//        public override void PostEntityLoad()
//        {
//            if (UserDefinedData02 != null)
//                if (UserDefinedData02.Contains(";"))
//                {
//                    string[] vars = UserDefinedData02.Split(';');

//                    if (vars.Length > 2)
//                        _waterScale = new Vector3(float.Parse(vars[0]), float.Parse(vars[1]), float.Parse(vars[2]));

//                    ResetScaleCubes();
//                }
//            base.PostEntityLoad();
//        }

//        public override void Dispose()
//        {
//            //Scene.WaterEntities.Remove(this);
//            /*
//            Scene.PhyicsSimulation.Remove(terrin);
//            Scene.PhysicsDebugViewer.Remove(terrin);
//            */
//            DestroyCubes();

//            base.Dispose();
//        }



//        public void DrawWater(vxCamera3D Camera)
//        {
//            //// Draws the model using the Models Effect, but with the Utility Meshes Vertices
//            //foreach (vxModelMesh mesh in Model.Meshes)
//            //{
//            //    mesh.MainEffect.CurrentTechnique = mesh.MainEffect.Techniques["Technique_Water"];

//            //    mesh.MainEffect.Parameters["World"].SetValue(ScaleMatrix * WorldTransform);
//            //    mesh.MainEffect.Parameters["View"].SetValue(Camera.View);
//            //    mesh.MainEffect.Parameters["Projection"].SetValue(Camera.Projection);
//            //    mesh.MainEffect.Parameters["CameraPos"].SetValue(Camera.Position);
//            //    mesh.MainEffect.Parameters["LightDirection"].SetValue(Scene.Renderer.LightPosition);

//            //    mesh.Draw(mesh.MainEffect);
//            //}

//            //vxDebugShapeRenderer.AddBoundingBox(fluidVolume.BoundingBox, Color.Blue);
//            //WaterFlowOffset += Vector2.UnitX / 3000;
//            //if (Model.ModelMain != null)
//            //{
//            //    // Look up the bone transform matrices.
//            //    //Matrix[] transforms = new Matrix[Model.ModelMain.Bones.Count];
//            //    //Model.ModelMain.CopyAbsoluteBoneTransformsTo(transforms);

//            //    // Draw the model.
//            //    foreach (ModelMesh mesh in Model.ModelMain.Meshes)
//            //    {
//            //        foreach (Effect effect in mesh.Effects)
//            //        {
//            //            // Specify which effect technique to use.
//            //            effect.CurrentTechnique = effect.Techniques["Technique_Water"];

//            //            effect.Parameters["World"].SetValue(ScaleMatrix * World);
//            //            effect.Parameters["View"].SetValue(Camera.View);
//            //            effect.Parameters["Projection"].SetValue(Camera.Projection);
//            //            //effect.Parameters["WaterScale"].SetValue(WaterScale/32);
//            //            //effect.Parameters["ReflectionMap"].SetValue(reflectionMap);
//            //            //effect.Parameters["xWaterBumpMap"].SetValue(waterBumpMap);
//            //            //effect.Parameters["xWaveLength"].SetValue(xWaveLength);
//            //            //effect.Parameters["xWaveHeight"].SetValue(xWaveHeight);
//            //            //effect.Parameters["WaterFlowOffset"].SetValue(WaterFlowOffset);

//            //            effect.Parameters["CameraPos"].SetValue(Camera.Position);
//            //            effect.Parameters["LightDirection"].SetValue(Engine.Renderer.LightPosition);
//            //        }
//            //        mesh.Draw();
//            //    }
//            //}
//            if (vxDebug.IsDebugMeshVisible)
//                vxDebug.DrawBoundingSphere(this.BoundingShape, Color.LimeGreen);
//        }

//        /// <summary>
//        /// Draws the Models to the Distortion Target
//        /// </summary>
//        public void DrawModelDistortion(vxCamera3D Camera)
//        {
//            // draw the distorter
//            Matrix worldView = ScaleMatrix * WorldTransform * Camera.View;

//            // make sure the depth buffering is on, so only parts of the scene
//            // behind the distortion effect are affected
//            Engine.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

//            //foreach (ModelMesh mesh in Model.ModelMain.Meshes)
//            //{
//            //    foreach (Effect effect in mesh.Effects)
//            //    {
//            //        effect.CurrentTechnique =
//            //            effect.Techniques["Distortion"];
//            //        effect.Parameters["WorldView"].SetValue(worldView);
//            //        effect.Parameters["WorldViewProjection"].SetValue(worldView * Camera.Projection);
//            //        effect.Parameters["DisplacementMap"].SetValue(DistortionMap);
//            //        effect.Parameters["offset"].SetValue(0);
//            //        DistortionScale = 0.25f;
//            //        effect.Parameters["DistortionScale"].SetValue(DistortionScale);
//            //        //effect.Parameters["Time"].SetValue((float)gameTime.TotalGameTime.TotalSeconds);
//            //    }
//            //    mesh.Draw();
//            //}
//        }
//    }
//}
