using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VerticesEngine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using VerticesEngine.UI.Themes;

namespace VerticesEngine.UI.Controls
{
	public enum vxEnumButtonSize
	{
		Small,
		Big
	}

	public enum vxEnumTextHorizontalJustification
	{
		Left,
		Center,
		Right
	}

    /// <summary>
    /// Basic Button GUI Control.
    /// </summary>
    public class vxButtonControl : vxUIControl
    {
        public static int DefaultWidth = 64;
        public static int DefaultHeight = 24;


        /// <summary>
        /// Gets or sets the texture for this Menu Entry Background.
        /// </summary>
        /// <value>The texture.</value>
        public Texture2D BackgroundTexture;

        /// <summary>
        /// The icon for this button.
        /// </summary>
        public Texture2D Icon;


        public Rectangle SpriteSheetRegion = new Rectangle();

        public vxEnumButtonSize ButtonSize = vxEnumButtonSize.Small;

        public vxEnumTextHorizontalJustification TextHorizontalJustification = vxEnumTextHorizontalJustification.Center;

        /// <summary>
        /// Should the width be set by using the min width or the length of the text?
        /// </summary>
        public bool UseDefaultWidth = true;


        public vxButtonControl(string localTextKey, Vector2 Position, int Width, int Height)
            :this(localTextKey, Position)
        {
            LocalisationKey = localTextKey;

            this.Width = (int)(Width * vxLayout.Scale.X);
            this.Height = (int)(Height * vxLayout.Scale.Y);

            UseDefaultWidth = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxButton"/> class.
        /// </summary>
        /// <param name="Engine">Engine.</param>
        /// <param name="Text">Text.</param>
        /// <param name="Position">Position.</param>
        /// <param name="Width">Width.</param>
        /// <param name="Height">Height.</param>
        //public vxButtonControl(string Text, Vector2 Position, int Width, int Height) :
        //this(Text, Position)
        //{
        //    this.Width = (int)(Width * vxLayout.Scale.X);
        //    this.Height = (int)(Height * vxLayout.Scale.Y);

        //    UseDefaultWidth = false;
        //}

        //public vxButtonControl(string localTextKey, Vector2 Position) :this(vxLocalizer.Language[localTextKey], Position)
        //{
        //    LocalisationKey = localTextKey;
        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="VerticesEngine.UI.Controls.vxButtonControl"/> class.
        /// </summary>
        /// <param name="localTextKey">BUtton Text.</param>
        /// <param name="Position">Button Position.</param>
        public vxButtonControl(string localTextKey, Vector2 Position) : base(Position)
        {
            //Text
            LocalisationKey = localTextKey;
            this.Text = vxLocalizer.GetText(localTextKey);

            //Set up Font
            Font = vxUITheme.Fonts.Size24;

            DoBorder = true;

            //Have this button get a clone of the current Art Provider
			Width = (int)(Math.Max(vxUITheme.ArtProviderForButtons.DefaultWidth, (int)(this.Font.MeasureString(Text).X + Padding.X * 2)) * vxLayout.Scale.X);


			OnInitialHover += this_OnInitialHover;
			Clicked += this_Clicked;
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

        /// <summary>
        /// Draws the GUI Item
        /// </summary>
        public override void Draw()
        {
            //Now get the Art Provider to draw the scene
            vxUITheme.ArtProviderForButtons.DrawUIControl(this);

            base.Draw();
        }
    }
}
