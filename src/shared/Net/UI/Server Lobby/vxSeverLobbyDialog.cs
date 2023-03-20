using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using VerticesEngine;
using VerticesEngine.Utilities;
using VerticesEngine.Input;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Events;
using VerticesEngine.UI.Dialogs;
using VerticesEngine.Net.Events;
using VerticesEngine.UI;
using Lidgren.Network;
using VerticesEngine.Net;
using VerticesEngine.UI.MessageBoxs;
using VerticesEngine.Net.Messages;
using VerticesEngine.Graphics;


namespace VerticesEngine.Net.UI
{

    enum SessionState
    {
        Idle,
        Countdown,
        Launch
    }

    /// <summary>
    /// This is a Server Lobby Dialog is the 'waiting room' before a session launch.
    /// </summary>
    public class vxSeverLobbyDialog : vxDialogBase
    {
        #region Fields

        /// <summary>
        /// Client Callback Loop
        /// </summary>
        public SendOrPostCallback ClientCallBackLoop;

        public SendOrPostCallback ServerCallBackLoop;

        /// <summary>
        /// If the player is acting as Server, then let the player launch the session.
        /// </summary>
        private vxButtonControl m_launchServerButton;

        /// <summary>
        /// The vxListView GUI item which contains the list of all broadcasting servers on the subnet.
        /// </summary>
        private vxScrollPanel m_playersInLobbyScrollPanel;

        //SessionState SessionStateForServer = SessionState.Idle;
        SessionState SessionStateForClient = SessionState.Idle;


        //A Collection of General Messages From The Server
        const string SVRMSG_SERVER_SHUTDOWN_REASON = "SVRMSG_SERVER_SHUTDOWN_REASON";
        const string SVRMSG_UPDATE_PLAYER_LIST = "SVRMSG_UPDATE_PLAYER_LIST";
        const string SVRMSG_UPDATE_PLAYER_STATUS = "SVRMSG_UPDATE_PLAYER_STATUS";
        const string SVRMSG_LAUNCH_START = "SVRMSG_LAUNCH_START";
        const string SVRMSG_LAUNCH_COUNTDOWN = "SVRMSG_LAUNCH_COUNTDOWN";
        const string SVRMSG_LAUNCH_ABORT = "SVRMSG_LAUNCH_ABORT";

        //A Collection of General Messages From The Clients
        const string CLNTMSG_CLIENT_SHUTDOWN_REASON = "CLNTMSG_CLIENT_SHUTDOWN_REASON";
        const string CLNTMSG_REQUEST_PLAYER_LIST = "CLNTMSG_REQUEST_PLAYER_LIST";
        const string CLNTMSG_UPDATE_PLAYER_STATUS = "CLNTMSG_UPDATE_PLAYER_STATUS";


        protected List<vxServerLobbyPlayerItem> List_Items = new List<vxServerLobbyPlayerItem>();


        bool ReadyState = false;

        //int playerCount = 0;

        #endregion

        #region Initialization

        string originalTitle = "";
        /// <summary>
        /// Constructor.
        /// </summary>
        public vxSeverLobbyDialog(string Title)
            : base(Title, vxEnumButtonTypes.OkCancel)
        {
            this.Title = "Lobby // " + Title;
            
            originalTitle = this.Title;
        }


        /// <summary>
        /// Sets up Local Server Dialog. It also sends out the subnet broadcast here searching for any available servers on this subnet.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();


            m_playersInLobbyScrollPanel = new vxScrollPanel(new Vector2(
                                    this.ArtProvider.GUIBounds.X + this.ArtProvider.GUIBounds.Width / 2,
                                    this.ArtProvider.GUIBounds.Y + OKButton.Bounds.Height),
                                this.ArtProvider.GUIBounds.Width / 2,
                                this.ArtProvider.GUIBounds.Height - OKButton.Bounds.Height * 2);



            //Setup Client Events
            vxNetworkManager.Client.Disconnected += NetworkSessionManager_Disconnected;
            vxNetworkManager.Client.OtherPlayerConnected += ClientManager_OtherPlayerConnected;
            vxNetworkManager.Client.OtherPlayerDisconnected += ClientManager_OtherPlayerDisconnected;
            InternalGUIManager.Add(m_playersInLobbyScrollPanel);




