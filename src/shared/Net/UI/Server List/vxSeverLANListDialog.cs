using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using VerticesEngine.ContentManagement;
using VerticesEngine.Net.Events;
using VerticesEngine.Net.Messages;
using VerticesEngine.Profile;
using VerticesEngine.UI;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Dialogs;
using VerticesEngine.UI.Events;
using VerticesEngine.UI.MessageBoxs;
using VerticesEngine.UI.Themes;

namespace VerticesEngine.Net.UI
{
    /// <summary>
    /// This is a Server Dialog which searches and retrieves any game servers on this LAN.
    /// It also allows the Player to set up a local server as well.
    /// </summary>
    public class vxSeverLANListDialog : vxDialogBase
    {
        #region Fields

        /// <summary>
        /// The vxListView GUI item which contains the list of all broadcasting servers on the subnet.
        /// </summary>
        protected vxScrollPanel m_scrollPanel;

        /// <summary>
        /// Let's the player create a new local server.
        /// </summary>
        protected vxButtonControl m_createNewLocalServerButton;


        protected List<vxServerListItem> List_Items = new List<vxServerListItem>();


        int DefaultServerPort
        {
            get { return vxNetworkManager.Config.ServerLANDefaultPort; }
        }

        int PortRange
        {
            get { return vxNetworkManager.Config.ServerLANPortRange; }
        }



        #endregion



        /// <summary>
        /// Constructor.
        /// </summary>
        public vxSeverLANListDialog(string Title)
            : base(Title, vxEnumButtonTypes.OkApplyCancel)
        {

        }

        

        /// <summary>
        /// Sets up Local Server Dialog. It also sends out the subnet broadcast here searching for any available servers on this subnet.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            // half screen size
            //m_scrollPanel = new vxScrollPanel(new Vector2(
            //                        this.ArtProvider.GUIBounds.X + this.ArtProvider.GUIBounds.Width / 2,
            //                        this.ArtProvider.GUIBounds.Y + OKButton.Bounds.Height),
            //                    this.ArtProvider.GUIBounds.Width/2,
            //                    this.ArtProvider.GUIBounds.Height - OKButton.Bounds.Height * 2);

            m_scrollPanel = new vxScrollPanel(new Vector2(
                                    this.ArtProvider.GUIBounds.X,
                                    this.ArtProvider.GUIBounds.Y + OKButton.Bounds.Height),
                                this.ArtProvider.GUIBounds.Width,
                                this.ArtProvider.GUIBounds.Height - OKButton.Bounds.Height * 2);


            InternalGUIManager.Add(m_scrollPanel);

            //Vector2 viewportSize = new Vector2(vxGraphics.GraphicsDevice.Viewport.Width, vxGraphics.GraphicsDevice.Viewport.Height);
            Rectangle GUIBounds = ArtProvider.GUIBounds;

            //Create The New Server Button
            m_createNewLocalServerButton = new vxButtonControl("Create LAN Server", 
                                                           new Vector2(GUIBounds.X / 2 - 115, GUIBounds.Y / 2 + 20));
            
            //Set the Button's Position relative too the background rectangle.
            m_createNewLocalServerButton.Position = new Vector2(
				this.ArtProvider.GUIBounds.X, 
				this.ArtProvider.GUIBounds.Y) + new Vector2(
                vxEngine.Instance.GUITheme.Padding.X * 2,
					this.ArtProvider.GUIBounds.Height - vxUITheme.ArtProviderForButtons.DefaultHeight - vxEngine.Instance.GUITheme.Padding.Y * 2);

            m_createNewLocalServerButton.Clicked += Btn_CreateNewLocalServer_Clicked;
            InternalGUIManager.Add(m_createNewLocalServerButton);


            //The Cancel Button is Naturally the 'Back' button
            CancelButton.Clicked += new EventHandler<vxUIControlClickEventArgs>(OnCancelButtonClicked);
            ApplyButton.Text = "Refresh";


            
            //Initialise the network client
            vxNetworkManager.Client.Initialise(vxNetworkBackend.CrossPlatform);

