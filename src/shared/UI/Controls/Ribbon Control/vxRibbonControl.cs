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
	public class vxRibbonControl : vxTabControl
	{
		/// <summary>
		/// The default width of the ribbon.
		/// </summary>
		public static int DefaultRibbonWidth = 650;

		/// <summary>
		/// The default height of the ribbon.
		/// </summary>
		public static int DefaultRibbonHeight = 92;

        public Color BackgroundColour = new Color(50, 50, 50, 255);

        public Color ForegroundColour = Color.WhiteSmoke * 0.95f;


        public vxRibbonToolbarLogoButtonControl TitleButton;

        /// <summary>
        /// The title toolbar.
        /// </summary>
        public vxRibbonToolbar TitleToolbar;

        public vxRibbonControl(vxUIManager GuiManager) :
        this(GuiManager ,Vector2.Zero){}

        public vxRibbonControl(vxUIManager GuiManager, Vector2 Position):
        base(new Rectangle((int)(Position.X),(int)(Position.Y+24), 
                                              vxGraphics.GraphicsDevice.Viewport.Width, DefaultRibbonHeight))
        {
			Theme = new vxUIControlTheme(
			    new vxColourTheme(BackgroundColour, BackgroundColour, BackgroundColour),
			    new vxColourTheme(ForegroundColour, ForegroundColour, ForegroundColour));

            TitleToolbar = new vxRibbonToolbar(Position);
            GuiManager.Add(TitleToolbar);


            GuiManager.Add(this);
            
            TitleButton = new vxRibbonToolbarLogoButtonControl(this, vxInternalAssets.UI.VirtexLogo)
            {
                Width = 42,
                Height = 42,
                Padding = new Vector2(0),
            };

            DefaultRibbonWidth = vxGraphics.GraphicsDevice.Viewport.Width;

		}

        public void AddContextTab(vxTabPageControl page)
        {
            //if(Pages.Contains(page) == false)
                Add(page);
            TitleToolbar.IsTitleShown = false;
        }

        public void RemoveContextTab(vxTabPageControl page)
        {
            Remove(page);
            TitleToolbar.IsTitleShown = true;
        }

        /// <summary>
        /// Ons the selected tab change.
        /// </summary>
        public override void OnSelectedTabChange()
		{
			base.OnSelectedTabChange();

			if (SelectedIndex < Pages.Count)
			{
				vxRibbonTabPage tabPag = (vxRibbonTabPage)Pages[SelectedIndex];

				Width = Math.Max(tabPag.GroupWidth, DefaultRibbonWidth);
			}
		}

        public vxRibbonTabPage GetTabPage(string name)
        {
            foreach(var page in Pages)
            {
                vxRibbonTabPage tabPag = (vxRibbonTabPage)page;
                if (tabPag.Tab.Text == name)
                    return tabPag;
            }
            return null;
        }

        public override void Draw()
        {
            // First draw the Background
            SpriteBatch.Draw(DefaultTexture, Bounds, BackgroundColour * Opacity);

            int tabCount = 0;
            int RunningWidth = 0;

            // Next draw the Tabs for each of the pages
            foreach (vxTabPageControl page in Pages)
            {

                // First Set the Position of the tab
                page.Tab.Position = Position + new Vector2(RunningWidth + TabStartOffset + TabPadding, 0);
                RunningWidth += page.Tab.Width + TabPadding;


                // Always draw the Tab last
                page.DrawTab();

                // Only draw the panel of the tab if it's the Selected Index.
                if (page.Index == SelectedIndex)
                    page.Draw();

                tabCount++;
            }

            TitleButton.Draw();
        }
	}
}
