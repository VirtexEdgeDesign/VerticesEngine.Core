using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine
{ 
    [System.AttributeUsage(System.AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class vxSettingsAttribute : Attribute
    {
        public string DisplayName { get; private set; }

        public string Description { get; private set; }

        /// <summary>
        /// Should this setting be saved to an ini file.
        /// </summary>
        public bool IsSavedToINIFile { get; private set; }

        /// <summary>
        /// Is this setting a menu setting, or should it only show in an ini file.
        /// </summary>
        public bool IsMenuSetting { get; private set; }

        /// <summary>
        /// This tags a class as a sandbox item and allows you to explicitly set the asset path
        /// </summary>
        /// <param name="name"></param>
        /// <param name="categoryKey"></param>
        /// <param name="subCategory"></param>
        /// <param name="assetPath"></param>
        public vxSettingsAttribute(string displayname, string description = "", bool isSavedToINI = true, bool isMenuSetting = false)
        {
            DisplayName = displayname;
            Description = description;
            this.IsSavedToINIFile = isSavedToINI;
            this.IsMenuSetting = isMenuSetting;

        }
    }
}
