using System;
using System.Collections.Generic;
using System.Text;
using VerticesEngine.Workshop.Events;

namespace VerticesEngine.Workshop
{
    public interface vxIWorkshopWrapper
    {
        /// <summary>
        /// Called when a workshop item (or level) is published.
        /// </summary>
        event EventHandler<vxWorkshopItemPublishedEventArgs> ItemPublished;

        /// <summary>
        /// Publish a folder to the given Workshop Backend
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="imgPath"></param>
        /// <param name="uploadPath"></param>
        /// <param name="tags"></param>
        /// <param name="idToUpdate"></param>
        /// <param name="changelog"></param>
        void Publish(string title, string description, string imgPath, string uploadPath, string[] tags,
                           string idToUpdate = "", string changelog = "");

        /// <summary>
        /// Search the workshop
        /// </summary>
        /// <param name="searchCrteria"></param>
        void Search(vxWorkshopSearchQuery searchCrteria, Action<vxWorkshopSearchResults> callback);

        /// <summary>
        /// Downloads a given file 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="callback"></param>
        void Download(vxIWorkshopItem item, Action<bool, string> callback);


        /// <summary>
        /// Runs any update callbacks
        /// </summary>
        void Update();

        /// <summary>
        /// Returns the URL for a given workshop item. 
        /// </summary>
        /// <param name="id">Workshop specific id</param>
        /// <returns>URL path to the workshop item</returns>
        string GetWorkshopItemURL(string id);

    }
}
