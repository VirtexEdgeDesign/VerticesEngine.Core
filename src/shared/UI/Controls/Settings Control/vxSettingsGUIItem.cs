using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using VerticesEngine.Graphics;
using VerticesEngine.UI.Dialogs;
using VerticesEngine.UI.Themes;

namespace VerticesEngine.UI.Controls
{
    /// <summary>
    ///  <see cref="VerticesEngine.UI.Controls.vxSettingsGUIItem"/> control which allows for spinning through options
    /// </summary>
	public class vxSettingsGUIItem : vxScrollPanelItem
    {
        
		/// <summary>
		/// Gets the selected index of the Combox Box.
		/// </summary>
		/// <value>The index of the selected.</value>
		public int SelectedIndex
		{
			get { return _selectedIndex; }
            set {
                _selectedIndex = value;
                SetOption();
            }
		}
        int _selectedIndex = 0;


        /// <summary>
        /// Increment Up Button
        /// </summary>
        public vxButtonImageControl IncrementUpButton
        {
            get { return _incrementUpButton; }
        }
        vxButtonImageControl _incrementUpButton;

        /// <summary>
        /// Increment Down Button
        /// </summary>
        public vxButtonImageControl IncrementDownButton
        {
            get { return _incrementDownButton; }
        }
        vxButtonImageControl _incrementDownButton;

        /// <summary>
        /// Returns True if the current SelectedIndex is equal to the first Option
        /// </summary>
        /// <returns></returns>
        public bool ToBool()
        {
            return (SelectedIndex == 0);
        }


        public int ToEnum()
        {
            return (SelectedIndex);
        }



        /// <summary>
        /// Gets the current Value of this setting
        /// </summary>
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
        string _value = "";
        
        Vector2 valuePos;

        Vector2 TitlePos;

        /// <summary>
        /// The arrow spacing which is set externally so that all items have the same width
        /// </summary>
        public static int ArrowSpacing = 100;


        List<object> Options = new List<object>();

        #region - Events -

        public EventHandler<EventArgs> ValueChangedEvent;

        #endregion
        bool wrapInc = true;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxSettingsComboBoxGUIItem"/> class.
        /// </summary>
        /// <param name="Engine">Engine.</param>
        /// <param name="UIManager">GUIM anager.</param>
        /// <param name="Title">Title.</param>
        /// <param name="Value">Value.</param>
        public vxSettingsGUIItem(vxUIManager UIManager, string Name, string InitValue, bool wrapInc = true) :
            base(Name, Vector2.Zero, null, 0)
        {
            this.wrapInc = wrapInc;
            _value = InitValue;
            Height = vxLayout.GetScaledHeight(64);

            if (vxEngine.PlatformType == vxPlatformHardwareType.Mobile)
            {
                Height = vxLayout.GetScaledHeight(128);
            }

            //Set GUI Stuff.
            int ofst = vxLayout.GetScaledSize(8);

            _incrementUpButton = new vxButtonImageControl(vxUITheme.SpriteSheetLoc.ArrowBtnFwd, Vector2.Zero)
            {
                Width = Height - ofst * 2,
                Height = Height - ofst * 2
            };

            _incrementUpButton.Clicked += delegate
            {
                IncrementUp();
            };
            //GUIItemList.Add (Btn_Add);

            _incrementDownButton = new vxButtonImageControl(vxUITheme.SpriteSheetLoc.ArrowBtnBack, Vector2.Zero)
            {
                Width = Height - ofst * 2,
                Height = Height - ofst * 2
            };

            _incrementDownButton.Clicked += delegate
            {
                IncrementDown();
            };

            // Set the Padding and Sizes
            this.Font = vxUITheme.Fonts.Size16;

            Bounds = new Rectangle(0, 0, (int)(vxLayout.Scale.X * 64), Height);
            Padding = new Vector2((int)(vxLayout.Scale.X * 16), (Bounds.Height / 2 - _incrementUpButton.Bounds.Height / 2));


            Theme.Background.HoverColour = Color.DarkOrange;
        }

        public void IncrementUp()
        {
            if (Options.Count == 0)
                return;

            if (wrapInc)
                _selectedIndex = (_selectedIndex + 1) % Options.Count;
            else
                _selectedIndex = MathHelper.Clamp(_selectedIndex + 1, 0, Options.Count - 1);

            SetOption();

            if (ValueChangedEvent != null)
                ValueChangedEvent(this, new EventArgs());
        }

