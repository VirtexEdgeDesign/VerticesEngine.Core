using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Graphics;
using VerticesEngine.UI.Themes;

namespace VerticesEngine.UI.Controls
{
    /// <summary>
    /// Button which has no text, only an Image.
    /// </summary>
    public class vxButtonImageControl : vxUIControl
    {
        /// <summary>
        /// Gets or sets the button image.
        /// </summary>
        /// <value>The button image.</value>
        public Texture2D ButtonImage;

        /// <summary>
        /// Gets or sets the button image.
        /// </summary>
        /// <value>The button image.</value>
        public Texture2D HoverButtonImage;

        /// <summary>
        /// Gets or sets the blank draw hover background.
        /// </summary>
        /// <value>The draw hover background.</value>
        public bool DrawHoverBackground;

        /// <summary>
        /// The shadow drop offset.
        /// </summary>
        public int ShadowDrop = 4;

        /// <summary>
        /// The color of the shadow.
        /// </summary>
        public Color ShadowColor = Color.Black * 0.5f;

        public vxButtonImageControl(Texture2D buttonImage, Vector2 position) :
        this(buttonImage, buttonImage, position, buttonImage.Width, buttonImage.Height)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VerticesEngine.UI.Controls.vxButtonImageControl"/> class.
        /// </summary>
        /// <param name="Engine">The Vertices Engine Reference.</param>
        /// <param name="position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
        /// <param name="buttonImage">Button image.</param>
        /// <param name="hoverImage">Hover image.</param>
        public vxButtonImageControl(Texture2D buttonImage,Texture2D hoverImage, Vector2 position) :
        this(buttonImage, hoverImage, position, buttonImage.Width, buttonImage.Height)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VerticesEngine.UI.Controls.vxButtonImageControl"/> class.
        /// </summary>
        /// <param name="buttonImage">Button image.</param>
        /// <param name="hoverImage">Hover image.</param>
        /// <param name="position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public vxButtonImageControl(Texture2D buttonImage, Texture2D hoverImage, Vector2 position, int width, int height) : base(position)
        {
            //Set Button Images
            ButtonImage = buttonImage;
            HoverButtonImage = hoverImage;

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
        /// <param name="MainSpriteSheetLocation">Main sprite sheet location.</param>
        /// <param name="position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
		public vxButtonImageControl(Rectangle MainSpriteSheetLocation, Vector2 position) :
        this(MainSpriteSheetLocation, MainSpriteSheetLocation, position, false)
        {
            
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxButtonImage"/> class.
        /// </summary>
        /// <param name="Engine">Engine.</param>
        /// <param name="position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
        /// <param name="MainSpriteSheetLocation">Main sprite sheet location.</param>
        /// <param name="HoverSpriteSheetLocation">Hover sprite sheet location.</param>
		public vxButtonImageControl(Rectangle MainSpriteSheetLocation, Rectangle HoverSpriteSheetLocation, Vector2 position, bool DrawHoverBackground = true) 
        {
            this.DrawHoverBackground = DrawHoverBackground;
            this.MainSpriteSheetLocation = MainSpriteSheetLocation;
            this.HoverSpriteSheetLocation = HoverSpriteSheetLocation;

            UseSpriteSheet = true;

            Init(position);
        }

        void Init(Vector2 position)
        {

            //Set Position
            Position = position;
            OriginalPosition = position;

            //Set Default Colours

            //Default is true
            this.OnInitialHover += Event_InitialHover;
            this.Clicked += OnClicked;

            DrawHoverBackground = !(MainSpriteSheetLocation == HoverSpriteSheetLocation);

            if(DrawHoverBackground)
                Theme.Background = new vxColourTheme(Color.White);
            else
                Theme.Background = new vxColourTheme(Color.White, Color.Gray);

        }

        void OnClicked(object sender, VerticesEngine.UI.Events.vxUIControlClickEventArgs e)
        {
#if !NO_DRIVER_OPENAL
            PlaySound(vxUITheme.SoundEffects.MenuConfirm, 0.3f);
#endif
        }

        private void Event_InitialHover(object sender, EventArgs e)
        {
            //If Previous Selection = False and Current is True, then Create Highlite Sound Instsance
#if !NO_DRIVER_OPENAL
            PlaySound(vxUITheme.SoundEffects.MenuHover, 0.25f);

#endif
        }

        public float Rotation = 0;

        /// <summary>
        /// Draws the GUI Item
        /// </summary>
        public override void Draw()
        {
            if (IsVisible)
            {
                Theme.SetState(this);

                if (UseSpriteSheet)
                {
                    //Draw Regular Image
                    if (DoBorder)
                    {
                        vxGraphics.SpriteBatch.Draw(vxUITheme.SpriteSheet, Bounds.GetBorder(BorderSize), MainSpriteSheetLocation,
                                                (IsEnabled ? Color.Black : Color.Black * 0.5f) * Alpha);
                    }

                    if (IsShadowVisible)
                    {
                        Position += new Vector2(ShadowDrop);

                        vxGraphics.SpriteBatch.Draw(vxUITheme.SpriteSheet,Bounds,
                                                MainSpriteSheetLocation,
                                                ShadowColor * Alpha);
                        Position -= new Vector2(ShadowDrop);
                    }


                    vxGraphics.SpriteBatch.Draw(vxUITheme.SpriteSheet, Bounds,
                                            (HasFocus ? HoverSpriteSheetLocation : MainSpriteSheetLocation),
											Theme.Background.Color * Alpha, Rotation, Vector2.Zero, SpriteEffects.None, 1);

                    //vxGraphics.SpriteBatch.Draw(vxUITheme.SpriteSheet, Bounds, HoverSpriteSheetLocation, Color_Normal * HoverAlpha);

                    if (IsTogglable && ToggleState)
                    {
                        vxGraphics.SpriteBatch.Draw(vxUITheme.SpriteSheet, Bounds, 
                                                HoverSpriteSheetLocation, Theme.Background.Color, Rotation, Vector2.Zero, SpriteEffects.None, 1);
                    }
                }
                else
                {
                    //Draw Regular Image
                    if (ButtonImage != null)
                    {
                        if (DoBorder)
                        {
                            vxGraphics.SpriteBatch.Draw(ButtonImage, Bounds.GetBorder(BorderSize)
                                                    , (IsEnabled ? Color.Black : Color.Black * 0.5f) * Alpha);
                        }

                        if (IsShadowVisible)
                        {
                            vxGraphics.SpriteBatch.Draw(ButtonImage, (Bounds.Location + new Point(ShadowDrop)).ToVector2(), ShadowColor * Alpha);
                        }


                        vxGraphics.SpriteBatch.Draw(ButtonImage, Bounds, Theme.Background.Color * Alpha);

                        if (HoverButtonImage != null)
                            vxGraphics.SpriteBatch.Draw(HoverButtonImage, Bounds, Theme.Background.HoverColour * HoverAlpha * Alpha);
                        else
                            vxGraphics.SpriteBatch.Draw(ButtonImage, Bounds, Theme.Background.HoverColour * Alpha);
                    }

                    if (IsTogglable && ToggleState)
                    {

                        if (HoverButtonImage != null)
                            vxGraphics.SpriteBatch.Draw(HoverButtonImage, Bounds, Theme.Background.NormalColour);
                        else
                            vxGraphics.SpriteBatch.Draw(ButtonImage, Bounds, Theme.Background.HoverColour * Alpha);
                    }
                }
            }
        }
    }
}
