#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using VerticesEngine.Audio;
using VerticesEngine;
using VerticesEngine.Commands;
using VerticesEngine.Diagnostics;

using VerticesEngine.Particles;
using VerticesEngine.Input;
using VerticesEngine.Input.Events;
using VerticesEngine.Profile;
using VerticesEngine.Screens.Async;
using VerticesEngine.Serilization;
using VerticesEngine.UI;
using VerticesEngine.UI.Dialogs;
using VerticesEngine.UI.Events;
using VerticesEngine.UI.Menus;
using VerticesEngine.UI.MessageBoxs;
using VerticesEngine.Utilities;
using VerticesEngine.Graphics;
using System.Collections;
using System.Diagnostics;
using VerticesEngine.ContentManagement;
using VerticesEngine.Entities;

#endregion

namespace VerticesEngine
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    public class vxGameplaySceneBase : vxBaseScene
    {
        internal List<vxMeshRenderer> MeshRenderers = new List<vxMeshRenderer>();

        /// <summary>
        /// The cameras collection.
        /// </summary>
        public List<vxCamera> Cameras
        {
            get { return m_cameras; }
        }
        private List<vxCamera> m_cameras = new List<vxCamera>();


        public List<vxUtilCamera3D> UtilCameras = new List<vxUtilCamera3D>();


        /// <summary>
        /// The particle system.
        /// </summary>
        public vxParticleSystemManager ParticleSystem
        {
            get { return _particleSystem; }
        }
        private vxParticleSystemManager _particleSystem;

        

        #region Fields

        public vxIPlayerProfile PlayerProfile
        {
            get { return vxPlatform.Player; }
        }

        /// <summary>
        /// The content manager for this scene. This is unloaded at the end of the scene.
        /// </summary>
        public ContentManager SceneContent;

        /// <summary>
        /// The type of the game, whether its a Local game or Networked.
        /// </summary>
        public vxNetworkGameType NetGameType = vxNetworkGameType.Local;

        /// <summary>
        /// The Scene UI Manager
        /// </summary>
        public vxUIManager UIManager
        {
            get { return _uiManager; }
        }
        private vxUIManager _uiManager;


        /// <summary>
        /// Is this level the start background
        /// </summary>
        public bool IsStartBackground = false;

        /// <summary>
        /// The Level Title
        /// </summary>
        public string Title
        {
            get { return SandBoxFile.LevelTitle; }
            set { SandBoxFile.LevelTitle = value; }
        }

        /// <summary>
        /// The Level Description
        /// </summary>
        public string Description
        {
            get { return SandBoxFile.LevelDescription; }
            set { SandBoxFile.LevelDescription = value; }
        }

        
        public string WorkshopID
        {
            get { return SandBoxFile.workshopId; }
            set { SandBoxFile.workshopId = value; }
        }

        /// <summary>
        /// Should the GUI be shown?
        /// </summary>
        public bool IsGUIVisible = true;


        /// <summary>
        /// Whether or not to dim the scene when it's covered by another screen.
        /// </summary>
        public bool IsSceneDimmedOnCover = true;


        /// <summary>
        /// The number of players in this Scene. This must be set in the constructor.
        /// </summary>
        public readonly int NumberOfPlayers;

        /// <summary>
        /// Gets the default texture.
        /// </summary>
        /// <value>The default texture.</value>
        public Texture2D DefaultTexture
        {
            get { return vxInternalAssets.Textures.Blank; }
        }



        /// <summary>
        /// This is the Pause Alpha Amount based off of the poisition the screen is in terms of
        /// transitioning too a new screen.
        /// </summary>
        public float PauseAlpha;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is pausable.
        /// </summary>
        /// <value><c>true</c> if this instance is pausable; otherwise, <c>false</c>.</value>
        public bool IsPausable = true;

        /// <summary>
        /// The command manager to handle undo redos.
        /// </summary>
        public vxCommandManager CommandManager
        {
            get { return _commandManager; }
        }
        private vxCommandManager _commandManager;


        /// <summary>
        /// The entity collection for this Scene.
        /// </summary>
        public List<vxEntity> Entities
        {
            get { return _entities; }
        }
        private List<vxEntity> _entities = new List<vxEntity>();


        /// <summary>
        /// Editor Entities
        /// </summary>
        public List<vxEntity3D> EditorEntities
        {
            get { return _editorEntities; }
        }
        private List<vxEntity3D> _editorEntities = new List<vxEntity3D>();


        /// <summary>
        /// Is the Sandbox In Testing Mode
        /// </summary>
        public vxEnumSandboxStatus SandboxCurrentState
        {
            get { return _currentSandboxState; }
           protected internal set { _currentSandboxState = value; }
        }
        private vxEnumSandboxStatus _currentSandboxState = vxEnumSandboxStatus.EditMode;


        public vxStartGameMode SandboxStartGameType
        {
            get { return _sandboxStartGameType; }
            set
            {
                _sandboxStartGameType = value;
            }
        }
        private vxStartGameMode _sandboxStartGameType = vxStartGameMode.GamePlay;


        /// <summary>
        /// The viewport manager.
        /// </summary>
        public vxViewportManager ViewportManager;

        /// <summary>
        /// Is this a sandbox scene, if so then we will load any and all registered types. Otherwise you'll
        /// </summary>
        protected bool IsSandbox = true;

        protected Dictionary<string, Type> TypeRegister = new Dictionary<string, Type>();


        #endregion

        #region Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.vxSceneBase"/> class.
        /// </summary>
        /// <param name="sandboxStartGameType">Sandbox start game type.</param>
        /// <param name="FilePath">File path.</param>
        /// <param name="NumberOfPlayers">Number of players.</param>
        public vxGameplaySceneBase(vxStartGameMode sandboxStartGameType = vxStartGameMode.GamePlay, string FilePath = "", int NumberOfPlayers = 1)
        {
            vxConsole.WriteLine("");
            vxConsole.WriteLine("Starting Scene - " + this.GetType());
            vxConsole.WriteLine("---------------------------------------------------");

            // clear the temp directory
            vxIO.ClearTempDirectory();

            // only set the number of players if it's play mode, if its in sandbox mode then we only ever want 1 player
            if (sandboxStartGameType == vxStartGameMode.GamePlay)
            {
                this.NumberOfPlayers = NumberOfPlayers;
            }
            else
            {
                this.NumberOfPlayers = 1;
            }

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            // Initiaise the File Structure
            SandBoxFile = InitSaveFile();

            this._sandboxStartGameType = sandboxStartGameType;

            this.FilePath = FilePath;
        }

        /// <summary>
        /// Initialises the physics engine for this scene.
        /// </summary>
        protected virtual void InitialisePhysics() { }


        /// <summary>
        /// Initialises the cameras for this scene.
        /// </summary>
        protected virtual void InitialiseCameras() { }


        protected virtual void OnInitialiseGUI() { }

        /// <summary>
        /// Initialises the Viewport Manager.
        /// </summary>
		protected virtual void InitialiseViewportManager()
        {
            ViewportManager = new vxViewportManager(this);
        }

        protected virtual void RegisterSandboxEntities() { }

        protected virtual void SetupObjectPools() {

            vxObjectPool.Instance.InitScene();
            foreach(var pool in vxEntityRegister.ObjectPools.Values)
            {
                Console.WriteLine($"{pool.type} - {pool.Count} Items");
            }
        }

        protected virtual void UnloadObjectPools()
        {
            vxObjectPool.Instance.OnSceneEnd();
        }



        private void Internal_InitialiseSubSystems()
        {
            vxEngine.Instance.CurrentScene = this;

            _uiManager = new vxUIManager();
            _particleSystem = new vxParticleSystemManager();
            vxConsole.WriteVerboseLine($"vxParticleSystemManager {stopwatch.ElapsedMilliseconds}");
            _commandManager = new vxCommandManager();
            vxConsole.WriteVerboseLine($"vxCommandManager {stopwatch.ElapsedMilliseconds}");


            vxContentManager.Instance.SetActiveContentManager(SceneContent);
            vxConsole.WriteVerboseLine($"ContentManager {stopwatch.ElapsedMilliseconds}");

            InitialisePhysics();
            vxConsole.WriteVerboseLine($"InitialisePhysics {stopwatch.ElapsedMilliseconds}");

            // Set up the Camera
            InitialiseCameras();
            vxConsole.WriteVerboseLine($"InitialiseCameras {stopwatch.ElapsedMilliseconds}");

            // Initialise the Viewport Manager
            InitialiseViewportManager();
            vxConsole.WriteVerboseLine($"InitialiseViewportManager {stopwatch.ElapsedMilliseconds}");

            InitialiseSubSystems();
        }

        /// <summary>
        /// Initialise your scenes subsystems here
        /// </summary>
        protected virtual void InitialiseSubSystems()
        {

        }

        Stopwatch stopwatch = new Stopwatch();

        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            stopwatch.Start();

            IsLoadingTimeMeasured = true;

            base.LoadContent();
            vxConsole.WriteVerboseLine($"base.LoadContent() {stopwatch.ElapsedMilliseconds}");

            DebugMethodCall("LoadContent", ConsoleColor.Magenta);


            if (SceneContent == null)
                SceneContent = new ContentManager(vxEngine.Game.Services, "Content");

            Internal_InitialiseSubSystems();
        }


        internal override IEnumerator LoadSceneContentAsync()
        {
            // Setup GUI Items
            OnInitialiseGUI();
            vxLoadingScreen.SetLoadPercentage(10);
            yield return null;

            LoadParticlePools();
            vxLoadingScreen.SetLoadPercentage(20);
            yield return null;

            // Now that all of the GUI Items have been initalised, now register all items.
            if (IsSandbox)
                RegisterSandboxEntities();
            vxLoadingScreen.SetLoadPercentage(30);
            yield return null;

            // Create Object Pools
            SetupObjectPools();

            yield return null;

        }

        protected virtual void LoadParticlePools() { }

        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            for (int i = 0; i < Entities.Count;)
                Entities[i].Dispose();

            UnloadObjectPools();

            foreach (var camera in Cameras)
                camera.Dispose();

            Cameras.Clear();
            m_cameras = null;

            foreach (var camera in UtilCameras)
                camera.Dispose();

            UtilCameras.Clear();
            UtilCameras = null;

            for(int m = 0; m < MeshRenderers.Count; m++)
                MeshRenderers[m].Dispose();

            MeshRenderers.Clear();
            MeshRenderers = null;


            ParticleSystem.Dispose();

            _entities.Clear();
            _entities = null;

            _editorEntities.Clear();
            _editorEntities = null;


            vxGameObject.NameRegister.Clear();

            vxAudioManager.Stop();


            DebugMethodCall("UnloadContent", ConsoleColor.DarkMagenta);

            base.UnloadContent();

            UIManager.Dispose();

            for(int s = 0; s < _subsystems.Count; s++)
            {
                _subsystems[s].Dispose();
            }
            _subsystems.Clear();

            SceneContent.Unload();
            SceneContent.Dispose();


            GC.Collect();
        }

        public void DebugMethodCall(string method, ConsoleColor ConsoleColor = ConsoleColor.Yellow)
        {
            vxConsole.InternalWriteLine(this.ToString().Substring(this.ToString().LastIndexOf('.') + 1) + "." + method + "()");
        }
        #endregion

        protected internal override void OnGraphicsRefresh()
        {
            base.OnGraphicsRefresh();


            foreach (vxCamera camera in Cameras)
                camera.OnGraphicsRefresh();
        }

        #region -- Scene Subsystems --

        private List<vxISubSystem> _subsystems = new List<vxISubSystem>();

        /// <summary>
        /// Add a subsystem of type <see cref="vxISceneSubSystem"/>. It must be scene subsystem
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected virtual T AddSystem<T>() where T: vxISceneSubSystem
        {
            // create a new instance of the sub system
            T sceneSubSystem = Activator.CreateInstance<T>();
            
            // now check that we're adding a scene sub system, otherwise throw an error
            if(sceneSubSystem.Type == SubSystemType.Scene)
            {
                _subsystems.Add(sceneSubSystem);
                sceneSubSystem.Initialise();
                return sceneSubSystem;
            }
            else
            {
                throw new Exception($"Invalid Sub System Type. You are trying to add a sub system of type {typeof(T)} which is a {sceneSubSystem.Type}");
            }
        }

        /// <summary>
        /// Get the subsystem of type <see cref="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetSubSystem<T>() where T: vxISubSystem
        {
            foreach(var subSystem in _subsystems)
            {
                if(typeof(T).IsAssignableFrom(subSystem.GetType()))
                {
                    return (T)subSystem;
                }
            }

            return default(T);
        }

        /// <summary>
        /// Tries to get the subsystem of type <see cref="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subSystem"></param>
        /// <returns></returns>
        public bool TryGetSubSystem<T>(out T subSystem) where T : vxISubSystem
        {
            subSystem = GetSubSystem<T>();

            return (subSystem != null);
        }
        #endregion

        #region IO

        public virtual vxFileInfo GetFileInfo()
        {
            var fileInfo = new vxFileInfo();
            fileInfo.Initialise();

            // Set current version and info
            fileInfo.SandboxFileInfo.Title = Title;
            fileInfo.SandboxFileInfo.Description = Description;
            
            fileInfo.WorkshopFileInfo.Author = vxPlatform.Player.Name;
            fileInfo.WorkshopFileInfo.id = WorkshopID;

            return fileInfo;
        }

        /// <summary>
        /// Is the sandbox level a content file or external/DLC?
        /// </summary>
        protected virtual bool IsSandboxLevelContentFile
        {
            get { return (SandboxStartGameType == vxStartGameMode.GamePlay); }
        }

        protected virtual vxSandboxFileLoadResult LoadFile(string FilePath)
        {
            vxSandboxFileLoadResult loadResult = new vxSandboxFileLoadResult();
            // Only load a file if the File Path isn't empty
            if (FilePath != "")
            {
                DateTime startTime = DateTime.Now;

                vxConsole.WriteIODebug("=====================================================");
                vxConsole.WriteIODebug("Loading File: " + FilePath);

                FileName = Path.GetFileName(FilePath);
                FileName = FileName.Substring(0, FileName.LastIndexOf('.'));

                // First clear the temp folder
                vxIO.ClearTempDirectory();


                // Decompress The file
                try
                {
                    string macContentPath = "../Resources/" + FilePath;
                    string androidContentPath = "assets/" + FilePath;
                    if (File.Exists(FilePath))
                    {
                        vxIO.DecompressToDirectory(FilePath, vxIO.PathToTempFolder, null, IsSandboxLevelContentFile);
                    }
                    else if(File.Exists(macContentPath))
                    {
                        vxIO.DecompressToDirectory(macContentPath, vxIO.PathToTempFolder, null, IsSandboxLevelContentFile);
                    }
                    else if (File.Exists(androidContentPath))
                    {
                        Console.WriteLine("ANDROID CONTENT PATH");
                        return new vxSandboxFileLoadResult(FilePath);
                    }
                    else
                    {
                        try
                        {

                            vxIO.DecompressToDirectory(FilePath, vxIO.PathToTempFolder, null,
                                                       (SandboxStartGameType == vxStartGameMode.GamePlay));

                        }
                        catch
                        {
                            return new vxSandboxFileLoadResult(FilePath);
                        }
                    }

                }
                catch (Exception ex)
                {
                    vxConsole.WriteException("Error Loading Sandbox File", ex);
                    return new vxSandboxFileLoadResult(FilePath);
                }

                // First load the file info
                var fileInfo = new vxFileInfo();
                fileInfo.Load(vxIO.PathToTempFolder);

                OnFileInfoLoad(fileInfo);

                // Future Use, Handle IO Version Differences Here.
                loadResult = LoadXMLFile(FilePath, fileInfo.Version);

                // load preview image if available
                string previewImgPath = Path.Combine(vxIO.PathToTempFolder, "preview.jpg");
                m_previewImage = vxIO.LoadImage(previewImgPath, false);


                DateTime endTime = DateTime.Now;

                vxDebug.LogIO(new
                {
                    fileVersion=fileInfo.Version,
                    fileLoadTime = (float)(endTime - startTime).Milliseconds / 1000.0f,
                    fileGameVersion =fileInfo.GameInfo.Version,
                });
                //vxConsole.WriteIODebug(string.Format("Finished. File Loaded in: {0} ms", (float)(endTime - startTime).Milliseconds / 1000));
                //vxConsole.WriteIODebug(string.Format("     File Version: {0}.", fileInfo.Version));
                vxConsole.WriteIODebug("=====================================================");
            }
            return loadResult;
        }

        /// <summary>
        /// When File Info is Loaded. This is called before LoadFile(...) is called.
        /// </summary>
        /// <param name="fileInfo">File info.</param>
        protected virtual void OnFileInfoLoad(vxFileInfo fileInfo)
        {

        }

        protected virtual vxSandboxFileLoadResult LoadXMLFile(string FilePath, int version)
        {
            vxMessageBox ErrorMsgBox = new vxMessageBox(
                "This file version is not supported.\nMake sure you have the most up to date version\nand try again!",
                "Uncompatible File Version.");

            vxSceneManager.AddScene(ErrorMsgBox);
            Console.WriteLine("File Version is not supported.");

            return new vxSandboxFileLoadResult(FilePath);
        }


        /// <summary>
        /// Saves the current Sandbox File.
        /// </summary>
        /// <param name="takeScreenshot"></param>
        public virtual void SaveFile(bool takeScreenshot, bool DoDump = false)
        {
            // first check that there's a file name
            if (FileName == "")
                SaveFileAs();
            else
            {
                //takeScreenshot = true;
                if (takeScreenshot)
                {
                    IsGUIVisible = false;
                    this.Draw();
                    ThumbnailImage = vxScreen.TakeScreenshot();// LastFrame;
                    IsGUIVisible = true;
                }
                IsDumping = DoDump;
                //SerializeFile();
                vxSceneManager.AddScene(GetAsyncSaveScreen());
            }
        }
        public bool IsDumping = false;

        /// <summary>
        /// Gets the async save screen. Override this to provide a custom save screeen.
        /// </summary>
        /// <returns>The async save screen.</returns>
        public virtual vxSaveBusyScreen GetAsyncSaveScreen()
        {
            return new vxSaveBusyScreen(this);
        }



        /// <summary>
        /// Saves the support files such as as thumbnail and img. Override to add your own files.
        /// </summary>
        public virtual void SaveSupportFiles()
        {
            //First Check, if the Items Directory Doesn't Exist, Create It
            string ExtractionPath = vxIO.PathToTempFolder;

            // clear the temp directory

            string path = vxIO.PathToSandbox;

            if (IsDumping)
                ExtractionPath = path + "/dump";

            //First Check, if the Items Directory Doesn't Exist, Create It
            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);

            if (Directory.Exists(ExtractionPath) == false)
                Directory.CreateDirectory(ExtractionPath);

            if (ThumbnailImage == null)
                ThumbnailImage = vxScreen.TakeScreenshot();

            var size = OnGetPreviewImageSize();

            Texture2D img = ThumbnailImage.Resize(size.X, size.Y);

            img.SaveToDisk(ExtractionPath + "/img.png");

            if (m_previewImage == null)
                m_previewImage = ThumbnailImage;

            Texture2D preImg = PreviewImage.Resize(size.X, size.Y);
            preImg.SaveToDisk(ExtractionPath + "/preview.jpg", vxExtensions.ImageType.JPG);

            img.Resize(96, 96).SaveToDisk(ExtractionPath + "/thumbnail.png");

        }

        public virtual Point OnGetPreviewImageSize()
        {
            return vxScreen.Resolution;
        }

        /// <summary>
        /// This is a Verbose Debug info for which game saved the file.
        /// </summary>
        /// <returns></returns>
        public virtual string GetExporterInfo()
        {
            return "Saved by the Default Engine Exporter! v." + vxEngine.EngineVersion;
        }

        protected List<vxEntity> DisposalQueue = new List<vxEntity>();
        public virtual void AddForDisposal(vxEntity entity)
        {
            DisposalQueue.Add(entity);
        }

        /// <summary>
        /// File Format
        /// </summary>
        public vxSerializableSceneBaseData SandBoxFile;

        /// <summary>
        /// Initialises the Save File. If the XML Save file uses a different type, then
        /// this can be overridden.
        /// </summary>
        /// <returns></returns>
        public virtual vxSerializableSceneBaseData InitSaveFile()
        {
            return new vxSerializableSceneBaseData();
        }


        /// <summary>
        /// Returns a Deserializes the File. If you want to use a different type to Serialise than the base 'vxSerializableScene3DData'
        /// then you must override this or it will throw an error.
        /// </summary>
        public virtual vxSerializableSceneBaseData DeserializeFile(string path)
        {
            vxSerializableSceneBaseData file;
            XmlSerializer deserializer = new XmlSerializer(typeof(vxSerializableSceneBaseData));
            TextReader reader = new StreamReader(path);
            file = (vxSerializableSceneBaseData)deserializer.Deserialize(reader);
            reader.Close();

            return file;
        }

        /// <summary>
        /// A thumbnail of the latest run
        /// </summary>
        public Texture2D ThumbnailImage;

        /// <summary>
        /// The preview image used for this level
        /// </summary>
        public Texture2D PreviewImage
        {
            get {
                if (m_previewImage == null)
                    m_previewImage = vxScreen.TakeScreenshot();
                
                return m_previewImage; }
            set { m_previewImage = value; }
        }
        private Texture2D m_previewImage;

        /// <summary>
        /// Returns back whether this is a new sandbox file
        /// </summary>
        public bool IsNewSandboxFile
        {
            get { return FilePath == string.Empty; }
        }

        public string FilePath = "";

        public string FileName = "";

        /// <summary>
        /// An in game Open File Dialog too access files from specific directories.
        /// </summary>
        public vxOpenSandboxFileDialog OpenFileDialog;

        /// <summary>
        /// Save As Message Box.
        /// </summary>
        //vxMessageBoxSaveAs SaveAsMsgBox;


        /// <summary>
        /// Start a New File
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Event_NewFileToolbarItem_Clicked(object sender, vxUIControlClickEventArgs e)
        {
            vxMessageBox NewFile = new vxMessageBox("Are you sure you want to Start a New File,\nAll Unsaved Work will be Lost", "quit?");
            vxSceneManager.AddScene(NewFile, ControllingPlayer);
            NewFile.Accepted += Event_NewFile_Accepted;
        }

        /// <summary>
        /// When to do when the New File button is clicked. This must be overridden.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Event_NewFile_Accepted(object sender, PlayerIndexEventArgs e)
        {
            vxSceneManager.LoadScene(OnNewSandbox());
        }

        /// <summary>
        /// Called when a New Sandbox file is called. Override this to provide your base class.
        /// </summary>
        /// <returns>The new sandbox.</returns>
        public virtual vxGameplaySceneBase OnNewSandbox()
        {
            return new vxGameplaySceneBase();
        }





        /// <summary>
        /// Event for Opening a File
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Event_OpenFileToolbarItem_Clicked(object sender, vxUIControlClickEventArgs e)
        {
            OpenFileDialog = new vxOpenSandboxFileDialog( "sbx");
            vxSceneManager.AddScene(OpenFileDialog, ControllingPlayer);
            OpenFileDialog.Accepted += Event_OpenFileDialog_Accepted;
        }

        public virtual void Event_OpenFileDialog_Accepted(object sender, vxSandboxFileSelectedEventArgs e)
        {
            vxMessageBoxSaveBeforeQuit saveBeforeCloseCheck = new vxMessageBoxSaveBeforeQuit("Are you sure you want to close without Saving?\nAll un-saved work will be lost", "Close Without Saving?");
            saveBeforeCloseCheck.Apply += Event_SaveBeforeCloseCheck_Save;
            saveBeforeCloseCheck.Accepted += Event_SaveBeforeCloseCheck_DontSave;
            vxSceneManager.AddScene(saveBeforeCloseCheck, ControllingPlayer);
        }

        /// <summary>
        /// Called when a New Sandbox file is called. Override this to provide your base class.
        /// </summary>
        /// <returns>The new sandbox.</returns>
        public virtual vxGameplaySceneBase OnOpenSandboxFile(string filePath)
        {
            return new vxGameplaySceneBase();
        }

        void LoadFileFromOpenDialog()
        {
            vxSceneManager.LoadScene(OnOpenSandboxFile(OpenFileDialog.SelectedItem));
        }


        public virtual void Event_SaveBeforeCloseCheck_Save(object sender, PlayerIndexEventArgs e)
        {
            SaveFile(true);
            LoadFileFromOpenDialog();
        }

        public virtual void Event_SaveBeforeCloseCheck_DontSave(object sender, PlayerIndexEventArgs e)
        {
            LoadFileFromOpenDialog();
        }

        public virtual void SaveFileAs(string saveAsMsg = "Save the current file as...")
        {
            // Capture the Thumbnail before the Message Box Pops up.
            IsGUIVisible = false;
            ThumbnailImage = vxScreen.TakeScreenshot();
            IsGUIVisible = true;

            //SaveAsMsgBox = new vxMessageBoxSaveAs(saveAsMsg, "Save As", FileName);
            //vxSceneManager.AddScene(SaveAsMsgBox, ControllingPlayer);
            //SaveAsMsgBox.Accepted += Event_SaveAsMsg_Accepted;

            vxMessageInputBox.Show("Save As", saveAsMsg, FileName, (input) =>
            {
                SaveAs(input);
            });
        }

        /// <summary>
        /// Event for Saving the Current File
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Event_SaveFileToolbarItem_Clicked(object sender, vxUIControlClickEventArgs e)
        {
            SaveFile(true);
        }


        /// <summary>
        /// Event for Saving As the Current File
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Event_SaveAsFileToolbarItem_Clicked(object sender, vxUIControlClickEventArgs e)
        {
            SaveFileAs();
        }

        //public virtual void Event_SaveAsMsg_Accepted(object sender, PlayerIndexEventArgs e)
        //{
        //    //FileName = SaveAsMsgBox.Textbox.Text;
        //    //SaveAs(SaveAsMsgBox.Textbox.Text);
        //}

        void SaveAs(string filename)
        {
            FileName = filename;
            if (File.Exists(Path.Combine(vxIO.PathToSandbox, filename + ".sbx")))
            {
                var confirmOverriteMsgBox = new vxMessageBox("Overwrite File '" + filename + ".sbx'?", "Overwrite?");

                confirmOverriteMsgBox.Accepted += delegate {
                    vxConsole.WriteIODebug("Saving: " + filename);
                    SaveFile(false);
                };
                vxSceneManager.AddScene(confirmOverriteMsgBox);
            }
            else
            {
                vxConsole.WriteIODebug("Saving: " + filename);
                SaveFile(false);
            }
        }

        public virtual void DumpFile()
        {
            string compFile = vxIO.PathToSandbox + "/dump/" + FileName + ".sbx";

            vxConsole.WriteLine(string.Format("Dumping file '{0}'...", compFile));

            //SaveFile(true, true);
            IsDumping = true;
            var saveScreen = GetAsyncSaveScreen();
            saveScreen.StartSave();

            vxConsole.WriteLine(string.Format("Files Dumped"));
        }

        public virtual void PackFile()
        {
            vxConsole.WriteLine(string.Format("Packing Files from '{0}'", vxIO.PathToSandbox + "/dump/"));

            string compFile = vxIO.PathToSandbox + "/dump/" + FileName + ".sbx";

            if (File.Exists(compFile))
                File.Delete(compFile);

            vxIO.CompressDirectory(vxIO.PathToSandbox + "/dump", compFile, null);

            vxConsole.WriteLine(string.Format("Files packed into '{0}'", compFile));
        }


        #endregion

        #region Particles 


        /// <summary>
        /// Spawns a new particle using the specified key from the Particle System.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="emitter"></param>
        public void SpawnParticle(object key, vxGameObject emitter)
        {
            ParticleSystem.SpawnParticle(key, emitter);
        }

        #endregion

        #region Sandbox

        public virtual void SimulationStart() { }

        public virtual void SimulationStop() { }

        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the vxGameBaseScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        protected internal override void Update()
        {
            base.Update();

            // Handle Disposing of Items
            if (DisposalQueue.Count > 0)
            {
                // Loop through entire queue
                foreach (var entity in DisposalQueue)
                    entity.Dispose();

                // now clear it
                DisposalQueue.Clear();
            }

            //Only update the GUI if the Current Screen is Active
            if (IsActive && IsGUIVisible)
            {
                _uiManager.Update();
            }

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen && IsSceneDimmedOnCover)
                PauseAlpha = Math.Min(PauseAlpha + 1f / 32, 1);
            else
                PauseAlpha = Math.Max(PauseAlpha - 1f / 32, 0);

            if (IsActive || IsPausable == false)
            {
                //UpdateScene (gameTime, otherScreenHasFocus,coveredByOtherScreen);
            }

            // Update Camera
            //**********************************************************************************************
            
            if(Cameras != null)
            for (int c = 0; c < Cameras.Count; c++)
                Cameras[c].Update();

            ParticleSystem.Update();

            // update the subsystems
            for(int s = 0; s < _subsystems.Count; s++)
            {
                if (_subsystems[s].IsEnabled)
                {
                    _subsystems[s].Update();
                }
            }
        }



        /// <summary>
        /// Main Game Update Loop which is accessed outside of the vxEngine
        /// </summary>
        protected internal virtual void UpdateScene()
        {

        }

        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        protected internal override void HandleInput()
        {

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            if (vxInput.IsPauseGame())
            {
                if (SandboxStartGameType != vxStartGameMode.GamePlay &&
                    SandboxCurrentState == vxEnumSandboxStatus.Running)
                    SimulationStop();
                else
                    ShowPauseScreen();
            }
            else
            {
                HandleInputBase();
            }
        }

        /// <summary>
        /// Handles the input base.
        /// </summary>
        /// <param name="input">Input.</param>
        protected virtual void HandleInputBase()
        {

        }


        /// <summary>
        /// This Method Loads the Engine Base Pause Screen (PauseMenuScreen()), but 
        /// more items might be needed to be added. Override to
        /// load your own Pause Screen.
        /// </summary>
        /// <example> 
        /// This sample shows how to override the <see cref="ShowPauseScreen"/> method. 'MyGamesCustomPauseScreen()' inheirts
        /// from the <see cref="VerticesEngine.UI.Menus.vxMenuBaseScreen"/> Class.
        /// <code>
        /// //This Allows to show your own custom pause screen.
        /// public override void ShowPauseScreen()
        /// {
        ///     vxSceneManager.AddScreen(new MyGamesCustomPauseScreen(), ControllingPlayer);
        /// }
        /// </code>
        /// </example>
        public virtual void ShowPauseScreen()
        {
            vxSceneManager.AddScene(new vxPauseMenuScreen(), ControllingPlayer);
        }


        /// <summary>
        /// The color to clear the back buffer with.
        /// </summary>
        public Color BackBufferClearColor = Color.DarkMagenta;


        /// <summary>
        /// Draw's the background before proceeding with other drawing. This is useful for Skyboxes and backgrounds as a whole
        /// </summary>
        public virtual void DrawBackground()
        {

        }

        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw()
        {
            vxGraphics.GraphicsDevice.Clear(ClearOptions.Target, BackBufferClearColor, 0, 0);

            // Draw the scene
            DrawScene();


            vxProfiler.BeginMark(vxProfiler.Tags.UI_DRAW);
            // if the GUI should be shown, then show it.
            if (IsGUIVisible && !IsUIVisibilitySuppressed)
            {
                vxGraphics.SpriteBatch.Begin("UI.MainGUI");
                DrawGUIItems();
                vxGraphics.SpriteBatch.End();
            }
            vxProfiler.EndMark(vxProfiler.Tags.UI_DRAW);


            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || PauseAlpha > 0)
                DrawTransition(MathHelper.Lerp(1f - TransitionAlpha, 1f, PauseAlpha * PauseAlphaFactor));

            DrawDebug();

        }

        float PauseAlphaFactor = 0.5f;
        protected internal virtual void DrawTransition(float transitionPosition)
        {
            vxSceneManager.FadeBackBufferToBlack(transitionPosition  * (IsExiting ? 1 : 0.67f));
        }


        protected internal virtual void DrawDebug()
        {
            vxRenderPipeline.Instance.DrawDebug();
        }


        /// <summary>
        /// Draws the GUII tems. this is pre-faced by a SpriteBatch.Begin() call.
        /// </summary>
        protected internal virtual void DrawGUIItems()
        {
            _uiManager.DrawByOwner();
        }



        /// <summary>
        /// This is called before the main sprite draw, but after the SpriteBatch.Begin() call. Overload this
        /// if you want to do some drawing before the main sprite draws, but within the same SpriteBatch call
        /// for efficinency.
        /// </summary>
        protected internal virtual void PreDraw() { }

        /// <summary>
        /// This is called after the main sprite draw, but before the SpriteBatch.End() call. Overload this
        /// if you want to do some drawing after the main sprite draws, but within the same SpriteBatch call
        /// for efficinency.
        /// </summary>
        protected internal virtual void PostDraw() { }

        /// <summary>
        /// Called after the full scene is drawn and all post processes are applied.
        /// </summary>
        protected internal virtual void OnPostRender() { }

        /// <summary>
        /// Is the UI Visibility suppressed. This is useful for certain cases in Screen shots
        /// </summary>
        public bool IsUIVisibilitySuppressed = false;

        /// <summary>
        /// Draws the scene.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public virtual void DrawScene()
        {

            // **********************************************************************************************
            // first render each camera to their respective back buffers
            for (int c = 0; c < UtilCameras.Count; c++)
            {
                if (UtilCameras[c].IsUtilCameraActive)
                {
                    UtilCameras[c].Render();
                    UtilCameras[c].IsUtilCameraActive = false;
                }
            }

            // first render each camera to their respective back buffers
            for (int c = 0; c < Cameras.Count; c++)
            {
                Cameras[c].Render();
            }

            // now combine each of the camera's to the Engine's Final Back buffer
            vxGraphics.GraphicsDevice.SetRenderTarget(vxGraphics.FinalBackBuffer);

            vxGraphics.SpriteBatch.Begin("Scene Draw");

            for (int c = 0; c < Cameras.Count; c++) 
            {
                vxGraphics.SpriteBatch.Draw(Cameras[c].FinalScene, Cameras[c].Viewport.Bounds.Location.ToVector2(), Color.White);
            }

            vxGraphics.SpriteBatch.End();

            IsGUIVisible = (SandboxCurrentState == vxEnumSandboxStatus.EditMode);

            // Debug Rendering & Draw any inherited or overriden code
            //**********************************************************************************************
            DrawGameplayScreen();

            // Draw Overlay items such as 3D Sandbox Cursor and HUD
            //**********************************************************************************************
            DrawOverlayItems();

            DrawHUD();
        }

        /// <summary>
        /// Draws the Temp Entities using a special renderer and material
        /// </summary>
        protected internal virtual void DrawTempEntities() { }

        protected internal virtual void DrawOverlayItems() { }

        protected internal virtual void DrawHUD() { }

        /// <summary>
        /// Main Gameplay Draw Loop which is accessed outside of the Engine.
        /// </summary>
        protected internal virtual void DrawGameplayScreen() { }


        /// <summary>
        /// Draws the viewport splitters. This is called during the HUD draw call, therefore
        /// will be scooped up into that Spritebatch Call.
        /// </summary>
        protected internal virtual void DrawViewportSplitters() { }

        #endregion

        #region Debug Drawing

        /// <summary>
        /// Draws the physics debug overlay for the individual systems.
        /// </summary>
        protected internal virtual void DrawPhysicsDebug(vxCamera camera) { }

        #endregion
    }
}
