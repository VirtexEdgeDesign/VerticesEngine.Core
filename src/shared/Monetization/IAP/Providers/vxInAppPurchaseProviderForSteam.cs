using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace VerticesEngine.Monetization.Purchases
{
    public class vxInAppPurchaseProviderForSteam : vxIInAppPurchaseProvider
    {
        public void ConsumeAllPurchases()
        {
            // todo setup with Steam IAP
        }

        public void PurchaseItem(object key)
        {
            string sku = key.ToString();
            //vxNotificationManager.Show("Purchasing: " + sku, Microsoft.Xna.Framework.Color.Magenta);
            // get the associated vxInAppProduct
            if (vxInAppProductManager.Instance.TryGetProduct(key, out var product))
            {
            }
            else
            {
                vxConsole.WriteError("SKU Not Found - sku: " + sku);
            }
        }

        public void RestorePurchases()
        {

        }
    }
}
