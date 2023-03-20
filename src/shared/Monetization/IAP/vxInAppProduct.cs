using System;
using System.Collections.Generic;
using System.Text;
using VerticesEngine.Utilities;

namespace VerticesEngine.Monetization.Purchases
{
    /// <summary>
    /// This is an in app product which is available for purchase from the 
    /// relavant plaform store.
    /// </summary>
    public class vxInAppProduct
    {
        /// <summary>
        /// The product ID which must match with the registered SKU online
        /// </summary>
        public string Id
        {
            get { return _id.Value; }
        }
        vxPlatformString _id;

        /// <summary>
        /// Informative name for the product
        /// </summary>
        public string Name
        {
            get { return _name; }
        }
        string _name;

        /// <summary>
        /// What type of product is this (Consumable, Subscription etc...)
        /// </summary>
        public vxInAppProductType ProductType
        {
            get { return _productType; }
        }
        vxInAppProductType _productType = vxInAppProductType.NonConsumable;


        /// <summary>
        /// Has this item been purchased
        /// </summary>
        public bool IsPurchased
        {
            get { return _isPurchased; }
        }
        internal bool _isPurchased = false;


        /// <summary>
        /// Creates a new in app product item register
        /// </summary>
        /// <param name="name"></param>
        /// <param name="productType"></param>
        /// <param name="id"></param>
        public vxInAppProduct(string name, vxInAppProductType productType, vxPlatformString id)
        {
            _id = id;
            _name = name;
            _productType = productType;
        }
    }
}
