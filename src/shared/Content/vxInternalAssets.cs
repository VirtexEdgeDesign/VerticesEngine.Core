
using System;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.ContentManagement;
using VerticesEngine.Graphics;
using VerticesEngine.UI.MessageBoxs;
using VerticesEngine.Utilities;

namespace VerticesEngine
{
    /// <summary>
    /// Class for holding all Internal Engine Assets
    /// </summary>
    public static class vxInternalAssets
    {
        public static class Fonts
        {
            public static SpriteFont MenuTitleFont;
            public static SpriteFont MenuFont;
            public static SpriteFont BaseFont { get; internal set; }
            public static SpriteFont DebugFont;
            public static SpriteFont ViewerFont;
        };

        public static class Textures
        {
            public static Texture2D Blank;
            public static Texture2D Gradient;
            public static Texture2D RandomValues;
            public static Texture2D Arrow_Left, Arrow_Right;

            public static Texture2D WaterBumpMap, WaterDistortionMap;

            public static Texture2D Texture_WaterEnvr;
            public static Texture2D Texture_WaterWaves;
            public static Texture2D Texture_WaterDistort;

            public static Texture2D Texture_Cube_Null;

            /// <summary>
            /// A texture which represents a missing and/or invalid texture.
            /// </summary>
            public static Texture2D ErrorTexture;

            public static Texture2D DefaultDiffuse;
            public static Texture2D DefaultNormalMap;
            public static Texture2D DefaultSurfaceMap;

            public static Texture2D Texture_ArrowDown;
            public static Texture2D Texture_ArrowRight;

            public static Texture2D TreeMesh;
            public static Texture2D TreeModel, TreeRoot;


            public static Texture2D Texture_Sun_Glow;
            public static Texture2D LensFlareGlow;
        }
       public static class UI
       {
                // -- ribbon textures --
                public static Texture2D VirtexLogo;

            public static Texture2D ToggleSandbox;

            public static Texture2D ComboBoxArrowDown;

            public static Texture2D PropertyControlIcon;
            public static Texture2D PropertyBulletDown;
            public static Texture2D PropertyBulletRight;

            public static Texture2D PropertyBulletPlus;
            public static Texture2D PropertyBulletMinus;
            public static Texture2D PropertyCheckedBoxUnchecked;
            public static Texture2D PropertyCheckedBoxChecked;
            public static Texture2D PropertyCheckedBoxMixed;

            public static Texture2D RibbonGradientVertical;

            public static Texture2D RibbonNew16;
            public static Texture2D RibbonOpen16;
            public static Texture2D RibbonOpen32;
            public static Texture2D RibbonSave16;
            public static Texture2D RibbonSaveAs16;

            public static Texture2D RibbonUndo16;
            public static Texture2D RibbonRedo16;

            public static Texture2D RibbonImport16;
            public static Texture2D RibbonExport16;

            public static Texture2D RibbonGizmoGlobal;
            public static Texture2D RibbonGizmoLocal;

            public static Texture2D RibbonCursorGridSnap;


            public static Texture2D RibbonEngineSettings32;
            public static Texture2D RibbonEngineHelp16;
            public static Texture2D RibbonEngineLogo16;

            public static Texture2D RibbonSelectMode16;
            public static Texture2D RibbonAddEntity16;
            public static Texture2D RibbonAddEntity32;
            public static Texture2D RibbonDeleteEntity16;
            public static Texture2D RibbonAddTerrain16;
            public static Texture2D RibbonAddWater16;

            // - terrain -
            public static Texture2D RibbonEditTerrain32;

            public static Texture2D RibbonDebugFps;
            public static Texture2D RibbonDebugGraph;
            public static Texture2D RibbonDebugConsole;

            public static Texture2D UploadIcon;
            public static Texture2D BugReport;
            public static Texture2D Social_Discord;
            public static Texture2D Social_Reddit;

            public static Texture2D ContextMenuFrameView;
            public static Texture2D ContextMenuOrbitSelection;

            public static Texture2D Terrain_Mode_Sculpt;
            public static Texture2D Terrain_Sculpt_Delta;
            public static Texture2D Terrain_Sculpt_Avg;
            public static Texture2D Terrain_Edit_Smooth;
            public static Texture2D Terrain_Edit_Linear;
            public static Texture2D Terrain_Edit_Flat;

            public static Texture2D Terrain_Edit_Exit;
        };

        public static class SandboxUI
        {
            public static Texture2D TabbedPinOff;
            public static Texture2D TabbedPinOn;
        };

            public static class SoundEffects
        {
            public static SoundEffect MenuClick;
            public static SoundEffect MenuConfirm;
            public static SoundEffect MenuError;
        };

        /// <summary>
        /// Internal Engine Sound Effects.
        /// </summary>
        //public AssetSoundEffects SoundEffects = new AssetSoundEffects();



        public static class Models
        {
            public static vxMesh UnitArrow;
            public static vxMesh UnitTorus;
            public static vxMesh UnitBox;
            public static vxMesh UnitCylinder;
            public static vxMesh UnitSphere;
            public static vxMesh UnitPlane;
            public static vxMesh UnitPlanePan;

            /// <summary>
            ///  The Sphere used for Point Lights. Note this is a simple Model, not a vxModel.
            /// </summary>
            public static Model PointLightSphere;
            public static vxMesh WaterPlane;

            //public vxModel Sun_Mask;

            // The Model Viewer Plane
            public static vxMesh ViewerPlane;
        };

        /// <summary>
        /// The models.
        /// </summary>
        //public AssetModels Models = new AssetModels();




        public static class Shaders
        {
            public static Effect MainShader;
            public static Effect CascadeShadowShader;
            public static Effect PrepPassShader;
            public static Effect SkyboxShader;
            public static Effect DebugShader;
            public static Effect EditorTempEntityShader;
            public static Effect IndexEncodedColourShader;
            public static Effect OutlineShader;
            public static Effect CartoonShader;
            //public Effect DistortionShader;
            public static Effect WaterReflectionShader;
            public static Effect HeightMapTerrainShader;

            public static Effect EditorControlShader;
        };

        /// <summary>
        /// Model Shaders.
        /// </summary>
        //public AssetShaders Shaders = new AssetShaders();







        public static class PostProcessShaders
        {

            //Deffered Rendering
            public static Effect DrfrdRndrClearGBuffer;
            public static Effect DrfrdRndrCombineFinal;
            public static Effect LightingCombine;
            public static Effect DrfrdRndrDirectionalLight;
            public static Effect DrfrdRndrPointLight;

            public static Effect BloomExtractEffect;
            public static Effect BloomCombineEffect;
            public static Effect GaussianBlurEffect;

            public static Effect SceneBlurEffect;

            public static Effect CartoonEdgeDetect;
            public static Effect SelectionEdgeDetection;
            public static Effect FogPostProcessShader;

            /// <summary>
            /// Handles the sun lighting effects such as lens flare
            /// and god rays
            /// </summary>
            public static Effect SunLightingPostProcessShader;

            /// <summary>
            /// An Effect Which applies Post Processes once the scene has been lit in the
            /// defferred renderer. 
            /// The Post Processes are SSR and God Rays.
            /// </summary>
            public static Effect ScreenSpaceCombine;

            /// <summary>
            /// This Effect Draws the Lights to be masked for the Post Lighting Effect
            /// </summary>
            public static Effect LensFlareEffect;
            public static Effect SunGodRaysEffect;
            public static Effect MaskedSunEffect;

            public static Effect SSAOEffect;

            /// <summary>
            /// This Post Process preforms a Screen Space Reflection on the Scene given the Depth and Normal Maps.
            /// It creates a UV Map of the location of the reflected pixels.
            /// </summary>
            public static Effect ScreenSpaceReflectionEffect;

            public static Effect DistortSceneEffect;

            public static Effect CameraMotionBlurEffect;

            public static Effect FXAA;
        };

