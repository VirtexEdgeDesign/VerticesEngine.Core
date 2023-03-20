#if __ANDROID__
using Android.Gms.Ads;
using Android.Gms.Ads.Interstitial;
using Microsoft.Xna.Framework;
using System;
using VerticesEngine.UI.Controls;

namespace VerticesEngine.Monetization.Ads
{

    /// <summary>
    /// Creates a new Interstitial Ad for Android
    /// </summary>
    public class vxInterstitialAdAndroid : InterstitialAdLoadCallback, vxIInterstitialAd
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

        public vxInterstitialAdAndroid()
        {

        }


        /// <summary>
        /// Initialises the Intersitial ad with the required ad unity id
        /// </summary>
        /// <param name="adUnitID"></param>
        public void Initailise(string adUnitID)
        {
            m_adUnitID = adUnitID;

            LoadNewAd();

            m_isInitialised = true;

        }

        /// <summary>
        /// Requests a New Ad to be loaded
        /// </summary>
        public void LoadNewAd()
        {
            vxConsole.WriteMonLine(">>>>>>>>>>>> AdViewInterstitial.LoadNewAd:" + this.IsLoaded);
            if (this.IsLoaded == false)
            {
                var adreq = new AdRequest.Builder().Build();

                InterstitialAd.Load(Game.Activity.ApplicationContext, m_adUnitID, adreq, this);
            }
        }

        public void ShowAd()
        {
            vxConsole.WriteMonLine(">>>>>>>>>>>> AdViewInterstitial.IsReady:" + this.IsLoaded);
            try
            {
                if (mInterstitialAd != null)
                {
                    mInterstitialAd.Show(Game.Activity);
                    mInterstitialAd = null;
                }
                LoadNewAd();
            }
            catch
            {
                // do nothing for now
                vxNotificationManager.Add(new vxNotification("Error Showing Ad", Color.Red));
            }
        }

        private Android.Gms.Ads.Interstitial.InterstitialAd mInterstitialAd;
        public override void OnInterstitialAdLoaded(Android.Gms.Ads.Interstitial.InterstitialAd interstitialAd)
        {
            base.OnInterstitialAdLoaded(interstitialAd);
            mInterstitialAd = interstitialAd;
            m_isLoaded = true;
        }


        public override void OnAdFailedToLoad(LoadAdError p0)
        {
            base.OnAdFailedToLoad(p0);
            mInterstitialAd = null;
        }
    }
}

#endif