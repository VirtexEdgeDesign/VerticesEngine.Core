#region File Description
//-----------------------------------------------------------------------------
// DebugCommandUI.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Utilities;
using VerticesEngine.Graphics;

#endregion

namespace VerticesEngine.Diagnostics
{
	class vxDebugText
    {
        public string Text { get; private set; }

        public Color Color{ get; private set; }

        public vxDebugText(string text, Color color)
        {
            Text = text;

            Color = color;
        }
    }


        /// <summary>
        /// CommandInfo class that contains information to run the command.
        /// </summary>
    public class CommandInfo
    {
        public CommandInfo(
            string command, string description, DebugCommandExecute callback)
        {
            this.command = command;
            this.description = description;
            this.callback = callback;
        }

        // command name
        public string command;

        // Description of command.
        public string description;

        // delegate for execute the command.
        public DebugCommandExecute callback;
    }
    /// <summary>
    /// Command Window class for Debug purpose.
    /// </summary>
    /// <remarks>
    /// Debug command UI that runs in the Game.
    /// You can type commands using the keyboard, even on the Xbox
    /// just connect a USB keyboard to it
    /// This works on all 3 platforms (Xbox, Windows, Phone)
    /// 
    /// How to Use:
    /// 1) Add this component to the game.
    /// 2) Register command by RegisterCommand method.
    /// 3) Open/Close Debug window by Tab key.
    /// </remarks>
	public class vxDebugCommandUI  : vxDebugUIControlBaseClass, IDebugCommandHost// DrawableGameComponent, IDebugCommandHost 
    {
        #region Constants

        /// <summary>
        /// Maximum lines that shows in Debug command window.
        /// </summary>
        int MaxLineDisplayCount = 30;

		int MaxLineCount = 500;

		int LineScrollPos = 0;

        /// <summary>
        /// Maximum command history number.
        /// </summary>
        const int MaxCommandHistory = 128;

        /// <summary>
        /// Cursor character.
        /// </summary>
        const string Cursor = "_";

        /// <summary>
        /// Default Prompt string.
        /// </summary>
        public const string DefaultPrompt = "CMD>";

        #endregion

        #region Properties

        /// <summary>
        /// Gets/Sets Prompt string.
        /// </summary>
        public string Prompt;

        /// <summary>
        /// Is it waiting for key inputs?
        /// </summary>
        public bool Focused { get { return state != State.Closed; } }

        #endregion

        #region Fields

        // Command window states.
        enum State
        {
            Closed,
            Opening,
            Opened,
            Closing
        }



        public override bool RegisterCommand()
        {
            return false;
        }

        // Reference to DebugManager.
        //private vxDebugManager debugManager;
        //{
        //    get { return Engine.DebugSystem.DebugManager; }
        //}

        // Current state
        private State state = State.Closed;

        // timer for state transition.
        private float stateTransition;

        // Registered echo listeners.
        List<IDebugEchoListner> listenrs = new List<IDebugEchoListner>();

        // Registered command executioner.
        Stack<IDebugCommandExecutioner> executioners = new Stack<IDebugCommandExecutioner>();

        // Registered commands
        private Dictionary<string, CommandInfo> commandTable =
                                                new Dictionary<string, CommandInfo>();

        // Current command line string and cursor position.
        private string commandLine = String.Empty;
        private int cursorIndex = 0;

        private Queue<vxDebugText> lines = new Queue<vxDebugText>();

        // Command history buffer.
        private List<string> commandHistory = new List<string>();

        // Selecting command history index.
        private int commandHistoryIndex;

        #region variables for keyboard input handling.

        // Previous frame keyboard state.
        private KeyboardState prevKeyState;

        // Key that pressed last frame.
        private Keys pressedKey;

        // Timer for key repeating.
        private float keyRepeatTimer;

        // Key repeat duration in seconds for the first key press.
        private float keyRepeatStartDuration = 0.3f;

        // Key repeat duration in seconds after the first key press.
        private float keyRepeatDuration = 0.03f;

        #endregion

        #endregion

        #region Initialization


