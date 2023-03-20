using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using VerticesEngine;
using VerticesEngine.UI.Controls;
using VerticesEngine.Utilities;
using VerticesEngine.UI.Dialogs;
using VerticesEngine.Graphics;

namespace VerticesEngine.UI.Themes
{
	public class vxDialogArtProvider : vxArtProviderBase, IGuiArtProvider
	{

		#region Title Properties
		/// <summary>
		/// Gets or sets the title background image.
		/// </summary>
		/// <value>The title background image.</value>
		public Texture2D TitleBackgroundImage {get; set;}

		/// <summary>
		/// Gets or sets the title text colour.
		/// </summary>
		/// <value>The title text colour.</value>
		public Color TitleTextColour;

		/// <summary>
		/// Gets or sets the color of the title background.
		/// </summary>
		/// <value>The color of the title background.</value>
		public Color TitleBackgroundColour;

		/// <summary>
		/// Gets or sets the title alpha.
		/// </summary>
		/// <value>The title alpha.</value>
		public float TitleAlpha;

		/// <summary>
		/// Gets or sets the form bounding rectangle.
		/// </summary>
		/// <value>The form bounding rectangle.</value>
		public Rectangle TitleBounds {get; set;}

		/// <summary>
		/// Gets or sets the title padding.
		/// </summary>
		/// <value>The title padding.</value>
		public Vector2 TitlePadding;

		/// <summary>
		/// Gets or sets the title position.
		/// </summary>
		/// <value>The title position.</value>
		public Vector2 TitlePosition;

		/// <summary>
		/// Should the title text have a shadow
		/// </summary>
		public bool DoTitleTextShadow = false;

		/// <summary>
		/// The title shadow padding.
		/// </summary>
		public Vector2 TitleShadowPadding = new Vector2(4);

		/// <summary>
		/// The title shadow alpha.
		/// </summary>
		public float TitleShadowAlpha = 0.5f;


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
        public virtual SpriteFont GetTitleFont()
        {
            return vxUITheme.Fonts.Size36;
        }

		#endregion


		#region Main Form Properties


		/// <summary>
		/// This is the bounding rectangle which all controls should be kept within.
		/// </summary>
		public Rectangle GUIBounds;

		/// <summary>
		/// Gets or sets the border of that background rectangle.
		/// </summary>
		/// <value>The brdr background rectangle.</value>
		public Rectangle brdr_backgroundRectangle;

        /// <summary>
        /// This is the bounding rectangle of the entire Dialog Form.
		/// </summary>
		/// <remarks>X, Y, Z, W = Left, Top, Right, Bottom</remarks>
		public Rectangle FormBounds;

		/// <summary>
		/// The position offset for the gui panel.
		/// </summary>
		public Vector2 PosOffset = new Vector2 (0);

		#endregion

		//public Viewport Viewport;
		public Vector2 TitleTextSize;

        // Buffer between the Buttons and the GUI Bounds
        public int ButtonBuffer = 0;

		public vxDialogArtProvider() : base()
        {
			DefaultWidth = 150;
			DefaultHeight = 24;

			Alpha = 1;

			TitleTextColour = Color.Black;
			TitleAlpha = 1;
			TitlePadding = new Vector2 (10, 10);
			TitleBackgroundColour = Color.DarkOrange;

			Padding = new Vector2 (10, 10);

			Margin = new Vector4 (0);

			DoBorder = true;
            BorderWidth = 2;
			BackgroundImage = vxInternalAssets.Textures.Blank;
			TitleBackgroundImage = vxInternalAssets.Textures.Blank;


            Theme = new vxUIControlTheme(
                new vxColourTheme(Color.Black * 0.75f),
                new vxColourTheme(Color.Black));


            TitleTextSize = vxUITheme.Fonts.Size24.MeasureString("A");

			SetBounds();
		}

		public override void SetBounds()
		{
			base.SetBounds();

			TitleBounds = new Rectangle(
				(int)Padding.X,
				(int)Padding.Y,
				(int)(Viewport.Width - Padding.X * 2),
				(int)(TitleTextSize.Y + Padding.Y * 2));
            
			TitlePosition = new Vector2(
				TitleBounds.X,
				TitleBounds.Y + Padding.Y);

			GUIBounds = new Rectangle(
				(int)(TitlePosition.X),
				(int)(TitlePosition.Y + TitleBounds.Height),
				(int)(Viewport.Width - Padding.X * 2),
				(int)(Viewport.Height - Padding.Y - TitleBounds.Height - TitlePosition.Y));

			FormBounds = new Rectangle(
				(int)(GUIBounds.X - Margin.X),
				(int)(GUIBounds.Y - Margin.Y),
				(int)(GUIBounds.Width + Margin.X + Margin.Z),
				(int)(GUIBounds.Height + Margin.Y + Margin.W));
		}


		public object Clone()
		{
			return this.MemberwiseClone();
		}

        public virtual void SetButtonPositions(vxDialogBase dialog)
        {
            // reposition buttons
            if (dialog.IsCustomButtonPosition == false)
            {
                // reposition buttons
                dialog.CancelButton.Position =
                          new Vector2(GUIBounds.Right - dialog.CancelButton.Width,
                                      GUIBounds.Bottom - dialog.CancelButton.Height) - Padding / 2;

                dialog.OKButton.Position =
                          new Vector2(GUIBounds.Right - dialog.CancelButton.Width - dialog.OKButton.Width - Padding.X / 4,
                                      GUIBounds.Bottom - dialog.CancelButton.Height) - Padding / 2;

                dialog.ApplyButton.Position =
                          new Vector2(GUIBounds.Right - dialog.CancelButton.Width - dialog.OKButton.Width - dialog.ApplyButton.Width - Padding.X / 2,
                                      GUIBounds.Bottom - dialog.CancelButton.Height) - Padding / 2;
            }
        }

		public virtual void Draw(object guiItem)
		{
			vxDialogBase dialog = (vxDialogBase)guiItem;

			// Center the message text in the viewport.
			//viewport = vxGraphics.GraphicsDevice.Viewport;
			TitleTextSize = TitleFont.MeasureString(dialog.Title);

            SetButtonPositions(dialog);

            // Darken down any other screens that were drawn beneath the popup.
            //Engine.FadeBackBufferToBlack(dialog.TransitionAlpha * 2 / 3);


            // Draw the message box text.
            vxGraphics.SpriteBatch.Draw(BackgroundImage, FormBounds, Theme.Background.Color);

            // Draw the Title
            vxGraphics.SpriteBatch.Draw(TitleBackgroundImage, TitleBounds.GetBorder(BorderWidth), Color.Black);
            vxGraphics.SpriteBatch.Draw(TitleBackgroundImage, TitleBounds, TitleBackgroundColour);
            vxGraphics.SpriteBatch.DrawString(TitleFont, dialog.Title, TitlePosition, TitleTextColour);
		}
	}
}

