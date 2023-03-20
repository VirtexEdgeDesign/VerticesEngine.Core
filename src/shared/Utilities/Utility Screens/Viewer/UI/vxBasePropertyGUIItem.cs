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
//	public class vxBaseTitleBar : vxPanel
//	{
//		public vxBaseTitleBar(vxEngine Engine, int Height):
//		base(Engine, Vector2.Zero, 100, Height)
//		{
//			Font = vxInternalAssets.Fonts.ViewerFont;
//			DoBorder = true;

//			//Color_Normal = new Color(0.15f, 0.15f, 0.15f, 1);
//			BorderColour = new Color(0.1f, 0.1f, 0.1f, 1);

//		}
//	}

//    /// <summary>
//    /// Toolbar control that holds <see cref="VerticesEngine.UI.Controls.vxScrollPanelComboxBoxItem"/> 
//    /// </summary>
//	public class vxBasePropertyGUIItem : vxScrollPanelItem
//    {

//		/// <summary>
//		/// The title button panel.
//		/// </summary>
//        public vxPanel TitleButton;


//		/// <summary>
//		/// The height of the title button.
//		/// </summary>
//		public readonly int TitleButtonHeight;

//		/// <summary>
//		/// The title.
//		/// </summary>
//		public string Title;

//		public Color TitleTextColor = Color.White;

//		public Color InternalLabelColor = Color.LightGray * 0.85f;

//		/// <summary>
//		/// The root position for all GUI Items.
//		/// </summary>
//		public Vector2 RootPos;

//		/// <summary>
//		/// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxBasePropertyGUIItem"/> class.
//		/// </summary>
//		/// <param name="Engine">Engine.</param>
//		/// <param name="UIManager">GUIM anager.</param>
//		/// <param name="Title">Title.</param>
//		/// <param name="TitleButtonHeight">Title button height.</param>
//        public vxBasePropertyGUIItem(vxEngine Engine, vxGUIManager UIManager, string Title, int TitleButtonHeight = 20) :
//            base(Engine, Title, Vector2.Zero, null, 0)
//        {
//			this.Title = Title;
//			this.Text = Title;

//			this.TitleButtonHeight = TitleButtonHeight;
 
//			TitleButton = new vxBaseTitleBar(Engine, TitleButtonHeight);
//            UIManager.Add(TitleButton);

//            //Color_Normal = new Color(0.15f, 0.15f, 0.15f, 0.5f);
//			//Color_Highlight = new Color(0.195f, 0.195f, 0.195f, 0.5f);
//			//Color_Selected = Color_Highlight;

//			Height = 64;
//        }

//		/// <summary>
//		/// Draw the specified Engine.
//		/// </summary>
//        public override void Draw()
//        {
//			TitleButton.Position = this.Position;
//			TitleButton.Width = this.Width;

//			// Set the root position
//			RootPos = new Vector2(Bounds.Left, Bounds.Y + TitleButtonHeight) + Padding * 2;

//            //
//            //Draw Button
//            //
//            Engine.SpriteBatch.Draw(vxInternalAssets.Textures.Blank, Bounds, Theme.Background.Color);

//			TitleButton.Draw();
//			Engine.SpriteBatch.DrawString(vxInternalAssets.Fonts.ViewerFont,
//                                          this.Title, Position + new Vector2(6, 2), TitleTextColor);
//        }
//    }
//}
