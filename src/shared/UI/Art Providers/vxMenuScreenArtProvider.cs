using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using VerticesEngine.Graphics;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Menus;

namespace VerticesEngine.UI.Themes
{
    /// <summary>
    /// The Art Provider for Menu Screens. If you want to customize the draw call, then create an inherited class
    /// of this one and override this draw call. 
    /// </summary>
	public class vxMenuScreenArtProvider : vxArtProviderBase, IGuiArtProvider
    {

        /// <summary>
        /// Gets or sets the menu start position.
        /// </summary>
        /// <value>The menu start position.</value>
        public Vector2 MenuStartPosition
        {
            get { return _menuStartPosition; }
            set { _menuStartPosition = value; }
        }
        Vector2 _menuStartPosition = new Vector2(200, 200);
        //Vector2 position = new Vector2(0, 0);

        /// <summary>
        /// Gets or sets the offset between Menu Item
        /// </summary>
        /// <value>The offset for the next menu item.</value>
        public Vector2 NextMenuItemOffset
        {
            get { return nextMenuItemOffset; }
            set { nextMenuItemOffset = value; }
        }
        Vector2 nextMenuItemOffset = new Vector2(0, 0);


        /// <summary>
        /// Title Position.
        /// </summary>
        public Vector2 TitlePosition
        {
            get { return titlePosition; }
            set { titlePosition = value; }
        }
        public Vector2 titlePosition = new Vector2(0, 0);

		/// <summary>
		/// Gets or sets the color of the title.
		/// </summary>
		/// <value>The color of the title.</value>
		public Color TitleColor;

		/// <summary>
		/// Gets or sets the title padding.
		/// </summary>
		/// <value>The title padding.</value>
		public Vector2 TitlePadding;

        public bool IsTitleVisible = true;

        /// <summary>
        /// Is there a background image on the title
        /// </summary>
        public bool IsTitleBackgroundVisible = true;



       /// <summary>
        /// The title font.
        /// </summary>
        /// <value>The title font.</value>
        public SpriteFont TitleFont
        {
            get { return GetTitleFont(); }
        }

        /// <summary>
        /// Gets the title font. GUITheme.FontTitle is the default. Override this function to provide your own custom fonts.
        /// </summary>
        /// <returns>The title font.</returns>
        protected virtual SpriteFont GetTitleFont()
        {
            return vxUITheme.Fonts.Size36;
        }

		/// <summary>
		/// Gets or sets the color of the title background.
		/// </summary>
		/// <value>The color of the title background.</value>
		public Color TitleBackgroundColor;

        public Rectangle TitleBoundingRectangle = new Rectangle();

		/// <summary>
		/// Should the title text have a shadow
		/// </summary>
		public bool IsTitleTextShadowVisible = false;

		/// <summary>
		/// The title shadow padding.
		/// </summary>
		public Vector2 TitleShadowPadding = new Vector2(4);

		/// <summary>
		/// The title shadow alpha.
		/// </summary>
		public float TitleShadowAlpha = 0.5f;

		/// <summary>
		/// The text justification.
		/// </summary>
        public vxEnumTextHorizontalJustification TextJustification;


		/// <summary>
		/// Event Raised before art provider draws anything
		/// </summary>
		public event EventHandler<EventArgs> OnPreDraw;

		/// <summary>
		/// Event Raised after the art provider draws stuff
		/// </summary>
		public event EventHandler<EventArgs> OnPostDraw;



