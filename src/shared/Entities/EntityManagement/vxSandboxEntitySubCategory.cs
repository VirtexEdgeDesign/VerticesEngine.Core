using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine
{
    /// <summary>
    /// This a sub category which holds a list of entities within it.
    /// </summary>
    public class vxSandboxEntitySubCategory
    {
        /// <summary>
        /// The Sub Category name
        /// </summary>
        public string Name;

        public List<vxSandboxEntityRegistrationInfo> types = new List<vxSandboxEntityRegistrationInfo>();

        public vxSandboxEntitySubCategory(object name)
        {
            this.Name = name.ToString().SplitIntoSentance();
        }
    }
}
