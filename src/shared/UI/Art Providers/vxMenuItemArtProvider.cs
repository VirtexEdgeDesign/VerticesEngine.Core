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
using VerticesEngine.UI.Themes;
using VerticesEngine.Graphics;

namespace VerticesEngine.UI.Themes
{    /// <summary>
     /// The Art Provider for Menu Screen Items. If you want to customize the draw call, then create an inherited class
     /// of this one and override this draw call. 
     /// </summary>
	public class vxMenuItemArtProvider : vxArtProviderBase, IGuiArtProvider
    {
        /// <summary>
        /// Defines whether or not the icon should be shown. The default is false.
        /// </summary>
        public bool ShowIcon;

        /// <summary>
        /// Icon Padding.
        /// </summary>
        public Vector2 IconPadding
        {
            get { return _iconPadding;  }
            set { _iconPadding = value; }
        }
        private Vector2 _iconPadding = new Vector2(4);




		/// <summary>
		/// Initializes a new instance of the <see cref="VerticesEngine.UI.Themes.vxMenuItemArtProvider"/> class.
		/// </summary>
		public vxMenuItemArtProvider():base()
        {

			SpriteSheetRegion = new Rectangle(0, 0, 4, 4);
			ShowIcon = false;
            Padding = new Vector2 (10, 4) * vxLayout.Scale;

            Theme = new vxUIControlTheme(
                new vxColourTheme(Color.White, Color.DarkOrange, Color.DeepSkyBlue),
                new vxColourTheme(Color.Black),
            new vxColourTheme(Color.Black));


			DrawBackgroungImage = true;

			//BackgroundImage = Engine.InternalContentManager.Load<Texture2D>("Gui/DfltThm/vxUITheme/vxMenuEntry/Bckgrnd_Nrml");
        }


        public object Clone()
        {
            return this.MemberwiseClone();
        }


        /// <summary>
        /// The Draw Method for the Menu Screen Art Provider. If you want to customize the draw call, then create an inherited class
        /// of this one and override this draw call. 
        /// </summary>
        /// <param name="guiItem"></param>
        public virtual void Draw(object guiItem)
        {
            //First Cast the GUI Item to be a Menu Entry
            vxMenuEntry menuEntry = (vxMenuEntry)guiItem;

            SpriteFont font = menuEntry.Font;
            Vector2 Size = font.MeasureString(menuEntry.Text) * vxLayout.Scale;

            Theme.SetState(menuEntry);

            //Padding = new Vector2(0);
            //Update Rectangle
            menuEntry.Bounds = new Rectangle(
                (int)(menuEntry.Position.X - Size.X / 2 - Padding.X),
                (int)(menuEntry.Position.Y - Size.Y / 2 - Padding.Y),
                (int)(Size.X + 2 * Padding.X),
                (int)(Size.Y + 2 * Padding.Y));

            //Set Opacity from Parent Screen Transition Alpha
            menuEntry.Opacity = menuEntry.ParentScreen.TransitionAlpha;

            //Do a last second null check.
            if (menuEntry.Texture == null)
                menuEntry.Texture = vxInternalAssets.Textures.Blank;

            //Draw Button
            //if (DrawBackgroungImage)
                //vxGraphics.SpriteBatch.Draw(BackgroundImage, menuEntry.Bounds, menuEntry.Colour * menuEntry.Opacity);

			if (DrawBackgroungImage)
                vxGraphics.SpriteBatch.Draw(vxUITheme.SpriteSheet, menuEntry.Bounds, SpriteSheetRegion, Theme.Background.Color * menuEntry.Opacity);


            vxGraphics.SpriteBatch.DrawString(
                font,
                menuEntry.Text ,
                menuEntry.Position,
				Theme.Text.Color * menuEntry.Opacity,
            0,
                font.MeasureString(menuEntry.Text) / 2,
            vxLayout.ScaleAvg,
            SpriteEffects.None,
            1);
        }
    }
}
