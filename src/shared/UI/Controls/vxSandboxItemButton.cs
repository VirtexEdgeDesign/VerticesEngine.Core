using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine;
using VerticesEngine.Utilities;
using VerticesEngine.UI.Themes;
using VerticesEngine.Graphics;

namespace VerticesEngine.UI.Controls
{
	public enum vxEnumClickType
	{
		OnPress,
		OnRelease
	}

    public class vxSandboxItemButton : vxUIControl
    {
		/// <summary>
		/// Gets or sets the button image.
		/// </summary>
		/// <value>The button image.</value>
		public Texture2D ButtonImage {get; set;}

        public string Key;

		//Vector2 TextSize = new Vector2();

		public vxSandboxItemButton(){
		}


        public vxSandboxItemButton(Texture2D ButtonImage, string Text, string ElementKey, Vector2 Position, int Width, int Height)
        {
            this.Position = Position;

			Font = vxInternalAssets.Fonts.ViewerFont;
            this.Text = Text;

            this.Key = ElementKey;

            this.ButtonImage = ButtonImage;
            Bounds = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);


            this.Width = Width;
            this.Height = Height;
			Padding = new Vector2(2);

			ClickType = vxEnumClickType.OnRelease;
			this.Clicked += OnButtonClicked;

        }

        protected override void OnDisposed()
        {
            this.Clicked -= OnButtonClicked;
            base.OnDisposed();
            //ButtonImage.Dispose();
            ButtonImage = null;
        }

        /// <summary>
        /// This method is fired when the button is clicked. Override this to access the ButtonClicked Event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public virtual void OnButtonClicked(object sender, Events.vxUIControlClickEventArgs e)
		{
			
		}


		/// <summary>
		/// Draw this instance.
		/// </summary>
		public override void Draw()
        {
			//if (IsSelected)
				//Console.WriteLine("Selected");
			// Draw the Back Image
            //vxGraphics.SpriteBatch.Draw(DefaultTexture, BorderBounds, HasFocus ? Color.DeepSkyBlue : Color.Black);
			vxGraphics.SpriteBatch.Draw(DefaultTexture, BorderBounds, GetStateColour(Theme.Background));
            //vxGraphics.SpriteBatch.Draw(ButtonImage, Bounds, Theme.Background.Color);
            vxGraphics.SpriteBatch.Draw(ButtonImage, Bounds, Color.White);
            

            if (Text != null)
            {
                int BackHeight = (int)(Bounds.Height - vxInternalAssets.Fonts.DebugFont.MeasureString(Text).Y - 10);

				vxGraphics.SpriteBatch.Draw(DefaultTexture,
                    new Rectangle(
                        Bounds.Location.X,
                        Bounds.Location.Y + BackHeight,
                        Bounds.Width,
                        Bounds.Height - BackHeight),
                    Color.Black * 0.5f);

            }
        }

        public override void DrawText()
        {
			vxGraphics.SpriteBatch.DrawString(
				Font,
				Text,
				new Vector2(
					(int)(Bounds.Location.X +Bounds.Width/2 - TextSize.X/2),
					Bounds.Location.Y + (int)(Bounds.Height - vxInternalAssets.Fonts.DebugFont.MeasureString(Text).Y - 10) + 5),
				Color.LightGray);
        }
    }
}
