using System;
namespace VerticesEngine.Monetization.Ads
{
    public interface vxIInterstitialAd
    {
        /// <summary>
        /// The Ad Unity ID used by this ad
        /// </summary>
        string AdUnitID { get; }

        /// <summary>
        /// Has this ad been initialised with an Ad Unity ID. Note this does not garauentee that ads are showing.
        /// </summary>
        bool IsInitialised { get; }

        /// <summary>
        /// Is an Interstitial Ad loaded and ready to be shown?
        /// </summary>
        bool IsLoaded { get; }

        /// <summary>
        /// Initialises a new Intersitial ad
        /// </summary>
        /// <param name="adUnitID"></param>
        void Initailise(string adUnitID);

        /// <summary>
        /// Requests to load a new interstitial ad to be shown at a later time
        /// </summary>
        void LoadNewAd();


        /// <summary>
        /// Shows the Interstitial Ad
        /// </summary>
        void ShowAd();
    }
}
