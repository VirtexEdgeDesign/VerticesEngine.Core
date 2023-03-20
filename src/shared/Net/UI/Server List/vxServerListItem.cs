
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Utilities;
using VerticesEngine;
using VerticesEngine.UI.Events;
using VerticesEngine.UI.Themes;
using VerticesEngine.Net.Messages;
using VerticesEngine.Graphics;
using VerticesEngine.UI.Dialogs;
using VerticesEngine.UI;
using System.Net;
using System.Net.NetworkInformation;
using System.ComponentModel;
using System.Threading;

namespace VerticesEngine.Net.UI
{
    /// <summary>
    /// File Chooser Dialor Item.
    /// </summary>
    public class vxServerListItem : vxScrollPanelItem
    {
        /// <summary>
        /// The name of the Server.
        /// </summary>
        public string ServerName
        {
            get { return serverInfo.ServerName; }
        }

        /// <summary>
        /// The Server Addess
        /// </summary>
        public string ServerAddress
        {
            get { return serverInfo.ServerIP; }
        }

        /// <summary>
        /// The Server Port
        /// </summary>
        public string ServerPort
        {
            get { return serverInfo.ServerPort; }
        }



        vxNetMsgServerInfo serverInfo;


        float currentPingWait = 0;

        float pingInterval = 1.0f;

        float pingSum = 0;

        int pingCount = 0;

        /// <summary>
        /// Are we pinging the hosts at an interval or should we only do this once at init
        /// </summary>
        bool isPingAveragedAndRepeated = false;

        Color iconColour = Color.LightGray;

        /// <summary>
        /// Sets up a Serve List Dialog Item which holds information pertaining too a Discovered Server.
        /// </summary>
        /// <param name="serverInfo"></param>
        /// <param name="Position"></param>
        /// <param name="buttonImage"></param>
        public vxServerListItem(vxNetMsgServerInfo serverInfo, Vector2 Position, Texture2D buttonImage):
            base(serverInfo.ServerName, Position, buttonImage)
        {
            this.serverInfo = serverInfo;

            // Set Text
            Text = ServerName;

            Font = vxUITheme.Fonts.Size12;

            Theme = new vxUIControlTheme(
                new vxColourTheme(new Color(0.15f, 0.15f, 0.15f, 0.5f), Color.DarkOrange, Color.DeepPink),
                new vxColourTheme(Color.LightGray, Color.Black, Color.Black));

            PingHost(serverInfo.ServerIP);

            Height = 48;

            if (serverInfo.ServerName.Contains("metric-na-west"))
                iconColour = Color.DarkOrange;
            if (serverInfo.ServerName.Contains("metric-na-east"))
                iconColour = Color.LimeGreen;
            if (serverInfo.ServerName.Contains("metric-euro"))
                iconColour = Color.DeepSkyBlue;
            if (serverInfo.ServerName.Contains("metric-asia"))
                iconColour = Color.DeepPink;

            clientVersion = new Version(vxEngine.Game.Version);
        }

        protected internal override void Update()
        {
            base.Update();

            if (isPingAveragedAndRepeated)
            {
                currentPingWait += vxTime.DeltaTime;

                if (currentPingWait > pingInterval)
                {
                    PingHost(serverInfo.ServerIP);
                    currentPingWait -= pingInterval;
                }
            }
        }

        static int middleColnPos = 0;

        Version clientVersion;

