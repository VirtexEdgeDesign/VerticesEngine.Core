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

//namespace VerticesEngine.UI.Controls
//{
//	/// <summary>
//	/// Toolbar control that holds <see cref="VerticesEngine.UI.Controls.vxScrollPanelComboxBoxItem"/> 
//	/// </summary>
//	public class vxVector4PropertyGUIItem : vxBasePropertyGUIItem
//	{
//		/// <summary>
//		/// The Value Label
//		/// </summary>
//		public vxLabel ValueLabel;

//		public Vector4 Value;

//		/// <summary>
//		/// The slider.
//		/// </summary>
//		public vxTextbox TxtbxX;
//		public vxTextbox TxtbxY;
//		public vxTextbox TxtbxZ;
//		public vxTextbox TxtbxW;

//		string[] LabelTexts = { "X:", "Y:", "Z:", "W:" };

//		/// <summary>
//		/// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxVector4GUIItem"/> class.
//		/// </summary>
//		/// <param name="Engine">Engine.</param>
//		/// <param name="UIManager">GUIM anager.</param>
//		/// <param name="Title">Title.</param>
//		/// <param name="Value">Value.</param>
//		public vxVector4PropertyGUIItem(vxEngine Engine, vxGUIManager UIManager,
//						       string Title, Vector4 Value) :
//		base(Engine, UIManager, Title)
//		{
//			UIManager.Add(this);

//			this.Value = Value;

//			//ValueLabel = new vxLabel(Engine, Title, position + new Vector2(10, 5));
//			//UIManager.Add(ValueLabel);

//			TxtbxX = new vxTextbox(Engine, Value.X.ToString(), new Vector2(0, 25));
//			TxtbxX.Font = vxInternalAssets.Fonts.ViewerFont;
//			TxtbxX.Width = 100;
//			UIManager.Add(TxtbxX);
//			TxtbxX.TextChanged += delegate { OnValueChanged(); };

//			TxtbxY = new vxTextbox(Engine, Value.Y.ToString(), Vector2.Zero);
//			TxtbxY.Font = vxInternalAssets.Fonts.ViewerFont;
//			TxtbxY.Width = 100;
//			UIManager.Add(TxtbxY);
//			TxtbxY.TextChanged += delegate { OnValueChanged(); };

//			TxtbxZ = new vxTextbox(Engine, Value.Z.ToString(), Vector2.Zero);
//			TxtbxZ.Font = vxInternalAssets.Fonts.ViewerFont;
//			TxtbxZ.Width = 100;
//			UIManager.Add(TxtbxZ);
//			TxtbxZ.TextChanged += delegate { OnValueChanged(); };

//			TxtbxW = new vxTextbox(Engine, Value.W.ToString(), Vector2.Zero);
//			TxtbxW.Font = vxInternalAssets.Fonts.ViewerFont;
//			TxtbxW.Width = 100;
//			UIManager.Add(TxtbxW);
//			TxtbxW.TextChanged += delegate { OnValueChanged(); };

//			foreach(string label in LabelTexts)
//			{
//				vxLabel newLabel = new vxLabel(Engine, label, Vector2.Zero);

//				Labels.Add(newLabel);
//				UIManager.Add(newLabel);
//			}



//			Height = 128;
//		}
//		public List<vxLabel> Labels = new List<vxLabel>();

//		public event EventHandler<EventArgs> ValueChanged;


//		// Handle any Value Chane
//		void OnValueChanged()
//		{
//			try
//			{
//				Value = new Vector4(
//					float.Parse(TxtbxX.Text),
//					float.Parse(TxtbxY.Text),
//					float.Parse(TxtbxZ.Text),
//					float.Parse(TxtbxW.Text));
//			}
//			catch{}

//			// Raise the 'ValueChanged' event.
//			if (ValueChanged != null)
//				ValueChanged(this, new EventArgs());
//		}

//		/// <summary>
//		/// Draws the GUI Item
//		/// </summary>
//		public override void Draw()
//		{
//			base.Draw();

//			//Update Rectangle
//			//BoundingRectangle = new Rectangle((int)(Position.X), (int)(Position.Y), TotalLength, Height);
//			Title = this.Text + " (" + Value + ")";

//			TxtbxX.Position = new Vector2(Bounds.Left + 50, Bounds.Y + 32);
//			TxtbxY.Position = new Vector2(Bounds.Left + 50, Bounds.Y + 48);
//			TxtbxZ.Position = new Vector2(Bounds.Left + 50, Bounds.Y + 64);
//			TxtbxW.Position = new Vector2(Bounds.Left + 50, Bounds.Y + 80);

//			int y = 30;
//			foreach (vxLabel label in Labels)
//			{label.Font =vxInternalAssets.Fonts.ViewerFont;
//				label.Position = new Vector2(Bounds.Left + 25, Bounds.Y + y);
//				y += 16;
//			}

//		}
//	}
//}