            //Set up the Server Code
            if (vxNetworkManager.Client.PlayerNetworkRole == vxEnumNetworkPlayerRole.Server)
            {
                Vector2 viewportSize = new Vector2(vxGraphics.GraphicsDevice.Viewport.Width, vxGraphics.GraphicsDevice.Viewport.Height);

                //Create The New Server Button
                m_launchServerButton = new vxButtonControl("Launch Game", new Vector2(viewportSize.X / 2 - 115, viewportSize.Y / 2 + 20));
                m_launchServerButton.Clicked += Btn_LaunchServer_Clicked; ;
                InternalGUIManager.Add(m_launchServerButton);


                //Now Start The Server
                vxNetworkManager.Server.Start();


                //Now Connect to it's self if it's the Server
                var approval = vxNetworkManager.Client.CreateMessage();
                approval.Write("secret");
                vxNetworkManager.Client.Connect(NetUtility.Resolve("localhost").ToString(),
                                                     vxNetworkManager.Server.Port, approval);

            }
            //The Server acts as a Client as well, so no need for an 'else if' block here.

         OKButton.Text = "Ready?";

        }


        protected virtual void NetworkSessionManager_Disconnected(object sender, vxNetClientEventDisconnected e)
        {
            //Console.WriteLine("Player Disconnected");
            Console.WriteLine(e.Reason);
            ExitScreen();
        }


        #region Client Events

        public virtual vxServerLobbyPlayerItem GetNewPlayerLobbyItem(vxNetPlayerInfo playerInfo)
        {
            Texture2D thumbnail = vxInternalAssets.Textures.Arrow_Right;

            return new vxServerLobbyPlayerItem(playerInfo,
                                               new Vector2(
                                                   ArtProvider.Padding.X,
                                                   ArtProvider.Padding.Y + (ArtProvider.Padding.Y / 10 + 68) * (List_Items.Count + 1)),
                                               thumbnail);
        }

        //This event fires if a new player is found in the updated list
        private void ClientManager_OtherPlayerConnected(object sender, vxNetClientEventPlayerConnected e)
        {
            
            //First Add a New Player in the Manager. The details will come in an update.
            
            vxServerLobbyPlayerItem item = GetNewPlayerLobbyItem(e.ConnectedPlayer);


            //Set Item Width
			item.Width = m_playersInLobbyScrollPanel.Width - (int)(2 * this.ArtProvider.Padding.X) - m_playersInLobbyScrollPanel.ScrollBarWidth;

            //Set Clicked Event
            item.Clicked += GetHighlitedItem;

            //Add item too the list
            List_Items.Add(item);

            m_playersInLobbyScrollPanel.Clear();

            foreach (vxServerLobbyPlayerItem it in List_Items)
                m_playersInLobbyScrollPanel.AddItem(it);

            m_playersInLobbyScrollPanel.UpdateItemPositions();
        }
        private void ClientManager_OtherPlayerDisconnected(object sender, vxNetClientEventPlayerDisconnected e)
        {
            Console.WriteLine(e.DisconnectedPlayer.UserName + " Left the Lobby");
            for (int i = 0; i < List_Items.Count; i++)
            {
                //if(List_Items[i].Player != null)
                if (List_Items[i].Player.ID == e.DisconnectedPlayer.ID)
                {
                    List_Items.RemoveAt(i);
                    //Now Decrement the index since the count just dropped one
                    i--;
                }
            }

            //Now Re-Introduce list
            m_playersInLobbyScrollPanel.Clear();

            foreach (vxServerLobbyPlayerItem it in List_Items)
                m_playersInLobbyScrollPanel.AddItem(it);

            m_playersInLobbyScrollPanel.UpdateItemPositions();
        }

        #endregion



        /// <summary>
        /// This method is called at the end of the countdown in the lobby to launch the session.
        /// </summary>
        public virtual void LaunchSession()
        {
            if (vxNetworkManager.Client.PlayerNetworkRole == vxEnumNetworkPlayerRole.Server)
            {
                //Turn off the Server
                vxNetworkManager.Server.IsAcceptingIncomingConnections = false;
            }

        }



        /// <summary>
        /// Launches the Server. This is only available to the player acting as the server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Btn_LaunchServer_Clicked(object sender, vxGuiItemClickEventArgs e)
        {
            LaunchSession();
        }