		/// <summary>
		/// The toggle console key.
		/// </summary>
		Keys ToggleConsoleKey;

        /// <summary>
        /// Constructor
        /// </summary>
		public vxDebugCommandUI() :  base("Command UI")
        {
            Prompt = DefaultPrompt;

            // Add this instance as a service.
            //vxEngine.CurrentGame.Services.AddService(typeof(IDebugCommandHost), this);

            // Draw the command UI on top of everything
            //DrawOrder = int.MaxValue;

            // Adding default commands.

			ToggleConsoleKey = Keys.OemTilde;

            // Help command displays registered command information.
            RegisterCommand("help", "Show Debug Console Command help",
            delegate(IDebugCommandHost host, string command, IList<string> args)
            {
                int maxLen = 0;
                foreach (CommandInfo cmd in commandTable.Values)
                    maxLen = Math.Max(maxLen, cmd.command.Length);

                //string fmt = String.Format("{{0,-{0}}}    {{1}}", maxLen);
					Echo("");
					Echo("     Vertices Debug Console Help");
					Echo("---------------------------------------------------");

                foreach (CommandInfo cmd in commandTable.Values)
                {
						int cmdlen = cmd.command.Length;
						Echo(String.Format("     {0}"+new String(' ', 15 - cmdlen)+"{1}", cmd.command, cmd.description));
					}
					Echo("");
            });

			// Clear screen command
			RegisterCommand("clear", "Clears the Debug Console Screen",
            delegate(IDebugCommandHost host, string command, IList<string> args)
            {
                //lines.Clear();
					LineScrollPos = lines.Count;
            });

            // Echo command
            RegisterCommand("echo", "Echo a given line of text",
            delegate(IDebugCommandHost host, string command, IList<string> args)
            {
                Echo(command.Substring(5));
            });

            // get the current console lines
            foreach(var line in vxConsole.Lines)
            {
                Echo(DebugCommandMessage.Engine, line);
            }
        }



        #endregion

        #region IDebugCommandHostinterface implemenration

        /// <summary>
        /// Register new command with the Debug Command UI.
        /// </summary>
        /// <param name="command">command name</param>
        /// <param name="description">description of command</param>
        /// <param name="callback">Execute delegation</param>
        public void RegisterCommand(
            string command, string description, DebugCommandExecute callback)
        {
            string lowerCommand = command.ToLower();
            if (commandTable.ContainsKey(lowerCommand))
            {
                throw new InvalidOperationException(
                    String.Format("Command \"{0}\" is already registered.", command));
            }

            commandTable.Add(
                lowerCommand, new CommandInfo(command, description, callback));
        }

		/// <summary>
		/// Register an array of new commands for a specific function with the Debug Command UI.
		/// </summary>
		/// <param name="command">command name</param>
		/// <param name="description">description of command</param>
		/// <param name="callback">Execute delegation</param>
		public void RegisterCommand(
			string[] commands, string description, DebugCommandExecute callback)
		{
			foreach (string command in commands) {
				string lowerCommand = command.ToLower ();
				if (commandTable.ContainsKey (lowerCommand)) {
					throw new InvalidOperationException (
						String.Format ("Command \"{0}\" is already registered.", command));
				}

				commandTable.Add (
					lowerCommand, new CommandInfo (command, description, callback));
			}
		}

		/// <summary>
		/// Unregister new command with the Debug Command UI.
		/// </summary>
		/// <param name="command">command name</param>
        public void UnregisterCommand(string command)
        {
            string lowerCommand = command.ToLower();
            if (!commandTable.ContainsKey(lowerCommand))
            {
                throw new InvalidOperationException(
                    String.Format("Command \"{0}\" is not registered.", command));
            }

            commandTable.Remove(command);
        }

