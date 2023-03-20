using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VerticesEngine.UI.Dialogs;

namespace VerticesEngine.Workshop.Events
{
    public class vxWorkshopItemOpenEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <value>The item.</value>
        public vxIWorkshopItem Item
        {
            get { return _item; }
        }
        vxIWorkshopItem _item;

        /// <summary>
        /// Constructor.
        /// </summary>
        public vxWorkshopItemOpenEventArgs(vxIWorkshopItem item)
        {
            _item = item;
        }
    }




    public class vxWorkshopItemPublishedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the item identifier.
        /// </summary>
        /// <value>The item identifier.</value>
        public string ItemID
        {
            get { return _itemID; }
        }

        string _itemID;


        /// <summary>
        /// Gets a value indicating whether the upload was successful.
        /// </summary>
        /// <value><c>true</c> if is upload successful; otherwise, <c>false</c>.</value>
        public bool IsUploadSuccessful 
        {
            get { return _isSuccessful; }
        }

        bool _isSuccessful = false;


        /// <summary>
        /// Gets the info.
        /// </summary>
        /// <value>The info.</value>
        public string Info
        {
            get { return _info; }
        }

        string _info = "";


        /// <summary>
        /// Constructor.
        /// </summary>
        public vxWorkshopItemPublishedEventArgs(string itemID, bool status, string info)
        {
            _itemID = itemID;
            _isSuccessful = status;
                _info = info;
        }
    }
}