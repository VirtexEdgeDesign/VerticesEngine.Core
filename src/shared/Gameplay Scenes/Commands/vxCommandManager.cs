using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using VerticesEngine;
using VerticesEngine.Graphics;

namespace VerticesEngine.Commands
{
	/// <summary>
	/// Manages Do-Undo control in a sandbox enviroment.
	/// </summary>
	public class vxCommandManager
	{
        public bool IsActive = true;

		internal List<vxCommand> Commands = new List<vxCommand>();

		/// <summary>
		/// Gets the number of Commands currently in the list.
		/// </summary>
		/// <value>The count.</value>
		public int Count
		{
			get { return Commands.Count; }
		}


		/// <summary>
		/// The index of the current cmd. 
		/// </summary>
		public int CurrentCmdIndex = -1;

        /// <summary>
        /// Should the Command List show the current list.
        /// </summary>
        public bool ShowDebugOutput = false;

		/// <summary>
		/// Gets a value indicating whether this <see cref="T:VerticesEngine.Base.vxCommandManager"/> can do.
		/// </summary>
		/// <value><c>true</c> if can do; otherwise, <c>false</c>.</value>
		public bool CanRedo
		{
			get { return (CurrentCmdIndex < Count-1); }
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="T:VerticesEngine.Base.vxCommandManager"/> can undo.
		/// </summary>
		/// <value><c>true</c> if can undo; otherwise, <c>false</c>.</value>
		public bool CanUndo
		{
			get { return (CurrentCmdIndex >= 0); }
		}





		/// <summary>
		/// Initializes a new instance of the <see cref="T:VerticesEngine.Commands.vxCommandManager"/> class.
		/// </summary>
		/// <param name="Engine">Engine.</param>
		public vxCommandManager()
		{

		}

		public event EventHandler<EventArgs> OnChange;

		/// <summary>
		/// Add the specified command and clears all commands after the current Command Index.
		/// </summary>
		/// <returns>The add.</returns>
		/// <param name="command">Command.</param>
		public void Add(vxCommand command)
		{
            if (IsActive)
            {
                // Remore any commands which are ahead of the current cmd index
                if (Count > 0)
                {
                    while (CurrentCmdIndex < Count - 1)
                        Commands.RemoveAt(Count - 1);
                }

                // Add it to the Commands Collection
                Commands.Add(command);
                CurrentCmdIndex = Count - 1;

                command.Do();
            }

			if (OnChange != null)
				OnChange(this, new EventArgs());
		}


		/// <summary>
		/// Calls the Redo for the current command.
		/// </summary>
		public void ReDo()
		{
			if (CanRedo && IsActive)
			{
				CurrentCmdIndex++;
				CurrentCmdIndex = MathHelper.Clamp(CurrentCmdIndex, -1, Count - 1);
				Commands[CurrentCmdIndex].Do();
			}


			if (OnChange != null)
				OnChange(this, new EventArgs());
		}

		/// <summary>
		/// Calls the Undo for the current command.
		/// </summary>
		public void Undo()
		{
			if (CanUndo && IsActive)
			{
				Commands[CurrentCmdIndex].Undo();
				CurrentCmdIndex--;
				CurrentCmdIndex = MathHelper.Clamp(CurrentCmdIndex, -1, Count - 1);
			}

			if (OnChange != null)
				OnChange(this, new EventArgs());
		}
    }
}
