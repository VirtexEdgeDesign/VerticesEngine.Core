#region File Description
//-----------------------------------------------------------------------------
// DebugSystem.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
using System.Collections.Generic;
using System;
using VerticesEngine.Utilities;


#endregion

/*
 * To get started with the GameDebugTools, go to your main game class, override the Initialize method and add the
 * following line of code:
 * 
 * GameDebugTools.DebugSystem.Initialize(this, "MyFont");
 * 
 * where "MyFont" is the name a SpriteFont in your content project. This method will initialize all of the debug
 * tools and add the necessary components to your game. To begin instrumenting your game, add the following line of 
 * code to the top of your Update method:
 *
 * GameDebugTools.DebugSystem.Instance.TimeRuler.StartFrame()
 * 
 * Once you have that in place, you can add markers throughout your game by surrounding your code with BeginMark and
 * EndMark calls of the TimeRuler. For example:
 * 
 * GameDebugTools.DebugSystem.Instance.TimeRuler.BeginMark("SomeCode", Color.Blue);
 * // Your code goes here
 * GameDebugTools.DebugSystem.Instance.TimeRuler.EndMark("SomeCode");
 * 
 * Then you can display these results by setting the Visible property of the TimeRuler to true. This will give you a
 * visual display you can use to profile your game for optimizations.
 *
 * The GameDebugTools also come with an FpsCounter and a DebugCommandUI, which allows you to type commands at runtime
 * and toggle the various displays as well as registering your own commands that enable you to alter your game without
 * having to restart.
 */

using Microsoft.Xna.Framework;
using VerticesEngine.UI.Dialogs.Utilities;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine;
using System.Reflection;
using System.Linq;
using VerticesEngine.Graphics;
using VerticesEngine.Diagnostics;

namespace VerticesEngine
{

    /// <summary>
    /// DebugSystem is a helper class that streamlines the creation of the various GameDebug
    /// pieces. While games are free to add only the pieces they care about, DebugSystem allows
    /// games to quickly create and add all the components by calling the Initialize method.
    /// </summary>
	public partial class vxDebug
    {
        /// <summary>
        /// Is the Debug Mesh Visible
        /// </summary>
        [vxEngineSettingsAttribute("IsDebugMeshVisible", "", true, true)]
        public static bool IsDebugMeshVisible { get; set; }

        /// <summary>
        /// Is the Debug Render Targets Visible
        /// </summary>
        [vxEngineSettingsAttribute("IsDebugRenderTargetsVisible", "", true, true)]
        public static bool IsDebugRenderTargetsVisible { get; set; }

        /// <summary>
        /// Game Status Console is a list of constant updating info
        /// </summary>
        public static bool IsGameStatusConsoleVisible = false;

        /// <summary>
        /// Is output Verbose
        /// </summary>
        //public static bool IsVerbose = false;

        /// <summary>
        /// Gets the DebugCommandUI for the system.
        /// </summary>
        public static vxDebugCommandUI CommandUI { get; private set; }
        

		/// <summary>
		/// Is debug output verbose. This can be set by running the app with the '-v' flag.
		/// </summary>
		public static bool IsDebugOutputVerbose = false;
        
		internal static List<vxDebugUIControlBaseClass> DebugTools = new List<vxDebugUIControlBaseClass>();

		public static void AddDebugTool(vxDebugUIControlBaseClass tool)
		{
			DebugTools.Add(tool);

            vxDebug.LogEngine(new
            {
                name = tool.DebugToolName,
                cmd = tool.GetCommand(),
                desc = tool.GetDescription(),
            });

            if (tool.RegisterCommand())
            {
                CommandUI.RegisterCommand(
                    tool.GetCommand(),              // Name of command
                    tool.GetDescription(),     // Description of command
                   delegate (IDebugCommandHost host, string command, IList<string> args)
                   {
                       tool.CommandExecute(host, command, args);
                   });
            }
		}

        public static vxDebugUIControlBaseClass GetDebugControl(Type type)
        {
            foreach(var debugTool in DebugTools)
            {
                if (type == debugTool.GetType())
                    return debugTool;
            }
            return null;
        }