        public override void Draw()
        {
            //base.Draw();

            Theme.SetState(this);

            //Draw Button Background
            vxGraphics.SpriteBatch.Draw(DefaultTexture, Bounds, Color.Black);
            vxGraphics.SpriteBatch.Draw(DefaultTexture, Bounds.GetBorder(-1), GetStateColour(Theme.Background));


            //Draw Icon
            int iconOffset = 0;
            if (ButtonImage != null)
            {
                vxGraphics.SpriteBatch.Draw(ButtonImage, new Rectangle(
                    (int)(Position.X + Padding.X),
                    (int)(Position.Y + Padding.Y),
                    (int)(Height - Padding.X * 2),
                    (int)(Height - Padding.Y * 2)),
                                        iconColour);

                iconOffset = Height;
            }


            //Draw Text String
            var titleFont = vxUITheme.Fonts.Size16;
            middleColnPos = (int)(Math.Max(titleFont.MeasureString(Text).X, middleColnPos));
            vxGraphics.SpriteBatch.DrawString(titleFont, Text,
                                          new Vector2(
                                              (int)(Position.X + iconOffset + Padding.X * 2),
                                              (int)(Bounds.Top + Padding.Y)),
                                          GetStateColour(Theme.Text));



            //Console.WriteLine(ServerAddress +":" + ServerPort+":"+ ToggleState);

            string serverState = "unknown";

            switch (serverInfo.SessionState)
            {
                case vxEnumNetSessionState.InLobby:
                    serverState = "In Lobby";
                    break;
                case vxEnumNetSessionState.LoadingNextLevel:
                    serverState = "Starting Race";
                    break;
                case vxEnumNetSessionState.PlayingGame:
                    serverState = "Racing";
                    break;
                case vxEnumNetSessionState.PostGame:
                    serverState = "Finishing Race";
                    break;
            }

            bool isVersionAllowed = clientVersion >= serverInfo.ServerVersion;

            this.IsEnabled = (serverInfo.SessionState == vxEnumNetSessionState.InLobby && isVersionAllowed);

            if(isVersionAllowed == false)
            {
                serverState = "Please Update Client To Play";
            }


            vxGraphics.SpriteBatch.DrawString(Font, serverState,
                                          new Vector2(
                                              (int)(Position.X + iconOffset + Padding.X * 10 + middleColnPos),
                                              (int)(Bounds.Top + Padding.Y)),
                                          Theme.Text.Color);

            if (isVersionAllowed)
            {
                string players = string.Format("Racers: {0} / {1}", serverInfo.NumberOfPlayers, serverInfo.MaxNumberOfPlayers);
                vxGraphics.SpriteBatch.DrawString(Font, players,
                                              new Vector2(
                                                  Position.X + Bounds.Width * 2.0f / 3.0f - Padding.X * 8,
                                                  (int)(Bounds.Top + Padding.Y)),
                                              Theme.Text.Color);

                string pingText = string.Format("Ping: ---");
                if (pingCount > 0)
                {
                    float pingValue = (pingSum / pingCount);

                    if (pingValue < 1)
                        pingText = string.Format("Ping: <1ms");
                    else
                        pingText = string.Format("Ping: {0}ms", pingValue.ToString("#,##0"));
                }
            vxGraphics.SpriteBatch.DrawString(Font, pingText,
                                          new Vector2(
                                              (int)(Bounds.Right - Font.MeasureString(pingText).X - Padding.X * 2),
                                              (int)(Bounds.Top + Padding.Y)),
                                          Theme.Text.Color);

            }

            var debugFont = vxInternalAssets.Fonts.ViewerFont;

            string infoText = "v." + serverInfo.ServerVersion + " | ";

            if (serverInfo.IsDedicated)
                infoText += "Dedicated Server";
#if DEBUG
            infoText += " | Address: " + ServerAddress + ":" + ServerPort;
#endif


            vxGraphics.SpriteBatch.DrawString(debugFont, infoText,
                                          new Vector2(
                                              (int)(Position.X + iconOffset + Padding.X * 2),
                                              (int)(Bounds.Bottom - debugFont.LineSpacing - Padding.Y)),
                                          Theme.Text.Color * 0.5f);
        }


        private void PingHost(string who)
        {
            try
            {
                //Console.WriteLine(">>>>>Pinging Host " + who);
                // we'll update our own ping here string who = args[0];
                AutoResetEvent waiter = new AutoResetEvent(false);

                Ping pingSender = new Ping();

                // When the PingCompleted event is raised,
                // the PingCompletedCallback method is called.
                pingSender.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);

                // Create a buffer of 32 bytes of data to be transmitted.
                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] buffer = System.Text.Encoding.ASCII.GetBytes(data);

                // Wait 12 seconds for a reply.
                int timeout = 12000;

                // Set options for transmission:
                // The data can go through 64 gateways or routers
                // before it is destroyed, and the data packet
                // cannot be fragmented.
                PingOptions options = new PingOptions(64, true);

                //Console.WriteLine("Time to live: {0}", options.Ttl);
                //Console.WriteLine("Don't fragment: {0}", options.DontFragment);

                // Send the ping asynchronously.
                // Use the waiter as the user token.
                // When the callback completes, it can wake up this thread.
                pingSender.SendAsync(who, timeout, buffer, options, waiter);

                // Prevent this example application from ending.
                // A real application should do something useful
                // when possible.
                //waiter.WaitOne();
                //Console.WriteLine("Ping example completed.");
            }
            catch (Exception ex)
            {
                vxConsole.WriteException(ex);
            }
        }
        private void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {
            try
            {
                // If the operation was canceled, display a message to the user.
                if (e.Cancelled)
                {
                    vxConsole.WriteError("Ping canceled.");

                    // Let the main thread resume.
                    // UserToken is the AutoResetEvent object that the main thread
                    // is waiting for.
                    ((AutoResetEvent)e.UserState).Set();
                }

                // If an error occurred, display the exception to the user.
                if (e.Error != null)
                {
                    vxConsole.WriteError("Ping failed:");
                    vxConsole.WriteError(e.Error.ToString());

                    // Let the main thread resume.
                    ((AutoResetEvent)e.UserState).Set();
                }

                PingReply reply = e.Reply;

                DisplayReply(reply);

                // Let the main thread resume.
                ((AutoResetEvent)e.UserState).Set();
            }
            catch(Exception ex)
            {
                vxConsole.WriteException(ex);
            }
        }

        private void DisplayReply(PingReply reply)
        {
            if (reply == null)
                return;

            //Console.WriteLine("ping status: {0}", reply.Status);
            if (reply.Status == IPStatus.Success)
            {
                //Console.WriteLine("Address: {0}", reply.Address.ToString());
                //Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
                ////Console.WriteLine("Time to live: {0}", reply.Options.Ttl);
                ////Console.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
                //Console.WriteLine("Buffer size: {0}", reply.Buffer.Length);

                pingSum = reply.RoundtripTime;
                pingCount++;
            }
        }
    }
}
