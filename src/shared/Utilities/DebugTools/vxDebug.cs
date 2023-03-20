using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using VerticesEngine.Diagnostics;
using VerticesEngine.Graphics;
using VerticesEngine.Utilities;

namespace VerticesEngine
{

    public static partial class vxDebug
    {
        /// <summary>
        /// Initializes the renderer.
        /// </summary>
        /// <param name="graphicsDevice">The GraphicsDevice to use for rendering.</param>
        public static void Init()
        {
            try
            {
                // If we already have a graphics device, we've already initialized once. We don't allow that.
                if (graphics != null)
                    throw new InvalidOperationException("Initialize can only be called once.");

                // Save the graphics device
                graphics = vxGraphics.GraphicsDevice;

                // Create and initialize our effect
                effect = new BasicEffect(vxGraphics.GraphicsDevice);
                effect.VertexColorEnabled = true;
                effect.TextureEnabled = false;
                effect.DiffuseColor = Vector3.One;
                effect.World = Matrix.Identity;

                // Create our unit sphere vertices
                InitializeSphere();

                InitialiseTool();

                // Get all Engine Methods
                vxDebug.RetreiveDebugMethods(Assembly.GetExecutingAssembly());

                // Get all Game Methods
                vxDebug.RetreiveDebugMethods(Assembly.GetEntryAssembly());
            }
            catch (Exception e) {
                vxConsole.WriteException("vxDebug", e);
            }

        }

        /// <summary>
        /// Gets all static methods which have the 'vxDebugMethodAttribute' flag and registers them with the Debug UI
        /// </summary>
        /// <param name="assembly"></param>
        public static void RetreiveDebugMethods(Assembly assembly)
        {
            // gets the methods with the 'DebugMethod' Attribute
            if (assembly != null)
            {
                MethodInfo[] methods = assembly.GetTypes().SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
                         .Where(m => m.GetCustomAttributes(typeof(vxDebugMethodAttribute), false).Length > 0)
                         .ToArray();

                // loop through all methods and register them
                foreach (var method in methods)
                {
                    vxDebugMethodAttribute debugAttribute = method.GetCustomAttribute<vxDebugMethodAttribute>();

                    vxDebug.CommandUI.RegisterCommand(
                    debugAttribute.cmd,              // Name of command
                    debugAttribute.description,     // Description of command
                    delegate (IDebugCommandHost host, string command, IList<string> args)
                    {
                    // find the appropriate method to invoke
                    ParameterInfo[] parameterInfo = method.GetParameters();
                        if (parameterInfo.Length == 0)
                        {
                        // call a method with no parameters
                        method.Invoke(null, null);
                        }
                        else
                        {
                            if (parameterInfo[0].ParameterType == typeof(string[]))
                            {
                            // pass just the arguments through
                            method.Invoke(null, new object[] { args.ToArray() });
                            }
                            else if (parameterInfo[0].ParameterType == typeof(vxEngine))
                            {
                                if (parameterInfo.Length > 1 && parameterInfo[1].ParameterType == typeof(string[]))
                                {
                                // pass the engine reference and arguments through
                                method.Invoke(null, new object[] { vxEngine.Instance, args.ToArray() });
                                }
                                else
                                {
                                // pass just the engine reference through
                                method.Invoke(null, new object[] { vxEngine.Instance });
                                }
                            }
                            else
                            {
                                vxConsole.WriteError("Could Not Find Appropriate Method to Invoke for Command '" + command + "'");
                            }
                        }
                    });
                }
            }
        }




        public static void DumpError(Exception exception, string errorInfo, int depth = 0)
        {
            string prefix = new string(' ', depth * 5);
            Console.WriteLine(prefix + "----------------------------------------------");
            Console.WriteLine(prefix + "Error Dump: " + errorInfo);
            Console.WriteLine(prefix + "----------------------------------------------");
            if (exception != null)
            {
                Console.WriteLine(prefix + "Message:      " + exception.Message);
                Console.WriteLine(prefix + "Target Site: " + exception.TargetSite);
                Console.WriteLine(prefix + "Source:      " + exception.Source);
                Console.WriteLine(prefix + "Data:        " + exception.Data);

                Console.WriteLine(prefix + "Stack Trace:");
                Console.WriteLine(exception.StackTrace);

                if (exception.InnerException != null)
                {
                    DumpError(exception.InnerException, "Inner Exception", depth + 1);
                }
            }
            Console.WriteLine("----------------------------------------------");
        }
    }
}