using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using VerticesEngine.Utilities;
using VerticesEngine.Serilization;
using VerticesEngine.Graphics;
using System.Reflection;
using VerticesEngine.Diagnostics;

namespace VerticesEngine
{
    public enum vxEnumQuality
    {
        None = 0,
        Low = 1,
        Medium = 2,
        High = 3,
        Ultra = 4
    }

    /// <summary>
    /// The Serializable Settings class. This class creates Property get/set .
    /// </summary>
    public class vxSettings
    {
        [vxGameSettingsAttribute("GameVersion")]
        public static vxSerializableVersion GameVersion;

        [vxGameSettingsAttribute("Language")]
        public static string Language = "";


        [vxGameSettingsAttribute("srp")]
        public static bool ShownReviewPage = false;


        [vxGameSettingsAttribute("rc")]
        public static int ReviewCounter = 0;

        [vxEngineSettingsAttribute("IsConsoleSavedOnExit")]
        public static bool IsConsoleSavedOnExit = false;

        #region -- Loading and Saving ini files

        public static List<Type> settingTypes = new List<Type>();

        internal static void Init()
        {
            GameVersion = new vxSerializableVersion();
            // get all classes which inherit from the vxSettingsAttribute class
            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (t.IsSubclassOf(typeof(vxSettingsAttribute)))
                    settingTypes.Add(t);
            }

            // the entry assembly can be null on some platforms such as mobile
            var entryAssembly = Assembly.GetEntryAssembly();

            if (entryAssembly != null)
            {
                foreach (Type t in entryAssembly.GetTypes())
                {
                    if (t.IsSubclassOf(typeof(vxSettingsAttribute)))
                        settingTypes.Add(t);
                }
            }

            // if we're not on Android, then we can load settings straight away,
            // android platforms will have their settings loaded during the InitScreen once they've 
            // said okay to the proper permissions
#if __ANDROID__
        if(vxEngine.Game.IsPermissionsRequestRequired() == false)
        {
            LoadINI();
        }
#else
            LoadINI();
#endif
        }

        public static void Load()
        {
            LoadINI();
        }

        public static void Save()
        {
            SaveINI();
        }

        private static void LoadINI()
        {
            foreach (var tp in settingTypes)
            {
                try
                {
                    LoadINIFile(tp);
                }
                catch(Exception ex)
                {
                    vxConsole.WriteException("vxSettings", ex);
                }
            }
        }

        static void LoadINIFromAssembly(Assembly assembly, Type tp, ref Dictionary<string, string> settings)
        {
            if (assembly == null)
                return;

            foreach (Type type in assembly.GetTypes())
            {
                // get member
                var fields = type.GetFields().Where(field => Attribute.IsDefined(field, tp));
                foreach (FieldInfo setting in fields)
                {
                    var set = setting.GetCustomAttribute<vxSettingsAttribute>();
                    if (settings.ContainsKey(set.DisplayName))
                    {
                        if (setting.FieldType == typeof(bool))
                        {
                            setting.SetValue(setting, bool.Parse(settings[set.DisplayName]));
                        }
                        else if (setting.FieldType == typeof(int))
                        {
                            setting.SetValue(setting, int.Parse(settings[set.DisplayName]));
                        }
                        else if (setting.FieldType == typeof(float))
                        {
                            setting.SetValue(setting, float.Parse(settings[set.DisplayName]));
                        }
                        else if (setting.FieldType.IsEnum)
                        {
                            setting.SetValue(setting, Enum.Parse(setting.FieldType, settings[set.DisplayName]));
                        }
                        else if (setting.FieldType == typeof(string))
                        {
                            setting.SetValue(setting, settings[set.DisplayName]);
                        }
                        else if (setting.FieldType == typeof(vxSerializableVersion))
                        {
                            setting.SetValue(setting, new vxSerializableVersion(settings[set.DisplayName]));
                        }
                        else
                        {
                            Console.WriteLine("----- No Setting Found for '" + setting.FieldType.ToString() + "' -----");
                        }
                        //Console.WriteLine("Property " + field.Name + " - " + field);
                    }

                }


                // get member
                var properties = type.GetProperties().Where(field => Attribute.IsDefined(field, tp));
                foreach (PropertyInfo setting in properties)
                {
                    var set = setting.GetCustomAttribute<vxSettingsAttribute>();
                    if (settings.ContainsKey(set.DisplayName))
                    {
                        if (setting.PropertyType == typeof(bool))
                        {
                            setting.SetValue(setting, bool.Parse(settings[set.DisplayName]));
                        }
                        else if (setting.PropertyType == typeof(int))
                        {
                            setting.SetValue(setting, int.Parse(settings[set.DisplayName]));
                        }
                        else if (setting.PropertyType == typeof(float))
                        {
                            setting.SetValue(setting, float.Parse(settings[set.DisplayName]));
                        }
                        else if (setting.PropertyType.IsEnum)
                        {
                            setting.SetValue(setting, Enum.Parse(setting.PropertyType, settings[set.DisplayName]));
                        }
                        else if (setting.PropertyType == typeof(string))
                        {
                            setting.SetValue(setting, settings[set.DisplayName]);
                        }
                        else
                        {
                            Console.WriteLine("----- No Setting Found for '" + setting.PropertyType.ToString() + "' -----");
                        }
                        //Console.WriteLine("Property " + field.Name + " - " + field);
                    }

                }
            }
        }