		/// <summary>
		/// Gets the path for the internal assets and content for the engine.
		/// </summary>
		/// <value>The content of the path to engine.</value>
        internal static string PathToEngineContent
		{
			get { return "EngineContent"; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:VerticesEngine.ContentManagement.vxInternalAssetManager"/> class.
		/// </summary>
		/// <param name="engine">Engine.</param>
		internal static void Init()
		{
            /********************************************************************************************/
            /*										Fonts												*/
            /********************************************************************************************/
            Fonts.MenuTitleFont = vxContentManager.Instance.Load<SpriteFont>("vxengine/fonts/font_gui_title");
            Fonts.MenuFont = vxContentManager.Instance.Load<SpriteFont>("vxengine/fonts/font_gui");
            Fonts.BaseFont = vxContentManager.Instance.Load<SpriteFont>("vxengine/fonts/font_gui");
            Fonts.DebugFont = vxContentManager.Instance.Load<SpriteFont>("vxengine/fonts/font_debug");
            Fonts.ViewerFont = vxContentManager.Instance.Load<SpriteFont>("vxengine/fonts/font_viewer");


            /********************************************************************************************/
            /*										Textures											*/
            /********************************************************************************************/
            
            Textures.Blank = vxContentManager.Instance.Load<Texture2D>(vxEngineAssetsPaths.TEXTURES_DEFAULTS_BLANK_PNG);
            Textures.Gradient = vxContentManager.Instance.Load<Texture2D>(vxEngineAssetsPaths.TEXTURES_DEFAULTS_GRADIENT_PNG);
            Textures.RandomValues = vxContentManager.Instance.Load<Texture2D>(vxEngineAssetsPaths.TEXTURES_DEFAULTS_RANDOM_PNG);

            Textures.DefaultDiffuse = vxContentManager.Instance.Load<Texture2D>(vxEngineAssetsPaths.TEXTURES_DEFAULTS_MODEL_DIFFUSE_PNG);
            Textures.ErrorTexture = vxContentManager.Instance.Load<Texture2D>(vxEngineAssetsPaths.TEXTURES_DEFAULTS_MODEL_DIFFUSE_PNG);
            Textures.DefaultNormalMap = vxContentManager.Instance.Load<Texture2D>(vxEngineAssetsPaths.TEXTURES_DEFAULTS_MODEL_NORMALMAP_PNG);
            Textures.DefaultSurfaceMap = vxContentManager.Instance.Load<Texture2D>(vxEngineAssetsPaths.TEXTURES_DEFAULTS_MODEL_SURFACEMAP_PNG);


            // Handle GUI Items
            Textures.Arrow_Left = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/gui/Slider/Arrow_Left");
            Textures.Arrow_Right = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/gui/Slider/Arrow_Right");
            
            // water textures
            Textures.WaterBumpMap = vxContentManager.Instance.Load<Texture2D>("vxengine/models/water/waterbump");
            Textures.WaterDistortionMap = vxContentManager.Instance.Load<Texture2D>("vxengine/models/water/water_plane_dm");

            Textures.Texture_ArrowDown = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/gui/icons/arrow_down");
            Textures.Texture_ArrowRight = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/gui/icons/arrow_right");

            Textures.Texture_Sun_Glow = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/godrays/rays0");
            Textures.LensFlareGlow = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/lensflares/glow");

            // -- ribbon icons --
            UI.VirtexLogo = vxContentManager.Instance.Load<Texture2D>(vxEngineAssetsPaths.MODELS_TITLESCREEN_LOGO_VRTX_VRTX_BTN_PNG);

            UI.ComboBoxArrowDown = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/misc/bullet_arrow_down");

            UI.ToggleSandbox = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/rbn/main/test_32");

            UI.PropertyControlIcon = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/gui/icons/properties");
            UI.PropertyBulletDown = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/misc/bullet_arrow_down");
            UI.PropertyBulletRight = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/misc/bullet_arrow_right");
            UI.PropertyBulletPlus = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/misc/bullet_toggle_plus");
            UI.PropertyBulletMinus = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/misc/bullet_toggle_minus");
            UI.PropertyCheckedBoxChecked = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/misc/check_box");
            UI.PropertyCheckedBoxMixed = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/misc/check_box_mixed");
            UI.PropertyCheckedBoxUnchecked = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/misc/check_box_uncheck");

            UI.RibbonGradientVertical = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/Defaults/gradient_vertical");

            UI.RibbonNew16 = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/rbn/main/new_16");
            UI.RibbonOpen16 = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/rbn/main/open_16");
            UI.RibbonOpen32 = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/rbn/main/open_32");
            UI.RibbonSave16 = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/rbn/main/save_16");
            UI.RibbonSaveAs16 = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/rbn/main/save_as_16");

            UI.RibbonUndo16 = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/rbn/main/undo_16");
            UI.RibbonRedo16 = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/rbn/main/redo_16");

            UI.RibbonImport16 = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/rbn/main/import_16");
            UI.RibbonExport16 = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/rbn/main/export_16");

            UI.RibbonGizmoGlobal = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/rbn/gimbal/transform/gimbal_global");
            UI.RibbonGizmoLocal = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/rbn/gimbal/transform/gimbal_local");

            UI.RibbonCursorGridSnap = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/gui/icons/cursor_grid");

            UI.RibbonEngineSettings32 = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/rbn/main/setting_32");
            UI.RibbonEngineHelp16 = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/rbn/main/help_16");
            UI.RibbonEngineLogo16 = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/rbn/logo_16");

            UI.RibbonSelectMode16 = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/rbn/main/select");
            UI.RibbonAddEntity16 = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/rbn/entities/entity_add_16");
            UI.RibbonAddEntity32 = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/rbn/entities/entity_add_32");
            UI.RibbonDeleteEntity16 = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/rbn/entities/entity_delete_16");
            UI.RibbonAddTerrain16 = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/rbn/entities/terrain_add_16");
            UI.RibbonAddWater16 = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/rbn/entities/water_add_16");

            UI.RibbonEditTerrain32 = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/rbn/terrain/terrain_edit");

            UI.RibbonDebugFps = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/gui/icons/debug_fbs");
            UI.RibbonDebugGraph = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/gui/icons/debug_graph");
            UI.RibbonDebugConsole = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/gui/icons/debug_console");
            
            UI.ContextMenuFrameView = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/gui/icons/centroid");
            UI.ContextMenuOrbitSelection = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/gui/icons/centroid");



            UI.Terrain_Mode_Sculpt = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/rbn/terrain/mode_sulpt");
            UI.Terrain_Sculpt_Delta = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/rbn/terrain/sculpt_delta");
            UI.Terrain_Sculpt_Avg = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/rbn/terrain/sculpt_avg");
            UI.Terrain_Edit_Smooth = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/rbn/terrain/edit_smooth");
            UI.Terrain_Edit_Linear = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/rbn/terrain/edit_linear");
            UI.Terrain_Edit_Flat = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/rbn/terrain/edit_flat");
            UI.Terrain_Edit_Exit = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/rbn/terrain/exit");


            SandboxUI.TabbedPinOff = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/misc/pin_unchecked");
            SandboxUI.TabbedPinOn = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/misc/pin_checked");

            /********************************************************************************************/
            /*										Sound Effects  										*/
            /********************************************************************************************/
            SoundEffects.MenuClick = vxContentManager.Instance.Load<SoundEffect>("vxengine/sndFx/Menu/click");
            SoundEffects.MenuConfirm = vxContentManager.Instance.Load<SoundEffect>("vxengine/sndFx/Menu/confirm");
            SoundEffects.MenuError = vxContentManager.Instance.Load<SoundEffect>("vxengine/sndFx/Menu/error");



            /********************************************************************************************/
            /*										Shaders												*/
            /********************************************************************************************/

            // Certain shaders need modifications for MonoGame, so there is an alternate version of them

            //Shader Collection
            Shaders.MainShader = vxContentManager.Instance.Load<Effect>("vxengine/shaders/ModelShaders/MainModelShader");
            Shaders.CartoonShader = vxContentManager.Instance.Load<Effect>("vxengine/shaders/ModelShaders/CellModelShader");
            Shaders.CascadeShadowShader = vxContentManager.Instance.Load<Effect>("vxengine/shaders/Shadows/CascadeShadowShader");
            Shaders.PrepPassShader = vxContentManager.Instance.Load<Effect>("vxengine/shaders/Utility/PrepPassShader");
            Shaders.DebugShader = vxContentManager.Instance.Load<Effect>("vxengine/shaders/Utility/DebugShader");
            Shaders.EditorTempEntityShader = vxContentManager.Instance.Load<Effect>("vxengine/shaders/Utility/EditorTempEntityShader");
            vxGraphics.Util.WireframeShader = new vxDebugEffect(Shaders.DebugShader);
            vxGraphics.Util.EditorTempEntityShader = new vxEditorTempEntityEffect(Shaders.EditorTempEntityShader);

            Shaders.IndexEncodedColourShader = vxContentManager.Instance.Load<Effect>("vxengine/shaders/Utility/IndexEncodedColourShader");
            Shaders.OutlineShader = vxContentManager.Instance.Load<Effect>("vxengine/shaders/Utility/OutlineShader");
            //vxRenderPipeline3D.ShadowEffect = new vxShadowEffect(Shaders.CascadeShadowShader);

            //Water Shader
            Shaders.WaterReflectionShader = vxContentManager.Instance.Load<Effect>("vxengine/shaders/Water/WaterShader");

            // Height Map Terrain Shader
            Shaders.HeightMapTerrainShader = vxContentManager.Instance.Load<Effect>("vxengine/shaders/Terrain/TerrainShader");

            Shaders.WaterReflectionShader = vxContentManager.Instance.Load<Effect>("vxengine/shaders/Water/WaterShader");
            Shaders.PrepPassShader = vxContentManager.Instance.Load<Effect>("vxengine/shaders/Utility/PrepPassShader");
            Shaders.SkyboxShader = vxContentManager.Instance.Load<Effect>("vxengine/shaders/Skybox/SkyBoxShader");
            Shaders.HeightMapTerrainShader = vxContentManager.Instance.Load<Effect>("vxengine/shaders/Terrain/TerrainShader");
            PostProcessShaders.CartoonEdgeDetect = vxContentManager.Instance.Load<Effect>("vxengine/shaders/EdgeDetection/CartoonEdgeDetection");
            PostProcessShaders.FogPostProcessShader = vxContentManager.Instance.Load<Effect>("vxengine/shaders/enviroment/FogPostProcess");
            Shaders.EditorControlShader = vxContentManager.Instance.Load<Effect>("vxengine/shaders/Utility/EditorControlShader");


            //Bloom
            PostProcessShaders.BloomExtractEffect = vxContentManager.Instance.Load<Effect>("vxengine/shaders/Bloom/BloomExtract");
            PostProcessShaders.BloomCombineEffect = vxContentManager.Instance.Load<Effect>("vxengine/shaders/Bloom/BloomCombine");
            PostProcessShaders.GaussianBlurEffect = vxContentManager.Instance.Load<Effect>("vxengine/shaders/Bloom/GaussianBlur");

            PostProcessShaders.CartoonEdgeDetect = vxContentManager.Instance.Load<Effect>("vxengine/shaders/EdgeDetection/CartoonEdgeDetection");
            PostProcessShaders.SelectionEdgeDetection = vxContentManager.Instance.Load<Effect>("vxengine/shaders/EdgeDetection/SelectionEdgeDetection");
            
            //Distortion Shaders
            PostProcessShaders.DistortSceneEffect = vxContentManager.Instance.Load<Effect>("vxengine/shaders/Distorter/DistortScene");
            PostProcessShaders.CameraMotionBlurEffect = vxContentManager.Instance.Load<Effect>("vxengine/shaders/Blur/CameraMotionBlur");

            PostProcessShaders.SceneBlurEffect = vxContentManager.Instance.Load<Effect>("vxengine/shaders/Blur/SceneBlur");

            // Anti Aliasing Shaders
            PostProcessShaders.FXAA = vxContentManager.Instance.Load<Effect>("vxengine/shaders/AntiAliasing/FXAA/FXAA");


            //Defferred Shading
            PostProcessShaders.DrfrdRndrClearGBuffer = vxContentManager.Instance.Load<Effect>("vxengine/shaders/Lighting/ClearGBuffer");
            PostProcessShaders.DrfrdRndrCombineFinal = vxContentManager.Instance.Load<Effect>("vxengine/shaders/Lighting/CombineFinal");
            //PostProcessShaders.LightingCombine = vxContentManager.Instance.Load<Effect>("Shaders/Lighting/LightingCombinePass");
            PostProcessShaders.LightingCombine = vxContentManager.Instance.Load<Effect>("vxengine/shaders/Lighting/LightingCombinePass");
            PostProcessShaders.DrfrdRndrDirectionalLight = vxContentManager.Instance.Load<Effect>("vxengine/shaders/Lighting/DirectionalLight");
            PostProcessShaders.DrfrdRndrPointLight = vxContentManager.Instance.Load<Effect>("vxengine/shaders/Lighting/PointLight");
            PostProcessShaders.MaskedSunEffect = vxContentManager.Instance.Load<Effect>("vxengine/shaders/Lighting/MaskedSun");
            PostProcessShaders.SunGodRaysEffect = vxContentManager.Instance.Load<Effect>("vxengine/shaders/enviroment/GodRays");
            PostProcessShaders.LensFlareEffect = vxContentManager.Instance.Load<Effect>("vxengine/shaders/enviroment/LensFlare");

            // Screen Space Reflections
            PostProcessShaders.SSAOEffect = vxContentManager.Instance.Load<Effect>("vxengine/shaders/ScreenSpace/SSAO");
            PostProcessShaders.ScreenSpaceReflectionEffect = vxContentManager.Instance.Load<Effect>("vxengine/shaders/ScreenSpace/SSR");
            PostProcessShaders.ScreenSpaceCombine = vxContentManager.Instance.Load<Effect>("vxengine/shaders/ScreenSpace/ScreenSpaceCombine");

            /********************************************************************************************/
/*										Models												*/
/********************************************************************************************/
//Unit Models
            var UnitArrow = vxContentManager.Instance.LoadMesh("vxengine/models/utils/unit_arrow/unit_arrow");
            Models.UnitArrow = vxContentManager.Instance.LoadMesh("vxengine/models/utils/unit_arrow/unit_arrow");
            Models.UnitTorus = vxContentManager.Instance.LoadMesh("vxengine/models/utils/unit_torus/unit_torus");
            Models.UnitPlanePan = vxContentManager.Instance.LoadMesh("vxengine/models/utils/unit_plane/unit_plane_pan");

			Models.UnitBox = vxContentManager.Instance.LoadMesh("vxengine/models/utils/unit_box/unit_box");
            Models.UnitCylinder = vxContentManager.Instance.LoadMesh("vxengine/models/utils/unit_cylinder/unit_cylinder");
			Models.UnitPlane = vxContentManager.Instance.LoadMesh("vxengine/models/utils/unit_plane/unit_plane");
			Models.UnitSphere = vxContentManager.Instance.LoadMesh("vxengine/models/utils/unit_sphere/unit_sphere");
			Models.WaterPlane = vxContentManager.Instance.LoadMesh("vxengine/models/water/water_plane");
			Models.ViewerPlane = vxContentManager.Instance.LoadMesh("vxengine/models/viewer/plane");
            //Models.PointLightSphere = vxContentManager.Instance.Load<Model>("vxengine/models/lghtng/unit_sphere");
        }
        

		/// <summary>
		/// Asset paths for vxengine used in VerticesEngine.AppDemo
		/// </summary>
		public static class vxEngineAssetsPaths
		{
			public const string MODELS_SUN_SUN_MASK_XNB="vxengine/models/sun/sun_mask";
			public const string MODELS_SUN_SUN_0_XNB="vxengine/models/sun/Sun_0";
			public const string FONTS_FONT_SPLASH_SPRITEFONT="vxengine/fonts/font_splash";
			public const string FONTS_FONT_DEBUG_SPRITEFONT="vxengine/fonts/font_debug";
			public const string FONTS_FONT_GUI_TITLE_SPRITEFONT="vxengine/fonts/font_gui_title";
			public const string FONTS_FONT_GUI_SPRITEFONT="vxengine/fonts/font_gui";
			public const string FONTS_FONT_VIEWER_SPRITEFONT="vxengine/fonts/font_viewer";
			public const string SHADERS_WATER_WATER_PLANE_FBX="vxengine/shaders/Water/Water_Plane";
			public const string SHADERS_WATER_WATER_PLANE_MG_FBX="vxengine/shaders/Water/Water_Plane_mg";
			public const string SHADERS_SKYBOX_CUBE_NORMALS_OUT_FBX="vxengine/shaders/Skybox/cube_normals_out";
			public const string SHADERS_SKYBOX_CUBE_FBX="vxengine/shaders/Skybox/cube";
			public const string SHADERS_SKYBOX_CUBE_FLIPPEDNORMALS_FBX="vxengine/shaders/Skybox/cube_flippednormals";
			public const string MODELS_VIEWER_PLANE_FBX="vxengine/models/viewer/plane";
			public const string MODELS_WATER_WATER_PLANE_FBX="vxengine/models/water/water_plane";
			public const string MODELS_SUN_SUN_FBX="vxengine/models/sun/sun";
			public const string MODELS_SUN_SUN_MASK_MG_FBX="vxengine/models/sun/sun_mask_mg";
			public const string MODELS_UTILS_UNIT_PLANE_UNIT_PLANE_PAN_FBX="vxengine/models/utils/unit_plane/unit_plane_pan";
			public const string MODELS_UTILS_UNIT_PLANE_UNIT_PLANE_FBX="vxengine/models/utils/unit_plane/unit_plane";
			public const string MODELS_UTILS_UNIT_PLANE_UNIT_PLANE_MG_FBX="vxengine/models/utils/unit_plane/unit_plane_mg";
			public const string MODELS_UTILS_UNIT_BOX_UNIT_BOX_FBX="vxengine/models/utils/unit_box/unit_box";
			public const string MODELS_UTILS_UNIT_TORUS_UNIT_TORUS_FBX="vxengine/models/utils/unit_torus/unit_torus";
			public const string MODELS_UTILS_UNIT_ARROW_UNIT_ARROW_FBX="vxengine/models/utils/unit_arrow/unit_arrow";
			public const string MODELS_UTILS_UNIT_CYLINDER_UNIT_CYLINDER_FBX="vxengine/models/utils/unit_cylinder/unit_cylinder";
			public const string MODELS_UTILS_UNIT_SPHERE_UNIT_SPHERE_FBX="vxengine/models/utils/unit_sphere/unit_sphere";
			public const string MODELS_UTILS_UNIT_SPHERE_UNIT_SPHERE_MG_FBX="vxengine/models/utils/unit_sphere/unit_sphere_mg";
			public const string MODELS_UTILS_UNIT_PAN_PLANE_UNIT_PAN_PLANE_FBX="vxengine/models/utils/unit_pan_plane/unit_pan_plane";
			public const string MODELS_VEHICLE_WHEEL_CARWHEEL_SPHERE_FBX="vxengine/models/Vehicle/Wheel/carWheel_sphere";
			public const string SHADERS_WATER_WATERSHADER_FX="vxengine/shaders/Water/WaterShader";
			public const string SHADERS_DISTORTER_DISTORTSCENE_FX="vxengine/shaders/Distorter/DistortScene";
			public const string SHADERS_BLOOM_GAUSSIANBLUR_FX="vxengine/shaders/Bloom/GaussianBlur";
			public const string SHADERS_BLOOM_BLOOMEXTRACT_FX="vxengine/shaders/Bloom/BloomExtract";
			public const string SHADERS_BLOOM_BLOOMCOMBINE_FX="vxengine/shaders/Bloom/BloomCombine";
			public const string SHADERS_GODRAYS_MASKEDSUN_FX="vxengine/shaders/GodRays/MaskedSun";
			public const string SHADERS_SCREENSPACE_SSAO_FX="vxengine/shaders/ScreenSpace/SSAO";
			public const string SHADERS_SCREENSPACE_SSR_FX="vxengine/shaders/ScreenSpace/SSR";
			public const string SHADERS_SCREENSPACE_SCREENSPACECOMBINE_FX="vxengine/shaders/ScreenSpace/ScreenSpaceCombine";
			public const string SHADERS_SKYBOX_SKYBOXSHADER_FLATSKY_FX="vxengine/shaders/Skybox/SkyBoxShader_flatsky";
			public const string SHADERS_SKYBOX_SKYBOXSHADER_CUBE_FX="vxengine/shaders/Skybox/SkyBoxShader_cube";
			public const string SHADERS_SKYBOX_SKYBOXSHADER_FX="vxengine/shaders/Skybox/SkyBoxShader";
			public const string SHADERS_MODELSHADERS_MAINMODELSHADER_FX="vxengine/shaders/ModelShaders/MainModelShader";
			public const string SHADERS_MODELSHADERS_CELLMODELSHADER_FX="vxengine/shaders/ModelShaders/CellModelShader";
			public const string SHADERS_SHADOWS_CASCADESHADOWSHADER_FX="vxengine/shaders/Shadows/CascadeShadowShader";
			public const string SHADERS_UTILITY_EDITORCONTROLSHADER_FX="vxengine/shaders/Utility/EditorControlShader";
			public const string SHADERS_UTILITY_EDITORTEMPENTITYSHADER_FX="vxengine/shaders/Utility/EditorTempEntityShader";
			public const string SHADERS_UTILITY_OUTLINESHADER_FX="vxengine/shaders/Utility/OutlineShader";
			public const string SHADERS_UTILITY_INDEXENCODEDCOLOURSHADER_FX="vxengine/shaders/Utility/IndexEncodedColourShader";
			public const string SHADERS_UTILITY_DEBUGSHADER_FX="vxengine/shaders/Utility/DebugShader";
			public const string SHADERS_UTILITY_PREPPASSSHADER_FX="vxengine/shaders/Utility/PrepPassShader";
			public const string SHADERS_LIGHTING_MASKEDSUN_FX="vxengine/shaders/Lighting/MaskedSun";
			public const string SHADERS_LIGHTING_POINTLIGHT_FX="vxengine/shaders/Lighting/PointLight";
			public const string SHADERS_LIGHTING_LIGHTINGCOMBINEPASS_FX="vxengine/shaders/Lighting/LightingCombinePass";
			public const string SHADERS_LIGHTING_DIRECTIONALLIGHT_FX="vxengine/shaders/Lighting/DirectionalLight";
			public const string SHADERS_LIGHTING_CLEARGBUFFER_FX="vxengine/shaders/Lighting/ClearGBuffer";
			public const string SHADERS_LIGHTING_COMBINEFINAL_FX="vxengine/shaders/Lighting/CombineFinal";
			public const string SHADERS_EDGEDETECTION_CARTOONEDGEDETECTION_FX="vxengine/shaders/EdgeDetection/CartoonEdgeDetection";
			public const string SHADERS_EDGEDETECTION_SELECTIONEDGEDETECTION_FX="vxengine/shaders/EdgeDetection/SelectionEdgeDetection";
			public const string SHADERS_TERRAIN_TERRAINSHADER_FX="vxengine/shaders/Terrain/TerrainShader";
			public const string SHADERS_BLUR_CAMERAMOTIONBLUR_FX="vxengine/shaders/Blur/CameraMotionBlur";
			public const string SHADERS_BLUR_SCENEBLUR_FX="vxengine/shaders/Blur/SceneBlur";
			public const string SHADERS_ENVIROMENT_GODRAYS_FX="vxengine/shaders/enviroment/GodRays";
			public const string SHADERS_ENVIROMENT_LENSFLARE_FX="vxengine/shaders/enviroment/LensFlare";
			public const string SHADERS_ENVIROMENT_FOGPOSTPROCESS_FX="vxengine/shaders/enviroment/FogPostProcess";
			public const string SHADERS_ENVIROMENT_FOGPOSTPROCESS_COPY_FX="vxengine/shaders/enviroment/FogPostProcess copy";
			public const string SHADERS_ANTIALIASING_FXAA_FXAA_FX="vxengine/shaders/AntiAliasing/FXAA/FXAA";
			public const string MODELS_VIEWER_J_PNG="vxengine/models/viewer/j";
			public const string MODELS_VIEWER_PLANE_DDS_PNG="vxengine/models/viewer/plane_dds";
			public const string MODELS_WATER_WATERBUMP_PNG="vxengine/models/water/waterbump";
			public const string MODELS_WATER_WATERBUMP_JPG="vxengine/models/water/waterbump";
			public const string MODELS_WATER_WATER_PLANE_DM_PNG="vxengine/models/water/water_plane_dm";
			public const string MODELS_WATER_WATER_PLANE_RM_PNG="vxengine/models/water/water_plane_rm";
			public const string MODELS_WATER_WATER_PLANE_ICON_PNG="vxengine/models/water/water_plane_ICON";
			public const string MODELS_WATER_WATER_PLANE_NM_PNG="vxengine/models/water/water_plane_nm";
			public const string MODELS_WATER_WATER_PLANE_DDS_PNG="vxengine/models/water/water_plane_dds";
			public const string MODELS_SUN_RAYS_PNG="vxengine/models/sun/Rays";
			public const string MODELS_SUN_SUN_PNG="vxengine/models/sun/Sun";
			public const string MODELS_UTILS_UNIT_PLANE_TEXTURE_PNG="vxengine/models/utils/unit_plane/texture";
			public const string MODELS_UTILS_UNIT_PLANE_IMAGE_PNG="vxengine/models/utils/unit_plane/image";
			public const string MODELS_UTILS_UNIT_BOX_TEXTURE_UNITBOX_PNG="vxengine/models/utils/unit_box/texture_unitbox";
			public const string MODELS_UTILS_UNIT_TORUS_TEXTURE_PNG="vxengine/models/utils/unit_torus/texture";
			public const string MODELS_UTILS_UNIT_ARROW_TEXTURE_ARROW_PNG="vxengine/models/utils/unit_arrow/texture_arrow";
			public const string MODELS_UTILS_UNIT_CYLINDER_IMAGE_PNG="vxengine/models/utils/unit_cylinder/image";
			public const string MODELS_UTILS_UNIT_SPHERE_TEXTURE_UNITBOX_PNG="vxengine/models/utils/unit_sphere/texture_unitbox";
			public const string MODELS_UTILS_UNIT_PAN_PLANE_TEXTURE_PNG="vxengine/models/utils/unit_pan_plane/texture";
			public const string MODELS_VEHICLE_WHEEL_WHEEL_PNG="vxengine/models/Vehicle/Wheel/wheel";
			public const string TEXTURES_DEFAULTS_MODEL_SURFACEMAP_PNG="vxengine/textures/Defaults/model_surfacemap";
			public const string TEXTURES_DEFAULTS_MODEL_DIFFUSE_PNG="vxengine/textures/Defaults/model_diffuse";
			public const string TEXTURES_DEFAULTS_NULL_CUBE_DDS="vxengine/textures/Defaults/null_cube";
			public const string TEXTURES_DEFAULTS_BLANK_PNG="vxengine/textures/Defaults/blank";
			public const string TEXTURES_DEFAULTS_GRADIENT_PNG="vxengine/textures/Defaults/gradient";
			public const string TEXTURES_DEFAULTS_RANDOM_PNG="vxengine/textures/Defaults/random";
			public const string TEXTURES_DEFAULTS_MODEL_NORMALMAP_PNG="vxengine/textures/Defaults/model_normalmap";
			public const string TEXTURES_DEFAULTS_GRADIENT_VERTICAL_PNG="vxengine/textures/Defaults/gradient_vertical";
			public const string TEXTURES_TERRAIN_TERRAIN_ICON_PNG="vxengine/textures/terrain/terrain_ICON";
			public const string TEXTURES_TERRAIN_HEIGHTMAP_PNG="vxengine/textures/terrain/Heightmap";
			public const string TEXTURES_TERRAIN_HEIGHTMAP128_PNG="vxengine/textures/terrain/Heightmap128";
			public const string TEXTURES_TERRAIN_HEIGHTMAP256_PNG="vxengine/textures/terrain/Heightmap256";
			public const string TEXTURES_LOGO_LOGO_72_PNG="vxengine/textures/logo/logo_72";
			public const string TEXTURES_MATERIALS_DOTS_PNG="vxengine/textures/Materials/dots";
			public const string TEXTURES_MATERIALS_PAVEMENT_PNG="vxengine/textures/Materials/pavement";
			public const string TEXTURES_MATERIALS_SQUARES_PNG="vxengine/textures/Materials/squares";
			public const string TEXTURES_MATERIALS_BLANK_PNG="vxengine/textures/Materials/blank";
			public const string TEXTURES_MATERIALS_WAVES_PNG="vxengine/textures/Materials/waves";
			public const string TEXTURES_GODRAYS_RAYS0_PNG="vxengine/textures/godrays/rays0";
			public const string TEXTURES_LENSFLARES_FLARE2_PNG="vxengine/textures/lensflares/flare2";
			public const string TEXTURES_LENSFLARES_FLARE1_PNG="vxengine/textures/lensflares/flare1";
			public const string TEXTURES_LENSFLARES_GLOW_PNG="vxengine/textures/lensflares/glow";
			public const string TEXTURES_LENSFLARES_FLARE3_PNG="vxengine/textures/lensflares/flare3";
			public const string TEXTURES_FLARES_FLARE2_PNG="vxengine/textures/Flares/flare2";
			public const string TEXTURES_FLARES_FLARE1_PNG="vxengine/textures/Flares/flare1";
			public const string TEXTURES_FLARES_GLOW_PNG="vxengine/textures/Flares/glow";
			public const string TEXTURES_FLARES_FLARE3_PNG="vxengine/textures/Flares/flare3";
			public const string TEXTURES_NULLTEXTURES_NULL_CUBE_DDS="vxengine/textures/nullTextures/null_cube";
			public const string TEXTURES_NULLTEXTURES_NULL_DIFFUSE_PNG="vxengine/textures/nullTextures/null_diffuse";
			public const string TEXTURES_TERRAIN_TXTRS_TEXTURE_1_PNG="vxengine/textures/terrain/txtrs/texture_1";
			public const string TEXTURES_TERRAIN_TXTRS_TEXTURE_0_PNG="vxengine/textures/terrain/txtrs/texture_0";
			public const string TEXTURES_TERRAIN_TXTRS_TEXTURE_2_PNG="vxengine/textures/terrain/txtrs/texture_2";
			public const string TEXTURES_TERRAIN_TXTRS_TEXTURE_3_PNG="vxengine/textures/terrain/txtrs/texture_3";
			public const string TEXTURES_TERRAIN_CURSOR_CURSOR_PNG="vxengine/textures/terrain/cursor/cursor";
			public const string TEXTURES_TERRAIN_CURSOR_GRID_MAP_PNG="vxengine/textures/terrain/cursor/grid_map";
			public const string TEXTURES_TERRAIN_PAINT_BRUSH_01_PNG="vxengine/textures/terrain/paint/brush_01";
			public const string TEXTURES_TERRAIN_PAINT_BRUSHES_01_PNG="vxengine/textures/terrain/paint/brushes_01";
			public const string TEXTURES_SANDBOX_TERRAIN_EXIT_PNG="vxengine/textures/sandbox/terrain/exit";
			public const string TEXTURES_SANDBOX_TERRAIN_SCULPT_DELTA_PNG="vxengine/textures/sandbox/terrain/sculpt_delta";
			public const string TEXTURES_SANDBOX_TERRAIN_SCULPT_FLAT_PNG="vxengine/textures/sandbox/terrain/sculpt_flat";
			public const string TEXTURES_SANDBOX_TERRAIN_TXTRPAINT_OVERLAY_PNG="vxengine/textures/sandbox/terrain/txtrpaint_overlay";
			public const string TEXTURES_SANDBOX_TERRAIN_EXIT_HOVER_PNG="vxengine/textures/sandbox/terrain/exit_hover";
			public const string TEXTURES_SANDBOX_TERRAIN_TXTRPAINT_HOVER_PNG="vxengine/textures/sandbox/terrain/txtrpaint_hover";
			public const string TEXTURES_SANDBOX_TERRAIN_SCULPT_SMOOTH_HOVER_PNG="vxengine/textures/sandbox/terrain/sculpt_smooth_hover";
			public const string TEXTURES_SANDBOX_TERRAIN_TXTRPAINT_PNG="vxengine/textures/sandbox/terrain/txtrpaint";
			public const string TEXTURES_SANDBOX_TERRAIN_SCULPT_AVG_PNG="vxengine/textures/sandbox/terrain/sculpt_avg";
			public const string TEXTURES_SANDBOX_TERRAIN_SCULPT_LINEAR_HOVER_PNG="vxengine/textures/sandbox/terrain/sculpt_linear_hover";
			public const string TEXTURES_SANDBOX_TERRAIN_SCULPT_DELTA_HOVER_PNG="vxengine/textures/sandbox/terrain/sculpt_delta_hover";
			public const string TEXTURES_SANDBOX_TERRAIN_SCULPT_SMOOTH_PNG="vxengine/textures/sandbox/terrain/sculpt_smooth";
			public const string TEXTURES_SANDBOX_TERRAIN_SCULPT_LINEAR_PNG="vxengine/textures/sandbox/terrain/sculpt_linear";
			public const string TEXTURES_SANDBOX_TERRAIN_SCULPT_PNG="vxengine/textures/sandbox/terrain/sculpt";
			public const string TEXTURES_SANDBOX_TERRAIN_SCULPT_AVG_HOVER_PNG="vxengine/textures/sandbox/terrain/sculpt_avg_hover";
			public const string TEXTURES_SANDBOX_TERRAIN_SCULPT_HOVER_PNG="vxengine/textures/sandbox/terrain/sculpt_hover";
			public const string TEXTURES_SANDBOX_TERRAIN_SCULPT_FLAT_HOVER_PNG="vxengine/textures/sandbox/terrain/sculpt_flat_hover";
			public const string TEXTURES_SANDBOX_FILEEXPLORER_ARROW_UP_PNG="vxengine/textures/sandbox/fileexplorer/arrow_up";
			public const string TEXTURES_SANDBOX_FILEEXPLORER_ARROW_BACK_PNG="vxengine/textures/sandbox/fileexplorer/arrow_back";
			public const string TEXTURES_SANDBOX_FILEEXPLORER_ARROW_FORWARD_PNG="vxengine/textures/sandbox/fileexplorer/arrow_forward";
			public const string TEXTURES_SANDBOX_FILEEXPLORER_UPDATE_PNG="vxengine/textures/sandbox/fileexplorer/update";
			public const string TEXTURES_SANDBOX_RBN_LOGO_32_PNG="vxengine/textures/sandbox/rbn/logo_32";
			public const string TEXTURES_SANDBOX_RBN_LOGO_16_PNG="vxengine/textures/sandbox/rbn/logo_16";
			public const string TEXTURES_SANDBOX_TOOLBAR_ICONS_LEVEL_TEST_STOP_PNG="vxengine/textures/sandbox/toolbar_icons/Level_Test_Stop";
			public const string TEXTURES_SANDBOX_TOOLBAR_ICONS_LEVEL_TEST_REPEAT_PNG="vxengine/textures/sandbox/toolbar_icons/Level_Test_Repeat";
			public const string TEXTURES_SANDBOX_TOOLBAR_ICONS_LEVEL_CLICK_ADDITEMS_PNG="vxengine/textures/sandbox/toolbar_icons/Level_Click_AddItems";
			public const string TEXTURES_SANDBOX_TOOLBAR_ICONS_LEVEL_MANAGE_NEW_PNG="vxengine/textures/sandbox/toolbar_icons/Level_Manage_New";
			public const string TEXTURES_SANDBOX_TOOLBAR_ICONS_TOOLBAR_SEPERATOR_PNG="vxengine/textures/sandbox/toolbar_icons/Toolbar_Seperator";
			public const string TEXTURES_SANDBOX_TOOLBAR_ICONS_LEVEL_MANAGE_SAVE_PNG="vxengine/textures/sandbox/toolbar_icons/Level_Manage_Save";
			public const string TEXTURES_SANDBOX_TOOLBAR_ICONS_LEVEL_TEST_START_PNG="vxengine/textures/sandbox/toolbar_icons/Level_Test_Start";
			public const string TEXTURES_SANDBOX_TOOLBAR_ICONS_LEVEL_MANAGE_OPEN_PNG="vxengine/textures/sandbox/toolbar_icons/Level_Manage_Open";
			public const string TEXTURES_SANDBOX_TOOLBAR_ICONS_LEVEL_CLICK_SELECT_PNG="vxengine/textures/sandbox/toolbar_icons/Level_Click_Select";
			public const string TEXTURES_SANDBOX_TOOLBAR_ICONS_LEVEL_MANAGE_SAVEAS_PNG="vxengine/textures/sandbox/toolbar_icons/Level_Manage_SaveAs";
			public const string TEXTURES_SANDBOX_MISC_BULLET_TOGGLE_MINUS_PNG="vxengine/textures/sandbox/misc/bullet_toggle_minus";
			public const string TEXTURES_SANDBOX_MISC_BULLET_ARROW_DOWN_PNG="vxengine/textures/sandbox/misc/bullet_arrow_down";
			public const string TEXTURES_SANDBOX_MISC_PIN_UNCHECKED_PNG="vxengine/textures/sandbox/misc/pin_unchecked";
			public const string TEXTURES_SANDBOX_MISC_BULLET_TOGGLE_PLUS_PNG="vxengine/textures/sandbox/misc/bullet_toggle_plus";
			public const string TEXTURES_SANDBOX_MISC_CHECK_BOX_UNCHECK_PNG="vxengine/textures/sandbox/misc/check_box_uncheck";
			public const string TEXTURES_SANDBOX_MISC_PIN_CHECKED_PNG="vxengine/textures/sandbox/misc/pin_checked";
			public const string TEXTURES_SANDBOX_MISC_CHECK_BOX_PNG="vxengine/textures/sandbox/misc/check_box";
			public const string TEXTURES_SANDBOX_MISC_CHECK_BOX_MIXED_PNG="vxengine/textures/sandbox/misc/check_box_mixed";
			public const string TEXTURES_SANDBOX_MISC_BULLET_ARROW_RIGHT_PNG="vxengine/textures/sandbox/misc/bullet_arrow_right";
			public const string TEXTURES_GUI_SLIDER_ARROW_LEFT_PNG="vxengine/textures/gui/Slider/Arrow_Left";
			public const string TEXTURES_GUI_SLIDER_ARROW_RIGHT_PNG="vxengine/textures/gui/Slider/Arrow_Right";
			public const string TEXTURES_GUI_TREE_TREE_MODEL_PNG="vxengine/textures/gui/tree/tree_model";
			public const string TEXTURES_GUI_TREE_TREE_WORLD_PNG="vxengine/textures/gui/tree/tree_world";
			public const string TEXTURES_GUI_TREE_ARROW_RIGHT_PNG="vxengine/textures/gui/tree/arrow_right";
			public const string TEXTURES_GUI_TREE_ARROW_DOWN_PNG="vxengine/textures/gui/tree/arrow_down";
			public const string TEXTURES_GUI_TREE_TREE_MESH_PNG="vxengine/textures/gui/tree/tree_mesh";
			public const string TEXTURES_GUI_ICONS_CURSOR_GRID_PNG="vxengine/textures/gui/icons/cursor_grid";
			public const string TEXTURES_GUI_ICONS_ZOOM_SELECTION_PNG="vxengine/textures/gui/icons/zoom_selection";
			public const string TEXTURES_GUI_ICONS_EXIT_PNG="vxengine/textures/gui/icons/exit";
			public const string TEXTURES_GUI_ICONS_WORLD_PNG="vxengine/textures/gui/icons/world";
			public const string TEXTURES_GUI_ICONS_MODEL_MESH_PNG="vxengine/textures/gui/icons/model_mesh";
			public const string TEXTURES_GUI_ICONS_ZOOM_OUT_PNG="vxengine/textures/gui/icons/zoom_out";
			public const string TEXTURES_GUI_ICONS_DEBUG_CONSOLE_PNG="vxengine/textures/gui/icons/debug_console";
			public const string TEXTURES_GUI_ICONS_PICTURE_PNG="vxengine/textures/gui/icons/picture";
			public const string TEXTURES_GUI_ICONS_ZIP_PNG="vxengine/textures/gui/icons/zip";
			public const string TEXTURES_GUI_ICONS_PROPERTIES_PNG="vxengine/textures/gui/icons/properties";
			public const string TEXTURES_GUI_ICONS_MODEL_PNG="vxengine/textures/gui/icons/model";
			public const string TEXTURES_GUI_ICONS_ARROW_RIGHT_PNG="vxengine/textures/gui/icons/arrow_right";
			public const string TEXTURES_GUI_ICONS_LIGHT_ADD_PNG="vxengine/textures/gui/icons/light_add";
			public const string TEXTURES_GUI_ICONS_DEBUG_GRAPH_PNG="vxengine/textures/gui/icons/debug_graph";
			public const string TEXTURES_GUI_ICONS_ARROW_DOWN_PNG="vxengine/textures/gui/icons/arrow_down";
			public const string TEXTURES_GUI_ICONS_DEBUG_FBS_PNG="vxengine/textures/gui/icons/debug_fbs";
			public const string TEXTURES_GUI_ICONS_ZOOM_IN_PNG="vxengine/textures/gui/icons/zoom_in";
			public const string TEXTURES_GUI_ICONS_DOCUMENT_PNG="vxengine/textures/gui/icons/document";
			public const string TEXTURES_GUI_ICONS_CENTROID_PNG="vxengine/textures/gui/icons/centroid";
			public const string TEXTURES_GUI_CURSOR_CURSOR_PNG="vxengine/textures/gui/cursor/Cursor";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_7Z_PNG="vxengine/textures/gui/file extentions/file_extension_7z";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_XPI_PNG="vxengine/textures/gui/file extentions/file_extension_xpi";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_AIFF_PNG="vxengine/textures/gui/file extentions/file_extension_aiff";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_SES_PNG="vxengine/textures/gui/file extentions/file_extension_ses";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_TXT_PNG="vxengine/textures/gui/file extentions/file_extension_txt";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_PST_PNG="vxengine/textures/gui/file extentions/file_extension_pst";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_MOV_PNG="vxengine/textures/gui/file extentions/file_extension_mov";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_DAT_PNG="vxengine/textures/gui/file extentions/file_extension_dat";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_SEA_PNG="vxengine/textures/gui/file extentions/file_extension_sea";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_INDD_PNG="vxengine/textures/gui/file extentions/file_extension_indd";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_CAB_PNG="vxengine/textures/gui/file extentions/file_extension_cab";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_RM_PNG="vxengine/textures/gui/file extentions/file_extension_rm";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_MPEG_PNG="vxengine/textures/gui/file extentions/file_extension_mpeg";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_M4P_PNG="vxengine/textures/gui/file extentions/file_extension_m4p";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_HTM_PNG="vxengine/textures/gui/file extentions/file_extension_htm";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_WAV_PNG="vxengine/textures/gui/file extentions/file_extension_wav";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_DIVX_PNG="vxengine/textures/gui/file extentions/file_extension_divx";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_HTML_PNG="vxengine/textures/gui/file extentions/file_extension_html";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_M4V_PNG="vxengine/textures/gui/file extentions/file_extension_m4v";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_OGG_PNG="vxengine/textures/gui/file extentions/file_extension_ogg";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_PSD_PNG="vxengine/textures/gui/file extentions/file_extension_psd";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_BAT_PNG="vxengine/textures/gui/file extentions/file_extension_bat";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_TMP_PNG="vxengine/textures/gui/file extentions/file_extension_tmp";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_CDR_PNG="vxengine/textures/gui/file extentions/file_extension_cdr";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_LOG_PNG="vxengine/textures/gui/file extentions/file_extension_log";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_MPG_PNG="vxengine/textures/gui/file extentions/file_extension_mpg";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_MCD_PNG="vxengine/textures/gui/file extentions/file_extension_mcd";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_XLS_PNG="vxengine/textures/gui/file extentions/file_extension_xls";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_ISO_PNG="vxengine/textures/gui/file extentions/file_extension_iso";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_DSS_PNG="vxengine/textures/gui/file extentions/file_extension_dss";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_SIT_PNG="vxengine/textures/gui/file extentions/file_extension_sit";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_ASX_PNG="vxengine/textures/gui/file extentions/file_extension_asx";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_AMR_PNG="vxengine/textures/gui/file extentions/file_extension_amr";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_ACE_PNG="vxengine/textures/gui/file extentions/file_extension_ace";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_IFO_PNG="vxengine/textures/gui/file extentions/file_extension_ifo";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_BUP_PNG="vxengine/textures/gui/file extentions/file_extension_bup";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_WMA_PNG="vxengine/textures/gui/file extentions/file_extension_wma";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_JAR_PNG="vxengine/textures/gui/file extentions/file_extension_jar";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_CDL_PNG="vxengine/textures/gui/file extentions/file_extension_cdl";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_VOB_PNG="vxengine/textures/gui/file extentions/file_extension_vob";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_SWF_PNG="vxengine/textures/gui/file extentions/file_extension_swf";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_MID_PNG="vxengine/textures/gui/file extentions/file_extension_mid";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_PUB_PNG="vxengine/textures/gui/file extentions/file_extension_pub";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_TGZ_PNG="vxengine/textures/gui/file extentions/file_extension_tgz";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_FLA_PNG="vxengine/textures/gui/file extentions/file_extension_fla";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_CDA_PNG="vxengine/textures/gui/file extentions/file_extension_cda";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_SS_PNG="vxengine/textures/gui/file extentions/file_extension_ss";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_CBR_PNG="vxengine/textures/gui/file extentions/file_extension_cbr";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_DWG_PNG="vxengine/textures/gui/file extentions/file_extension_dwg";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_CHM_PNG="vxengine/textures/gui/file extentions/file_extension_chm";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_3GP_PNG="vxengine/textures/gui/file extentions/file_extension_3gp";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_QBB_PNG="vxengine/textures/gui/file extentions/file_extension_qbb";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_ASF_PNG="vxengine/textures/gui/file extentions/file_extension_asf";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_EML_PNG="vxengine/textures/gui/file extentions/file_extension_eml";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_TORRENT_PNG="vxengine/textures/gui/file extentions/file_extension_torrent";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_EXE_PNG="vxengine/textures/gui/file extentions/file_extension_exe";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_PNG_PNG="vxengine/textures/gui/file extentions/file_extension_png";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_DOC_PNG="vxengine/textures/gui/file extentions/file_extension_doc";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_BMP_PNG="vxengine/textures/gui/file extentions/file_extension_bmp";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_M4B_PNG="vxengine/textures/gui/file extentions/file_extension_m4b";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_TIF_PNG="vxengine/textures/gui/file extentions/file_extension_tif";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_MSI_PNG="vxengine/textures/gui/file extentions/file_extension_msi";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_PS_PNG="vxengine/textures/gui/file extentions/file_extension_ps";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_AI_PNG="vxengine/textures/gui/file extentions/file_extension_ai";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_ZIP_PNG="vxengine/textures/gui/file extentions/file_extension_zip";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_PPS_PNG="vxengine/textures/gui/file extentions/file_extension_pps";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_WPS_PNG="vxengine/textures/gui/file extentions/file_extension_wps";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_QBW_PNG="vxengine/textures/gui/file extentions/file_extension_qbw";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_HQX_PNG="vxengine/textures/gui/file extentions/file_extension_hqx";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_RAR_PNG="vxengine/textures/gui/file extentions/file_extension_rar";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_AIF_PNG="vxengine/textures/gui/file extentions/file_extension_aif";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_DVF_PNG="vxengine/textures/gui/file extentions/file_extension_dvf";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_GZ_PNG="vxengine/textures/gui/file extentions/file_extension_gz";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_MP4_PNG="vxengine/textures/gui/file extentions/file_extension_mp4";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_JPG_PNG="vxengine/textures/gui/file extentions/file_extension_jpg";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_EPS_PNG="vxengine/textures/gui/file extentions/file_extension_eps";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_RTF_PNG="vxengine/textures/gui/file extentions/file_extension_rtf";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_DMG_PNG="vxengine/textures/gui/file extentions/file_extension_dmg";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_VCD_PNG="vxengine/textures/gui/file extentions/file_extension_vcd";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_LNK_PNG="vxengine/textures/gui/file extentions/file_extension_lnk";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_SITX_PNG="vxengine/textures/gui/file extentions/file_extension_sitx";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_DLL_PNG="vxengine/textures/gui/file extentions/file_extension_dll";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_GIF_PNG="vxengine/textures/gui/file extentions/file_extension_gif";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_FLV_PNG="vxengine/textures/gui/file extentions/file_extension_flv";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_THM_PNG="vxengine/textures/gui/file extentions/file_extension_thm";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_M4A_PNG="vxengine/textures/gui/file extentions/file_extension_m4a";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_BIN_PNG="vxengine/textures/gui/file extentions/file_extension_bin";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_RMVB_PNG="vxengine/textures/gui/file extentions/file_extension_rmvb";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_RAM_PNG="vxengine/textures/gui/file extentions/file_extension_ram";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_PTB_PNG="vxengine/textures/gui/file extentions/file_extension_ptb";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_QXD_PNG="vxengine/textures/gui/file extentions/file_extension_qxd";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_MP2_PNG="vxengine/textures/gui/file extentions/file_extension_mp2";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_TTF_PNG="vxengine/textures/gui/file extentions/file_extension_ttf";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_MDB_PNG="vxengine/textures/gui/file extentions/file_extension_mdb";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_MSWMM_PNG="vxengine/textures/gui/file extentions/file_extension_mswmm";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_PDF_PNG="vxengine/textures/gui/file extentions/file_extension_pdf";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_JPEG_PNG="vxengine/textures/gui/file extentions/file_extension_jpeg";
			public const string TEXTURES_GUI_FILE_EXTENTIONS_FILE_EXTENSION_WMV_PNG="vxengine/textures/gui/file extentions/file_extension_wmv";
			public const string TEXTURES_GUI_NET_NETWORK_IP_PNG="vxengine/textures/gui/net/network_ip";
			public const string TEXTURES_GUI_NET_NETWORK_HUB_PNG="vxengine/textures/gui/net/network_hub";
			public const string TEXTURES_SANDBOX_RBN_GIMBAL_ORIGIN_PNG="vxengine/textures/sandbox/rbn/gimbal/origin";
			public const string TEXTURES_SANDBOX_RBN_ENTITIES_ENTITY_ADD_32_PNG="vxengine/textures/sandbox/rbn/entities/entity_add_32";
			public const string TEXTURES_SANDBOX_RBN_ENTITIES_TERRAIN_ADD_16_PNG="vxengine/textures/sandbox/rbn/entities/terrain_add_16";
			public const string TEXTURES_SANDBOX_RBN_ENTITIES_TERRAIN_ADD_32_PNG="vxengine/textures/sandbox/rbn/entities/terrain_add_32";
			public const string TEXTURES_SANDBOX_RBN_ENTITIES_WATER_ADD_16_PNG="vxengine/textures/sandbox/rbn/entities/water_add_16";
			public const string TEXTURES_SANDBOX_RBN_ENTITIES_ENTITY_DELETE_16_PNG="vxengine/textures/sandbox/rbn/entities/entity_delete_16";
			public const string TEXTURES_SANDBOX_RBN_ENTITIES_LIGHTBULB_ADD_PNG="vxengine/textures/sandbox/rbn/entities/lightbulb_add";
			public const string TEXTURES_SANDBOX_RBN_ENTITIES_ENTITY_ADD_16_PNG="vxengine/textures/sandbox/rbn/entities/entity_add_16";
			public const string TEXTURES_SANDBOX_RBN_TERRAIN_EDIT_FLAT_PNG="vxengine/textures/sandbox/rbn/terrain/edit_flat";
			public const string TEXTURES_SANDBOX_RBN_TERRAIN_EXIT_PNG="vxengine/textures/sandbox/rbn/terrain/exit";
			public const string TEXTURES_SANDBOX_RBN_TERRAIN_SCULPT_DELTA_PNG="vxengine/textures/sandbox/rbn/terrain/sculpt_delta";
			public const string TEXTURES_SANDBOX_RBN_TERRAIN_TERRAIN_EDIT_PNG="vxengine/textures/sandbox/rbn/terrain/terrain_edit";
			public const string TEXTURES_SANDBOX_RBN_TERRAIN_MODE_TXTRPAINT_PNG="vxengine/textures/sandbox/rbn/terrain/mode_txtrpaint";
			public const string TEXTURES_SANDBOX_RBN_TERRAIN_EDIT_SMOOTH_PNG="vxengine/textures/sandbox/rbn/terrain/edit_smooth";
			public const string TEXTURES_SANDBOX_RBN_TERRAIN_SCULPT_AVG_PNG="vxengine/textures/sandbox/rbn/terrain/sculpt_avg";
			public const string TEXTURES_SANDBOX_RBN_TERRAIN_MODE_SULPT_PNG="vxengine/textures/sandbox/rbn/terrain/mode_sulpt";
			public const string TEXTURES_SANDBOX_RBN_TERRAIN_EDIT_LINEAR_PNG="vxengine/textures/sandbox/rbn/terrain/edit_linear";
			public const string TEXTURES_SANDBOX_RBN_MAIN_EXPORT_16_PNG="vxengine/textures/sandbox/rbn/main/export_16";
			public const string TEXTURES_SANDBOX_RBN_MAIN_SELECT_PNG="vxengine/textures/sandbox/rbn/main/select";
			public const string TEXTURES_SANDBOX_RBN_MAIN_SAVE_AS_16_PNG="vxengine/textures/sandbox/rbn/main/save_as_16";
			public const string TEXTURES_SANDBOX_RBN_MAIN_SAVE_16_PNG="vxengine/textures/sandbox/rbn/main/save_16";
			public const string TEXTURES_SANDBOX_RBN_MAIN_REDO_16_PNG="vxengine/textures/sandbox/rbn/main/redo_16";
			public const string TEXTURES_SANDBOX_RBN_MAIN_NEW_16_PNG="vxengine/textures/sandbox/rbn/main/new_16";
			public const string TEXTURES_SANDBOX_RBN_MAIN_OPEN_32_PNG="vxengine/textures/sandbox/rbn/main/open_32";
			public const string TEXTURES_SANDBOX_RBN_MAIN_OPEN_16_PNG="vxengine/textures/sandbox/rbn/main/open_16";
			public const string TEXTURES_SANDBOX_RBN_MAIN_TEST_16_PNG="vxengine/textures/sandbox/rbn/main/test_16";
			public const string TEXTURES_SANDBOX_RBN_MAIN_UNDO_16_PNG="vxengine/textures/sandbox/rbn/main/undo_16";
			public const string TEXTURES_SANDBOX_RBN_MAIN_TEST_32_PNG="vxengine/textures/sandbox/rbn/main/test_32";
			public const string TEXTURES_SANDBOX_RBN_MAIN_IMPORT_16_PNG="vxengine/textures/sandbox/rbn/main/import_16";
			public const string TEXTURES_SANDBOX_RBN_MAIN_SETTING_32_PNG="vxengine/textures/sandbox/rbn/main/setting_32";
			public const string TEXTURES_SANDBOX_RBN_MAIN_HELP_16_PNG="vxengine/textures/sandbox/rbn/main/help_16";
			public const string TEXTURES_SANDBOX_TOOLBAR_ICONS_CNTRLS_MS_SLCT_R_PNG="vxengine/textures/sandbox/toolbar_icons/cntrls/ms_slct_R";
			public const string TEXTURES_SANDBOX_TOOLBAR_ICONS_CNTRLS_MS_SLCT_L_PNG="vxengine/textures/sandbox/toolbar_icons/cntrls/ms_slct_L";
			public const string TEXTURES_SANDBOX_TOOLBAR_ICONS_CNTRLS_MS_SLCT_SCRLL_PNG="vxengine/textures/sandbox/toolbar_icons/cntrls/ms_slct_scrll";
			public const string TEXTURES_SANDBOX_MISC_REF_TOGGLE_PNG="vxengine/textures/sandbox/misc/ref/toggle";
			public const string TEXTURES_SANDBOX_MISC_REF_BULLET_TOGGLE_MINUS_COPY_PNG="vxengine/textures/sandbox/misc/ref/bullet_toggle_minus copy";
			public const string TEXTURES_SANDBOX_MISC_REF_CHECK_BOX_UNCHECK_PNG="vxengine/textures/sandbox/misc/ref/check_box_uncheck";
			public const string TEXTURES_SANDBOX_MISC_REF_BULLET_TOGGLE_PLUS_COPY_PNG="vxengine/textures/sandbox/misc/ref/bullet_toggle_plus copy";
			public const string TEXTURES_SANDBOX_MISC_REF_TOGGLE_EXPAND_PNG="vxengine/textures/sandbox/misc/ref/toggle_expand";
			public const string TEXTURES_SANDBOX_MISC_REF_CHECK_BOX_PNG="vxengine/textures/sandbox/misc/ref/check_box";
			public const string TEXTURES_SANDBOX_TLBR_GIMBAL_ORIGIN_PNG="vxengine/textures/sandbox/tlbr/gimbal/origin";
			public const string TEXTURES_SANDBOX_TLBR_CMD_REDO_PNG="vxengine/textures/sandbox/tlbr/cmd/redo";
			public const string TEXTURES_SANDBOX_TLBR_CMD_REDO_HOVER_PNG="vxengine/textures/sandbox/tlbr/cmd/redo_hover";
			public const string TEXTURES_SANDBOX_TLBR_CMD_UNDO_PNG="vxengine/textures/sandbox/tlbr/cmd/undo";
			public const string TEXTURES_SANDBOX_TLBR_CMD_UNDO_HOVER_PNG="vxengine/textures/sandbox/tlbr/cmd/undo_hover";
			public const string TEXTURES_SANDBOX_TLBR_FILE_FILE_SAVEAS_HOVER_PNG="vxengine/textures/sandbox/tlbr/file/file_saveas_hover";
			public const string TEXTURES_SANDBOX_TLBR_FILE_FILE_OPEN_PNG="vxengine/textures/sandbox/tlbr/file/file_open";
			public const string TEXTURES_SANDBOX_TLBR_FILE_FILE_SAVE_PNG="vxengine/textures/sandbox/tlbr/file/file_save";
			public const string TEXTURES_SANDBOX_TLBR_FILE_FILE_SAVE_HOVER_PNG="vxengine/textures/sandbox/tlbr/file/file_save_hover";
			public const string TEXTURES_SANDBOX_TLBR_FILE_FILE_OPEN_HOVER_PNG="vxengine/textures/sandbox/tlbr/file/file_open_hover";
			public const string TEXTURES_SANDBOX_TLBR_FILE_FILE_NEW_HOVER_PNG="vxengine/textures/sandbox/tlbr/file/file_new_hover";
			public const string TEXTURES_SANDBOX_TLBR_FILE_FILE_NEW_PNG="vxengine/textures/sandbox/tlbr/file/file_new";
			public const string TEXTURES_SANDBOX_TLBR_FILE_FILE_SAVEAS_PNG="vxengine/textures/sandbox/tlbr/file/file_saveas";
			public const string TEXTURES_SANDBOX_TLBR_TERRAIN_EXIT_PNG="vxengine/textures/sandbox/tlbr/terrain/exit";
			public const string TEXTURES_SANDBOX_TLBR_TERRAIN_SCULPT_DELTA_PNG="vxengine/textures/sandbox/tlbr/terrain/sculpt_delta";
			public const string TEXTURES_SANDBOX_TLBR_TERRAIN_SCULPT_FLAT_PNG="vxengine/textures/sandbox/tlbr/terrain/sculpt_flat";
			public const string TEXTURES_SANDBOX_TLBR_TERRAIN_TXTRPAINT_OVERLAY_PNG="vxengine/textures/sandbox/tlbr/terrain/txtrpaint_overlay";
			public const string TEXTURES_SANDBOX_TLBR_TERRAIN_EXIT_HOVER_PNG="vxengine/textures/sandbox/tlbr/terrain/exit_hover";
			public const string TEXTURES_SANDBOX_TLBR_TERRAIN_TXTRPAINT_HOVER_PNG="vxengine/textures/sandbox/tlbr/terrain/txtrpaint_hover";
			public const string TEXTURES_SANDBOX_TLBR_TERRAIN_SCULPT_SMOOTH_HOVER_PNG="vxengine/textures/sandbox/tlbr/terrain/sculpt_smooth_hover";
			public const string TEXTURES_SANDBOX_TLBR_TERRAIN_TXTRPAINT_PNG="vxengine/textures/sandbox/tlbr/terrain/txtrpaint";
			public const string TEXTURES_SANDBOX_TLBR_TERRAIN_SCULPT_AVG_PNG="vxengine/textures/sandbox/tlbr/terrain/sculpt_avg";
			public const string TEXTURES_SANDBOX_TLBR_TERRAIN_SCULPT_LINEAR_HOVER_PNG="vxengine/textures/sandbox/tlbr/terrain/sculpt_linear_hover";
			public const string TEXTURES_SANDBOX_TLBR_TERRAIN_SCULPT_DELTA_HOVER_PNG="vxengine/textures/sandbox/tlbr/terrain/sculpt_delta_hover";
			public const string TEXTURES_SANDBOX_TLBR_TERRAIN_SCULPT_SMOOTH_PNG="vxengine/textures/sandbox/tlbr/terrain/sculpt_smooth";
			public const string TEXTURES_SANDBOX_TLBR_TERRAIN_SCULPT_LINEAR_PNG="vxengine/textures/sandbox/tlbr/terrain/sculpt_linear";
			public const string TEXTURES_SANDBOX_TLBR_TERRAIN_SCULPT_PNG="vxengine/textures/sandbox/tlbr/terrain/sculpt";
			public const string TEXTURES_SANDBOX_TLBR_TERRAIN_SCULPT_AVG_HOVER_PNG="vxengine/textures/sandbox/tlbr/terrain/sculpt_avg_hover";
			public const string TEXTURES_SANDBOX_TLBR_TERRAIN_SCULPT_HOVER_PNG="vxengine/textures/sandbox/tlbr/terrain/sculpt_hover";
			public const string TEXTURES_SANDBOX_TLBR_TERRAIN_SCULPT_FLAT_HOVER_PNG="vxengine/textures/sandbox/tlbr/terrain/sculpt_flat_hover";
			public const string TEXTURES_SANDBOX_TLBR_IO_IO_EXPORT_HOVER_PNG="vxengine/textures/sandbox/tlbr/io/io_export_hover";
			public const string TEXTURES_SANDBOX_TLBR_IO_IO_IMPORT_PNG="vxengine/textures/sandbox/tlbr/io/io_import";
			public const string TEXTURES_SANDBOX_TLBR_IO_IO_IMPORT_HOVER_PNG="vxengine/textures/sandbox/tlbr/io/io_import_hover";
			public const string TEXTURES_SANDBOX_TLBR_IO_IO_EXPORT_PNG="vxengine/textures/sandbox/tlbr/io/io_export";
			public const string TEXTURES_SANDBOX_TLBR_TEST_TEST_RUN_PNG="vxengine/textures/sandbox/tlbr/test/test_run";
			public const string TEXTURES_SANDBOX_TLBR_TEST_TEST_RUN_HOVER_PNG="vxengine/textures/sandbox/tlbr/test/test_run_hover";
			public const string TEXTURES_SANDBOX_TLBR_TEST_TEST_STOP_HOVER_PNG="vxengine/textures/sandbox/tlbr/test/test_stop_hover";
			public const string TEXTURES_SANDBOX_TLBR_TEST_TEST_RESTART_HOVER_PNG="vxengine/textures/sandbox/tlbr/test/test_restart_hover";
			public const string TEXTURES_SANDBOX_TLBR_TEST_TEST_RESTART_PNG="vxengine/textures/sandbox/tlbr/test/test_restart";
			public const string TEXTURES_SANDBOX_TLBR_TEST_TEST_STOP_PNG="vxengine/textures/sandbox/tlbr/test/test_stop";
			public const string TEXTURES_SANDBOX_TLBR_ITEMS_ITEMS_ADD_PNG="vxengine/textures/sandbox/tlbr/items/items_add";
			public const string TEXTURES_SANDBOX_TLBR_ITEMS_ITEMS_ADD_HOVER_PNG="vxengine/textures/sandbox/tlbr/items/items_add_hover";
			public const string TEXTURES_SANDBOX_TLBR_MISC_BULLET_TOGGLE_MINUS_PNG="vxengine/textures/sandbox/tlbr/misc/bullet_toggle_minus";
			public const string TEXTURES_SANDBOX_TLBR_MISC_BULLET_ARROW_DOWN_PNG="vxengine/textures/sandbox/tlbr/misc/bullet_arrow_down";
			public const string TEXTURES_SANDBOX_TLBR_MISC_PIN_UNCHECKED_PNG="vxengine/textures/sandbox/tlbr/misc/pin_unchecked";
			public const string TEXTURES_SANDBOX_TLBR_MISC_BULLET_TOGGLE_PLUS_PNG="vxengine/textures/sandbox/tlbr/misc/bullet_toggle_plus";
			public const string TEXTURES_SANDBOX_TLBR_MISC_CHECK_BOX_UNCHECK_PNG="vxengine/textures/sandbox/tlbr/misc/check_box_uncheck";
			public const string TEXTURES_SANDBOX_TLBR_MISC_PIN_CHECKED_PNG="vxengine/textures/sandbox/tlbr/misc/pin_checked";
			public const string TEXTURES_SANDBOX_TLBR_MISC_CHECK_BOX_PNG="vxengine/textures/sandbox/tlbr/misc/check_box";
			public const string TEXTURES_SANDBOX_TLBR_MISC_BULLET_ARROW_RIGHT_PNG="vxengine/textures/sandbox/tlbr/misc/bullet_arrow_right";
			public const string TEXTURES_SANDBOX_TLBR_SEL_SEL_ADDITEM_PNG="vxengine/textures/sandbox/tlbr/sel/sel_addItem";
			public const string TEXTURES_SANDBOX_TLBR_SEL_SEL_TERRAINEDIT_HOVER_PNG="vxengine/textures/sandbox/tlbr/sel/sel_terrainEdit_hover";
			public const string TEXTURES_SANDBOX_TLBR_SEL_SEL_SELITEM_PNG="vxengine/textures/sandbox/tlbr/sel/sel_selItem";
			public const string TEXTURES_SANDBOX_TLBR_SEL_LAYER_GRID_PNG="vxengine/textures/sandbox/tlbr/sel/layer_grid";
			public const string TEXTURES_SANDBOX_TLBR_SEL_SEL_TERRAINEDIT_PNG="vxengine/textures/sandbox/tlbr/sel/sel_terrainEdit";
			public const string TEXTURES_SANDBOX_TLBR_SEL_SEL_ADDITEM_HOVER_PNG="vxengine/textures/sandbox/tlbr/sel/sel_addItem_hover";
			public const string TEXTURES_SANDBOX_TLBR_SEL_LAYER_TRANSPARENT_PNG="vxengine/textures/sandbox/tlbr/sel/layer_transparent";
			public const string TEXTURES_SANDBOX_TLBR_SEL_SEL_SELITEM_HOVER_PNG="vxengine/textures/sandbox/tlbr/sel/sel_selItem_hover";
			public const string TEXTURES_SANDBOX_RBN_GIMBAL_TRANSFORM_GIMBAL_LOCAL_PNG="vxengine/textures/sandbox/rbn/gimbal/transform/gimbal_local";
			public const string TEXTURES_SANDBOX_RBN_GIMBAL_TRANSFORM_GIMBAL_GLOBAL_PNG="vxengine/textures/sandbox/rbn/gimbal/transform/gimbal_global";
			public const string TEXTURES_SANDBOX_TLBR_GIMBAL_TRANSFORM_GIMBAL_LOCAL_PNG="vxengine/textures/sandbox/tlbr/gimbal/transform/gimbal_local";
			public const string TEXTURES_SANDBOX_TLBR_GIMBAL_TRANSFORM_GIMBAL_GLOBAL_HOVER_PNG="vxengine/textures/sandbox/tlbr/gimbal/transform/gimbal_global_hover";
			public const string TEXTURES_SANDBOX_TLBR_GIMBAL_TRANSFORM_GIMBAL_LOCAL_HOVER_PNG="vxengine/textures/sandbox/tlbr/gimbal/transform/gimbal_local_hover";
			public const string TEXTURES_SANDBOX_TLBR_GIMBAL_TRANSFORM_GIMBAL_GLOBAL_PNG="vxengine/textures/sandbox/tlbr/gimbal/transform/gimbal_global";
			public const string TEXTURES_SANDBOX_TLBR_MISC_REF_TOGGLE_PNG="vxengine/textures/sandbox/tlbr/misc/ref/toggle";
			public const string TEXTURES_SANDBOX_TLBR_MISC_REF_BULLET_TOGGLE_MINUS_COPY_PNG="vxengine/textures/sandbox/tlbr/misc/ref/bullet_toggle_minus copy";
			public const string TEXTURES_SANDBOX_TLBR_MISC_REF_CHECK_BOX_UNCHECK_PNG="vxengine/textures/sandbox/tlbr/misc/ref/check_box_uncheck";
			public const string TEXTURES_SANDBOX_TLBR_MISC_REF_BULLET_TOGGLE_PLUS_COPY_PNG="vxengine/textures/sandbox/tlbr/misc/ref/bullet_toggle_plus copy";
			public const string TEXTURES_SANDBOX_TLBR_MISC_REF_TOGGLE_EXPAND_PNG="vxengine/textures/sandbox/tlbr/misc/ref/toggle_expand";
			public const string TEXTURES_SANDBOX_TLBR_MISC_REF_CHECK_BOX_PNG="vxengine/textures/sandbox/tlbr/misc/ref/check_box";
			public const string MODELS_TITLESCREEN_SPHERE_SPHERE_FBX="vxengine/models/titleScreen/sphere/sphere";
			public const string MODELS_TITLESCREEN_LOGO_SPLIITER_DARK_PNG="vxengine/models/titleScreen/logo/spliiter_dark";
			public const string MODELS_TITLESCREEN_LOGO_SPLIITER_PNG="vxengine/models/titleScreen/logo/spliiter";
			public const string MODELS_TITLESCREEN_LOGO_VRTX_VRTX_TITLE_PNG="vxengine/models/titleScreen/logo/vrtx/vrtx_title";
			public const string MODELS_TITLESCREEN_LOGO_VRTX_VRTX_TITLE_DARK_PNG="vxengine/models/titleScreen/logo/vrtx/vrtx_title_dark";
			public const string MODELS_TITLESCREEN_LOGO_VRTX_LOGO_BTN_PNG="vxengine/models/titleScreen/logo/vrtx/logo_btn";
			public const string MODELS_TITLESCREEN_LOGO_VRTX_VRTX_BTN_PNG="vxengine/models/titleScreen/logo/vrtx/vrtx_btn";
			public const string MODELS_TITLESCREEN_LOGO_VRTX_LOGO_72_PNG="vxengine/models/titleScreen/logo/vrtx/logo_72";
			public const string MODELS_TITLESCREEN_LOGO_VRTC_VRTC_LOGO_512_PNG="vxengine/models/titleScreen/logo/vrtc/vrtc_logo_512";
			public const string MODELS_TITLESCREEN_LOGO_VRTC_BUILT_WITH_PNG="vxengine/models/titleScreen/logo/vrtc/built_with";
			public const string MODELS_TITLESCREEN_LOGO_VRTC_VRTC_TITLE_DARK_PNG="vxengine/models/titleScreen/logo/vrtc/vrtc_title_dark";
			public const string MODELS_TITLESCREEN_LOGO_VRTC_VRTC_TITLE_PNG="vxengine/models/titleScreen/logo/vrtc/vrtc_title";
			public const string MODELS_TITLESCREEN_LOGO_VRTC_VRTC_LOGO_256_PNG="vxengine/models/titleScreen/logo/vrtc/vrtc_logo_256";
			public const string MODELS_TITLESCREEN_LOGO_VRTC_BUILT_WITH_DARK_PNG="vxengine/models/titleScreen/logo/vrtc/built_with_dark";
			public const string MODELS_TITLESCREEN_LOGO_VRTC_VRTC_LOGO_128_PNG="vxengine/models/titleScreen/logo/vrtc/vrtc_logo_128";
			public const string TITLESCREEN_SPHERE_SPHERE_FBX="vxengine/titleScreen/sphere/sphere";
			public const string SNDFX_MENU_CLICK_ERROR_MP3="vxengine/sndFx/Menu/Click/error";
			public const string SNDFX_MENU_CLICK_WAV="vxengine/sndFx/Menu/click";
			public const string SNDFX_MENU_ERROR_WAV="vxengine/sndFx/Menu/error";
			public const string SNDFX_MENU_CONFIRM_WAV="vxengine/sndFx/Menu/confirm";
			public const string SNDFX_MENU_ORIG_CLICK_WAV="vxengine/sndFx/Menu/orig/click";
			public const string SNDFX_MENU_ORIG_ERROR_WAV="vxengine/sndFx/Menu/orig/error";
			public const string SNDFX_MENU_ORIG_MENUSOUND1_WAV="vxengine/sndFx/Menu/orig/MenuSound1";
			public const string SNDFX_MENU_ORIG_CONFIRM_WAV="vxengine/sndFx/Menu/orig/confirm";
			public const string SNDFX_MENU_CLICK_CLICK_WAV="vxengine/sndFx/Menu/Click/click";
			public const string SNDFX_MENU_CLICK_ERROR_WAV="vxengine/sndFx/Menu/Click/error";
			public const string SNDFX_MENU_CLICK_PREVIOUS_MENU_CLICK_WAV="vxengine/sndFx/Menu/Click/previous/Menu_Click";
			public const string SNDFX_MENU_CLICK_PREVIOUS_1MENU_CLICK_WAV="vxengine/sndFx/Menu/Click/previous/1Menu_Click";
			public const string SNDFX_MENU_CLICK_PREVIOUS_2MENU_CLICK_WAV="vxengine/sndFx/Menu/Click/previous/2Menu_Click";
			public const string MODELS_LGHTNG_UNIT_SPHERE_FBX="vxengine/models/lghtng/unit_sphere";
			public const string MODELS_LGHTNG_UNIT_SPHERE_MG_FBX="vxengine/models/lghtng/unit_sphere_mg";
			public const string FONTS_FONT_SPLASH_24_SPRITEFONT="vxengine/fonts/font_splash_24";
		}


	}
}
