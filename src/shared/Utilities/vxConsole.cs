using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine;
using System.Diagnostics;
using VerticesEngine.Diagnostics;
using VerticesEngine.Utilities;
using VerticesEngine.Graphics;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace VerticesEngine
{
    /// <summary>
    /// Console Utility which provides output too both the in-game console window as well as 
    /// the system console if available.
    /// </summary>
    public class vxConsole
    {
        internal struct ScreenDebugLine
        {
            public string text;
            public Color color;
        }

        /// <summary>
        /// The collection of Debug Strings.
        /// </summary>
        internal static List<ScreenDebugLine> InGameDebugLines = new List<ScreenDebugLine>(1024);
    
        internal static long CurrentUpdateTick = 0;


        /// <summary>
        /// Initialises the vxConsole Static Object.
        /// </summary>
        /// <param name="engine">Engine.</param>
        internal static void Init()
        {
            //This is just temporary, this is re-loaded for global uses when the vxEngine is Initialised.
            string gameVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();


#if !__IOS__ && !__ANDROID__
            try
            {
                Console.Title = "VIRTICES ENGINE DEBUG CONSOLE v." + gameVersion;
            }
            catch
            {
            }
#endif

            string backend = "COMPILER FLAG NOT FOUND";
            //There's only two choices for a backend, XNA or MonoGame. The entire code base will be eventually
            //be moved over ONLY too MonoGame as XNA is no longer supported.
#if VRTC_PLTFRM_XNA
			backend = "XNA";
#elif VRTC_PLTFRM_DX
			backend = "MonoGame [DirectX]";
#elif VRTC_PLTFRM_GL
            backend = "MonoGame [OpenGL]";
#elif __IOS__ || __ANDROID__
			backend = "MonoGame [Android]";
#elif __IOS__
			backend = "MonoGame [iOS]";
#endif

            backend = string.Format($"{vxEngine.ReleasePlatformType} - {vxEngine.GraphicalBackend}");

            // Set Build Tag info
            string EngineBuildType = "Engine Build Flags: ";

#if DEBUG
            EngineBuildType += "-Debug ";
#else
			EngineBuildType += "-Release ";
#endif



            InternalWriteLine("____   ____             __  .__                     ");
            InternalWriteLine("\\   \\ /   /____________/  |_|__| ____  ____   ______");
            InternalWriteLine(" \\   Y   // __ \\_  __ \\   __\\  |/ ___\\/ __ \\ /  ___/");
            InternalWriteLine("  \\     /\\  ___/|  | \\/|  | |  \\  \\__\\  ___/ \\___ \\ ");
            InternalWriteLine("   \\___/  \\___  >__|   |__| |__|\\___  >___  >____  >");
            InternalWriteLine("              \\/                    \\/    \\/     \\/ ");
            InternalWriteLine("VERTICES ENGINE - (C) VIRTEX EDGE DESIGN");
            InternalWriteLine("///////////////////////////////////////////////////////////////////////");
            InternalWriteLine(string.Format("Game Name:          {0}", vxEngine.Game.Name));
            InternalWriteLine(string.Format("Game Version:       {0}", vxEngine.Game.Version));
            InternalWriteLine(string.Format("Graphical Backend:  {0}", backend));
            InternalWriteLine(string.Format("Engine Version      v.{0}", vxEngine.EngineVersion));
            InternalWriteLine(EngineBuildType);

            fileSafeGameName = vxEngine.Game.Name.ToLower();
            fileSafeGameName = Regex.Replace(fileSafeGameName, @"\s+", "");


            InternalWriteLine(string.Format("Cmd Line Args:      {0}", vxEngine.Instance.CMDLineArgsToString()));

            InternalWriteLine("System Info");
            // now loop through all system info
            foreach(var stat in vxSystemInfo.Stats)
            {
                try
                {
                    InternalWriteLine($"     {stat.Key}={stat.Value}");
                }
                catch { }
            }
            InternalWriteLine("///////////////////////////////////////////////////////////////////////");

        }

        static string fileSafeGameName = "VerticesGame";

        [Diagnostics.vxDebugMethod("dumplogs", "Dumps all logs.")]
        static void DumpLogFromCMD(vxEngine Engine, string[] args)
        {
            DumpLogsOnClose = true;
            DumpLog();
            DumpLogsOnClose = false;
        }

        [Diagnostics.vxDebugMethod("testcrash", "Test Crash")]
        static void TestCrash(vxEngine Engine, string[] args)
        {
            throw new Exception("Test Exception");
        }

        /// <summary>
        /// Writes a debug line which is outputed to both the engine debug window and the system console.
        /// </summary>
        /// <remarks>If debug information is needed, this method is useful for outputing any object.ToString() value 
        /// too both the in-engine debug window as well as the system console if it is available.</remarks>
        /// <param name="output">The object too be outputed in the console.</param>
        /// <example> 
        /// This sample shows how to call the <see cref="WriteLine"/> method.
        /// <code>
        /// vxConsole.WriteLine("Output of Foo is: " + foo.Output.ToString());
        /// </code>
        /// </example>
        /// <example> 
        /// This sample shows how to call the <see cref="WriteLine"/> method with different variable types as inputs.
        /// <code>
        /// vxConsole.WriteLine(string.Format("X,Y,Z Max: {0}, {1}, {2}", Level_X_Max, Level_Y_Max, Level_Z_Max));
        /// </code>
        /// </example>
        public static void WriteLine(object output)
        {
            WriteLine(output, ConsoleColor.Green);
        }

        public static void WriteLine(object output, ConsoleColor consoleColor)
        {
            WriteLine(DebugCommandMessage.Info, output, consoleColor);
        }

        public static void WriteDLCLine(object output)
        {
            WriteLine(DebugCommandMessage.PlugIn, output, ConsoleColor.White);
        }

        public static void WriteIODebug(object output)
        {
            WriteLine(DebugCommandMessage.IO, output, ConsoleColor.DarkGray);
        }

        public static void WriteSettings(object output)
        {
            WriteLine(DebugCommandMessage.IO, output, ConsoleColor.DarkCyan);
        }

        internal static void InternalWriteLine(object output)
        {
            WriteLine(DebugCommandMessage.Engine, output, ConsoleColor.Magenta);
        }

        public static void WriteNetworkLine(object output)
        {
            WriteLine(DebugCommandMessage.Net, output, ConsoleColor.Cyan);
        }

        public static void WriteMonLine(object output)
        {
            WriteLine(DebugCommandMessage.MON, output, ConsoleColor.Cyan);
        }

        /// <summary>
        /// Logs a network message specifiying whether this instance is a server or client
        /// </summary>
        /// <param name="text"></param>
        public static void NetLog(string text)
        {
            NetLog(text, Net.vxNetworkManager.PlayerNetworkRole == Net.vxEnumNetworkPlayerRole.Client ? ConsoleColor.Yellow : ConsoleColor.DarkCyan);
        }

        /// <summary>
        /// Net logs with a given colour
        /// </summary>
        /// <param name="text"></param>
        /// <param name="colour"></param>
        public static void NetLog(string text, ConsoleColor colour)
        {
            WriteLine(DebugCommandMessage.Net, $"[{Net.vxNetworkManager.NetID}]:{text}", colour);
        }


        /// <summary>
        /// Writes out a warning to the debug and system console.
        /// </summary>
        /// <param name="SourceFile">Source file where the warning is being sent from. Helpful for tracking where warning's 
        /// are being generated. </param>
        /// <param name="output">The object holding the warning data too be outputed in the console.</param>
        /// <example> 
        /// This sample shows how to call the <see cref="WriteWarning"/> method.
        /// <code>
        ///     try
        ///     {
        ///         foo.bar();
        ///     }
        ///     catch(Exception ex)
        ///     {
        ///         vxConsole.WriteWarning(this.ToString(), ex.Message);
        ///     }
        /// </code>
        /// </example>
        public static void WriteWarning(string SourceFile, string output)
        {
            WriteLine(DebugCommandMessage.Warn, SourceFile + "':" + output, ConsoleColor.Yellow);
        }

        public static void WriteError(string text)
        {
            WriteLine(DebugCommandMessage.Error, text, ConsoleColor.Red);
        }

        internal static List<string> Lines
        {
            get { return m_lines; }
        }
        static List<string> m_lines = new List<string>();

        public static void WriteLine(DebugCommandMessage messageType, object output, ConsoleColor consoleColor)
        {
            var line = "[" + messageType.ToString().ToUpper() + "]: " + output;
            
            m_lines.Add(line);

#if !__IOS__ && !__ANDROID__
            Console.ForegroundColor = consoleColor;
#endif
            Console.WriteLine(line);


            if (vxDebug.CommandUI != null)
            {
                vxDebug.CommandUI.Echo(messageType, line);
            }
        }

        public static void ResetConsoleColour()
        {
#if !__IOS__ && !__ANDROID__
            Console.ResetColor();
#endif
        }

        /// <summary>
        /// Writes out a Verbose Line. This line will be written to the console/output window if the Verbose Property is set too true
        /// </summary>
        /// <param name="output"></param>
        public static void WriteVerboseLine(string output)
        {
            if (vxDebug.IsDebugOutputVerbose)
                WriteLine(DebugCommandMessage.Verbose, output, ConsoleColor.DarkMagenta);

        }

        public static void WriteVerboseLine(string output, params object[] param)
        {
            if (vxDebug.IsDebugOutputVerbose)
            {
                WriteLine(DebugCommandMessage.Verbose, string.Format(output, param), ConsoleColor.DarkMagenta);
            }

        }

        /// <summary>
        /// Writes out a Verbose Line. This line will be written to the console/output window if the Verbose Property is set too true
        /// </summary>
        /// <param name="output"></param>
        /// <param name="consoleColor"></param>
        public static void WriteVerboseLine(string output, ConsoleColor consoleColor)
        {
            if (vxDebug.IsDebugOutputVerbose)
                WriteLine(DebugCommandMessage.Verbose, output, consoleColor);
        }

        /// <summary>
        /// Similar to the <see cref="WriteLine"/> method. This method writes out a line of text which is 
        /// is prefixed with a "Networking" tag to the console output line to help distuignish it against regular 
        /// console output.
        /// </summary>
        /// <param name="output">The object too be outputed in the console.</param>
        /// <example> 
        /// This sample shows how to call the <see cref="WriteNetworkLine"/> method.
        /// <code>
        /// vxConsole.WriteNetworkLine("Ping: " + foo.Ping.ToString());
        /// </code>
        /// </example>



        /// <summary>
        /// Echos the Text to the Console
        /// </summary>
        /// <param name="text"></param>
        public static void Echo(string text)
        {
            Console.WriteLine(text);
            vxDebug.CommandUI.Echo(DebugCommandMessage.IO, "     " + text);
        }

        /// <summary>
        /// Writes out an error the error.
        /// </summary>
        /// <param name="SourceFile">Source file where the error is being sent from. Helpful for tracking where error's 
        /// are being generated. </param>
        /// <param name="output">The object holding the error data too be outputed in the console.</param>
        /// <example> 
        /// This sample shows how to call the <see cref="WriteError"/> method.
        /// <code>
        ///     try
        ///     {
        ///         foo.bar();
        ///     }
        ///     catch(Exception ex)
        ///     {
        ///         vxConsole.WriteError(this.ToString(), ex.Message);
        ///     }
        /// </code>
        /// </example>
        public static void WriteException(object sender, Exception ex)
        {
            WriteException(sender, ex, null);
        }

        public static void WriteException(Exception ex, [System.Runtime.CompilerServices.CallerMemberNameAttribute] string caller ="")
        {
            WriteException(caller, ex, null);
        }

        public static bool IsVerboseErrorLoggingEnabled = false;

        private static int m_errorCount = 0;

        public static Exception lastLoggedException;

        public static void WriteException(object sender, Exception ex, string[] extrInfo)
        {
            lastLoggedException = ex;
            m_errorCount++;
            if (IsVerboseErrorLoggingEnabled == false)
            {
                WriteError("Exception Thrown in '" + sender.ToString() + "': " + ((ex != null) ? ex.Message : ""));

                if (extrInfo != null)
                {
                    WriteError("Extra Info");
                    foreach (string line in extrInfo)
                        WriteError("     " + line);
                }
            }
            else
            {
                WriteError($"************************* START OF ERROR {m_errorCount} *************************");
                WriteError("Exception Thrown:");

                if (ex != null)
                {
                    WriteError("     Error Code: " + ex.HResult);
                    
                    if (ex.TargetSite != null)
                        WriteError("     TargetSite " + ex.TargetSite);
                    if (ex.Source != null)
                        WriteError("     Source " + ex.Source);

                    if (extrInfo != null)
                    {
                        WriteError("Extra Info");
                        foreach (string line in extrInfo)
                            WriteError("     " + line);
                    }

                    if (ex.Message != null)
                    {
                        // Write 
                        WriteError("Message");
                        foreach (string line in ex.Message.Split(new char[] { '\n' }))
                            WriteError("     " + line);

                        // Write Inner Exception

                        WriteError("Inner Exception");
                        if (ex.InnerException != null && ex.InnerException.Message != null)
                            foreach (string line in ex.InnerException.Message.Split(new char[] { '\n' }))
                                WriteError("     " + line);
                        else
                            WriteError("null");
                    }

                    // Write Stack Trace
                    if (ex.StackTrace != null)
                    {
                        WriteError("StackTrace");
                        foreach (string line in ex.StackTrace.Split(new char[] { '\n' }))
                            WriteError("     " + line);
                    }
                }
                WriteError($"************************* END OF ERROR {m_errorCount} *************************");
            }
        }



        public static bool DumpLogsOnClose = false;

        static int fileWriteCount = 0;
        public static void DumpLog()
        {
            if (DumpLogsOnClose || vxSettings.IsConsoleSavedOnExit)
            {
                string filename = $"{fileSafeGameName}-log-{DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss")}-{(DumpLogsOnClose ? "crash" : string.Empty)}-{fileWriteCount++}.txt";
                string filepath = Path.Combine(vxIO.LogDirectory, filename);
                try
                {

                    Console.WriteLine($"Saving log file to {filepath}");

                    using (var writer = new StreamWriter(filepath))
                    {
                        foreach (var line in m_lines)
                        {
                            writer.WriteLine(line);
                        }
                    }

                    try
                    {
                        MsgBoxResult result = NativeMessageBox.MsgBox(
                            $"{vxEngine.Game.Name} has hit an unrecoverable error. Please contact Virtex Edge on Discord or on the Steam Discussion boards.\n\nA log file has saved to:\n{filepath}",
                            $"// {vxEngine.Game.Name} - Error", MsgBoxStyle.Critical);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                catch (Exception ex)
                {
                    string message = "Metric has hit an unrecoverable error and also could not save a crash log file to disc.";

                    message += "\nPlease screenshot this error dialog and share to the Steam Discussion boards or on Discord.";

                    if (ex != null)
                    {
                        if (ex.Message != null)
                            message += "\n\nError Message: \n     " + ex.Message;

                        if (ex.StackTrace != null)
                        {
                            message += "\n\nStackTrace";
                            foreach (string line in ex.StackTrace.Split(new char[] { '\n' }))
                                message += "\n     " + line;
                        }
                    }
                    else
                    {
                        message += "";
                    }


                    if (lastLoggedException != null)
                    {
                        message += "\n\n\n======= ERROR CAUSING CRASH ==========";


                        if (lastLoggedException.Message != null)
                            message += "\n\nError Message: \n     " + lastLoggedException.Message;

                        if (lastLoggedException.StackTrace != null)
                        {
                            message += "\n\nStackTrace";
                            foreach (string line in lastLoggedException.StackTrace.Split(new char[] { '\n' }))
                                message += "\n     " + line;
                        }

                        message += "\n============= END ====================";


                        try
                        {
                            MsgBoxResult result = NativeMessageBox.MsgBox(message,
                                "// Metric Racer - Error", MsgBoxStyle.Exclamation);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This dumps an exception
        /// </summary>
        /// <param name="ex"></param>
        public static void DumpException(Exception ex)
        {
            try
            {
                string filename = $"HARDCRASH-metricracer-{DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss")}.txt";
                string filepath = Path.Combine(vxIO.LogDirectory, filename);
                
                // make sure it exsits
                vxIO.EnsureDirExists(vxIO.LogDirectory);

                Console.WriteLine($"Saving log file to {filepath}");

                using (var writer = new StreamWriter(filepath))
                {
                    writer.WriteLine($"************************* MAJOR EXCEPTION *************************");
                    writer.WriteLine("Exception Thrown:");

                    if (ex != null)
                    {
                        writer.WriteLine("     Error Code: " + ex.HResult);
                        writer.WriteLine("     TargetSite " + ex.TargetSite);
                        writer.WriteLine("     Source " + ex.Source);


                        // Write 
                        writer.WriteLine("Message");
                        foreach (string line in ex.Message.Split(new char[] { '\n' }))
                            writer.WriteLine("     " + line);

                        // Write Inner Exception

                        writer.WriteLine("Inner Exception");
                        if (ex.InnerException != null)
                            foreach (string line in ex.InnerException.Message.Split(new char[] { '\n' }))
                                writer.WriteLine("     " + line);
                        else
                            writer.WriteLine("null");


                        // Write Stack Trace
                        if (ex.StackTrace != null)
                        {
                            writer.WriteLine("StackTrace");
                            foreach (string line in ex.StackTrace.Split(new char[] { '\n' }))
                                writer.WriteLine("     " + line);
                        }
                    }
                    writer.WriteLine($"************************* END OF ERROR {m_errorCount} *************************");
                }
            }
            catch
            {
                // worst case we might not be able to save a file
            }
        }




        /// <summary>
        /// Writes to in game debug. Activate the in-game debug window by running the 'cn' command.
        /// </summary>
        /// <remarks>NOTE: This is different than the Engine Debug console.</remarks>
        /// <param name="output">The object holding the error data too be outputed in the console.</param>
        /// <example> 
        /// This sample shows how to call the <see cref="WriteToScreen"/> method.
        /// <code>
        /// vxConsole.WriteToScreen("Player Position: " + foo.Position.ToString());
        /// </code>
        /// </example>
        public static void WriteToScreen(object sender, object output)
        {
            WriteToScreen(sender, output, Color.White);
        }


        /// <summary>
        /// Writes to in game debug. Activate the in-game debug window by running the 'cn' command.
        /// </summary>
        /// <remarks>NOTE: This is different than the Engine Debug console.</remarks>
        /// <param name="output">The object holding the error data too be outputed in the console.</param>
        /// <example> 
        /// This sample shows how to call the <see cref="WriteToScreen"/> method.
        /// <code>
        /// vxConsole.WriteToScreen("Player Position: " + foo.Position.ToString(), Color.White);
        /// </code>
        /// </example>
        public static void WriteToScreen(object sender, object output, Color color)
        {
            if (vxDebug.IsGameStatusConsoleVisible)
            {
                var text = string.Empty;
                if (output != null)
                    text = (sender + " : " + output);
                else
                    text = ("NULL");

                WriteToScreen(text, color);
            }
        }
        public static void WriteToScreen(object text)
        {
            WriteToScreen(text, Color.White);
        }

        public static void WriteToScreen(object text, Color color)
        {
            ScreenDebugLine line = new ScreenDebugLine();
            line.text = text.ToString();
            line.color = color;

            InGameDebugLines.Add(line);
        }

        /// <summary>
        /// Clears the Ingame console ahead of the draw call, this is helpful if you have a lot of information being outputed,
        /// but only need a certain amount.
        /// </summary>
        public static void ClearInGameConsole()
        {
            InGameDebugLines.Clear();
        }
    }
}
