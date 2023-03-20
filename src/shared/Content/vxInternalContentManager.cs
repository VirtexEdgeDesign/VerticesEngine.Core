using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using VerticesEngine.Diagnostics;
using VerticesEngine.Graphics;

namespace VerticesEngine.ContentManagement
{
    /// <summary>
    /// This handles all internal asset loading for the Vertices Engine. This is only used for loading stock and embedded assets. If you'd like
    /// to load game content, then use <see cref="T:VerticesEngine.ContentManagement.vxContentManager"/>
    /// </summary>
	public class vxInternalContentManager : Microsoft.Xna.Framework.Content.ContentManager
    {
        /// <summary>
        /// Content Manager Instance
        /// </summary>
        public static vxInternalContentManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new vxInternalContentManager();
                }

                return _instance;
            }
        }
        private static vxInternalContentManager _instance;


        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.ContentManagement.vxInternalContentManager"/> class for handling 
        /// internal assets.
        /// </summary>
        /// <param name="Engine">Engine.</param>
        private vxInternalContentManager() : base(vxEngine.Game.Services, vxInternalAssets.PathToEngineContent)
        {

        }

        internal void Init()
        {
            //Initialise Internal Assets
            vxInternalAssets.Init();

            // Setup the 2D Asset Creator. This can generate Textures at runtime
            vxAssetCreator2D.Init();
        }



#if !__MOBILE__
        protected override Stream OpenStream(string assetName)
        {
            assetName = "VerticesEngine." + RootDirectory + "." + assetName.Replace('\\', '.').Replace('/', '.') + ".xnb";
            return System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(assetName);
        }
