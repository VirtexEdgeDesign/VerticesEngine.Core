using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.Monetization.Purchases
{
    /// <summary>
    /// In App Purchase wrapper
    /// </summary>
    public interface vxIInAppPurchaseProvider
    {
        /// <summary>
        /// Consumes all purchases
        /// </summary>
        void ConsumeAllPurchases();

        /// <summary>
        /// Purchases an item for the given sku
        /// </summary>
        /// <param name="sku"></param>
        void PurchaseItem(object key);


        /// <summary>
        /// Retreives product info for the list of available items
        /// </summary>
        void RestorePurchases();
    }
}
