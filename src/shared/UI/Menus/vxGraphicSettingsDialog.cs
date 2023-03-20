using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VerticesEngine.Graphics;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Events;
using VerticesEngine.Utilities;

namespace VerticesEngine.UI.Dialogs
{


    /// <summary>
    /// The graphic settings dialog.
    /// </summary>
    public class vxGraphicSettingsDialog : vxSettingsBaseDialog
    {

        /// <summary>
        /// The Graphics Settings Dialog
        /// </summary>
        public vxGraphicSettingsDialog()
            : base("Graphics Settings", typeof(vxGraphicalSettingsAttribute))
        {

        }

        public override void LoadContent()
        {
            base.LoadContent();
        }


        protected override bool FilterAttribute(vxSettingsAttribute attribute)
        {
            if (attribute.GetType() == typeof(vxGraphicalSettingsAttribute))
            {
                var gSet = ((vxGraphicalSettingsAttribute)attribute);

                // TODO: Games shouldn't care if their 2D or 3D
                if (gSet.Usage.IsFlagSet(vxGameEnviromentType.TwoDimensional) && vxEngine.Game.GameType.IsFlagSet(vxGameEnviromentType.TwoDimensional))
                {
                    return true;
                }

                if (gSet.Usage.IsFlagSet(vxGameEnviromentType.ThreeDimensional) && vxEngine.Game.GameType.IsFlagSet(vxGameEnviromentType.ThreeDimensional))
                {
                    return true;
                }
            }

            return false;
        }

        Point _tempResolution;
        //bool _tempIsFullscreen = true;
        private vxFullScreenMode _tempFullScreenMode = vxFullScreenMode.Borderless;
        protected override void OnSettingsRetreival()
        {
            //Resolutions
            // *****************************************************************************************************
            List<DisplayMode> DisplayModes = new List<DisplayMode>();

            string currentRes = string.Format("{0}x{1}", vxScreen.Width, vxScreen.Height);
            //string currentRes2 = $"{ vxScreen.Width}x{ vxScreen.Height}";
            
            var resolution = new vxSettingsGUIItem(InternalGUIManager, vxLocalizer.GetText(vxLocKeys.Settings_Graphics_Resolution), currentRes);


            _tempResolution = vxScreen.Resolution;

            foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                //Don't Show All Resolutions
                if (mode.Width > 599 || mode.Height > 479)
                {
                    // Add the Display mode to the list
                    DisplayModes.Add(mode);

                    // now add the text representation
                    resolution.AddOption(string.Format("{0}x{1}", mode.Width, mode.Height));
                }
            }
            resolution.ValueChangedEvent += delegate
            {
                var mode = DisplayModes[resolution.SelectedIndex];
                
                //vxScreen.SetResolution
                _tempResolution = new Point(mode.Width, mode.Height);
            };
            ScrollPanel.AddItem(resolution);

            //Full Screen
            // *****************************************************************************************************

            _tempFullScreenMode = vxScreen.FullScreenMode;
            var fullscreenSetting = new vxSettingsGUIItem(InternalGUIManager, vxLocalizer.GetText(vxLocKeys.Settings_Graphics_FullScreen),
                _tempFullScreenMode.ToString());
            
            foreach (var propType in Enum.GetValues(typeof(vxFullScreenMode)))
                fullscreenSetting.AddOption(propType.ToString());

            fullscreenSetting.ValueChangedEvent += delegate
            {
                _tempFullScreenMode = (vxFullScreenMode)fullscreenSetting.SelectedIndex;
            };
             ScrollPanel.AddItem(fullscreenSetting);


            //VSync
            // *****************************************************************************************************
            var VSyncSettingsItem = new vxSettingsGUIItem(InternalGUIManager, "Vertical Sync",
                vxScreen.IsVSyncOn ? "On" : "Off");

            VSyncSettingsItem.AddOption("On");
            VSyncSettingsItem.AddOption("Off");

            ScrollPanel.AddItem(VSyncSettingsItem);

            base.OnSettingsRetreival();

            this.Title = vxLocalizer.GetText(vxLocKeys.Settings_Graphics);
        }


        /// <inheritdoc/>
        protected override void OnApplyButtonClicked(object sender, vxUIControlClickEventArgs e)
        {
            SetSettings();

            // Now Exit Screen, and Readd teh screen
            ExitScreen();
            vxSceneManager.AddScene(new vxGraphicSettingsDialog());
        }

        /// <inheritdoc/>
        protected override void OnOKButtonClicked(object sender, vxUIControlClickEventArgs e)
        {
            SetSettings();
            ExitScreen();
        }



        void SetSettings()
        {
            //Save Settings
            vxScreen.FullScreenMode = _tempFullScreenMode;
            vxScreen.SetResolution(_tempResolution);
            vxScreen.RefreshGraphics();

            vxSettings.Save();

            vxConsole.WriteLine("Settings Graphics to the Following Settings:");

        }

        void DebugSettingChange(string name, object setting)
        {
            vxConsole.WriteLine(string.Format("     {0} : {1}", name, setting));
        }
    }
}
