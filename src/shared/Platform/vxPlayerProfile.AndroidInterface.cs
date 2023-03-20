#if __ANDROID__
using Android.App;
using Android.Content;

namespace VerticesEngine.Profile
{
    /// <summary>
    /// Basic interface which all Google Play wrappers use for interfacing with the GooglePlayServices Game API's
    /// </summary>
    public interface vxGooglePlayServicePlatformWrapper
    {
        void Start();

        void Stop();

        void OnActivityResult(int requestCode, Result resultCode, Intent data);
    }
}
#endif