		/// <summary>
		/// Executes command.
		/// </summary>
		/// <param name="command">Command.</param>
        public void ExecuteCommand(string command, bool IsCmdLine = false)
        {
            // Call registered executioner.
            if (executioners.Count != 0)
            {
                executioners.Peek().ExecuteCommand(command);
                return;
            }

            // Run the command.
            char[] spaceChars = new char[] { ' ' };

            Echo(Prompt + command);

            command = command.TrimStart(spaceChars);

            var stringArgs = IsCmdLine ? System.Environment.GetCommandLineArgs() : command.Split(spaceChars);
            
            List<string> args = new List<string>(stringArgs);

                // remove the first
                if (IsCmdLine)
                    args.RemoveAt(0);

            if (args.Count == 0)
                return;

            string cmdText = args[0];
                args.RemoveAt(0);

            CommandInfo cmd;
            if (commandTable.TryGetValue(cmdText.ToLower(), out cmd))
            {
                try
                {
                    // Call registered command delegate.
                    cmd.callback(this, command, args);
                }
                catch (Exception e)
                {
                    // Exception occurred while running command.
                    List<string> data = new List<string>();
                    data.Add("Unhandled Exception occurred executing command");

                    data.Add("command: " + command);
                    data.Add("args: " + args.Count);
                    foreach(var arg in args)
                        data.Add("     " + arg);
                    

                    vxConsole.WriteException(this, e, data.ToArray());
                }
            }
            else
            {
                vxConsole.WriteError(string.Format("Unknown Command '{0}'", command));
            }

            // Add to command history.
            commandHistory.Add(command);
            while (commandHistory.Count > MaxCommandHistory)
                commandHistory.RemoveAt(0);

            commandHistoryIndex = commandHistory.Count;

        }

		/// <summary>
		/// Register message listener.
		/// </summary>
		/// <param name="listner"></param>
        public void RegisterEchoListner(IDebugEchoListner listner)
        {
            listenrs.Add(listner);
        }

		/// <summary>
		/// Unregister message listener.
		/// </summary>
		/// <param name="listner"></param>
        public void UnregisterEchoListner(IDebugEchoListner listner)
        {
            listenrs.Remove(listner);
        }

        Color DefaultColor = Color.Lime;
		/// <summary>
		/// Output message.
		/// </summary>
		/// <param name="messageType">type of message</param>
		/// <param name="text">message text</param>
        public void Echo(DebugCommandMessage messageType, string text)
        {
            Color color = DefaultColor;
            switch(messageType)
            {
                case DebugCommandMessage.Error:
                    color = Color.Red;
                    //text = messageType + ": " + text;
                    break;
                case DebugCommandMessage.Warn:
                    color = Color.Yellow;
                    //text = messageType + ": " + text;
                    break;
                case DebugCommandMessage.Net:
                    color = Color.DeepSkyBlue;
                    //text = messageType+": " + text;
                    break;
                case DebugCommandMessage.IO:
                    color = Color.White;
                    //text = messageType + ": " + text;
                    break;
                case DebugCommandMessage.Engine:
                    color = Color.DarkCyan;
                    //text = messageType + ": " + text;
                    break;
                case DebugCommandMessage.Verbose:
                    color = Color.Cyan;
                    //text = messageType + ": " + text;
                    break;
            }
            lines.Enqueue(new vxDebugText(text, color));

            while (lines.Count >= MaxLineCount)
                lines.Dequeue();

            // Call registered listeners.
            foreach (IDebugEchoListner listner in listenrs)
                listner.Echo(messageType, text);
        }

		/// <summary>
		/// Output Standard message.
		/// </summary>
		/// <param name="text"></param>
        public void Echo(string text)
        {
            Echo(DebugCommandMessage.Info, text);
        }

        public void EchoCMD(string text)
        {
            Echo(DebugCommandMessage.Info, "     " + text);
        }

        /// <summary>
        /// Output Warning message.
        /// </summary>
        /// <param name="text"></param>
  //      public void EchoWarning(string text)
  //      {
  //          Echo(DebugCommandMessage.Warning, text);
  //      }

		///// <summary>
		///// Output Error message.
		///// </summary>
		///// <param name="text"></param>
  //      public void EchoError(string text)
  //      {
  //          Echo(DebugCommandMessage.Error, text);
  //      }

