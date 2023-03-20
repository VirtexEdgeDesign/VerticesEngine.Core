using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VerticesEngine;
using VerticesEngine.Graphics;
using VerticesEngine.Utilities;
using VerticesEngine.Diagnostics;
//using BEPUphysics;

namespace VerticesEngine.ContentManagement
{
    public struct LevelContentEntry
    {
        public object entityRef;
        
        public string path;
    }

	/// <summary>
	/// Class which encorporates a number of different functions for asset loading and content management.
	/// </summary>
	public class vxLevelContentManager : Microsoft.Xna.Framework.Content.ContentManager
    {

        /// <summary>
        /// A List collection of all loaded vxModel's.
        /// </summary>
        public List<vxMesh> LoadedModels = new List<vxMesh>();



		/// <summary>
		/// Initializes a new instance of the <see cref="T:VerticesEngine.ContentManagement.vxContentManager"/> class for handling 
        /// internal assets.
		/// </summary>
		/// <param name="Engine">Engine.</param>
        public vxLevelContentManager() : base(vxEngine.Game.Services, "Content")
        {

        }

    }
}
