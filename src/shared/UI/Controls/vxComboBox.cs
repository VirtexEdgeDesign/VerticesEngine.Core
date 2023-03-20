using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.ContentManagement;
using VerticesEngine.Graphics;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Events;
using VerticesEngine.UI.Themes;


namespace VerticesEngine.UI.Events
{
    /// <summary>
    /// Event Args for Combo Box Selection Change
    /// </summary>
    public class vxComboBoxSelectionChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public vxComboBoxSelectionChangedEventArgs(vxComboBoxItem comboBoxItem)
        {
            this.comboBoxItem = comboBoxItem;
        }


        /// <summary>
        /// Gets the Currently Selected vxComboBoxItem Item that is associated with the Selection change.
        /// </summary>
        public vxComboBoxItem SelectedItem
        {
            get { return comboBoxItem; }
        }
        vxComboBoxItem comboBoxItem;

        /// <summary>
        /// Gets the Currently Selected vxComboBoxItem Index that is associated with the Selection change.
        /// </summary>
        public int SelectedIndex
        {
            get { return comboBoxItem.Index; }
        }
    }
}


namespace VerticesEngine.UI.Controls
{
    /// <summary>
    /// Provides a ComboBox Control which can be populated be vxComboBoxItems.
    /// </summary>
    public class vxComboBox : vxUIControl
    {
        protected Texture2D DropDownArrow;

        public vxEnumTextHorizontalJustification TextJustification = vxEnumTextHorizontalJustification.Center;

        /// <summary>
        /// The choices in the combo box.
        /// </summary>
		public List<vxComboBoxItem> Choices = new List<vxComboBoxItem>();

		public vxComboBoxItem SelectedItem
		{
			get { return Choices.ToArray()[SelectedIndex]; }
		}

        //Padding for Item Seperation
        public int ItemPadding = 6;

        /// <summary>
		/// Selected Index
        /// </summary>
        public int SelectedIndex = 0;

        /// <summary>
        /// Decides Whether or not too Display List
        /// </summary>
        protected bool DisplayList = false;

        /// <summary>
        /// Event Raised when the selected item is changed.
        /// </summary>
        public event EventHandler<vxComboBoxSelectionChangedEventArgs> SelectionChanged;


        /// <summary>
        /// The drop down backing.
        /// </summary>
        public Color DropDownBacking = Color.Black * 0.85f;

        /// <summary>
        /// The height of the line.
        /// </summary>
        protected int LineHeight = 16;


        /// <summary>
        /// The text position.
        /// </summary>
        protected Vector2 TextPosition = new Vector2();

