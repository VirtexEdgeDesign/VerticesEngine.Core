using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine;
using VerticesEngine.Utilities;

namespace VerticesEngine.UI.Controls
{
	/// <summary>
	/// Tab control.
	/// </summary>
	public class vxTabControl : vxPanel
	{
		/// <summary>
		/// The Collection of Pages for this Tab Control
		/// </summary>
		public List<vxTabPageControl> Pages = new List<vxTabPageControl>();

		/// <summary>
		/// Gets the count.
		/// </summary>
		/// <value>The count.</value>
		public int Count
		{
			get
			{
				return Pages.Count;
			}
		}

		/// <summary>
		/// Gets or sets the index of the selected.
		/// </summary>
		/// <value>The index of the selected.</value>
		public int SelectedIndex
		{
			get { return MathHelper.Clamp(_selectedIndex, 0, Count-1); }
			set { _selectedIndex = (int)MathHelper.Clamp(value, 0, Count); }
		}
		int _selectedIndex = 0;


		/// <summary>
		/// The tab start offset.
		/// </summary>
		public int TabStartOffset = 0;

		public int TabPadding = 4;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Virtex.Lib.Iris.Gui.Controls.vxButton"/> class.
		/// </summary>
		/// <param name="GuiManager">GUI manager.</param>
		/// <param name="Bounds">Bounds.</param>
		public vxTabControl(Rectangle Bounds) :base(Bounds)
		{
			
		}



		/// <summary>
		/// Add the specified page.
		/// </summary>
		/// <param name="page">Page.</param>
		public void Add(vxTabPageControl page)
		{
			// Reset the Position
			page.Position = Position;

            page.TabControl = this;

            page.Bounds = Bounds;

			// Now set the Index of the Page
			page.Index = Pages.Count;

			if(Pages.Contains(page)==false)
				Pages.Add(page);
		}

        public void Remove(vxTabPageControl page)
        {
            Pages.Remove(page);
        }


        protected override void OnDisposed()
        {
            base.OnDisposed();

            foreach (var page in Pages)
                page.Dispose();

            Pages.Clear();
        }

        public virtual void OnSelectedTabChange()
		{
			

		}

		/// <summary>
		/// Update this instance.
		/// </summary>
		protected internal override void Update()
		{
			base.Update();

            for (int i = 0; i < Pages.Count; i++)
			{
                vxTabPageControl page = Pages[i];
				// Always Update the Tab
				page.Tab.Update();

				// Only Update the Page if it's Selected
				if(page.Index == SelectedIndex)
					page.Update();
			}
		}

		/// <summary>
		/// Draw this instance.
		/// </summary>
		public override void Draw()
		{
			// First draw the Background
			//SpriteBatch.Draw(DefaultTexture, Bounds, Color.Gray * Opacity);

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
		}
	}
}
