using Microsoft.Xna.Framework;
using VerticesEngine.Graphics;

namespace VerticesEngine.UI.Controls
{
    /// <summary>
    /// Properties scroll panel control.
    /// </summary>
    public class vxPropertiesScrollPanelControl : vxScrollPanel
    {
        public vxLineBatch LineBatch;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxPropertiesScrollPanelControl"/> class.
		/// </summary>
		/// <param name="Engine">Engine.</param>
		/// <param name="Position">Position.</param>
		/// <param name="Width">Width.</param>
		/// <param name="Height">Height.</param>
        public vxPropertiesScrollPanelControl(Vector2 Position, int Width, int Height)
            : base(Position, Width, Height)
        {
            Padding = new Vector2(0);

            DoBorder = true;

            ArtProvider.Theme.Background.NormalColour = Color.Black * 0.25f;

            LineBatch = new vxLineBatch(vxGraphics.GraphicsDevice);
        }


        public override void ResetLayout()
        {
            Vector2 RunningLength = Padding + new Vector2(0, 0);// Position;

            foreach (var item in Items)
            {
                // Set position
                item.Width = this.Width - (int)Padding.X * 10 - this.ScrollBarWidth;
                item.Position = RunningLength + Padding;//+ new Vector2(1, 0);
                item.OriginalPosition = item.Position;
                RunningLength += Vector2.UnitY * item.Height;// + Padding;

                item.ResetLayout();
                BorderBounds.Width = this.Width;
            }

            SetScrollLength();
			OnPanelScroll(0);
        }

        public override void AddItem(vxUIControl guiItem)
        {
            base.AddItem(guiItem);

            ResetLayout();
            Update();
        }


        public override void DrawBorder()
        {
            LineBatch.Begin(Matrix.CreateOrthographicOffCenter(0f,
                                                               (vxGraphics.GraphicsDevice.Viewport.Width), (vxGraphics.GraphicsDevice.Viewport.Height),
                                                               0f, 0f, 1f),
                           Matrix.Identity);


            if (Items.Count > 0)
                LineBatch.DrawLine(Items[0].Position, Items[0].Position + Vector2.UnitX * Items[0].Width);

            foreach (var item in Items)
                item.DrawBorder();


            LineBatch.End();

        }
    }
}
