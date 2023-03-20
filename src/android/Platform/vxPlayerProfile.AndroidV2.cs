#if __ANDROID__
using Android.App;
using Android.Content;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common.Apis;
using Android.Gms.Games;
using Android.Gms.Games.LeaderBoard;
using Android.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using VerticesEngine.UI.Controls;

namespace VerticesEngine.Profile
{

    /// <summary>
    /// Basic wrapper for interfacing with the GooglePlayServices Game API's
    /// </summary>
    public sealed class vxPlayerProfileGooglePlayWrapperV2 : Java.Lang.Object, vxIPlayerProfile, vxGooglePlayServicePlatformWrapper
    {
        // The Games Activity
        Activity activity;

        public static bool CheckPermissions = false;

        GoogleSignInClient googleSignInClient;

        GoogleSignInAccount googleAccount;
        IAchievementsClient achievements;
        ILeaderboardsClient leaderboards;

        Dictionary<string, List<ILeaderboardScore>> scores = new Dictionary<string, List<ILeaderboardScore>>();


        // Activity Request Screens
        const int REQUEST_LEADERBOARD = 9002;
        const int REQUEST_ALL_LEADERBOARDS = 9003;
        const int REQUEST_ACHIEVEMENTS = 9004;
        const int RC_RESOLVE = 9001;
        const int RC_SIGN_IN = 9000;
        const int RC_PERM_REQ = 9100;


        /// <summary>
        /// Gets or sets the gravity for the GooglePlay Popups. 
        /// Defaults to Bottom|Center
        /// </summary>
        /// <value>The gravity for popups.</value>
        public GravityFlags GravityForPopups { get; set; }

        /// <summary>
        /// The View on which the Popups should show
        /// </summary>
        /// <value>The view for popups.</value>
        public View ViewForPopups { get; set; }

        /// <summary>
        /// This event is fired when a user successfully signs in
        /// </summary>
        public event EventHandler OnSignedIn;

        /// <summary>
        /// This event is fired when the Sign in fails for any reason
        /// </summary>
        public event EventHandler OnSignInFailed;

        /// <summary>
        /// This event is fired when the user Signs out
        /// </summary>
        public event EventHandler OnSignedOut;


        // Sign In - Out
        // **********************************************************
        public bool IsSignedIn
        {
            get
            {
                return _isSignedIn;
            }

        }
        private bool _isSignedIn = false;

        /// <summary>
        /// Gets the display name which is the Current Account Name.
        /// </summary>
        /// <value>The display name.</value>
        public string Name
        {
            get
            {
                return _name;
            }
        }
        private string _name = "android player";

        /// <summary>
        /// Gets the user identifier. This doesn't return anything for Google Serverices.
        /// </summary>
        /// <value>The user identifier.</value>
        public string Id
        {
            get { return _uid; }
        }
        private string _uid = string.Empty;


        public vxPlatformType PlatformType
        {
            get { return vxPlatformType.GooglePlayStore; }
        }



        public Texture2D Avatar
        {
            get
            {
                return null;
            }
        }

        public string PreferredLanguage
        {
            get { return "english"; }
        }




        public vxPlayerProfileGooglePlayWrapperV2()
        {
            this.activity = Game.Activity;
            this.GravityForPopups = GravityFlags.Top | GravityFlags.Center;

            if (vxEngine.Game.HasProfileSupport)
            {
                // Set Gravity and View for Popups
                GravityForPopups = (GravityFlags.Top | GravityFlags.Center);
                ViewForPopups = vxEngine.Game.Services.GetService<View>();

                // Hook up events
                OnSignedIn += (object sender, EventArgs e) =>
                {
                    Console.WriteLine("SignedIn!");
                    if (vxEngine.Game.IsGameContentLoaded)
                    {
                        vxNotificationManager.Add(new vxNotification("Signed In as: " + Name, Color.DeepSkyBlue));
                    }

                };
                OnSignInFailed += (object sender, EventArgs e) =>
                {
                    vxConsole.WriteError("Error Signing In!");
                    vxConsole.WriteError(e.GetType().ToString());
                    vxConsole.WriteError(e.ToString());
                    if (vxEngine.Game.IsGameContentLoaded)
                    {
                        vxNotificationManager.Add(new vxNotification("Error Signing In...", Color.Red));
                    }

                };
                OnSignedOut += (object sender, EventArgs e) =>
                {
                    Console.WriteLine("Sign Out");
                    if (vxEngine.Game.IsGameContentLoaded)
                    {
                        vxNotificationManager.Add(new vxNotification("Signed Out", Color.DarkViolet));
                    }
                    //_userName = "";

                };
            }
        }

