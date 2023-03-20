using System;
using System.Collections.Generic;
using System.Text;
using VerticesEngine.Workshop.Events;

namespace VerticesEngine.Workshop.Providers.Generic
{
    /// <summary>
    /// An empty generic workshop wrapper.
    /// </summary>
    public class vxGenericWorkshopWrapper : vxIWorkshopWrapper
    {
#pragma warning disable 067, 649

        public event EventHandler<vxWorkshopSeachReceievedEventArgs> SearchResultReceived;
        public event EventHandler<vxWorkshopItemPublishedEventArgs> ItemPublished;

        public void Download(vxIWorkshopItem id, Action<bool, string> callback)
        {

        }

        public string GetWorkshopItemURL(string id)
        {
            return string.Empty;
        }

        public void Publish(string title, string description, string imgPath, string folderPath, string[] tags, string idToUpdate = "", string changelog = "")
        {
            vxConsole.WriteWarning(this.GetType().ToString(), "You're trying to publish using a generic workshop wrapper. Nothing will actually happen");
        }

        public void Search(vxWorkshopSearchQuery searchCrteria, Action<vxWorkshopSearchResults> callback)
        {
            vxConsole.WriteWarning(this.GetType().ToString(), "You're trying to search using a generic workshop wrapper. Nothing will actually happen");
        }


        public void Update()
        {

        }
#pragma warning restore 067, 649
    }
}