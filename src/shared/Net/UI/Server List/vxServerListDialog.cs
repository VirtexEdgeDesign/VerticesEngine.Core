using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using VerticesEngine.Input;
using VerticesEngine.Net.Events;
using VerticesEngine.UI;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Dialogs;
using VerticesEngine.UI.Events;



namespace VerticesEngine.Net.UI
{
    // TODO: Update to be the abstract base class for the LAN and Online server lists

    /// <summary>
    /// This Dislog Displays all active server's on the connected master server.
    /// </summary>
    public class vxServerListDialog : vxDialogBase
    {
        #region Fields

        vxListView ScrollPanel;

        private System.ComponentModel.BackgroundWorker BckgrndWrkr_FileOpen;

       // string FileExtentionFilter;

        int CurrentlySelected = -1;

		List<vxFileDialogItem> List_Items = new List<vxFileDialogItem>();

        float TimeSinceLastClick = 1000;

        int HighlightedItem_Previous = -1;
        
        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public vxServerListDialog()
            : base("Server List", vxEnumButtonTypes.OkApplyCancel)
        {
            
            BckgrndWrkr_FileOpen = new System.ComponentModel.BackgroundWorker();
            BckgrndWrkr_FileOpen.WorkerReportsProgress = true;
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




            OKButton.Clicked += new EventHandler<vxUIControlClickEventArgs>(Btn_Ok_Clicked);
          CancelButton.Clicked += new EventHandler<vxUIControlClickEventArgs>(Btn_Cancel_Clicked);
          ApplyButton.Clicked += new EventHandler<vxUIControlClickEventArgs>(Btn_Apply_Clicked);
          ApplyButton.Text = "Refresh";

            ScrollPanel = new vxListView(new Vector2(
					this.ArtProvider.GUIBounds.X + this.ArtProvider.Padding.X, 
					this.ArtProvider.GUIBounds.Y + this.ArtProvider.Padding.Y),
				(int)(this.ArtProvider.GUIBounds.Width - this.ArtProvider.Padding.X * 2),
				(int)(this.ArtProvider.GUIBounds.Height - OKButton.Bounds.Height - this.ArtProvider.Padding.Y * 3));            

            InternalGUIManager.Add(ScrollPanel);
        }

        void Btn_Ok_Clicked(object sender, vxUIControlClickEventArgs e)
        {
            
        }

        void Btn_Cancel_Clicked(object sender, vxUIControlClickEventArgs e)
        {

        }

        void Engine_GameServerListRecieved(object sender, vxGameServerListRecievedEventArgs e)
        {
            int index = 0;
            foreach (string parsestring in e.ServerList)
            {
                if (index != 0)
                {					
					vxConsole.WriteNetworkLine("IP: " + parsestring.ReadXML("ip") + ", Port: " + parsestring.ReadXML("port"));

                    vxListViewItem item = new vxListViewItem(parsestring.ReadXML("ip"));
					item.ButtonWidth = ScrollPanel.Width - (int)(4 * this.ArtProvider.Padding.X);
                    
                    ScrollPanel.AddItem(item);
                }
                index++;
            }
        }

        void Btn_Apply_Clicked(object sender, vxUIControlClickEventArgs e)
        {
            //Send the request string
            //Engine.SendMessage("vrtx_request_serverList");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Responds to user input, accepting or cancelling the message box.
        /// </summary>
        protected internal override void HandleInput()
        {
            if (vxInput.IsNewMouseButtonPress(MouseButtons.LeftButton))
            {
                if (TimeSinceLastClick < 20)
                {
                    if(CurrentlySelected == HighlightedItem_Previous)
                        OKButton.Select();
                }
                else
                {
                    TimeSinceLastClick = 0;
                }

                HighlightedItem_Previous = CurrentlySelected;
            }
        }
        
        #endregion


        bool FirstLoop = true;
        //float LoadingAlpha = 0;
        //float LoadingAlpha_Req = 1;
        protected internal override void Update()
        {
            base.Update();

            TimeSinceLastClick++;

            if (FirstLoop)
            {
                FirstLoop = false;

                BckgrndWrkr_FileOpen.RunWorkerAsync();
            }
        }

        #region Draw

        string FileName = "";
        int GetHighlitedItem(int i)
        {
			foreach (vxFileDialogItem fd in List_Items)
            {
                fd.UnSelect();
            }

            List_Items[i].ThisSelect();
            CurrentlySelected = i;

            FileName = List_Items[i].FileName;
            return 0;
        }

        #endregion
    }
}

