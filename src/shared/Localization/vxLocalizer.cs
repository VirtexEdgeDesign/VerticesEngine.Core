using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using VerticesEngine.ContentManagement;
using VerticesEngine.UI.Themes;
using VerticesEngine.Utilities;

namespace VerticesEngine
{
    /// <summary>
    /// Holds, converts and localizes content for different regions.
    /// </summary>
    public static class vxLocalizer
    {
        /// <summary>
        /// The Content Root path for localization files. The default is 'local' but you can change this to anything else before calling 'SetLocalization'
        /// </summary>
        public static string LocalContentRootPath = @"local";

        /// <summary>
        /// A key-value pair for the Supported Languages by this game. The key is the 2-digit ISO code, and the value is the language name
        /// </summary>
        public static Dictionary<string, string> SupportedLangagues
        {
            get { return m_supportedLangagues; }
        }
        private static Dictionary<string, string> m_supportedLangagues = new Dictionary<string, string>();
        
        private static Dictionary<string, string> m_langaugeKeys = new Dictionary<string, string>();

        public static string CurrentLanguage
        {
            get { return m_supportedLangagues[_currentLocKey]; }
        }

        private static string _currentLocKey;

        private static void GetSystemLocalization()
        {
            vxDebug.LogEngine(new
            {
                name = CultureInfo.CurrentCulture.Name,
                fullName = CultureInfo.CurrentCulture.EnglishName,
                iso3 = CultureInfo.CurrentCulture.ThreeLetterISOLanguageName,
                iso = CultureInfo.CurrentCulture.TwoLetterISOLanguageName,
            });
        }

        /// <summary>
        /// Initialises the Localization Manager. The supported languages are defined in a file at "Content/local/_languages.json". This should be a dictionary of
        /// 2-digit language codes and language name.
        /// </summary>
        public static void Init()
        {
            try
            {
                GetSystemLocalization();

                //var langTxt = File.ReadAllText("Content/local/_languages.json");
                //m_supportedLangagues = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(langTxt);
                //m_supportedLangagues = vxIO.LoadJson<Dictionary<string, string>>("Content/local/_languages.json");
                m_supportedLangagues = vxContentManager.Instance.LoadJson<Dictionary<string, string>>("local/_languages");
            }
            catch { }

            _currentLocKey = "en";


            if (vxSettings.Language != "")
            {
                // finally apply the language
                SetLocalization(vxSettings.Language);
            }
            else
            {
                SetLocalization(_currentLocKey);
            }
        }



        /// <summary>
        /// Set's the current loca
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool SetLocalization(string key)
        {
            try
            {
                if (m_supportedLangagues.ContainsKey(key) == false)
                {
                    key = "en";
                }
                // load in json file
                _currentLocKey = key;

                //var locFilePath = $"Content/local/{key}.json";
                var locFilePath = $"local/{key}";
                //if (File.Exists(locFilePath))
                {
                    //var locTxt = File.ReadAllText(locFilePath);
                    //m_langaugeKeys = vxIO.LoadJson<Dictionary<string, string>>(locFilePath);
                    m_langaugeKeys = vxContentManager.Instance.LoadJson<Dictionary<string, string>>(locFilePath);

                    // now load the localised content
                    vxEngine.Game.LoadLocalisedContent(key);

                    // now tell all screens to reset their text and assets
                    vxSceneManager.OnLocalizationChanged();
                }

                vxSettings.Language = key;

                vxDebug.LogEngine(new
                {
                    key=key,
                    name=CurrentLanguage
                });

                return true;
            }
            catch(Exception ex)
            {
                vxDebug.Exception(ex);
                return false;
            }
        }

        /// <summary>
        /// Returns the localised text for the give key. If no key exists in the language pack, then it simply returns the key
        /// </summary>
        /// <param name="key"></param>
        #if DEBUG
        public static string GetText(string key, 
            [System.Runtime.CompilerServices.CallerMemberName] string caller = "", 
            [System.Runtime.CompilerServices.CallerFilePath] string callerFile = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int callerLine = 0)
            #else
        public static string GetText(string key)
#endif
        {
            if (key == string.Empty)
                return string.Empty;

            if(m_langaugeKeys.TryGetValue(key, out var txt))
            {
                return txt;
            }


#if DEBUG
            
            if (missingKeys.ContainsKey(key) == false)
                missingKeys.Add(key, key);

            vxConsole.WriteError($"Missing Loc Key for \"{key}\" for language {_currentLocKey} from: {caller} at {callerFile}:{callerLine}");

#else
            //vxConsole.WriteError($"Missing Loc Key for {key} for language {_currentLocKey}");

#endif
            return key;
        }

        static Dictionary<string, string> missingKeys = new Dictionary<string, string>();

        [VerticesEngine.Diagnostics.vxDebugMethod("localfix", "The dumps missing localization keys")]
        public static void DumpMissingKeys()
        {
            var filePath = Path.Combine(vxIO.PathToCacheFolder, "missingLocKeys.txt");

            string txt = "";
            foreach(var key in missingKeys.Values)
            {
                txt += key + "\n";
            }
            File.WriteAllText(filePath, txt);
        }
    }
}
