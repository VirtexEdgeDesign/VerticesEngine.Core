
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using VerticesEngine.Controllers;
using VerticesEngine.Editor.Entities;
using VerticesEngine.Editor.UI;
using VerticesEngine.Graphics;
using VerticesEngine.Profile;
using VerticesEngine.Serilization;
using VerticesEngine.UI;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Dialogs;
using VerticesEngine.UI.Events;
using VerticesEngine.UI.Menus;
using VerticesEngine.UI.MessageBoxs;
using VerticesEngine.Util;
using VerticesEngine.Utilities;
using VerticesEngine.Workshop;

namespace VerticesEngine
{
    /// <summary>
    /// Editor Camera Mode
    /// </summary>
    public enum vxEditorCameraMode
    {
        /// <summary>
        /// Editor Scene Camera Controls
        /// </summary>
        Editor,

        /// <summary>
        /// Fly Camera
        /// </summary>
        Fly,

        /// <summary>
        /// Orbit Mode around a specific piece
        /// </summary>
        Orbit,
    }

    /// <summary>
    /// What should happen when we click a Mouse Click
    /// </summary>
    public enum MouseClickState
    {
        /// <summary>
        /// A regular mode where we select an item and it's properties are shown in the inspector
        /// </summary>
        SelectItem,

        /// <summary>
        /// A special selection mode where we return the selected item without selecting it
        /// </summary>
        ReturnItemToInspector
    }

    public partial class vxGameplayScene3D
    {
        /// <summary>
        /// Current Key being used too add new entities.
        /// </summary>
        public string CurrentlySelectedKey = "";

        public vxEnumAddMode AddMode = vxEnumAddMode.OnPlane;

        public MouseClickState MouseClickState = MouseClickState.SelectItem;

        /// <summary>
        /// Gets or sets the camera edit mode.
        /// </summary>
        /// <value>The camera edit mode.</value>
        vxEditorCameraMode CameraEditMode 
        {
            get { return _cameraEditMode; }
            set{
                
                _cameraEditMode = value;
                switch (_cameraEditMode)
                {
                    case vxEditorCameraMode.Orbit:
                        foreach(var camera in Cameras)
                            camera.CameraType = vxCameraType.Orbit;
                        break;
                    case vxEditorCameraMode.Fly:
                        foreach (var camera in Cameras)
                            camera.CameraType = vxCameraType.Freeroam;
                        break;

                    case vxEditorCameraMode.Editor:
                        foreach (var camera in Cameras)
                            camera.CameraType = vxCameraType.SceneEditor;
                        break;
                }
            }
        }
        vxEditorCameraMode _cameraEditMode = vxEditorCameraMode.Fly;

        /// <summary>
        /// What Time is it Mr. Wolf.
        /// </summary>
        public TimeOfDay TimeOfDay
        {
            get { return SandBoxFile.Enviroment.TimeOfDay; }
            set { SandBoxFile.Enviroment.TimeOfDay = value; }
        }

        /// <summary>
        /// Gets or sets the sandbox edit mode.
        /// </summary>
        /// <value>The sandbox edit mode.</value>
        public vxEnumSanboxEditMode SandboxEditMode
        {
            get { return _sandboxEditMode; }
            set { 
                _sandboxEditMode = value; 

                if(_sandboxEditMode != vxEnumSanboxEditMode.AddItem)
                    DisposeOfTempPart();
            }
        }
        vxEnumSanboxEditMode _sandboxEditMode = vxEnumSanboxEditMode.SelectItem;
                

        /// <summary>
        /// Player
        /// </summary>
        public CharacterControllerInput character;

        /// <summary>
        /// File Format
        /// </summary>
        public new vxSerializableScene3DData SandBoxFile
        {
            get { return (vxSerializableScene3DData)base.SandBoxFile; }
        }

        /// <summary>
        /// List of Current Selected Items
        /// </summary>
        public List<vxEntity3D> SelectedItems
        {
            get { return m_selectedItems; }
        }
        private List<vxEntity3D> m_selectedItems;

        /// <summary>
        /// The Currently Selected Type of Entity to be added in the Sandbox
        /// </summary>
        protected vxEntity3D TempPart;

        /// <summary>
        /// The current temporary sandbox entity. This can be null.
        /// </summary>
        public vxEntity3D TempSandboxEntity
        {
            get { return TempPart; }
        }

        /// <summary>
        /// Previous Working Plane Intersection Point
        /// </summary>
        public Vector3 PreviousIntersection = new Vector3();

        /// <summary>
        /// "Out Of Sight" Position
        /// </summary>
        public Vector3 OutofSight = new Vector3();

        /// <summary>
        /// Should Ray Selection Handling be enabled.
        /// </summary>
        public bool IsRaySelectionEnabled = false;


        /// <summary>
        /// The boolean for if the mouse is over snap box or not for entity placement..
        /// </summary>
        public bool IsMouseOverSnapBox = false;

        /// <summary>
        /// The is grid snap.
        /// </summary>
        public bool IsGridSnap = true;


        public Vector3 Intersection;

        //public vxSandboxNewItemDialog NewSandboxItemDialog;

        #region -- Sandbox Editor Entities --

        /// <summary>
        /// The entity editing gimbal which controls selected entity translation and rotation.
        /// </summary>
        private vxGizmo m_gizmo;

        /// <summary>
        /// Working Plane
        /// </summary>
        private vxWorkingPlane m_workingPlane;

        #endregion
        
        #region -- UI Elemnts --
        

        
        #region - Ribbon Control -

