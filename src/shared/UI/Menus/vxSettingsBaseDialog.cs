using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VerticesEngine.Graphics;
using VerticesEngine.Input;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Events;

namespace VerticesEngine.UI.Dialogs
{
    /// <summary>
    /// This is the base dialog for creating settings from a specified attribute type
    /// </summary>
    public class vxSettingsBaseDialog : vxDialogBase
    {

        protected vxScrollPanel ScrollPanel;

        Type settingsType;

        /// <summary>
        /// The Graphics Settings Dialog
        /// </summary>
        public vxSettingsBaseDialog(string title, Type settingsType)
            : base(title, vxEnumButtonTypes.OkApplyCancel)
        {
            this.settingsType = settingsType;
        }

        /// <summary>
        /// Load graphics content for the screen.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            //this.Title = LanguagePack[ChaoticLocKeys.Graphics];

            ScrollPanel = new vxScrollPanel(new Vector2(
        ArtProvider.GUIBounds.X + ArtProvider.Padding.X,
        ArtProvider.GUIBounds.Y + ArtProvider.Padding.Y) + ArtProvider.PosOffset,
    ArtProvider.GUIBounds.Width - (int)ArtProvider.Padding.X * 2,
    ArtProvider.GUIBounds.Height - OKButton.Bounds.Height - (int)ArtProvider.Padding.Y * 4);
            InternalGUIManager.Add(ScrollPanel);

            vxSettingsGUIItem.ArrowSpacing = 0;

            OnSettingsRetreival();

            // now get gui items which can be traveresed
            GuiItems.Clear();
            foreach(var panelItem in ScrollPanel.Items)
            {
                GuiItems.Add(panelItem);
            }

            GuiItems.Add(ApplyButton);
            GuiItems.Add(OKButton);
            GuiItems.Add(CancelButton);
        }

        public List<vxUIControl> GuiItems = new List<vxUIControl>();

        int guiNavIndex = -1;

        protected internal override void Update()
        {
            base.Update();

            if (vxInput.IsNewMainNavDown())
            {
                NavDown();
            }
            if (vxInput.IsNewMainNavUp())
            {
                NavUp();
            }

            if(vxInput.IsNewMainNavLeft())
            {
                NavLeft();
            }
            if (vxInput.IsNewMainNavRight())
            {
                NavRight();
            }
        }

        public void NavUp()
        {

            guiNavIndex--;
            if (guiNavIndex < 0)
                guiNavIndex = GuiItems.Count - 1;

            vxInput.Cursor = GuiItems[guiNavIndex].Bounds.Center.ToVector2();
        }

        public void NavDown()
        {
            guiNavIndex = (guiNavIndex + 1) % GuiItems.Count;

            vxInput.Cursor = GuiItems[guiNavIndex].Bounds.Center.ToVector2();
        }
        public void NavLeft()
        {
            if (guiNavIndex >= 0 && guiNavIndex < GuiItems.Count && GuiItems[guiNavIndex] is vxSettingsGUIItem)
            {
                ((vxSettingsGUIItem)GuiItems[guiNavIndex]).IncrementDown();
            }
            else
            {
                NavUp();
            }
        }
        public void NavRight()
        {
            if (guiNavIndex >= 0 && guiNavIndex < GuiItems.Count && GuiItems[guiNavIndex] is vxSettingsGUIItem)
            {
                ((vxSettingsGUIItem)GuiItems[guiNavIndex]).IncrementUp();
            }
            else
            {
                NavDown();
            }
        }

        protected virtual bool FilterAttribute(vxSettingsAttribute attribute)
        {
            return true;
        }
        class PropInfo
        {
            public string title;
            public PropertyInfo prop;
            public vxSettingsAttribute settings;
        }

        void ProcessAssembly(Assembly assembly)
        {
            if (assembly == null)
                return;

            // get all Graphical Settings
            foreach (Type type in assembly.GetTypes())
            {
                // this is handeled manually above
                if (type == typeof(vxScreen))
                    continue;

                // get member
                var properties = type.GetProperties().Where(field => Attribute.IsDefined(field, this.settingsType));
                if (properties == null) continue;


                List<PropInfo> settings = new List<PropInfo>();                
                foreach (PropertyInfo property in properties)
                {
                    var attribute = property.GetCustomAttribute<vxSettingsAttribute>();
                    settings.Add(new PropInfo()
                    {
                        prop =  property,
                        settings = attribute
                    });
                }
                
                foreach (var prop in settings)
                {
                    var property = prop.prop;
                    var attribute = prop.settings;
                    // check if this attribute is needed for this game type
                    //if(attribute.us)
                    if (FilterAttribute(attribute) == false)
                        continue;

                    if (attribute.IsMenuSetting)
                    {
                        string settingValue = property.GetValue(property).ToString();
                        var settingsGUIItem = new vxSettingsGUIItem(InternalGUIManager, attribute.DisplayName.SplitIntoSentance(), settingValue);

                        // Now set values based on type
                        if (property.PropertyType == typeof(bool))
                        {
                            //property.SetValue(setting, bool.Parse(settings[set.DisplayName]));
                            settingsGUIItem.AddOption("On");
                            settingsGUIItem.AddOption("Off");
                            settingValue = (bool)property.GetValue(property) ? "On" : "Off";

                            settingsGUIItem.ValueChangedEvent += delegate
                            {
                                property.SetValue(property, (settingsGUIItem.SelectedIndex % 2) == 0);
                            };

                        }

                        else if (property.PropertyType.IsEnum)
                        {
                            //property.SetValue(setting, Enum.Parse(setting.FieldType, settings[set.DisplayName]));
                            foreach (var propType in Enum.GetValues(property.PropertyType))
                                settingsGUIItem.AddOption(propType.ToString());

                            settingsGUIItem.ValueChangedEvent += delegate
                            {
                                property.SetValue(property, settingsGUIItem.SelectedIndex);
                            };
                        }

                        else
                        {
                            Console.WriteLine("----- No Setting Found for '" + property.PropertyType.ToString() + "' -----");
                        }

                        settingsGUIItem.Value = settingValue;
                        ScrollPanel.AddItem(settingsGUIItem);
                    }
                }
            }
        }

        protected virtual void OnSettingsRetreival()
        {
            ProcessAssembly(Assembly.GetExecutingAssembly());
            ProcessAssembly(Assembly.GetEntryAssembly());
        }

        void Btn_Apply_Clicked(object sender, vxUIControlClickEventArgs e)
        {
            SetSettings();

            // Now Exit Screen, and Readd teh screen
            ExitScreen();
        }

        /// <inheritdoc/>
        protected override void OnOKButtonClicked(object sender, vxUIControlClickEventArgs e)
        {
            SetSettings();
            ExitScreen();
        }



        void SetSettings()
        {
            vxSettings.Save();
        }
    }
}
