using System;
using System.Collections.Generic;
using VerticesEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.UI.Themes;

namespace VerticesEngine.UI.Controls
{
    public class vxRibbonControlGroup : vxPanel
    {
        public vxRibbonTabPage RibbonTabPage;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Virtex.Lib.Iris.Gui.Controls.vxButton"/> class.
        /// </summary>
        /// <param name="RibbonTabPage">Ribbon Tab Page.</param>
        /// <param name="GroupTitle">The Group Title.</param>
        public vxRibbonControlGroup(vxRibbonTabPage RibbonTabPage, string GroupTitle) :
        base(new Rectangle(0, 0, 128, RibbonTabPage.Bounds.Height))
        {
            this.RibbonTabPage = RibbonTabPage;

            Text = GroupTitle;
            RibbonTabPage.Add(this);
            UIManager = RibbonTabPage.UIManager;

            Font = vxInternalAssets.Fonts.ViewerFont;

         Color BackgroundColour = new Color(50, 50, 50, 255);

         Color ForegroundColour = Color.WhiteSmoke * 0.95f;
            Theme = new vxUIControlTheme(
                new vxColourTheme(Color.Transparent, ForegroundColour * 0.2f, ForegroundColour * 0.2f),
                new vxColourTheme(Color.Transparent, ForegroundColour, ForegroundColour));

            Padding = new Vector2(18, 5);

        }

        /// <summary>
        /// Add the specified control.
        /// </summary>
        /// <param name="control">Control.</param>
        public override void Add(vxUIControl control)
        {
            // Add an Item to the list, and then reorganise all items in the panel to place this one at the end.
            base.Add(control);

            // 3 Small Controls per column, or one large control. A Small control is defined as one thats 32px high or less.
            // That is to say, that once the running height is greater than 3 * 32px = 96 px, then the running Length Moves
            // over the distance of the widest element in this column.

            // The Running Heights and Lengths
            float runningHeight = Padding.Y;
            float runningLength = Padding.X;
            float widestElement = 0;


            // Now reorganise all of the items and re-set their local start point.
            foreach (vxUIControl item in Items)
            {
                // First Check what the new height is
                float checkHeight = runningHeight + item.Height + Padding.Y;

                // If the checked height is greater than 96px, then move into the next column
                if (checkHeight > vxRibbonControl.DefaultRibbonHeight - Font.LineSpacing)
                {
                    runningHeight = Padding.Y;
                    runningLength += widestElement + Padding.X;

                    // Now reset the widest Element variable to 0
                    widestElement = 0;
                }

                // Set the Item Positions
                item.Position = new Vector2(runningLength, runningHeight);
                item.OriginalPosition = item.Position;

                // Now add to the runnign height
                runningHeight += item.Height + Padding.Y;

                // now check if this element is the wisest in the column.
                widestElement = Math.Max(widestElement, item.Width);
            }

            // Add the final widest element in the last column to the list
            runningLength += widestElement;

            // Now set the Width of the Control Group as the Running Length + one more Padding
            Width = (int)(runningLength + Padding.X);

        }



        /// <summary>
        /// Draw this instance.
        /// </summary>
        public override void Draw()
        {
            base.Draw();

            // Draw Forward Border
            BorderBounds = new Rectangle(Bounds.X, Bounds.Y + (int)Padding.Y, 1, Bounds.Height - 2 * (int)Padding.Y);

            // Draw the Splitter
            SpriteBatch.Draw(DefaultTexture, BorderBounds, Color.LightGray);

            // Draw the Bottom Text
            SpriteBatch.DrawString(Font, Text, new Vector2(
                (int)(Bounds.X + GetCenteredTextPosition(Font, Text).X),
                (int)(Bounds.Bottom - Font.MeasureString(Text).Y - Padding.Y / 2)),
                                   Color.Gray);

            base.DrawItems();
        }

        public override void DrawItems() { }
    }
}
