using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine;
using VerticesEngine.UI.Events;
using VerticesEngine.UI.Themes;
using VerticesEngine.Utilities;

namespace VerticesEngine.UI.Controls
{
	public class vxTabPageTab : vxButtonControl
	{
		vxTabPageControl TabPage;

		/// <summary>
		/// Is this tab selected?
		/// </summary>
		public bool IsTabSelected
        {
			get { return TabPage.IsTabSelected; }
		}

		/// <summary>
		/// Gets or sets the art provider.
		/// </summary>
		/// <value>The art provider.</value>
		public vxTabPageTabArtProvider ArtProvider;

		public static int TabWidth = 96;

		public static int TabHeight = 24;

		public vxTabPageTab(string PageTitle, vxTabPageControl TabPage) : base(PageTitle, Vector2.Zero)
		{
			this.TabPage = TabPage;

			Width = TabWidth;
			Height = TabHeight;

			DoSelectionBorder = false;

			//Have this button get a clone of the current Art Provider
			ArtProvider = (vxTabPageTabArtProvider)vxUITheme.ArtProviderForTabs.Clone();
			ArtProvider.SetDefaults();
		}

		public override void Draw()
		{
			ArtProvider.Theme.Background.SelectedColour = (TabPage.Index == TabPage.TabControl.SelectedIndex) ? Color.White : Color.Gray;

			//Now get the Art Provider to draw the scene
			this.ArtProvider.Draw(this);

			DrawToolTip();
		}

	}



	/// <summary>
	/// Tab page control.
	/// </summary>
	public class vxTabPageControl : vxPanel
	{
		/// <summary>
		/// The owning tab control.
		/// </summary>
		public vxTabControl TabControl;

		/// <summary>
		/// The tab for this page.
		/// </summary>
		public vxTabPageTab Tab;

		/// <summary>
		/// Is this Tab Page active.
		/// </summary>
		public bool IsActive = false;

		public bool IsTabSelected = false;

        /// <summary>
        /// The index of the page within the Tab Control.
        /// </summary>
        public new int Index = 0;


		/// <summary>
		/// Initializes a new instance of the <see cref="T:Virtex.Lib.Iris.Gui.Controls.vxTabPageControl"/> class.
		/// </summary>
		/// <param name="TabControl">Tab control.</param>
		/// <param name="PageTitle">Page title.</param>
		public vxTabPageControl(vxTabControl TabControl, string PageTitle):base(TabControl.Bounds)
		{
			this.TabControl = TabControl;
			Tab = new vxTabPageTab(PageTitle, this);
			Tab.Clicked += OnTabClicked;


			Tab.Theme = vxUITheme.UnselectedItemTheme;
		}

        public vxTabPageControl(string PageTitle) : 
            base(new Rectangle(0,0,250,100))
        {
            //this.TabControl = TabControl;
            Tab = new vxTabPageTab(PageTitle, this);
            Tab.Clicked += OnTabClicked;

            Tab.Theme = vxUITheme.UnselectedItemTheme;
        }

        /// <summary>
        /// Selects the tab.
        /// </summary>
        public virtual void SelectTab()
		{
            Tab.Theme = vxUITheme.SelectedItemTheme;
            TabControl.SelectedIndex = this.Index;
			TabControl.OnSelectedTabChange();
			IsTabSelected = true;

		}


		/// <summary>
		/// Uns the select tab.
		/// </summary>
		public virtual void UnSelectTab()
		{
            Tab.Theme = vxUITheme.UnselectedItemTheme;
			IsTabSelected = false;
		}


        protected override void OnDisposed()
        {
            base.OnDisposed();

            Tab.Clicked -= OnTabClicked;
            Tab.Dispose();
        }


        /// <summary>
        /// Fired when the Tab is clicked.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void OnTabClicked(object sender, vxUIControlClickEventArgs e)
		{
			// Unselect Previous Tab
			TabControl.Pages[TabControl.SelectedIndex].UnSelectTab();

			// Set the New Selected Index
			TabControl.SelectedIndex = Index;

			// Now Select The New Tab
			TabControl.Pages[MathHelper.Clamp(TabControl.SelectedIndex, 0, TabControl.Pages.Count-1)].SelectTab();
		}


		public override void Draw()
		{
			// First Reset the Panel Position
			Position = TabControl.Position + new Vector2(0, Tab.Height);

			//Colour = Color.Transparent;

			base.Draw();
		}

        public virtual void DrawTab()
        {
            Tab.Draw();
        }
	}
}
