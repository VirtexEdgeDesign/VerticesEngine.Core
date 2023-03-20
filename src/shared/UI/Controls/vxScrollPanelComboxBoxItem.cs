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
    /// Toolbar control that holds <see cref="VerticesEngine.UI.Controls.vxScrollPanelComboxBoxItem"/> 
    /// </summary>
	public class vxScrollPanelComboxBoxItem : vxScrollPanelItem
    {
        vxLabel Label;

        public vxComboBox ValueComboBox;

		/// <summary>
		/// Gets the selected index of the Combox Box.
		/// </summary>
		/// <value>The index of the selected.</value>
		public int SelectedIndex
		{
			get { return ValueComboBox.SelectedIndex; }
		}


		/// <summary>
		/// Gets the selected item in the Combo Box control.
		/// </summary>
		/// <value>The selected item.</value>
		public vxComboBoxItem SelectedItem
		{
			get { return ValueComboBox.SelectedItem; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxSettingsComboBoxGUIItem"/> class.
		/// </summary>
		/// <param name="Engine">Engine.</param>
		/// <param name="UIManager">GUIM anager.</param>
		/// <param name="Title">Title.</param>
		/// <param name="Value">Value.</param>
		public vxScrollPanelComboxBoxItem(vxUIManager UIManager, string Title, string Value) :
            this(UIManager, Title, Value, Vector2.Zero)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxSettingsComboBoxGUIItem"/> class.
        /// </summary>
        /// <param name="Engine">Engine.</param>
        /// <param name="UIManager">GUIM anager.</param>
        /// <param name="Title">Title.</param>
        /// <param name="Value">Value.</param>
        /// <param name="position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
		public vxScrollPanelComboxBoxItem(vxUIManager UIManager, string Title, string Value, Vector2 Position): 
            base("", Vector2.Zero, null, 0)
        {
            Label = new vxLabel(Title, Position + new Vector2(10, 5));
            Label.UIManager = UIManager;
            Label.Font = vxUITheme.Fonts.Size12;

            ValueComboBox = new vxComboBox(Value, Position + new Vector2(275, 10));
            ValueComboBox.UIManager = UIManager;

            Height = 40;
        }

        public void AddOption(string item)
        {
            ValueComboBox.AddItem(item);
        }

        protected internal override void Update()
        {
            base.Update();

            Label.Update();
            ValueComboBox.Update();
        }

        /// <summary>
        /// Draws the GUI Item
        /// </summary>
        public override void Draw()
        {
            //base.Draw();
            //
            //Draw Button
            //
            Label.Position = Label.OriginalPosition + Position;
            ValueComboBox.Position = new Vector2(Bounds.Right - ValueComboBox.Width - Padding.X, Bounds.Y + Height/2 - 10);
            vxGraphics.SpriteBatch.Draw(DefaultTexture, Bounds.GetBorder(1), Color.Black);
            vxGraphics.SpriteBatch.Draw(DefaultTexture, Bounds, Theme.Background.Color);

            Label.Draw();
            ValueComboBox.Draw();
        }

        public override void DrawText()
        {
            Label.DrawText();
            ValueComboBox.DrawText();
        }
    }
}
