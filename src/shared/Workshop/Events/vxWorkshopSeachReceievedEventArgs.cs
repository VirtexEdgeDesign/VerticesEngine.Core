using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VerticesEngine.UI.Dialogs;

namespace VerticesEngine.Workshop.Events
{
    public class vxWorkshopSeachReceievedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        public List<vxIWorkshopItem> Items
        {
            get { return _items; }
        }
        List<vxIWorkshopItem> _items = new List<vxIWorkshopItem>();

        /// <summary>
        /// Constructor.
        /// </summary>
        public vxWorkshopSeachReceievedEventArgs(List<vxIWorkshopItem> items)
        {
            foreach (var item in items)
                Items.Add(item);
        }
    }
}