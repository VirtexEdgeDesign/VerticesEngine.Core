using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VerticesEngine
{

    public enum vxPlatformOS
    {
        Windows,

        OSX,

        Linux,

        iOS,

        Android,

        Switch
    }

    /// <summary>
    /// Which type of hardware are we running on? Desktop, Console, Mobile etc...
    /// </summary>
    public enum vxPlatformHardwareType
    {
        Desktop,
        Console,
        Mobile,
        Web
    }

    public enum vxPlatformType
    {
        /// <summary>
        /// There is no store listing for this game
        /// </summary>
        None,

        /// <summary>
        /// This Build is for a Steam release
        /// </summary>
        Steam,


        /// <summary>
        /// This Build is for an ItchIO release
        /// </summary>
        ItchIO,


        /// <summary>
        /// This Build is for a Discord release
        /// </summary>
        Discord,


        /// <summary>
        /// The Google Play Store
        /// </summary>
        GooglePlayStore,

        /// <summary>
        /// The Amazon Mobile store
        /// </summary>
        AmazonPlayStore,


        /// <summary>
        /// The Apple App Store
        /// </summary>
        AppleAppStore,

        /// <summary>
        /// The Nintendo Switch eShop 
        /// </summary>
        NintendoSwitch,
    }

    /// <summary>
    /// enum of build configuration types. This can be used instead of compiler flags so
    /// to allows for special debug code that can still be run in a release version.
    /// </summary>
    public enum vxBuildType
    {
        /// <summary>
        /// The engine is in Debug mode. This can unclock a number of functions in th e
        /// engine for debugging. Launch the game with the '-dev' launch parameter to 
        /// set this flag in a release enviroment.
        /// </summary>
        Debug,

        /// <summary>
        /// The Release build tag. This deactivates all Debug info.
        /// </summary>
        Release
    }

    /// <summary>
    /// This has the games initializing stages
    /// </summary>
    public enum GameInitializationStage
    {
        /// <summary>
        /// The game is waiting for a response or for user input and shouldn't
        /// perform any function until then
        /// </summary>
        Waiting,

        /// <summary>
        /// Primary startup of game loading basic info
        /// </summary>
        PrimaryStartup,

        /// <summary>
        /// Showing the title pages
        /// </summary>
        TitlePage,

        /// <summary>
        /// Notify use of permissions (mainly for Android)
        /// </summary>
        NotifyOfPermissions,

        /// <summary>
        /// Sign user into any platform if available
        /// </summary>
        SigningInUser,


        /// <summary>
        /// Is it loading global content
        /// </summary>
        LoadingGlobalContent,

        /// <summary>
        /// Checks if the game has been updated
        /// </summary>
        CheckIfUpdated,


        /// <summary>
        /// Set of game specific checks
        /// </summary>
        GameSpecificChecks,

        /// <summary>
        /// All initializations have been fired, we can start up
        /// </summary>
        ReadyToRun,

        /// <summary>
        /// The game is running
        /// </summary>
        Running,
    }

    /// <summary>
    /// Selection state, useful for Sandbox items. 
    /// </summary>
    public enum vxSelectionState
    {
        /// <summary>
        /// Item is unseleced and is not hovered.
        /// </summary>
        None,

        /// <summary>
        /// Item is being hovered.
        /// </summary>
        Hover,

        /// <summary>
        /// The Item is selected.
        /// </summary>
        Selected
    }
}


namespace VerticesEngine.Graphics
{
    /// <summary>
    /// An enum holding the platform/backend type for this engine build.
    /// </summary>
    public enum vxGraphicalBackend
    {
        /// <summary>
        /// This build is running with the DirectX backend
        /// </summary>
        DirectX,


        /// <summary>
        /// This build is running with the Desktop OpenGL backend
        /// </summary>
        OpenGL,


        /// <summary>
        /// This build is running with the Android backend
        /// </summary>
        Android,


        /// <summary>
        /// This build is running with the iOS backend
        /// </summary>
        iOS
    }
}

/// <summary>
/// Main orientation for Mobile Games.
/// </summary>
public enum vxOrientationType
{
    Portrait,

    Landscape
}



/// <summary>
/// Is the game a 2D or 3D Game. Is it VR? etc...
/// </summary>
[Flags]
public enum vxGameEnviromentType
{
    /// <summary>
    /// This is mainly a 2D game
    /// </summary>
    TwoDimensional,

    /// <summary>
    /// This is mainly a 3D game
    /// </summary>
    ThreeDimensional,

    VR,

    /// <summary>
    /// This game should support both 2D and 3D
    /// </summary>
    Both
}

/// <summary>
/// Is the game a Local game, or is it networked.
/// </summary>
public enum vxNetworkGameType
{
    /// <summary>
    /// The game is a local game.
    /// </summary>
    Local,

    /// <summary>
    /// The game is a network game.
    /// </summary>
    Networked
}

/*****************************************************************************/
/*							Cascade Shadow Mapping  						 */
/*****************************************************************************/

/// <summary>
/// Shadow map overlay mode.
/// </summary>
public enum vxEnumShadowMapOverlayMode
{
    None,
    ShadowFrustums,
    ShadowMap,
    ShadowMapAndViewFrustum
};

/// <summary>
/// Virtual camera mode.
/// </summary>
public enum vxEnumVirtualCameraMode
{
    None,
    ViewFrustum,
    ShadowSplits
};

/// <summary>
/// Scene shadow mode.
/// </summary>
public enum vxEnumSceneDebugMode
{
    Default,
    EncodedIndex,
    SplitColors,
    /*
    MeshTessellation,
    TexturedWireFrame,
    WireFrame,
    NormalMap,
    DepthMap,
    LightMap,
    SSAO,
    SSRUVs,
    BlankShadow,
    SplitColors,
    BlockPattern,
    */
    PhysicsDebug,
};
