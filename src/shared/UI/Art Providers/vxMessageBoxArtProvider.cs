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
using VerticesEngine.UI.MessageBoxs;
using VerticesEngine.Graphics;

namespace VerticesEngine.UI.Themes
{    /// <summary>
     /// The Art Provider for Menu Screen Items. If you want to customize the draw call, then create an inherited class
     /// of this one and override this draw call. 
     /// </summary>
	public class vxMessageBoxArtProvider : vxArtProviderBase, IGuiArtProvider
    {
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

        public SpriteFont TitleFont { get; protected set; }

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
        /// Gets or sets the form bounding rectangle.
        /// </summary>
        /// <value>The form bounding rectangle.</value>
        public Rectangle FormBounds;// {get; set;}

        //public Vector2 PosOffset = new Vector2(0);


		/// <summary>
		/// Initializes a new instance of the <see cref="VerticesEngine.UI.Themes.vxMenuItemArtProvider"/> class.
		/// </summary>
		/// <param name="Engine">Engine.</param>
		public vxMessageBoxArtProvider() : base()
        {

			TitleAlpha = 1;
			Alpha = 1;

			TitlePadding = new Vector2 (10, 5) * vxLayout.ScaleAvg;
			Padding = new Vector2 (10, 5) * vxLayout.ScaleAvg;

			TitleBackgroundColour = Color.DarkOrange;
            TitleTextColour = Color.Black;

			BackgroundImage = DefaultTexture;

            //Set Font
            Theme = new vxUIControlTheme(
                new vxColourTheme(Color.Black * 0.75f, Color.DarkOrange, Color.DeepSkyBlue),
                new vxColourTheme(Color.White, Color.Black, Color.Black));

        }

		/// <summary>
		/// Clone this instance.
		/// </summary>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

		/// <summary>
		/// Gets the size of the text.
		/// </summary>
		/// <returns>The text size.</returns>
		/// <param name="text">This GUI Items Text.</param>
		public Vector2 GetTextSize(string text)
		{
			if (Font != null) {
				return Font.MeasureString (text);
			} else
				return new Vector2 (10, 10);
		}

        public Vector2 GetTitleSize(string text)
        {
            if (TitleFont != null)
            {
                return TitleFont.MeasureString(text);
            }
            else
                return new Vector2(10, 10);
        }

        public virtual SpriteFont GetTitleFont()
        {
            return vxUITheme.Fonts.Size24;
        }



        public Vector2 TitleTextSize;
        public Vector2 TextSize;
        public Vector2 TextPosition;
        public Vector2 TitleTextPosition;
        //public Vector2 TitleSize;
        public int ButtonBuffer = 0; 
        /// <summary>
        /// The Draw Method for the Menu Screen Art Provider. If you want to customize the draw call, then create an inherited class
        /// of this one and override this draw call. 
        /// </summary>
        /// <param name="guiItem"></param>
        public virtual void Draw(object guiItem)
        {
			//First Cast the GUI Item to be a Menu Entry
			vxMessageBox msgBox = (vxMessageBox)guiItem;

            TitleFont = GetTitleFont();

            TitleTextSize = GetTitleSize(msgBox.Title);

            int buttonWidth = 0;
            switch(msgBox.ButtonTypes)
            {
                case vxEnumButtonTypes.Ok:
                    buttonWidth = (int)(msgBox.OKButton.Width + Padding.X);
                        break;
                case vxEnumButtonTypes.OkCancel:
                    buttonWidth = (int)(msgBox.OKButton.Width + msgBox.CancelButton.Width + Padding.X);
                        break;
                case vxEnumButtonTypes.OkApplyCancel:
                    buttonWidth = (int)(msgBox.OKButton.Width + msgBox.CancelButton.Width + msgBox.ApplyButton.Width + Padding.X * 2);
                    break;
            }

            //int btnCnt = msgBox.ButtonTypes == vxEnumButtonTypes.OkApplyCancel ? 3 : 2;
            
            // Center the message text in the viewport.
            TextSize = Font.MeasureString(msgBox.Message) * vxLayout.ScaleAvg;
            int formWidth = (int)(Math.Max(buttonWidth, TextSize.X) + 2 * Padding.X);


            TextPosition = (Viewport.Bounds.Size.ToVector2() - new Vector2(formWidth, TextSize.Y)) / 2;

            int btnHeight = msgBox.ButtonTypes == vxEnumButtonTypes.None ? 0 : 1;

            

			FormBounds = new Rectangle (
                (int)(TextPosition.X - Padding.X),
                (int)(TextPosition.Y - Padding.Y),
                (int)(formWidth),
                (int)(TextSize.Y + 2 * Padding.Y + ButtonBuffer + btnHeight * msgBox.OKButton.Height + Padding.Y));

			TitleBounds = new Rectangle (
                (int)(FormBounds.X),
                (int)(FormBounds.Top - TitleTextSize.Y - 2 * TitlePadding.Y),
                (int)(FormBounds.Width),
				(int)(TitleTextSize.Y + 2 * TitlePadding.Y));


            TitlePosition = TitleBounds.Location.ToVector2() + TitlePadding;

            //SetButtonPositions(msgBox);

            // Draw the message box text.
            DrawBackground(msgBox);

            DrawText(msgBox);

            DrawTitle(msgBox);
        }

        public virtual void DrawBackground(vxMessageBox msgBox)
        {
            vxGraphics.SpriteBatch.Draw(BackgroundImage, FormBounds, Theme.Background.Color * Alpha * msgBox.TransitionAlpha);
        }


        public virtual void DrawText(vxMessageBox msgBox)
        {
            vxGraphics.SpriteBatch.DrawString(Font, msgBox.Message, TextPosition.ToIntValue(),
                                          Theme.Text.Color * Alpha * msgBox.TransitionAlpha,
                                         0, Vector2.Zero,
                                         vxLayout.ScaleAvg,
                                         SpriteEffects.None, 1);
        }


        public virtual void DrawTitle(vxMessageBox msgBox)
        {
            // Draw the Title
            vxGraphics.SpriteBatch.Draw(vxUITheme.SpriteSheet, TitleBounds, TitleSpriteSheetRegion, TitleBackgroundColour * TitleAlpha * msgBox.TransitionAlpha);

            if (DoTitleTextShadow)
                vxGraphics.SpriteBatch.DrawString(TitleFont, msgBox.Title, TitlePosition + TitleShadowPadding,
                    Color.Black * msgBox.TransitionAlpha * TitleShadowAlpha,
                                         0, Vector2.Zero,
                                         vxLayout.ScaleAvg,
                                         SpriteEffects.None, 1);


            vxGraphics.SpriteBatch.DrawString(TitleFont, msgBox.Title, TitlePosition.ToIntValue(), TitleTextColour * TitleAlpha * msgBox.TransitionAlpha,
                                         0, Vector2.Zero,
                                         vxLayout.ScaleAvg,
                                         SpriteEffects.None, 1);

        }
    }
}