        GoogleSignInOptions googleSignInOptions;
        public void Initialise()
        {
            googleSignInOptions = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultGamesSignIn)
       .RequestId()
       .RequestProfile()
       .RequestScopes(GamesClass.ScopeGames)
       .RequestScopes(GamesClass.ScopeGamesLite)
        .Build();

            googleSignInClient = GoogleSignIn.GetClient(Application.Context, googleSignInOptions);
            googleAccount = GoogleSignIn.GetLastSignedInAccount(Application.Context);
        }


        public void InitialisePlayerInfo()
        {

        }


        /// <summary>
        /// Start the GooglePlayClient. This should be called from your Activity Start
        /// </summary>
        public void Start()
        {

        }

        /// <summary>
        /// Disconnects from the GooglePlayClient. This should be called from your Activity Stop
        /// </summary>
        public void Stop()
        {

        }

        /// <summary>
        /// Reconnect to google play.
        /// </summary>
        public void Reconnect()
        {

        }



        /// <summary>
        /// Attempt to Sign in to Google Play
        /// </summary>
        public void SignIn()
        {
            if (googleAccount != null)
            {
#if !DEBUG
                if (CheckPermissions && GoogleSignIn.HasPermissions(googleAccount, googleSignInOptions.GetScopeArray()))
                {
                    GoogleSignIn.RequestPermissions(Game.Activity, RC_PERM_REQ, googleAccount, googleSignInOptions.GetScopeArray());
                }
#endif
                InternalSignIn();
            }
            else
            {
                Intent signInIntent = googleSignInClient.SignInIntent;

                Game.Activity.StartActivityForResult(signInIntent, RC_SIGN_IN);
            }
        }

        /// <summary>
        /// Sign out of Google Play and make sure we don't try to auto sign in on the next startup
        /// </summary>
        public void SignOut()
        {
            googleSignInClient?.RevokeAccess();
            googleSignInClient?.SignOut();
            _isSignedIn = false;
        }

        void InternalSignIn()
        {
            if (googleAccount == null)
                googleAccount = GoogleSignIn.GetLastSignedInAccount(Application.Context);


            GoogleSignInAccount signedInAccount = googleAccount;

            var gclient = GamesClass.GetGamesClient(Game.Activity, signedInAccount);//.SetViewForPopups(layout);
            ViewForPopups = vxEngine.Game.Services.GetService<View>();
            gclient.SetViewForPopups(ViewForPopups);

            achievements = GamesClass.GetAchievementsClient(Game.Activity, signedInAccount);
            leaderboards = GamesClass.GetLeaderboardsClient(Game.Activity, signedInAccount);

            var currentPlayer = GamesClass.GetPlayersClient(Game.Activity, signedInAccount).GetCurrentPlayer();

            currentPlayer.AddOnCompleteListener(Game.Activity, new TaskCallback<PlayerEntity>(
                (playerEntity) =>
                {
                    _name = playerEntity.DisplayName;
                    _uid = playerEntity.PlayerId;
                    _isSignedIn = true;

                    if (OnSignedIn != null)
                        OnSignedIn(this, null);
                },
                (error) =>
                {
                    vxConsole.WriteError(error);
                }));

            GamesClass.GetSnapshotsClient(vxGame.Activity, signedInAccount);

        }

        class TaskCallback<T> : Java.Lang.Object, Android.Gms.Tasks.IOnCompleteListener where T : Java.Lang.Object
        {
            Action<T> OnSuccess;

            Action<string> OnFailure;

