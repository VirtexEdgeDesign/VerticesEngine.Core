using System;

namespace VerticesEngine.Monetization.Purchases
{
    public class vxInAppPurchaseEventArgs : EventArgs
    {
        public vxInAppProduct Product
        {
            get { return _product; }
        }
        private vxInAppProduct _product;

        public string Message
        {
            get { return _message; }
        }
        private string _message;

        public vxInAppPurchaseEventArgs(vxInAppProduct product, string message = "")
        {
            _product = product;
            _message = message;
        }
    }
}