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
//	public class vxBaseModelViewerPropertyGUIItem : vxScrollPanelItem
//    {
//		/// <summary>
//		/// The Name label.
//		/// </summary>
//        public vxLabel TitleLabel;


//		vxScrollPanel ScrollPanel;

//		/// <summary>
//		/// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxModelInfoGUIItem"/> class.
//		/// </summary>
//		/// <param name="Engine">Engine.</param>
//		/// <param name="ScrollPanelItem">ScrollPanelItem.</param>
//        public vxBaseModelViewerPropertyGUIItem(vxEngine Engine, string Title) :
//            base(Engine, Title, Vector2.Zero, null, 0)
//        {
//            //ScrollPanel.UIManager.Add(this);


//            TitleLabel = new vxLabel(Engine, Text, new Vector2(10, 5));
//			//ScrollPanel.UIManager.Add(TitleLabel);

//            Height = 24;
//        }

//		/// <summary>
//		/// Draw the specified Engine.
//		/// </summary>
//        public override void Draw()
//        {
//            TitleLabel.Font = vxInternalAssets.Fonts.ViewerFont;
//			//TitleLabel.//Colour_Text = Color.Black;

//            int offset = (int)(TitleLabel.Font.LineSpacing + Padding.Y * 2);



//            //
//            //Draw Button
//            //
//            Engine.SpriteBatch.Draw(vxInternalAssets.Textures.Blank, Bounds, Theme.Background.Color);

//			TitleLabel.Position = this.Position + Padding;
//            TitleLabel.Text = this.Text;
//			TitleLabel.Draw();

//        }
//    }
//}
