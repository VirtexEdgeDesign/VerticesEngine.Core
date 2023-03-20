using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine;
using VerticesEngine.Graphics;
using VerticesEngine.UI.Themes;

namespace VerticesEngine.UI.Controls
{
	/// <summary>
	/// Ribbon control.
	/// </summary>
	public class vxRibbonTabPage : vxTabPageControl
	{
		public int GroupWidth = 0;

        public vxRibbonControl RibbonControl;
		
        public vxRibbonTabPage(vxRibbonControl RibbonControl, string PageTitle) : 
        base(RibbonControl, PageTitle)
		{
            this.Text = PageTitle;
            this.RibbonControl = RibbonControl;
            UIManager = TabControl.UIManager;
            Font = vxInternalAssets.Fonts.ViewerFont;

            Theme = new vxUIControlTheme(
                new vxColourTheme(
                    Color.Transparent, RibbonControl.BackgroundColour * 1.5f, RibbonControl.ForegroundColour),
                                              new vxColourTheme(
                                                  Color.White * 0.75f, Color.White, Color.Black));


            Tab.Font = vxInternalAssets.Fonts.ViewerFont;
            Tab.DoBorder = false;
		}


		/// <summary>
		/// Adds the group.
		/// </summary>
		/// <param name="NewGroup">New group.</param>
		public void AddGroup(vxRibbonControlGroup NewGroup)
		{
			Items.Add(NewGroup);

			int runningWidth = 0;
			// Set Each Groups Position
			foreach (vxRibbonControlGroup grp in Items)
			{
				grp.Position = new Vector2(Position.X + runningWidth, 0);
				grp.OriginalPosition = new Vector2(Position.X + runningWidth, 0);

				runningWidth += grp.Width;
			}
		}

        public override void SelectTab()
        {
            base.SelectTab();
            //Update();
            foreach (vxRibbonControlGroup grp in Items)
            {
                grp.HasFocus = false;
                foreach (var item in grp.Items)
                {
                    grp.HasFocus = false;
                    HasMouseBeenUpYet = false;
                }
            }

            int runningWidth = 0;
            // Set Each Groups Position
            foreach (vxRibbonControlGroup grp in Items)
            {
                grp.Position = new Vector2(Position.X + runningWidth, 0);
                grp.OriginalPosition = new Vector2(Position.X + runningWidth, 0);

                runningWidth += grp.Width;
            }
            Width = TabControl.Width;
        }



        public override void UpdateItems()
        {
            if (HasFocus)
                base.UpdateItems();
        }

		public override void Draw()
		{
            base.Draw();
			// Draw Bottom Border
			BorderBounds = new Rectangle(Bounds.X, Bounds.Bottom, Bounds.Width, 1);

            // Draw the Splitter
            SpriteBatch.Draw(DefaultTexture, Bounds, RibbonControl.ForegroundColour);
			SpriteBatch.Draw(DefaultTexture, BorderBounds, Color.Gray);
            DrawItems();
		}

        public override void DrawTab()
        {
            Font = vxInternalAssets.Fonts.ViewerFont;
            //Update Rectangle
            if (Tab.UseDefaultWidth)
            {
                //Set Width and Height
                Tab.Width = (int)(Math.Max(125, (int)(this.Font.MeasureString(Tab.Text).X + Padding.X * 2)));
                Tab.Height = (int)(Math.Max(24, (int)(this.Font.MeasureString(Tab.Text).Y + Padding.Y * 2)));
            }

            Tab.ToggleState = (Index == TabControl.SelectedIndex);

            vxGraphics.SpriteBatch.Draw(DefaultTexture, Tab.Bounds, Tab.GetStateColour(Theme.Background) * Opacity);

            Point TxtPos = new Point((int)(Tab.Position.X + Tab.Width / 2 - Font.MeasureString(Tab.Text).X / 2 - Padding.X),
                                     (int)(Tab.Position.Y + Tab.Height / 2 - Font.MeasureString(Tab.Text).Y / 2));
            
            vxGraphics.SpriteBatch.DrawString(this.Font, Tab.Text, TxtPos.ToVector2(), Tab.GetStateColour(Theme.Text) * Opacity);
                                         //0,
                                         //Vector2.Zero,
                                         //vxLayout.ScaleAvg,
                                         //SpriteEffects.None,
                                         //1);
        }
	}
}
