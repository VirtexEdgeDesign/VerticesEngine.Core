using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.UI.MessageBoxs
{
    /// <summary>
    /// Displays an about box with info about this game and the engine
    /// </summary>
    public class vxAboutBox : vxMessageBox
    {
        /// <summary>
        /// Shows an About Box
        /// </summary>
        public static vxAboutBox Show()
        {
            var aboutBox = new vxAboutBox();
            vxSceneManager.AddScene(aboutBox);
            return aboutBox;
        }

        private vxAboutBox() : base("", "About", vxEnumButtonTypes.Ok)
        {
            this.Message = $"About this game\n\n" +
                $"Game Name:            {vxEngine.Game.Name}\n" +
                $"Game Version          v.{vxEngine.Game.Version}\n" +
                $"Release Version:      {vxEngine.ReleasePlatformType}\n" +
                $"Engine Version:       v.{vxEngine.EngineVersion}\n" +
                $"";
        }
    }
}
