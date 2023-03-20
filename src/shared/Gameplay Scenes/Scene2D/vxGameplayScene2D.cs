using FarseerPhysics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using VerticesEngine;
using VerticesEngine.DebugUtilities;
using VerticesEngine.Diagnostics;

using VerticesEngine.Particles;
using VerticesEngine.Input;
using VerticesEngine.Utilities;
using VerticesEngine.Physics;

namespace VerticesEngine
{
    /// <summary>
    /// The vxGameplayScene2D class implements the actual game logic for 2D Games.
    /// </summary>
	public partial class vxGameplayScene2D : vxGameplaySceneBase
	{

		/// <summary>
		/// The distortion entities.
		/// </summary>
		public List<vxDistortionEntity2D> DistortionEntities = new List<vxDistortionEntity2D>();


		/// <summary>
		/// Physics World Space
		/// </summary>
		public World PhysicsSimulation;

        /// <summary>
        /// The gravity vector for the Physics Simulation. Note this is set at start time of
        /// this class, but can be changed afterwards.
        /// </summary>
        public static Vector2 DefaultGravity = Vector2.UnitY * 9.81f;

		/// <summary>
		/// The display to sim unit ratio for Farseer.
		/// </summary>
		//public static float DisplayToSimUnitRatio = 32;
        public static float DisplayToSimUnitRatio = 32;


		/// <summary>
		/// The allow camera input.
		/// </summary>
		public bool AllowCameraInput = true;


		/// <summary>
		/// Initializes a new instance of the <see cref="T:VerticesEngine.vxScene2D"/> class.
		/// </summary>
        public vxGameplayScene2D (vxStartGameMode startGameAsType = vxStartGameMode.GamePlay, string FilePath="") : 
        base(startGameAsType, FilePath)
		{

		}


        /// <summary>
        /// Initialises the level.
        /// </summary>
        protected override void InitialisePhysics ()
		{
            //Accelerometer.Initialize();   
            vxPhysicsSystem.Instance.SetPhysicsEngine(PhysicsEngineType.Farseer);

            ConvertUnits.SetDisplayUnitToSimUnitRatio(DisplayToSimUnitRatio);
            //FarseerPhysics.Settings.AllowSleep = false;
            //FarseerPhysics.Settings.PositionIterations = 10;
			//Initialise Physics vxEngine
			PhysicsSimulation = new World(DefaultGravity);

			DebugView = new vxFarseerDebugView(PhysicsSimulation);
			DebugView.RemoveFlags(DebugViewFlags.Shape);
			DebugView.RemoveFlags(DebugViewFlags.Joint);
			DebugView.DefaultShapeColor = Color.White;
			DebugView.SleepingShapeColor = Color.LightGray;
			DebugView.LoadContent();
		}

        protected override void InitialiseCameras()
		{
			//Camera = new vxCamera2D(Engine);
			//Camera.ResetCamera();

            // Setup Cameras
            for (int i = 0; i < NumberOfPlayers; i++)
            {
                vxCamera2D newCamera = new vxCamera2D(this);
                newCamera.ResetCamera();
                Cameras.Add(newCamera);
            }
            //SetCameraViewports();

        }


        protected override void LoadParticlePools()
        {
            foreach (var particleSet in vxEntityRegister.ParticleDefinitions.Values)
            {
                var particlePool = new vxParticlePool(particleSet.Type, particleSet.PoolSize);

                for (int i = 0; i < particleSet.PoolSize; i++)
                {
                    System.Reflection.ConstructorInfo ctor = particleSet.Type.GetConstructor(new[] { typeof(vxGameplayScene2D), typeof(Vector2), typeof(int) });
                    particlePool.Pool.Add((vxParticle2D)ctor.Invoke(new object[] { this, Vector2.Zero, i }));
                }
                ParticleSystem.AddPool(particlePool);
            }
        }


        protected internal override void OnGraphicsRefresh()
        {
            base.OnGraphicsRefresh();


            foreach (var Camera in Cameras)
            {
                Camera.ResetCamera();
                Camera.OnGraphicsRefresh();
            }
            foreach (var entity in Entities)
                entity.OnGraphicsRefresh();
        }

