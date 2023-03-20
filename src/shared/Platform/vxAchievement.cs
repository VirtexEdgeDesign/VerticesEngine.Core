using System;
using Microsoft.Xna.Framework.Graphics;

#if __ANDROID__
using Android.OS;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Games;
using Android.Gms.Games.Achievement;
using Android.Gms.Games.LeaderBoard;
using Android.App;
using Android.Content;
using Android.Views;
using Java.Interop;
#endif
using System.Collections.Generic;
using System.Threading.Tasks;

using VerticesEngine;
using Microsoft.Xna.Framework;
using VerticesEngine.UI.Controls;


namespace VerticesEngine.Profile
{
    /// <summary>
    /// This class holds all information for a user profile. The sign in method depends on
    /// release (i.e. Android would be google signin, PC/Desktop would be Steam etc...).
    /// </summary>
    public class vxAchievement
    {
        public bool Achieved = false;

        /// <summary>
        /// Gets the identifier ID for the achievements for the given platform.
        /// </summary>
        /// <value>The identifier.</value>
        public string ID
        {
            get
            {
                return _id;
            }
        }

        private readonly string _id = "xxxxx-xxxxxx";

        /// <summary>
        /// Initializes a new instance of the <see cref="vxAchievement"/> class.
        /// </summary>
        public vxAchievement(string id)
        {
            this._id = id;
        }

        //public void UnlockAchievement(string achievementCode)
        //{
        //    GamesClass.Achievements.Unlock(vxAndroidPlayerProfile.client, achievementCode);
        //}

        //public void IncrementAchievement(string achievementCode, int progress)
        //{
        //    GamesClass.Achievements.Increment(vxAndroidPlayerProfile.client, achievementCode, progress);
        //}
    }
}
