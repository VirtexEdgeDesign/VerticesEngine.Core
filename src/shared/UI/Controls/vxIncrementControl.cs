using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VerticesEngine;
using VerticesEngine.Graphics;
using VerticesEngine.UI;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Dialogs;
using VerticesEngine.UI.Events;
using VerticesEngine.UI.Themes;
using VerticesEngine.Utilities;

namespace VerticesEngine.UI.Controls
{
	/// <summary>
	/// Scrollbar Item base class. This can be inherited too expand controls within one scrollbar item.
	/// </summary>
	public class vxIncrementControl : vxUIControl
    {
        vxSpinnerControl Spinner;
		List<vxUIControl> GUIItemList = new List<vxUIControl>();

		public float Value = 5;
		public float MinValue = 0;
		public float MaxValue = 10;

		public float Incremnet=1;

		/// <summary>
		/// The arrow start offset.
		/// </summary>
		public int ArrowStartOffset = 250;

        /// <summary>
        /// The size of the button sqaure.
        /// </summary>
		//public static int ButtonSqaureSize = 40;

		/// <summary>
		/// The space between arrows. This is measured as the maximum value between the text width of the 
		/// minimum and maximum values.
		/// </summary>
		public int ArrowSpace = 50;


		/// <summary>
		/// Occurs when the value changes.
		/// </summary>
		public event EventHandler<vxValueChangedEventArgs> ValueChanged;


		/// <summary>
		/// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxIncrementControl"/> class.
		/// </summary>
		/// <param name="Value">Value.</param>
		/// <param name="MinValue">Minimum value.</param>
		/// <param name="MaxValue">Max value.</param>
		public vxIncrementControl (string Text, Vector2 Position, float Value, float MinValue, float MaxValue) : 
		this(Text, Position, Value, MinValue, MaxValue, (MaxValue - MinValue) / 10)
        {

		}


        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxIncrementControl"/> class.
        /// </summary>
        /// <param name="Text">This GUI Items Text.</param>
        /// <param name="position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
        /// <param name="Value">Value.</param>
        /// <param name="MinValue">Minimum value.</param>
        /// <param name="MaxValue">Max value.</param>
        /// <param name="inc">Inc.</param>
        public vxIncrementControl(string Text, Vector2 Position, float Value, float MinValue, float MaxValue, float inc) : 
        this(Text, Position, Value, MinValue, MaxValue, inc, 250)
        {
        }



        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxIncrementControl"/> class.
        /// </summary>
        /// <param name="Engine">Engine.</param>
        /// <param name="text">This GUI Items Text.</param>
        /// <param name="position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
        /// <param name="Value">Value.</param>
        /// <param name="MinValue">Minimum value.</param>
        /// <param name="MaxValue">Max value.</param>
        /// <param name="inc">Inc.</param>
        /// <param name="ArrowStartOffset">Arrow start offset.</param>
		public vxIncrementControl(string Text, Vector2 Position, float Value, float MinValue, float MaxValue, float inc, int ArrowStartOffset) 
		{
            Height = (int)(64 * vxLayout.ScaleAvg * 3);
            int btnHeight = Height;
			this.Text = Text;
			this.Position = Position;
			this.Value = Value;
			this.MaxValue = MaxValue;
			this.MinValue = MinValue;
            this.ArrowStartOffset = (int)(ArrowStartOffset + (100) * vxLayout.ScaleAvg);

			Incremnet = inc;

			Font = vxInternalAssets.Fonts.MenuFont;
				
			btnHeight = (int)(64 * vxLayout.ScaleAvg);

            ArrowSpace = (int)(ArrowSpace * vxLayout.ScaleAvg);

            // The Spinner Control
            Spinner = new vxSpinnerControl((int)Value, new Vector2(ArrowStartOffset, 0), 
                (int)(vxUITheme.SpriteSheetLoc.ArrowBtnBack.Width * vxLayout.ScaleAvg), 
                                    ArrowSpace, 
                                    (int)inc,
                                   vxUITheme.SpriteSheetLoc.ArrowBtnBack,
                                    vxUITheme.SpriteSheetLoc.ArrowBtnFwd);

            GUIItemList.Add(Spinner);

            Spinner.ValueChanged += Spinner_ValueChanged;

			foreach (vxUIControl item in GUIItemList)
				item.Font = Font;

			Bounds = new Rectangle(0, 0, vxUITheme.SpriteSheetLoc.ArrowBtnBack.Width, vxUITheme.SpriteSheetLoc.ArrowBtnBack.Height);

			EnabledStateChanged += OnEnabledStateChanged;
        }

        void Spinner_ValueChanged(object sender, vxValueChangedEventArgs e)
		{
			if (ValueChanged != null)
				ValueChanged(this, e);
        }

        void OnEnabledStateChanged(object sender, EventArgs e)
		{
			foreach (vxUIControl item in GUIItemList)
				item.IsEnabled = IsEnabled;
		}

		protected internal override void Update()
        {
            base.Update();
			foreach (vxUIControl item in GUIItemList)
			{
				item.Position = Position + item.OriginalPosition;
				item.Update();
			}
        }

		/// <summary>
		/// Draw the specified Engine.
		/// </summary>
		public override void Draw()
        {
            base.Draw();


			Vector2 textSize = Font.MeasureString(Text);

			// Draw the Text
			vxGraphics.SpriteBatch.DrawString (vxUITheme.Fonts.Size24, 
				Text, 
			                                 new Vector2 (
				                               Position.X,
											   Position.Y + Height / 2 - textSize.Y / 2),
				 Theme.Text.Color,
                                          0,
                                          Vector2.Zero,
                                          Math.Max(1, vxLayout.ScaleAvg),
                                          SpriteEffects.None,
                                          1);


            foreach (vxUIControl item in GUIItemList)
            {
                item.Draw();
            }
        }
    }
}