//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Graphics;
//using VerticesEngine.Utilities;
//using VerticesEngine;
//using VerticesEngine.UI.Dialogs;
//using VerticesEngine.UI.Events;

//namespace VerticesEngine.UI.Controls
//{
//	/// <summary>
//	/// Toolbar control that holds <see cref="VerticesEngine.UI.Controls.vxScrollPanelComboxBoxItem"/> 
//	/// </summary>
//	public class vxCheckBoxPropertyGUIItem : vxBasePropertyGUIItem
//	{
//		/// <summary>
//		/// The Name label.
//		/// </summary>
//		public vxLabel Label;


//		/// <summary>
//		/// The check box.
//		/// </summary>
//		public vxButtonControl CheckBox;

//		/// <summary>
//		/// Is the Checked Box Checked.
//		/// </summary>
//		public bool IsChecked = false;

//		/// <summary>
//		/// The size of the check box.
//		/// </summary>
//		int BoxSize = 24;

//		public event EventHandler<EventArgs> CheckedStatusChange;


//		/// <summary>
//		/// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxCheckBoxPropertyGUIItem"/> class.
//		/// </summary>
//		/// <param name="Engine">Engine.</param>
//		/// <param name="UIManager">GUIM anager.</param>
//		/// <param name="Title">Title.</param>
//		/// <param name="isChecked">If set to <c>true</c> is checked.</param>
//		public vxCheckBoxPropertyGUIItem(vxEngine Engine, vxGUIManager UIManager, string Title, bool isChecked) :
//		base(Engine, UIManager, Title)
//		{
//			UIManager.Add(this);

//			Label = new vxLabel(Engine, Title, Vector2.Zero);

//			UIManager.Add(Label);

//			// Set the Chceked Status
//			IsChecked = isChecked;

//			// Create the Check Box Button
//			CheckBox = new vxButtonControl(Engine, "", new Vector2(5), BoxSize, BoxSize);


//			// Set status
//			SetStatus();

//			// Handle Clicking
//			CheckBox.Clicked += delegate
//			{
//				IsChecked = !IsChecked;

//				// Set status
//				SetStatus();

//				if (CheckedStatusChange != null)
//					CheckedStatusChange(this, new EventArgs());

//			};
//			UIManager.Add(CheckBox);

//			Height = 64;
//		}

//		void SetStatus()
//		{

//			if (IsChecked)
//				CheckBox.Text = "x";
//			else
//				CheckBox.Text = "";
//		}

//		/// <summary>
//		/// Draws the GUI Item
//		/// </summary>
//		public override void Draw()
//		{
//			base.Draw();

//			//Update GUI Item Positions and Values

//			CheckBox.Position = RootPos;

//			this.Title = this.Text + " (Value = " + IsChecked + ")";
//			Label.Text = this.Text + " : " + IsChecked;
//			//Label.//Colour_Text = InternalLabelColor;
//			Label.Font = vxInternalAssets.Fonts.ViewerFont;
//			Label.Position = RootPos + new Vector2(Padding.X + BoxSize, Padding.Y);


//		}
//	}
//}
