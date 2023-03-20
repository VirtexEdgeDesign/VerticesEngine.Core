
using BEPUphysics.BroadPhaseEntries;
using BEPUutilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using VerticesEngine;
using VerticesEngine.ContentManagement;
using VerticesEngine.Entities.Terrain;
using VerticesEngine.Graphics;
using VerticesEngine.Input;
using VerticesEngine.Serilization;

namespace VerticesEngine.EnvTerrain
{
    public class vxTerrainChunk : vxEntity3D
    {
        int dimension = 128;

        float _cellSize = 8;

        #region Serialization
        
        /// <summary>
        /// This holds the Terrain Data to be saved.
        /// </summary>
        public vxSerializableTerrainData TerrainData = new vxSerializableTerrainData();

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.Entities.vxTerrainEntity"/> class.
        /// </summary>
        public vxTerrainChunk()
        {
           Transform.Scale = new Vector3(5);
        }

        public Texture2D Texture01 { set
            {
                if (TerrainMaterial.Shader?.Parameters["Texture01"] != null)
                    TerrainMaterial.Shader.Parameters["Texture01"].SetValue(value); }
        }
        public Texture2D Texture02 { set
            {
                if (TerrainMaterial.Shader?.Parameters["Texture02"] != null)
                    TerrainMaterial.Shader.Parameters["Texture02"].SetValue(value); } }
        public Texture2D Texture03 { set
            {
                if (TerrainMaterial.Shader?.Parameters["Texture03"] != null)
                    TerrainMaterial.Shader.Parameters["Texture03"].SetValue(value); } }
        public Texture2D Texture04 { set
            {
                if (TerrainMaterial.Shader?.Parameters["Texture04"] != null)
                    TerrainMaterial.Shader.Parameters["Texture04"].SetValue(value); } }

        public Texture2D CursorTexture
        {
            get { return _cursorTexture; }
            set
            {
                _cursorTexture = value;
                if (TerrainMaterial.Shader?.Parameters["CursorMap"] != null)
                    TerrainMaterial.Shader.Parameters["CursorMap"].SetValue(value);
            }
        }
        Texture2D _cursorTexture;

        public float CursorScale
        {
            get { return _cursorScale; }
            set
            {
                _cursorScale = value;
                if (TerrainMaterial.Shader.Parameters["CursorScale"] != null)
                    TerrainMaterial.Shader.Parameters["CursorScale"].SetValue(value);
            }
        }
        float _cursorScale;



        public Vector2 CursorPosition
        {
            get { return _cursorPosition; }
            set
            {
                _cursorPosition = value;
                if(TerrainMaterial.Shader.Parameters["CursorPosition"] != null)
                TerrainMaterial.Shader.Parameters["CursorPosition"].SetValue((value - Position.ToVector2()) / _cellSize);
            }
        }
        Vector2 _cursorPosition;



        public Color CursorColour
        {
            get { return _cursorColour; }
            set
            {
                _cursorColour = value;
                if (TerrainMaterial.Shader.Parameters["CursorColour"] != null)
                    TerrainMaterial.Shader.Parameters["CursorColour"].SetValue(value.ToVector4());
            }
        }
        Color _cursorColour;

        float TextureUVScale
        {
            get { return _textureUVScale; }
            set
            {
                _textureUVScale = value;
                if (TerrainMaterial.Shader.Parameters["TxtrUVScale"] != null)
                    TerrainMaterial.Shader.Parameters["TxtrUVScale"].SetValue(value);
            }
        }
        float _textureUVScale;


        float MaxHeight
        {
            get { return _maxHeight; }
            set
            {
                _maxHeight = value;
                if (TerrainMaterial.Shader.Parameters["maxHeight"] != null)
                    TerrainMaterial.Shader.Parameters["maxHeight"].SetValue(value);
            }
        }
        float _maxHeight;

        public bool IsInEditMode
        {
            get { return _isInEditMode; }
            set
            {
                _isInEditMode = value;
                if (TerrainMaterial.Shader.Parameters["IsEditMode"] != null)
                    TerrainMaterial.Shader.Parameters["IsEditMode"].SetValue(_isInEditMode);
            }
        }
        bool _isInEditMode;

        float Minheight = -200;


        // a check to see if the entire buffer needs an update
        bool BufferNeedsFullUpdate = false;

        public virtual Texture2D GetInitialHeightMap()
        {
            return vxContentManager.Instance.Load<Texture2D>("vxengine/textures/terrain/Heightmap");
        }

        vxTerrainMaterial TerrainMaterial;

        protected override vxMesh OnLoadModel()
        {
            vxMesh model = new vxMesh("terrain");

            TerrainMaterial = new vxTerrainMaterial();
            model.AddModelMesh(new vxTerrainModelMesh(this));
            model.Meshes[0].MeshParts.Add(new vxTerrainMeshPart(GetInitialHeightMap(), (int)_cellSize));

            model.UpdateBoundingBox();
            return model;
        }

