#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Input.Events;
using VerticesEngine;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Dialogs.Utilities;
using VerticesEngine.Diagnostics;


#if __ANDROID__
using Android.App;
using Android.Content;
using Android.Views;
using Android.Views.InputMethods;
#endif

#endregion

namespace VerticesEngine.UI.Dialogs
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    public class vxDebugSettingsDialog : vxSettingsBaseDialog
    {

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public vxDebugSettingsDialog() : base("Debug Settings", typeof(vxEngineSettingsAttribute))
        {
           
        }
        protected override void OnSettingsRetreival()
        {
            // add all tools
            foreach(var tool in vxDebug.DebugTools)
            {
                var settingsItem = new vxSettingsGUIItem(InternalGUIManager, tool.DebugToolName,  (tool.IsVisible ? "On" : "Off"));

                settingsItem.AddOption("On");
                settingsItem.AddOption("Off");
                
                settingsItem.ValueChangedEvent += delegate
                {
                    tool.IsVisible = !tool.IsVisible;
                    settingsItem.Text = tool.DebugToolName + " - " + (tool.IsVisible ? "Yes" : "No");
                };

                ScrollPanel.AddItem(settingsItem);
            }


            base.OnSettingsRetreival();

        }

        #endregion

        
    }
}
