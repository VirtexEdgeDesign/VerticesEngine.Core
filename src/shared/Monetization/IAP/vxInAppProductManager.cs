using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
#if __ANDROID__ || __IOS__
using Plugin.InAppBilling;
using VerticesEngine.UI.Controls;
#endif

namespace VerticesEngine.Monetization.Purchases
{
    /// <summary>
    /// The In App Product Manager which holds all information for available in app products available for this game on this platform.
    /// </summary>
    public class vxInAppProductManager
    {
        /// <summary>
        /// Get's the instance of the in app product manager
        /// </summary>
        public static vxInAppProductManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new vxInAppProductManager();
                }

                return _instance;
            }
        }
        private static vxInAppProductManager _instance;

        public Dictionary<string, vxInAppProduct> Prodcuts
        {
            get { return _prodcuts; }
        }

        private Dictionary<string, vxInAppProduct> _prodcuts = new Dictionary<string, vxInAppProduct>();

        private vxInAppProductManager()
        {

        }

        /// <summary>
        /// Add's a new product
        /// </summary>
        /// <param name="key">The Key (usually an enum) to retreieve this product</param>
        /// <param name="appProduct"></param>
        public void AddProduct(object key, vxInAppProduct appProduct)
        {
            _prodcuts.Add(key.ToString(), appProduct);
        }

        /// <summary>
        /// Returns a product reference
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public vxInAppProduct GetProduct(object key)
        {
            return _prodcuts[key.ToString()];
        }

        public bool TryGetProduct(object key, out vxInAppProduct product)
        {
            if (_prodcuts.TryGetValue(key.ToString(),  out var prod))
            {
                product = prod;
                return true;
            }
            else
            {
                product = null;
                return false;
            }
        }

        /// <summary>
        /// Has this product been purchased?
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsProductPurchased(object key)
        {
            if(_prodcuts.TryGetValue(key.ToString(), out var product))
            {
                return product.IsPurchased;
            }
            return false;
        }

        #region -- Events --

#pragma warning disable CS0067

        
        /// <summary>
        /// Called when a successful purchase is made
        /// </summary>
        public event EventHandler<vxInAppPurchaseEventArgs> OnPurchased;
        internal void OnPurchasedCallback(vxInAppPurchaseEventArgs arg)
        {
            OnPurchased?.Invoke(this, arg);
        }

        public event EventHandler<vxInAppPurchaseEventArgs> OnPurchaseError;
        internal void OnPurchaseErrorCallback(vxInAppPurchaseEventArgs arg)
        {
            OnPurchaseError?.Invoke(this, arg);
        }


        /// <summary>
        /// Called when a sucessfull purchase is consumed. This is called after 'OnPurchased' and
        /// only for Consumable product
        /// </summary>
        public event EventHandler<vxInAppPurchaseEventArgs> OnPurchaseConsumed;
        internal void OnPurchaseConsumedCallback(vxInAppPurchaseEventArgs arg)
        {
            OnPurchaseConsumed?.Invoke(this, arg);
        }


        /// <summary>
        /// Called when a purchase is restored
        /// </summary>
        public event EventHandler<vxInAppPurchaseEventArgs> OnPurchaseRestored;
        internal void OnPurchaseRestoredCallback(vxInAppPurchaseEventArgs arg)
        {
            OnPurchaseRestored?.Invoke(this, arg);
        }
#pragma warning restore CS0067

        #endregion

        /// <summary>
        /// Purchases an item for the given sku
        /// </summary>
        /// <param name="sku"></param>
        public void PurchaseItem(object key)
        {
            string sku = key.ToString();
            //vxNotificationManager.Show("Purchasing: " + sku, Microsoft.Xna.Framework.Color.Magenta);
            // get the associated vxInAppProduct
            if (_prodcuts.ContainsKey(sku) == false)
            {
                vxConsole.WriteError("SKU Not Found - sku: " + sku);
            }
#if __MOBILE__
            Task.Run(() => MakeMobilePurchaseAsync(sku));
#endif
        }

        /// <summary>
        /// Consumes all purchases
        /// </summary>
        public void ConsumeAllPurchases()
        {
            //vxNotificationManager.Show("Consuming All Purchases ", Microsoft.Xna.Framework.Color.Magenta);
#if __MOBILE__
            Task.Run(() => ConsumeAllPurchasesAsync());
#endif
        }

        /// <summary>
        /// Retreives product info for the list of available items
        /// </summary>
        public void RestorePurchases()
        {
#if __MOBILE__
            Task.Run(() => GetPurchasesAsync());
#endif
        }

        #region -- Mobile --

#if __MOBILE__

        public void RestorePurchase(string productId)
        {
            Task.Run(() => WasItemPurchased(productId));
        }

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
                    if(_prodcuts.ContainsKey(productId))
                        OnPurchased(this, new vxInAppPurchaseEventArgs(_prodcuts[productId]));
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
                if(_prodcuts.ContainsKey(sku) == false)
                {
                    string msg = "SKU Not Found - sku: " + sku;
                    vxConsole.WriteError(msg);
                    OnPurchaseError(this, new vxInAppPurchaseEventArgs(null, msg));
                    return false;
                }

                vxInAppProduct product = _prodcuts[sku];

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
                    OnPurchaseError(this, new vxInAppPurchaseEventArgs(null, "Could Not Make Purchase..."));
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
                    OnPurchased(this, new vxInAppPurchaseEventArgs(product));

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
                            OnPurchaseConsumed(this, new vxInAppPurchaseEventArgs(product, "Purchased"));
                        }
                    }

                }

                return true;
            }
            catch (Exception ex)
            {
                //Something bad has occurred, alert user
                vxConsole.WriteException(this, ex);
                OnPurchaseError(this, new vxInAppPurchaseEventArgs(null, "Error Purchasing " + sku));
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
                    foreach(var v in _prodcuts.Values)
                    {
                        if(v.Id == purch.ProductId)
                        {
                            vxConsole.WriteLine("======================= purchase restored for " + purch.ProductId + " =======================");
                            v._isPurchased = true;
                            OnPurchased(this, new vxInAppPurchaseEventArgs(v, "Restored"));
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
                    if (_prodcuts.ContainsKey(purch.Id))
                    {
                        _prodcuts[purch.Id]._isPurchased = true;
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

#endif

        #endregion
    }
}
