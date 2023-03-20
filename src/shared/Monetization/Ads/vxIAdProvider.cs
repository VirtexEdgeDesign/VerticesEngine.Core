using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.Monetization.Ads
{
    /// <summary>
    /// The ad provider interface which gives a method for injecting different ad providers
    /// </summary>
    public interface vxIAdProvider
    {
        vxIBannerAd GetBannerAd();
        vxIInterstitialAd GetInterstitialAd();
        vxIRewardAd GetRewardAd();
    }
}
