using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine
{
    /// <summary>
    /// This is a main category of Sandbox Entity types. This holds a name and a list of 'sub categories'.
    /// </summary>
    public class vxSandboxEntityCategory
    {
        public string name;
        public Dictionary<object, vxSandboxEntitySubCategory> SubCategories = new Dictionary<object, vxSandboxEntitySubCategory>();

        public vxSandboxEntityCategory(object name)
        {
            this.name = name.ToString().SplitIntoSentance();
        }
    }
}
