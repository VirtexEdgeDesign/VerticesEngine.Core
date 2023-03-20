#if __IOS__
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VerticesEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Foundation;
using GameKit;
using UIKit;
using VerticesEngine.UI.Controls;

namespace VerticesEngine.Profile
{
    /// <summary>
    /// Basic wrapper for interfacing with the GooglePlayServices Game API's
    /// </summary>
    public sealed class vxPlayerProfileiOSWrapper : vxIPlayerProfile
    {
        UIViewController ViewController
        {
            get
            {
                return vxEngine.Game.Services.GetService(typeof(UIViewController)) as UIViewController;
            }
        }



        public bool IsSignedIn
        {
            get { return GKLocalPlayer.LocalPlayer.Authenticated; }
        }


        public string Name
        {
            get { return m_displayName; }
        }
        private string m_displayName = "Player";



        public void SignIn()
        {
            GKLocalPlayer.LocalPlayer.AuthenticateHandler = (ui, error) =>
            {

                // If ui is null, that means the user is already authenticated,
                // for example, if the user used Game Center directly to log in

                if (ui != null)
                    Console.WriteLine(ui);
                else
                {
                    // Check if you are authenticated:
                    var authenticated = GKLocalPlayer.LocalPlayer.Authenticated;

                    if (authenticated == true)
                    {

                        //AppDelegate.Shared.ViewController.PresentViewController(controller, true, null);
                    }
                    m_displayName = GKLocalPlayer.LocalPlayer.Alias;

                    GKAchievementDescription.LoadAchievementDescriptions(delegate (GKAchievementDescription[] achievements, NSError er)
                    {
                        if (achievements != null)
                        {
                            foreach (GKAchievementDescription a in achievements)
                            {
                                Console.WriteLine(new
                                {
                                    Name = a.Title,
                                    Key = a.Identifier,
                                    Description = a.AchievedDescription,
                                    HowToEarn = a.UnachievedDescription,
                                    DisplayBeforeEarned = !a.Hidden
                                });

                                if(!iosAchivements.ContainsKey(a.Identifier))
                                    iosAchivements.Add(a.Identifier, a);
                            }
                        }
                    });
                }
                Console.WriteLine("Authentication result: {0}", error);
            };
        }

        private Dictionary<string, GKAchievementDescription> iosAchivements = new Dictionary<string, GKAchievementDescription>();
        public void SignOut()
        {

        }


        public vxPlayerProfileiOSWrapper()
        {

        }



        // User Info
        // **********************************************************



        public void ShareImage(string path, string extratxt = "")
        {
            try
            {
                //UIViewController ViewController = vxEngine.CurrentGame.Services.GetService(typeof(UIViewController)) as UIViewController;

                UIImage image = new UIImage(path);
                NSObject[] activityItems = { image };

                UIActivityViewController activityViewController = new UIActivityViewController(activityItems, null);
                activityViewController.ExcludedActivityTypes = new NSString[] { };

                if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
                {
                    activityViewController.PopoverPresentationController.SourceView = ViewController.View;
                    activityViewController.PopoverPresentationController.SourceRect = new CoreGraphics.CGRect((ViewController.View.Bounds.Width / 2), (ViewController.View.Bounds.Height / 4), 0, 0);
                }

                ViewController.PresentViewController(activityViewController, true, null);
            }
            catch
            {
                vxNotificationManager.Add(new vxNotification("Error Sharing Results...", Color.Red));
            }
        }




        public string[] GetInstalledMods()
        {
            return new string[] { };
        }


        public string Id
        {
            get { return GKLocalPlayer.LocalPlayer.PlayerID; }
        }

        
        public vxPlatformType PlatformType
        {
            get { return vxPlatformType.AppleAppStore; }
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




        // Achievements
        // **********************************************************


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
            _achievements[key].Achieved = true;
        }


        public void UnlockAchievement(object key)
        {
            try
            {
                var achievement = new GKAchievement(Achievements[key].ID)
                {
                    PercentComplete = 100
                    //PercentComplete = 100
                };

                //iosAchivements[""].ac

                GKAchievement.ReportAchievements(new[] { achievement },
                                                 delegate (NSError error)
                                                {
                                                    if (error != null)
                                                        Console.WriteLine(error.ToString());
                                                    else
                                                        Console.WriteLine("ERROR IS NULL");
                                                });
                //GKAchievement.ReportAchievementsAsync(new[] { achievement }, null);
                //vxNotificationManager.Add(new vxNotification(Engine, "Achievement Unlocked! : " + key, Color.DeepPink));


                _achievements[key].Achieved = true;
            }
            catch
            {
                vxConsole.WriteLine("Error Creating Achievement");
            }
            vxNotificationManager.Add(new vxNotification("Achievement Unlocked! : " + key, Color.DeepPink));
        }

        public void ViewAchievments()
        {
            GKGameCenterViewController controller = new GKGameCenterViewController();
            controller.Finished += (object sender, EventArgs e) =>
            {
                controller.DismissViewController(true, null);
            };
            controller.ViewState = GKGameCenterViewControllerState.Achievements;
            ViewController.PresentViewController(controller, true, null);
        }




        // Leaderboards
        // **********************************************************
        public void SubmitLeaderboardScore(string id, long score)
        {
            try
            {
                GKScore newScore = new GKScore(id);
                newScore.Value = score;
                GKScore.ReportScores(new[] { newScore },
                                                     delegate (NSError error)
                                                     {
                                                         if (error != null)
                                                             Console.WriteLine(error.ToString());
                                                         else
                                                             Console.WriteLine("ERROR IS NULL");
                                                     });
            }
            catch { }
        }

        public void ViewLeaderboard(string id)
        {

            GKGameCenterViewController controller = new GKGameCenterViewController();
            controller.Finished += (object sender, EventArgs e) =>
            {
                controller.DismissViewController(true, null);
            };
            controller.ViewState = GKGameCenterViewControllerState.Leaderboards;
            ViewController.PresentViewController(controller, true, null);
        }


        public void Initialise()
        {

        }

        public void InitialisePlayerInfo()
        {

        }

        public void Dispose()
        {

        }

        public void ViewAllLeaderboards()
        {
            GKGameCenterViewController controller = new GKGameCenterViewController();
            controller.Finished += (object sender, EventArgs e) =>
            {
                controller.DismissViewController(true, null);
            };
            controller.ViewState = GKGameCenterViewControllerState.Leaderboards;
            ViewController.PresentViewController(controller, true, null);
        }

        public void Update()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Opens a URL from in the game
        /// </summary>
        /// <param name="url"></param>
        public void OpenURL(string url)
        {
            UIKit.UIApplication.SharedApplication.OpenUrl(new Foundation.NSUrl(url));
        }

        public void OpenStorePage(string appid)
        {
            Console.WriteLine("OpenStorePage()");
            UIKit.UIApplication.SharedApplication.OpenUrl(new Foundation.NSUrl(@"itms-apps://itunes.apple.com/app/id"+appid));
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
            //throw new NotImplementedException();
        }

        public void SetStat(string key, int value)
        {
            //throw new NotImplementedException();
        }

        public void GetPlayerIconFromPlatform(string id, Action<bool, Texture2D> callback)
        {
            callback?.Invoke(false, vxInternalAssets.Textures.ErrorTexture);
        }

        public string GetAuthTicket()
        {
            return string.Empty;
        }
    }
}
#endif