        vxRibbonToolbarButtonControl undoBtn;
        vxRibbonToolbarButtonControl redoBtn;
        vxRibbonToolbarButtonControl selcMode, tlbrNewEntity, tlbrDeleteEntity;

        #endregion


        #region - Context Menu -

        /// <summary>
        /// The avaialble Context menu control. This can be accessed with [LCtrl] + [Right Mouse Click].
        /// </summary>
        protected vxContextMenuControl ContextMenu
        {
            get { return m_contextMenu; }
        }
        private vxContextMenuControl m_contextMenu;

        //protected vxContextMenuItem CntxtMenuCameraToggle;
        protected vxContextMenuItem CntxtMenuViewProperties;

        #endregion

        private vxSlideTabControl EntitySlideTabControl;


        protected vxSlideTabPage SandboxEntitySelector
        {
            get { return m_entitySlideTab; }
        }
        private vxSlideTabPage m_entitySlideTab;

        #region - Property Controls -

        /// <summary>
        /// Main Tab Control Which Holds All Properties
        /// </summary>
        public vxSlideTabControl PropertiesTabControl;


        /// <summary>
        /// The vxScrollPanel control which is used too store Entity Properties. See the GetProperties Method for examples.
        /// </summary>
        public vxPropertiesControl EntityPropertiesControl;

        private vxPropertiesControl WorlPropertiesControl;

        private vxPropertiesControl EffectPropertiesControl;
        #endregion


        #endregion

        /****************************************************************************/
        /*                               EVENTS
        /****************************************************************************/
        /// <summary>
        /// The Event Fired when a New Item is Selected
        /// </summary>
        public event EventHandler<EventArgs> ItemSelected;

        /// <summary>
        /// The event called when a property in the inspector is waiting for the user to select a game object
        /// </summary>
        public event EventHandler<vxSandboxItemSelectedForInspectorEventArgs> ItemSelectedForInspector;

        public ISnapbox HoveredSnapBox;

        public vxTransform HoveredSnapBoxWorld;
        
        private Ray MouseRay;


        public vxEnumTerrainEditMode TerrainEditState = vxEnumTerrainEditMode.Sculpt;




        public virtual bool IsSurface(vxEntity3D HoveredEntity)
        {
            if (HoveredEntity == TempPart)
            {
                return false;
            }
            else
                return true;
        }


        /// <summary>
        /// Starts the Sandbox.
        /// </summary>
        public override void SimulationStart()
        {
            foreach (var camera in Cameras)
                camera.ProjectionType = camera.DefaultProjectionType;

            //Clear out the Edtor Items
            m_editorItems.Clear();

            foreach (vxEntity3D entity in Entities)
            {
                if (entity != null)
                {
                    m_editorItems.Add(entity);
                    entity.OnSandboxStatusChanged(true);
                }
            }
        }


        /// <summary>
        /// Stops the Sandbox.
        /// </summary>
        public override void SimulationStop()
        {
            foreach (vxCamera3D camera in Cameras)
                camera.CameraType = vxCameraType.SceneEditor;

            SandboxEditMode = vxEnumSanboxEditMode.SelectItem;

            foreach (var camera in Cameras)
            {
                camera.ProjectionType = camera.EditorProjectionType;
                camera.CameraType = vxCameraType.SceneEditor;
            }

            CurrentlySelectedKey = "";
            for (int i = 0; i < Entities.Count; i++)
            {
                if (Entities[i] != null)
                {
                    Entities[i].OnSandboxStatusChanged(false);

                    if (m_editorItems.Contains(Entities[i]) == false)
                    {
                        Entities[i].Dispose();
                        i--;
                    }
                }
            }
        }

