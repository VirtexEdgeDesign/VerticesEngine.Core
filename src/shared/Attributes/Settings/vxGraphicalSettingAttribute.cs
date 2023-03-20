using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine
{
    [System.AttributeUsage(System.AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class vxGraphicalSettingsAttribute : vxSettingsAttribute
    {
        public vxGameEnviromentType Usage { get; private set; }
        /// <summary>
        /// A graphical setting. If isMenuSetting is false, this value will be set and serialised to a corresponding *.ini file.
        /// </summary>
        /// <param name="displayname">The display name for this setting.</param>
        /// <param name="description">The description for this setting.</param>
        /// <param name="isSavedToINI">Is this setting saved to it's ini file?</param>
        /// <param name="isMenuSetting">Is this setting a menu setting?Note: only Properties are loaded as menu items</param>
        /// <param name="usage">Is this setting 2D or 3D specific, or is it used for both?</param>
        public vxGraphicalSettingsAttribute(string displayname, string description = "", bool isSavedToINI = true, bool isMenuSetting = false, 
            vxGameEnviromentType usage = vxGameEnviromentType.TwoDimensional | vxGameEnviromentType.ThreeDimensional | vxGameEnviromentType.VR) : 
            base(displayname, description, isSavedToINI, isMenuSetting)
        {
            Usage = usage;
        }
    }

}
