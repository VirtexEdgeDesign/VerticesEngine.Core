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
//    /// <summary>
//    /// Toolbar control that holds <see cref="VerticesEngine.UI.Controls.vxScrollPanelComboxBoxItem"/> 
//    /// </summary>
//	public class vxImagePropertyGUIItem : vxScrollPanelItem
//    {
//		/// <summary>
//		/// The Name label.
//		/// </summary>
//        public vxLabel Label;

//        public vxLabel Info;

//        Texture2D Image;
//        /// <summary>
//        /// Initializes a new instance of the <see cref="VerticesEngine.UI.Controls.vxScrollPanelComboxBoxItem"/> class.
//        /// </summary>
//        /// <param name="position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
//        public vxImagePropertyGUIItem(vxEngine Engine, vxGUIManager UIManager, 
//		                               string Title, Texture2D Image) :
//            base(Engine, "", Vector2.Zero, null, 0)
//        {
//            UIManager.Add(this);

//            this.Text = Title;

//            Label = new vxLabel(Engine, Title, new Vector2(10, 5));
//            UIManager.Add(Label);

//            Info = new vxLabel(Engine, "", new Vector2(10, 5));
//            UIManager.Add(Info);

//            this.Image = Image;

//            Height = 128;
//        }

//        int BorderSize = 1;
//		/// <summary>
//		/// Draw the specified Engine.
//		/// </summary>
//        public override void Draw()
//        {
//            Label.Font = vxInternalAssets.Fonts.ViewerFont;
//            Info.Font = vxInternalAssets.Fonts.ViewerFont;

//            int offset = (int)(Label.Font.LineSpacing + Padding.Y * 2);
//            Rectangle imageBounds = new Rectangle(
//                (int)(Bounds.Left + Padding.X),
//               (int)(Bounds.Y + offset),
//               (int)(Height - offset - Padding.X), (int)(Height - offset - Padding.X));

//            Rectangle imageBoundsBorder = new Rectangle(
//                imageBounds.X - BorderSize,
//                imageBounds.Y - BorderSize,
//                imageBounds.Width + 2 * BorderSize,
//                imageBounds.Height + 2 * BorderSize);

//            //Update Rectangle
//            //BoundingRectangle = new Rectangle((int)(Position.X), (int)(Position.Y), TotalLength, Height);
//            Label.Text = this.Text;
            
//            Label.Position = new Vector2(Bounds.Left + Padding.X, Bounds.Y + Padding.Y);
//			Info.Position = new Vector2(imageBounds.Right + Padding.X, imageBounds.Y);

//			string name = "";

//            //
//            //Draw Button
//            //
//            Engine.SpriteBatch.Draw(vxInternalAssets.Textures.Blank, Bounds, Theme.Background.Color);

//			if (Image != null)
//			{
//			if (Image.Name != "")
//				name = System.IO.Path.GetFileName(Image.Name);
			
//            Info.Text = string.Format("Name: {0}\n", name);
//            Info.Text += string.Format("Size: {0}x{1}\n", Image.Bounds.Width, Image.Bounds.Height);
//            Info.Text += string.Format("Format: {0}\n", Image.Format);
//            Info.Text += string.Format("LevelCount: {0}\n", Image.LevelCount);
//            Info.Text += string.Format("Tag: {0}\n", Image.Tag);
            

//				Engine.SpriteBatch.Draw(Image, imageBoundsBorder, Color.Black);
//				Engine.SpriteBatch.Draw(Image, imageBounds, Color.White);
//			}
//			else
//			{
//				Engine.SpriteBatch.Draw(vxInternalAssets.Textures.DefaultDiffuse, imageBoundsBorder, Color.Black);
//				Engine.SpriteBatch.Draw(vxInternalAssets.Textures.DefaultDiffuse, imageBounds, Color.White);
//			}

//        }
//    }
//}
