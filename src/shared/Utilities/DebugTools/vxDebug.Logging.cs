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
        /// A generic log message
        /// </summary>
        /// <param name="logObj"></param>
        /// <param name="caller"></param>
        public static void Log(object logObj, [CallerMemberName] string caller = "")
        {
            vxConsole.WriteLine($"{ caller }:{logObj}");
        }

        public static void LogIO(object logObj, [CallerMemberName] string caller = "")
        {
            vxConsole.WriteIODebug($"{ caller }:{logObj}");
        }


        internal static void LogEngine(object logObj, [CallerMemberName] string caller = "")
        {
            vxConsole.InternalWriteLine($"{ caller }:{logObj}");
        }


        /// <summary>
        /// A Network Specific Log message
        /// </summary>
        /// <param name="logObj"></param>
        /// <param name="caller"></param>
        public static void LogNet(object logObj, [CallerMemberName] string caller = "")
        {
            vxConsole.WriteNetworkLine($"{ caller }:{logObj}");
        }


        /// <summary>
        /// Log a warning
        /// </summary>
        /// <param name="logObj"></param>
        /// <param name="caller"></param>
        public static void Warn(object logObj, [CallerMemberName] string caller = "")
        {
            vxConsole.WriteWarning(caller, $"{logObj}");
        }

        /// <summary>
        /// Log an error
        /// </summary>
        /// <param name="logObj"></param>
        /// <param name="caller"></param>
        public static void Error(object logObj, [CallerMemberName] string caller = "")
        {
            vxConsole.WriteError($"{ caller }:{logObj}");
        }
        public static void Exception(Exception ex, [CallerMemberName] string caller = "")
        {
            vxConsole.WriteError($"{ caller }:{ex.Message}");
            vxConsole.WriteException(caller, ex);
        }
    }
}