        public void IncrementDown()
        {
            if (Options.Count == 0)
                return;

            if (wrapInc)
            {
                _selectedIndex = (_selectedIndex - 1) % Options.Count;

                if (_selectedIndex < 0)
                    _selectedIndex = Options.Count - 1;
            }
            else
            {
                _selectedIndex = MathHelper.Clamp(_selectedIndex - 1, 0, Options.Count - 1);
            }

            SetOption();

            if (ValueChangedEvent != null)
                ValueChangedEvent(this, new EventArgs());
        }

        void SetOption()
        {
            if (_selectedIndex >= 0 && _selectedIndex < Options.Count)
                _value = Options[_selectedIndex].ToString();
            else
                _value = "Invalid Option";
        }



        public void AddOption(object item)
        {
            Options.Add(item);

            // now check if the arrow width needs to be increased
            ArrowSpacing = Math.Max(ArrowSpacing, (int)(Font.MeasureString(item.ToString()).X * vxLayout.Scale.X + Padding.X * 2));

            if(item.ToString() == Value)
            {
                _selectedIndex = Options.Count - 1;
            }
        }


        protected internal override void Update()
        {
            base.Update();

            // Left Justified
            TitlePos = new Vector2(Bounds.Left + Padding.X, Position.Y + Height / 2);

            UpdateIncrementButtons();
        }

        protected virtual void UpdateIncrementButtons()
        {
            // cenetered
            valuePos = new Vector2(Bounds.Right - Padding.X - _incrementUpButton.Width - ArrowSpacing / 2, Position.Y + Height / 2);

            // Set the Position walking left from the right side
            _incrementUpButton.Position = new Vector2(Bounds.Right - Padding.X - _incrementUpButton.Width, Position.Y + Padding.Y);
            _incrementUpButton.Update();


            _incrementDownButton.Position = new Vector2(Bounds.Right - Padding.X - _incrementUpButton.Width - ArrowSpacing - _incrementDownButton.Width, Position.Y + Padding.Y);
            _incrementDownButton.Update();
        }


        /// <summary>
        /// Draws the GUI Item
        /// </summary>
        public override void Draw()
        {
            TitlePos = new Vector2(Bounds.Left + Padding.X, Position.Y + Height / 2);
            //base.Draw();
            vxGraphics.SpriteBatch.Draw(DefaultTexture, Bounds.GetBorder(1), Color.Black);
            vxGraphics.SpriteBatch.Draw(DefaultTexture, Bounds, Theme.Background.Color);

            //Label.Draw();


            var titleFont = vxUITheme.Fonts.Size16;

            SpriteBatch.DrawString(titleFont, Text, TitlePos + Vector2.One * 2, (HasFocus ? Color.Black * 0.75f : Color.Gray * 0.5f), vxLayout.ScaleAvg, vxHorizontalJustification.Left, vxVerticalJustification.Middle);
            SpriteBatch.DrawString(titleFont, Text, TitlePos, Color.White, vxLayout.ScaleAvg, vxHorizontalJustification.Left, vxVerticalJustification.Middle);
            DrawIncrementButtons();
        }

        protected virtual void DrawIncrementButtons()
        {
            var titleFont = vxUITheme.Fonts.Size16;
            valuePos = new Vector2(Bounds.Right - Padding.X - _incrementUpButton.Width - ArrowSpacing / 2, Position.Y + Height / 2);
            _incrementUpButton.Position = new Vector2(Bounds.Right - Padding.X - _incrementUpButton.Width, Position.Y + Padding.Y);
            _incrementDownButton.Position = new Vector2(Bounds.Right - Padding.X - _incrementUpButton.Width - ArrowSpacing - _incrementDownButton.Width, Position.Y + Padding.Y);

            SpriteBatch.DrawString(titleFont, Value, valuePos + Vector2.One * 2, (HasFocus ? Color.Black * 0.75f : Color.Gray * 0.5f), vxLayout.ScaleAvg, vxHorizontalJustification.Center, vxVerticalJustification.Middle);
            SpriteBatch.DrawString(titleFont, Value, valuePos, Color.White, vxLayout.ScaleAvg, vxHorizontalJustification.Center, vxVerticalJustification.Middle);

            if (true)
            {
                _incrementUpButton.Draw();
                _incrementDownButton.Draw();
            }
        }
    }
}
