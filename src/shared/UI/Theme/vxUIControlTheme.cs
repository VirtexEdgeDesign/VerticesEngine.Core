using System;
using Microsoft.Xna.Framework;

namespace VerticesEngine.UI
{
	/// <summary>
	/// Item Theme
	/// </summary>
	public class vxUIControlTheme
	{
        /// <summary>
        /// The background colour theme.
        /// </summary>
		public vxColourTheme Background = new vxColourTheme();

        /// <summary>
        /// The text colour theme.
        /// </summary>
		public vxColourTheme Text = new vxColourTheme(Color.WhiteSmoke, Color.Black, Color.Black);


		/// <summary>
		/// The border colour theme.
		/// </summary>
		public vxColourTheme Border = new vxColourTheme(Color.Black, Color.Black, Color.Black);

        /// <summary>
        /// Whether or not this theme should have a border.
        /// </summary>
        public bool DoBorder = true;

		public vxUIControlTheme()
		{

		}

		public vxUIControlTheme(vxColourTheme Background)
		{
			this.Background = Background;
		}

		public vxUIControlTheme(vxColourTheme Background, vxColourTheme Text)
		{
			this.Background = Background;
			this.Text = Text;
		}

        public vxUIControlTheme(vxColourTheme Background, vxColourTheme Text, vxColourTheme Border)
        {
            this.Background = Background;
            this.Text = Text;
            this.Border = Border;
        }

        /// <summary>
        /// Sets the Colors based off the current state.
        /// </summary>
        /// <param name="item">Item.</param>
        public void SetState(vxUIControl item)
        {
            Text.State = item.State;
            Background.State = item.State;
            Border.State = item.State;
        }
	}
}
