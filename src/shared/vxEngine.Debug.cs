using Microsoft.Xna.Framework;

using VerticesEngine.Utilities;
using VerticesEngine.Net;

using System;
using VerticesEngine;
using System.Collections.Generic;
using VerticesEngine.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Graphics;
using System.Diagnostics;

namespace VerticesEngine
{
    public sealed partial class vxEngine// : DrawableGameComponent
    {

        internal void RegisterCommand(string command, string description, DebugCommandExecute callback)
        {
            vxDebug.CommandUI.RegisterCommand(command, description, callback);
        }

        /// <summary>
        /// Applies any command line arguments in the .
        /// </summary>
        private void ApplyCommandLineArgs()
        {
            //Get Command Line Arguments that have been passed to the main game
            string[] args = System.Environment.GetCommandLineArgs();

            string argoutput = "Applying Command Line Arguments: ";

            for (int argIndex = 1; argIndex < args.Length; argIndex++)
            {
                argoutput += args[argIndex] + " ";
            }

            //Parse Command Line Arguments Here
            for (int argIndex = 0; argIndex < args.Length; argIndex++)
            {

                try
                {
                    TryToApplyCMDArg(args, argIndex);
                }
                catch (Exception ex)
                {
                    vxConsole.WriteLine(string.Format(" >> cmd line error: {0} <<", ex.Message));
                }

            }
        }

        /// <summary>
        /// Holds all process started by this game
        /// </summary>
        List<Process> processes = new List<Process>();

        /// <summary>
        /// Tries to apply CMD argument.
        /// </summary>
        /// <param name="args">Arguments.</param>
        /// <param name="argIndex">Argument index.</param>
        private void TryToApplyCMDArg(string[] args, int argIndex)
        {
            //Get Argument
            string arg = args[argIndex];

            switch (arg)
            {

                //Sets the Build Conifg too Debug. This for debugging release builds in the wild
                case "-dev":
                case "-debug":
                case "-console":
                    SetBuildType(vxBuildType.Debug);
                    break;

                //Set Resolution Width
                case "-w":
                case "-width":
                    vxDebug.CommandUI.ExecuteCommand(string.Format("width {0}", args[argIndex + 1]));
                    break;
                //Set Resolution Height
                case "-h":
                case "-height":
                    vxDebug.CommandUI.ExecuteCommand(string.Format("height {0}", args[argIndex + 1]));
                    break;

#if !__MOBILE__
                case "-netdev":

                    string newArgs = "";
                    foreach(var sysArg in System.Environment.GetCommandLineArgs())
                    {
                        if(sysArg != "-netdev")
                        {
                            newArgs += " " + sysArg;
                        }
                    }

                    newArgs += "-wpos 256 128";

                    if (vxEngine.PlatformOS == vxPlatformOS.Windows)
                    {                        
                        var proc = System.Diagnostics.Process.Start(Game.GetProcessName() + ".exe", newArgs);
                        processes.Add(proc);
                    }
                    else
                    {
                        var proc = System.Diagnostics.Process.Start(Game.GetProcessName(), newArgs);
                        processes.Add(proc);
                    }

                    _game.Window.Position = new Point(1800, 128);
                    _game.Window.Title += "NET DEV PLAYER 1 - ";

                    break;
#endif

                case "-wpos":
#if !__MOBILE__
                    _game.Window.Position = new Point(int.Parse(args[argIndex + 1]), int.Parse(args[argIndex + 2]));
#endif
                    break;
                case "-sw":
                case "-startwindowed":
                case "-window":
                case "-windowed":
                    vxDebug.CommandUI.ExecuteCommand(string.Format("windowed"));
                    break;

                //Set Fullsreen
                case "-full":
                case "-fullscreen":
                    vxDebug.CommandUI.ExecuteCommand(string.Format("fullscreen"));
                    break;

                case "-fps":
                    vxDebug.CommandUI.ExecuteCommand(string.Format("fps"));
                    break;
                case "-tr":
                    vxDebug.CommandUI.ExecuteCommand(string.Format("tr"));
                    break;

                case "-v":
                    vxDebug.IsDebugOutputVerbose = true;
                    break;

                // only load mods if it's in debug/dev mode
                case "-mod":
                    if (BuildType == vxBuildType.Debug)
                    {
                        try
                        {
                            DevModPaths.Add(args[argIndex + 1]);
                            //PluginManager.LoadAssembly(args[argIndex + 1]);
                        }
                        catch (Exception ex)
                        {
                            vxConsole.WriteException(this, ex);
                        }
                    }
                    break;
            }
        }

