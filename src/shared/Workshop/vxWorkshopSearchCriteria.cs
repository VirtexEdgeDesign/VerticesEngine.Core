using System;
using System.Collections.Generic;

namespace VerticesEngine.Workshop
{
    /// <summary>
    /// Steam Workshop item search criteria.
    /// </summary>
    public enum vxWorkshopItemSearchCriteria
    {
        /// <summary>
        /// Searches all workshop items
        /// </summary>
        All,

        /// <summary>
        /// Searches all favourited items
        /// </summary>
        Favourited
    }

    /// <summary>
    /// Workshop search criteria.
    /// </summary>
    public class vxWorkshopSearchQuery
    {
        /// <summary>
        /// The item criteria.
        /// </summary>
        public vxWorkshopItemSearchCriteria ItemCriteria { get; private set; }

        /// <summary>
        /// The search text.
        /// </summary>
        public string SearchText { get; private set; }

        /// <summary>
        /// The tags to include.
        /// </summary>
        public string[] Tags = { };

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.Workshop.vxWorkshopSearchCriteria"/> class.
        /// </summary>
        /// <param name="SearchText">Search text.</param>
        /// <param name="ItemCriteria">Item criteria.</param>
        public vxWorkshopSearchQuery(vxWorkshopItemSearchCriteria ItemCriteria, string SearchText="") : this(ItemCriteria, SearchText, new string[] { })
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.Workshop.vxWorkshopSearchCriteria"/> class.
        /// </summary>
        /// <param name="SearchText">Search text.</param>
        /// <param name="ItemCriteria">Item criteria.</param>
        /// <param name="Tags">Tags to include.</param>
        public vxWorkshopSearchQuery(vxWorkshopItemSearchCriteria ItemCriteria, string SearchText, string[] Tags)
        {
            this.ItemCriteria = ItemCriteria;
            this.Tags = Tags;
        }
    }

    /// <summary>
    /// The workshop search results
    /// </summary>
    public class vxWorkshopSearchResults
    {
        /// <summary>
        /// results
        /// </summary>
        public List<vxIWorkshopItem> ItemResults { get; private set; }

        /// <summary>
        /// The original query
        /// </summary>
        public vxWorkshopSearchQuery Query { get; private set; }

        public vxWorkshopSearchResults(vxWorkshopSearchQuery query, List<vxIWorkshopItem> results)
        {
            ItemResults = results;
            Query = query;
        }
    }
}
