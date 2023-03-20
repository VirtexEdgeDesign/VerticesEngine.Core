using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using VerticesEngine.Graphics;
using VerticesEngine.UI.Dialogs;
using VerticesEngine.UI.Events;

namespace VerticesEngine.UI.Controls
{
    /// <summary>
    /// Toolbar control that holds <see cref="VerticesEngine.UI.Controls.vxScrollPanelComboxBoxItem"/> 
    /// </summary>
    public class vxScrollPanelIncrementItem : vxScrollPanelItem
	{
		vxLabel Label;



		vxButtonImageControl AddButton;
		vxButtonImageControl SubtractButton;

		/// <summary>
		/// The value.
		/// </summary>
		public float Value = 5;

		/// <summary>
		/// The minimum value.
		/// </summary>
		public float MinValue = 0;

		/// <summary>
		/// The max value.
		/// </summary>
		public float MaxValue = 10;

		/// <summary>
		/// The value to increment by.
		/// </summary>
		public float Tick = 1;

		public static Texture2D LeftArrow, LeftArrowHover;
		public static Texture2D RightArrow, RightArrowHover;


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
		/// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxScrollPanelIncrementItem"/> class.
		/// </summary>
		/// <param name="ScrollPanel">Scroll panel.</param>
		/// <param name="Title">Title.</param>
		/// <param name="Value">Value.</param>
		/// <param name="MinValue">Minimum value.</param>
		/// <param name="MaxValue">Max value.</param>
		public vxScrollPanelIncrementItem(vxScrollPanel ScrollPanel, string Title, float Value, float MinValue, float MaxValue) :
								this(ScrollPanel, Title, Value, MinValue, MaxValue, (MaxValue - MinValue) / 10)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxScrollPanelIncrementItem"/> class.
		/// </summary>
		/// <param name="ScrollPanel">Scroll panel.</param>
		/// <param name="Title">Title.</param>
		/// <param name="Value">Value.</param>
		/// <param name="MinValue">Minimum value.</param>
		/// <param name="MaxValue">Max value.</param>
		/// <param name="Tick">Tick.</param>
		public vxScrollPanelIncrementItem(vxScrollPanel ScrollPanel, string Title, float Value, float MinValue, float MaxValue, float Tick) :
					base("", Vector2.Zero, null, 0)
		{
			// Set Constructor Input Variables
			this.Text = Title;
			this.Value = Value;
			this.MaxValue = MaxValue;
			this.MinValue = MinValue;
			this.Tick = Tick;

			// Addit to the Scroll Panel
			ScrollPanel.AddItem(this);

			Height = 40;


			if (LeftArrow == null)
			{
				LeftArrow = vxInternalAssets.Textures.Arrow_Left;
				LeftArrowHover = vxInternalAssets.Textures.Arrow_Left;

				RightArrow = vxInternalAssets.Textures.Arrow_Right;
				RightArrowHover = vxInternalAssets.Textures.Arrow_Right;
			}

			Label = new vxLabel(Title, new Vector2(10, 5));
			Label.UIManager = UIManager;


			Point pos = new Point(1, 1);

			//Height = ButtonSqaureSize;

			AddButton = new vxButtonImageControl(RightArrow, RightArrowHover, 
                Vector2.Zero + pos.ToVector2(),
                Height -pos.X*2,
				Height-pos.Y*2)
			{
				//DrawHoverBackground = false,
			};

			AddButton.Clicked += delegate
			{
				float oldValue = this.Value;
				this.Value += Tick;
				this.Value = MathHelper.Clamp(this.Value, MinValue, MaxValue);
				OnValueChanged(this.Value, oldValue);
			};


			SubtractButton = new vxButtonImageControl(
				LeftArrow,
				LeftArrowHover, Vector2.Zero + pos.ToVector2(),
                Height -pos.X*2,
				Height-pos.Y*2)
			{
				//DrawHoverBackground = false,
			};

			SubtractButton.Clicked += delegate
			{
				float oldValue = this.Value;
				this.Value -= Tick;
				this.Value = MathHelper.Clamp(this.Value, MinValue, MaxValue);

				OnValueChanged(this.Value, oldValue);
			};

			//Height = 40;

			EnabledStateChanged += OnEnabledStateChanged;
		}

		void OnValueChanged(float newValue, float oldValue)
		{
			if (ValueChanged != null)
				ValueChanged(this, new vxValueChangedEventArgs(this, newValue, oldValue));
		}

		void OnEnabledStateChanged(object sender, EventArgs e)
		{
			AddButton.IsEnabled = IsEnabled;
			SubtractButton.IsEnabled = IsEnabled;
		}


		protected internal override void Update()
		{
			base.Update();

			Label.Update();
			AddButton.Update();
			SubtractButton.Update();
		}

		/// <summary>
		/// Draws the GUI Item
		/// </summary>
		public override void Draw()
		{
			// Set Label Position
			Label.Position = Label.OriginalPosition + Position;

			AddButton.Position = new Vector2(
				Bounds.Right - AddButton.Width - Padding.X,
				Bounds.Y + Height / 2 - AddButton.Height / 2);


			SubtractButton.Position = new Vector2(
				Bounds.Right - Padding.X - AddButton.Width - Padding.X - ArrowSpace - Padding.X - SubtractButton.Width,
				Bounds.Y + Height / 2 - SubtractButton.Height / 2);

			//ValueComboBox.Position = new Vector2(Bounds.Right - ValueComboBox.Width - Padding.X, Bounds.Y + Height / 2 - 10);
			vxGraphics.SpriteBatch.Draw(vxInternalAssets.Textures.Blank, Bounds, Theme.Background.Color);

			vxGraphics.SpriteBatch.DrawString(Font, Value.ToString(),
			                              new Vector2(
				                              (AddButton.Bounds.Right + SubtractButton.Bounds.Left)/2 - Font.MeasureString(Value.ToString()).X/2, 
				                              Label.Position.Y), Label.Theme.Text.Color);

			Label.Draw();
			AddButton.Draw();
			SubtractButton.Draw();
		}
	}
}
