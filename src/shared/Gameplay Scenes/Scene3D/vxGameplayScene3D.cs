
#region Using Statements

using BEPUphysics;
using BEPUphysicsDrawer.Models;
using BEPUutilities.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using VerticesEngine.Audio;
using VerticesEngine.Diagnostics;
using VerticesEngine.Editor.Entities;
using VerticesEngine.Entities;
using VerticesEngine.EnvTerrain;
using VerticesEngine.Graphics;
using VerticesEngine.Graphics.Rendering;
using VerticesEngine.Input;
using VerticesEngine.Physics;
using VerticesEngine.Screens.Async;
using VerticesEngine.Serilization;
using VerticesEngine.UI.MessageBoxs;
using VerticesEngine.Utilities;

#endregion

namespace VerticesEngine
{
    /// <summary>
    /// The vxGameplayScene3D class implements the actual game logic for 3D Games.
    /// </summary>
    public partial class vxGameplayScene3D : vxGameplaySceneBase
    {
        #region Fields

        /// <summary>
        /// Collection of Lights in the Level.
        /// </summary>
        public List<vxLightEntity> LightItems = new List<vxLightEntity>();

        /// <summary>
        /// The main BEPU Physics Simulation Space used in the game.
        /// </summary>
        public BEPUphysics.Space PhyicsSimulation;

        /// <summary>
        /// This is the multithreaded parrallel looper class used by BEPU too
        /// multi-thread the physics engine.
        /// </summary>
        private ParallelLooper BEPUParallelLooper;

        /// <summary>
        /// Model Drawer for debugging the phsyics system
        /// </summary>
        public ModelDrawer PhysicsDebugViewer;

        /// <summary>
        /// Manages the Sun Class.
        /// </summary>
        public vxSunEntity SunEmitter;


        /// <summary>
        /// Gets or sets the light positions.
        /// </summary>
        /// <value>The light positions.</value>
		public Vector3 LightPositions
        {
            //get { return SunEmitter.SunWorldPosition; }
            get { return _lightPositions; }
            set
            {
                _lightPositions = value * new Vector3(-1, 1, 1);
                _lightPositions.Normalize();
                //SunEmitter.SunWorldPosition = _lightPositions;
                vxRenderPipeline.Instance.LightDirection = _lightPositions;
            }
        }
        private Vector3 _lightPositions;


        /// <summary>
        /// Sky box
        /// </summary>
        private vxSkyBox m_skyBox;

        float pauseAlpha;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor which starts the game as a Normal (Non-Networked) game.
        /// </summary>
        public vxGameplayScene3D(vxStartGameMode sandboxStartMode, string fileToOpen = "", int numberOfPlayers = 1)
            : base(sandboxStartMode, fileToOpen, numberOfPlayers)
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);


            // Inisitalise the Selected Items Collection
            m_selectedItems = new List<vxEntity3D>();

            // Default is Edit Mode
            SandboxCurrentState = vxEnumSandboxStatus.EditMode;

            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            SandboxEditMode = vxEnumSanboxEditMode.SelectItem;

            this.SandboxStartGameType = sandboxStartMode;

            if (sandboxStartMode == vxStartGameMode.GamePlay)
                SandboxCurrentState = vxEnumSandboxStatus.Running;
            else
                SandboxCurrentState = vxEnumSandboxStatus.EditMode;

