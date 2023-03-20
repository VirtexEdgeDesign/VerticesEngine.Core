using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.UI.Controls
{
    public class vxPropertyControlSelectEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the GUI manager.
        /// </summary>
        /// <value>The GUI manager.</value>
        public vxPropertiesControl PropertiesControl
        {
            get { return propertiesControl; }
        }
        vxPropertiesControl propertiesControl;

        /// <summary>
        /// Constructor.
        /// </summary>
        public vxPropertyControlSelectEventArgs(vxPropertiesControl propertiesControl)
        {
            this.propertiesControl = propertiesControl;
        }
    }
}