        /// <summary>
        /// Constructor for the Menu Screen Art Provider.
        /// </summary>
        /// <param name="Engine"></param>
		public vxMenuScreenArtProvider() : base()
        {

			//Set up default values
			_menuStartPosition = new Vector2(200, 200);
            titlePosition = new Vector2(vxGraphics.GraphicsDevice.Viewport.Width / 2, 80);
			TitleColor = Color.Black;
			TitleBackgroundColor = Color.White;
			TitlePadding = new Vector2 (10, 10);

			//TitleBackground = Engine.InternalContentManager.Load<Texture2D>("Gui/DfltThm/vxUITheme/vxMenuScreen/Bckgrnd_Nrml");
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

		public virtual void SetTitlePosition(vxMenuBaseScreen MenuScreen)
		{
			TitlePosition = new Vector2(vxGraphics.GraphicsDevice.Viewport.Width / 2, 80) - vxUITheme.Fonts.Size24.MeasureString (MenuScreen.MenuTitle) / 2;
		}

        public virtual void OnNewMenuStart(vxMenuBaseScreen MenuScreen)
        {

        }

        /// <summary>
        /// The Draw Method for the Menu Item Art Provider. If you want to customize the draw call, then create an inherited class
        /// of this one and override this draw call. 
        /// </summary>
        /// <param name="guiItem"></param>
		public virtual void Draw(object guiItem)
        {
			vxMenuBaseScreen MenuScreen = (vxMenuBaseScreen)guiItem;


			GraphicsDevice graphics = vxGraphics.GraphicsDevice;
			vxSpriteBatch spriteBatch = vxGraphics.SpriteBatch;
			//SpriteFont font = vxUITheme.Fonts.Size24;

			SetTitlePosition(MenuScreen);

            // make sure our entries are in the right place before we draw them
            UpdateMenuEntryLocations(MenuScreen);


            // Raise the Pre Draw event.
            if (OnPreDraw != null)
                OnPreDraw(this, new EventArgs());

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            //float transitionOffset = (float)Math.Pow(MenuScreen.TransitionPosition, 2);

            if (IsTitleVisible)
            {
                float titleScale = vxLayout.ScaleAvg;


                TitleBoundingRectangle = new Rectangle(
                    (int)(TitlePosition.X - TitlePadding.X),
                    (int)(TitlePosition.Y - TitlePadding.Y),
                    (int)(TitleFont.MeasureString(MenuScreen.MenuTitle).X * vxLayout.Scale.X + TitlePadding.X * 2),
                    (int)(TitleFont.MeasureString(MenuScreen.MenuTitle).Y * vxLayout.Scale.Y + TitlePadding.Y * 2));

                if (IsTitleBackgroundVisible && MenuScreen.MenuTitle != "")
                {
                    vxGraphics.SpriteBatch.Draw(vxUITheme.SpriteSheet,
                        TitleBoundingRectangle,
                                            TitleSpriteSheetRegion,
                                            TitleBackgroundColor * MenuScreen.TransitionAlpha);
                }

                if (IsTitleTextShadowVisible)
                    spriteBatch.DrawString(TitleFont, MenuScreen.MenuTitle, TitlePosition + TitleShadowPadding,
                        Color.Black * MenuScreen.TransitionAlpha * TitleShadowAlpha, 0, Vector2.Zero, titleScale, SpriteEffects.None, 0);

                spriteBatch.DrawString(TitleFont, MenuScreen.MenuTitle, TitlePosition,
                    TitleColor * MenuScreen.TransitionAlpha, 0, Vector2.Zero, titleScale, SpriteEffects.None, 0);
            }
			// Raise the Post Draw event.
			if (OnPostDraw != null)
				OnPostDraw(this, new EventArgs());
        }


        /// <summary>
        /// Updates the Position of all Menu Entries. For custom layout, override this method.
        /// </summary>
        protected virtual void UpdateMenuEntryLocations(vxMenuBaseScreen MenuScreen)
        {
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(MenuScreen.TransitionPosition, 2);

            //Set the Top Menu Start Position
            var position = _menuStartPosition;

            // update each menu entry's location in turn
            for (int i = 0; i < MenuScreen.MenuEntries.Count; i++)
            {
                vxMenuEntry menuEntry = MenuScreen.MenuEntries[i];
                NextMenuItemOffset = new Vector2(0, menuEntry.Height + Margin.Y);

                //Set Menu Item Location
                if (TextJustification == vxEnumTextHorizontalJustification.Left)
                    position.X = _menuStartPosition.X;// -vxMenuEntry.Width / 2;
                else if (TextJustification == vxEnumTextHorizontalJustification.Center)
                    position.X = _menuStartPosition.X - menuEntry.Width / 2;// * vxLayout.ScaleAvg;
                else if (TextJustification == vxEnumTextHorizontalJustification.Right)
                    position.X = _menuStartPosition.X - menuEntry.Width;
				

                if (MenuScreen.ScreenState == ScreenState.TransitionOn)
                    position.X -= transitionOffset * 256;
                else
                    position.X += transitionOffset * 512;

                // set the entry's position
                menuEntry.Position = position;

                // move down for the next entry the size of this entry
                position += NextMenuItemOffset;

                //this.vxInput.ShowCursor = true;
            }
        }
    }
}
