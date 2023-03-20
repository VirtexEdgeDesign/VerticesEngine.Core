using System;
using Microsoft.Xna.Framework;
using VerticesEngine;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.MessageBoxs;
using VerticesEngine.Net;

namespace VerticesEngine.UI.MessageBoxs
{
    /// <summary>
    /// A popup message box screen, used to display "are you sure?"
    /// confirmation messages.
    /// </summary>
	public class vxMessageBoxNetworkLoggingIn : vxMessageBox
    {
        #region Fields
        
		/// <summary>
		/// The textbox.
		/// </summary>
        public vxTextbox Textbox;
        
        //File Name
        private string IpAddressToConnectTo = "127.0.0.1";
        private int Port = 14242;
        private string HailMsg = "vrtx_hail_msg";
        
        #endregion

        #region Events

        ///// <summary>
        ///// Event Fired when the Log in is Successful
        ///// </summary>
        //public event EventHandler<EventArgs> LogInSuccessful;

        ///// <summary>
        ///// Event Fired when the Log In is UNSucessful
        ///// </summary>
        //public event EventHandler<EventArgs> LogInUNSuccessful;

        #endregion
        
        #region Initialization


        /// <summary>
        /// Constructor automatically includes the standard "A=ok, B=cancel"
        /// usage text prompt.
        /// </summary>
		public vxMessageBoxNetworkLoggingIn(string ipAddressToConnectTo, int port, string hailMsg)
            : base("Enter Your Log In Information", "Logging In...")
        {
            this.IpAddressToConnectTo = ipAddressToConnectTo;
            this.Port = port;
            this.HailMsg = hailMsg;

            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }

        int Inc = 0;
        /// <summary>
        /// Loads graphics content for this screen. This uses the shared ContentManager
        /// provided by the Game class, so the content will remain loaded forever.
        /// Whenever a subsequent MessageBoxScreen tries to load this same content,
        /// it will just get back another reference to the already loaded data.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            
            //Reset the Button Positions
            //Set Gui Item Positions
            OKButton.Position = new Vector2(-100,-100);            
        }

        //bool FirstLoop = true;
        //int SuccessPoint = 0;
        protected internal override void Update()
        {
            if (Inc == 25)
            {
                //Engine.Connect(this.IpAddressToConnectTo, this.Port, this.HailMsg);
                //FirstLoop = false;
            }
            string LogInText = "Logging In To Server ";

            Inc++;

            LogInText += new string('.', (int)(Inc / 10) % 5);

            //if (Engine.MasterServerConnectionStatus == vxEnumNetworkConnectionStatus.Running)
            //{
            //    Title = "Success";
            //    LogInText = "Logged In!";
            //    if (SuccessPoint == 0)
            //        SuccessPoint = Inc;

            //    if (Inc - SuccessPoint > 25)
            //    {
            //        OnCancel();

            //        if (LogInSuccessful != null)
            //            LogInSuccessful(this, new EventArgs());
            //    }
            //}
            //else if (Inc > 250 && Engine.MasterServerConnectionStatus == vxEnumNetworkConnectionStatus.Stopped)
            //{
            //    Title = "Connection Timed Out";
            //    LogInText = "Please Try Again Later.";

            //    if (LogInUNSuccessful != null)
            //        LogInUNSuccessful(this, new EventArgs());
            //}

            Message = LogInText;

            base.Update();

        }
        #endregion
    }
}
