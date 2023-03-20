using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VerticesEngine.UI.Dialogs;

namespace VerticesEngine.UI.Events
{
    public class vxUIControlClickEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public vxUIControlClickEventArgs(vxUIControl guiItem)
        {
            _guiItem = guiItem;
        }


        /// <summary>
        /// Gets the index of the player who triggered this event.
        /// </summary>
        public vxUIControl GUIitem
        {
            get { return _guiItem; }
        }
        vxUIControl _guiItem;


        /// <summary>
        /// Gets the index of the player who triggered this event.
        /// </summary>
        public PlayerIndex PlayerIndex
        {
            get { return playerIndex; }
        }

        PlayerIndex playerIndex = PlayerIndex.One;
    }

	public class vxUIManagerItemAddEventArgs : EventArgs
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public vxUIManagerItemAddEventArgs(vxUIManager guiManager)
		{
			_guiManager = guiManager;
		}


		/// <summary>
        /// Gets the GUI manager.
        /// </summary>
        /// <value>The GUI manager.</value>
		public vxUIManager GuiManager
		{
			get { return _guiManager; }
		}
		vxUIManager _guiManager;
	}


	public class vxFileDialogItemClickEventArgs : EventArgs
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public vxFileDialogItemClickEventArgs(vxFileDialogItem vxbaseGuiItem)
		{
			this.fileDialogItem = vxbaseGuiItem;
		}


		/// <summary>
		/// Gets the index of the player who triggered this event.
		/// </summary>
		public vxFileDialogItem FileDialogItem
		{
			get { return fileDialogItem; }
		}
		vxFileDialogItem fileDialogItem;
	}

	public class vxValueChangedEventArgs : EventArgs
	{
		/// <summary>
		/// Gets the index of the player who triggered this event.
		/// </summary>
		public vxUIControl GUIitem
		{
			get { return _baseGuiItem; }
		}
		vxUIControl _baseGuiItem;

		/// <summary>
		/// Gets the new value.
		/// </summary>
		/// <value>The new value.</value>
		public float NewValue
		{
			get { return _newValue; }
		}
		float _newValue;


		public float PreviousValue
		{
			get { return _previousValue; }
		}
		float _previousValue;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxValueChangedEventArgs"/> class.
		/// </summary>
		/// <param name="vxbaseGuiItem">Vxbase GUI item.</param>
		/// <param name="newValue">New value.</param>
		/// <param name="previousValue">Previous value.</param>
		public vxValueChangedEventArgs(vxUIControl vxbaseGuiItem, float newValue, float previousValue)
		{
			_baseGuiItem = vxbaseGuiItem;

			_newValue = newValue;
			_previousValue = previousValue;
		}
	}

}
