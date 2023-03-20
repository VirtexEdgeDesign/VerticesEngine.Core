#if !__MOBILE__
using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.Monetization.Ads
{
    /// <summary>
    /// Android ad provider which provides custom classes for Banner, Interstitial and Reward Ads
    /// </summary>
    public class vxGenericAdProvider : vxIAdProvider
    {
        public vxIBannerAd GetBannerAd()
        {
            return new vxGenericBannerAd();
        }

        public vxIInterstitialAd GetInterstitialAd()
        {
            return new vxGenericInterstitialAd();
        }

        public vxIRewardAd GetRewardAd()
        {
            return new vxGenericRewardAd();
        }
    }
}
#endif