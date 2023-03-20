using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using VerticesEngine.Diagnostics;
using VerticesEngine.Graphics;


namespace VerticesEngine.ContentManagement
{
    /// <summary>
    /// Class which encorporates a number of different functions for asset loading and content management.
    /// </summary>
    public class vxContentManager
    {
        /// <summary>
        /// Content Manager Instance
        /// </summary>
        public static vxContentManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new vxContentManager();
                }

                return _instance;
            }
        }
        private static vxContentManager _instance;


		private Dictionary<string, vxMesh> m_cachedModels = new Dictionary<string, vxMesh>();

        private ContentManager mainContentManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:VerticesEngine.ContentManagement.vxContentManager"/> class for handling 
		/// internal assets.
		/// </summary>
		private vxContentManager()
        {
			mainContentManager = new ContentManager(vxEngine.Game.Services, "Content");
			m_activeContentManager = mainContentManager;
        }

		/// <summary>
		/// Loads a graphical asset
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="path"></param>
		/// <returns></returns>
		public T Load<T>(string path)
        {
			// When building the vxengine git submodule , MG puts the engine content in a funny folder for android and iOS
			// lets check that
			if(vxEngine.PlatformOS == vxPlatformOS.Android || vxEngine.PlatformOS == vxPlatformOS.iOS)
            {
				if(path.Contains("vxengine"))
                {
					path = "../" + path;
                }
            }

			try
			{
                if (typeof(T) == typeof(vxMesh))
                {
                    T meshObj = m_activeContentManager.Load<T>(path);

                    vxMesh mobj = (vxMesh)(object)meshObj;

                    mobj.UpdateBoundingBox();

                    //First Load in the Texture packs based off of the mesh name
                    LoadModelTextures(mobj, path, m_activeContentManager, "");

                    return meshObj;
                }
                else
                {
                    return m_activeContentManager.Load<T>(path);
                }
            }
			catch(Exception ex)
            {
				if(ex.InnerException != null)
					vxConsole.WriteError(ex.InnerException.Message);

				return m_activeContentManager.Load<T>(path);
			}
		}

		/// <summary>
		/// Loads a json content file.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="path"></param>
		/// <returns></returns>
		public T LoadJson<T>(string path)
        {
			var objString = Load<TextAsset>(path);

			var resultObj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(objString.text);

			return resultObj;
		}

		Dictionary<string, object> assets = new Dictionary<string, object>();

		// the active content manager, this can be replaced when a new scene starts up
		private ContentManager m_activeContentManager;

		public void SetActiveContentManager(ContentManager contentManager)
		{
            m_activeContentManager = contentManager;
        }


        /// <summary>
        /// This Loads Models at Run time performing a number of functions. See remarks for full details.
        /// </summary>
        /// <remarks>
        /// Model Loading
        /// =============================
        /// This loads a vxModel with a Specified Effect as well as applies the CascadeShadowEffect to 
        /// the vxModel's internal Shadow Model as well. XNA and potentially other back ends do not allow
        /// multiple loading of the same asset, therefore if a Shadow Model.xnb is not found, then it is created
        /// from a copy of the main model as 'mainmodelname_shdw.xnb'. 
        /// 
        /// 
        /// Texture Loading
        /// =============================
        /// Furthermore, Textures are loaded based off of the name of the model mesh name.
        /// 
        /// For Example
        /// -------------
        /// ModelMesh Name = "ship"
        /// 
        /// Then the content importer will look for textures under the following names:
        /// 
        /// Diffuse Texture:    ship_dds
        /// Normal Map:         ship_nm
        /// Specular Map:       ship_sm
        /// 
        /// The path to each of these is saved in the vxModel as well too allow for reloading of
        /// other resolution packs later on.
        /// </remarks>
        /// <returns>The loaded model.</returns>
        public vxMesh LoadMesh(string path)
        {
            var mesh = Load<vxMesh>(path);

            mesh.UpdateBoundingBox();

            //First Load in the Texture packs based off of the mesh name
            LoadModelTextures(mesh, path, m_activeContentManager, "");

            return mesh;
        }

        static Dictionary<string, Texture2D> textureCache = new Dictionary<string, Texture2D>();

        Texture2D LoadTexture(string path, ContentManager Content, bool IsBeingImported)
        {
            //if (textureCache.ContainsKey(path))
            //    return textureCache[path];

            Texture2D texture;

            if (IsBeingImported)
            {
#if __ANDROID__                
                string fPath = "Content/" + path + ".png";
                
                Stream fileStream = vxGame.Activity.Assets.Open("Content/" + path + ".png");
                return Texture2D.FromStream(vxGraphics.GraphicsDevice, fileStream);
#else
                using (var fileStream = new FileStream(path + ".png", FileMode.Open))
                {
                    texture = Texture2D.FromStream(vxGraphics.GraphicsDevice, fileStream);

                    if (texture.Name == null)
                        texture.Name = new FileInfo(path + ".png").Name;
                    //return txtr;
                }
#endif
            }
            else
            {
                texture = Load<Texture2D>(path);
            }

            return texture;
        }


        /// <summary>
        /// Loads the textures for a given model and/or mesh
        /// </summary>
        /// <param name="Content">Content.</param>
        /// <param name="TexturePath">Texture path.</param>
        public void LoadModelTextures(vxMesh model, string ModelPath, ContentManager Content, string TexturePath = "", bool IsBeingImported = false)
        {
            string rootDirectory = IsBeingImported ? "" : Content.RootDirectory + "/";
            string ext = IsBeingImported ? ".png" : ".xnb";
            if (TexturePath != "")
                TexturePath += "/";
                       

            //Load the Textures for hte Model Main.
            //foreach (ModelMesh mesh in ModelMain.Meshes)
            foreach (var mesh in model.Meshes)
            {
                Texture2D _diffusetexture;
                Texture2D _normalMap;
                Texture2D _surfaceMap;


                //First Create The Path to the Textures and Maps
                string modelParentPath = ModelPath.GetParentPathFromFilePath();
                string pathToDiffTexture = modelParentPath + "/" + TexturePath + mesh.Name + "_dds";
                string pathToNrmMpTexture = modelParentPath + "/" + TexturePath + mesh.Name + "_nm";
                string pathToSpecMpTexture = modelParentPath + "/" + TexturePath + mesh.Name + "_sm";
                string pathToDistMpTexture = modelParentPath + "/" + TexturePath + mesh.Name + "_dm";
                string pathToEmsvMpTexture = modelParentPath + "/" + TexturePath + mesh.Name + "_em";


                // Load/Create Diffuse Texture 
                //**************************************************************************************************************
                //First try to find the corresponding diffuse texture for this mesh, if it isn't found, then set the null texture as a fall back
                if (File.Exists(rootDirectory + pathToDiffTexture + ext))
                    _diffusetexture = LoadTexture(pathToDiffTexture, Content, IsBeingImported);
                else
                    _diffusetexture = vxInternalAssets.Textures.DefaultDiffuse;

                try
                {
                    _diffusetexture = LoadTexture(pathToDiffTexture, Content, IsBeingImported);
                }
                catch
                {
                    _diffusetexture = vxInternalAssets.Textures.DefaultDiffuse;
                }


                // only add it if it doesn't exist yet
                if (mesh.MeshTextures.ContainsKey(MeshTextureType.Diffuse) == false)
                    mesh.AddTexture(MeshTextureType.Diffuse, _diffusetexture);

                // Load/Create Normal Map 
                //**************************************************************************************************************                
                //First try to find the corresponding normal map texture for this mesh, if it isn't found, then set the null texture as a fall back
                if (File.Exists(rootDirectory + pathToNrmMpTexture + ext))
                {
                    _normalMap = LoadTexture(pathToNrmMpTexture, Content, IsBeingImported);
                }
                else
                {
                    _normalMap = vxInternalAssets.Textures.DefaultNormalMap;
                }

                if (mesh.MeshTextures.ContainsKey(MeshTextureType.NormalMap) == false)
                    mesh.AddTexture(MeshTextureType.NormalMap, _normalMap);

                // Load/Create Surface Map 
                //**************************************************************************************************************
                //First try to find the corresponding specular map texture for this mesh, if it isn't found, then set the null texture as a fall back
                if (File.Exists(rootDirectory + pathToSpecMpTexture + ext))
                    _surfaceMap = LoadTexture(pathToSpecMpTexture, Content, IsBeingImported);
                else
                    _surfaceMap = vxInternalAssets.Textures.DefaultSurfaceMap;


                if (_diffusetexture != null && _diffusetexture.Name.Length > 4)
                {
                    var rma_path = _diffusetexture.Name.Substring(0, _diffusetexture.Name.Length - 2) + "_rma";

                    if (File.Exists(rootDirectory + rma_path + ext))
                        _surfaceMap = LoadTexture(rma_path, Content, IsBeingImported);
                }

                mesh.AddTexture(MeshTextureType.RMAMap, _surfaceMap);

                // Load/Create Distortion Map 
                //**************************************************************************************************************
                //First try to find the corresponding normal map texture for this mesh, if it isn't found, then set it to just null as a fall back
                if (File.Exists(rootDirectory + pathToDistMpTexture + ext))
                {
                    mesh.AddTexture(MeshTextureType.DistortionMap, LoadTexture(pathToDistMpTexture, Content, IsBeingImported));
                }

                // Try to Load Emissive Map
                //**************************************************************************************************************
                //First try to find the corresponding normal map texture for this mesh, if it isn't found, then set it to just null as a fall back
                if (File.Exists(rootDirectory + pathToEmsvMpTexture + ext))
                {
                    mesh.AddTexture(MeshTextureType.EmissiveMap, LoadTexture(pathToEmsvMpTexture, Content, IsBeingImported));
                }

                // Now get the Primitive Count for Debuging Purposes.
                //TotalPrimitiveCount = vxGeometryHelper.GetModelPrimitiveCount(ModelMain);
            }
        }


        #region -- Debug Methods --

        [vxDebugMethod("content", "View the 'path' to the internal content embedded in the main engine 'dll'.")]
        static void ListInternalContent()
        {
            foreach (var path in System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames())
                vxConsole.WriteLine(path);
        }

#endregion
    }
}
