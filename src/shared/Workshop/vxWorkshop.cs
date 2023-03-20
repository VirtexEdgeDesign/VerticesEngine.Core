using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.UI.Dialogs;
using VerticesEngine;

namespace VerticesEngine.Workshop
{
    /// <summary>
    /// Workshop class which handles different player creation providers like Steam Workshop
    /// </summary>
    public class vxWorkshop 
    {
        public static vxIWorkshopWrapper Instance
        {
            get
            {
                return _instance;
            }
        }
        private static vxIWorkshopWrapper _instance;

        internal static void Init()
        {
            _instance = vxEngine.Game.OnInitWorkshopWrapper();
        }

        //public static vxWorkshopSearchQuery PreviousSearch
        //{
        //    get { return _previousSearch; }   
        //}
        //static vxWorkshopSearchQuery _previousSearch;

        //internal static void OnSearch(vxWorkshopSearchQuery search)
        //{
        //    _previousSearch = search;
        //}

        //internal static void OnSearchResultsReceived(List<vxIWorkshopItem> results)
        //{
        //    _previousSearch.setResults(results);
        //}
    }
}
