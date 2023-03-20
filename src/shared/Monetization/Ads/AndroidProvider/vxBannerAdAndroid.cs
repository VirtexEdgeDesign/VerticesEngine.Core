#if __ANDROID__
using Android.Gms.Ads;
using Android.Views;
using Microsoft.Xna.Framework;


namespace VerticesEngine.Monetization.Ads
{
    /// <summary>
    /// Creates a new Banner Ad for Android
    /// </summary>
    public class vxBannerAdAndroid : vxIBannerAd
    {
        /// <summary>
        /// The Ad Unity ID for this Banner
        /// </summary>
        public string AdUnitID
        {
            get { return m_adUnitID; }
        }
        private string m_adUnitID;

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
            set {

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


        /// <summary>
        /// The banner ad view
        /// </summary>
        private static AdView bannerAd;


        public vxBannerAdAndroid()
        {

        }

        public void Initailise(string adUnitID, Vector2 location)
        {
            m_adUnitID = adUnitID;
            // The actual ad
            bannerAd = new AdView(Game.Activity)
            {
                AdUnitId = adUnitID,
                AdSize = AdSize.Banner,
            };

            //bannerAd.AdSize = new AdSize(512, 64);
            if (vxAdManager.IsInTestMode)
            {
                bannerAd.LoadAd(new AdRequest.Builder()
                    //.AddTestDevice(vxAdManager.TestDeviceId) // Prevents generating real impressions while testing
                    .Build());
            }
            else
            {
            
                bannerAd.LoadAd(new AdRequest.Builder()
                    .Build());
            }
            if (location != Vector2.Zero)
            {
                vxAdManager.AdContainer.SetX(location.X);
                vxAdManager.AdContainer.SetY(location.Y);
            }

            // Give the game methods to show/hide the ad.
            vxAdManager.AdContainer.AddView(bannerAd);

            m_isInitialised = true;
        }



        public void Show()
        {
            if (m_isVisible)
                return;

            m_isVisible = true;
            bannerAd.LoadAd(new AdRequest.Builder()
                    .Build());
            bannerAd.Visibility = ViewStates.Visible;
        }

        public void Hide()
        {
            if (m_isVisible == false)
                return;

            bannerAd.Destroy();
            bannerAd.Visibility = ViewStates.Gone;
            
            m_isVisible = false;
        }


        public void Update()
        {
            
        }

    }
}

#endif