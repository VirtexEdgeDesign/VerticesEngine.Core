using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine
{
    [System.AttributeUsage(System.AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class vxInputSettingsAttribute : vxSettingsAttribute
    {

        /// <summary>
        /// This tags a class as a sandbox item and allows you to explicitly set the asset path
        /// </summary>
        /// <param name="name"></param>
        /// <param name="categoryKey"></param>
        /// <param name="subCategory"></param>
        /// <param name="assetPath"></param>
        public vxInputSettingsAttribute(string displayname, string description = "") : base(displayname, description)
        {

        }
    }
}
