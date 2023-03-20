
#region Using Statements
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;

#if __ANDROID__
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Content.PM;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Gms.Ads;
#endif

#endregion

namespace VerticesEngine
{
    /// <summary>
    /// Flags which are provided at launch which tell Vertices what settings and 
    /// systems this game supports
    /// </summary>
    [Flags]
    public enum vxGameConfigFlags
    {
        ControlsSettings = 1,
        GraphicsSettings = 2,
        AudioSettings = 4,
        LanguageSettings = 8,
        NetworkEnabled = 16,
        IsCursorVisible = 32,

        /// <summary>
        /// This app has player profile support. For different platforms will have
        /// the game log in using a different system, such as Steam, Google Play, Apple etc...
        /// </summary>
        PlayerProfileSupport = 128,

        /// <summary>
        /// This app has Leaderboards
        /// </summary>
        LeaderboardsSupport = 256,

        /// <summary>
        /// This app has achievements,
        /// </summary>
        AchievementsSupport = 512,

        /// <summary>
        /// This app contains in app purchases, and therefore will have 
        /// certain settings enabled such as 'Restore All Purchases' button
        /// </summary>
        InAppPurchases= 1024,
    }

    public class vxGameConfig
    { 
    
    }
}

