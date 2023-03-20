/**
 * @file
 * @brief This is a Server Dialog which searches and retrieves any game servers on this LAN. It also allows the Player to set up a local server as well.
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using VerticesEngine.ContentManagement;
using VerticesEngine.Net.Events;
using VerticesEngine.Net.Messages;
using VerticesEngine.UI;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Dialogs;
using VerticesEngine.UI.Events;

namespace VerticesEngine.Net.UI
{
    /// <summary>
    /// This is a Server Dialog which searches and retrieves any game servers on this LAN.
    /// It also allows the Player to set up a local server as well.
    /// </summary>
    public class vxSeverOnlineListDialog : vxDialogBase
    {
        #region Fields

        /// <summary>
        /// The vxListView GUI item which contains the list of all broadcasting servers on the subnet.
        /// </summary>
        protected vxScrollPanel m_scrollPanel;

        protected List<vxServerListItem> List_Items = new List<vxServerListItem>();


        int DefaultServerPort
        {
            get { return vxNetworkManager.Config.ServerLANDefaultPort; }
        }

        int PortRange
        {
            get { return vxNetworkManager.Config.ServerLANPortRange; }
        }

        /// <summary>
        /// Do we have a server list? We usually get this by pinging a master server
        /// </summary>
        protected bool IsServerListAvailable = false;

        /// <summary>
        /// Have we initialised yet?
        /// </summary>
        protected bool IsFullyInit = false;


        #endregion



        /// <summary>
        /// Constructor.
        /// </summary>
        public vxSeverOnlineListDialog(string Title)
            : base(Title, vxEnumButtonTypes.OkApplyCancel)
        {

        }

        

        /// <summary>
        /// Sets up Local Server Dialog. It also sends out the subnet broadcast here searching for any available servers on this subnet.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            m_scrollPanel = new vxScrollPanel(new Vector2(
                                    this.ArtProvider.GUIBounds.X,
                                    this.ArtProvider.GUIBounds.Y + OKButton.Bounds.Height),
                                this.ArtProvider.GUIBounds.Width,
                                this.ArtProvider.GUIBounds.Height - OKButton.Bounds.Height * 2);


            InternalGUIManager.Add(m_scrollPanel);

            //Vector2 viewportSize = new Vector2(vxGraphics.GraphicsDevice.Viewport.Width, vxGraphics.GraphicsDevice.Viewport.Height);
            Rectangle GUIBounds = ArtProvider.GUIBounds;

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
            vxConsole.WriteLine("Updating Server List...");

            // let's start the server request
            RequestAvailableServers();
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

            if(vxNetworkManager.Client.IsInitialised && IsFullyInit == false && IsServerListAvailable)
            {
                IsFullyInit = true;
                SendDiscoverySignal();

                OKButton.IsEnabled = false;

            }

            if(IsFullyInit)
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
                        item.Clicked += OnHighlitedItemClicked;

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



        private void OnHighlitedItemClicked(object sender, vxUIControlClickEventArgs e)
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
                vxConsole.WriteError($"Error Connecting to server {serverName} : {err}");
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

        /// <inheritdoc/>
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

        /// <summary>
        /// This is called at the very start to get the most up to date server list
        /// </summary>
        protected virtual void RequestAvailableServers()
        {
            throw new Exception("Server Request is not made, we'll never get the most up to date servers");
        }

        protected virtual List<string> GetServerList()
        {
            throw new Exception("Server List not provided");
        }

        void SendDiscoverySignal()
        {
            List_Items.Clear();
            m_scrollPanel.Clear();
            recievedServerQueue.Clear();


            List<string> ips = GetServerList();

            for (int s = 0; s < ips.Count; s++)
            {
                vxCoroutineManager.Instance.StartCoroutine(GetServersAsync(ips[s]));
            }
        }


        System.Collections.IEnumerator GetServersAsync(string host)
        {
            int port = DefaultServerPort;

            yield return null;

                while (port < DefaultServerPort + PortRange)
                {
                    //vxDebug.LogNet("Searching " + host + ":" + port);
                    vxNetworkManager.Client.SendDiscoverySignal(host, port);
                    port++;
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