            public TaskCallback(Action<T> OnSuccess, Action<string> OnFailure)
            {
                this.OnSuccess = OnSuccess;
                this.OnFailure = OnFailure;
            }


            public void OnComplete(Android.Gms.Tasks.Task task)
            {
                if (task.IsSuccessful)
                {
                    OnSuccess?.Invoke((T)task.Result);
                }
                else
                {
                    OnFailure?.Invoke(task.Exception?.Message);
                }
            }

        }

        /// <summary>
        /// Processes the Activity Results from the Signin process. MUST be called from your activity OnActivityResult override.
        /// </summary>
        /// <param name="requestCode">Request code.</param>
        /// <param name="resultCode">Result code.</param>
        /// <param name="data">Data.</param>
        public void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == RC_PERM_REQ)
            {
                if (resultCode == Result.Ok)
                {
                    InternalSignIn();
                }
                else
                {
                    if (OnSignInFailed != null)
                        OnSignInFailed(this, EventArgs.Empty);
                }
            }
            if (requestCode == RC_SIGN_IN)
            {
                if (resultCode == Result.Ok)
                {
                    InternalSignIn();
                }
                else
                {
                    if (OnSignInFailed != null)
                        OnSignInFailed(this, EventArgs.Empty);
                }
            }
        }



#region -- Achievements --

        public Dictionary<object, vxAchievement> Achievements
        {
            get
            {
                return _achievements;
            }
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
            achievements?.Increment(_achievements[key].ID, increment);
            _achievements[key].Achieved = true;
        }

        public void UnlockAchievement(object key)
        {            
            achievements?.Unlock(_achievements[key].ID);
            _achievements[key].Achieved = true;
            vxNotificationManager.Show("Achievement Unlocked! : " + key, Color.DeepPink);
        }

        public void ViewAchievments()
        {
            achievements?.GetAchievementsIntent().AddOnCompleteListener(Game.Activity, new TaskCallback<Intent>(
                (intent) =>
                {
                    activity.StartActivityForResult(intent, REQUEST_ACHIEVEMENTS);
                },
            (error) =>
            {
                vxConsole.WriteError(error);
            }));
        }

#endregion

#region -- Leaderboards --

        public void SubmitLeaderboardScore(string id, long score)
        {
            vxNotificationManager.Show("Score Submitted: " + score, Color.DeepPink);
            leaderboards?.SubmitScore(id, score);
        }

        public void ViewLeaderboard(string id)
        {
            leaderboards?.GetLeaderboardIntent(id).AddOnCompleteListener(Game.Activity, new TaskCallback<Intent>(
                (intent) =>
                {
                    activity.StartActivityForResult(intent, REQUEST_LEADERBOARD);
                },
            (error) =>
            {
                vxConsole.WriteError(error);
            }));
        }

        public void ViewAllLeaderboards()
        {
            leaderboards?.GetAllLeaderboardsIntent().AddOnCompleteListener(Game.Activity, new TaskCallback<Intent>(
                (intent) =>
                {
                    activity.StartActivityForResult(intent, REQUEST_ALL_LEADERBOARDS);
                },
            (error) =>
            {
                vxConsole.WriteError(error);
            }));
        }

#endregion


        public void Update()
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
            callback?.Invoke(false, vxInternalAssets.Textures.ErrorTexture);
        }

        public string GetAuthTicket()
        {
            return string.Empty;
        }