        public static void InitialiseTool()
		{
            // Create all of the system components
			CommandUI = new vxDebugCommandUI();

            vxDebug.LogEngine("Loading Tools");
            AddDebugTool(CommandUI);

            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.GetCustomAttributes(typeof(vxDebugControlAttribute), true).Length > 0)
                { 
                    AddDebugTool((vxDebugUIControlBaseClass)Activator.CreateInstance(type));
                }
            }

            


            //Setup Basic Commands
            // Register's Command to Show Render Targets on the Screen
            /*****************************************************************************************************/
            CommandUI.RegisterCommand (
				"rt",              // Name of command
				"Toggle Viewing Individual Render Targets.",     // Description of command
				delegate (IDebugCommandHost host, string command, IList<string> args) {
                    vxDebug.IsDebugRenderTargetsVisible = !vxDebug.IsDebugRenderTargetsVisible;
				});

            // Register's Command to Show Render Targets on the Screen
            /*****************************************************************************************************/
            CommandUI.RegisterCommand (
				"dmesh",              // Name of command
				"Toggles Debug Mesh's",     // Description of command
				delegate (IDebugCommandHost host, string command, IList<string> args) {
                    vxDebug.IsDebugMeshVisible = !vxDebug.IsDebugMeshVisible;
                });



			// Register's Command to Show Render Targets on the Screen
			/*****************************************************************************************************/
			//CommandUI.RegisterCommand (
			//	"gcon",              // Name of command
			//	"Toggle the in-game console which won't pause the game (Different than this console)",     // Description of command
			//	delegate (IDebugCommandHost host, string command, IList<string> args) {
   //                 vxDebug.IsGameStatusConsoleVisible = !vxDebug.IsGameStatusConsoleVisible;
   //             });





			// Set Resolution Width
			/*****************************************************************************************************/
			CommandUI.RegisterCommand (
				"width",              // Name of command
				"Set's Resoultion Width. (Example: -width 1280)",     // Description of command
				delegate (IDebugCommandHost host, string command, IList<string> args) {
                    //Engine.Settings.Graphics.Screen.SetResolutionX(Convert.ToInt32(args[0]));
                    vxScreen.SetResolution(Convert.ToInt32(args[0]), vxScreen.Height);
                host.Echo("Width Setting Set. Call 'graref' to apply");
				});


			// Set Resolution Height
			/*****************************************************************************************************/
			CommandUI.RegisterCommand (
				"height" ,              // Name of command
				"Set's Resoultion Height. (Example: -width 720)",     // Description of command
				delegate (IDebugCommandHost host, string command, IList<string> args) {
                    //Engine.Settings.Graphics.Screen.SetResolutionY(Convert.ToInt32(args[0]));
                    vxScreen.SetResolution(vxScreen.Width, Convert.ToInt32(args[0]));
                    host.Echo("Height Setting Set. Call 'graref' to apply");
				});


			// Set Windowed Mode
			/*****************************************************************************************************/
			CommandUI.RegisterCommand (
				"windowed",              // Name of command
				"Forces the Game to start in Windowed Mode.",     // Description of command
				delegate (IDebugCommandHost host, string command, IList<string> args) {
                    //vxScreen.IsFullScreen = false;
                    vxScreen.FullScreenMode = vxFullScreenMode.Windowed;
                    host.Echo("Window Setting Set. Call 'graref' to apply");
				});


			// Set Windowed Mode
			/*****************************************************************************************************/
			CommandUI.RegisterCommand (
				"fullscreen",              // Name of command
				"Forces the Game to start in Fullscreen Mode.",     // Description of command
				delegate (IDebugCommandHost host, string command, IList<string> args) {
                    //Engine.Settings.Graphics.Screen.IsFullScreen = true;
                    // vxScreen.IsFullScreen = true;
                    vxScreen.FullScreenMode = vxFullScreenMode.Fullscreen;
                    host.Echo("Full Screen Setting Set. Call 'graref' to apply");
				});

        }

		internal static void UpdateTools()
		{
			foreach(var t in DebugTools)
				t.Update();
		}

        /// <summary>
        /// Draws all Debug Tools and Overlayrs
        /// </summary>
        /// <param name="gameTime"></param>
		internal static void Draw()
        {
            foreach (var t in DebugTools)
			{
				if(t.IsVisible)
					t.Draw();
			}

        }
    }
}
