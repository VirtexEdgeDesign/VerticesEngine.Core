
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

            Textures.Blank = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/Defaults/blank");
            Textures.Gradient = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/Defaults/gradient");
            Textures.RandomValues = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/Defaults/random");

            Textures.DefaultDiffuse = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/Defaults/model_diffuse");
            Textures.ErrorTexture = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/Defaults/model_diffuse");
            Textures.DefaultNormalMap = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/Defaults/model_normalmap");
            Textures.DefaultSurfaceMap = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/Defaults/model_surfacemap");


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
            UI.VirtexLogo = vxContentManager.Instance.Load<Texture2D>("vxengine/titleScreen/logo/vrtx/vrtx_btn");

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
            Models.PointLightSphere = vxContentManager.Instance.Load<Model>("vxengine/models/lghtng/sphere");
        }
	}
}
