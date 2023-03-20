using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using VerticesEngine.Plugins.Mods;
using VerticesEngine.Utilities;


namespace VerticesEngine.Plugins
{
    /// <summary>
    /// This class handles all of the plugins for a Vertices game, whether it's DLC or Mods
    /// </summary>
    public static class vxPluginManager
    {
        /// <summary>
        /// The main game content pack which is retreieved in an override method in the vxGame class. This is
        /// to allow for the main game content pack to be processed and handeled no differently then any other
        /// plugin such as DLC or Mods
        /// </summary>
        public static vxIPlugin CoreGameModule
        {
            get { return _coreGameModule; }
        }
        static vxIPlugin _coreGameModule;

        public static Dictionary<string, vxIPlugin> DLCPacks
        {
            get { return _dlc; }
        }
        static Dictionary<string, vxIPlugin> _dlc = new Dictionary<string, vxIPlugin>();


        static public Dictionary<string, vxIPlugin> ModPacks
        {
            get { return _mods; }
        }
        static Dictionary<string, vxIPlugin> _mods = new Dictionary<string, vxIPlugin>();

        internal static void Init()
        {
            
        }


        internal static void LoadPlugins()
        {
            // Load the games main content pack
            _coreGameModule = vxEngine.Game.GetCoreGamePlugin();
            _coreGameModule.Initialise();

            // TODO: Fix loading mods later
            return;

#pragma warning disable CS0162
            // Load the DLC
            foreach (var dlc in vxEngine.Game.GetDLCPaths())
                LoadAssembly(dlc, vxPluginType.DLC);

#pragma warning restore CS0162

            // Now Load Mods

            // first check if the mod list is available
            var modListFile = System.IO.Path.Combine(vxIO.PathToMods, "mods.json");
            if (File.Exists(modListFile))
            {
                var reader = new StreamReader(modListFile);
                string fileText = reader.ReadToEnd();
                reader.Close();

                // first read the mods list
                var tempModList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<vxModItemStatus>>(fileText);

                // now add all the enabled mods
                foreach (var mod in tempModList)
                {
                    if(mod.IsEnabled)
                    {
                        LoadAssembly(mod.Path, vxPluginType.Mod);
                    }
                }
            }

            // if it's in debug mode, load the dev-mods
            if (vxEngine.BuildType == vxBuildType.Debug)
            {
                foreach (var devmod in vxEngine.Instance.LoadDevMods())
                    LoadAssembly(devmod, vxPluginType.Mod);
            }
        }


        /// <summary>
        /// Loads a Plugin Assembly. This could be DLC or a Mod.
        /// </summary>
        /// <param name="assemblyFilePath"></param>
        private static void LoadAssembly(String assemblyFilePath, vxPluginType pluginToLoadType)
        {
            if (!File.Exists(assemblyFilePath) || !assemblyFilePath.EndsWith(".dll", true, null))
            {
                vxConsole.WriteError(string.Format("Assembly Not Found: '{0}'", assemblyFilePath));
                return;
            }
            Assembly assembly = null;

            try
            {
                assembly = Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, assemblyFilePath));

                Type pluginType = typeof(vxIPlugin);

                ICollection<Type> pluginTypes = new List<Type>();

                if (assembly != null)
                {
                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        if (type.IsInterface || type.IsAbstract)
                        {
                            continue;
                        }
                        else
                        {
                            if (type.GetInterface(pluginType.FullName) != null)
                            {
                                pluginTypes.Add(type);
                            }
                        }
                    }
                }


