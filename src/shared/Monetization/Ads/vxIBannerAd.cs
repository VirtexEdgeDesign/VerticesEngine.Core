using System;
using Microsoft.Xna.Framework;

namespace VerticesEngine.Monetization.Ads
{
    public interface vxIBannerAd
    {
        /// <summary>
        /// The Ad Unity ID used by this banner ad
        /// </summary>
        string AdUnitID { get; }

        /// <summary>
        /// Is this banner ad currently visible
        /// </summary>
        bool IsVisible { get; set; }

        /// <summary>
        /// Has this ad been initialised with an Ad Unity ID. Note this does not garauentee that ads are showing.
        /// </summary>
        bool IsInitialised { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:VerticesEngine.Monetization.Ads.vxAdManager"/> ads are being received.
        /// </summary>
        /// <value><c>true</c> if ads are being received; otherwise, <c>false</c>.</value>
        bool AdsAreBeingReceived { get; }

        /// <summary>
        /// Initialises a new banner ad with the specified Ad Unit Id.
        /// </summary>
        /// <param name="adUnitID"></param>
        /// <param name="location"></param>
        void Initailise(string adUnitID, Vector2 location);

        void Show();

        void Hide();

        void Update();
    }
}