            //Now setup the Event Handlers
            vxNetworkManager.Client.DiscoverySignalResponseRecieved += ClientManager_DiscoverySignalResponseRecieved;

            //By Default, The Game will start looking for other networked games as a client.
            vxNetworkManager.Client.SetPlayerNetworkRole(vxEnumNetworkPlayerRole.Client);
            
            //Finally at the end, send out a pulse of discovery signals 
            vxConsole.WriteLine("Sending Discovery Signal...");

        }

        public override void UnloadContent()
        {
            //Now Deactivate all Event Handlers
            vxNetworkManager.Client.DiscoverySignalResponseRecieved -= ClientManager_DiscoverySignalResponseRecieved;

            base.UnloadContent();
        }




        #region Handle GUI Events


        protected internal override void Update()
        {
            base.Update();

            m_createNewLocalServerButton.Height = OKButton.Height;
            m_createNewLocalServerButton.Position = new Vector2(
                ArtProvider.GUIBounds.Right - m_createNewLocalServerButton.Width, 
                ArtProvider.GUIBounds.Top);

            if(vxNetworkManager.Client.IsInitialised && isInit == false)
            {
                isInit = true;
                SendDiscoverySignal();

                OKButton.IsEnabled = false;
#if DEBUG
                if (vxEngine.Game.IsCmdArgPassed("-netdev") && isServerCreated == false)
                {
                    isServerCreated = true;
                    //CreateServer("Ratchet's Server");
                }
#endif
            }

            if(isInit)
            {
                // check if we've recieved any servers
                if(recievedServerQueue.Count > 0)
                {
                    var response = recievedServerQueue.Dequeue();
                    // check that it's valid
                    if (!string.IsNullOrEmpty(response.AppName) && !string.IsNullOrEmpty(response.ServerIP))
                    {
                        var item = OnCreateServerListItem(response);

                        //Set Item Width
                        item.Width = m_scrollPanel.Width - (int)(2 * this.ArtProvider.Padding.X) - m_scrollPanel.ScrollBarWidth;

                        //Set Clicked Event
                        item.Clicked += GetHighlitedItem;

                        //Add item too the list
                        List_Items.Add(item);

                        m_scrollPanel.Clear();

                        foreach (vxServerListItem it in List_Items)
                            m_scrollPanel.AddItem(it);

                        m_scrollPanel.UpdateItemPositions();
                    }
                }
            }
        }
#if DEBUG
        static bool isServerCreated = false;
#endif
        public bool isInit = false;

        public void GetHighlitedItem(object sender, vxUIControlClickEventArgs e)
        {
            foreach (var it in m_scrollPanel.Items)
                it.ToggleState = false;

            e.GUIitem.ToggleState = true;
            currentlySelected = (vxServerListItem)e.GUIitem;
            OKButton.IsEnabled = true;
        }
        vxServerListItem currentlySelected;


        /// <inheritdoc/>
        protected override void OnOKButtonClicked(object sender, vxUIControlClickEventArgs e)
        {
            var serverName = currentlySelected.ServerName;
            var ip = currentlySelected.ServerAddress;
            var port = Convert.ToInt32(currentlySelected.ServerPort);

            //Connect to the Selected Server. This opens an async busy dialog which will loop and have a 'on connected' callback
            var connectDialog = new vxMultiplayerLANConnectBusyDialog(serverName, ip, port);
            connectDialog.OnServerJoined += ()=>
            {
                //Now Add go to the Server Lobby. The Lobby info will be added in by the global Client Connection Object.
                OnOpenServerLobby(currentlySelected.ServerName, currentlySelected.ServerAddress, currentlySelected.ServerPort);
                ExitScreen();
            };

            connectDialog.OnError += (ConnectionErrorIssue err) =>
            {

            };
            vxSceneManager.AddScene(connectDialog, PlayerIndex.One);

        }

