﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VerticesEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.UI.Themes;
using VerticesEngine.Graphics;
using VerticesEngine.UI.Controls;

namespace VerticesEngine.UI
{
    /// <summary>
    /// Art provider base class which holds common elements such as Padding, Highlight colour etc...
    /// </summary>
    [System.Obsolete("You should upgrade this Art Provider to use vxUIArtProvider<T>")]
    public class vxArtProviderBase
	{
        /// <summary>
        /// Gets the graphics device.
        /// </summary>
        /// <value>The graphics device.</value>
        public GraphicsDevice GraphicsDevice
        {
            get { return vxGraphics.GraphicsDevice; }
        }


        /// <summary>
        /// Gets the viewport.
        /// </summary>
        /// <value>The viewport.</value>
        public Viewport Viewport
        {
            get { return vxScreen.Viewport; }
        }

        /// <summary>
        /// Gets the gui theme.
        /// </summary>
        /// <value>The theme.</value>
        public vxUITheme GUITheme
        {
            get { return vxEngine.Instance.GUITheme; }
        }

        /// <summary>
        /// Gets the sprite batch.
        /// </summary>
        /// <value>The sprite batch.</value>
        public vxSpriteBatch SpriteBatch
        {
            get { return vxGraphics.SpriteBatch; }
        }

        /// <summary>
        /// Gets or sets the margin.
        /// </summary>
        /// <value>The margin.</value>
        public Vector4 Margin;

        /// <summary>
        /// Gets or sets the padding.
        /// </summary>
        /// <value>The padding.</value>
        public Vector2 Padding;

        /// <summary>
        /// The alpha transparency value.
        /// </summary>
        public float Alpha;

        /// <summary>
        /// This controls whether or not the control should be drawn using regions of the GUI
        /// Sprite Sheet.
        /// </summary>
        public bool UseSpriteSheet = false;


		/// <summary>
		/// The sprite sheet region.
		/// </summary>
		public Rectangle SpriteSheetRegion;

        /// <summary>
        /// The title sprite sheet region.
        /// </summary>
        public Rectangle TitleSpriteSheetRegion = new Rectangle(0, 0, 4, 4);

		/// <summary>
		/// Gets or sets the opacity of the current GUI Item.
		/// </summary>
		/// <value>The opacity.</value>
		public float Opacity
		{
			get { return opacity; }
			set { opacity = value; }
		}
		float opacity = 1;

		/// <summary>
		/// Text Of GUI Item
		/// </summary>
		public SpriteFont Font
        { 
            get {return GetFont(); }
        }

        public virtual SpriteFont GetFont()
        {
            return vxUITheme.Fonts.Size16;
        }

        /// <summary>
        /// Gets or sets the background image.
        /// </summary>
        /// <value>The background image.</value>
        public Texture2D BackgroundImage;

		/// <summary>
		/// Gets or sets a value indicating whether this
		/// <see cref="VerticesEngine.UI.Themes.vxMenuItemArtProvider"/> draw backgroung image.
		/// </summary>
		/// <value><c>true</c> if draw backgroung image; otherwise, <c>false</c>.</value>
		public bool DrawBackgroungImage;

		/// <summary>
		/// Gets or sets the width of the border.
		/// </summary>
		/// <value>The width of the border.</value>
		public int BorderWidth;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="VerticesEngine.UI.vxArtProviderBase"/> do border.
		/// </summary>
		/// <value><c>true</c> if do border; otherwise, <c>false</c>.</value>
		public bool DoBorder;

		/// <summary>
		/// Gets or sets the default width.
		/// </summary>
		/// <value>The default width.</value>
		public int DefaultWidth;

		/// <summary>
        /// The default height.
        /// </summary>
		public int DefaultHeight;


        /// <summary>
        /// The colour theme for this specific Art Provider.
        /// </summary>
        public vxUIControlTheme Theme;


        /// <summary>
        /// Gets the default texture.
        /// </summary>
        /// <value>The default texture.</value>
        public Texture2D DefaultTexture
        {
            get { return vxInternalAssets.Textures.Blank; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.vxArtProviderBase"/> class.
        /// </summary>
        /// <param name="Engine">Engine.</param>
		public vxArtProviderBase()
		{
            DefaultWidth = 150;//(int)(150 * vxLayout.ScreenSizeScaler);
            DefaultHeight = 24;//(int)(24 * vxLayout.ScreenSizeScaler);

            SpriteSheetRegion = new Rectangle(0, 0, 4, 4);

			Alpha = 1;

            Theme = new vxUIControlTheme();
		}


		/// <summary>
		/// Many sizes and positions are set based off of screensize,
		/// although this can change if a user changes the resolution 
		/// settings. Therefore, this SetBounds() method will be where the
		/// sizes will be set.
		/// </summary>
		public virtual void SetBounds()
		{

		}
	}
}
