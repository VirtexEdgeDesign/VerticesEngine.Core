using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Utilities;
using VerticesEngine;
using VerticesEngine.Input;
using VerticesEngine.Graphics;

namespace VerticesEngine.UI.Controls
{
    /// <summary>
    /// Key binding settings GUII tem.
    /// </summary>
	public class vxKeyBindingSettingsGUIItem : vxUIControl
    {
        public vxLabel Label;
        public vxButtonControl Button;

		// Is this gui item waiting to take in the next key press.
		bool TakingInput = false;

		public vxKeyBinding KeyBinding;

		public object BindingID;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxKeyBindingSettingsGUIItem"/> class.
        /// </summary>
        /// <param name="Engine">Engine.</param>
        /// <param name="UIManager">GUI Manager.</param>
        /// <param name="Title">Title.</param>
        /// <param name="KeyBinding">Key binding.</param>
        /// <param name="position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
        public vxKeyBindingSettingsGUIItem(vxUIManager UIManager, string Title, vxKeyBinding KeyBinding, object id, Vector2 position)
			: base( position)
        {
            UIManager.Add(this);

			this.KeyBinding = KeyBinding;

			BindingID = id;

            Label = new vxLabel(Title, position + new Vector2(10, 5));
            UIManager.Add(Label);

			Button = new vxButtonControl(KeyBinding.Key.ToString(), position + new Vector2(200, 10));
			Button.Clicked += delegate {
				TakingInput = true;
				Button.Text = "Press Any Key...";
			};
            UIManager.Add(Button);

            Height = 40;
        }

		/// <summary>
		/// Draws the GUI Item
		/// </summary>
        public override void Draw()
        {
            base.Draw();

			if (TakingInput)
			{
				if (Keyboard.GetState().GetPressedKeys().Length > 0)
				{
					Keys newKey = Keyboard.GetState().GetPressedKeys()[0];

					if(newKey != Keys.Escape && newKey != Keys.OemTilde)
						KeyBinding.Key = newKey;

					TakingInput = false;

					Button.Text = KeyBinding.Key.ToString();
				}
			}


            //Update Rectangle
            int length = 354;
            Bounds = new Rectangle((int)(Position.X), (int)(Position.Y), length, Height);
          
            //
            //Draw Button
            vxGraphics.SpriteBatch.Draw(vxInternalAssets.Textures.Blank, Bounds, Color.Black * 0.5f);
        }
    }
}
