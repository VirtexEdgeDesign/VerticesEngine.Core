using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using VerticesEngine.Input;
using VerticesEngine.UI.Events;
using VerticesEngine.UI.Themes;

namespace VerticesEngine.UI.Controls
{
    #region Event Arguments



    #endregion

    /// <summary>
    /// Basic Button GUI Control.
    /// </summary>
    public class vxSlider : vxUIControl
    {
		/// <summary>
		/// Gets or sets the art provider.
		/// </summary>
		/// <value>The art provider.</value>
		public vxSliderArtProvider ArtProvider {get; set;}


        /// <summary>
        /// The Minimum Value of the Slider
        /// </summary>
        public float Min = 0;

        /// <summary>
        /// The Maximum Value of the Slider
        /// </summary>
        public float Max = 1;

        /// <summary>
        /// The Value of the Slider.
        /// </summary>
        public float Value = 0.5f;

		/// <summary>
		/// The previous value to check if anything has changed.
		/// </summary>
		float PreviousValue = 0.5f;

		/// <summary>
		/// The total width.
		/// </summary>
        public int TotalWidth = 150;

		/// <summary>
		/// The total height.
		/// </summary>
        public int TotalHeight = 4;

		/// <summary>
		/// The marker rec.
		/// </summary>
		public Rectangle MarkerRec;

		/// <summary>
		/// The slider position.
		/// </summary>
        public Vector2 SliderPosition = Vector2.Zero;

        /// <summary>
        /// The Incremental value that the Slider can be set to.
        /// </summary>
        public float Tick = 0.01f;

		/// <summary>
		/// Occurs when the value changes.
		/// </summary>
		public event EventHandler<vxValueChangedEventArgs> ValueChanged;


		public vxSlider(float Min, float Max, float Value, Vector2 position) : this(Min, Max, Value, position, 150)
		{

		}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxSlider"/> class.
        /// </summary>
        /// <param name="Engine">The Vertices Engine Reference.</param>
        /// <param name="Min">Minimum.</param>
        /// <param name="Max">Max.</param>
        /// <param name="Value">Value.</param>
        /// <param name="position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
		public vxSlider(float Min, float Max, float Value, Vector2 position, int SliderLength) : base(position)
        {
            //Text
            Text = Value.ToString();
            
            this.Min = Min;
            this.Max = Max;
            this.Value = Value;
			PreviousValue = Value;

            //Set up Font
            Font = vxUITheme.Fonts.Size24;

			TotalWidth = SliderLength;

            SliderPosition = position;

            Width = 16;

            Height = 32;

            //Update Rectangle
            Bounds = new Rectangle(
                (int)(SliderPosition.X),
                (int)(SliderPosition.Y),
                Width, Height);

            MarkerRec = new Rectangle(
                (int)(Position.X),
                (int)(Position.Y + Height / 2 - TotalHeight / 2),
                TotalWidth, TotalHeight);

            SetSliderPosition(Value);

            //Have this button get a clone of the current Art Provider
            this.ArtProvider = (vxSliderArtProvider)vxUITheme.ArtProviderForSlider.Clone ();

            this.OnInitialHover += this_OnInitialHover;
			this.Clicked += this_Clicked;
		}

		private void this_OnInitialHover(object sender, EventArgs e)
        {
			//If Previous Selection = False and Current is True, then Create Highlite Sound Instsance
#if !NO_DRIVER_OPENAL
			PlaySound(vxUITheme.SoundEffects.MenuHover, 0.3f);

#endif
		}

		void this_Clicked (object sender, VerticesEngine.UI.Events.vxUIControlClickEventArgs e)
		{
#if !NO_DRIVER_OPENAL
			PlaySound(vxUITheme.SoundEffects.MenuConfirm, 0.3f);
			#endif
		}

        bool CanTakeInput = false;
        protected internal override void Update()
        {
            base.Update();


            if (CanTakeInput == true && vxInput.MouseState.LeftButton == ButtonState.Released)
                CanTakeInput = false;
            if (HasFocus && vxInput.IsNewMouseButtonPress(MouseButtons.LeftButton))
                CanTakeInput = true;



            if (CanTakeInput)
            {
                // First get the new X component based off of the mouse movement.
                float newX = SliderPosition.X + vxInput.Cursor.X - vxInput.PreviousCursor.X;

                // Next clamp it between the MarkerRec X bounds.
                newX = MathHelper.Clamp(newX, MarkerRec.Left, MarkerRec.Right - Width);

                float mvmntPercentage = (newX - MarkerRec.Left) / ((MarkerRec.Right - Width) - MarkerRec.Left);

                Value = Min + (Max - Min) * mvmntPercentage;

                Value = vxMathHelper.RoundToNearestSpecifiedNumber(Value, Tick);

            }
            // Now re-set the newX value based on the rounded Value
            SetSliderPosition(Value);

            if (PreviousValue != Value)
			{
                OnValueChange(Value, PreviousValue);

                if (ValueChanged != null)
					ValueChanged(this, new vxValueChangedEventArgs(this, Value, PreviousValue));
			}

			PreviousValue = Value;
        }

        protected virtual void OnValueChange(float newValue, float previousValue) { }

        void SetSliderPosition(float value)
        {
            float newX = (value - Min) / (Max - Min) * ((MarkerRec.Right - Width) - MarkerRec.Left) + MarkerRec.Left;
            SliderPosition = new Vector2(newX, SliderPosition.Y);
        }

        /// <summary>
        /// Draws the GUI Item
        /// </summary>
		public override void Draw()
        {
            //Update Rectangle
            Bounds = new Rectangle(
                (int)(SliderPosition.X),
                (int)(Position.Y),
                Width, Height);

            MarkerRec = new Rectangle(
                (int)(Position.X),
                (int)(Position.Y + Height / 2 - TotalHeight / 2),
                TotalWidth, TotalHeight);

            //Now get the Art Provider to draw the scene
            ArtProvider.Draw (this);
        }
    }
}
