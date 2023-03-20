using System;
using System.Collections.Generic;
using System.Text;
using VerticesEngine.Monetization.Purchases;
using VerticesEngine.Profile;

namespace VerticesEngine.InitSteps
{
    /// <summary>
    /// Signs in the user
    /// </summary>
    public class SignInUserInitStep : vxIInitializationStep
    {
        public bool IsComplete
        {
            get { return _isComplete; }
        }
        protected bool _isComplete;

        public string Status
        {
            get { return _status; }
        }
        protected string _status;



        /// <summary>
        /// Signing Timeout
        /// </summary>
        protected int TimeOut = 5;

        protected float curTime = 0;

        public void Start()
        {
            vxEngine.Game.InitializationStage = GameInitializationStage.SigningInUser;

            string platformName = "Your Account";

            switch(vxPlatform.Platform)
            {
                case vxPlatformType.GooglePlayStore:
                    platformName = "Google Play";
                    break;
                case vxPlatformType.AppleAppStore:
                    platformName = "Game Center";
                    break;
                case vxPlatformType.Steam:
                    platformName = "Steam";
                    break;
                case vxPlatformType.ItchIO:
                    platformName = "ItchIO";
                    break;
                case vxPlatformType.Discord:
                    platformName = "Discord";
                    break;
                case vxPlatformType.NintendoSwitch:
                    platformName = "Nintendo";
                    break;
            }

            _status = string.Format("Signing In to {0}", platformName);
#if DEBUG
            TimeOut = 2;
#endif
        }

        protected virtual bool CanExit()
        {
            return curTime > TimeOut + 1.5f || vxPlatform.Player.IsSignedIn;
        }

        bool isLoggingIn = false;
        private bool hasPurchasesChecked = false;
        public void Update()
        {
               curTime += vxTime.DeltaTime;

            if (curTime > 0.50f && isLoggingIn == false)
            {
                isLoggingIn = true;
                if (vxPlatform.Player.IsSignedIn == false)
                    vxPlatform.Player.SignIn();
            }


            //_status = MainMessageText;

            // we've either passed the timeout OR we're signed in, then exit
            if (CanExit())
            {
                _isComplete = true;
                if (hasPurchasesChecked == false)
                {
                    hasPurchasesChecked = true;
                    vxInAppProductManager.Instance.RestorePurchases();
                }
            }



            // have we passed the timeout period?
            if (curTime > TimeOut)
            {
                if (isTimedOut == false)
                {
                    isTimedOut = true;
                    _status = "Request Timed Out...";

                    OnTimeOut();
                }
            }
        }

        private bool isTimedOut = false;

        protected virtual void OnTimeOut()
        {

        }
    }
}