        protected override vxMaterial OnMapMaterialToMesh(vxModelMesh mesh)
        {
            return TerrainMaterial;
        }

        protected override vxEntityRenderer CreateRenderer()
        {
            return AddComponent<vxTerrainRenderer>();
        }

        public vxTerrainMeshPart TerrainMesh
        {
            get { return (vxTerrainMeshPart)Model.Meshes[0].MeshParts[0]; }
        }


        public override void OnBeforeEntitySerialize()
        {
            base.OnBeforeEntitySerialize();

            TerrainData = new vxSerializableTerrainData(
                this.Id,
                this.ToString(),
                this.Transform.Matrix4x4Transform,
                dimension,
                _cellSize);

            TerrainData.Dimension = dimension;
            TerrainData.CellSize = _cellSize;

            TerrainData.HeightData.Clear();

            foreach (vxMeshVertex vertex in TerrainMesh.MeshVertices)
                TerrainData.HeightData.Add(
                    new vxSerializableTerrainVertex(vertex.Position, 0.5f));

        }


        public override void OnAfterEntityDeserialized()
        {
            base.OnAfterEntityDeserialized();

            foreach (vxSerializableTerrainVertex vertex in TerrainData.HeightData)
            {
                int x = (int)(vertex.X / TerrainData.CellSize);
                int z = (int)(vertex.Z / TerrainData.CellSize);

                // Set the Height
                TerrainMesh[x, z] = vertex.Y;
            }

            TerrainMesh.CalculateNormals();
            TerrainMesh.UpdateDynamicVertexBuffer();

            //OnAdded();
        }


        protected override void OnFirstUpdate()
        {
            base.OnFirstUpdate();

            // Add into the Terrain manager on the first loop
            vxTerrainManager.Instance.Add(this);
        }