        protected virtual SpriteFont GetFont ()
        {
            return vxUITheme.Fonts.Size24;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxComboBox"/> class.
        /// </summary>
        /// <param name="Engine">Engine.</param>
        /// <param name="text">Text.</param>
        /// <param name="position">Position.</param>
		public vxComboBox(string text, Vector2 position):base(position)
        {
            Text = text;

			this.Font = GetFont();

            LineHeight = vxLayout.GetScaledHeight(Font.LineSpacing);

            Width = 150;
            Height = 128;


            this.PositionChanged+= OnComboBoxPositionChanged;
			this.Clicked += OnComboBoxClicked;

            Padding = new Vector2(2);

            Theme.Background = new vxColourTheme(
                Color.DarkOrange,
                Color.DarkOrange * 1.25f,
                Color.DeepSkyBlue);

            Theme.Text = new vxColourTheme(
                Color.Black,
                Color.Black * 1.05f,
                Color.Black);

            DoBorder = true;

            DropDownArrow = vxInternalAssets.UI.ComboBoxArrowDown;
        }

        /// <summary>
        /// Add's a String to the Combo Box
        /// </summary>
        /// <param name="Item"></param>
        public virtual void AddItem(string Item)
        {
            vxComboBoxItem item = new vxComboBoxItem(Item, Choices.Count, new Vector2((int)(Position.X), Bounds.Bottom + (Choices.Count) * LineHeight));

            item.Clicked += OnItemClicked;

            Choices.Add(item);

            if (this.Text == Item)
                SelectedIndex = Choices.Count - 1;

            SetItemPositions();
        }



        /// <summary>
        /// Sets the item positions.
        /// </summary>
        protected void SetItemPositions()
        {
            for(int y = 0; y < Choices.Count; y++)
            {
                Choices[y].Position = new Vector2(Position.X, Bounds.Bottom + y * Choices[y].Bounds.Height);
            }
        }



        #region Events

        protected void OnComboBoxClicked (object sender, vxUIControlClickEventArgs e)
		{
			DisplayList = !DisplayList;
            CaptureInput = DisplayList;

			//if (PositionInvalided == true) {
                SetItemPositions();
			//	PositionInvalided = false;
			//}
		}


        protected void OnComboBoxPositionChanged (object sender, EventArgs e)
        {
			
        }


        /// <summary>
        /// Called when a combo box item is selected.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected virtual void OnItemClicked(object sender, vxUIControlClickEventArgs e)
        {
            SelectedIndex = e.GUIitem.Index;
            Text = e.GUIitem.Text;

            // Raise the Clicked event.
            if (SelectionChanged != null)
                SelectionChanged(this, new vxComboBoxSelectionChangedEventArgs((vxComboBoxItem)e.GUIitem));


            //Close the list
            DisplayList = false;
            CaptureInput = DisplayList;
        }

        #endregion


        /// <summary>
        /// Updates the GUI Item
        /// </summary>
        protected internal override void Update()
        {
            //Update Each Button
            if (DisplayList)
            {
                foreach (vxComboBoxItem btn in Choices)
                {
                    btn.TextJustification = TextJustification;

                    btn.Font = this.Font;
                    btn.Opacity = this.Opacity;
                    btn.Width = Width;
                    btn.Height = Height;
                    btn.Update();
                }
                SetItemPositions();
            }
            switch(TextJustification)
            {
                case vxEnumTextHorizontalJustification.Left:
                    TextPosition = new Vector2(Position.X + Padding.X, (int)(Position.Y + Height / 2 - vxLayout.GetScaledSize(Font.MeasureString(Text).Y) / 2));
                    break;
                case vxEnumTextHorizontalJustification.Center:
                    TextPosition = new Vector2(Position.X + (Width - Height) / 2 - vxLayout.GetScaledSize(Font.MeasureString(Text).X) / 2 - Padding.X, 
                                               Position.Y + Height / 2 - LineHeight / 2);
                    break;
            }

            base.Update();

        }

		public override void Draw()
        {
            base.Draw();
            
            switch (TextJustification)
            {
                case vxEnumTextHorizontalJustification.Left:
                    TextPosition = new Vector2(Position.X + Padding.X, (int)(Position.Y + Height / 2 - vxLayout.GetScaledSize(Font.MeasureString(Text).Y) / 2));
                    break;
                case vxEnumTextHorizontalJustification.Center:
                    TextPosition = new Vector2(Position.X + (Width - Height) / 2 - vxLayout.GetScaledSize(Font.MeasureString(Text).X) / 2 - Padding.X,
                                               Position.Y + Height / 2 - LineHeight / 2);
                    break;
            }


            //Draw Each Button
            if (DisplayList)
            {
                UIManager.HasFocus = true;

                //First Draw the Backing for the drop down items but only if there is at least one item.
                if (Choices.Count > 0)
                {
                    Rectangle backing = new Rectangle(
                    (int)(Bounds.X - Padding.X),
                    (int)(Bounds.Y - Padding.Y),
                    (int)(Bounds.Width + Padding.X * 2),
                        (int)(Choices[Choices.Count - 1].Bounds.Bottom - Bounds.Top + Padding.Y * 2));

                    SpriteBatch.Draw(DefaultTexture, backing, DropDownBacking);
                }

                foreach (vxComboBoxItem btn in Choices)
                {
                    btn.Draw();
                }
                IsSelected = true;
            }

            //Draw Button
            if (DoBorder)
                SpriteBatch.Draw(DefaultTexture, Bounds.GetBorder(1), GetStateColour(Theme.Border));

            SpriteBatch.Draw(DefaultTexture, Bounds, GetStateColour(Theme.Background) * Opacity);            

			// Drawtge Arraow
            Rectangle bnds = new Rectangle(Bounds.Right - Height, Bounds.Y-1, Height, Height);
			SpriteBatch.Draw(DropDownArrow, bnds, Color.White * (IsEnabled ? 1.0f : 0.25f));
        }

        public override void DrawText()
		{
			//SpriteBatch.DrawString(Font, Text, TextPosition, GetStateColour(Theme.Text) * Opacity, vxLayout.Scale);
			SpriteBatch.DrawString(Font, Text, TextPosition - Vector2.UnitY * 2, GetStateColour(Theme.Text) * Opacity);
		}

        public override void OnLayoutInvalidated()
        {
            base.OnLayoutInvalidated();
        }
    }

    

