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
//	public class vxSliderPropertyGUIItem : vxBasePropertyGUIItem
//    {
//		/// <summary>
//		/// The Name label.
//		/// </summary>
//        public vxLabel Label;

//        /// <summary>
//        /// The Value Label
//        /// </summary>
//        public vxLabel ValueLabel;

//        /// <summary>
//        /// The slider.
//        /// </summary>
//        public vxSlider Slider;


//        /// <summary>
//        /// Initializes a new instance of the <see cref="VerticesEngine.UI.Controls.vxScrollPanelComboxBoxItem"/> class.
//        /// </summary>
//        /// <param name="position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
//        public vxSliderPropertyGUIItem(vxEngine Engine, vxGUIManager UIManager, 
//		                               string Title, float Min, float Max, float Value, 
//		                               Vector2 position, float SliderOffset) :
//		base(Engine, UIManager, Title)
//        {
//            UIManager.Add(this);

//			// Setup Label
//            Label = new vxLabel(Engine, Title, position + new Vector2(10, 5));
//            UIManager.Add(Label);

//			// Setup Slider Control
//            Slider = new vxSlider(Engine, Min, Max, Value, position + new Vector2(SliderOffset, 4), (int)(150 + SliderOffset - 275));
//			Slider.Height = 16;
//			UIManager.Add(Slider);
//        }

//		public vxSliderPropertyGUIItem(vxEngine Engine, vxGUIManager UIManager,
//									   string Title, float Min, float Max, float Value,
//									   Vector2 position):this(Engine, UIManager,
//		                                                      Title, Min, Max, Value, position, 275)
//		{

//        }

//        public vxSliderPropertyGUIItem(vxEngine Engine, vxGUIManager UIManager,
//                                       string Title, float Min, float Max, float Value) : this(Engine, UIManager,
//                                                              Title, Min, Max, Value, Vector2.Zero, 275)
//        {

//        }

//        /// <summary>
//        /// Draws the GUI Item
//        /// </summary>
//        public override void Draw()
//        {
//			base.Draw();

//            //Update Rectangle
//            this.Title = this.Text + " (Value = " + Slider.Value + ")";
//            Label.Text = this.Text + ":" + Slider.Value;
//            Label.Font = vxInternalAssets.Fonts.ViewerFont;
//			//Label.//Colour_Text = InternalLabelColor;
//			Label.Position = RootPos;
//            Slider.Position = new Vector2(Bounds.Right - Slider.MarkerRec.Width - Padding.X, RootPos.Y);
//        }
//    }
//}
