using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using VerticesEngine.Graphics;
using VerticesEngine.UI.Themes;

namespace VerticesEngine.UI.Controls
{

    /// <summary>
    /// Toolbar Button Controls for the vxToolbar Class.
    /// </summary>
    public class vxToolbarButton : vxUIControl
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


        bool UseSpriteSheet = false;
        public Rectangle MainSpriteSheetLocation;
        public Rectangle HoverSpriteSheetLocation;

        public vxToolbarButton(Rectangle MainSpriteSheetLocation) :
        this(MainSpriteSheetLocation,
             new Rectangle(MainSpriteSheetLocation.X,
                           MainSpriteSheetLocation.Y + MainSpriteSheetLocation.Height,
                           MainSpriteSheetLocation.Width,
                           MainSpriteSheetLocation.Height))
        {

        }


        public vxToolbarButton(Rectangle MainSpriteSheetLocation, Rectangle HoverSpriteSheetLocation)
        {
            this.MainSpriteSheetLocation = MainSpriteSheetLocation;
            this.HoverSpriteSheetLocation = HoverSpriteSheetLocation;

            UseSpriteSheet = true;

            Width = MainSpriteSheetLocation.Width;
            Height = MainSpriteSheetLocation.Height;

            Init();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VerticesEngine.UI.Controls.vxToolbarButton"/> class. Note the texutres
        /// are loaded by the Games Content manager.
        /// </summary>
        /// <param name="TexturesPath">Path to the textures, note a 'hover texture' must be present with a '_hover' suffix</param>
        public vxToolbarButton(string TexturesPath) : this(TexturesPath, vxEngine.Instance.CurrentScene.SceneContent)
        {

        }

        /// <summary>
		/// Initializes a new instance of the <see cref="VerticesEngine.UI.Controls.vxToolbarButton"/> class.
        /// </summary>
		/// <param name="Engine">The current Vertices vxEngine Instance</param>
		/// <param name="Content">Content Manager too load the Textures with.</param>
        /// <param name="TexturesPath">Path to the textures, note a 'hover texture' must be present with a '_hover' suffix</param>
		public vxToolbarButton(string TexturesPath, ContentManager Content)
        {            
            //Set Button Images
            if (TexturesPath != "")
            {
                ButtonImage = Content.Load<Texture2D>(TexturesPath);

                try
                {
                    HoverButtonImage = Content.Load<Texture2D>(TexturesPath + "_hover");
                }
                catch
                {
                    HoverButtonImage = Content.Load<Texture2D>(TexturesPath + "_Hover");
                }
                //Set Initial Bounding Rectangle
                Width = ButtonImage.Width;
                Height = ButtonImage.Height;
                Bounds = new Rectangle(0, 0, Width, Height);
            }
            Init();
        }

        void Init()
        {

            //Position is Set by Toolbar
            Position = Vector2.Zero;

            //Setup initial Events to handle mouse sounds
            this.OnInitialHover += Event_OnInitialHover;
            this.Clicked += Event_OnClicked;

            Theme.Background = new vxColourTheme(Color.White);
        }

        private void Event_OnInitialHover(object sender, EventArgs e)
        {
            //If Previous Selection = False and Current is True, then Create Highlite Sound Instsance
#if !NO_DRIVER_OPENAL

            if (IsEnabled)
            {
                PlaySound(vxUITheme.SoundEffects.MenuHover, 0.3f);
            }
#endif
        }

        void Event_OnClicked(object sender, VerticesEngine.UI.Events.vxUIControlClickEventArgs e)
        {
#if !NO_DRIVER_OPENAL

            if (IsEnabled)
            {
                PlaySound(vxUITheme.SoundEffects.MenuConfirm, 0.3f);
            }
#endif


        }

        /// <summary>
        /// Draws the GUI Item
        /// </summary>
        public override void Draw()
        {
            base.Draw();

            Theme.SetState(this);

            if (UseSpriteSheet)
            {
                //Draw Regular Image
                vxGraphics.SpriteBatch.Draw(vxUITheme.SpriteSheet, Bounds, MainSpriteSheetLocation, Theme.Background.Color);

                if (IsEnabled)
                {
                    //Draw Hover Items
                    //vxGraphics.SpriteBatch.Draw (Engine.Assets.Textures.Blank, BoundingRectangle, Color_Highlight * HoverAlpha);

                    vxGraphics.SpriteBatch.Draw(vxUITheme.SpriteSheet, Bounds, HoverSpriteSheetLocation, Theme.Background.Color * HoverAlpha);

                    if (IsTogglable && ToggleState)
                    {
                        vxGraphics.SpriteBatch.Draw(vxUITheme.SpriteSheet, Bounds, HoverSpriteSheetLocation, Theme.Background.NormalColour);
                    }
                }
            }
            else
            {
                //Draw Regular Image
                vxGraphics.SpriteBatch.Draw(ButtonImage, Bounds, Theme.Background.Color);

                if (IsEnabled)
                {

                    //Draw Hover Items
                    //vxGraphics.SpriteBatch.Draw (Engine.Assets.Textures.Blank, BoundingRectangle, Color_Highlight * HoverAlpha);

                    if (HoverButtonImage != null)
                        vxGraphics.SpriteBatch.Draw(HoverButtonImage, Bounds, Theme.Background.NormalColour * HoverAlpha);

                    if (IsTogglable && ToggleState)
                    {

                        if (HoverButtonImage != null)
                            vxGraphics.SpriteBatch.Draw(HoverButtonImage, Bounds, Theme.Background.NormalColour);
                    }
                }
            }
        }
    }
}