            //Set File name
            FilePath = fileToOpen;
            FileName = fileToOpen.GetFileNameFromPath();

        }


        /// <summary>
        /// Load graphics content for the game. Note the base.LoadContent() must be called at the
        /// top of any override function so that all systems are properly set up before loading your code.
        /// The calling order is:
        /// <para />InitialiseRenderingEngine(); 
        /// <para />InitialisePhysics();
        /// <para />InitialiseCameras();
        /// <para />InitialiseViewportManager();
        /// <para />InitialiseSky();
        /// <para />InitialiseAudioManager();
        /// </summary>
        public override void LoadContent()
        {
            // Call the base Method
            base.LoadContent();

            // Initialise the Sky
            OnInitialiseSkyBox();

        }

        protected override void InitialiseSubSystems()
        {
            AddSystem<vxStaticMeshBatchRenderSystem>();
        }

        protected virtual void RefreshWorldPropertyControl()
        {
            if (WorlPropertiesControl != null)
            {
                WorlPropertiesControl.Clear();
                WorlPropertiesControl.GetPropertiesFromObject(_worldProperties);
                WorlPropertiesControl.ResetLayout();
            }

            if (EffectPropertiesControl != null)
            {
                EffectPropertiesControl.Clear();
                EffectPropertiesControl.GetPropertiesFromObject(vxRenderSettings.Instance);
                EffectPropertiesControl.ResetLayout();
            }
        }

        private IEnumerator baseSceneLoader;

        internal override IEnumerator LoadSceneContentAsync()
        {
            //yield return null;

            if (baseSceneLoader == null)
                baseSceneLoader = base.LoadSceneContentAsync();

            while (baseSceneLoader.MoveNext())
            {
                yield return null;
            }

            SandboxEditMode = vxEnumSanboxEditMode.SelectItem;

            if (SandboxCurrentState == vxEnumSandboxStatus.EditMode)
                vxInput.IsCursorVisible = true;

            IsPausable = !(NetGameType == vxNetworkGameType.Networked);

            yield return 2;

            vxLoadingScreen.SetLoadPercentage(5);

            vxSandboxFileLoadResult loadResult = new vxSandboxFileLoadResult();
            //Load the files if it isn't a new 
            if (FilePath != "")
            {
                // we are loading a file
                IsLoadingFile = true;

                vxDebug.LogIO("Loading File " + FilePath);
                loadResult = LoadFile(FilePath);
            }
            yield return 2;
            vxLoadingScreen.SetLoadPercentage(10);
            //if(loadResult.IsSuccessful)
            OnPostFileLoad();

            yield return 2;

            if (WorlPropertiesControl != null)
            {
                // final task is to setup property grids
                _worldProperties = InitWorldProperties();
                //SandBoxFile.Enviroment.Fog
                RefreshWorldPropertyControl();
            }
            yield return 2;

            if (loadResult.IsSuccessful == false)
            {
                vxMessageBox.Show("Error Loading File", "There was an error loading the sandbox file. Check the console for more information.");
            }

            {
                vxDebug.LogIO("Loading Imported Entities");
                foreach (var importedEntity in SandBoxFile.ImportedEntities)
                {
                    // first lets try to load it
                    var importResult = vxMeshHelper.Import(Path.Combine(vxIO.PathToImportFolder, importedEntity.guid, importedEntity.fileName));
                    if (importResult.ImportResultStatus == vxImportResultStatus.Success)
                    {
                        RegisterImportedModel(importedEntity.guid, importedEntity.originalFilePath, importResult.ImportedModel);
                    }
                }

                vxDebug.LogIO("Loading Entities");
                if (SandBoxFile.Entities.Count > 0)
                {
                    //while (entitiesCount < SandBoxFile.Entities.Count)
                    for (int e = 0; e < SandBoxFile.Entities.Count; e++)
                    {
                        vxSerializableEntityState3D part = SandBoxFile.Entities[e];

                        //vxConsole.WriteLine(part.id + " - " + part.Type);
                        
                        TempPart = AddSandboxItem(part.Type, part.Orientation);

                        if (TempPart != null)
                        {
                            TempPart.Id = part.id;
                            TempPart.UserDefinedData = part.UserDefinedData;
                            TempPart.SandboxData = part.SandboxData;
                            TempPart.UserDefinedData01 = part.UserDefinedData01;
                            TempPart.UserDefinedData02 = part.UserDefinedData02;
                            TempPart.UserDefinedData03 = part.UserDefinedData03;
                            TempPart.UserDefinedData04 = part.UserDefinedData04;
                            TempPart.UserDefinedData05 = part.UserDefinedData05;

                            //TempPart.OnAfterEntityDeserialized();
                        }
                        vxTime.ResetElapsedTime();
                        vxLoadingScreen.SetLoadPercentage(10 + (float)e / SandBoxFile.Entities.Count * 40f);
                        yield return null;
                    }
                }
                vxDebug.LogIO("Loading Terrain");
                if (SandBoxFile.Entities.Count > 0)
                {
                    //while (terrainCount < SandBoxFile.Terrains.Count)
                    for (int t = 0; t < SandBoxFile.Terrains.Count; t++)
                    {
                        vxSerializableTerrainData terrainData = SandBoxFile.Terrains[t];

                        TempPart = AddSandboxItem(terrainData.Type, terrainData.Orientation);

                        if (TempPart != null)
                        {
                            var temp_terrain = (vxTerrainChunk)TempPart;

                            temp_terrain.TerrainData = terrainData;

                            //temp_terrain.OnAfterEntityDeserialized();
                        }
                        vxTime.ResetElapsedTime();
                        vxLoadingScreen.SetLoadPercentage(50 + (float)t / SandBoxFile.Terrains.Count * 10f);
                        yield return null;
                    }
                }
            }

            // we assume this point we are no longer loading the file
            IsLoadingFile = false;
            vxConsole.WriteLine("On Post File Load");
            // Now call Post Load Initalise on all entities
            while (finalEntityLoad < Entities.Count)
            {
                Entities[finalEntityLoad].IsDeserializing = true;
                Entities[finalEntityLoad].SetSerialisedSandboxData();
                Entities[finalEntityLoad].OnAfterEntityDeserialized();
                Entities[finalEntityLoad].IsDeserializing = false;

                finalEntityLoad++;
                vxLoadingScreen.SetLoadPercentage(60 + (float)finalEntityLoad / Entities.Count * 30f);
                yield return null;
            }
            yield return null;
            vxTime.ResetElapsedTime();
            yield return null;

            vxDebug.LogIO(new
            {
                level = FilePath,
                entityCount = SandBoxFile.Entities.Count,
                terrainCount = SandBoxFile.Terrains.Count
            });
        }


        int finalEntityLoad = 0;

        public override IEnumerator LoadContentAsync()
        {
            // test code
            for (int j = 0; j < 2; j++)
            {
                yield return j;
            }

            yield return null;
        }

        /// <summary>
        /// This is called after the file is loaded but still during the asyc content load
        /// </summary>
        protected virtual void OnPostFileLoad()
        {

        }


        protected internal override void OnGraphicsRefresh()
        {
            base.OnGraphicsRefresh();

            foreach (var camera in Cameras)
            {
                camera.OnGraphicsRefresh();
                camera.FieldOfView = vxCamera.DefaultFieldOfView * MathHelper.Pi / 180;
            }


            SetCameraViewports();
            ViewportManager.ResetMainViewport();
        }



        /// <summary>
        /// This Initalises the Physics System
        /// </summary>
        protected override void InitialisePhysics()
        {
            vxPhysicsSystem.Instance.SetPhysicsEngine(PhysicsEngineType.BEPU);

            int threadCount = 1;
            // Next Initialise the Physics Engine
            if (vxPhysicsSystem.Instance.IsMultiThreaded)
            {
                //Starts BEPU with multiple Cores
                BEPUParallelLooper = new ParallelLooper();
                
                if (Environment.ProcessorCount > 1)
                {
                    for (int i = 0; i < Environment.ProcessorCount; i++)
                    {
                        BEPUParallelLooper.AddThread();
                    }
                }

                PhyicsSimulation = new BEPUphysics.Space(BEPUParallelLooper);
                threadCount = PhyicsSimulation.ParallelLooper.ThreadCount;
            }
            else
            {
                // Start BEPU without Multi Threading
                PhyicsSimulation = new BEPUphysics.Space();
            }
            PhyicsSimulation.Solver.IterationLimit = 100;
            PhyicsSimulation.ForceUpdater.Gravity = new Vector3(0, -9.81f, 0);

            vxDebug.LogEngine(new
            {
                //type = this.GetType() + ".InitialisePhysics()",
                isMultiThreaded = PhyicsSimulation.ForceUpdater.AllowMultithreading,
                threadCount = threadCount,
                iterationLimit = PhyicsSimulation.Solver.IterationLimit,
                gravity = PhyicsSimulation.ForceUpdater.Gravity,

            });

            PhysicsDebugViewer = new ModelDrawer(vxEngine.Game);


            // Initalise the Gizmo
            m_gizmo = new vxGizmo(this);

            //Sets The Outof sifht Position
            OutofSight = Vector3.One * 100000;

            m_workingPlane = new vxWorkingPlane(this, vxInternalAssets.Models.UnitPlane, new Vector3(0, 0, 0));

            CurrentlySelectedKey = "";
        }


        /// <summary>
        /// Initialises the cameras for this Scene. The number of Cameras used is based on the number of Players
        /// specified in the optional constructor argument. If you want multiply players but only one Camera, then overload this 
        /// method.
        /// </summary>
        protected override void InitialiseCameras()
        {
            // Setup Cameras
            for (int i = 0; i < NumberOfPlayers; i++)
            {
                Cameras.Add(new vxCamera3D(this, vxCameraType.Orbit, new Vector3(0, 15, 0)));
            }
            SetCameraViewports();
        }

        /// <summary>
        /// Sets the Camera Viewports
        /// </summary>
        protected void SetCameraViewports()
        {
            for (int i = 0; i < NumberOfPlayers; i++)
            {
                //Viewport thisCamerasViewport = vxGraphics.GraphicsDevice.Viewport;
                Viewport thisCamerasViewport = vxScreen.Viewport;// new Viewport(0,0,vxScreen.Width, vxScreen.Height);

                switch (NumberOfPlayers)
                {
                    case 1:
                        // Do nothing
                        break;

                    case 2:
                        thisCamerasViewport = new Viewport(0, thisCamerasViewport.Height / 2 * i,
                                                           thisCamerasViewport.Width, thisCamerasViewport.Height / 2);
                        break;


                    case 3:
                    case 4:
                        int j = ((i) % 2);
                        thisCamerasViewport = new Viewport(thisCamerasViewport.Width / 2 * (i % 2), thisCamerasViewport.Height / 2 * (i / 2),
                                                           thisCamerasViewport.Width / 2, thisCamerasViewport.Height / 2);
                        break;
                }

                Cameras[i].Viewport = thisCamerasViewport;
            }
        }



        /// <summary>
        /// Initialises the SkyBoxes and the Sun Entity
        /// </summary>
        protected virtual void OnInitialiseSkyBox()
        {
            //Setup Sun
            SunEmitter = new vxSunEntity();
            m_skyBox = OnSkyboxInit();
        }

        /// <summary>
        /// This returns the skybox. Override this to provide your own skybox
        /// </summary>
        /// <returns></returns>
        protected virtual vxSkyBox OnSkyboxInit()
        {
            return new vxSkyBox();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            // dipost of UI elements
            if (PropertiesTabControl != null)
                PropertiesTabControl.Dispose();

            m_workingPlane.Dispose();

            CanSceneBeRemovedCompletely = true;

            ViewportManager.Dispose();

            vxTerrainManager.Instance.Terrains.Clear();

            if (m_skyBox != null)
                m_skyBox.Dispose();
            m_skyBox = null;

            LightItems.Clear();

            if (SunEmitter != null)
                SunEmitter.Dispose();
            SunEmitter = null;

            m_gizmo.Dispose();
            m_gizmo = null;

            // Clean up Entities

            base.UnloadContent();

            // Cleanup Systems

            PhysicsDebugViewer.Clear();
            BEPUParallelLooper.Dispose();
            
            PhyicsSimulation = null;
            PhysicsDebugViewer = null;
        }

        #endregion

        #region Update and Draw

        

        /// <summary>
        /// Updates the state of the game. This method checks the vxGameBaseScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        protected internal override void Update()
        {
            //int a = 0, j = 0;
            //a = a / j;
            //Update Audio Manager
            //**************************************
            for (int c = 0; c < Cameras.Count; c++)
            {
                vxAudioManager.Listener.Position = Cameras[c].Position / 10;
                vxAudioManager.Listener.Forward = Cameras[c].View.Forward;
                vxAudioManager.Listener.Up = Cameras[c].View.Up;
                vxAudioManager.Listener.Velocity = ((vxCamera3D)Cameras[c]).Velocity;//.View.Forward;
            }

            base.Update();

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen && IsPausable)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive || IsPausable == false)
            {

                // Update Debug Code
                UpdateDebug();


                // Update Terrain Manager
                //**********************************************************************************************
                vxTerrainManager.Instance.Update();

                // Update Physics
                //**********************************************************************************************
                // Start measuring time for "Physics".
                vxProfiler.BeginMark(vxProfiler.Tags.PHYSICS_UPDATE);

                //Update the Physics System.
                //Console.WriteLine(PhyicsSimulation.Solver.TimeStepSettings.TimeStepDuration);
                int physSteps = vxPhysicsSystem.StepsPerFrame;
                float step = 0.0167f / physSteps;

                for (int i = 0; i < physSteps; i++)
                    PhyicsSimulation.Update(step);

                //PhyicsSimulation.PositionUpdater.TimeStepSettings.MaximumTimeStepsPerFrame = physSteps;

                if (vxEngine.BuildType == vxBuildType.Debug)
                    vxConsole.WriteToScreen("vxTime dt", vxTime.DeltaTime);

                // Stop measuring time for "Physics".
                vxProfiler.EndMark(vxProfiler.Tags.PHYSICS_UPDATE);


                // Update the Scene
                //**********************************************************************************************
                UpdateScene();



                // Update Scene Entities
                //**********************************************************************************************
                for (int i = 0; i < Entities.Count; i++)
                {
                    if (Entities[i].IsEnabled)
                    {
                        Entities[i].Update();

                        if (i < Entities.Count && Entities[i].KeepUpdating == false)
                            Entities.RemoveAt(i);
                    }
                }

                for (int i = 0; i < Entities.Count; i++)
                {
                    if (Entities[i].IsEnabled)
                    {
                        if (i < Entities.Count)
                            Entities[i].PostUpdate();
                    }
                }
            }
            else
                vxInput.IsCursorVisible = true;
        }


        /// <summary>
        /// Main Game Update Loop which is accessed outside of the vxEngine
        /// </summary>
        protected internal override void UpdateScene()
        {
            if (IsActive)
            {
                // What to update if it's in Edit mode
                if (SandboxCurrentState == vxEnumSandboxStatus.EditMode)
                {
                    HoveredSnapBoxWorld = vxTransform.Identity;
                    HoveredSnapBox = null;
                    //Reset to 'MouseOver' each loop
                    IsMouseOverSnapBox = false;
                    AddMode = vxEnumAddMode.OnPlane;

                    // Edit mode is always in one viewport mode.
                    MouseRay = vxGeometryHelper.CalculateCursorRay(Cameras[0].Projection, Cameras[0].View);

                    m_gizmo.Update(MouseRay);

                    if (m_gizmo.IsMouseHovering == false && IsRaySelectionEnabled)
                        HandleMouseRay(MouseRay);


                    //If Index still equals -1, then it isn't over any elements, and a new element can be added.
                    PreviousIntersection = Intersection;

                    if (SandboxCurrentState == vxEnumSandboxStatus.EditMode)
                    {
                        if (MouseRay.Intersects(vxWorkingPlane.Instance.WrknPlane) != null)
                        {
                            Intersection = (float)MouseRay.Intersects(vxWorkingPlane.Instance.WrknPlane) * MouseRay.Direction + MouseRay.Position;
                        }

                        if (TempPart != null)
                        {
                            if (AddMode == vxEnumAddMode.OnSurface && TempPart.CanBePlacedOnSurface == true)
                                TempPart.Transform = HoveredSnapBoxWorld;
                            else if (IsMouseOverSnapBox)
                                TempPart.Transform = HoveredSnapBoxWorld;
                            else
                            {
                                TempPart.Position = IsGridSnap ? Intersection.ToIntValue() : Intersection;
                                //TempPart.OnSetTransform();

                            }
                        }
                    }
                    else
                    {
                        //Get it WAYYYY out of the scene
                        Intersection = OutofSight;
                    }
                }
            }

            IsGUIVisible = (SandboxCurrentState == vxEnumSandboxStatus.EditMode);

        }



        #endregion
    }
}