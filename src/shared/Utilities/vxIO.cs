using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.IO.Compression;
using Microsoft.Xna.Framework;
using VerticesEngine;
using VerticesEngine.Graphics;
using VerticesEngine.UI.MessageBoxs;
using VerticesEngine.UI;
using System.Text.RegularExpressions;
#if __ANDROID__
using Android.Views;
using Android.Content;
#endif

namespace VerticesEngine.Utilities
{
	/// <summary>
	/// Collection of static utility methods.
	/// </summary>
    public static class vxIO
    {
        /// <summary>
        /// Gets the log directory.
        /// </summary>
        /// <value>The log directory.</value>
        public static string LogDirectory
        {
            get
            {
                string path = "Logs";
                path = Path.Combine(PathToRoot, path);
                return path;
            }
        }

        static string virtexrootfolder = "My Games";

        static string GameName
        {
            get { return vxEngine.Game.Name; }
        }


        internal static void Init()
        {
            //GameName = vxEngine.Game.Name;

            // a release of vertices went out with bad paths, this is to check for that
            //string verticePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), virtexrootfolder, "VerticesEngine");

            //if(Directory.Exists(verticePath))
            //{
            //    Directory.Move(verticePath, PathToRoot);
            //}

            CheckDirExist();
        }

        /// <summary>
        /// This checks if the appropriate directories exist, but only fires after the permissions have been requested on the proper platforms.
        /// </summary>
        /// <param name="engine"></param>
        static void CheckDirExist()
        {
            EnsureDirExists(PathToSettings);
            EnsureDirExists(PathToCacheFolder);
            EnsureDirExists(PathToSandbox);
            EnsureDirExists(PathToTempFolder);
            EnsureDirExists(PathToMods);
            EnsureDirExists(LogDirectory);
        }

        public static void EnsureDirExists(string path)
        {
            try
            {
                if (Directory.Exists(path) == false)
                    Directory.CreateDirectory(path);
            }
            catch(Exception ex)
            {
                vxConsole.WriteException(ex);
            }
        }