                foreach (Type type in pluginTypes)
                {
                    vxConsole.WriteDLCLine("Initialising Plugin DLL '" + type + ".dll'");

                    vxIPlugin plugin = (vxIPlugin)Activator.CreateInstance(type);
                    plugin.Initialise();

                    if (pluginToLoadType == vxPluginType.DLC)
                        _dlc.Add(plugin.ID, plugin);
                    else
                        _mods.Add(plugin.ID, plugin);

                    vxConsole.WriteDLCLine("     Name '" + plugin.Name + "'");
                    vxConsole.WriteDLCLine("     Description '" + plugin.Description + "'");
                }
            }
            catch (Exception ex)
            {
                vxConsole.WriteException("vxPluginManager", ex);
            }
        }

        //static void LoadPluginContent(vxIContentPack plugin)
        //{
        //    // first load plugin content
        //    plugin.LoadContent();

        //    // now register all plugin entities
        //    vxEntityRegister.RegisterAssemblyEntityTypes(plugin);
        //}


        ///// <summary>
        ///// This loads all of the content for the DLC packs
        ///// </summary>
        //internal static void LoadContent()
        //{
        //    vxConsole.WriteDLCLine("Loading Main Content...");
        //    LoadPluginContent(_mainGameContentPack);

        //    vxConsole.WriteDLCLine("Loading Plugin Content...");
        //    foreach (var plugin in _dlc.Values)
        //    {
        //        try
        //        {
        //            vxConsole.WriteDLCLine("     Loading DLC '" + plugin.Name + "' Content...");
        //            LoadPluginContent(plugin);
        //        }
        //        catch (Exception ex)
        //        {
        //            vxConsole.WriteException("vxPluginManager", ex);
        //        }
        //    }
        //    foreach (var plugin in _mods.Values)
        //    {
        //        try
        //        {
        //            vxConsole.WriteDLCLine("     Loading Mod '" + plugin.Name + "' Content...");
        //            LoadPluginContent(plugin);
        //        }
        //        catch (Exception ex)
        //        {
        //            vxConsole.WriteException("vxPluginManager", ex);
        //        }
        //    }
        //}

        /// <summary>
        /// Gets the available mods in path.
        /// </summary>
        /// <returns>The available mods in path.</returns>
        /// <param name="path">Path.</param>
        public static string[] GetAvailableModsInPath(string path)
        {
            DirectoryInfo d = new DirectoryInfo(path);

            List<string> mods = new List<string>();
            if (Directory.Exists(d.FullName))
            {
                // Now loop through all workshop directories
                foreach (var modPath in Directory.GetDirectories(d.FullName))
                {
                    //var pluginConfigPath = Path.Combine(modPath, vxPluginConfig.ConfigFileName);
                    // if the items directory has a modconfig.json file, then it's a mod pack.
                    if (File.Exists(Path.Combine(modPath, vxModConfig.ConfigFileName)))
                    {
                        // now get the file name to be loaded
                        var pluginFile = vxModConfig.Load(modPath);



                        // first check if the plugin files id is matches the steam dir. if not
                        // then this might be the first draft and the appid or something went wrong.
                        // never fear, we can pull this from the parent dir,
                        // let's get it, set it, and resave it.

                        //DirectoryInfo info = new DirectoryInfo(modPath);
                        //if (info.Name != pluginFile.ID.ToString())
                        //{
                        //    try

                        //    {
                        //        vxConsole.WriteWarning("Mod loading", "Mod config for '" + info.Name + "' has wrong Steam App ID. Resetting it.");
                        //        pluginFile.ID = long.Parse(info.Name);
                        //        pluginFile.Save();

                        //        // now reload it
                        //        pluginFile = vxModConfig.Load(modPath);
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        vxConsole.WriteException("vxPluginManager.GetAvailableModsInPath(path)", ex);
                        //    }
                        //}

                        mods.Add(Path.Combine(modPath, pluginFile.File));
                    }
                }
            }

            return mods.ToArray();
        }

        #region -- Debug Methods --

        [Diagnostics.vxDebugMethod("mods", "Lists the Currently Loaded Mods")]
        static void ListCurrentlyLoadedMods()
        {
            vxConsole.WriteDLCLine("Listing " + vxPluginManager.ModPacks.Count + " loaded mods");
            foreach (var mod in vxPluginManager.ModPacks.Values)
            {
                vxConsole.WriteDLCLine("Name: " + mod.Name);
                vxConsole.WriteDLCLine("     Desc. : " + mod.Description);
                vxConsole.WriteDLCLine("     ID    : " + mod.ID);
            }
        }

        #endregion
    }
}