        public override void OnOKButtonClicked(object sender, vxGuiItemClickEventArgs e)
        {
            ReadyState = !ReadyState;

            if (ReadyState)
             OKButton.Text = "Ready!";
            else
             OKButton.Text = "Ready?";


            vxNetworkManager.Client.PlayerInfo.Status = ReadyState ? vxEnumNetPlayerStatus.InServerLobbyReady : vxEnumNetPlayerStatus.InServerLobbyNotReady;
            vxNetworkManager.Client.SendMessage(new vxNetmsgUpdatePlayerLobbyStatus(vxNetworkManager.Client.PlayerInfo));
        }

        /// <summary>
        /// Closes the Local Server Dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnCancelButtonClicked(object sender, vxGuiItemClickEventArgs e)
        {
            // Disconnect The Client, 
            vxNetworkManager.Client.Disconnect();
        }



        public override void UnloadContent()
        {
            base.UnloadContent();

            //Now untether handles
            vxNetworkManager.Client.OtherPlayerConnected -= ClientManager_OtherPlayerConnected;
            vxNetworkManager.Client.OtherPlayerDisconnected -= ClientManager_OtherPlayerDisconnected;
        }

        #endregion
        int loop = 0;

        float LaunchCountdown = 5.0f;

        bool HasLaunched = false;


        protected virtual int GetCountdown()
        {
            return 4;
        }


        protected virtual int GetMinPlayerCount()
        {
            return 1;
        }


        protected virtual int GetMaxPlayerCount()
        {
            return 4;
        }

        bool HasSentData = false;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="otherScreenHasFocus"></param>
        /// <param name="coveredByOtherScreen"></param>
        public override void Update()
        {
            base.Update();
            loop++;


            // Set Button Size and Position
            if (m_launchServerButton != null)
            {
                m_launchServerButton.Height = OKButton.Height;
                m_launchServerButton.Position = new Vector2(
                    ArtProvider.GUIBounds.Left, OKButton.Position.Y);
            }

            //Get the User List on the First Update
            if (loop > 25 && HasSentData == false)
            {
                HasSentData = true;
                //First, automatically send user data
                vxNetworkManager.Client.PlayerInfo.Status = vxEnumNetPlayerStatus.InServerLobbyNotReady;

                //Send message with User Data
                vxNetworkManager.Client.SendMessage(new vxNetmsgAddPlayer(vxNetworkManager.Client.PlayerInfo));
            }



            // Check if it has reached the Min Player Count to Launch
            if (vxNetworkManager.Client.PlayerManager.Players.Count >= GetMinPlayerCount() && LaunchCountdown > 1)
            {
                SessionStateForClient = SessionState.Countdown;
                // Launch Control
                /**********************************************************************************************/
                foreach (KeyValuePair<string, vxNetPlayerInfo> entry in vxNetworkManager.Client.PlayerManager.Players)
                {
                    if (entry.Value.Status != vxEnumNetPlayerStatus.InServerLobbyReady)
                    {
                        SessionStateForClient = SessionState.Idle;
                        break;
                    }
                }
            }



            // Handle Idle
            if (SessionStateForClient == SessionState.Idle)
            {
                LaunchCountdown = GetCountdown();
                this.Title = originalTitle;
            }


            // Handle If it's Counting Down
            else if (SessionStateForClient == SessionState.Countdown)
            {
                LaunchCountdown += -1 * vxTime.DeltaTime;

                if (LaunchCountdown < 1)
                    SessionStateForClient = SessionState.Launch;

                this.Title = string.Format("Launching in: {0}", (int)LaunchCountdown);
            }


            // Is it Ready To Launch?
            else if (SessionStateForClient == SessionState.Launch && HasLaunched == false)
            {
                HasLaunched = true;

                LaunchSession();
            }
        }

        #region Draw

        public void GetHighlitedItem(object sender, vxGuiItemClickEventArgs e)
        {
            foreach (vxServerLobbyPlayerItem fd in List_Items)
            {
                fd.UnSelect();
            }
            int i = e.GUIitem.Index;

            List_Items[i].ThisSelect();

            //SelectedServerIp = List_Items[i].ServerAddress;
        }


        #endregion
    }
}
