using System;
using VerticesEngine.UI;
using VerticesEngine.UI.MessageBoxs;

namespace VerticesEngine.Net.UI
{
    /// <summary>
    /// The base sever creation busy dialog for creating Servers. The LAN and Steam P2P sessions inherit from this.
    /// </summary>
    internal abstract class vxMultiplayerBaseCreateServerBusyDialog : vxMessageBox
    {
        /// <summary>
        /// Signing Timeout
        /// </summary>
        protected int TimeOut = 5;

        /// <summary>
        /// The amount of time to wait before sending the request
        /// </summary>
        protected float TimeToSendRequest = 0.50f;

        protected float curTime = 0;

        protected string MainMessageText = "";

        protected bool IsDotAnimEnabled = true;



        /// <summary>
        /// Callback when we've successfully connected to the server
        /// </summary>
        public event Action<string, string, int> OnServerCreated = (string name, string ip, int port) => { };

        /// <summary>
        /// Callback when we've hit an error or issue when creating the server
        /// </summary>
        public event Action<string, string, int> OnError = (string name, string ip, int port) => { };


        /// <summary>
        /// The Network Backend used for creating this server
        /// </summary>
        protected virtual vxNetworkBackend NetworkBackend
        {
            get
            {
                throw new Exception("Network Backend not specified for " + this.GetType());
            }
        }
        int PortRange
        {
            get { return vxNetworkManager.Config.ServerLANPortRange; }
        }

        /// <summary>
        /// The default server port. The initial value is taken from 'vxNetworkManager.Config.ServerLANPortRange'.
        /// </summary>
        protected virtual int DefaultServerPort
        {
            get { return vxNetworkManager.Config.ServerLANPortRange; }
        }


        private int periodIncrementer = 0;

        private string buffer = "\n" + new string(' ', 64);

        private bool isTimedOut = false;

        private string _serverName;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.Net.UI.vxMultiplayerServerConnectBusyDialog"/> class.
        /// </summary>
        public vxMultiplayerBaseCreateServerBusyDialog(string serverName)
            : base("Creating Server", "Creating Server", vxEnumButtonTypes.None)
        {
            IsPopup = true;
            _serverName = serverName;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);

            MainMessageText = string.Format("Creating Server {0}\nPlease Wait ", serverName);

        }


        private bool isConnectionRequested = false;

        private void CreateServer()
        {
            bool KeepLooping = true;

            int port = vxNetworkManager.Config.ServerLANDefaultPort;

            while (KeepLooping)
            {
                try
                {
                    vxNetworkManager.Server.Initialise(NetworkBackend, _serverName, vxEngine.Game.Version, port,  delegate
                    {
                        vxConsole.NetLog("SERVER IS Initialised");

                        //Now Start The Server
                        vxNetworkManager.Server.Start();

                        OnServerCreated?.Invoke(_serverName, "localhost", port);

                        ExitScreen();
                    });

                    //Set the User's Network Roll to be Server.
                    vxNetworkManager.Client.SetPlayerNetworkRole(vxEnumNetworkPlayerRole.Server);

                    KeepLooping = false;
                }
                catch (Exception ex)
                {
                    if (port > DefaultServerPort + PortRange)
                        KeepLooping = false;

                    vxMessageBox.Show("Error Creating Server", "Could not start Server on port: " + port);

                    vxConsole.WriteWarning(this.GetType().ToString(), "Could not start Server on port: " + port);
                    vxConsole.WriteException(this, ex);

                    port++;
                }
            }
        }

        protected internal override void Update()
        {
            curTime += vxTime.DeltaTime;

            // if we're past TimeToSendRequest seconds and haven't requested to connect yet, then let's do that
            if (curTime > TimeToSendRequest && isConnectionRequested == false)
            {
                isConnectionRequested = true;
                CreateServer();
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
            //OnError?.Invoke(ConnectionErrorIssue.TimedOut);
        }
    }
}