        /// <summary>
        /// The path to the game data folder.
        /// Android this specifies the app specific directory under '/storage/emulated/0/Android/data/com.yourcompany.yourappid/files'. This will be 
        /// deleted if your app is ever removed. If there are settings or files which you want to keep then you should save them to the External Storage Directory.
        /// You can do so by calling:
        /// <code>
        /// string externalPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, virtexrootfolder , GameName)
        /// </code>
        /// </summary>
        public static string PathToRoot
        {
            get
            {
#if __ANDROID__
                //var pt = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, virtexrootfolder , GameName);
                //var pt = Game.Activity.GetExternalFilesDir(string.Empty).AbsolutePath;
                if(string.IsNullOrEmpty(_pathToRoot))
                {
                    _pathToRoot = Game.Activity.GetExternalFilesDir(string.Empty).AbsolutePath;
                }

//#elif __IOS__
//                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), virtexrootfolder, GameName);
#else
                if (string.IsNullOrEmpty(_pathToRoot))
                {
                    _pathToRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), virtexrootfolder, GameName);
                }
#endif
                return _pathToRoot;
            }
        }
        private static string _pathToRoot = string.Empty;


        public static string PathToRootExternal
        {
            get
            {
#if __ANDROID__
                return Game.Activity.BaseContext.GetExternalFilesDir("").AbsolutePath;
                var pt = Path.Combine(Game.Activity.BaseContext.GetExternalFilesDir("").AbsolutePath, virtexrootfolder , GameName);
                return pt;
#else
                return PathToRoot;
#endif
            }
        }
        

        /// <summary>
        /// The path to settings.
        /// </summary>
        public static string PathToSettings
        {
            get
            {
                string path = "Settings";

                path = Path.Combine(PathToRoot, path);
                return path;
            }
        }

        /// <summary>
        /// The path to profile saved data
        /// </summary>
        public static string PathToProfiles
        {
            get
            {
                string path = "Profiles";

                path = Path.Combine(PathToRoot, path);
                return path;
            }
        }


        /// <summary>
        /// The path to sandbox.
        /// </summary>
        public static string PathToSandbox
        {
            get
            {
                if (_sandboxPath == string.Empty)
                    _sandboxPath = Path.Combine(PathToRootExternal, "Sandbox");

                // always make sure that the sandbox exists
                EnsureDirExists(_sandboxPath);
                return _sandboxPath;
            }
        }
        static string _sandboxPath = string.Empty;

        static List<string> permissions = new List<string>();

        public static bool IsPermissionsRequestRequired()
        {
#if __ANDROID__
            permissions.Clear();
            //permissions.Add(Android.Manifest.Permission.GetAccounts);
            permissions.Add(Android.Manifest.Permission.WriteExternalStorage);
            permissions.Add(Android.Manifest.Permission.ReadExternalStorage);

            foreach (var permission in permissions)
            {
                vxConsole.WriteLine("checking " + permission);
                if (AndroidX.Core.App.ActivityCompat.CheckSelfPermission(Game.Activity, permission) == Android.Content.PM.Permission.Denied)
                {
                    vxConsole.WriteLine("DENIED!");
                    return true;
                }

                vxConsole.WriteLine("Granted!");
            }

            return false;
#else
            // other platforms don't need permission
            return false;
#endif
        }

        public static vxMessageBox OnShowPermissionRequestMessage()
        {
            return vxMessageBox.Show("Permissions",
                string.Format(
                "{0} Requires\npermissions to save sandbox files \nto your device.", GameName)
                , vxEnumButtonTypes.OkCancel);
        }

        public static void RequestPermissions()
        {
#if __ANDROID__
            AndroidX.Core.App.ActivityCompat.RequestPermissions(Game.Activity, permissions.ToArray(), 0);
#endif
        }


        /// <summary>
        /// The path to temp folder.
        /// </summary>
        public static string PathToTempFolder
        {
            get
            {
                return Path.Combine(PathToRoot, "Temp");
            }
        }

        public static string PathToImportFolder
        {
            get
            {
                return Path.Combine(PathToTempFolder, "imported");
            }
        }

        /// <summary>
        /// Cache folder which can be cleared sometimes
        /// </summary>
        public static string PathToCacheFolder
        {
            get
            {
                return Path.Combine(PathToRoot, "Cache");
            }
        }

        /// <summary>
        /// The path to localization files.
        /// </summary>
        public static string PathToLocalizationFiles
        {
            get
            {
                return Path.Combine(ContentRootPath, "local");
            }
        }



        public static string PathToMods
        {
            get
            {
                return Path.Combine(PathToRoot, "Mods");
            }
        }


        public static string ContentRootPath
        {
            get
            {
                var filePath = "Content";

                // OSX needs to move up a directory
                if (vxEngine.PlatformOS == vxPlatformOS.OSX && Directory.Exists("../Resources"))
                {
                    filePath = Path.Combine("../Resources", filePath);
                    vxConsole.WriteLine("USING OSX RESOURCE FOLDER");
                }
                return filePath;
            }
        }

        private static bool m_isTempDirectoryClearEnabled = true;

        /// <summary>
        /// Clears the temp directory.
        /// </summary>
        public static void ClearTempDirectory()
        {
            if (m_isTempDirectoryClearEnabled)
            {
                //Clear Out the Temp Directory
                DirectoryInfo tempDirectory = new DirectoryInfo(PathToTempFolder);

                try
                {
                    foreach (FileInfo file in tempDirectory.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in tempDirectory.GetDirectories())
                    {
                        try
                        {
                            dir.Delete(true);
                        }
                        catch
                        {
                        }
                    }
                }
                catch { }
            }
        }

        public static StreamReader LoadTextFile(string path)
        {
#if __ANDROID__
            return new StreamReader(TitleContainer.OpenStream(path));
#else
           return new StreamReader(path);
#endif
        }

        /// <summary>
        /// Loads a content file json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T LoadJson<T>(string path)
        {
            
#if __ANDROID__
            StreamReader reader = new StreamReader(TitleContainer.OpenStream(path));
#else
            StreamReader reader = new StreamReader((path));
#endif

            string json = reader.ReadToEnd();

            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            return obj;

        }

        /// <summary>
        /// Saves a given object to json
        /// </summary>
        /// <param name="path">The path to save too</param>
        /// <param name="obj">The object to serialise</param>
        public static void SaveJson(string path, object obj, bool IsIndented = true)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj, IsIndented ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None);

            File.WriteAllText(path, json);
        }


        /// <summary>
        /// Load an Image from it's path. this is not for *.xnb files. Note this loads from the content directory
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Texture2D LoadImage(string path, bool isContentImg = true)
        {
            Texture2D texture = null;// vxInternalAssets.Textures.DefaultDiffuse;
            try
            {
#if __ANDROID__
                Stream fileStream = Game.Activity.Assets.Open("Content/" + path + ".png");
                texture = Texture2D.FromStream(vxGraphics.GraphicsDevice, fileStream);

#elif __IOS__
                var filePath = "Content/" + path + ".png";

                using (var fileStream = TitleContainer.OpenStream(filePath))
                {
                    texture = Texture2D.FromStream(vxGraphics.GraphicsDevice, fileStream);
                }

#else
                var filePath = "Content/" + path + ".png";
                if (vxEngine.PlatformOS == vxPlatformOS.OSX && Directory.Exists("../Resources"))
                    filePath = "../Resources/" + filePath;

                if (isContentImg == false)
                    filePath = path;

                using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Open))
                {
                    texture = Texture2D.FromStream(vxGraphics.GraphicsDevice, fileStream);
                    texture.Name = fileStream.Name;
                }
#endif
            }
            catch(Exception ex)
            {
                vxConsole.WriteException("vxIO.LoadImage(...)", ex);
            }
            return texture;
            /*
            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                Texture2D texture = Texture2D.FromStream(vxGraphics.GraphicsDevice, fileStream);
                fileStream.Close();
                return texture;
            }*/

            //using (var fileStream = TitleContainer.OpenStream(path))
            //{
            //    Texture2D texture = Texture2D.FromStream(vxGraphics.GraphicsDevice, fileStream);
            //    fileStream.Close();
            //    return texture;
            //}
        }