    /// <summary>
    /// Drop down Combo Box GUI Control.
    /// </summary>
    public class vxComboBoxItem : vxUIControl
    {
        public vxEnumTextHorizontalJustification TextJustification = vxEnumTextHorizontalJustification.Center;

        /// <summary>
        /// The text position.
        /// </summary>
        protected Vector2 TextPosition = new Vector2();

        /// <summary>
        /// The height of the line.
        /// </summary>
        int LineHeight = 16;

        protected virtual SpriteFont GetFont()
        {
            return vxUITheme.Fonts.Size12;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxComboBoxItem"/> class.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="Index">Index.</param>
        /// <param name="position">Position.</param>
        public vxComboBoxItem(string text, int Index, Vector2 position) 
        {
            // This will be set anyways by the Owning Combox Control
            Font = GetFont();

            LineHeight = vxLayout.GetScaledHeight(Font.LineSpacing);

            Text = text;
            this.Index = Index;
            Position = position;

            int val = vxLayout.GetScaledSize(Font.MeasureString(Text).X) + (int)Padding.Y * 2;
            Width = Math.Max(100, val);
            Height = (int)(LineHeight + Padding.Y);


            Bounds = new Rectangle(
                (int)(Position.X - Padding.X),
                (int)(Position.Y - Padding.Y / 2),
                Width,
                Height);

            Theme = new vxUIControlTheme(
                new vxColourTheme(Color.White * 0.1f, Color.White * 0.25f),
                new vxColourTheme(Color.White * 0.75f, Color.White));
        }

        /// <summary>
        /// Draws the GUI Item
        /// </summary>
        public override void Draw()
        {
            base.Draw();

            switch (TextJustification)
            {
                case vxEnumTextHorizontalJustification.Left:
                    TextPosition = new Vector2(Position.X + Padding.X, Position.Y - 1);
                    break;
                case vxEnumTextHorizontalJustification.Center:
                    TextPosition = new Vector2(Position.X + Width / 2 - vxLayout.GetScaledSize(Font.MeasureString(Text).X / 2) - Padding.X, Position.Y - 1);
                    break;
            }

            Vector2 txtSize = Font.MeasureString(Text);

            //get the unscaled txt height relative to the actual height
            float txtSizeHeight = Bounds.Height / txtSize.Y * 0.9f;

            //Draw Button
            vxGraphics.SpriteBatch.Draw(DefaultTexture, Bounds.GetBorder(-1), Theme.Background.Color);
            //vxGraphics.SpriteBatch.DrawString(Font, Text, Bounds.Center.ToVector2() + Vector2.UnitY, Theme.Text.Color, vxLayout.Scale, txtSize/2);
            vxGraphics.SpriteBatch.DrawString(Font, Text, (Bounds.Center.ToVector2() + 3*Vector2.UnitY).ToIntValue(), Theme.Text.Color, Vector2.One * txtSizeHeight, (txtSize / 2).ToIntValue());
        }
    }
}
