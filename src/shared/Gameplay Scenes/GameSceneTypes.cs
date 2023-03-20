using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine
{
    /// <summary>
    /// Sandbox game type. Is it a fresh start, should it open a file, or is it running the file as a game.
    /// </summary>
    public enum vxStartGameMode
    {
        /// <summary>
        /// Starts the level in Editor Mode
        /// </summary>
        Editor,

        /// <summary>
        /// Runs the level as if it's the game, no editing is allowed in this setting.
        /// </summary>
        GamePlay,
    }

    /// <summary>
    /// Sandbox game state.
    /// </summary>
    public enum vxEnumSandboxStatus
    {
        /// <summary>
        /// Sandbox is in Edit Mode
        /// </summary>
        EditMode,

        /// <summary>
        /// Sandbox is Running
        /// </summary>
        Running,
    }

    public enum vxEnumSanboxEditMode
    {
        /// <summary>
        /// Adds an Item on Click
        /// </summary>
        AddItem,

        /// <summary>
        /// Selects the Highlited Item on Click
        /// </summary>
        SelectItem,

        /// <summary>
        /// 
        /// </summary>
        TerrainEdit,

    }

    public enum vxEnumTerrainEditMode
    {
        Disabled,

        Sculpt,

        /// <summary>
        /// Selects the Highlited Item on Click
        /// </summary>
        TexturePaint,
    }


    public enum vxEnumAddMode
    {
        OnPlane,

        OnSurface
    }

    /// <summary>
    /// What time is it Mr. Wolf.
    /// </summary>
    public enum TimeOfDay
    {
        Morning,
        Day,
        Evening,
        Night
    }

    public enum vxSandboxFileLoadError
    {
        None = 0,
        Misc = 1,
        FileNotFound = 2,
        PluginNotFound = 3,
    }

}