        protected internal override void Update()
        {
            base.Update();


            // Set whether it's in Edit mode or not
            IsInEditMode = (Scene.SandboxEditMode == vxEnumSanboxEditMode.TerrainEdit);
            
            if (IsInEditMode)
            {
                CursorTexture = vxTerrainManager.Instance.CursorTexture;
                CursorPosition = vxTerrainManager.Instance.CursorPosition;
                CursorScale = vxTerrainManager.Instance.CursorScale;

                // The Modify Vector Factor, simple factor to handle Additive or Subtractive Modeling
                int ModifyVectorFactor = 0;

                if (vxInput.IsKeyDown(Keys.LeftShift))
                {
                    if (vxInput.MouseState.LeftButton == ButtonState.Pressed)
                    {
                        ModifyVectorFactor = 1;
                    }

                    if (vxInput.MouseState.RightButton == ButtonState.Pressed)
                    {
                        ModifyVectorFactor = -1;
                    }
                }
                CursorColour = vxTerrainManager.Instance.CursorColour;

                // Handle Input
                if (ModifyVectorFactor != 0)
                {
                    BufferNeedsFullUpdate = false;

                    switch (Scene.TerrainEditState)
                    {
                        // Handle Sculpt Mode
                        case vxEnumTerrainEditMode.Sculpt:

                            // Set Colour Based on Modify Vector
                            CursorColour = ModifyVectorFactor > 0 ? Color.DarkOrange : Color.Magenta;

                            // Gets the location of the Center Point in Relation to the Height Map
                            Point HeightMapRelativeLocation = GetHeightLocation(vxTerrainManager.Instance.CursorPosition);

                            int Radius = (int)(vxTerrainManager.Instance.CursorScale) / 2;

                            for (int x = HeightMapRelativeLocation.X - Radius + 2; x < HeightMapRelativeLocation.X + Radius; x++)
                            {
                                for (int y = HeightMapRelativeLocation.Y - Radius + 2; y < HeightMapRelativeLocation.Y + Radius; y++)
                                {
                                    Point point = new Point(x, y);

                                    if (IsInBounds(point))
                                    {
                                        // This is the Distance from the center
                                        float disFac = (vxTerrainManager.Instance.CursorPosition - GetWorldLocation(point)).Length() / (Radius * _cellSize);

                                        // This is a linearised value of the distance this point is from the "center" point.
                                        // Essentially controling the amount of change. As a basic instantiation, it does a linear factor first.
                                        float DistanceFactor = MathHelper.Clamp(1 - disFac, 0, 1);


                                        switch (Scene.FalloffRate)
                                        {
                                            case vxEnumFalloffRate.Flat:
                                                DistanceFactor = 1;
                                                break;
                                            case vxEnumFalloffRate.Linear:
                                                // Do nothing, this is the start case

                                                break;
                                            case vxEnumFalloffRate.Smooth:
                                                //Console.WriteLine(string.Format("{0} : {1} ", i, Math.Atan(j)));

                                                // the funtion y = (sin(x - 3.14159 / 2) + 1) / 2 gives a shape which 
                                                // is smooth from y = 0 to y = 1 for
                                                // x = 0 too x = 2 * pi.

                                                float xi = (float)Math.PI * DistanceFactor;
                                                float yi = (float)(Math.Sin(xi - Math.PI / 2) + 1) / 2;

                                                //DistanceFactor = (float)Math.Sin(DistanceFactor * (float)Math.PI / 2);
                                                //float x = 
                                                DistanceFactor = yi;
                                                break;
                                        }

                                        switch (Scene.AreaOfEffectMode)
                                        {
                                            case vxEnumAreaOfEffectMode.Delta:
                                                // Do nothing, this is the basic value.
                                                break;

                                            case vxEnumAreaOfEffectMode.Averaged:

                                                // First get the height distance between current location
                                                // and the center point.
                                                //float CenterHeight = TerrainMesh.Vertices[HeightMapRelativeLocation.X * (Dimension + 1) + HeightMapRelativeLocation.Y].Position.Y;
                                                //float ThisHeight = TerrainMesh.Vertices[x * (Dimension + 1) + y].Position.Y;
                                                float CenterHeight = TerrainMesh[HeightMapRelativeLocation.X, HeightMapRelativeLocation.Y];
                                                float ThisHeight = TerrainMesh[x, y];

                                                float HeightDelta = (CenterHeight - ThisHeight) * DistanceFactor / 10;

                                                DistanceFactor *= HeightDelta;

                                                break;
                                        }

                                        // Get the Factor for the New Height
                                        float NewHeight = DistanceFactor * ModifyVectorFactor;

                                        // Make sure it's within the bounds. Too high can cause GPU glitch outs.
                                        if (NewHeight <= 200 && NewHeight >= Minheight)
                                            TerrainMesh[x, y] += NewHeight;


                                        BufferNeedsFullUpdate = true;
                                    }
                                }
                            }

                            if (BufferNeedsFullUpdate)
                                TerrainMesh.UpdateDynamicVertexBuffer();

                            break;


                        case vxEnumTerrainEditMode.TexturePaint:

                            // Blend the Previous Texture Map with the current brush
                            //vxGraphics.GraphicsDevice.SetRenderTarget(CanvasTextureMap);

                            ////vxGraphics.GraphicsDevice.Clear(Color.Transparent);

                            //var TextureMapSize = vxTerrainManager.Instance.TextureMapSize;
                            //int Scale = (int)(vxTerrainManager.Instance.CursorScale * (TextureMapSize / 128.0f)) / 2;

                            //Point BrushLocation = new Point((int)(vxTerrainManager.Instance.CursorPosition.X * (TextureMapSize / 128.0f) / CellSize) - Scale / 2, (int)(vxTerrainManager.Instance.CursorPosition.Y * (TextureMapSize / 128.0f) / CellSize) - Scale / 2);

                            //float factor = ((float)Scene.TexturePaintType) / 4 - 0.25f;

                            //if (factor > 0.7f)
                            //    factor = 0.825f;
                            //Console.WriteLine(factor);
                            //vxGraphics.SpriteBatch.Begin("Terrain - Texture Paint", 0, BlendState.Opaque, SamplerState.PointClamp, null, null);
                            ////vxGraphics.SpriteBatch.Begin();
                            //vxGraphics.SpriteBatch.Draw(TextureMap, Vector2.Zero, Color.White);
                            ////vxGraphics.SpriteBatch.Draw(TextureBrush, new Rectangle(BrushLocation.X, BrushLocation.Y, Scale, Scale), Color.Black);
                            //vxGraphics.SpriteBatch.Draw(vxTerrainManager.Instance.TextureBrush, new Rectangle(BrushLocation.X, BrushLocation.Y, Scale, Scale), Color.White * factor);
                            //vxGraphics.SpriteBatch.End();

                            //// Now draw the Canvas Map to the Texture Map
                            //vxGraphics.GraphicsDevice.SetRenderTarget(TextureMap);
                            //vxGraphics.SpriteBatch.Begin("Terrain - Draw Canvas", 0, BlendState.Opaque, SamplerState.PointClamp, null, null);
                            //vxGraphics.SpriteBatch.Draw(CanvasTextureMap, Vector2.Zero, Color.White);
                            //vxGraphics.SpriteBatch.End();
                            //vxGraphics.GraphicsDevice.SetRenderTarget(null);

                            //TextureWeightMap = TextureMap;
                            break;
                    }
                }
                else if (vxInput.IsNewMouseButtonRelease(MouseButtons.LeftButton) ||
                    vxInput.IsNewMouseButtonRelease(MouseButtons.RightButton))
                {
                    if (BufferNeedsFullUpdate)
                    {
                        //Current3DScene.BEPUPhyicsSpace.Remove(PhysicsMesh);
                        //Current3DScene.BEPUDebugDrawer.Remove(PhysicsMesh);

                        //UpdatePhysicsMesh();

                        // Recalcuate the normals
                        TerrainMesh.CalculateNormals();
                        TerrainMesh.UpdateDynamicVertexBuffer();
                    }
                }
            }

            // Code only during Non-Edit Mode.
            else
            {

            }
        }


