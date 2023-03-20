using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine;
using VerticesEngine.UI.Themes;

namespace VerticesEngine.UI.Controls
{
    public class vxRibbonToolbarSplitterControl : vxUIControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxToolbarSplitterControl"/> class.
        /// </summary>
        /// <param name="Ribbon">Ribbon.</param>
        public vxRibbonToolbarSplitterControl(vxRibbonControl Ribbon) : base(Vector2.Zero)
        {
            Ribbon.TitleToolbar.AddItem(this);

            this.Position = Position;

            Padding = new Vector2(4);

            Theme = new vxUIControlTheme(
                new vxColourTheme(new Color(50, 50, 50), new Color(50, 50, 50), new Color(50, 50, 50)));

            Width = 16;
            Height = 24;

        }

        public override void Draw()
        {
            // Draw the Main Background
            SpriteBatch.Draw(DefaultTexture, new Rectangle(Bounds.X + Width / 2, Bounds.Y + 4, 1, Bounds.Height - 8), Color.WhiteSmoke * 0.25f);

            base.Draw();
        }
    }
}