#endif


        /// <summary>
        /// Load a Model and apply the Main Shader Effect to it. Reffer to full-option override for full remarks.
        /// </summary>
        /// <param name="Path">The Model File Path</param>
        /// <param name="TexturePath">The subdirectory path to any textures pertaining to this model. 
        /// The default is the models directory.</param>
        /// <returns>A Model Object With the Main Shader Applied</returns>
        //public vxMesh LoadModel(string Path, string TexturePath = "")
        //{
        //    return LoadModel(Path, this, TexturePath);
        //}

        //      /// <summary>
        //      /// Load a Model and apply the Spefecifeid Shader Effect to it. Reffer to full-option override for full remarks.
        //      /// </summary>
        //      /// <returns>The model.</returns>
        //      /// <param name="Path">Path.</param>
        //      /// <param name="EffectToSet">Effect to set.</param>
        //      /// <param name="TexturePath">Texture path.</param>
        //      public vxMesh LoadModel(string Path, Effect EffectToSet, string TexturePath = "")
        //      {
        //          return LoadModel(Path, this, new vxMaterial(new vxShader(EffectToSet)), TexturePath);
        //      }

        ///// <summary>
        ///// Load a Model and apply the Main Shader Effect to it. Reffer to full-option override for full remarks.
        ///// </summary>
        ///// <param name="Path">The Model File Path</param>
        ///// <param name="Content">The Content Manager to load this model with.</param>
        ///// <param name="TexturePath">Path to the Texture files if it differs from the Models path.</param>
        ///// <returns>A Model Object With the Main Shader Applied</returns>
        //public vxMesh LoadModel(string Path, ContentManager Content, string TexturePath = "")
        //      {
        //	return LoadModel(Path, this, new vxMaterial(new vxShader(vxInternalAssets.Shaders.MainShader)), TexturePath);
        //}


        ///// <summary>
        ///// Load a Model and apply a Custom Shader Effect to it. Reffer to full-option override for full remarks.
        ///// </summary>
        ///// <returns>The model.</returns>
        ///// <param name="Path">Path.</param>
        ///// <param name="Content">The Content Manager to load this model with.</param>
        ///// <param name="EffectToSet">Effect to set.</param>
        ///// <param name="TexturePath">Path to the Texture files if it differs from the Models path.</param>
        //public vxMesh LoadModel(string Path, ContentManager Content, Effect EffectToSet, string TexturePath = "")
        //      {
        //	return LoadModel(Path, this, new vxMaterial(new vxShader(EffectToSet)), TexturePath);
        //}


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
        /// <param name="PathToModel">Path to model.</param>
        /// <param name="Content">The Content Manager to load this model with.</param>
        /// <param name="EffectToSet">The Main Effect to set.</param>
        /// <param name="ShadowEffect">Shadow effect.</param>
        /// <param name="UtilityEffect">Utility effect.</param>
        /// <param name="TexturePath">Path to the Texture files if it differs from the Models path.</param>
  //      public vxMesh LoadModel(string PathToModel, ContentManager Content, string TexturePath = "")
  //      {
		//	vxConsole.WriteVerboseLine("     Importing Model: " + PathToModel);

		//	// Create the Model Object to return
		//	var newModel = new vxMesh(PathToModel);
            
		//	// Next Load in the Main Model.
		//	var mgModel = Content.Load<Model>(PathToModel);

  //          // Now extract the vertice info
  //          var tempModelMesh = new vxModelMesh();

		//	// Load the Textures for hte Model Main.
		//	foreach (ModelMesh mesh in mgModel.Meshes)
		//	{
		//		tempModelMesh.Name = mesh.Name;
  //              foreach (var effect in mesh.Effects)
  //              {
  //                  if (effect is BasicEffect)
  //                  {
  //                      var beffect = (BasicEffect)effect;

  //                      if (beffect.Texture != null)
  //                      {
  //                          // copy over the default texture
  //                          //tempModelMesh.Material.Texture = beffect.Texture;
  //                      }
  //                  }
  //              }
  //              foreach (ModelMeshPart part in mesh.MeshParts)
		//		{
		//			if (part.Tag == null)
		//				part.Tag = mesh;
					
  //                  var tempModelMeshPart = new vxModelMeshPart(PathToModel, part);
		//			tempModelMeshPart.Tag = part.Tag;
		//			tempModelMesh.MeshParts.Add(tempModelMeshPart);
		//		}

  //              newModel.AddModelMesh(tempModelMesh);
		//		tempModelMesh = new vxModelMesh();
  //          }

  //          //First Load in the Texture packs based off of the mesh name
  //          //newModel.LoadTextures(Content, TexturePath);

  //          // Add a tag of the Path to the model for debuging
  //          //newModel.ModelMain.Tag = PathToModel;

  //          return newModel;
		//}



		///// <summary>
		///// Loads the basic effect model.
		///// </summary>
		///// <returns>The basic effect model.</returns>
		///// <param name="PathToModel">Path to model.</param>
		//public vxMesh LoadBasicEffectModel(string PathToModel)
		//{
		//	return LoadBasicEffectModel(PathToModel, vxEngine.CurrentGame.Content);
		//}

		///// <summary>
		///// Loads the basic effect model.
		///// </summary>
		///// <returns>The basic effect model.</returns>
		///// <param name="PathToModel">Path to model.</param>
		///// <param name="Content">Content.</param>
		//public vxMesh LoadBasicEffectModel(string PathToModel, ContentManager Content)
		//{
		//	return LoadModelWithBasicEffect(PathToModel, Content);
		//}



		///// <summary>
		///// Loads the model with the BasicEffect.
		///// </summary>
		///// <returns>The basic effect model.</returns>
		///// <param name="PathToModel">Path to model.</param>
		///// <param name="Content">Content.</param>
		///// <param name="ShadowEffect">Shadow effect.</param>
		///// <param name="UtilityEffect">Utility effect.</param>
		//public vxMesh LoadModelWithBasicEffect(string PathToModel, ContentManager Content, string TexturePath = "")
		//{
  //          return LoadModel(PathToModel, Content, new BasicEffect(vxGraphics.GraphicsDevice), TexturePath);
		//}

        #region -- Debug Methods --

        [vxDebugMethod("content-engine", "View the 'path' to the internal content embedded in the main engine 'dll'.")]
        static void ListInternalContent()
        {
            foreach (var path in System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames())
                vxConsole.WriteLine(path);
        }

        #endregion
    }
}