#region File Compression

        public delegate void ProgressDelegate(string sMessage);

        public static void CompressFile(string sDir, string sRelativePath, GZipStream zipStream)
        {
            //Compress file name
            char[] chars = sRelativePath.ToCharArray();
            zipStream.Write(BitConverter.GetBytes(chars.Length), 0, sizeof(int));
            foreach (char c in chars)
                zipStream.Write(BitConverter.GetBytes(c), 0, sizeof(char));

            //Compress file content
            byte[] bytes = File.ReadAllBytes(Path.Combine(sDir, sRelativePath));
            zipStream.Write(BitConverter.GetBytes(bytes.Length), 0, sizeof(int));
            zipStream.Write(bytes, 0, bytes.Length);
        }

        public static bool DecompressFile(string sDir, GZipStream zipStream, ProgressDelegate progress)
        {
            //Decompress file name
            byte[] bytes = new byte[sizeof(int)];
            int Readed = zipStream.Read(bytes, 0, sizeof(int));
            if (Readed < sizeof(int))
                return false;

            int iNameLen = BitConverter.ToInt32(bytes, 0);
            bytes = new byte[sizeof(char)];
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < iNameLen; i++)
            {
                zipStream.Read(bytes, 0, sizeof(char));
                char c = BitConverter.ToChar(bytes, 0);
                sb.Append(c);
            }
            string sFileName = sb.ToString();
            if (progress != null)
                progress(sFileName);

            //Decompress file content
            bytes = new byte[sizeof(int)];
            zipStream.Read(bytes, 0, sizeof(int));
            int iFileLen = BitConverter.ToInt32(bytes, 0);

            bytes = new byte[iFileLen];
            zipStream.Read(bytes, 0, bytes.Length);

            string sFilePath = Path.Combine(sDir, sFileName);
            string sFinalDir = Path.GetDirectoryName(sFilePath);
            if (!Directory.Exists(sFinalDir))
                Directory.CreateDirectory(sFinalDir);
            TryAgain:

            try
            {
                using (FileStream outFile = new FileStream(sFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                    outFile.Write(bytes, 0, iFileLen);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                goto TryAgain;
            }
            //Console.WriteLine("Decompressed too: "+sFinalDir);
            return true;
        }

        public static void CompressDirectory(string sInDir, string sOutFile, ProgressDelegate progress)
        {
            string[] sFiles = Directory.GetFiles(sInDir, "*.*", SearchOption.AllDirectories);
            int iDirLen = sInDir[sInDir.Length - 1] == Path.DirectorySeparatorChar ? sInDir.Length : sInDir.Length + 1;

            using (FileStream outFile = new FileStream(sOutFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            using (GZipStream str = new GZipStream(outFile, CompressionMode.Compress))
                foreach (string sFilePath in sFiles)
                {
                    string sRelativePath = sFilePath.Substring(iDirLen);
                    if (progress != null)
                        progress(sRelativePath);
                    CompressFile(sInDir, sRelativePath, str);
                }
        }

        public static void DecompressToDirectory(string sCompressedFile, string sDir, ProgressDelegate progress, bool IsContentFile = true)
		{
#if __ANDROID__
            //if (GamePlayType == GamePlayType.GamePlay)
            //{
            //    Stream reader = Game.Activity.Assets.Open(this.FilePath);
            //    LevelFile = (CartoonPhysicsLevelFile)deserializer.Deserialize(reader);
            //    reader.Close();
            //}
            //else
            //{
            //StreamReader reader = new StreamReader(this.FilePath);
            //LevelFile = (CartoonPhysicsLevelFile)deserializer.Deserialize(reader);
            //reader.Close();
            //}
            if (IsContentFile)
            {
                using (Stream inFile = Game.Activity.Assets.Open(sCompressedFile))
                using (GZipStream zipStream = new GZipStream(inFile, CompressionMode.Decompress, true))
                    while (DecompressFile(sDir, zipStream, progress));
            }
            else
			{
				using (FileStream inFile = new FileStream(sCompressedFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				using (GZipStream zipStream = new GZipStream(inFile, CompressionMode.Decompress, true))
					while (DecompressFile(sDir, zipStream, progress)) ;
            }
#else
            using (FileStream inFile = new FileStream(sCompressedFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (GZipStream zipStream = new GZipStream(inFile, CompressionMode.Decompress, true))
                while (DecompressFile(sDir, zipStream, progress)) ;
#endif
		}


        /// <summary>
        /// Copies a folder over to another
        /// </summary>
        /// <param name="SourcePath"></param>
        /// <param name="DestinationPath"></param>
        /// <param name="filePattern"></param>
        public static void CopyDirectory(string SourcePath, string DestinationPath, string filePattern)
        {
            Directory.CreateDirectory(DestinationPath);

            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(SourcePath, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));

            //Copy all the files & Replaces any files with the same name
            var files = Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories).Where(file => Regex.IsMatch(file, @"^.+\.(obj|png|mtl)$"));
            foreach (string imageFile in files)
                File.Copy(imageFile, imageFile.Replace(SourcePath, DestinationPath), true);
        }




        #endregion
    }
}