#region -- Social and Sharing --

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

        public string[] GetInstalledMods()
        {
            return new string[] { };
        }


        // User Info
        // **********************************************************
        public void ShareImage(string path, string extratxt = "")
        {
            try
            {
                var shareIntent = new Intent(Intent.ActionSend);
                //intent.PutExtra(Intent.ExtraSubject, subject);
                shareIntent.PutExtra(Intent.ExtraText, extratxt);

                Java.IO.File photoFile = new Java.IO.File(path);

                var uri = Android.Net.Uri.FromFile(photoFile);
                shareIntent.PutExtra(Intent.ExtraStream, uri);
                shareIntent.SetType("image/*");
                shareIntent.AddFlags(ActivityFlags.GrantReadUriPermission);

                Android.Content.PM.PackageManager pm = Game.Activity.PackageManager;
                var lract = pm.QueryIntentActivities(shareIntent, Android.Content.PM.PackageInfoFlags.MatchDefaultOnly);
                //bool resolved = false;
                //foreach (Android.Content.PM.ResolveInfo ri in lract)
                //{
                //    if (ri.ActivityInfo.Name.Contains("twitter"))
                //    {
                //        tweetIntent.SetClassName(ri.ActivityInfo.PackageName,
                //                ri.ActivityInfo.Name);
                //        resolved = true;
                //        break;
                //    }
                //}

                Game.Activity.StartActivity(Intent.CreateChooser(shareIntent, "Choose one"));
                //Android.App.Application.Context.StartActivity(Intent.CreateChooser(shareIntent, "Choose one"));
                //Android.App.Application.Context.StartActivity(resolved ?
                //tweetIntent :
                //Intent.CreateChooser(tweetIntent, "Choose one"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                vxNotificationManager.Show("Error Sharing Results...", Color.Red);
            }
        }

        public void OpenURL(string url)
        {
            Intent webIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(url));
            webIntent.SetFlags(Android.Content.ActivityFlags.NewTask);
            Android.App.Application.Context.StartActivity(webIntent);
        }

        public void OpenStorePage(string appid)
        {
            //        try {

            //            var uri = Android.Net.Uri.Parse("market://details?id=" + appid);
            //            var intent = new Android.Content.Intent(Android.Content.Intent.ActionView, uri);
            //            //Android.App.Application.Context.StartActivity(new Intent(Intent.ActionView, uri));
            //intent.SetFlags(Android.Content.ActivityFlags.NewTask);
            //Android.App.Application.Context.StartActivity(intent);

            //        } catch (Android.Content.ActivityNotFoundException anfe) {
            //            var uri = Android.Net.Uri.Parse("http://play.google.com/store/apps/details?id=" + appid);
            //var intent = new Android.Content.Intent(Android.Content.Intent.ActionView, uri);
            ////Android.App.Application.Context.StartActivity(new Intent(Intent.ActionView, uri));
            //intent.SetFlags(Android.Content.ActivityFlags.NewTask);
            //Android.App.Application.Context.StartActivity(intent);
            //}
            // you can also use BuildConfig.APPLICATION_ID

            Intent rateIntent = new Intent(Intent.ActionView,
                Android.Net.Uri.Parse("market://details?id=" + appid));
            bool marketFound = false;

            // find all applications able to handle our rateIntent
            var otherApps = Android.App.Application.Context.PackageManager.QueryIntentActivities(rateIntent, 0);
            foreach (Android.Content.PM.ResolveInfo otherApp in otherApps)
            {
                // look for Google Play application
                if (otherApp.ActivityInfo.ApplicationInfo.PackageName == "com.android.vending")
                {

                    Android.Content.PM.ActivityInfo otherAppActivity = otherApp.ActivityInfo;
                    ComponentName componentName = new ComponentName(
                        otherAppActivity.ApplicationInfo.PackageName,
                        otherAppActivity.Name
                            );
                    // make sure it does NOT open in the stack of your activity
                    rateIntent.AddFlags(ActivityFlags.NewTask);
                    // task reparenting if needed
                    rateIntent.AddFlags(ActivityFlags.ResetTaskIfNeeded);
                    // if the Google Play was already open in a search result
                    //  this make sure it still go to the app page you requested
                    rateIntent.AddFlags(ActivityFlags.ClearTop);
                    // this make sure only the Google Play app is allowed to
                    // intercept the intent
                    rateIntent.SetComponent(componentName);
                    Android.App.Application.Context.StartActivity(rateIntent);
                    marketFound = true;
                    break;

                }
            }

            // if GP not present on device, open web browser
            if (!marketFound)
            {
                Intent webIntent = new Intent(Intent.ActionView,
                Android.Net.Uri.Parse("https://play.google.com/store/apps/details?id=" + appid));
                webIntent.SetFlags(Android.Content.ActivityFlags.NewTask);
                Android.App.Application.Context.StartActivity(webIntent);
            }
        }
#endregion
    }
}
#endif