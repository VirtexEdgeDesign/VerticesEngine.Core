using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using VerticesEngine.Graphics;
using VerticesEngine.Input;
using VerticesEngine.Net.Events;
using VerticesEngine.Net.Messages;
using VerticesEngine.UI;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Dialogs;
using VerticesEngine.UI.Events;
using VerticesEngine.UI.MessageBoxs;

namespace VerticesEngine.Net.UI
{
    /// <summary>
    /// What is the net lobby state? Are we idle accepting connections? Are we counting down to launch or are we launching?
    /// </summary>
    enum vxNetLobbyState
    {
        /// <summary>
        /// We're idle in the lobby currently
        /// </summary>
        Idle,

        /// <summary>
        /// We are counting down to launch
        /// </summary>
        Countdown,

        /// <summary>
        /// The game is launchign
        /// </summary>
        Launching
    }

    /// <summary>
    /// This is a Server Lobby Dialog is the 'waiting room' before a session launch.
    /// </summary>
    public class vxServerLobbyDialog : vxDialogBase
    {
        #region Fields

        /// <summary>
        /// If the player is acting as Server, then let the player launch the session.
        /// </summary>
        private vxButtonControl m_launchServerButton;

        /// <summary>
        /// The vxListView GUI item which contains the list of all broadcasting servers on the subnet.
        /// </summary>
        private vxScrollPanel m_playersInLobbyScrollPanel;

        private vxNetLobbyState m_sessionStateForClient = vxNetLobbyState.Idle;


        protected List<vxServerLobbyPlayerItem> List_Items = new List<vxServerLobbyPlayerItem>();

        bool IsPlayerReady = false;

        #endregion

        #region Initialization

        string originalTitle = "";

        protected readonly string ServerName = "";

        int loop = 0;

        float LaunchCountdown = 5.0f;

        bool HasLaunched = false;


        protected virtual int GetCountdown()
        {
#if DEBUG
            return 2;
#else
            return 4;
#endif
        }


        protected virtual int GetMinPlayerCount()
        {
            return 2;
        }


        protected virtual int GetMaxPlayerCount()
        {
            return 4;
        }

        bool HasSentData = false;

        protected bool IsMidGameLobby = false;
        /// <summary>
        /// Constructor.
        /// </summary>
        public vxServerLobbyDialog(string Title, bool IsMidGameLobby=false)
            : base(Title, vxEnumButtonTypes.OkCancel)
        {
            this.Title = "Lobby // " + Title;

            originalTitle = this.Title;
            this.IsMidGameLobby = IsMidGameLobby;
            ServerName = Title;
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
            vxNetworkManager.Client.PlayerListUpdated += Client_PlayerListUpdated;
            vxNetworkManager.Client.NetSessionStatusChanged += Client_NetSessionStatusChanged;
            vxNetworkManager.Client.Disconnected += Client_NetworkSessionManager_Disconnected;
            vxNetworkManager.Client.OtherPlayerConnected += ClientManager_OtherPlayerConnected;
            vxNetworkManager.Client.OtherPlayerDisconnected += ClientManager_OtherPlayerDisconnected;
            vxNetworkManager.Client.UpdatedPlayerMetaDataRecieved += Client_UpdatedPlayerMetaDataRecieved;

            InternalGUIManager.Add(m_playersInLobbyScrollPanel);

            //Set up the Server Code
            if (vxNetworkManager.Client.PlayerNetworkRole == vxEnumNetworkPlayerRole.Server)
            {
                Vector2 viewportSize = new Vector2(vxGraphics.GraphicsDevice.Viewport.Width, vxGraphics.GraphicsDevice.Viewport.Height);

                //Create The New Server Button
                m_launchServerButton = new vxButtonControl("Launch Game", new Vector2(viewportSize.X / 2 - 115, viewportSize.Y / 2 + 20));
                m_launchServerButton.Clicked += Btn_LaunchServer_Clicked;
                InternalGUIManager.Add(m_launchServerButton);
            }

            //The Server acts as a Client as well, so no need for an 'else if' block here.
            OKButton.Text = "Ready?";
            OKButton.IsEnabled = false;
            IsPlayerReady = false;
            vxNetworkManager.Client.PlayerState.Status = vxEnumNetPlayerStatus.InServerLobbyNotReady;

            vxInput.IsCursorVisible = true;
        }


