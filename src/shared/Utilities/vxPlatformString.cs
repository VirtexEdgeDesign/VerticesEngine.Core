using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.Utilities
{

    /// <summary>
    /// Different set of strings based off of 
    /// </summary>
    public class vxPlatformString
    {
        private string steamID = "";
        private string itchIoID = "";
        private string googlePlayID = "";
        private string appleID = "";

        /// <summary>
        /// Returns the Value for this specific platform
        /// </summary>
        public string Value
        {
            get { return GetValueForPlatform(vxEngine.ReleasePlatformType); }
        }

        /// <summary>
        /// Returns the Value for the specefied platform
        /// </summary>
        public string GetValueForPlatform(vxPlatformType platformType)
        {
            string id = "";
            switch (platformType)
            {
                case vxPlatformType.Steam:
                    id = steamID;
                    break;
                case vxPlatformType.ItchIO:
                    id = itchIoID;
                    break;
                case vxPlatformType.GooglePlayStore:
                    id = googlePlayID;
                    break;
                case vxPlatformType.AppleAppStore:
                    id = appleID;
                    break;
            }

            return id;
        }

        /// <summary>
        /// Is there a valid entry for this platform. This will return true if the value is not empty.
        /// </summary>
        public bool IsValid
        {
            get { return !(Value == string.Empty); }
        }


        /// <summary>
        /// Holds a set of Platform strings for different platforms
        /// </summary>
        /// <param name="googlePlayID"></param>
        /// <param name="appleID"></param>
        /// <param name="steamID"></param>
        /// <param name="itchIoID"></param>
        public vxPlatformString(string googlePlayID = "", string appleID = "", string steamID = "", string itchIoID = "")
        {
            this.steamID = steamID;
            this.itchIoID = itchIoID;
            this.googlePlayID = googlePlayID;
            this.appleID = appleID;
        }

        /// <summary>
        /// Holds a set of Platform strings for different platforms differentiated by platform type
        /// </summary>
        /// <param name="desktop"></param>
        /// <param name="mobile"></param>
        public vxPlatformString(string desktop = "", string mobile = "")
        {
            this.steamID = desktop;
            this.itchIoID = desktop;
            this.googlePlayID = mobile;
            this.appleID = mobile;
        }
    }
}