        List<string> DevModPaths = new List<string>();
        internal string[] LoadDevMods()
        {
            //DevModPaths.Add(@"C:\Users\rtroe\Documents\GitHub\chaoticworkshop-modkit\ChaoticWorkshopModKit\bin\Debug\ChaoticWorkshop.Mods.Template.dll");
            return DevModPaths.ToArray();
        }

        public void DrawDebugFlag()
        {
            //enginevtext += "\n"+GameName + " v." + GameVersion;
            //enginevtext += "\nVertices Engine v." + EngineVersion + " - (" + PlatformType + ")";
            if (DevModPaths.Count > 0)
            {
                SpriteFont engineFont = vxInternalAssets.Fonts.ViewerFont;

                // Engine Version Text
                string enginevtext = _game.Name + " - Dev Mode\n========================\n";

                enginevtext += "Loaded Development DLL's:\n";

                foreach (var dll in DevModPaths)
                    enginevtext += "  " + dll + "\n";

                // Text Size + Padding and offset from screen edges
                Vector2 EngineTextSize = engineFont.MeasureString(enginevtext);
                int padding = 5;

                vxGraphics.SpriteBatch.DrawString(vxInternalAssets.Fonts.ViewerFont,
                                       enginevtext,
                                       (padding + 1) * Vector2.One,
                                       Color.Black);

                vxGraphics.SpriteBatch.DrawString(vxInternalAssets.Fonts.ViewerFont,
                                       enginevtext,
                                       (padding) * Vector2.One,
                                       Color.Lime);
            }
        }

        /// <summary>
        /// Draws the version info in the corner of the screen.
        /// </summary>
        /// <param name="color">Color to draw the text with.</param>
        /// <param name="alpha">Alpha to draw with, usually the TransitionAlpha value for the current screen.</param>
        public void DrawVersionInfo(Color color, float alpha)
        {
            float engineTextAlpha = 0.75f;

            SpriteFont engineFont = vxInternalAssets.Fonts.ViewerFont;

			// Engine Version Text
			string enginevtext = string.Format("[{0} {1} Build] - v. {2} - Engine v." + EngineVersion, PlatformOS, _buildConfigType, _game.Version);

			// Text Size + Padding and offset from screen edges
			Vector2 EngineTextSize = engineFont.MeasureString(enginevtext);
			int padding = 5;
			int offset = 5;

			// Set the text position
			Vector2 EngineTextPosition = new Vector2(offset + padding, vxGraphics.GraphicsDevice.Viewport.Height - EngineTextSize.Y - (offset + padding));

			vxGraphics.SpriteBatch.DrawString(engineFont,
								   enginevtext,
								   EngineTextPosition,
								   color * engineTextAlpha * alpha);
		}


         void DrawIPContent(Color color, float alpha)
        {
            SpriteFont engineFont = vxInternalAssets.Fonts.ViewerFont;

            // Engine Version Text
            string enginevtext = "Engine v." + EngineVersion + " - (" + PlatformOS + ")";

            // Text Size + Padding and offset from screen edges
            Vector2 EngineTextSize = engineFont.MeasureString(enginevtext);
            int padding = 5;
            int offset = 5;

            // Set the text position
            Vector2 EngineTextPosition = new Vector2(offset + padding, vxGraphics.GraphicsDevice.Viewport.Height - EngineTextSize.Y - (offset + padding));

            vxGraphics.SpriteBatch.DrawString(vxInternalAssets.Fonts.ViewerFont, enginevtext, EngineTextPosition, color * 0.75f * alpha);
        }
	}
}
