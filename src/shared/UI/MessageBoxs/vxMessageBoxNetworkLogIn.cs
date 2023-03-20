using System;
using Microsoft.Xna.Framework;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.MessageBoxs;

namespace VerticesEngine.UI.MessageBoxs
{
    /// <summary>
    /// A popup message box screen, used to display "are you sure?"
    /// confirmation messages.
    /// </summary>
	public class vxMessageBoxNetworkLogIn : vxMessageBox
    {
        #region Fields
        
        public vxTextbox Textbox;
        
        //File Name
        private string IpAddressToConnectTo = "127.0.0.1";
        
        #endregion
        
        #region Initialization


        /// <summary>
        /// Constructor automatically includes the standard "A=ok, B=cancel"
        /// usage text prompt.
        /// </summary>
		public vxMessageBoxNetworkLogIn(string ipAddressToConnectTo)
            : base("Enter Your Log In Information", "Log In")
        {
            IpAddressToConnectTo = ipAddressToConnectTo;

            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }
                
        /// <summary>
        /// Loads graphics content for this screen. This uses the shared ContentManager
        /// provided by the Game class, so the content will remain loaded forever.
        /// Whenever a subsequent MessageBoxScreen tries to load this same content,
        /// it will just get back another reference to the already loaded data.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            OKButton.Text = "Log In";
            CancelButton.Text = "Offline";
        }
        #endregion
    }
}