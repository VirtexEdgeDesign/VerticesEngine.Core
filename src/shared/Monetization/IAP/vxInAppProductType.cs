using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.Monetization.Purchases
{
    /// <summary>
    /// Types of items which can be bought
    /// </summary>
    public enum vxInAppProductType
    {
        /// <summary>
        /// An item which can be purchased multiple times once consumed
        /// </summary>
        Consumable,

        /// <summary>
        /// An item which can be purchased only once
        /// </summary>
        NonConsumable,

        /// <summary>
        /// An item which has a reoccuring purchase window
        /// </summary>
        Subscription
    }
}
