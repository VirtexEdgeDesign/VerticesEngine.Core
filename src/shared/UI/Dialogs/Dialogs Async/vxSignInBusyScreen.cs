using System;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using VerticesEngine.Util;

using VerticesEngine.UI.Controls;
using VerticesEngine.UI.MessageBoxs;
using VerticesEngine.Serilization;
using VerticesEngine.Utilities;
using VerticesEngine.Graphics;
using VerticesEngine.Profile;

namespace VerticesEngine.UI
{
    /// <summary>
    /// The sign in busy screen.
    /// </summary>
    public class vxSignInBusyScreen : vxMessageBox
    {
        int Inc = 0;

        /// <summary>
        /// Signing Timeout
        /// </summary>
        protected int TimeOut = 5;

        protected float curTime = 0;

        protected string MainMessageText = "Signing In";

        string buffer = "\n" + new string(' ', 64);

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.vxSignInBusyScreen"/> class.
        /// </summary>
        public vxSignInBusyScreen()
            : base("Signing In", "Signing In", vxEnumButtonTypes.None)
        {
            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            TimeOut = 1;

            MainMessageText = "Signing In \nPlease Wait";
#if DEBUG

            TimeOut = 1;
#endif
        }

        protected bool IsDotAnimEnabled = true;

        public override void UnloadContent()
        {
            // if the initialise step is waiting, then kick it to the next level
            if(vxEngine.Game.InitializationStage == GameInitializationStage.Waiting)
            {
                vxEngine.Game.InitializationStage = GameInitializationStage.CheckIfUpdated;
            }
            base.UnloadContent();
        }

        protected virtual bool CanExit()
        {
            return curTime > 2.5f && vxPlatform.Player.IsSignedIn;
        }



        protected internal override void Update()
        {
            curTime += vxTime.DeltaTime;


            if (curTime > 1.0f)
            {
                if (vxPlatform.Player.IsSignedIn == false)
                    vxPlatform.Player.SignIn();
            }


            Inc++;

            string SavingText = MainMessageText + (IsDotAnimEnabled == true ? new string('.', (int)(Inc / 10) % 5) : "");

            Message = SavingText + buffer;

            // we've either passed the timeout OR we're signed in, then exit
            if (CanExit())
            {
                ExitScreen();
            }



            // have we passed the timeout period?
            if (curTime > TimeOut)
            {
                if (isTimedOut == false)
                {
                    isTimedOut = true;
                    Message = "Request Timed Out...";

                    OnTimeOut();
                }
            }

            if (curTime > TimeOut + 2.5f)
            {
                ExitScreen();
            }

            base.Update();
        }

        private bool isTimedOut = false;

        protected virtual void OnTimeOut()
        {

        }
    }
}
