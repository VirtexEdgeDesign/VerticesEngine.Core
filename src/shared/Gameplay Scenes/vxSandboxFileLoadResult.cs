using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VerticesEngine.Utilities;
using VerticesEngine.Plugins;
using System.Xml.Serialization;

namespace VerticesEngine
{


    public class vxSandboxFileLoadResult
    {
        public vxSandboxFileLoadError FileLoadErrorType { get; private set; }

        public bool IsSuccessful { get; private set; }

        public List<vxPluginMetadata> MissingPlugins = new List<vxPluginMetadata>();

        public string MissingFilePath { get; private set; }

        public vxSandboxFileLoadResult()
        {
            IsSuccessful = true;
            FileLoadErrorType = vxSandboxFileLoadError.None;
        }

        public vxSandboxFileLoadResult(string path)
        {
            MissingFilePath = path;
            IsSuccessful = false;
            FileLoadErrorType = vxSandboxFileLoadError.FileNotFound;
        }


        public vxSandboxFileLoadResult(List<vxPluginMetadata> missingPlugins)
        {
            IsSuccessful = false;
            FileLoadErrorType = vxSandboxFileLoadError.PluginNotFound;

            foreach(var plugin in missingPlugins)
            {
                MissingPlugins.Add(plugin);
            }
        }
    }


}
