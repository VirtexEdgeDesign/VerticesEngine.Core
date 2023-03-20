using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Utilities;
using VerticesEngine;
using VerticesEngine.UI.Dialogs;
using VerticesEngine.UI.Themes;
using VerticesEngine.Graphics;

namespace VerticesEngine.UI.Controls
{
    /// <summary>
    /// Toolbar control that holds <see cref="VerticesEngine.UI.Controls.vxScrollPanelSliderItem"/> 
    /// </summary>
    public class vxScrollPanelSliderItem : vxScrollPanelItem
    {
        /// <summary>
        /// The label.
        /// </summary>
        public vxLabel Label;

        /// <summary>
        /// The slider.
        /// </summary>
        public vxSlider Slider;


        /// <summary>
        /// Initializes a new instance of the <see cref="VerticesEngine.UI.Controls.vxScrollPanelComboxBoxItem"/> class.
        /// </summary>
        /// <param name="position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
        public vxScrollPanelSliderItem(vxUIManager UIManager, string Title, float Min, float Max, float Value, Vector2 position, float SliderOffset) :
                    base("", Vector2.Zero, null, 0)
        {
            this.Text = Title;

            Label = new vxLabel(Title, position + new Vector2(10, 5));
            Label.Font = vxUITheme.Fonts.Size12;
            Label.UIManager = UIManager;

            Slider = new vxSlider(Min, Max, Value,
                                  position + new Vector2(SliderOffset, 4), (int)(150 + SliderOffset - 275));

            //TotalLength = (int)(430 + 150 + SliderOffset - 275);
            Slider.UIManager = UIManager;
            //UIManager.Add(Slider);

            Height = 40;
        }

        public vxScrollPanelSliderItem(vxUIManager UIManager, string Title, float Min, float Max, float Value, Vector2 position) : 
            this(UIManager, Title, Min, Max, Value, position, 275)
        {

        }

        public vxScrollPanelSliderItem(vxUIManager UIManager, string Title, float Min, float Max, float Value) : 
            this(UIManager, Title, Min, Max, Value, Vector2.Zero, 275)
        {

        }
        protected internal override void Update()
        {
            base.Update();

            Label.Update();
            Slider.Update();
        }

        /// <summary>
        /// Draws the GUI Item
        /// </summary>
        public override void Draw()
        {
            //base.Draw();
            //Update Rectangle
            //BoundingRectangle = new Rectangle((int)(Position.X), (int)(Position.Y), TotalLength, Height);
            Label.Text = this.Text + "   [" + Slider.Value + "]";

            Label.Position = Label.OriginalPosition + Position;
            Slider.Position = new Vector2(Bounds.Right - Slider.MarkerRec.Width - Padding.X, Bounds.Y + Height / 2 - Slider.Height / 2);

            //
            //Draw Button
            //
            vxGraphics.SpriteBatch.Draw(DefaultTexture, Bounds.GetBorder(1), Color.Black);
            vxGraphics.SpriteBatch.Draw(DefaultTexture, Bounds, Theme.Background.Color);

            Label.Draw();
            Slider.Draw();

        }
    }
}
