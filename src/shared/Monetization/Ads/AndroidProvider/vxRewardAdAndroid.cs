#if __ANDROID__
using Android.Gms.Ads;
using Android.Gms.Ads.Rewarded;
using Microsoft.Xna.Framework;
using System;
using VerticesEngine.UI.Controls;

namespace VerticesEngine.Monetization.Ads
{

    /// <summary>
    /// Creates a new Interstitial Ad for Android
    /// </summary>
    public class vxRewardAdAndroid : RewardedAdLoadCallback, vxIRewardAd, IOnUserEarnedRewardListener
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
            get { return mRewardedAd != null; }
        }


        public vxRewardAdAndroid()
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

        private Android.Gms.Ads.Rewarded.RewardedAd mRewardedAd;

        /// <summary>
        /// Requests a New Ad to be loaded
        /// </summary>
        public void LoadNewAd()
        {
            vxConsole.WriteMonLine(">>>>>>>>>>>> REWARDAD.LoadNewAd:" + IsLoaded);
            CreateRewardAd(m_adUnitID);
        }

        private void CreateRewardAd(string adUnit)
        {
            var context = Android.App.Application.Context;
            var requestBuilder = new AdRequest.Builder(); ;
            RewardedAd.Load(context, adUnit, requestBuilder.Build(), this);
        }

        public void ShowAd()
        {
            vxConsole.WriteMonLine(">>>>>>>>>>>> REWARDAD.Show:" + IsLoaded);
            try
            {
                if (IsLoaded)
                {
                    mRewardedAd.Show(Game.Activity, this);
                    mRewardedAd = null;
                }
                else
                {
                    vxNotificationManager.Add(new vxNotification("Rewards Not Loaded", Color.Red));
                }
                LoadNewAd();
            }
            catch
            {
                // do nothing for now
                vxNotificationManager.Add(new vxNotification("Error Showing Rewards", Color.Red));
            }
        }


        public override void OnAdFailedToLoad(LoadAdError p0)
        {
            base.OnAdFailedToLoad(p0);
            vxConsole.WriteError("Error Loading Ad " + p0.Cause);
            vxNotificationManager.Add(new vxNotification("Error Loading Rewards:"+ p0.Cause, Color.Red));
            mRewardedAd = null;
        }


        public override void OnAdLoaded(RewardedAd rewardedAd)
        {
            base.OnAdLoaded(rewardedAd);

            mRewardedAd = rewardedAd;
            vxConsole.WriteMonLine(">>>>>>>>>>>> REWARD-AD.LOCKED-AND-LOADED:" + IsLoaded);
            vxNotificationManager.Add(new vxNotification("Rewards Available!", Color.Blue));
        }

        public void OnUserEarnedReward(Android.Gms.Ads.Rewarded.IRewardItem p0)
        {
            vxConsole.WriteMonLine(">>>>>>>>>>>> REWARD-AD.RECEIVED:" + p0.Type + "=" + p0.Amount);
            vxNotificationManager.Add(new vxNotification("Reward Recieved:"+p0.Type +"="+p0.Amount, Color.LimeGreen));
            vxAdManager.OnRewardReceived(p0.Type, p0.Amount);
        }
    }
}

#endif