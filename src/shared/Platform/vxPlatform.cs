/**
 * @file
 * @author rtroe (C) Virtex Edge Design
 * @brief This class allows access to all of the platform specific functions such as the signed in player, or unlocking achievements, viewing leaderboards etc...
 *
 */

namespace VerticesEngine.Profile
{
    /// <summary>
    /// This class allows access to all of the platform specific functions such as the signed in player, or unlocking achievements, viewing leaderboards etc...
    /// <example> 
    /// The default platform wrappers are provided for Steam, Google Play and iOS. You can create your own by implementing the <see cref="vxIPlayerProfile"/> interface
    /// and overriding the <see cref="VerticesEngine.vxGame.InitPlayerPlatformProfile()"/> method call.
    /// <code>
    /// public sealed class MyPlatformPlayerProfileWrapper : vxIPlayerProfile
    /// {
    /// ...
    /// }
    /// </code>
    /// </example>
    /// </summary>
    public class vxPlatform
    {
        /// <summary>
        /// This is the main player profile signed in to the game. You can unlock achievements and view leaderboards through this property here.
        /// </summary>
        public static vxIPlayerProfile Player
        {
            get { return _defaultPlayer; }
        }
        static vxIPlayerProfile _defaultPlayer;

        /// <summary>
        /// What platform are we currently running as?
        /// </summary>
        public static vxPlatformType Platform
        {
            get { return _defaultPlayer.PlatformType; }
        }



        /// <summary>
        /// initialises the player profile
        /// </summary>
        internal static void InitProfile()
        {
            _defaultPlayer = vxEngine.Game.InitPlayerPlatformProfile();
        }

        internal static void Initialise()
        {
            if (vxEngine.Game.HasProfileSupport)
                _defaultPlayer.Initialise();
        }

        internal static void Update()
        {
            if(_defaultPlayer != null)
                _defaultPlayer.Update();
        }

        /// <summary>
        /// Unlock Achievements for the current player
        /// </summary>
        /// <param name="key">The achievement key</param>
        public static void UnlockAchievement(object key)
        {
            _defaultPlayer.UnlockAchievement(key);
        }

        /// <summary>
        /// Gets the achievement for the current player
        /// </summary>
        /// <param name="key">The achievement key</param>
        /// <returns></returns>
        public static vxAchievement GetAchievement(object key)
        {
            return _defaultPlayer.GetAchievement(key);
        }
    }
}