        public override void UnloadContent()
        {
            base.UnloadContent();

            //Now untether handles
            vxNetworkManager.Client.PlayerListUpdated -= Client_PlayerListUpdated;
            vxNetworkManager.Client.NetSessionStatusChanged -= Client_NetSessionStatusChanged;
            vxNetworkManager.Client.Disconnected -= Client_NetworkSessionManager_Disconnected;
            vxNetworkManager.Client.OtherPlayerConnected -= ClientManager_OtherPlayerConnected;
            vxNetworkManager.Client.OtherPlayerDisconnected -= ClientManager_OtherPlayerDisconnected;
            vxNetworkManager.Client.UpdatedPlayerMetaDataRecieved -= Client_UpdatedPlayerMetaDataRecieved;
        }

        protected internal override void Update()
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
                OnEnterLobby();
            }

#if DEBUG // Debugging let's just get on with it
            if (loop == 35000)
            {
                OnOKButtonClicked(null, null);
            }
#endif

            // Check if it has reached the Min Player Count to Launch
            if (vxNetworkManager.Client.PlayerManager.Players.Count >= GetMinPlayerCount() && LaunchCountdown > 1)
            {
                m_sessionStateForClient = vxNetLobbyState.Countdown;
                // Launch Control
                /**********************************************************************************************/
                foreach (KeyValuePair<string, vxNetPlayerInfo> entry in vxNetworkManager.Client.PlayerManager.Players)
                {
                    if (entry.Value.Status != vxEnumNetPlayerStatus.InServerLobbyReady)
                    {
                        m_sessionStateForClient = vxNetLobbyState.Idle;
                        break;
                    }
                }
            }



            // Handle Idle
            if (m_sessionStateForClient == vxNetLobbyState.Idle)
            {
                LaunchCountdown = GetCountdown();
                this.Title = originalTitle;
            }


            // Handle If it's Counting Down
            else if (m_sessionStateForClient == vxNetLobbyState.Countdown)
            {
                LaunchCountdown += -1 * vxTime.DeltaTime;

                if (LaunchCountdown < 1)
                {
                    LaunchSession();
                }

                
                this.Title = string.Format("Launching in: {0}", MathHelper.Max((int)LaunchCountdown, 0));
            }

            if(playersToAddQueue.Count > 0)
            {
                var player = playersToAddQueue.Dequeue();
                OnPlayerJoinsLobby(player);
            }

