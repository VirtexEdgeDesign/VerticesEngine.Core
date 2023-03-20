using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine
{
    /// <summary>
    /// Event Args called when an item is selected for an inspector property
    /// </summary>
    public class vxSandboxItemSelectedForInspectorEventArgs : EventArgs
    {
        public vxGameObject SelectedGameObject { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public vxSandboxItemSelectedForInspectorEventArgs(vxGameObject SelectedGameObject)
        {
            this.SelectedGameObject = SelectedGameObject;
        }
    }
}
