
#if __ANDROID__
using Android.App;
using Android.Content;
using Plugin.InAppBilling;
using System;
using VerticesEngine.Diagnostics;
using VerticesEngine.Profile;

namespace VerticesEngine
{
    public sealed partial class vxEngine
    {
        public Android.App.Activity Activity;

        public void OnStart()
        {
            try
            {
                //game.Engine.PlayerProfile
                if (Game != null)
                    ((vxGooglePlayServicePlatformWrapper)vxPlatform.Player).Start();
            }
            catch (Exception ex)
            {
                vxCrashHandler.Thwrow(ex);
            }
        }

        public void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            try
            {
                // call any in app billing calls
                //InAppBillingImplementation.HandleActivityResult(requestCode, resultCode, data);
                

                // fire any activity results
                if (Game != null)
                    ((vxGooglePlayServicePlatformWrapper)vxPlatform.Player).OnActivityResult(requestCode, resultCode, data);


            }
            catch (Exception ex)
            {
                vxCrashHandler.Thwrow(ex);
            }
        }

        public void OnStop()
        {
            try
            {
                if (Game != null)
                    ((vxGooglePlayServicePlatformWrapper)vxPlatform.Player).Stop();
            }
            catch (Exception ex)
            {
                vxCrashHandler.Thwrow(ex);
            }
        }
    }
}
#endif