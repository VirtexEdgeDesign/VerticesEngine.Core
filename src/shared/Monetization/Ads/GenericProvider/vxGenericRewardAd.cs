using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.Monetization.Ads
{
    public class vxGenericRewardAd : vxIRewardAd
    {
        /// <summary>
        /// The Ad Unity ID for this Banner
        /// </summary>
        public string AdUnitID
        {
            get { return m_adUnitID; }
        }
        private string m_adUnitID = string.Empty;

        /// <summary>
        /// Has this ad been initialised yet
        /// </summary>
        public bool IsInitialised
        {
            get { return m_isInitialised; }
            set { m_isInitialised = value; }
        }
        private bool m_isInitialised = false;


        /// <summary>
        /// Gets a value indicating whether this <see cref="T:VerticesEngine.Monetization.Ads.vxAdManager"/> ads are being received.
        /// </summary>
        /// <value><c>true</c> if ads are being received; otherwise, <c>false</c>.</value>
        public bool IsLoaded
        {
            get { return m_isLoaded; }
            set { m_isLoaded = value; }
        }
        private bool m_isLoaded = false;

        public void Initailise(string adUnitID)
        {
            
        }

        public void LoadNewAd()
        {
            
        }

        public void ShowAd()
        {
            
        }
    }
}
