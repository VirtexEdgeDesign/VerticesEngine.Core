#if __MOBILE__
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Plugin.InAppBilling;
using VerticesEngine.UI.Controls;


namespace VerticesEngine.Monetization.Purchases
{
    public class vxInAppPurchaseProviderForMobile : vxIInAppPurchaseProvider
    {
        public void ConsumeAllPurchases()
        {
            Task.Run(() => ConsumeAllPurchasesAsync());
        }

        public void PurchaseItem(object key)
        {
            string sku = key.ToString();
            //vxNotificationManager.Show("Purchasing: " + sku, Microsoft.Xna.Framework.Color.Magenta);
            // get the associated vxInAppProduct
            // if (_prodcuts.ContainsKey(sku) == false)
            // {
            //     vxConsole.WriteError("SKU Not Found - sku: " + sku);
            // }
            Task.Run(() => MakeMobilePurchaseAsync(sku));
        }

        public void RestorePurchases()
        {
            Task.Run(() => GetPurchasesAsync());
        }

        public void RestorePurchase(string productId)
        {
            Task.Run(() => WasItemPurchased(productId));
        }

        public event Action<vxInAppPurchaseEventArgs> OnPurchased;
         async Task<bool> WasItemPurchased(string productId)
        {
            var billing = CrossInAppBilling.Current;
            try
            {
                var connected = await billing.ConnectAsync();

                if (!connected)
                {
                    vxNotificationManager.Show("Couldn't Connect", Color.Magenta);
                    //Couldn't connect
                    return false;
                }

                //check purchases
                var purchases = await billing.GetPurchasesAsync(ItemType.InAppPurchase);

                //check for null just incase
                if (purchases?.Any(p => p.ProductId == productId) ?? false)
                {
                    vxNotificationManager.Show("Purchase Restored", Color.Cyan);
                    //Purchase restored
                    if (vxInAppProductManager.Instance.TryGetProduct(productId, out var product))
                    {
                        vxInAppProductManager.Instance.OnPurchasedCallback(new vxInAppPurchaseEventArgs(product));
                    }
                    return true;
                }
                else
                {
                    vxNotificationManager.Show("Purcahsed not restored: ", Color.Magenta);
                    //no purchases found
                    return false;
                }
            }
            catch (InAppBillingPurchaseException purchaseEx)
            {
                //Billing Exception handle this based on the type
                vxNotificationManager.Show("Error: " + purchaseEx, Color.Red);
            }
            catch (Exception ex)
            {
                //Something has gone wrong
                vxNotificationManager.Show("Exception: " + ex.Message, Color.Red);
            }
            finally
            {
                await billing.DisconnectAsync();
            }

            return false;
        }

        private async Task<bool> MakeMobilePurchaseAsync(string sku)
        {
            vxConsole.WriteLine("Purchasing...");
            try
            {
                // get the associated vxInAppProduct
                if (vxInAppProductManager.Instance.TryGetProduct(sku, out var product))
                {

//                    vxInAppProduct product = _prodcuts[sku];

                    var connected = await CrossInAppBilling.Current.ConnectAsync();

                    if (!connected)
                    {
                        //Couldn't connect to billing, could be offline, alert user
                        vxConsole.WriteError("Not Connected");
                        return false;
                    }

                    //try to purchase item
                    var purchase = await CrossInAppBilling.Current.PurchaseAsync(product.Id, ItemType.InAppPurchase);
                    if (purchase == null)
                    {
                        //Not purchased, alert the user
                        vxNotificationManager.Show("Could Not Make Purchase...", Color.Red);
                        vxInAppProductManager.Instance.OnPurchaseErrorCallback(new vxInAppPurchaseEventArgs(null, "Could Not Make Purchase..."));
                        return false;
                    }
                    else
                    {
                        vxConsole.WriteLine("Purchased!");
                        //Purchased, save this information
                        var id = purchase.Id;
                        var token = purchase.PurchaseToken;
                        var state = purchase.State;
                        vxConsole.WriteLine(id);
                        vxConsole.WriteLine(token);
                        vxConsole.WriteLine(state.ToString());

                        // set the is purchased flag to true
                        product._isPurchased = true;

                        // call the purchase event
                        vxInAppProductManager.Instance.OnPurchasedCallback(new vxInAppPurchaseEventArgs(product));

                        // check if this product is a consumable
                        if (product.ProductType == vxInAppProductType.Consumable)
                        {
                            var consumedItem = await CrossInAppBilling.Current.ConsumePurchaseAsync(id, token);
                            if (consumedItem != null)
                            {
                                // since we've consumed this, then we can now 
                                product._isPurchased = false;
                                // Item has been consumed
                                vxConsole.WriteLine("Item has been consumed");
                                vxInAppProductManager.Instance.OnPurchaseConsumedCallback(
                                    new vxInAppPurchaseEventArgs(product, "Purchased"));
                            }
                        }

                    }

                    return true;
                }
                else
                {
                    string msg = "SKU Not Found - sku: " + sku;
                    vxConsole.WriteError(msg);
                    vxInAppProductManager.Instance.OnPurchaseErrorCallback( new vxInAppPurchaseEventArgs(null, msg));
                    return false;
                }
            }
            catch (Exception ex)
            {
                //Something bad has occurred, alert user
                vxConsole.WriteException(this, ex);
                vxInAppProductManager.Instance.OnPurchaseErrorCallback( new vxInAppPurchaseEventArgs(null, "Error Purchasing " + sku));
                return false;
            }
            finally
            {
                //Disconnect, it is okay if we never connected
                await CrossInAppBilling.Current.DisconnectAsync();
            }
        }