        /// <summary>
        /// Closes the Local Server Dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnCancelButtonClicked(object sender, vxUIControlClickEventArgs e)
        {
            vxNetworkManager.Client.Disconnect();

            base.OnCancelButtonClicked(sender, e);
        }

        protected override void OnApplyButtonClicked(object sender, vxUIControlClickEventArgs e)
        {
            SendDiscoverySignal();
        }

        #endregion


        private void ClientManager_DiscoverySignalResponseRecieved(object sender, vxNetClientEventDiscoverySignalResponse e)
        {
            OnDiscoveredServer(e.NetMsgServerInfo);
        }


        /// <summary>
        /// The event Fired when the user wants to create their own Local Server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Btn_CreateNewLocalServer_Clicked(object sender, vxUIControlClickEventArgs e)
        {
            vxMessageInputBox.Show("Create Server", "Create a Unique Name for your server...", 
                $"{vxPlatform.Player.Name} Game", (inputText)=>
                {
                    CreateServer(inputText);
                });
        }

        protected void CreateServer(string serverName)
        {
            var createServerDialog = new vxMultiplayerLANCreateServerBusyDialog(serverName);

            createServerDialog.OnServerCreated += (name, ip, port) =>
            {
                var connectDialog = new vxMultiplayerLANConnectBusyDialog(serverName, ip, port);
                connectDialog.OnServerJoined += () =>
                {
                    //Now Add go to the Server Lobby. The Lobby info will be added in by the global Client Connection Object.
                    OnOpenServerLobby(serverName, ip, port.ToString());
                    ExitScreen();
                };

                connectDialog.OnError += (ConnectionErrorIssue err) =>
                {

                };
                vxSceneManager.AddScene(connectDialog);
            };
            vxSceneManager.AddScene(createServerDialog);
        }


        /// <summary>
        /// This Method is Called to Open the Server Lobby. If your game uses an inherited version of vxSeverLobbyDialog then
        /// you should override this function.
        /// </summary>
        protected virtual void OnOpenServerLobby(string servername, string ip, string port)
        {
            var lobby = new vxServerLobbyDialog("Lobby");
            lobby.IsCustomButtonPosition = IsCustomButtonPosition;
            vxSceneManager.AddScene(lobby, PlayerIndex.One);
        }

        /// <summary>
        /// Called when the server list item is created.
        /// </summary>
        /// <returns></returns>
        protected virtual vxServerListItem OnCreateServerListItem(vxNetMsgServerInfo response)
        {
            Texture2D thumbnail = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/gui/net/network_hub");
            vxServerListItem item = new vxServerListItem(response,
                new Vector2(ArtProvider.Padding.X, ArtProvider.Padding.Y + (ArtProvider.Padding.Y / 10 + 68) * (List_Items.Count + 1)),
        thumbnail);
            return item;
        }


        #region Client Networking Code

        /// <summary>
        /// A Queue of recieved servers which allows the main UI to update one per frame
        /// </summary>
        private Queue<vxNetMsgServerInfo> recievedServerQueue = new Queue<vxNetMsgServerInfo>();


        void SendDiscoverySignal()
        {
            vxCoroutineManager.Instance.StartCoroutine(GetServersAsync());
        }

        System.Collections.IEnumerator GetServersAsync()
        {
            List_Items.Clear();
            m_scrollPanel.Clear();
            recievedServerQueue.Clear();

            int port = DefaultServerPort;

            yield return null;

            while (port < DefaultServerPort + PortRange)
            {
                vxNetworkManager.Client.SendLocalDiscoverySignal(port);
                port++;
                Console.WriteLine(port);
                yield return null;
            }

            yield return null;
        }

        protected virtual void OnDiscoveredServer(vxNetMsgServerInfo response)
        {
            recievedServerQueue.Enqueue(response);
        }

        #endregion
    }
}