            if(IsPlayerListUpdate)
            {
                OnPlayerListUpdated();
                IsPlayerListUpdate = false;
            }
        }

        #region UI Methods



        public void GetHighlitedItem(object sender, vxUIControlClickEventArgs e)
        {
            foreach (vxServerLobbyPlayerItem fd in List_Items)
            {
                fd.UnSelect();
            }
            int i = e.GUIitem.Index;

            List_Items[i].ThisSelect();
        }


        public virtual vxServerLobbyPlayerItem GetNewPlayerLobbyItem(vxNetPlayerInfo playerInfo)
        {
            return new vxServerLobbyPlayerItem(playerInfo,
                new Vector2(ArtProvider.Padding.X,ArtProvider.Padding.Y + (ArtProvider.Padding.Y / 10 + 68) * (List_Items.Count + 1)),
                vxInternalAssets.Textures.Arrow_Right);
        }



        /// <summary>
        /// Launches the Server. This is only available to the player acting as the server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Btn_LaunchServer_Clicked(object sender, vxUIControlClickEventArgs e)
        {
            LaunchSession();
        }

        /// <inheritdoc/>
        protected override void OnOKButtonClicked(object sender, vxUIControlClickEventArgs e)
        {
            IsPlayerReady = !IsPlayerReady;

            if (IsPlayerReady)
                OKButton.Text = "Ready!";
            else
                OKButton.Text = "Ready?";

            vxNetworkManager.Client.PlayerState.Status = IsPlayerReady ? vxEnumNetPlayerStatus.InServerLobbyReady : vxEnumNetPlayerStatus.InServerLobbyNotReady;
            vxNetworkManager.Client.SendMessage(new vxNetmsgUpdatePlayerLobbyStatus(vxNetworkManager.Client.PlayerState));
        }

        /// <summary>
        /// Closes the Local Server Dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnCancelButtonClicked(object sender, vxUIControlClickEventArgs e)
        {
            var bx = vxMessageBox.Show("Leave Lobby?", "Are you sure you want to leave this session?", vxEnumButtonTypes.OkCancel);

            bx.Accepted += delegate
            {
                IsDisconnectMsgBoxSuppressed = true;
                // Disconnect The Client, 
                vxNetworkManager.Client.Disconnect();

                

                // if this is a mid-game lobby then we should return to the main menu
                if(IsMidGameLobby)
                {
                    vxSceneManager.GoToMainMenu();
                }
            };
        }

        #endregion




        /// <summary>
        /// This is the thread safe call for when a player is added to the lobby. We use a producer-consumer queue for when
        /// new player calls come from the server
        /// </summary>
        /// <param name="player"></param>
        protected virtual void OnPlayerJoinsLobby(vxNetPlayerInfo player) { }

        /// <summary>
        /// Called when a player leaves the lobby
        /// </summary>
        protected virtual void OnPlayerLeavesLobby(vxNetPlayerInfo player) { }

        protected virtual void OnPlayerListUpdated()
        {
            IsPlayerListUpdate = false;
            // loop through each player in the list
            List_Items.Clear();
            var keys = vxNetworkManager.Client.PlayerManager.Players.Keys.ToArray();

            lock (List_Items)
            {
                foreach (var key in keys)
                {
                    //   foreach (var player in vxNetworkManager.Client.PlayerManager.Players)
                    //foreach(var key in keys)
                    {
                        var player = vxNetworkManager.Client.PlayerManager.Players[key];

                        //var player = vxNetworkManager.Client.PlayerManager.Players[key];
                        //First Add a New Player in the Manager. The details will come in an update.
                        vxServerLobbyPlayerItem item = GetNewPlayerLobbyItem(player);

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
                }
            }
        }


        #region Net Client Events

        /// <summary>
        /// Has the player list been updated? If so then we'll update it on the next update call
        /// </summary>
        protected bool IsPlayerListUpdate = false;

        Queue<vxNetPlayerInfo> playersToAddQueue = new Queue<vxNetPlayerInfo>();
        Queue<vxNetPlayerInfo> playersToRemoveQueue = new Queue<vxNetPlayerInfo>();


        private void Client_NetSessionStatusChanged(object sender, vxNetClientEventSessionStatusUpdated e)
        {
            if (e.NewSessionState == vxEnumNetSessionState.LoadingNextLevel)
            {
                // Is it Ready To Launch?
                if (HasLaunched == false)
                {
                    HasLaunched = true;
#if DEBUG // If we're debugging and have netdev in our cmd line args, then lets wait so we don't cause both local clients to read the level file at the same time
                    if(vxEngine.Game.IsCmdArgPassed("-netdev"))
                    {
                        System.Threading.Thread.Sleep(500);
                    }
#endif
                    OnLaunchSession();
                }
            }
        }

        protected virtual void Client_NetworkSessionManager_Disconnected(object sender, vxNetClientEventDisconnected e)
        {
            vxConsole.NetLog("Reason: " + e.Reason);
            if (!vxNetworkManager.IsHost && IsDisconnectMsgBoxSuppressed==false)
            {
                vxMessageBox.Show("Server Connection Lost", "Connection to the Server has been lost.");
            }
            ExitScreen();
        }

        /// <summary>
        /// Are we disconnecting for our own reasons? then don't prompt the player
        /// </summary>
        private bool IsDisconnectMsgBoxSuppressed = false;

        private void Client_PlayerListUpdated()
        {
            IsPlayerListUpdate = true;
        }

        protected virtual void Client_UpdatedPlayerMetaDataRecieved(vxNetmsgPlayerMetaData playerMetaData) { }

        //This event fires if a new player is found in the updated list
        private void ClientManager_OtherPlayerConnected(object sender, vxNetClientEventPlayerConnected e)
        {
            playersToAddQueue.Enqueue(e.ConnectedPlayer);
        }



        private void ClientManager_OtherPlayerDisconnected(object sender, vxNetClientEventPlayerDisconnected e)
        {
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

            OnPlayerLeavesLobby(e.DisconnectedPlayer);
        }

        #endregion





        /// <summary>
        /// This method is called at the end of the countdown in the lobby to launch the session.
        /// </summary>
        protected void LaunchSession()
        {
            m_sessionStateForClient = vxNetLobbyState.Launching;

            // if we're the host then let's turn off the incoming connections
            if (vxNetworkManager.IsHost)
            {
                vxNetworkManager.Client.SendMessage(new vxNetmsgUpdateSessionState(vxEnumNetSessionState.LoadingNextLevel));
            }
        }
        protected virtual void OnEnterLobby()
        {
            //First, automatically send user data
            vxNetworkManager.Client.PlayerState.Status = vxEnumNetPlayerStatus.InServerLobbyNotReady;

            //Send message with User Data
            vxNetworkManager.Client.SendMessage(new vxNetmsgAddPlayer(vxNetworkManager.Client.PlayerState));
            OKButton.IsEnabled = true;

        }

        /// <summary>
        /// On Launch is called when the 'Playing Game' Session State is set
        /// </summary>
        protected virtual void OnLaunchSession() { }


        #endregion


    }
}