        #region -- Utility Methods --

        bool IsInBounds(Point point)
        {
            return (point.X >= 0 &&
                point.Y >= 0 &&
                point.X <= dimension &&
                point.Y <= dimension);
        }

        Point GetHeightLocation(Vector2 Pos)
        {
            Pos = Pos - Position.ToVector2();

            return new Point((int)(Pos.X / _cellSize), (int)(Pos.Y / _cellSize));
        }

        Vector2 GetWorldLocation(Point point)
        {
            return new Vector2(point.X * _cellSize, point.Y * _cellSize) + Position.ToVector2();
        }

        #endregion

        //public override void Draw(vxCamera Camera, string renderpass)
        //{
        //    base.Draw(Camera, renderpass);

        //    if (IsInEditMode)
        //    {
        //        if(renderpass == vxRenderPipeline.Passes.TransparencyPass)
        //        DrawWireFrame(Camera, Color.Black*0.5f);
        //    }
        //}

        // TODO Move to TerrainRenderer
        private void DrawWireFrame(vxCamera Camera, Color wireColour)
        {
            // TODO: Update with DrawSelected

            vxGraphics.SetRasterizerState(FillMode.WireFrame);
            foreach (vxModelMesh mesh in MeshRenderer.Mesh.Meshes)
            {
                vxGraphics.Util.WireframeShader.DoDebugWireFrame = true;
                vxGraphics.Util.WireframeShader.WireColour = wireColour;
                vxGraphics.Util.WireframeShader.World = Transform.RenderPassData.World;
                vxGraphics.Util.WireframeShader.WVP = Transform.RenderPassData.WVP;

                mesh.Draw(vxGraphics.Util.WireframeShader);
            }
            vxGraphics.SetRasterizerState(FillMode.Solid);
        }

        // TODO Move to TerrainRenderer

        public void DrawEditMode(vxCamera camera)
        {
            if (IsInEditMode)
            {
                vxGraphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                vxGraphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                vxGraphics.SetRasterizerState(FillMode.WireFrame);

                //Matrix TempWorld = WorldTransform * Matrix.CreateTranslation(Vector3.Up);// * Renderer.Camera.ViewProjection;
                //Matrix TempWVP = TempWorld * camera.ViewProjection;

                //for (int mi = 0; mi < Model.Meshes.Count; mi++)
                //{
                //    var mesh = Model.Meshes[mi];
                //    mesh.Material.DebugEffect.DoDebugWireFrame = true;
                //    mesh.Material.DebugEffect.WireColour = Color.Black * 0.75f;
                //    mesh.Material.DebugEffect.World = TempWorld;
                //    mesh.Material.DebugEffect.WVP = TempWVP;

                //    mesh.Draw(mesh.Material.DebugEffect);
                //}

                vxGraphics.SetRasterizerState(FillMode.Solid);
            }
        }

        //public override void Draw(vxCamera Camera, string renderpass)
        //{
        //    var dist = (this.Position - Camera.Position).Length();
        //    vxConsole.WriteLine(dist);

        //    if (dist < 250)
        //        CURRENT_LOD = 0;
        //    else if (dist < 500)
        //        CURRENT_LOD = 1;
        //    else if (dist < 750)
        //        CURRENT_LOD = 2;
        //    else if (dist < 1000)
        //        CURRENT_LOD = 3;

        //    base.Draw(Camera, renderpass);
        //}

        //int CURRENT_LOD = 0;

        //public override void Draw(Matrix world, Matrix view, Matrix projection, string renderpass)
        //{
        // var   TempWVP = world * view * projection;
        //    var worldInvT = Matrix.Transpose(Matrix.Invert(world));

        //    int lodToDraw = Math.Min(CURRENT_LOD, Model.Meshes.Count - 1);

        //    var mesh = Model.Meshes[CURRENT_LOD];
        //    {
        //        if (renderpass == mesh.Material.MaterialRenderPass)
        //        {
        //            mesh.Material.World = world;
        //            mesh.Material.WorldInverseTranspose = worldInvT;// Matrix.Transpose(Matrix.Invert(world));
        //            mesh.Material.WVP = TempWVP;// world * view * projection;
        //            mesh.Material.View = view;
        //            mesh.Material.Projection = projection;
        //            mesh.Draw();
        //        }
        //    }
        //}
    }
}