        /// <summary>
        /// This method is called near the end of the LoadContent method to load al GUI Items pertaining to the 
        /// Sandbox including the toolbars and item registration.
        /// </summary>
        protected override void OnInitialiseGUI()
        {
            // Setup the Toolbars
            OnInitialiseInternalUIRibbonControl();

            OnInitialiseUITerrainToolbar();


            // Setup Properties Sliding Tab Control
            PropertiesTabControl = new vxSlideTabControl(vxUIItemOrientation.Right, new Vector2(-50, 140),
                350, vxGraphics.GraphicsDevice.Viewport.Height - 140);

            UIManager.Add(PropertiesTabControl);

            EntityPropertiesControl = CreatePropertiesControl(vxLocKeys.Entities);
            WorlPropertiesControl = CreatePropertiesControl(vxLocKeys.Sandbox_World);

            if (vxEngine.BuildType == vxBuildType.Debug)
                EffectPropertiesControl = CreatePropertiesControl(vxLocKeys.Sandbox_Renderer);


            EntitySlideTabControl = new vxSlideTabControl(vxUIItemOrientation.Bottom, new Vector2(50, -32), vxScreen.Width - 128, 256);
            UIManager.Add(EntitySlideTabControl);


            m_entitySlideTab = new vxSlideTabPage(EntitySlideTabControl, "Entities");
            vxSlideTabPage consoleSlideTab = new vxSlideTabPage(EntitySlideTabControl, vxLocKeys.Sandbox_Console);
            m_entitySlideTab.IsTitleVisible = false;
            EntitySlideTabControl.AddPage(m_entitySlideTab);
            //EntitySlideTabControl.AddPage(consoleSlideTab);

            m_uiSandboxEntityTabControl = new vxTabControl(new Rectangle(8, 8, vxScreen.Width - 164, 256));
            //UIManager.Add(m_uiSandboxEntityTabControl);
            m_entitySlideTab.AddItem(m_uiSandboxEntityTabControl);

            var selectionBox = new vxSelectionBox();

            selectionBox.OnSelection += SelectionBox_OnSelection;

            UIManager.Add(selectionBox);

            m_contextMenu = new vxContextMenuControl();

            UIManager.Add(ContextMenu);

            //ContextMenu.AddItem("Cut");
            //ContextMenu.AddItem("Copy");
            //ContextMenu.AddItem("Paste");
            ContextMenu.AddSplitter();
            var undoContext = ContextMenu.AddItem(vxLocKeys.Sandbox_Edit_Undo);
            undoContext.Clicked += UndoToolbarItem_Clicked;
            var redoContext = ContextMenu.AddItem(vxLocKeys.Sandbox_Edit_Redo);
            redoContext.Clicked += RedoToolbarItem_Clicked;

            ContextMenu.AddSplitter();

            var selectItemTypeContext = ContextMenu.AddItem("Select Items of General Type...");
            selectItemTypeContext.Clicked += delegate
            {
                if(SelectedItems.Count > 0)
                {
                    // get current selected item
                    var item = SelectedItems[0];

                    foreach(var entity in Entities)
                    {
                        if(entity.GetType().BaseType == item.GetType().BaseType)
                        {
                            SelectEntity((vxEntity3D)entity, false, false);
                        }
                    }
                }
                else
                {
                    vxNotificationManager.Show("Please select at least 1 item", Color.Red);
                }
            };


            var selectExactItemTypeContext = ContextMenu.AddItem("Select Items of Exact Type...");
            selectExactItemTypeContext.Clicked += delegate
            {
                if (SelectedItems.Count > 0)
                {
                    // get current selected item
                    var item = SelectedItems[0];

                    foreach (var entity in Entities)
                    {
                        if (entity.GetType() == item.GetType())
                        {
                            SelectEntity((vxEntity3D)entity, false, false);
                        }
                    }
                }
                else
                {
                    vxNotificationManager.Show("Please select at least 1 item", Color.Red);
                }
            };

            ContextMenu.AddSplitter();

            var frameViewContextItem = new vxContextMenuItem(ContextMenu, vxLocKeys.Sandbox_View_FrameView, vxInternalAssets.UI.ContextMenuFrameView);
            frameViewContextItem.Clicked += delegate
            {                
                if (SelectedItems.Count > 0 && SelectedItems[0] != null)
                {
                    foreach (var Camera in Cameras)
                    {
                        Camera.GetComponent<vxCameraSceneEditorController>().FrameToSelectedObject();
                    }
                }

            };

            var CntxtMenuCameraToggle = new vxContextMenuItem(ContextMenu, vxLocKeys.Sandbox_View_OrbitSelection, vxInternalAssets.UI.ContextMenuOrbitSelection);
            CntxtMenuCameraToggle.Clicked += delegate
            {
                
                if (SelectedItems.Count > 0 && SelectedItems[0] != null)
                {
                    foreach (var Camera in Cameras)
                    {
                        Camera.CameraType = vxCameraType.Orbit;
                        Camera.CastAs<vxCamera3D>().OrbitTarget = SelectedItems[0].Position;
                        var rad = SelectedItems[0].BoundingShape.Radius;
                        Camera.CastAs<vxCamera3D>().OrbitZoom = 250 * rad;
                    }
                }

            };
            ContextMenu.AddSplitter();

            CntxtMenuViewProperties = new vxContextMenuItem(ContextMenu, "Properties", vxInternalAssets.UI.PropertyControlIcon);
            CntxtMenuViewProperties.Clicked += delegate
            {
                PropertiesTabControl.Pages[0].Open();
            };

            ContextMenu.Position = new Vector2(0);

            InitContextMenu();
        }

        /// <summary>
        /// Called when the context menu is initialised. This is useful for upstream classes to use
        /// </summary>
        protected virtual void InitContextMenu()
        {

        }

        /// <summary>
        /// The world properties
        /// </summary>
        public vxSceneProperties WorldProperties
        {
            get { return _worldProperties; }
        }
        protected vxSceneProperties _worldProperties;

        protected virtual vxSceneProperties InitWorldProperties()
        {
            return new vxSceneProperties(this);
        }

        /// <summary>
        /// Holds all sandbox entity tabs
        /// </summary>
        vxTabControl m_uiSandboxEntityTabControl;


        /// <summary>
        /// Creates a new properties control window in the right hand properties slider control.
        /// </summary>
        /// <returns>The properties control.</returns>
        /// <param name="name">Name.</param>
        public vxPropertiesControl CreatePropertiesControl(string name)
        {
            vxSlideTabPage propertiesTabPage = new vxSlideTabPage(PropertiesTabControl, name);
            PropertiesTabControl.AddPage(propertiesTabPage);
            propertiesTabPage.Tab.Font = vxInternalAssets.Fonts.ViewerFont;

            vxPropertiesControl newPropertiesControl = new vxPropertiesControl(new Vector2(0, 24), propertiesTabPage.Bounds.Width, propertiesTabPage.Bounds.Height - vxLayout.GetScaledHeight(128));
            propertiesTabPage.AddItem(newPropertiesControl);

            return newPropertiesControl;

        }

