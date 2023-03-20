#if __IOS__
using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.Monetization.Ads
{
    /// <summary>
    /// Android ad provider which provides custom classes for Banner, Interstitial and Reward Ads
    /// </summary>
    public class vxAdProviderIOS : vxIAdProvider
    {
        public vxIBannerAd GetBannerAd()
        {
            return new vxBannerAdIOS();
        }

        public vxIInterstitialAd GetInterstitialAd()
        {
            return new vxInterstitialAdIOS();
        }

        public vxIRewardAd GetRewardAd()
        {
            return new vxRewardAdIOS();
        }
    }
}
#endif