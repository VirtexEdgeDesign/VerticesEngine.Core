using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine;
using Microsoft.Xna.Framework.Audio;
using VerticesEngine.Utilities;
using VerticesEngine.UI.Themes;
using VerticesEngine.Graphics;

namespace VerticesEngine.UI.Controls
{
	/// <summary>
	/// Button which has no text, only an Image.
	/// </summary>
	public class vxToggleImageButton : vxUIControl
	{
		/// <summary>
		/// Gets or sets the button image.
		/// </summary>
		/// <value>The button image.</value>
		public Texture2D OffButtonImage;

		/// <summary>
		/// Gets or sets the button image.
		/// </summary>
		/// <value>The button image.</value>
		public Texture2D OnButtonImage;

		/// <summary>
		/// Gets or sets the blank draw hover background.
		/// </summary>
		/// <value>The draw hover background.</value>
		public bool DrawHoverBackground;

		/// <summary>
		/// Does this have a shadow
		/// </summary>
		public bool DoShadow = false;

		/// <summary>
		/// The shadow drop offset.
		/// </summary>
		public int ShadowDrop = 4;

		/// <summary>
		/// The color of the shadow.
		/// </summary>
		public Color ShadowColor = Color.Black * 0.5f;

		/// <summary>
		/// Initializes a new instance of the <see cref="VerticesEngine.UI.Controls.vxButtonImageControl"/> class.
		/// </summary>
		/// <param name="Engine">The Vertices Engine Reference.</param>
		/// <param name="position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
		/// <param name="buttonImage">Button image.</param>
		/// <param name="hoverImage">Hover image.</param>
		public vxToggleImageButton(Texture2D offImage, Texture2D onImage, Vector2 position) :
		this(offImage,
			 onImage,
                             position,
             offImage.Width,
			 offImage.Height)
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="VerticesEngine.UI.Controls.vxButtonImageControl"/> class.
		/// </summary>
		/// <param name="Engine">The Vertices Engine Reference.</param>
		/// <param name="position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
		/// <param name="buttonImage">Button image.</param>
		/// <param name="hoverImage">Hover image.</param>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		public vxToggleImageButton(Texture2D offImage,Texture2D onImage, Vector2 position, int width, int height) 
		{
			//Set Button Images
			OffButtonImage = offImage;
			OnButtonImage = onImage;

			//Set Initial Bounding Rectangle
			Width = width;
			Height = height;
			//BorderSize = 2;
			Init(position);
		}


		bool UseSpriteSheet = false;
		public Rectangle MainSpriteSheetLocation;
		public Rectangle HoverSpriteSheetLocation;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxButtonImage"/> class.
		/// </summary>
		/// <param name="Engine">Engine.</param>
		/// <param name="position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
		/// <param name="MainSpriteSheetLocation">Main sprite sheet location.</param>
		public vxToggleImageButton(Rectangle MainSpriteSheetLocation, Vector2 position) :
		this(MainSpriteSheetLocation, MainSpriteSheetLocation, position)
		{

		}


		/// <summary>
		/// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxButtonImage"/> class.
		/// </summary>
		/// <param name="Engine">Engine.</param>
		/// <param name="position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
		/// <param name="MainSpriteSheetLocation">Main sprite sheet location.</param>
		/// <param name="HoverSpriteSheetLocation">Hover sprite sheet location.</param>
		public vxToggleImageButton(Rectangle MainSpriteSheetLocation, Rectangle HoverSpriteSheetLocation, Vector2 position)
		{
			this.MainSpriteSheetLocation = MainSpriteSheetLocation;
			this.HoverSpriteSheetLocation = HoverSpriteSheetLocation;

			UseSpriteSheet = true;

			Init(position);
		}
        public float UnFocusAlpha = 1;
		void Init(Vector2 position)
		{

			IsTogglable = true;
			ToggleState = true;

			//Set Position
			Position = position;
			OriginalPosition = position;

			//Set Default Colours
            Theme.Background = new vxColourTheme(Color.White * UnFocusAlpha, Color.White, Color.DeepSkyBlue);

			//Default is true
			DrawHoverBackground = true;
			this.OnInitialHover += OnInitialMenuItemHover;
			this.Clicked += OnImageClicked;
        }

        private void OnInitialMenuItemHover(object sender, EventArgs e)
        {
            //If Previous Selection = False and Current is True, then Create Highlite Sound Instsance
#if !NO_DRIVER_OPENAL
            PlaySound(vxUITheme.SoundEffects.MenuHover, 0.25f);

#endif
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();
            this.OnInitialHover -= OnInitialMenuItemHover;
            this.Clicked -= OnImageClicked;
        }

        void OnImageClicked(object sender, VerticesEngine.UI.Events.vxUIControlClickEventArgs e)
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
			if (IsVisible)
			{
				if (UseSpriteSheet)
				{
					//Draw Regular Image
					if (DoBorder)
					{
						vxGraphics.SpriteBatch.Draw(vxUITheme.SpriteSheet, Bounds.GetBorder(BorderSize), MainSpriteSheetLocation,
												(IsEnabled ? Color.Black : Color.Black * 0.5f) * Alpha);
					}

					if (DoShadow)
					{
						Position += new Vector2(ShadowDrop);

						vxGraphics.SpriteBatch.Draw(vxUITheme.SpriteSheet, Bounds,
												MainSpriteSheetLocation,
												ShadowColor * Alpha);
						Position -= new Vector2(ShadowDrop);
					}


					vxGraphics.SpriteBatch.Draw(vxUITheme.SpriteSheet, Bounds.GetBorder(vxLayout.Scale), MainSpriteSheetLocation,
											Theme.Background.NormalColour * Alpha * (HasFocus ? 0.750f : 1.00f));

					//vxGraphics.SpriteBatch.Draw(vxUITheme.SpriteSheet, Bounds, HoverSpriteSheetLocation, Color_Normal * HoverAlpha);

					if (IsTogglable && ToggleState)
					{
                        vxGraphics.SpriteBatch.Draw(vxUITheme.SpriteSheet, Bounds.GetBorder(vxLayout.Scale), HoverSpriteSheetLocation, Theme.Background.NormalColour * (HasFocus ? 0.750f : 1.00f));
					}
				}
				else
				{
                    Texture2D CurrentTexture = ToggleState ? OnButtonImage : OffButtonImage;
					if (true)
                        SpriteBatch.Draw(CurrentTexture, Bounds.GetBorder(vxLayout.Scale).GetBorder(BorderSize)
                                         , (HasFocus ? Color.Black : Color.Black * UnFocusAlpha) * Alpha);

					if (DoShadow)
                        SpriteBatch.Draw(CurrentTexture, (Bounds.Location + new Point(ShadowDrop)).ToVector2(), ShadowColor * Alpha);

					//ItemTheme
                    SpriteBatch.Draw(CurrentTexture,Bounds, GetStateColour(Theme.Background) * Alpha);
                    //SpriteBatch.Draw(CurrentTexture,Bounds.Location.ToVector2(), null, GetStateColour(Theme.Background) * Alpha, 0, Vector2.Zero, vxLayout.Scale, SpriteEffects.None, 0);
				}
			}
		}
	}
}