        /// <summary>
        /// Updates the state of the game. This method checks the vxGameBaseScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        protected internal override void Update()
        {
			base.Update();

            if (IsActive || IsPausable == false)
            {
                UpdateScene();
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected internal override void UpdateScene()
		{
            // Update Each Entity
            for (int i = 0; i < Entities.Count; i++)
            {
                if (Entities[i].IsEnabled)
                {
                    Entities[i].Update();
                }

            }
            for (int i = 0; i < Entities.Count; i++)
            {
                if (i < Entities.Count && Entities[i].IsEnabled)
                    Entities[i].PostUpdate();
            }

            //ParticleSystem.Update(gameTime);

			if(AllowCameraInput)
				HandleCamera ();

			Cameras[0].Update();


            //Engine.PhysicsDebugTimer.Start();
            vxProfiler.BeginMark("Physics");
            // variable time step but never less then 30 Hz
#if !DdEBUG
            try
			{
#endif
            float inc = vxTime.DeltaTime;
                PhysicsSimulation.Step(1f / 60f);


#if !DEdBUG
			}
			catch(Exception ex) {
                vxConsole.WriteError("Error Stepping Physics Simulation");
                vxConsole.WriteException (PhysicsSimulation,ex);
			}

#endif
            vxProfiler.EndMark("Physics");

            base.UpdateScene();
        }

		/// <summary>
		/// The camera move vector. Note: This is zero'd at the start of each loop.
		/// </summary>
		Vector2 camMove;

		/// <summary>
		/// The scroll sensitivity for zooming.
		/// </summary>
        float scrollSens = 1;

        private void HandleCamera()
		{
			camMove = Vector2.Zero;

            foreach (var Camera in Cameras)
            {

                if (vxInput.KeyboardState.IsKeyDown(Keys.Up))
                    camMove.Y -= 10f * vxTime.DeltaTime;
                if (vxInput.KeyboardState.IsKeyDown(Keys.Down))
                    camMove.Y += 10f * vxTime.DeltaTime;
                if (vxInput.KeyboardState.IsKeyDown(Keys.Left))
                    camMove.X -= 10f * vxTime.DeltaTime;
                if (vxInput.KeyboardState.IsKeyDown(Keys.Right))
                    camMove.X += 10f * vxTime.DeltaTime;
                if (vxInput.KeyboardState.IsKeyDown(Keys.PageUp))
                    Camera.Zoom += 5f * vxTime.DeltaTime * Camera.Zoom / 20f;
                if (vxInput.KeyboardState.IsKeyDown(Keys.PageDown))
                    Camera.Zoom -= 5f * vxTime.DeltaTime * Camera.Zoom / 20f;


                if (vxInput.MouseState.MiddleButton == ButtonState.Pressed)
                {
                    camMove = -Vector2.Subtract(vxInput.MouseState.Position.ToVector2(),
                        vxInput.PreviousMouseState.Position.ToVector2()) / 20;
                }

                //if (IsActive)
                {
                    if (vxInput.MouseState.ScrollWheelValue - vxInput.PreviousMouseState.ScrollWheelValue > 0)
                        Camera.Zoom += 5f * vxTime.DeltaTime * Camera.Zoom / scrollSens;
                    else if (vxInput.MouseState.ScrollWheelValue - vxInput.PreviousMouseState.ScrollWheelValue < 0)
                        Camera.Zoom -= 5f * vxTime.DeltaTime * Camera.Zoom / scrollSens;

                    if (camMove != Vector2.Zero)
                       ((vxCamera2D)Camera).MoveCamera(camMove);
                    if (vxInput.IsNewKeyPress(Keys.Home))
                        Camera.ResetCamera();
                }
            }

		}

        /// <summary>
        /// Handles the input base.
        /// </summary>
        /// <param name="input">Input.</param>
        protected override void HandleInputBase()
		{
			// Control debug view
			if (vxInput.IsNewButtonPressed(Buttons.Start))
			{
				EnableOrDisableFlag(DebugViewFlags.Shape);
				EnableOrDisableFlag(DebugViewFlags.DebugPanel);
				EnableOrDisableFlag(DebugViewFlags.PerformanceGraph);
				EnableOrDisableFlag(DebugViewFlags.Joint);
				EnableOrDisableFlag(DebugViewFlags.ContactPoints);
				EnableOrDisableFlag(DebugViewFlags.ContactNormals);
				EnableOrDisableFlag(DebugViewFlags.Controllers);
			}

			if (vxInput.IsNewKeyPress(Keys.F1))
				EnableOrDisableFlag(DebugViewFlags.Shape);
			if (vxInput.IsNewKeyPress(Keys.F2))
			{
				EnableOrDisableFlag(DebugViewFlags.DebugPanel);
				EnableOrDisableFlag(DebugViewFlags.PerformanceGraph);
			}
			if (vxInput.IsNewKeyPress(Keys.F3))
				EnableOrDisableFlag(DebugViewFlags.Joint);
			if (vxInput.IsNewKeyPress(Keys.F4))
			{
				EnableOrDisableFlag(DebugViewFlags.ContactPoints);
				EnableOrDisableFlag(DebugViewFlags.ContactNormals);
			}
			if (vxInput.IsNewKeyPress(Keys.F5))
				EnableOrDisableFlag(DebugViewFlags.PolygonPoints);
			if (vxInput.IsNewKeyPress(Keys.F6))
				EnableOrDisableFlag(DebugViewFlags.Controllers);
			if (vxInput.IsNewKeyPress(Keys.F7))
				EnableOrDisableFlag(DebugViewFlags.CenterOfMass);
			if (vxInput.IsNewKeyPress(Keys.F8))
				EnableOrDisableFlag(DebugViewFlags.AABB);
		}



        public override vxSaveBusyScreen GetAsyncSaveScreen()
        {
            return new vxSaveBusyScreen2D(this);
        }

		/// <summary>
		/// Loads the next level.
		/// </summary>
		public virtual void LoadNextLevel()
		{

		}

		/// <summary>
		/// Reloads the current level.
		/// </summary>
		public virtual void ReloadCurrentLevel()
		{

		}

        #region Sandbox





        /// <summary>
        /// Should an Item Be Added.
        /// </summary>
        public bool AddItem = true;

        /// <summary>
        /// Adds a new item with the defualt position.
        /// </summary>
        /// <returns>The new item.</returns>
        /// <param name="key">Key.</param>
        public virtual vxEntity2D AddNewItem(string key)
        {
            // Items are added initially at the Width and Height offset from the Center of the sreen. 
            // this is so no matter what size is chosen, the item will always spawn outside of 
            // the active bounds.
            return AddNewItem(key, Vector2.Zero);
        }

        /// <summary>
        /// Adds a new item with the add location position. 
        /// NOTE: (Position is specified in Camera View Space (origin at screen (width/2, height/2), 
        /// not Screen (origin at screen (0,0)) or Farseer Simulation Space (position is offset by a factor)).
        /// </summary>
        /// <returns>The new item.</returns>
        /// <param name="key">Key.</param>
        /// <param name="AddLocation">Position to Add Item (Position must be given in Camera View Space).</param>
        public virtual vxEntity2D AddNewItem(string key, Vector2 AddLocation)
        {

            vxEntity2D itemToReturn = null;

            if (TypeRegister.ContainsKey(key))
                itemToReturn = (vxEntity2D)Instaniate(TypeRegister[key], AddLocation);


            //This is the default setting, so it should always be reset, if a method needs it, it needs
            //to be specified before each call.
            AddItem = true;

            return itemToReturn;
        }

        protected virtual vxEntity2D Instaniate(Type type, Vector2 position)
        {
            System.Reflection.ConstructorInfo ctor = type.GetConstructor(new[] { typeof(vxGameplayScene2D), typeof(Vector2) });
            return (vxEntity2D)ctor.Invoke(new object[] { this, position });
        }

        #endregion
    }
}