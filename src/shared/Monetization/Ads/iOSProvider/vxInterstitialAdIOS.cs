#if __IOS__
using System;
using CoreGraphics;
using UIKit;
using Google.MobileAds;
using Microsoft.Xna.Framework;
using VerticesEngine.UI.Controls;

namespace VerticesEngine.Monetization.Ads
{
    /// <summary>
    /// Creates a new Interstitial Ad for iOS
    /// </summary>
    public class vxInterstitialAdIOS : vxIInterstitialAd
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
        /// Gets a value indicating whether this <see cref="T:VerticesEngine.Monetization.Ads.vxAdManager"/> ads are being received.
        /// </summary>
        /// <value><c>true</c> if ads are being received; otherwise, <c>false</c>.</value>
        public bool IsLoaded
        {
            get { return m_isLoaded; }
            set { m_isLoaded = value; }
        }
        private bool m_isLoaded = false;


        /// <summary>
        /// The interstitial ad view.
        /// </summary>
        Interstitial AdViewInterstitial;


        public vxInterstitialAdIOS()
        {

        }

        /// <summary>
        /// Initialises the Intersitial ad with the required ad unity id
        /// </summary>
        /// <param name="adUnitID"></param>
        public void Initailise(string adUnitID)
        {
            m_adUnitID = adUnitID;

            m_isInitialised = true;
        }

        /// <summary>
        /// Requests a New Ad to be loaded
        /// </summary>
        public void LoadNewAd()
        {
            Request request = Request.GetDefaultRequest();
            if (AdViewInterstitial == null || AdViewInterstitial.HasBeenUsed == true)
            {
                Console.WriteLine("Creating AdViewInterstitial");
                AdViewInterstitial = new Interstitial(AdUnitID);
                AdViewInterstitial.AdReceived += (object sender, EventArgs e) =>
                {
                    Console.WriteLine("Add Recived");
                };
                AdViewInterstitial.ScreenDismissed += delegate
                {
                    Console.WriteLine("Dissmised");
                    //vxEngine.Instance.Pause = false;
                };

                AdViewInterstitial.ReceiveAdFailed += (object sender, InterstitialDidFailToReceiveAdWithErrorEventArgs e) =>
                {
                    Console.WriteLine(e.Error.DebugDescription);
                    //throw new Exception(e.Error.DebugDescription);
                    //throw new Exception(e.Error.Description); Might be more helpful??
                };
                AdViewInterstitial.LoadRequest(request);
            }
        }

        public void ShowAd()
        {
            Console.WriteLine("AdViewInterstitial.IsReady:" + AdViewInterstitial.IsReady);
            try
            {
                if (AdViewInterstitial != null)
                {
                    if (AdViewInterstitial.IsReady)
                    {
                        UIViewController viewController = vxEngine.Game.Services.GetService(typeof(UIViewController)) as UIViewController;
                        //viewController.PresentViewController(AdViewInterstitial, true, null);
                        //AdViewInterstitial.PresentFromRootViewController(viewController);
                        AdViewInterstitial.Present(viewController);
                        //vxEngine.Instance.Pause = true;
                    }
                }
            }
            catch
            {
                // do nothing for now
                vxNotificationManager.Add(new vxNotification("Error Showing Ad", Color.Red));
            }
        }
    }
}

#endif