		/// <summary>
		/// Add Command executioner.
		/// </summary>
		/// <param name="executioner">Executioner.</param>
        public void PushExecutioner(IDebugCommandExecutioner executioner)
        {
            executioners.Push(executioner);
        }

		/// <summary>
		/// Remote Command executioner.
		/// </summary>
        public void PopExecutioner()
        {
            executioners.Pop();
        }

        #endregion

        public override string GetCommand()
        {
            return "console";
        }

        public override string GetDescription()
        {
            return "The in game debug console";
        }

        #region Update and Draw

        /// <summary>
        /// Show Debug Command window.
        /// </summary>
        public void Show()
        {
            if (state == State.Closed)
            {
                stateTransition = 0.0f;
                state = State.Opening;
            }
        }

        /// <summary>
        /// Hide Debug Command window.
        /// </summary>
        public void Hide()
        {
            if (state == State.Opened)
            {
                stateTransition = 1.0f;
                state = State.Closing;
            }
        }

		/// <summary>
		/// The state of the keyboard.
		/// </summary>
		KeyboardState keyState;

		const float OpenSpeed = 8.0f;
		const float CloseSpeed = 8.0f;

		/// <summary>
		/// Update the specified gameTime.
		/// </summary>
		/// <param name="gameTime">Game time.</param>
        protected internal override void Update()
        {
			if (vxEngine.BuildType == vxBuildType.Debug)
			{
				keyState = Keyboard.GetState();


				//Reset Line Count
				//MaxLineDisplayCount = this.vxGraphics.GraphicsDevice.Viewport.Height / (int)debugManager.DebugFont.MeasureString("A").Y - 3;

				float dt = vxTime.DeltaTime;

				switch (state)
				{
					case State.Closed:
						if (keyState.IsKeyDown(ToggleConsoleKey))
							Show();
						break;
					case State.Opening:
						stateTransition += dt * OpenSpeed;
						if (stateTransition > 1.0f)
						{
							stateTransition = 1.0f;
							state = State.Opened;
						}
						break;
					case State.Opened:
						ProcessKeyInputs(dt);
						break;
					case State.Closing:
						stateTransition -= dt * CloseSpeed;
						if (stateTransition < 0.0f)
						{
							stateTransition = 0.0f;
							state = State.Closed;
						}
						break;
				}

				prevKeyState = keyState;

				//base.Update();
			}
        }


        /// <summary>
        /// Hand keyboard input.
        /// </summary>
        /// <param name="dt"></param>
        public void ProcessKeyInputs(float dt)
        {
            //keyState = Keyboard.GetState();
            Keys[] keys = keyState.GetPressedKeys();

            bool shift = keyState.IsKeyDown(Keys.LeftShift) ||
                            keyState.IsKeyDown(Keys.RightShift);

            foreach (Keys key in keys)
            {
                if (!IsKeyPressed(key, dt)) continue;

				//Break out if the ` Key is pressed.
				if (key == Keys.OemTilde) {
					Hide ();
					break;
				}

                char ch;
                if (KeyboardUtils.KeyToString(key, shift, out ch))
                {
                    // Handle typical character input.
                    commandLine = commandLine.Insert(cursorIndex, new string(ch, 1));
                    cursorIndex++;
                }
                else
                {
                    switch (key)
                    {
					case Keys.PageUp:
							LineScrollPos--;
						LineScrollPos = Math.Max (LineScrollPos, 0);
						break;
					case Keys.PageDown:
							LineScrollPos++;
							LineScrollPos = Math.Min (LineScrollPos, lines.Count);
						break;
                        case Keys.Back:
                            if (cursorIndex > 0)
                                commandLine = commandLine.Remove(--cursorIndex, 1);
                            break;
                        case Keys.Delete:
                            if (cursorIndex < commandLine.Length)
                                commandLine = commandLine.Remove(cursorIndex, 1);
                            break;
                        case Keys.Left:
                            if (cursorIndex > 0)
                                cursorIndex--;
                            break;
                        case Keys.Right:
                            if (cursorIndex < commandLine.Length)
                                cursorIndex++;
                            break;
					case Keys.Enter:
                            // Run the command.
						ExecuteCommand (commandLine);
						commandLine = string.Empty;
						cursorIndex = 0;

						LineScrollPos++;

						ScrollToBottom ();
                            break;
                        case Keys.Up:
                            // Show command history.
                            if (commandHistory.Count > 0)
                            {
                                commandHistoryIndex =
                                    Math.Max(0, commandHistoryIndex - 1);

                                commandLine = commandHistory[commandHistoryIndex];
                                cursorIndex = commandLine.Length;
						}
						ScrollToBottom ();
                            break;
					case Keys.Down:
                            // Show command history.
						if (commandHistory.Count > 0) {
							commandHistoryIndex = Math.Min (commandHistory.Count - 1,
								commandHistoryIndex + 1);
							commandLine = commandHistory [commandHistoryIndex];
							cursorIndex = commandLine.Length;
						}
						ScrollToBottom ();
                            break;
                    }
                }
            }

        }

