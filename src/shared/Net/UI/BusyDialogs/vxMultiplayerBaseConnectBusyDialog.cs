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
using VerticesEngine.UI;

namespace VerticesEngine.Net.UI
{
    public enum ConnectionErrorIssue
    {
        TimedOut,
        ServerDenied
    }
    /// <summary>
    /// The sever connection busy dialog. This is used for connecting to a multiplayer server.
    /// </summary>
    internal class vxMultiplayerBaseConnectBusyDialog : vxMessageBox
    {
        /// <summary>
        /// Signing Timeout
        /// </summary>
        protected int TimeOut = 5;

        /// <summary>
        /// The amount of time to wait before sending the request
        /// </summary>
        protected float TimeToSendRequest = 1.0f;

        protected float curTime = 0;

        protected string MainMessageText = "";

        protected bool IsDotAnimEnabled = true;

        /// <summary>
        /// Callback when we've successfully connected to the server
        /// </summary>
        public event Action OnServerJoined = () => { };

        /// <summary>
        /// Callback for server connection errors
        /// </summary>
        public event Action<ConnectionErrorIssue> OnError = (ConnectionErrorIssue err) => { };


        private int periodIncrementer = 0;

        private string buffer = "\n" + new string(' ', 64);

        private bool isTimedOut = false;

        string ipAddress;

        int port;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.Net.UI.vxMultiplayerServerConnectBusyDialog"/> class.
        /// </summary>
        public vxMultiplayerBaseConnectBusyDialog(string serverName, string ipAddress, int port)
            : base("Joining Server", "Joining Lobby", vxEnumButtonTypes.None)
        {
            IsPopup = true;
            this.ipAddress = ipAddress;
            this.port = port;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);

            //TimeOut = 2.5f;

            MainMessageText = string.Format("Joining Server {0}\nPlease Wait ", serverName);

            vxNetworkManager.Client.Connected += OnClientConnected;
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            vxNetworkManager.Client.Connected -= OnClientConnected;
        }

        private void OnClientConnected(object sender, Events.vxNetClientEventConnected e)
        {
            OnServerJoined.Invoke();
            MainMessageText = "Connected\n";
            ExitScreen();
        }

        private bool isConnectionRequested = false;

        /// <summary>
        /// Called when the client requests to connect to the server
        /// </summary>
        protected virtual void RequestConnection()
        {
            vxConsole.NetLog(string.Format("Connecting to Server: {0} : {1}", ipAddress, port));
            var approval = vxNetworkManager.Client.CreateMessage();
            approval.Write("secret");
            vxNetworkManager.Client.Connect(ipAddress, port, approval);
            vxConsole.NetLog("Connection Request Sent!");
        }

        protected internal override void Update()
        {
            curTime += vxTime.DeltaTime;

            // if we're past TimeToSendRequest seconds and haven't requested to connect yet, then let's do that
            if (curTime > TimeToSendRequest && isConnectionRequested == false)
            {
                isConnectionRequested = true;
                RequestConnection();
            }


            // update the message text
            periodIncrementer++;
            string SavingText = MainMessageText + (IsDotAnimEnabled == true ? new string('.', (int)(periodIncrementer / 10) % 5) : "");
            Message = SavingText + buffer;



            // have we passed the timeout period?
            if (curTime > TimeOut)
            {
                if (isTimedOut == false)
                {
                    isTimedOut = true;
                    Message = "Request Timed Out...";

                    OnTimeOut();
                }

                // If we've timed out then 
                if (curTime > TimeOut + 2.5f)
                {
                    ExitScreen();
                }
            }


            base.Update();
        }

        protected virtual void OnTimeOut()
        {
            OnError?.Invoke(ConnectionErrorIssue.TimedOut);
        }
    }
}
