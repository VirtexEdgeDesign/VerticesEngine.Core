using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine;
using VerticesEngine.ContentManagement;
using VerticesEngine.Graphics;
using VerticesEngine.UI.Themes;

namespace VerticesEngine.UI.Controls
{
	/// <summary>
	/// Ribbon control.
	/// </summary>
    public class vxRibbonContextualTabPage : vxRibbonTabPage
    {
        string GroupName;

        Color GroupColor;

        public bool IsAdded = false;

        Texture2D gradient;

        public vxRibbonContextualTabPage(vxRibbonControl RibbonControl, string GroupName, string PageTitle, Color GroupColor) : 
        base(RibbonControl, PageTitle)
		{
            this.GroupName = GroupName;
            this.GroupColor = GroupColor;

            gradient = vxInternalAssets.UI.RibbonGradientVertical;
		}

        public override void DrawTab()
        {
            base.DrawTab();

            Rectangle topBounds = Tab.Bounds;
            topBounds.Location -= new Point(0, Tab.Bounds.Height);
            topBounds.Height += topBounds.Height-2;

            vxGraphics.SpriteBatch.Draw(gradient, topBounds, GroupColor * Opacity);

            Point TxtPos = new Point((int)(Tab.Position.X + Tab.Width / 2 - Font.MeasureString(GroupName).X / 2 - Padding.X),
                                     (int)(topBounds.Y + Tab.Height / 2 - Font.MeasureString(GroupName).Y / 2));
            
            vxGraphics.SpriteBatch.DrawString(this.Font, GroupName, TxtPos.ToVector2(), Color.WhiteSmoke * Opacity);
        }

        public override void Draw()
        {
            base.Draw();
        }

        public override void DrawItems()
        {
            vxGraphics.SpriteBatch.Draw(gradient, Bounds, null, GroupColor * 0.25f,
                                    0, Vector2.Zero, SpriteEffects.FlipVertically, 0);
            base.DrawItems();
        }
	}
}
