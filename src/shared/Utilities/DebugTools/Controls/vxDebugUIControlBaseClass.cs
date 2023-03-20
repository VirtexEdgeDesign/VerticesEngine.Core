using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VerticesEngine.Diagnostics;
using VerticesEngine.Utilities;

namespace VerticesEngine.Diagnostics
{
    /// <summary>
    /// Base UI Control class for creating Debug Controls. Inherit this class and add the 'vxDebugTool' attribute
    /// </summary>
    public class vxDebugUIControlBaseClass : vxGameObject
    {
        public string DebugToolName { get; private set; }

        public vxDebugUIControlBaseClass(string toolName) 
        {
            DebugToolName = toolName;


            AddArgument("-help", "help documentation for the '" + DebugToolName + "' command", delegate
            {
                Echo("");
                Echo(DebugToolName + " - help ");
                Echo("=================================================");
                Echo("Command");
                int cmdlen = GetCommand().Length;
                Echo(String.Format("     {0}" + new String(' ', 15 - cmdlen) + "{1}", GetCommand(), GetDescription()));
                Echo("");
                Echo("Options");
                foreach (var arg in arguments)
                {
                    cmdlen = arg.Key.Length;
                    Echo(String.Format("     {0}" + new String(' ', 15 - cmdlen) + "{1}", arg.Key, arg.Value.description));

                }
                Echo("");
            });
        }

        void Echo(string text)
        {
            vxConsole.Echo(text);
        }

        public virtual bool RegisterCommand()
        {
            return true;
        }


        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <returns>The command.</returns>
        public virtual string GetCommand()
        {
            throw new Exception();
        }



        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <returns>The description.</returns>
        public virtual string GetDescription()
        {
            throw new Exception();
        }

        Dictionary<string, CommandInfo> arguments = new Dictionary<string, CommandInfo>();

        protected virtual void AddArgument(string arg, string description, DebugCommandExecute callback)
        {
            arguments.Add(arg, new CommandInfo(arg, description, callback));
        }

        /// <summary>
        /// Called when the main 'command' linked to this debug tool is called.
        /// </summary>
        /// <param name="args">Arguments.</param>
        public virtual void CommandExecute(IDebugCommandHost host,
                                           string command,
                                           IList<string> args)
        {
            foreach (var arg in args)
            {
                if (arguments.ContainsKey(arg))
                {
                    arguments[arg].callback.Invoke(host, command, args);
                }
            }
        }


        protected internal virtual void Update() { }


        protected internal virtual void Draw() { }
    }

}
