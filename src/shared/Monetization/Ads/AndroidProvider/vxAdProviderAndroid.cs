#if __ANDROID__
using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.Monetization.Ads
{
    /// <summary>
    /// Android ad provider which provides custom classes for Banner, Interstitial and Reward Ads
    /// </summary>
    public class vxAdProviderAndroid : vxIAdProvider
    {
        public vxIBannerAd GetBannerAd()
        {
            return new vxBannerAdAndroid();
        }

        public vxIInterstitialAd GetInterstitialAd()
        {
            return new vxInterstitialAdAndroid();
        }

        public vxIRewardAd GetRewardAd()
        {
            return new vxRewardAdAndroid();
        }
    }
}
#endif