        vxScrollPanel m_importedEntitiesScrollPage;
        /// <summary>
        /// Override this Method and add in your Register Sandbox Entities code.
        /// </summary>
        protected override void RegisterSandboxEntities()
        {
            int width = 256;
            int height = 256;
            // Now use the Category/Sub Category structure to create the proper GUI laytout
            foreach (var category in vxEntityRegister.Categories.Values)
            {
                // for each category, create a new tab page
                string locTabName = "Sandbox/Tabs/" + category.name.Replace(" ", string.Empty);
                vxTabPageControl itemsTabPage = new vxTabPageControl(locTabName);

                //NewSandboxItemDialog.TabControl.Add(itemsTabPage);
                m_uiSandboxEntityTabControl.Add(itemsTabPage);

                var scrollPanel = new vxScrollPanel(Vector2.Zero, itemsTabPage.Width, itemsTabPage.Height - itemsTabPage.Tab.Height);
                width = itemsTabPage.Width;
                height = itemsTabPage.Height - itemsTabPage.Tab.Height;
                foreach (var subCategory in category.SubCategories.Values)
                {
                    // for each sub category, create a new section within the page
                    scrollPanel.AddItem(new vxScrollPanelSpliter(subCategory.Name));


                    foreach (var type in subCategory.types)
                    {
                        try
                        {
                            // now add each item to the proper category and sub category
                            var btn = RegisterNewSandboxItem(type);
                            if (btn != null)
                            {
                                if (type.IsVisibleInSandboxList)
                                    scrollPanel.AddItem(btn);
                            }
                            else
                            {
                                vxConsole.WriteLine("Trouble Registering " + type.ToString());
                            }
                        }
                        catch(Exception ex)
                        {
                            vxConsole.WriteException("Exception Registering "+type.ToString(), ex);
                        }
                    }
                }
                itemsTabPage.Add(scrollPanel);
            }

            // create the imported entity tab page
            vxTabPageControl m_importedEntitiesTabPag;
            m_importedEntitiesTabPag = new vxTabPageControl(vxLocKeys.Sandbox_Tabs_ImportedEntities);

            m_importedEntitiesScrollPage = new vxScrollPanel(Vector2.Zero, width, height);
            //m_importedEntitiesScrollPage = new vxScrollPanel(Vector2.Zero, m_importedEntitiesTabPag.Width, m_importedEntitiesTabPag.Height - m_importedEntitiesTabPag.Tab.Height);

            // for each sub category, create a new section within the page
            m_importedEntitiesScrollPage.AddItem(new vxScrollPanelSpliter("Imported Entities"));

            m_importedEntitiesTabPag.Add(m_importedEntitiesScrollPage);
            m_uiSandboxEntityTabControl.Add(m_importedEntitiesTabPag);


#if DEBUGF
            using (StreamWriter writer = new StreamWriter(Path.Combine(vxIO.PathToCacheFolder, "sandbox_icons.txt")))
            {
                //writer.WriteLine("#---------------------------------- Custom ---------------------------------#");

                // save out file
                foreach (var category in vxEntityRegister.Categories.Values)
                {
                    foreach (var subCategory in category.SubCategories.Values)
                    {
                        foreach (var type in subCategory.types)
                        {
                            string folderPath = vxIO.PathToCacheFolder + "/SandboxIcons";
                            if (Directory.Exists(folderPath) == false)
                                Directory.CreateDirectory(folderPath);
                            // check if it exists
                            string pngFilePath = Path.Combine(folderPath, type.Type.Name + "_icon.png");

                            string xnbFilePath = Path.Combine("txtrs","sandbox", "entity_icons", type.Type.Name + "_icon");

                            // if it's in the cache but no where else, then add it to the content manager
                            if (File.Exists(pngFilePath) && 
                                !File.Exists(Path.Combine(vxEngine.Game.Content.RootDirectory, type.FilePath + "_icon.xnb")) &&
                                !File.Exists(Path.Combine(vxEngine.Game.Content.RootDirectory, xnbFilePath + ".xnb")))
                            {
                                xnbFilePath = xnbFilePath.Replace('\\', '/');
                                writer.WriteLine("");
                                writer.WriteLine($"#begin {xnbFilePath}.png");
                                writer.WriteLine("/importer:TextureImporter");
                                writer.WriteLine("/processor:TextureProcessor");
                                writer.WriteLine("/processorParam:ColorKeyColor=255,0,255,255");
                                writer.WriteLine("/processorParam:ColorKeyEnabled=True");
                                writer.WriteLine("/processorParam:GenerateMipmaps=False");
                                writer.WriteLine("/processorParam:PremultiplyAlpha=True");
                                writer.WriteLine("/processorParam:ResizeToPowerOfTwo=False");
                                writer.WriteLine("/processorParam:MakeSquare=False");
                                writer.WriteLine("/processorParam:TextureFormat=Color");
                                writer.WriteLine($"/build:{xnbFilePath}.png");
                            }
                        }
                    }
                }
            }
#endif
        }


        /// <summary>
        /// Shows the settings dialog for this level editor. This needs to be overriden.
        /// </summary>
        protected virtual void OnShowSettingsDialog()
        {
            vxSceneManager.AddScene(new vxMessageBox("You should override the 'ShowSettingsDialog()'\n"+
                                              "method to add your own settings dialog.", "Settings"));
        }

