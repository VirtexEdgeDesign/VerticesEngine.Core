using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using VerticesEngine.ContentManagement;
using VerticesEngine.Input;

namespace VerticesEngine.EnvTerrain
{
    public class vxTerrainManager
    {
        public static vxTerrainManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new vxTerrainManager();
                }

                return _instance;
            }
        }
        static vxTerrainManager _instance;
        

        public List<Texture2D> Textures = new List<Texture2D>();


        public Texture2D InitialHeightMap;
        
        public int TextureMapSize = 256;


        /// <summary>
        /// A list of all the Terrains in the current scene
        /// </summary>
        public List<vxTerrainChunk> Terrains = new List<vxTerrainChunk>();


        public vxEnumTerrainEditMode EditMode;



        #region Cursor Indo

        public Texture2D CursorTexture
        {
            get { return _cursorTexture; }
            set
            {
                _cursorTexture = value;
                //TerrainMesh.Effect.Parameters["CursorMap"].SetValue(value);
            }
        }
        Texture2D _cursorTexture;

        public float CursorScale
        {
            get { return _cursorScale; }
            set
            {
                _cursorScale = value;
                //TerrainMesh.Effect.Parameters["CursorScale"].SetValue(value);
            }
        }
        float _cursorScale;



        public Vector2 CursorPosition
        {
            get { return _cursorPosition; }
            set
            {
                _cursorPosition = value;
                //TerrainMesh.Effect.Parameters["CursorPosition"].SetValue((value - Position.ToVector2()) / CellSize);
            }
        }
        Vector2 _cursorPosition;



        public Color CursorColour
        {
            get { return _cursorColour; }
            set
            {
                _cursorColour = value;
                //TerrainMesh.Effect.Parameters["CursorColour"].SetValue(value.ToVector4());
            }
        }
        Color _cursorColour;


        public bool IsInEditMode
        {
            get { return _isInEditMode; }
        }
        bool _isInEditMode;


        public Texture2D TextureBrush;

        //Vector2 OffsetPosition;

        /// <summary>
        /// The current scene of the game
        /// </summary>
        private vxGameplayScene3D Scene;

        #endregion  


        private vxTerrainManager()
        {
            // load the base pack
            LoadTexturePack("vxengine/textures/terrain/txtrs", true);

            InitialHeightMap = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/terrain/Heightmap");


            // Setup Cursor and Brushes
            CursorColour = Color.DeepSkyBlue;
            CursorTexture = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/terrain/cursor/cursor");
            TextureBrush = vxInternalAssets.Textures.Blank;


            EditMode = vxEnumTerrainEditMode.Disabled;

            Scene = vxEngine.Instance.GetCurrentScene<vxGameplayScene3D>();
        }

        /// <summary>
        /// Loads a Texture Pack using the speciefied path. Each texture must have the notation of
        /// 'texture_i' where 'i' is the texture index.
        /// </summary>
        /// <param name="path">path to the four textures.</param>
        public void LoadTexturePack(string path, bool UseEngineContentManager = false)
        {
            //ContentManager Content = vxEngine.Instance.CurrentScene.SceneContent;

            //if (UseEngineContentManager)
            //    Content = vxContentManager.Instance;

            Textures.Clear();

            for (int i = 0; i < 4; i++)
            {
                Textures.Add(vxContentManager.Instance.Load<Texture2D>(Path.Combine(path, "texture_" + i.ToString())));
            }                

            UpdateTextures();
        }

        /// <summary>
        /// Adds a Terrain to the terrain manager.
        /// </summary>
        /// <param name="terrain"></param>
        public void Add(vxTerrainChunk terrain)
        {
            // Set the Textures
            SetTextures(terrain);

            Terrains.Add(terrain);
        }

        void UpdateTextures()
        {
            foreach (vxTerrainChunk terrain in Terrains)
                SetTextures(terrain);
        }


        void SetTextures(vxTerrainChunk terrain)
        {
            terrain.Texture01 = Textures[0];
            terrain.Texture02 = Textures[1];
            terrain.Texture03 = Textures[2];
            terrain.Texture04 = Textures[3];
        }

        public void Update()
        {
            Scene = vxEngine.Instance.GetCurrentScene<vxGameplayScene3D>();

            _isInEditMode = (Scene.SandboxEditMode == vxEnumSanboxEditMode.TerrainEdit);
            
            if (IsInEditMode)
            {
                vxTerrainManager.Instance.CursorPosition = Scene.Intersection.ToVector2();// - new Vector2(CursorScale * CellSize / 4);// - Position.ToVector2();
                
                // Set Scroll Size when shift key is down
                if (vxInput.IsKeyDown(Keys.LeftShift))
                {
                    if (vxInput.ScrollWheelDelta > 0)
                        vxTerrainManager.Instance.CursorScale -= 0.25f + vxTerrainManager.Instance.CursorScale / 10;
                    else if (vxInput.ScrollWheelDelta < 0)
                        vxTerrainManager.Instance.CursorScale += 0.25f + vxTerrainManager.Instance.CursorScale / 10;
                }
                // Set and Clamp the Cursor Scale
                vxTerrainManager.Instance.CursorScale = MathHelper.Clamp(vxTerrainManager.Instance.CursorScale, 4, float.MaxValue);

                // Set the Base version of the Cursor Colour
                vxTerrainManager.Instance.CursorColour = Color.DeepSkyBlue;
            }
        }
    }
}