		/// <summary>
		/// Scrolls the console to the bottom.
		/// </summary>
		void ScrollToBottom()
		{
			if (lines.Count - LineScrollPos < MaxLineDisplayCount-1)
				LineScrollPos-=1;
			
			if ((lines.Count) > MaxLineDisplayCount && lines.Count - LineScrollPos > MaxLineDisplayCount)
				LineScrollPos = lines.Count - MaxLineDisplayCount + 1;

			if (lines.Count - LineScrollPos == MaxLineDisplayCount) {
				LineScrollPos++;
			}
		}

        /// <summary>
		/// Pressing check with key repeating.
        /// </summary>
        /// <returns><c>true</c> if this instance is key pressed the specified key dt; otherwise, <c>false</c>.</returns>
        /// <param name="key">Key.</param>
        /// <param name="dt">Dt.</param>
        bool IsKeyPressed(Keys key, float dt)
        {
            // Treat it as pressed if given key has not pressed in previous frame.
            if (prevKeyState.IsKeyUp(key))
            {
                keyRepeatTimer = keyRepeatStartDuration;
                pressedKey = key;
                return true;
            }

            // Handling key repeating if given key has pressed in previous frame.
            if (key == pressedKey)
            {
                keyRepeatTimer -= dt;
                if (keyRepeatTimer <= 0.0f)
                {
                    keyRepeatTimer += keyRepeatDuration;
                    return true;
                }
            }

            return false;
        }