        protected virtual void OnShowEngineSettingsDialog()
        {
            string text = "Engine Input Settings\n" +
                "                                                                                                                                                \n" +
                "Camera\n" +
                "  Zoom  -  [Middle Mouse Button]\n" +
                "  Rotate  -  [Right Mouse + Shift]\n\n" +
                "                                                                        \n" +
                "Items\n" +
                "  Place Item  -  [Left Mouse]\n" +
                "  Rotate Item  -  [Right Mouse + Shift]\n\n" +
            "                                                                        \n" +
            "Working Plane\n" +
            "  Raise/Lower  -  [Scroll Wheel + Shift]";
            vxSceneManager.AddScene(new vxMessageBox(text, "Settings"));
        }

        protected virtual void OnShowHelp()
        {
            //Engine.OpenWebPage("https://github.com/VirtexEdgeDesign/VerticesEngine/wiki");
        }

        protected virtual void OnShowAbout()
        {
            //Engine.OpenWebPage("https://virtexedgedesign.com/");
        }

        protected virtual void OnReportBug()
        {
            //Engine.OpenWebPage("https://virtexedgedesign.com/");
        }

        protected virtual void OnGoToDiscord()
        {
            //Engine.OpenWebPage("https://virtexedgedesign.com/");
        }
        protected virtual void OnGoToReddit()
        {
            //Engine.OpenWebPage("https://virtexedgedesign.com/");
        }

        void UpdateViewTabItemState()
        {
            m_uiCameraTypeDropDown.Text = Cameras[0].CameraType.ToString();
            m_uiCameraProjTypeDropDown.Text = Cameras[0].ProjectionType.ToString();
        }

        #region Main Ribbon Bar
        protected vxRibbonControl EditorRibbonControl;
        vxRibbonButtonControl m_uiGizmoGlobalTransl;
        vxRibbonButtonControl m_uiGizmoLocalTransl;
        vxRibbonDropdownControl m_uiCameraTypeDropDown;
        vxRibbonDropdownControl m_uiCameraProjTypeDropDown;

        protected vxRibbonTabPage HomeTabPage;
        protected vxRibbonTabPage PlayTabPage;

