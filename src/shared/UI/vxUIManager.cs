using System;
using System.Collections.Generic;
using System.Linq;
using VerticesEngine.Graphics;

namespace VerticesEngine.UI
{
    /// <summary>
    /// GUI Manager for a given Game Scene. This Handles all GUI Items within a given scene.
    /// </summary>
    public class vxUIManager : IDisposable
    {
        /// <summary>
        /// The collection of GUI items in this GUI Manager.
        /// </summary>
        public List<vxUIControl> Items
        {
            get { return _items; }
        }
        private List<vxUIControl> _items = new List<vxUIControl>();

		/// <summary>
		/// Does the GUI have focus.
		/// </summary>
        public bool HasFocus = false;


		/// <summary>
		/// This item is the current item with focus.
		/// </summary>
		public vxUIControl FocusedItem;

		/// <summary>
		/// Gets or sets the alpha of the GUI Manager.
		/// </summary>
		/// <value>The alpha.</value>
		public float Alpha
		{
			get { return _alpha; }
			set { _alpha = value; }
		}

		private float _alpha = 1;


        /// <summary>
        /// Initializes a new instance of the <see cref="VerticesEngine.UI.vxUIManager"/> class.
        /// </summary>
        public vxUIManager() { }
        
		/// <summary>
		/// Adds a vxGUI Item to thie GUI Manager.
		/// </summary>
		/// <param name="item"></param>
		public void Add(vxUIControl item)
		{
            item.Index = Items.Count();
			Items.Add(item);
			item.OnGUIManagerAdded(this);
        }



        /// <summary>
        /// Remove the specified item.
        /// </summary>
        /// <returns>The remove.</returns>
        /// <param name="item">Item.</param>
        public void Remove(vxUIControl item)
		{
			Items.Remove(item);
		}


		/// <summary>
		/// Adds a Range of vxGUI Items to thie GUI Manager.
		/// </summary>
		/// <param name="item">Xbase GUI item.</param>
        public void AddRange(IEnumerable<vxUIControl> item)
        {
            Items.AddRange(item);
        }

        /// <summary>
        /// Tells the GUI Manager too update each of the Gui Items
        /// </summary>
        public void Update()
        {
			// The GUI Manager Draws and Updates it's items from the back forwards. It
			// only allows one item to have focus, which is the most forward item with the mouse
			// over it.
            HasFocus = false;

            if (this.FocusedItem == null)
            {
                for(int i = 0; i < Items.Count; i++)
                {
                    vxUIControl guiItem = Items[i];
                    guiItem.GUIIndex = i;
					//guiItem.Alpha = this.Alpha;
                    guiItem.Update();

                    if (guiItem.HasFocus == true)
                        HasFocus = true;
                }
            }
            else
            {
                // If there's a focused item, then the UIManager has Focus
                this.FocusedItem.Update();
                HasFocus = true;
            }
        }

        internal void HandleInput()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].HandleInput();
            }
        }

        /// <summary>
        /// Tells the GUI Manager too Draw the Gui Items
        /// </summary>
        public void Draw()
        {
			// The GUI Manager tries to draw all of the GUI items in one SpriteBatch call.
			// Any items which require special SpriteBatch calls will need to End the call,
			// do what they need, then do a special call after wards.
			vxGraphics.SpriteBatch.Begin("GUI - Internal Manager Draw");
			DrawByOwner();
			vxGraphics.SpriteBatch.End();
        }

		/// <summary>
		/// Draws the GUI manager using a pre-started SpriteBatch the by owner.
		/// </summary>
		public void DrawByOwner()
		{
			foreach (vxUIControl guiItem in Items)
			{
				guiItem.Alpha = Alpha;
				guiItem.Draw();
			}

			foreach (vxUIControl guiItem in Items)
			{
				guiItem.DrawText();
			}

			if (this.FocusedItem != null)
			{
				this.FocusedItem.Draw();
				this.FocusedItem.DrawText();
                this.FocusedItem.DrawToolTip();
			}
		}

        public void Dispose()
        {
            foreach (var item in _items)
                item.Dispose();

            _items.Clear();
        }
    }
}
