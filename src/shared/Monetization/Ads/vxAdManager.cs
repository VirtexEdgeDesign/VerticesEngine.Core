using System;
using Microsoft.Xna.Framework;
using VerticesEngine;

using VerticesEngine.UI.Controls;
using VerticesEngine.Graphics;

#if __IOS__
using CoreGraphics;
using UIKit;
using Google.MobileAds;
#endif


#if __ANDROID__
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Content.PM;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Gms.Ads;
//using Android.Gms.Ads.Hack;
using System.Collections.Generic;

#endif

namespace VerticesEngine.Monetization.Ads
{
    /// <summary>
    /// The engine ad manager for mobile releases.
    /// </summary>
    public static class vxAdManager
    {
        /// <summary>
        /// The ad unit identifier.
        /// </summary>
        public static string AppAdId
        {
            get { return _appAdId; }
        }
        static string _appAdId = "";

        /// <summary>
        /// Is the Ad Manager in Testing Mode. If set to true it will
        /// serve ads using the test device id's that were set during the initialization
        /// </summary>
        public static bool IsInTestMode
        {
            get { return _isInTestMode; }
        }
        static bool _isInTestMode = false;


        /// <summary>
        /// The test device id. This is set only in Android
        /// </summary>
        public static string TestDeviceId
        {
            get { return _testDeviceId; }
        }
        static string _testDeviceId = "";



        /// <summary>
        /// Has this ad manager been initialised yet
        /// </summary>
        public static bool IsInitialised
        {
            get { return m_isInitialised; }
            set { m_isInitialised = value; }
        }
        private static bool m_isInitialised = false;

        /// <summary>
        /// The Banner Ad Instance for this game. You can check if it's Initialised and / or if it's
        /// visible currently.
        /// </summary>
        /// <remarks>
        /// To Initialise the banner ad properly you will need to call the following in your code:
        /// </remarks>
        /// <code>
        /// vxAdManager.InitBanner(string adUnitID)
        /// </code>
        public static vxIBannerAd BannerAd;

        /// <summary>
        /// The Interstitiail Ad instance. This ad will show over top of the game when 'ShowAd' is called.
        /// Note you will need to call 'LoadNewAd' after each time you call 'ShowAd()'
        /// </summary>
        public static vxIInterstitialAd InterstitialAd;

        /// <summary>
        /// The reward Ad Instance.
        /// </summary>
        public static vxIRewardAd RewardAd;

#if __ANDROID__

        /// <summary>
        /// The Android Game view.
        /// </summary>
        static View GameView;


        /// <summary>
        /// The ad container.
        /// </summary>
        internal static LinearLayout AdContainer;

#elif __IOS__

        /// <summary>
        /// The view controller.
        /// </summary>
        public static UIViewController ViewController;

#endif

        private static vxIAdProvider adProvider;

        /// <summary>
        /// Initialises the Ad Manager. This is a Cross platform method which will run on both Android and iOS.
        /// </summary>
        internal static void Init()
        {
            // initialise the ad manager
            adProvider = vxEngine.Game.OnInitAdProvider();
#if __ANDROID__
            MobileAds.Initialize(Game.Activity);

            if (vxEngine.BuildType == vxBuildType.Debug)
            {
                List<string> testids = new List<string>();
                testids.Add("8F8087B4B1F8F44FF22781AC724308EC");
                testids.Add("EA8141A02C84A071555D93E43D4551BC");
                if (!string.IsNullOrEmpty(_testDeviceId))
                    testids.Add(_testDeviceId);
                var rew = new RequestConfiguration.Builder().SetTestDeviceIds(testids).Build();
                MobileAds.RequestConfiguration = rew;
            }

            GameView = (View)vxEngine.Game.Services.GetService(typeof(View));

            // Create the Ad Container
            AdContainer = new LinearLayout(Game.Activity)
            {
                Orientation = Orientation.Horizontal
            };
            AdContainer.SetGravity(GravityFlags.CenterHorizontal | GravityFlags.Bottom);
            AdContainer.SetBackgroundColor(Android.Graphics.Color.Transparent); // Need on some devices, not sure why

            //AdContainer.AddView(RewardedVideoAd);

            // A layout to hold the ad container and game view
            var mainLayout = new FrameLayout(Game.Activity);

            mainLayout.AddView(GameView);
            mainLayout.AddView(AdContainer);
            Game.Activity.SetContentView(mainLayout);

#elif __IOS__
            ViewController = vxEngine.Game.Services.GetService(typeof(UIViewController)) as UIViewController;
            //AdLocation = new CGPoint(location.X, location.Y);

            MobileAds.SharedInstance.Start(completionHandler: null);

            // if it's a debug run, then just do test runs
            if (_isInTestMode)
            {
                MobileAds.SharedInstance.RequestConfiguration.TestDeviceIdentifiers = new[] { "GAD_SIMULATOR_ID", "kGADSimulatorID" };
            }
#endif

            // create ad objects
            BannerAd = adProvider.GetBannerAd();
            InterstitialAd = adProvider.GetInterstitialAd();
            RewardAd = adProvider.GetRewardAd();

            m_isInitialised = true;
        }