        void OnInitialiseInternalUIRibbonControl()
        {
            
            EditorRibbonControl = new vxRibbonControl(UIManager, new Vector2(0, 0));
            EditorRibbonControl.TabStartOffset = 38;
            
            HomeTabPage = new vxRibbonTabPage(EditorRibbonControl, "Home");

            var FileOpenGroup = new vxRibbonControlGroup(HomeTabPage, "File");

            var openFile = new vxRibbonButtonControl(FileOpenGroup, vxLocKeys.Sandbox_File_Open,vxInternalAssets.UI.RibbonOpen32, vxEnumButtonSize.Big);
            var saveFile = new vxRibbonButtonControl(FileOpenGroup, vxLocKeys.Sandbox_File_Save, vxInternalAssets.UI.RibbonSave16);
            var saveAsFile = new vxRibbonButtonControl(FileOpenGroup, vxLocKeys.Sandbox_File_SaveAs,vxInternalAssets.UI.RibbonSaveAs16);


            var importFile = new vxRibbonButtonControl(FileOpenGroup, vxLocKeys.Sandbox_Import, vxInternalAssets.UI.RibbonImport16);
            var exportFile = new vxRibbonButtonControl(FileOpenGroup, vxLocKeys.Sandbox_Export, vxInternalAssets.UI.RibbonExport16);

            openFile.Clicked += Event_OpenFileToolbarItem_Clicked;
            saveFile.Clicked += Event_SaveFileToolbarItem_Clicked;
            saveAsFile.Clicked += Event_SaveAsFileToolbarItem_Clicked;

            importFile.Clicked += ImportFileToolbarItem_Clicked;
            exportFile.Clicked += ExportFileToolbarItem_Clicked;

            var gizmoGroup = new vxRibbonControlGroup(HomeTabPage, "Transforms");
            m_uiGizmoGlobalTransl = new vxRibbonButtonControl(gizmoGroup, vxLocKeys.Sandbox_GlobalTranslation, vxInternalAssets.UI.RibbonGizmoGlobal);
            m_uiGizmoGlobalTransl.IsTogglable = true;
            m_uiGizmoGlobalTransl.ToggleState = true;

            m_uiGizmoLocalTransl = new vxRibbonButtonControl(gizmoGroup, vxLocKeys.Sandbox_LocalTranslation, vxInternalAssets.UI.RibbonGizmoLocal);
            m_uiGizmoLocalTransl.IsTogglable = true;

            m_uiGizmoGlobalTransl.Clicked += delegate
            {
                m_uiGizmoLocalTransl.ToggleState = false;
                vxGizmo.Instance.TransformationType = TransformationType.Global;
            };

            m_uiGizmoLocalTransl.Clicked += delegate
            {
                m_uiGizmoGlobalTransl.ToggleState = false;
                vxGizmo.Instance.TransformationType = TransformationType.Local;
            };


#if __STEAM__
            var steamWorkshopGroup = new vxRibbonControlGroup(HomeTabPage, "Workshop");
            var uploadToSteamWorkshopBtn = new vxRibbonButtonControl(steamWorkshopGroup, vxLocKeys.Sandbox_Upload, vxInternalAssets.UI.UploadIcon, vxEnumButtonSize.Big);
            uploadToSteamWorkshopBtn.Clicked += delegate
            {
                if (vxEngine.Game.IsDemo)
                {
                    vxMessageBox.Show("Demo", "Steamworkshop support is only available in the full-game!");
                }
                else
                {
                    vxSceneManager.AddScene(new vxWorkshopUploadDialog(this));
                }
            };

            var viewSteamWorkshopBtn = new vxRibbonButtonControl(steamWorkshopGroup, vxLocKeys.Sandbox_ViewWorkshop, vxInternalAssets.UI.RibbonCursorGridSnap);
            viewSteamWorkshopBtn.Clicked += delegate
            {
                vxPlatform.Player.OpenURL($"https://steamcommunity.com/app/{vxEngine.Game.AppID}/workshop/");
            };
#endif


            var cursorSnap = new vxRibbonButtonControl(gizmoGroup, vxLocKeys.Sandbox_Grid_Snap, vxInternalAssets.UI.RibbonCursorGridSnap);
            
            cursorSnap.IsTogglable = true;
            cursorSnap.ToggleState = IsGridSnap;
            cursorSnap.Clicked+=delegate {
                IsGridSnap = cursorSnap.ToggleState;
            };


            var settingsGroup = new vxRibbonControlGroup(HomeTabPage, "Settings");
            var showSettings = new vxRibbonButtonControl(settingsGroup, vxLocKeys.Settings, vxInternalAssets.UI.RibbonEngineSettings32, vxEnumButtonSize.Big);
            showSettings.Clicked += delegate
            {
                OnShowSettingsDialog();
            }; ;


            var engineSettings = new vxRibbonButtonControl(settingsGroup, vxLocKeys.Sandbox_EngineSettings, vxInternalAssets.UI.RibbonEngineSettings32);
            engineSettings.Clicked += delegate
            {
                OnShowEngineSettingsDialog();
            }; ;


            var aboutGroup = new vxRibbonControlGroup(HomeTabPage, "About");

            var showAbout = new vxRibbonButtonControl(aboutGroup, vxLocKeys.Sandbox_About, vxInternalAssets.UI.RibbonEngineLogo16);

            showAbout.Clicked += delegate
            {
                OnShowAbout();
            };

            var bugReport = new vxRibbonButtonControl(aboutGroup, vxLocKeys.Sandbox_ReportBug, vxInternalAssets.UI.BugReport);

            bugReport.Clicked += delegate
            {
                OnReportBug();
            };

            var showHelp = new vxRibbonButtonControl(aboutGroup, vxLocKeys.Help, vxInternalAssets.UI.RibbonEngineHelp16);
            showHelp.Clicked += delegate
            {
                OnShowHelp();
            };

            var openDiscord = new vxRibbonButtonControl(aboutGroup, vxLocKeys.Social_Discord, vxInternalAssets.UI.Social_Discord);
            openDiscord.Clicked += delegate
            {
                OnGoToDiscord();
            };
            var openReddit = new vxRibbonButtonControl(aboutGroup, vxLocKeys.Social_Reddit, vxInternalAssets.UI.Social_Reddit);
            openReddit.Clicked += delegate
            {
                OnGoToReddit();
            };




            vxRibbonTabPage ViewTabPage = new vxRibbonTabPage(EditorRibbonControl, "View");

            var cameraGroup = new vxRibbonControlGroup(ViewTabPage, "Camera");

            m_uiCameraTypeDropDown = new vxRibbonDropdownControl(cameraGroup, "Camera Type", CameraEditMode.ToString());

            foreach (var cameraType in vxUtil.GetValues<vxEditorCameraMode>())
            {
                m_uiCameraTypeDropDown.AddItem(cameraType.ToString());
            }

            m_uiCameraTypeDropDown.Dropdown.SelectionChanged += delegate {
                CameraEditMode = (vxEditorCameraMode)m_uiCameraTypeDropDown.Dropdown.SelectedIndex;
                UpdateViewTabItemState();
            };
              
            

            m_uiCameraProjTypeDropDown = new vxRibbonDropdownControl(cameraGroup, "Projection", Cameras[0].ProjectionType.ToString());

            foreach (var cameraProjType in vxUtil.GetValues<vxCameraProjectionType>())
            {
                m_uiCameraProjTypeDropDown.AddItem(cameraProjType.ToString());
            }

            m_uiCameraProjTypeDropDown.Dropdown.SelectionChanged += delegate
            {
                Cameras[0].ProjectionType = (vxCameraProjectionType)m_uiCameraProjTypeDropDown.Dropdown.SelectedIndex;
                UpdateViewTabItemState();
            };




            var sceneVIsualization = new vxRibbonControlGroup(ViewTabPage, "Scene Visualization");

            var cameraRenderType = new vxRibbonDropdownControl(sceneVIsualization, "Scene Debug", Cameras[0].SceneDebugDisplayMode.ToString());

            foreach (var renderType in vxUtil.GetValues<vxEnumSceneDebugMode>())
            {
                cameraRenderType.AddItem(renderType.ToString());
            }

            cameraRenderType.Dropdown.SelectionChanged += delegate {
                foreach(var cam in Cameras)
                {
                    cam.SceneDebugDisplayMode = (vxEnumSceneDebugMode)cameraRenderType.Dropdown.SelectedIndex;
                }
                
                UpdateViewTabItemState();
            };


            vxRibbonTabPage EntitiesTabPage = new vxRibbonTabPage(EditorRibbonControl, "Entities");

            var EntitiesGroup = new vxRibbonControlGroup(EntitiesTabPage, "Entities");

            // Add new entity
            var addItemFile = new vxRibbonButtonControl(EntitiesGroup, vxLocKeys.Sandbox_NewEntity,vxInternalAssets.UI.RibbonAddEntity32, vxEnumButtonSize.Big);
            addItemFile.Clicked += AddEntityToolbarItem_Clicked;

            var deleteEntities = new vxRibbonButtonControl(EntitiesGroup, vxLocKeys.Sandbox_Manage_DeleteEntities, vxInternalAssets.UI.RibbonDeleteEntity16);
            deleteEntities.Clicked += delegate
            {
                DeleteSelectedEntities();
            };

            // manage imported entities
            //#if DEBUG
            var manageImportedEntities = new vxRibbonButtonControl(EntitiesGroup, vxLocKeys.Manage_Imported_Entities, vxInternalAssets.UI.RibbonAddEntity16);
            manageImportedEntities.Clicked += ManageImportedEntities_Clicked;
//#endif

            //var addTerrainEntity = new vxRibbonButtonControl(EntitiesGroup, "New Terrain", vxInternalAssets.Textures.RibbonAddTerrain16);
            //var addWaterEntity = new vxRibbonButtonControl(EntitiesGroup, "New Water", vxInternalAssets.Textures.RibbonAddWater16);

            //addTerrainEntity.Clicked += delegate
            //{
            //    OnNewItemAdded(typeof(vxTerrainChunk).ToString());
            //};

            //addWaterEntity.Clicked += delegate
            //{
            //    OnNewItemAdded(typeof(vxWaterEntity).ToString());
            //};



            var entityEditGroup = new vxRibbonControlGroup(EntitiesTabPage, "Edit Entities");
            var terrainEditStart = new vxRibbonButtonControl(entityEditGroup, "Terrain Editor", vxInternalAssets.UI.RibbonEditTerrain32, vxEnumButtonSize.Big);

            vxRibbonTabPage ToolsTabPage = new vxRibbonTabPage(EditorRibbonControl, vxLocKeys.Sandbox_Debug_Tools);
            var debugTools = new vxRibbonControlGroup(ToolsTabPage, "Debug Tools");

            var fpsToggleBtn = new vxRibbonButtonControl(debugTools, vxLocKeys.Sandbox_Debug_FPSCounter,vxInternalAssets.UI.RibbonDebugFps);
            fpsToggleBtn.IsTogglable = true;
            fpsToggleBtn.Clicked += delegate
            {
                vxDebug.CommandUI.ExecuteCommand(string.Format("fps"));
            };

            var timeRulerToggleBtn = new vxRibbonButtonControl(debugTools, vxLocKeys.Sandbox_Debug_TimeRuler, vxInternalAssets.UI.RibbonDebugGraph);
            timeRulerToggleBtn.IsTogglable = true;
            timeRulerToggleBtn.Clicked += delegate
            {
                vxDebug.CommandUI.ExecuteCommand(string.Format("tr"));
            };


            var consoleToggleBtn = new vxRibbonButtonControl(debugTools, vxLocKeys.Sandbox_Debug_ToggleCommands, vxInternalAssets.UI.RibbonDebugConsole);
            consoleToggleBtn.IsTogglable = true;
            consoleToggleBtn.SetToolTip("This toggles the list of current commands in the sandbox.\nUseful in debugging undo-redo logic.");
            consoleToggleBtn.Clicked += delegate {
                vxDebug.CommandUI.ExecuteCommand(string.Format("cs"));
            };



            PlayTabPage = new vxRibbonTabPage(EditorRibbonControl, "Play");
            var playLevelRbnGroup = new vxRibbonControlGroup(PlayTabPage, "Play Level");
            var playLevelBtn = new vxRibbonButtonControl(playLevelRbnGroup, vxLocKeys.Play, vxInternalAssets.UI.ToggleSandbox, vxEnumButtonSize.Big);

            playLevelBtn.SetToolTip("Play the current level. Hit [ESC] to exit out of play mode.");
            playLevelBtn.Clicked += delegate {
                SimulationStart();
            };


            EditorRibbonControl.Add(HomeTabPage);
            EditorRibbonControl.Add(ViewTabPage);
            EditorRibbonControl.Add(EntitiesTabPage);
            EditorRibbonControl.Add(ToolsTabPage);
            EditorRibbonControl.Add(PlayTabPage);

            OnInitialiseUIEditorRibbonControls();

            OnInitialiseUITerrainToolbar();

            OnInitialiseUIScreenCaptureToolbar();

            OnInitialiseUIEditorRibbonContextTabs();

            terrainEditStart.Clicked += delegate
            {
                if (SandboxEditMode != vxEnumSanboxEditMode.TerrainEdit)
                {
                    SandboxEditMode = vxEnumSanboxEditMode.TerrainEdit;
                    if (terrainTabPage.IsAdded == false)
                    {
                        EditorRibbonControl.AddContextTab(terrainTabPage);
                        terrainTabPage.IsAdded = true;
                    }
                    terrainTabPage.SelectTab();
                }
            };


            HomeTabPage.SelectTab();



            // TOP TOOL BAR

            var fileNew = new vxRibbonToolbarButtonControl(EditorRibbonControl, vxInternalAssets.UI.RibbonNew16);
            var fileOpen = new vxRibbonToolbarButtonControl(EditorRibbonControl, vxInternalAssets.UI.RibbonOpen16);
            var fileSave = new vxRibbonToolbarButtonControl(EditorRibbonControl, vxInternalAssets.UI.RibbonSave16);
            var fileSaveAs = new vxRibbonToolbarButtonControl(EditorRibbonControl, vxInternalAssets.UI.RibbonSaveAs16);

            fileNew.Clicked += Event_NewFileToolbarItem_Clicked;
            fileOpen.Clicked += Event_OpenFileToolbarItem_Clicked;
            fileSave.Clicked += Event_SaveFileToolbarItem_Clicked;
            fileSaveAs.Clicked += Event_SaveAsFileToolbarItem_Clicked;

            new vxRibbonToolbarSplitterControl(EditorRibbonControl);

            undoBtn = new vxRibbonToolbarButtonControl(EditorRibbonControl, vxInternalAssets.UI.RibbonUndo16);
            undoBtn.Clicked += UndoToolbarItem_Clicked;
            undoBtn.SetToolTip("Undo the previous command.");

            redoBtn = new vxRibbonToolbarButtonControl(EditorRibbonControl, vxInternalAssets.UI.RibbonRedo16);
            redoBtn.Clicked += RedoToolbarItem_Clicked;
            redoBtn.SetToolTip("Redo the previous command.");

            EditorRibbonControl.TitleButton.ButtonImage = vxInternalAssets.UI.ToggleSandbox;
            EditorRibbonControl.TitleButton.Clicked += RunGameToolbarItem_Clicked;

            new vxRibbonToolbarSplitterControl(EditorRibbonControl);
            tlbrNewEntity = new vxRibbonToolbarButtonControl(EditorRibbonControl, vxInternalAssets.UI.RibbonAddEntity16);
            tlbrNewEntity.IsTogglable = true;

            tlbrDeleteEntity = new vxRibbonToolbarButtonControl(EditorRibbonControl, vxInternalAssets.UI.RibbonDeleteEntity16);
            tlbrDeleteEntity.IsTogglable = true;

            selcMode = new vxRibbonToolbarButtonControl(EditorRibbonControl, vxInternalAssets.UI.RibbonSelectMode16);
            selcMode.IsTogglable = true;

            selcMode.ToggleState = true;
            SandboxEditMode = vxEnumSanboxEditMode.SelectItem;

            tlbrDeleteEntity.Clicked += delegate
            {
                DeleteSelectedEntities();
            };

            tlbrNewEntity.Clicked += delegate
            {
                tlbrNewEntity.ToggleState = true;
                selcMode.ToggleState = false;
                SandboxEditMode = vxEnumSanboxEditMode.AddItem;
                //vxSceneManager.AddScene(NewSandboxItemDialog);

                m_entitySlideTab.Open();
            };

            selcMode.Clicked += delegate
            {
                tlbrNewEntity.ToggleState = false;
                selcMode.ToggleState = true;
                SandboxEditMode = vxEnumSanboxEditMode.SelectItem;
                DisposeOfTempPart();
            }; ;



            //SetUndoRedoButtonStatus();
            CommandManager.OnChange += CommandManager_OnChange;

            undoBtn.IsEnabled = CommandManager.CanUndo;
            redoBtn.IsEnabled = CommandManager.CanRedo;
        }

