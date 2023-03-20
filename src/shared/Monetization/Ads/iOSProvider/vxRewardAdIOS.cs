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
    public class vxRewardAdIOS : RewardBasedVideoAdDelegate, vxIRewardAd, IRewardedAdDelegate
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
#if OLDADMOB
            get { return RewardBasedVideoAd.SharedInstance.IsReady; }
#else
            get { return _rewardedAd.IsReady; }
#endif
        }

        public vxRewardAdIOS()
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
           //old method
            //if (RewardBasedVideoAd.SharedInstance.IsReady)
            //{
            //    return;
            //}

            //RewardBasedVideoAd.SharedInstance.CustomRewardString = options?.CustomData;

            Request request = Request.GetDefaultRequest();
            //RewardBasedVideoAd.SharedInstance.LoadRequest(request, m_adUnitID);

            //new method
            if (_rewardedAd==null)
                _rewardedAd = new RewardedAd(m_adUnitID);
            _rewardedAd.LoadRequest(request, (error) => {
                vxNotificationManager.Show("Rewards Available", Color.SkyBlue);

    });
        }
        RewardedAd _rewardedAd;
        public void ShowAd()
        {
            //old method
            //if (RewardBasedVideoAd.SharedInstance.IsReady)
            //{
            //    var window = UIApplication.SharedApplication.KeyWindow;
            //    var vc = window.RootViewController;
            //    while (vc.PresentedViewController != null)
            //    {
            //        vc = vc.PresentedViewController;
            //    }

            //    RewardBasedVideoAd.SharedInstance.Present(vc);
            //}

            //new method
            if (_rewardedAd.IsReady)
            {
                var window = UIApplication.SharedApplication.KeyWindow;
                var vc = window.RootViewController;
                while (vc.PresentedViewController != null)
                {
                    vc = vc.PresentedViewController;
                }
                _rewardedAd.Present(vc, this);
            }
            LoadNewAd();
        }

        //new method
        public void UserDidEarnReward(RewardedAd rewardedAd, AdReward reward)
        {
            vxConsole.WriteMonLine(">>>>>>>>>>>> NEW REWARD-AD.RECEIVED:" + reward.Type + "=" + reward.Amount);
            vxNotificationManager.Add(new vxNotification("NEW Reward Recieved:" + reward.Type + "=" + reward.Amount, Color.LimeGreen));
            vxAdManager.OnRewardReceived(reward.Type, reward.Amount.Int32Value);
        }

        //old method
        public override void DidRewardUser(RewardBasedVideoAd rewardBasedVideoAd, AdReward reward)
        {
            vxConsole.WriteMonLine(">>>>>>>>>>>> REWARD-AD.RECEIVED:" + reward.Type + "=" + reward.Amount);
            vxNotificationManager.Add(new vxNotification("Reward Recieved:" + reward.Type + "=" + reward.Amount, Color.LimeGreen));
            vxAdManager.OnRewardReceived(reward.Type, reward.Amount.Int32Value);
        }


        public override void DidClose(RewardBasedVideoAd rewardBasedVideoAd)
        {
            vxConsole.WriteLine("REWARD:DidClose");
        }

        public override void DidCompletePlaying(RewardBasedVideoAd rewardBasedVideoAd)
        {
            vxConsole.WriteLine("REWARD:DidCompletePlaying");
        }

        public override void DidFailToLoad(RewardBasedVideoAd rewardBasedVideoAd, Foundation.NSError error)
        {

            vxConsole.WriteLine("REWARD:DidFailToLoad");
        }

        public override void DidOpen(RewardBasedVideoAd rewardBasedVideoAd)
        {
            vxConsole.WriteLine("REWARD:DidOpen");

        }

        public override void DidReceiveAd(RewardBasedVideoAd rewardBasedVideoAd)
        {

            vxConsole.WriteLine("REWARD:DidReceiveAd");
        }

        public override void DidStartPlaying(RewardBasedVideoAd rewardBasedVideoAd)
        {

            vxConsole.WriteLine("REWARD:DidStartPlaying");
        }

        public override void WillLeaveApplication(RewardBasedVideoAd rewardBasedVideoAd)
        {

            vxConsole.WriteLine("REWARD:WillLeaveApplication");
        }
    }
}

#endif