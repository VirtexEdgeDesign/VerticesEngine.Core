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
//using VerticesEngine.Graphics;

//namespace VerticesEngine.UI.Controls
//{
//    /// <summary>
//    /// Toolbar control that holds <see cref="VerticesEngine.UI.Controls.vxScrollPanelComboxBoxItem"/> 
//    /// </summary>
//	public class vxModelInfoPropertyGUIItem : vxScrollPanelItem
//    {
//		/// <summary>
//		/// The Name label.
//		/// </summary>
//        public vxLabel Label;

//        public vxLabel Info;

//		vxModel Model;

//		/// <summary>
//		/// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxModelInfoGUIItem"/> class.
//		/// </summary>
//		/// <param name="Engine">Engine.</param>
//		/// <param name="UIManager">GUIM anager.</param>
//		/// <param name="Model">Model.</param>
//        public vxModelInfoPropertyGUIItem(vxEngine Engine, vxGUIManager UIManager, vxModel Model) :
//            base(Engine, "", Vector2.Zero, null, 0)
//        {
//            UIManager.Add(this);

//			this.Model = Model;

//            this.Text = "Model Info - " + Model.Name;

//            Label = new vxLabel(Engine, Text, new Vector2(10, 5));
//            UIManager.Add(Label);

//            Info = new vxLabel(Engine, "", new Vector2(10, 5));
//            UIManager.Add(Info);

//            //this.Image = Image;

//            Height = 128;
//        }

//		/// <summary>
//		/// Draw the specified Engine.
//		/// </summary>
//		public override void Draw()
//        {
//            Label.Font = vxInternalAssets.Fonts.ViewerFont;
//            Info.Font = vxInternalAssets.Fonts.ViewerFont;

//            int offset = (int)(Label.Font.LineSpacing + Padding.Y * 2);


//            Label.Text = this.Text;
            
//            Label.Position = new Vector2(Bounds.Left + Padding.X, Bounds.Y + Padding.Y);

//			string path = Label.Font.GetClampedString(Model.ModelPath, Bounds.Width/2);
//            Info.Text = string.Format("Primitive Count: {0}\n", Model.TotalPrimitiveCount);
//            Info.Text += string.Format("Tag: {0}\n", Model.ModelMain.Tag);
//			Info.Text += string.Format("Model Path: {0}\n", path);
//            Info.Position = new Vector2(Bounds.Left + Padding.X*4, Bounds.Y  + offset);
//            //
//            //Draw Button
//            //
//            Engine.SpriteBatch.Draw(vxInternalAssets.Textures.Blank, Bounds, Theme.Background.Color);


//        }
//    }
//}