        /// <summary>
        /// Initialises Editor Ribbon Controls. Override this to implement your own game specific ribbon controls.
        /// </summary>
        protected virtual void OnInitialiseUIEditorRibbonControls() { }

        /// <summary>
        /// Called when initialising Editor Ribbon Context Tabs, such as tabs which are only visible during certain situations or contexts (Like Terrain Editing)
        /// </summary>
        protected virtual void OnInitialiseUIEditorRibbonContextTabs() { }

        void CommandManager_OnChange(object sender, EventArgs e)
        {
            undoBtn.IsEnabled = CommandManager.CanUndo;
            redoBtn.IsEnabled = CommandManager.CanRedo;
            
            // also, update the properties UI
            RefreshEntityProptiesUI();
        }

        void UndoToolbarItem_Clicked(object sender, UI.Events.vxUIControlClickEventArgs e)
        {
            CommandManager.Undo();
        }

        void RedoToolbarItem_Clicked(object sender, UI.Events.vxUIControlClickEventArgs e)
        {
            CommandManager.ReDo();
        }


        /// <summary>
        /// Event Fired too test the Game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void RunGameToolbarItem_Clicked(object sender, vxUIControlClickEventArgs e)
        {
            SimulationStart();
        }

        /// <summary>
        /// Event Fired too stop the test of the Game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void StopGameToolbarItem_Clicked(object sender, vxUIControlClickEventArgs e)
        {
            SimulationStop();
        }


        private void AddEntityToolbarItem_Clicked(object sender, vxUIControlClickEventArgs e)
        {
            if (SandboxEditMode == vxEnumSanboxEditMode.TerrainEdit)
            {
                CloseTerrainEditor();
            }

            m_entitySlideTab.Open();
        }

        private void ManageImportedEntities_Clicked(object sender, vxUIControlClickEventArgs e)
        {
            var manageImportedEntitiesDialog = new vxManageImportedEntitiesDialog(this);
            vxSceneManager.AddScene(manageImportedEntitiesDialog);
        }

        public override void ShowPauseScreen()
        {
            // are we in sandbox mode?
            if (SandboxCurrentState == vxEnumSandboxStatus.EditMode)
            {
                if (SandboxEditMode == vxEnumSanboxEditMode.AddItem)
                {
                    SandboxEditMode = vxEnumSanboxEditMode.SelectItem;
                }
                else if(vxGizmo.Instance.IsInGrabMode)
                {
                    // do nothing
                }
                else
                {
                    vxSceneManager.AddScene(new vxPauseMenuScreen(), ControllingPlayer);
                }
            }
            else
            {
                vxSceneManager.AddScene(new vxPauseMenuScreen(), ControllingPlayer);
            }
        }

        #endregion

    }
}