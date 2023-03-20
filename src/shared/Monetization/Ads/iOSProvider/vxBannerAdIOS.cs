#if __IOS__
using System;
using CoreGraphics;
using UIKit;
using Google.MobileAds;
using Microsoft.Xna.Framework;


namespace VerticesEngine.Monetization.Ads
{
    /// <summary>
    /// Creates a new Banner Ad for iOS
    /// </summary>
    public class vxBannerAdIOS : vxIBannerAd
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
        /// The location.
        /// </summary>
        CGPoint AdLocation;

        /// <summary>
        /// The ad view banner.
        /// </summary>
        BannerView AdViewBanner;

        

        public vxBannerAdIOS()
        {

        }

        public void Initailise(string adUnitID, Vector2 location)
        {
            
            AdLocation = new CGPoint(location.X, location.Y);
            // Setup your BannerView, review AdSizeCons class for more Ad sizes. 
            AdViewBanner = new BannerView(AdSizeCons.SmartBannerPortrait,
                new CGPoint(0, UIScreen.MainScreen.Bounds.Size.Height - AdSizeCons.Banner.Size.Height))
            {
                AdUnitId = adUnitID,
                RootViewController = vxAdManager.ViewController
            };
            //AdViewBanner.RemoveFromSuperview
            //AdViewBanner.hid
            
            // Wire AdReceived event to know when the Ad is ready to be displayed
            AdViewBanner.AdReceived += (object sender, EventArgs e) =>
            {
                if (!m_isVisible)
                {
                    vxAdManager.ViewController.View.AddSubview(AdViewBanner);
                    m_isVisible = true;
                    m_adsAreBeingReceived = true;
                }
            };

            AdViewBanner.ReceiveAdFailed += (object sender, BannerViewErrorEventArgs e) =>
            {
                Console.WriteLine(e.Error.DebugDescription);
                //throw new Exception(e.Error.DebugDescription);
                //throw new Exception(e.Error.Description); Might be more helpful??
            };

            Request request = Request.GetDefaultRequest();
            //request.TestDevices = new[] { "GAD_SIMULATOR_ID", "kGADSimulatorID" };

            AdViewBanner.LoadRequest(request);

            m_isInitialised = true;
        }


        bool shouldUpdateView = false;
        public void Hide()
        {
            shouldUpdateView = true;
            m_isVisible = false;
        }

        public void Show()
        { }



        public void Update()
        {
            if(shouldUpdateView)
            {
                shouldUpdateView = false;

                if (AdViewBanner != null)
                {
                    if (m_isVisible == false)
                    {
                        AdViewBanner.RemoveFromSuperview();
                    }
                }
            }
        }

    }
}

#endif