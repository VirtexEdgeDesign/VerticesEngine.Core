using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace VerticesEngine.Profile
{
    /// <summary>
    /// Platform specific wrapper for handling player profile. This is used for providing which player is currently playing through steam.
    /// </summary>
    public interface vxIPlayerProfile
    {
        /// <summary>
        /// Is the user signed in.
        /// </summary>
        /// <returns><c>true</c>, if signed in was ised, <c>false</c> otherwise.</returns>
        bool IsSignedIn { get; }


        /// <summary>
        /// Gets the user identifier. This identifier is handed out by the virtex server.
        /// </summary>
        /// <value>The user identifier.</value>
        string Id { get; }

        /// <summary>
        /// Gets the name of the signed in user.
        /// </summary>
        /// <value>The name of the user.</value>
        string Name { get; }


        /// <summary>
        /// This Initialises the main info
        /// </summary>
        void Initialise();

        /// <summary>
        /// This initialises the player info like icons. This is called later on as the graphics device must be active.
        /// </summary>
        void InitialisePlayerInfo();

        /// <summary>
        /// Shutdown and close any connections
        /// </summary>
        void Dispose();

        /// <summary>
        /// Gets the Player Profile Backend, whether it's Steam, Google Play Services, etc...
        /// </summary>
        vxPlatformType PlatformType { get; }

        /// <summary>
        /// Gets the profile picture.
        /// </summary>
        /// <value>The profile picture.</value>
        Texture2D Avatar { get; }

        /// <summary>
        /// Signs the user in.
        /// </summary>
        /// <returns><c>true</c>, if in was signed, <c>false</c> otherwise.</returns>
        void SignIn();


        /// <summary>
        /// Signs the user out.
        /// </summary>
        /// <returns><c>true</c>, if out was signed, <c>false</c> otherwise.</returns>
        void SignOut();

        /// <summary>
        /// Returns an auth ticket for this platform as a string. This allows developers to get an auth ticket or token which they
        /// can then pass on to a backend server to authenticate that the user is who they say they are.
        /// </summary>
        /// <returns></returns>
        string GetAuthTicket();

        /// <summary>
        /// This method will scrub all names and profile text info for any characters which aren't
        /// available in the spritefonts which have been loaded.
        /// </summary>
        //void ScrubNames();
        string PreferredLanguage { get; }

        /// <summary>
        /// Returns the player icon for the current platform using the specified id. 
        /// </summary>
        /// <param name="id">The player profile icon</param>
        /// <param name="callback">The callback which says whether it was successful and with the texture2D</param>
        void GetPlayerIconFromPlatform(string id, Action<bool, Texture2D> callback);

        /// <summary>
        /// Runs callbacks for platform wrapper
        /// </summary>
        void Update();

        #region -- Sharing and Social --

        /// <summary>
        /// Shares an image. This is only supported on Mobile currently
        /// </summary>
        /// <param name="path"></param>
        /// <param name="extratxt"></param>
        void ShareImage(string path, string extratxt = "");

        /// <summary>
        /// Opens a URL using the specified platform
        /// </summary>
        /// <param name="url"></param>
        void OpenURL(string url);

        /// <summary>
        /// Opens the store page for this game given the app id info in the Config.
        /// </summary>
        /// <param name="url"></param>
        void OpenStorePage(string url);

        /// <summary>
        /// Sets the status for this user. This is useful for Discord and Steam Rich Presence
        /// </summary>
        /// <param name="status"></param>
        void SetStatus(string status);

        /// <summary>
        /// Sets the status for this user. This is useful for Discord and Steam Rich Presence
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetStatusKey(string key, string value);

        /// <summary>
        /// Clears the status for this user. This is useful for Discord and Steam Rich Presence
        /// </summary>
        void ClearStatus();

        #endregion

        #region -- Achievements --

        /// <summary>
        /// Retusn the acehivement look up dictionary
        /// </summary>
        Dictionary<object, vxAchievement> Achievements { get; }

        /// <summary>
        /// Gets the achievement.
        /// </summary>
        /// <returns>The achievement.</returns>
        /// <param name="key">Key.</param>
        vxAchievement GetAchievement(object key);


        /// <summary>
        /// Unlocks the achievement.
        /// </summary>
        /// <param name="key">Key.</param>
        void UnlockAchievement(object key);

        /// <summary>
        /// Increments the achievement.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="increment"></param>
        void IncrementAchievement(object key, int increment);

        /// <summary>
        /// Sets a stat value. This will incrememne the current value.
        /// </summary>
        /// <param name="key"></param>
        void SetStat(string key);

        /// <summary>
        /// Sets a stat value. This will explicitly set a value.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetStat(string key, int value);

        /// <summary>
        /// Adds the achievement.
        /// </summary>
        void AddAchievement(object key, vxAchievement achievement);

        /// <summary>
        /// View achievements for this platform
        /// </summary>
        void ViewAchievments();

        #endregion

        #region -- Leaderboards --

        /// <summary>
        /// Submits a leaderboard score for this platform
        /// </summary>
        /// <param name="id"></param>
        /// <param name="score"></param>
        void SubmitLeaderboardScore(string id, long score);

        /// <summary>
        /// View the leaderboard for this platform
        /// </summary>
        /// <param name="id"></param>
        void ViewLeaderboard(string id);

        /// <summary>
        /// Views all leaderboards for this platform
        /// </summary>
        void ViewAllLeaderboards();

        #endregion

        #region -- Mods --

        /// <summary>
        /// Gets all of the currently installed mods. Note this does not mean
        /// they are enabled.
        /// </summary>
        /// <returns></returns>
        string[] GetInstalledMods();

        #endregion
    }
}
