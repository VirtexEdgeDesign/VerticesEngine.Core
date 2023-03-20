
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using VerticesEngine.UI.Controls;

namespace VerticesEngine.Profile
{
    /// <summary>
    /// Basic wrapper for interfacing with a Specific Services Game API's. This is the generic version.
    /// </summary>
    public sealed class vxPlayerProfileGenericWrapper : vxIPlayerProfile
    {
#pragma warning disable 067, 649
        public vxPlayerProfileGenericWrapper()
        {

        }


        // Sign In - Out
        // **********************************************************

        public bool IsSignedIn
        {
            get { return _isSignedIn; }
        }

        private bool _isSignedIn = false;

        
        public void SignIn()
        {
            _isSignedIn = true;
            vxNotificationManager.Show("Signing In ...", Color.DeepPink);
        }

        public void SignOut()
        {
            _isSignedIn = false;
            vxNotificationManager.Show("Signing Out ...", Color.DeepPink);
        }

        /// <summary>
        /// This event is fired when a user successfully signs in
        /// </summary>
        public event EventHandler OnSignedIn;

        /// <summary>
        /// This event is fired when the Sign in fails for any reason
        /// </summary>
        public event EventHandler OnSignInFailed;

        // User Info
        // **********************************************************


        public void ShareImage(string path, string extratxt = "")
        {
            //throw new NotImplementedException();
        }

        public string Name
        {
            get { return m_displayName; }
        }
        private string m_displayName = "PlayerOne";

        public string Id
        {
            get { return "1234"; }
        }


        public vxPlatformType PlatformType
        {
            get { return vxPlatformType.None; }
        }


        // Achievements
        // **********************************************************





        public Texture2D Avatar
        {
            get
            {
                return vxInternalAssets.Textures.DefaultDiffuse;
            }
        }

        public string PreferredLanguage
        {
            get { return "english"; }
        }

        public Dictionary<object, vxAchievement> Achievements 
        { 
            get 
            {
                return _achievements;
            } 
        }

        public bool IsPublishing
        {
            get; set;
        }

        public float PublishProgress
        {
            get; set;
        }

        Dictionary<object, vxAchievement> _achievements = new Dictionary<object, vxAchievement>();

        public void AddAchievement(object key, vxAchievement achievement)
        {
            _achievements.Add(key, achievement);
        }

        public vxAchievement GetAchievement(object key)
        {
            return _achievements[key];
        }

        public Dictionary<object, vxAchievement> GetAchievements()
        {
            return _achievements;
        }

        public void IncrementAchievement(object key, int increment)
        {
            //Achievements[key].
        }

        public void UnlockAchievement(object key)
        {
            try
            {
                _achievements[key].Achieved = true;
                vxNotificationManager.Add(new vxNotification("Achievement Unlocked! : " + key, Color.DeepPink));
            }
            catch { }
        }

        public void ViewAchievments()
        {

        }




        // Leaderboards
        // **********************************************************
        public void SubmitLeaderboardScore(string id, long score)
        {
            vxNotificationManager.Add(new vxNotification("Score Submitted: " + score, Color.DeepPink));
        }

        public void ViewLeaderboard(string id)
        {

        }

        public void Initialise()
        {
            //throw new NotImplementedException();
        }

        public void InitialisePlayerInfo()
        {
            //throw new NotImplementedException();
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public void ViewAllLeaderboards()
        {
            //throw new NotImplementedException();
        }

        public void Update()
        {
            //throw new NotImplementedException();
        }

        public void OpenURL(string url)
        {  try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception e)
            {
                vxConsole.WriteError(e.Message);
            }
        }

        public void OpenStorePage(string url)
        { 
            OpenURL(url);
        }


        public string[] GetInstalledMods()
        {
            return new string[] { };
        }

        public void SetStatus(string status)
        {
            //throw new NotImplementedException();
        }

        public void SetStatusKey(string key, string value)
        {
            //throw new NotImplementedException();
        }

        public void ClearStatus()
        {
            //throw new NotImplementedException();
        }

        public void SetStat(string key)
        {

        }

        public void SetStat(string key, int value)
        {

        }

        public void GetPlayerIconFromPlatform(string id, Action<bool, Texture2D> callback)
        {
            callback?.Invoke(false, null);
        }

        public string GetAuthTicket()
        {
            return string.Empty;
        }
#pragma warning restore 067, 649
    }
}