        /// <summary>
        /// Draw the specified gameTime.
        /// </summary>
        protected internal override void Draw()
        {
            // Do nothing when command window is completely closed.
            if (state == State.Closed || vxEngine.BuildType != vxBuildType.Debug)
                return;


            SpriteFont font = vxInternalAssets.Fonts.DebugFont;
            vxSpriteBatch spriteBatch = vxGraphics.SpriteBatch;
            Texture2D whiteTexture = vxInternalAssets.Textures.Blank;

            // Compute command window size and draw.
            float w = vxGraphics.GraphicsDevice.Viewport.Width;
            float h = vxGraphics.GraphicsDevice.Viewport.Height;
            float topMargin = h * 0;
            float leftMargin = w * 0;

            Rectangle rect = new Rectangle();
            rect.X = (int)leftMargin;
            rect.Y = (int)topMargin;
            rect.Width = (int)(w);
            rect.Height = (int)((MaxLineDisplayCount+2) * font.LineSpacing);

            Matrix mtx = Matrix.CreateTranslation(
                        new Vector3(0, -rect.Height * (1.0f - stateTransition), 0));

            spriteBatch.Begin("Debug.CommandUI", SpriteSortMode.Deferred, null, null, null, null, null, mtx);

            spriteBatch.Draw(whiteTexture, rect, new Color(0, 0, 0, 200));

            // Draw each lines.
            Vector2 pos = new Vector2(leftMargin, topMargin).ToIntValue();

			Color consoleColour = new Color(0,255,0,255);

            //Show Title Bar
			spriteBatch.DrawString(font, "     VERTICES ENGINE DEBUG CONSOLE - v." + vxEngine.EngineVersion, pos,consoleColour);
            pos.Y += font.LineSpacing;
			spriteBatch.DrawString(font, "======================================================================", pos, consoleColour);
            pos.Y += font.LineSpacing;

			/*
            foreach (string line in lines)
            {
				spriteBatch.DrawString(font, line, pos, consoleColour);
                pos.Y += font.LineSpacing;
            }
            */

			vxDebugText[] linesarray = lines.ToArray ();

			//Just in case, do a last minute check to makes sure the line scroll pos is within bounds

			LineScrollPos = (int)MathHelper.Clamp (LineScrollPos, 0, lines.Count + MaxLineDisplayCount);

			int linestoshow = lines.Count;

			if (lines.Count > MaxLineDisplayCount) {
				linestoshow = LineScrollPos + MaxLineDisplayCount-1;

				//Check that it's not larger than the max number of lines
				linestoshow = Math.Min(linestoshow, lines.Count);
			}



			for (int index = LineScrollPos; index < linestoshow; index++) {
				
                spriteBatch.DrawString (font, linesarray [index].Text, pos, linesarray[index].Color);
				pos.Y += font.LineSpacing;
			}

            // Draw prompt string.
            string leftPart = Prompt + commandLine.Substring(0, cursorIndex);
            Vector2 cursorPos = pos + font.MeasureString(leftPart);
            cursorPos.Y = pos.Y;

            spriteBatch.DrawString(font,
				String.Format("{0}{1}", Prompt, commandLine), pos, consoleColour);
			spriteBatch.DrawString(font, Cursor, cursorPos, consoleColour);

			spriteBatch.Draw (vxInternalAssets.Textures.Blank, 
				new Rectangle (0, (MaxLineDisplayCount+2) * font.LineSpacing, 2000, 4),
				Color.Black * 0.25f);

			spriteBatch.Draw (vxInternalAssets.Textures.Blank, 
				new Rectangle (0, (MaxLineDisplayCount+2) * font.LineSpacing, 2000, 3),
				Color.Black * 0.6f);

			spriteBatch.Draw (vxInternalAssets.Textures.Blank, 
				new Rectangle (0, (MaxLineDisplayCount+2) * font.LineSpacing, 2000, 2),
				Color.Black);
			
			spriteBatch.Draw (vxInternalAssets.Textures.Blank, 
				new Rectangle (0, (MaxLineDisplayCount+2) * font.LineSpacing, 2000, 1),
				Color.White*0.75f);


			//Draw Scroll Bar


			//Get Max Scroll Travel
			//int maxheight = Math.Max((int)font.LineSpacing * (lines.Count+2), (int)font.LineSpacing * (MaxLineDisplayCount+2));

			//Get Height of Screen
			int scrnHeight = font.LineSpacing * (MaxLineDisplayCount+2);//debugManager.GraphicsDevice.Viewport.Height;

			//Scroll Bar Height is a ratio of Max Display Lines and the Total Number of Lines
			int barHeight = scrnHeight * MaxLineDisplayCount / (lines.Count+MaxLineDisplayCount);


			int barPos = scrnHeight * LineScrollPos / (lines.Count+MaxLineDisplayCount);

			//Now set the scrollpos 
			//barPos = Math.Min(barPos, maxtravel);

			int px = 5;
			int py = 5;
			rect = new Rectangle (
                vxGraphics.GraphicsDevice.Viewport.Width - 2 * px,
				py + barPos,
				3,
				barHeight - 2 * py);
			

			//Draw Scroll Bar
			spriteBatch.Draw (vxInternalAssets.Textures.Blank,
				rect, consoleColour);

			//Draw Scroll Bar Border
			spriteBatch.Draw (vxInternalAssets.Textures.Blank,
				new Rectangle (
                    vxEngine.Game.GraphicsDevice.Viewport.Width - 3 * px,
					py,
					1,
					scrnHeight - 2 * py), consoleColour);

            spriteBatch.End();
        }

        #endregion

    }
}
