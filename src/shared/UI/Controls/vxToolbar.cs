using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Utilities;
using VerticesEngine;
using VerticesEngine.UI.Themes;

namespace VerticesEngine.UI.Controls
{
	/// <summary>
	/// Toolbar control that holds <see cref="VerticesEngine.UI.Controls.vxToolbarButton"/> 
	/// </summary>
    public class vxToolbar : vxUIControl
    {
        /// <summary>
        /// List of Toolbar Items
        /// </summary>
        public List<vxUIControl> ToolbarItems = new List<vxUIControl>();

        /// <summary>
        /// Gets or sets the art provider.
        /// </summary>
        /// <value>The art provider.</value>
        public vxToolbarArtProvider ArtProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="VerticesEngine.UI.Controls.vxToolbar"/> class.
        /// </summary>
        /// <param name="position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
        public vxToolbar(Vector2 position): base(position)
        {
			Height = 32+2;

            HoverAlphaMax = 0.75f;
            HoverAlphaMin = 0.5f;
            HoverAlphaDeltaSpeed = 10;

            //Have this button get a clone of the current Art Provider
            ArtProvider = (vxToolbarArtProvider)vxUITheme.ArtProviderForToolbars.Clone();

        }

		/// <summary>
		/// When the GUIItem is Selected
		/// </summary>
        public override void Select()
        {
            HasFocus = true;
        }

		/// <summary>
		/// Adds the item.
		/// </summary>
		/// <param name="guiItem">GUI item.</param>
        public void AddItem(vxUIControl guiItem)
        {
            int tempPosition = (int)Padding.X;
            //First Set Position
            foreach (vxUIControl bsGuiItm in ToolbarItems)
            {
                tempPosition += bsGuiItm.Bounds.Width + (int)Padding.X;
                //Console.WriteLine("{0} : {1}", bsGuiItm.ToString() ,bsGuiItm.Width);
            }

            guiItem.Position = new Vector2(tempPosition + Padding.X / 2, Position.Y - guiItem.Bounds.Height + Padding.X);

            ToolbarItems.Add(guiItem);
        }

        /// <summary>
        /// Updates the GUI Item
        /// </summary>
        protected internal override void Update()
        {
            base.Update();

			Vector2 tempPosition = this.Position + new Vector2(Padding.X, Padding.Y);
            foreach (vxUIControl bsGuiItm in ToolbarItems)
			{
				//Set Position
				bsGuiItm.Position = tempPosition;
				bsGuiItm.Update();


				//Incrememnet Up the Position
				tempPosition += new Vector2(bsGuiItm.Width + Padding.X, 0);
            }
        }

		/// <summary>
		/// Draws the GUI Item
		/// </summary>
		public override void Draw()
        {
            base.Draw();


            //Now get the Art Provider to draw the scene
            this.ArtProvider.Draw(this);

            base.Draw();

            // Now draw each of the Toolbar Items.
            foreach (vxUIControl bsGuiItm in ToolbarItems)
            {
                bsGuiItm.Draw();
            }
        }
    }
}
