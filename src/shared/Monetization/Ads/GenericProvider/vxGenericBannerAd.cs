using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.Monetization.Ads
{
    public class vxGenericBannerAd : vxIBannerAd
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
        /// Is this Banner Ad currently visible?
        /// </summary>
        public bool IsVisible
        {
            get { return m_isVisible; }
            set
            {

                m_isVisible = value;
            }
        }
        private bool m_isVisible = false;


        /// <summary>
        /// Gets a value indicating whether this <see cref="T:VerticesEngine.Monetization.Ads.vxAdManager"/> ads are being received.
        /// </summary>
        /// <value><c>true</c> if ads are being received; otherwise, <c>false</c>.</value>
        public bool AdsAreBeingReceived
        {
            get { return m_adsAreBeingReceived; }
            set { m_adsAreBeingReceived = value; }
        }
        private bool m_adsAreBeingReceived = false;

        public void Show()
        {

        }

        public void Hide()
        {
            
        }

        public void Initailise(string adUnitID, Vector2 location)
        {
            
        }

        public void Update()
        {
            
        }
    }
}
