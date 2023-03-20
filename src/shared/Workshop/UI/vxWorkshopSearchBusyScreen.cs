using System;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using VerticesEngine.Util;
//using VerticesEngine;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.MessageBoxs;
using VerticesEngine.Serilization;
using VerticesEngine.Utilities;
using VerticesEngine;
using VerticesEngine.Workshop.Events;
using VerticesEngine.Profile;
using VerticesEngine.UI;

namespace VerticesEngine.Workshop.UI
{
    /// <summary>
    /// The Workshop Busy Screen
    /// </summary>
    public class vxWorkshopSearchBusyScreen : vxMessageBox
    {


        vxWorkshopSearchQuery searchCriteria;

        /// <summary>
        /// Initializes a new instance of the <see cref="vxWorkshopSearchBusyScreen"/> class.
        /// </summary>
        public vxWorkshopSearchBusyScreen(vxWorkshopSearchQuery searchCriteria)
            : base("Searching", "Searching Workshop", vxEnumButtonTypes.Ok)
        {
            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);

            this.searchCriteria = searchCriteria;

        }

        public override void LoadContent()
        {
            base.LoadContent();

            OKButton.Text = "Cancel";

            OKButton.Clicked += delegate {

                ExitScreen();

                //vxWorkshop.Instance.SearchResultReceived -= OnSearchResultReceived;
            };
        }


        int Inc = 0;


        protected internal override void Update()
        {
            if (Inc == 40)
            {
                StartSearch();
            }
            string msgText = "Searching " + searchCriteria.ItemCriteria + " Items";
            int origLength = msgText.Length + 5;
            Inc++;

            msgText += new string('.', (int)(Inc / 10) % 3);

            Message = msgText + "\n" + new string(' ', origLength * 3 /2 );;
            //Bounds.Width = 300;
            base.Update();

        }

        public void StartSearch()
        {            
            vxWorkshop.Instance.Search(searchCriteria, OnSearchResults);
            //vxWorkshop.Instance.SearchResultReceived += OnSearchResultReceived;
        }

        private void OnSearchResults(vxWorkshopSearchResults searchResults)
        {
            ExitScreen();

            vxSceneManager.AddScene(new vxWorkshopSearchResultDialog(searchResults, 0));
        }

        //void OnSearchResultReceived(object sender, vxWorkshopSeachReceievedEventArgs e)
        //{
        //    vxWorkshop.OnSearchResultsReceived(e.Items);

        //    ExitScreen();
            
        //    vxSceneManager.AddScene(new vxWorkshopSearchResultDialog( 0));

        //    vxWorkshop.Instance.SearchResultReceived -= OnSearchResultReceived;
        //}
    }
}