        private async Task<bool> GetMobileProductInfoAsync()
        {
            vxConsole.WriteLine("Checking Purchases...");
            try
            {
                var connected = await CrossInAppBilling.Current.ConnectAsync();

                if (!connected)
                {
                    //Couldn't connect to billing, could be offline, alert user
                    vxConsole.WriteError("Not Connected");
                    return false;
                }


                vxConsole.WriteLine("Getting Info");
                
                var items = await CrossInAppBilling.Current.GetProductInfoAsync(ItemType.InAppPurchase);

                foreach (var item in items)
                {
                    vxConsole.WriteLine(item.Description);
                    vxConsole.WriteLine(item.Name);
                }

                return true;
            }
            catch (Exception ex)
            {
                //Something bad has occurred, alert user
                vxConsole.WriteException(this, ex);
                return false;
            }
            finally
            {
                //Disconnect, it is okay if we never connected
                await CrossInAppBilling.Current.DisconnectAsync();
            }
        }

        public async Task<bool> GetPurchasesAsync()
        {
            vxConsole.WriteLine("Checking Purchases...");
            try
            {
                var connected = await CrossInAppBilling.Current.ConnectAsync();

                if (!connected)
                {
                    //Couldn't connect to billing, could be offline, alert user
                    vxConsole.WriteError("Not Connected");
                    return false;
                }


                vxConsole.WriteLine("Getting Purchases");

                //check purchases
                var purchases = await CrossInAppBilling.Current.GetPurchasesAsync(ItemType.InAppPurchase);

                var f = CrossInAppBilling.Current.GetPurchasesAsync(ItemType.InAppPurchase);
                

                foreach (var purch in purchases)
                {
                    // loop through all products, if we match the prod id, then set it to purchased
                    foreach(var v in vxInAppProductManager.Instance.Prodcuts.Values)
                    {
                        if(v.Id == purch.ProductId)
                        {
                            vxConsole.WriteLine("======================= purchase restored for " + purch.ProductId + " =======================");
                            v._isPurchased = true;
                            vxInAppProductManager.Instance.OnPurchasedCallback( new vxInAppPurchaseEventArgs(v, "Restored"));
                        }
                    }

                    vxConsole.WriteLine(purch.Id);
                    vxConsole.WriteLine(purch.ProductId);
                    vxConsole.WriteLine(purch.AutoRenewing);
                    vxConsole.WriteLine(purch.State);
                    vxConsole.WriteLine(purch.ConsumptionState);
                    vxConsole.WriteLine(purch.PurchaseToken);
                    vxConsole.WriteLine(purch.TransactionDateUtc);
                    vxConsole.WriteLine("===========================================================================================");


                    //var consumedItem = await billing.ConsumePurchaseAsync(purch.ProductId, purch.PurchaseToken);
                    //if (consumedItem != null)
                    //{
                    //    // Item has been consumed
                    //    vxConsole.WriteLine("Item has been consumed");
                    //}
                }


                return true;
            }
            catch (Exception ex)
            {
                //Something bad has occurred, alert user
                vxConsole.WriteException(this, ex);
                return false;
            }
            finally
            {
                vxConsole.WriteLine("Finished");
                //Disconnect, it is okay if we never connected
                await CrossInAppBilling.Current.DisconnectAsync();
            }
        }

        public async Task<bool> ConsumeAllPurchasesAsync()
        {
            vxConsole.WriteLine("Checking Purchases...");
            try
            {
                var connected = await CrossInAppBilling.Current.ConnectAsync();

                if (!connected)
                {
                    //Couldn't connect to billing, could be offline, alert user
                    vxConsole.WriteError("Not Connected");
                    return false;
                }


                vxConsole.WriteLine("Getting Purchases");

                //check purchases
                var purchases = await CrossInAppBilling.Current.GetPurchasesAsync(ItemType.InAppPurchase);


                foreach (var purch in purchases)
                {
                    if (vxInAppProductManager.Instance.TryGetProduct(purch.Id, out var product))
                    {
                        product._isPurchased = true;
                    }

                    vxConsole.WriteLine(purch.Id);
                    vxConsole.WriteLine(purch.ProductId);
                    vxConsole.WriteLine(purch.AutoRenewing);
                    vxConsole.WriteLine(purch.State);
                    vxConsole.WriteLine(purch.ConsumptionState);
                    vxConsole.WriteLine(purch.PurchaseToken);
                    vxConsole.WriteLine(purch.TransactionDateUtc);
                    vxConsole.WriteLine("=======================");


                    var consumedItem = await CrossInAppBilling.Current.ConsumePurchaseAsync(purch.ProductId, purch.PurchaseToken);
                    if (consumedItem != null)
                    {
                        // Item has been consumed
                        vxConsole.WriteLine("Item has been Consumed");
                        vxNotificationManager.Show(purch.ProductId + " Has Been Consumed", Microsoft.Xna.Framework.Color.Magenta);
                    }
                }


                return true;
            }
            catch (Exception ex)
            {
                //Something bad has occurred, alert user
                vxConsole.WriteException(this, ex);
                return false;
            }
            finally
            {
                //Disconnect, it is okay if we never connected
                await CrossInAppBilling.Current.DisconnectAsync();
            }
        }
    }
}
#endif