        public static void Update()
        {
            if (m_isInitialised)
            {
                BannerAd.Update();
            }
        }


        /// <summary>
        /// Initialises the Ad Manager.
        /// </summary>
        /// <param name="adAppId"></param>
        /// <param name="isTestMode"></param>
        /// <param name="testDeviceId"></param>
        public static void InitSettings(string adAppId, bool isTestMode = false, string testDeviceId = "")
        {
            if (m_isInitialised)
            {
                vxConsole.WriteError("Cannot call 'InitSettings' after Ad Manager has been initialised");
                return;
            }

            _appAdId = adAppId;
            _isInTestMode = isTestMode;
            _testDeviceId = testDeviceId;
        }

        /// <summary>
        /// Adds the banner with ad at the default position.
        /// </summary>
        /// <param name="adUnitID">Ad unit identifier.</param>
        public static void InitBanner(string adUnitID)
        {
            Vector2 pos = new Vector2(
                vxGraphics.GraphicsDevice.Viewport.Width / 4 - 320 / 2,
                vxGraphics.GraphicsDevice.Viewport.Height / 2 - 50);

            InitBanner(adUnitID, pos);
        }
        /// <summary>
        /// Adds the banner.
        /// </summary>
        /// <param name="adUnitID">Ad unit identifier.</param>
        /// <param name="Location">Location.</param>
        public static void InitBanner(string adUnitID, Vector2 Location)
        {
#if __ANDROID__
            Location = Vector2.One;
#endif
            BannerAd.Initailise(adUnitID, Location);
        }


        /// <summary>
        /// Adds the initerstialel ad.
        /// </summary>
        /// <param name="adUnitID">Ad unit identifier.</param>
        public static void InitInterstitialAd(string adUnitID)
        {
            InterstitialAd.Initailise(adUnitID);
            LoadInterstitialAd();
        }

        /// <summary>
        /// Load an Interstitial ad
        /// </summary>
        public static void LoadInterstitialAd()
        {
            InterstitialAd.LoadNewAd();

        }


        /// <summary>
        /// Shows the initersial ad if there's one loaded.
        /// </summary>
        public static void ShowInterstitial()
        {
            InterstitialAd.ShowAd();
        }

        /// <summary>
        /// Initialise a Reward Ad
        /// </summary>
        /// <param name="adUnitID"></param>
        public static void InitRewardAd(string adUnitID)
        {
            RewardAd.Initailise(adUnitID);
            LoadRewardVideo();
        }

        /// <summary>
        /// Load a reward ad
        /// </summary>
        /// <param name="adUnitID"></param>
        public static void LoadRewardVideo()
        {
            RewardAd.LoadNewAd();
        }

        /// <summary>
        /// Show a reward ad
        /// </summary>
        public static void ShowRewardAd()
        {
            RewardAd.ShowAd();
        }

        /// <summary>
        /// Called when ever a reward has been recieved
        /// </summary>
        /// <param name="type"></param>
        /// <param name="amount"></param>
        public static void OnRewardReceived(string type, int amount)
        {
            RewardReceived?.Invoke(type, amount);
        }

        /// <summary>
        /// Called when ever a reward is recevied from a reward ad
        /// </summary>
        public static event Action<string, int> RewardReceived;// = (type, amount) => {};
    }
}