        private static void LoadINIFile(Type tp)
        {
            var st = tp.Name;
            string settingName = st.Substring(2, st.IndexOf("Attribute") - 2);

            // dictionary of keys and values
            Dictionary<string, string> settings = new Dictionary<string, string>();

            string filePath = Path.Combine(vxIO.PathToSettings, settingName + ".ini");

            vxDebug.LogIO(new
            {
                file = filePath
            });

            // if there's no file, then save a version
            if (!File.Exists(filePath))
            {
                SaveINIFile(tp);
                //return;
            }

            //settings
            string[] settingsText = File.ReadAllLines(filePath);

            foreach (var line in settingsText)
            {
                if (line.Contains("="))
                {
                    string key = line.Substring(0, line.IndexOf("="));
                    string value = line.Substring(line.IndexOf("=") + 1);
                    if(settings.ContainsKey(key) == false)
                    settings.Add(key, value);
                    //Console.WriteLine(string.Format("'{0}':'{1}'", key, value));
                }
            }

            LoadINIFromAssembly(Assembly.GetExecutingAssembly(), tp, ref settings);
            LoadINIFromAssembly(Assembly.GetEntryAssembly(), tp, ref settings);
        }

        private static void SaveINI()
        {
            foreach (var tp in settingTypes)
            {
                try
                {
                    SaveINIFile(tp);
                }
                catch(Exception ex)
                {
                    vxConsole.WriteException("vxSettings.SaveINI()", ex);
                }
            }
        }

        private static void SaveAssemIniFile(Assembly assembly, StreamWriter writer, Type mainType)
        {
            bool hasAttribute = false;
            if (assembly != null)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    // get fields
                    var fields = type.GetFields().Where(field => Attribute.IsDefined(field, mainType));
                    foreach (FieldInfo field in fields)
                    {
                        if (hasAttribute == false)
                        {
                            hasAttribute = true;

                            writer.WriteLine("");
                            writer.WriteLine("[" + type.Name + "]");
                        }
                        var attr = field.GetCustomAttribute<vxSettingsAttribute>();

                        if (attr.IsSavedToINIFile)
                            writer.WriteLine(string.Format("{0}={1}", attr.DisplayName, field.GetValue(field)));

                    }

                    // get properties
                    var properties = type.GetProperties().Where(prop => Attribute.IsDefined(prop, mainType));
                    foreach (var prop in properties)
                    {
                        if (hasAttribute == false)
                        {
                            hasAttribute = true;

                            writer.WriteLine("");
                            writer.WriteLine("[" + type.Name + "]");
                        }
                        var attr = prop.GetCustomAttribute<vxSettingsAttribute>();

                        if (attr.IsSavedToINIFile)
                            writer.WriteLine(string.Format("{0}={1}", attr.DisplayName, prop.GetValue(prop)));
                    }


                    hasAttribute = false;
                }
            }
        }

        private static void SaveINIFile(Type tp)
        {
            var st = tp.Name;
            string settingName = st.Substring(2, st.IndexOf("Attribute") - 2);

            string filePath = Path.Combine(vxIO.PathToSettings, settingName + ".ini");

            vxDebug.LogIO(new
            {
                file=filePath
            });

            StreamWriter writer = new StreamWriter(filePath);


            SaveAssemIniFile(Assembly.GetExecutingAssembly(), writer, tp);
            SaveAssemIniFile(Assembly.GetEntryAssembly(), writer, tp);

            writer.Close();
        }


        #endregion
    }
}
