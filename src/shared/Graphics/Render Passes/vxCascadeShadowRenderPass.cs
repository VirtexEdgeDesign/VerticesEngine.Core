using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

using VerticesEngine.Utilities;

namespace VerticesEngine.Graphics
{
    /// <summary>
    /// Cascade Shadow Map Rendering Pass
    /// </summary>
    public class vxCascadeShadowRenderPass : vxRenderPass, vxIRenderPass
    {
        [vxGraphicalSettingsAttribute("Shadow.Quality", "Shadow Quality", true, true, vxGameEnviromentType.ThreeDimensional)]
        public static vxEnumQuality ShadowQaulity
        {
            get { return _shadowQaulity; }
            set
            {
                _shadowQaulity = value;
                switch (_shadowQaulity)
                {
                    case vxEnumQuality.Ultra:
                        NumberOfShadowSplits = 4;
                        ShadowMapSize = 2048;
                        ShadowBoundingBoxSize = 512;
                        break;
                    case vxEnumQuality.High:
                        NumberOfShadowSplits = 4;
                        ShadowMapSize = 1024;
                        ShadowBoundingBoxSize = 512;
                        break;
                    case vxEnumQuality.Medium:
                        NumberOfShadowSplits = 4;
                        ShadowMapSize = 1024;
                        ShadowBoundingBoxSize = 512;
                        break;
                    case vxEnumQuality.Low:
                        NumberOfShadowSplits = 4;
                        ShadowMapSize = 1024;
                        ShadowBoundingBoxSize = 512;
                        break;
                    case vxEnumQuality.None:
                        break;
                }
            }
        }
        static vxEnumQuality _shadowQaulity = vxEnumQuality.Medium;

        /// <summary>
        /// Intensity of the edge dection.
        /// </summary>
        public Matrix InverseViewProjection
        {
            set
            {
                if (Parameters["InverseViewProjection"] != null)
                    Parameters["InverseViewProjection"].SetValue(value);
            }
        }

        public Matrix ViewProjection
        {
            set
            {
                if (Parameters["ViewProjection"] != null)
                    Parameters["ViewProjection"].SetValue(value);
            }
        }

        public List<Viewport> ShadowViewports = new List<Viewport>();

        /// <summary>
        /// Shadow Map Render Target.
        /// </summary>
        public RenderTarget2D ShadowMap;

        /// <summary>
        /// The block texture for use in shadow map debugging.
        /// </summary>
        public RenderTarget2D ShadowMapBlockTexture;

        /// <summary>
        /// Gets the number of shadow splits.
        /// </summary>
        /// <value>The number of shadow splits.</value>
        //[vxGraphicalSettings("Shadow.NumberOfCascades")]
        public static int NumberOfShadowSplits
        {
            get { return _numShadowSplits; }
            set
            {
                _numShadowSplits = MathHelper.Clamp(value, 1, 4);
                recalcSpits = true; ;
            }
        }
        static int _numShadowSplits = 4;
        static bool recalcSpits = false;

        /// <summary>
        /// The m snap shadow maps.
        /// </summary>
        public bool mSnapShadowMaps = true;


        //Shadow Mapping Area's Dimensions
        BoundingBox shadowBounds;
        //const int MaxSupportedPrimitivesPerDraw = 1048575;

        /// <summary>
        /// Gets the shadow view.
        /// </summary>
        /// <value>The shadow view.</value>
        public Matrix ShadowView;

        /// <summary>
        /// Gets the shadow projection.
        /// </summary>
        /// <value>The shadow projection.</value>
        public Matrix ShadowProjection;

        /// <summary>
        /// The shadow projections.
        /// </summary>
        public Matrix[] ShadowProjections;

        /// <summary>
        /// The shadow split projections.
        /// </summary>
        public Matrix[] ShadowSplitProjections;

        /// <summary>
        /// The shadow split projections with tiling.
        /// </summary>
        public Matrix[] ShadowSplitProjectionsWithTiling;


        /// <summary>
        /// The shadow split tile bounds.
        /// </summary>
        public Vector4[] ShadowSplitTileBounds;

        /// <summary>
        /// The view frustum splits.
        /// </summary>
        public Vector3[][] ViewFrustumSplits;

        /// <summary>
        /// The color of the view frustum.
        /// </summary>
        public Color ViewFrustumColor = new Color(0, 255, 255, 32);


        /// <summary>
        /// Shadow Map Render Target Size.
        /// </summary>
        /// <value>A higher value will give sharper looking shaows. 
        /// The Default Value is 512, but it can go up too 2048.</value>
        //[vxGraphicalSettings("Shadow.MapSize")]
        public static int ShadowMapSize
        {
            get { return _shadowMapSize; }
            set { _shadowMapSize = MathHelper.Clamp(value, 256, 2048); }
        }
        static int _shadowMapSize = 2048;


        /// <summary>
        /// The size of the shadow bounding box.
        /// </summary>
        //[vxGraphicalSettings("Shadow.BoundsSize")]
        public static float ShadowBoundingBoxSize = 512;

        vxShadowEffect ShadowEffect;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.Graphics.vxSSAOPostProcess"/> class.
        /// </summary>
        /// <param name="Engine">Engine.</param>
        public vxCascadeShadowRenderPass() : base("Cascade Shadow Pass", vxInternalAssets.Shaders.CascadeShadowShader)
        {
            ShadowEffect = new vxShadowEffect(Effect);

            // Set the Bounding Volume
            shadowBounds = new BoundingBox(new Vector3(-ShadowBoundingBoxSize, -ShadowBoundingBoxSize, -ShadowBoundingBoxSize), new Vector3(ShadowBoundingBoxSize, ShadowBoundingBoxSize, ShadowBoundingBoxSize));

        }

        protected override void OnDisposed()
        {
            base.OnDisposed();


            //ShadowMap.Dispose();
            //ShadowMap = null;

            //ShadowMapBlockTexture.Dispose();
            //ShadowMapBlockTexture = null;
        }

        public void InitialiseShadowMapViewports()
        {

            ShadowViewports.Clear();
            // Initialise Viewports
            for (int i = 0; i < _numShadowSplits; ++i)
            {
                int x = i % 2;
                int y = i / 2;
                ShadowViewports.Add(new Viewport(x * ShadowMapSize, y * ShadowMapSize, ShadowMapSize, ShadowMapSize));
            }
        }

        public override void RegisterRenderTargetsForDebug()
        {
            base.RegisterRenderTargetsForDebug();

            Renderer.RegisterDebugRenderTarget("Shadow Map", ShadowMap);
        }

        public override void OnGraphicsRefresh()
        {
            base.OnGraphicsRefresh();

#if __ANDROID__
            ShadowMap = new RenderTarget2D(vxGraphics.GraphicsDevice, ShadowMapSize * 2, ShadowMapSize * 2, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            ShadowMapBlockTexture = new RenderTarget2D(vxGraphics.GraphicsDevice, ShadowMapSize * 2, ShadowMapSize * 2, false, SurfaceFormat.Color, DepthFormat.Depth24);
#else
            ShadowMap = new RenderTarget2D(vxGraphics.GraphicsDevice, ShadowMapSize * 2, ShadowMapSize * 2, false, SurfaceFormat.Single, DepthFormat.Depth24Stencil8);
            ShadowMapBlockTexture = new RenderTarget2D(vxGraphics.GraphicsDevice, ShadowMapSize * 2, ShadowMapSize * 2, false, SurfaceFormat.Single, DepthFormat.Depth24);

#endif
            //int tSize = 32;

            //RandomTexture3D = new Texture3D(mGraphicsDevice, tSize, tSize, tSize, false, SurfaceFormat.Rg32);
            // RandomTexture2D = new Texture2D(mGraphicsDevice, tSize, tSize, false, SurfaceFormat.Rg32);

            Random random = new Random();

            Func<int, IEnumerable<UInt16>> randomRotations = (count) =>
            {
                return Enumerable
                   .Range(0, count)
                    .Select(i => (float)(random.NextDouble() * Math.PI * 2))
                    .SelectMany(r => new[] { Math.Cos(r), Math.Sin(r) })
                    .Select(v => (UInt16)((v * 0.5 + 0.5) * UInt16.MaxValue));
            };
            fillTextureWithBlockPattern(ShadowMapBlockTexture, 32);
            //RandomTexture3D.SetData(randomRotations(RandomTexture3D.Width * RandomTexture3D.Height * RandomTexture3D.Depth).ToArray());
            //RandomTexture2D.SetData(randomRotations(RandomTexture2D.Width * RandomTexture2D.Height).ToArray());

            /*
           BinaryWriter writer = new BinaryWriter(File.Create("3DTextureByte.txt"));
           byte[] b = new byte[_randomTexture3D.Width * _randomTexture3D.Height * _randomTexture3D.Depth * 4];
           _randomTexture3D.GetData<byte>(b);


           // Writer raw data                
           writer.Write(b);
           writer.Flush();
           writer.Close();

           byte[] file = System.IO.File.ReadAllBytes("3DTextureByte.bin");
           _randomTexture3D.SetData<byte>(file);

           Stream streampng = File.OpenWrite("tiny.png");
           _randomTexture2D.SaveAsPng(streampng, tSize, tSize);
           streampng.Dispose();
           //texture.Dispose();
           */

            // TODO: Readd
            //Renderer.InitialiseShadowMapViewports();
        }


        public void Prepare(vxCamera camera)
        {
                // determine shadow frustums
                var l = vxEngine.Instance.GetCurrentScene<vxGameplayScene3D>().LightPositions;
                l.Normalize();
                SetLightPosition(l);
                //Engine.Renderer.setShadowTransforms(mVirtualCameraMode != vxEnumVirtualCameraMode.None ? mVirtualCamera : Camera);
                setShadowTransforms(camera);

            if (ShadowQaulity > vxEnumQuality.None)
            {
                // only render shadow map if we're not filling it with a block pattern
                //if (SceneDebugDisplayMode < vxEnumSceneDebugMode.BlockPattern)
                //{
                vxGraphics.GraphicsDevice.SetRenderTarget(ShadowMap);
                vxGraphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.White, 1.0f, 0);

                vxGraphics.GraphicsDevice.BlendState = BlendState.Opaque;
                vxGraphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                vxGraphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

                for (int i = 0; i < NumberOfShadowSplits; ++i)
                {
                    {
                        if (i >= ShadowViewports.Count)
                        {
                            InitialiseShadowMapViewports();
                            Init();
                        }
                        else
                        {
                            vxGraphics.GraphicsDevice.Viewport = ShadowViewports[i];
                        }
                    }

                    ShadowEffect.CurrentTechnique = ShadowEffect.Techniques["Shadow"];
                    ShadowEffect.ShadowViewProjection.SetValue(ShadowSplitProjections[i]);

                    // TODO: change to Vector2 array instead of 2 dim float[,] array
                    ShadowEffect.DepthBias.SetValue(new Vector2(vxShadowEffect.ShadowDepthBias[i, 0], vxShadowEffect.ShadowDepthBias[i, 1]));

                    if (camera.IsRenderListEnabled)
                    {
                        // we're going to render all meshes in the renderlist which are shadow casters
                        for (int r = 0; r < camera.totalItemsToDraw; r++)
                        {
                            var renderData = camera.renderList[r];
                            ShadowEffect.World.SetValue(renderData.renderPassData.World);
                            if (renderData.renderPassData.IsShadowCaster)
                            {
                                renderData.mesh.Draw(ShadowEffect);
                            }
                        }
                    }
                    else
                    {
                        for (int e = 0; e < camera.totalItemsToDraw; e++)
                        {
                            int drawIndex = camera.drawList[e];
                            if (drawIndex < vxEngine.Instance.CurrentScene.MeshRenderers.Count)
                            {
                                var meshRenderer = vxEngine.Instance.CurrentScene.MeshRenderers[drawIndex];
                                if (meshRenderer.IsShadowCaster && meshRenderer.IsMainRenderingEnabled)
                                {
                                    //meshRenderer.DrawShadow(ShadowEffect);
                                    ShadowEffect.World.SetValue(meshRenderer.RenderPassData.World);
                                    for (int mi = 0; mi < meshRenderer.Mesh.Meshes.Count; mi++)
                                    {
                                        if (meshRenderer.GetMaterial(mi).IsShadowCaster)
                                        {
                                            meshRenderer.Mesh.Meshes[mi].Draw(ShadowEffect);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                vxGraphics.GraphicsDevice.SetRenderTarget(ShadowMap);
                vxGraphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.White, 1.0f, 0);

                vxGraphics.GraphicsDevice.BlendState = BlendState.Opaque;
                vxGraphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                vxGraphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

                vxGraphics.SpriteBatch.Begin("Shadows");
                vxGraphics.SpriteBatch.Draw(DefaultTexture, vxGraphics.GraphicsDevice.Viewport.Bounds, Color.White);
                vxGraphics.SpriteBatch.End();
            }
        }

        public void Apply(vxCamera camera)
        {
            // nothing to apply for this loop
        }


        public void DrawDebug(vxCamera camera)
        {

            for (int i = 0; i < camera.totalItemsToDraw; i++)
            {
                var meshRenderer = vxEngine.Instance.CurrentScene.MeshRenderers[camera.drawList[i]];

                if (meshRenderer.Mesh != null)
                {
                    for (int mi = 0; mi < meshRenderer.Mesh.Meshes.Count; mi++)
                    {
                        var material = meshRenderer.GetMaterial(mi);
                        if (material.IsDefferedRenderingEnabled)
                        {
                            material.UtilityEffect.CurrentTechnique = material.UtilityEffect.Techniques["Technique_ShadowDebug"];
                            material.UtilityEffect.World = meshRenderer.RenderPassData.World;
                            material.UtilityEffect.WVP = meshRenderer.RenderPassData.WVP;

                            //ShadowBrightness = 0.5f;

                            //if (vxCascadeShadowRenderPass.ShadowQaulity > vxEnumQuality.None)
                            {
                                // Shadow Parameters
                                material.UtilityEffect.ShadowBlurStart = 2;
                                material.UtilityEffect.ShadowMap = ShadowMap;
                                material.UtilityEffect.ShadowTransform = ShadowSplitProjectionsWithTiling;
                                material.UtilityEffect.TileBounds = ShadowSplitTileBounds;
                            }

                            meshRenderer.Mesh.Meshes[mi].Draw(material.UtilityEffect);
                        }
                    }
                }
            }

        }

        float viewDistance = 1;
        float[] splitPlanes;

        void Init()
        {
            viewDistance = new[]
                {
                   shadowBounds.Max.X - shadowBounds.Min.X,
                   shadowBounds.Max.Y - shadowBounds.Min.Y,
                   shadowBounds.Max.Z - shadowBounds.Min.Z,
                }.Max() - 200.0f;


            splitPlanes = vxGeometryHelper.practicalSplitScheme(NumberOfShadowSplits, 1, viewDistance)
                .Select(v => -v)
                .ToArray();
        }

        /// <summary>
        /// Sets the shadow transforms.
        /// </summary>
        /// <param name="camera">Camera.</param>
        private void setShadowTransforms(vxCamera camera)
        {
            Matrix viewProj = camera.ViewProjection;
            Matrix viewProjInverse = camera.InverseViewProjection;
            Matrix projInverse = camera.InverseProjection;
            Matrix viewInverse = camera.InverseView;

            // figure out closest geometry empassing near and far plances based on the given scene bounding box
            var viewSpaceBB = vxGeometryHelper.TransformBoundingBox(shadowBounds, camera.View);
            var viewSpaceMin = Math.Min(-1, viewSpaceBB.Max.Z);
            var viewSpaceMax = Math.Min(0, viewSpaceBB.Min.Z);

            if (splitPlanes == null || recalcSpits)
            {
                recalcSpits = false;
                splitPlanes = vxGeometryHelper.practicalSplitScheme(NumberOfShadowSplits, 1, viewDistance)
                    .Select(v => -v)
                    .ToArray();
            }

            var splitDistances = splitPlanes.Select(c =>
            {
                var d = Vector4.Transform(new Vector3(0, 0, c), camera.Projection);
                return d.W != 0 ? d.Z / d.W : 0;
            }).ToArray();

            var splitData = Enumerable.Range(0, NumberOfShadowSplits).Select(i =>
            {
                var n = splitDistances[i];
                var f = splitDistances[i + 1];

                var viewSplit = vxGeometryHelper.splitFrustum(n, f, viewProjInverse).ToArray();
                var frustumCorners = viewSplit.Select(v => Vector3.Transform(v, ShadowView)).ToArray();
                var cameraPosition = Vector3.Transform(viewInverse.Translation, ShadowView);

                var viewMin = frustumCorners.Aggregate((v1, v2) => Vector3.Min(v1, v2));
                var viewMax = frustumCorners.Aggregate((v1, v2) => Vector3.Max(v1, v2));

                var arenaBB = vxGeometryHelper.TransformBoundingBox(shadowBounds, ShadowView);

                var minZ = -arenaBB.Max.Z;
                var maxZ = -arenaBB.Min.Z;

                var range = Math.Max(
                    1.0f / camera.Projection.M11 * -splitPlanes[i + 1] * 2.0f,
                    -splitPlanes[i + 1] - (-splitPlanes[i])
                );

                // range is slightly too small, so add in some padding
                float padding = 5.0f;
                var quantizationStep = (range + padding) / (float)ShadowMapSize;

                var x = vxGeometryHelper.determineShadowMinMax1D(frustumCorners.Select(v => v.X), cameraPosition.X, range);
                var y = vxGeometryHelper.determineShadowMinMax1D(frustumCorners.Select(v => v.Y), cameraPosition.Y, range);

                var projectionMin = new Vector3(x[0], y[0], minZ);
                var projectionMax = new Vector3(x[1], y[1], maxZ);

                // Add in padding
                {
                    range += padding;
                    projectionMin.X -= padding / 2.0f;
                    projectionMin.Y -= padding / 2.0f;
                }

                // quantize
                if (mSnapShadowMaps)
                {
                    // compute range
                    var qx = (float)Math.IEEERemainder(projectionMin.X, quantizationStep);
                    var qy = (float)Math.IEEERemainder(projectionMin.Y, quantizationStep);

                    projectionMin.X = projectionMin.X - qx;
                    projectionMin.Y = projectionMin.Y - qy;

                    projectionMax.X = projectionMin.X + range;
                    projectionMax.Y = projectionMin.Y + range;
                }

                // compute offset into texture atlas
                int tileX = i % 2;
                int tileY = i / 2;

                // [x min, x max, y min, y max]
                float tileBorder = 3.0f / (float)ShadowMapSize;
                var tileBounds = new Vector4(
                    0.5f * tileX + tileBorder,
                    0.5f * tileX + 0.5f - tileBorder,
                    0.5f * tileY + tileBorder,
                    0.5f * tileY + 0.5f - tileBorder
                );

                var tileMatrix = Matrix.Identity;
                tileMatrix.M11 = 0.25f;
                tileMatrix.M22 = -0.25f;
                tileMatrix.Translation = new Vector3(0.25f + tileX * 0.5f, 0.25f + tileY * 0.5f, 0);

                return new
                {
                    Distance = f,
                    ViewFrustum = viewSplit,
                    Projection = Matrix.CreateOrthographicOffCenter(projectionMin.X, projectionMax.X, projectionMin.Y, projectionMax.Y, projectionMin.Z, projectionMax.Z),
                    TileTransform = tileMatrix,
                    TileBounds = tileBounds,
                };
            }).ToArray();


            //ViewFrustumSplits = splitData.Select(s => s.ViewFrustum).ToArray();
            ShadowSplitProjections = splitData.Select(s => ShadowView * s.Projection).ToArray();
            ShadowSplitProjectionsWithTiling = splitData.Select(s => ShadowView * s.Projection * s.TileTransform).ToArray();
            ShadowSplitTileBounds = splitData.Select(s => s.TileBounds).ToArray();
            ShadowProjections = splitData.Select(s => s.Projection).ToArray();
        }


        /// <summary>
        /// Fills the texture with block pattern.
        /// </summary>
        /// <param name="targetTexture">Target texture.</param>
        /// <param name="blockSize">Block size.</param>
        public void fillTextureWithBlockPattern(Texture2D targetTexture, int blockSize)
        {
            targetTexture.SetData(
                Enumerable.Range(0, ShadowMap.Height * ShadowMap.Width).Select(i =>
                {
                    int x = i % ShadowMap.Width;
                    int y = i / ShadowMap.Height;

                    var xBlack = (x % blockSize) > (blockSize / 2);
                    var yBlack = (y % blockSize) > (blockSize / 2);

                    return (xBlack ^ yBlack) ? 1.0f : 0.0f;
                }).ToArray());
        }

        /// <summary>
        /// Swaps the shadow map with block texture.
        /// </summary>
        public void swapShadowMapWithBlockTexture()
        {
            RenderTarget2D tmp = ShadowMap;
            ShadowMap = ShadowMapBlockTexture;
            ShadowMapBlockTexture = tmp;
        }


        BoundingBox TransFormedBB;

        Vector3 LighLook;

        Vector3 LightUp;



        /// <summary>
        /// Sets the light position.
        /// </summary>
        /// <param name="lightPosition">Light position.</param>
        public void SetLightPosition(Vector3 lightPosition)
        {
            Renderer.LightDirection = lightPosition;

            LighLook = Vector3.Normalize(-lightPosition);
            LightUp = Vector3.Cross(LighLook, Vector3.Right);

            // Remember: XNA uses a right handed coordinate system, i.e. -Z goes into the screen
            ShadowView = Matrix.Invert(
                new Matrix(
                    1, 0, 0, 0,
                    0, 0, -1, 0,
                    -LighLook.X, -LighLook.Y, -LighLook.Z, 0,
                    lightPosition.X, lightPosition.Y, lightPosition.Z, 1
                )
            );

            // bounding box
            {
                TransFormedBB = vxGeometryHelper.TransformBoundingBox(shadowBounds, ShadowView);
                ShadowProjection = Matrix.CreateOrthographicOffCenter(TransFormedBB.Min.X, TransFormedBB.Max.X, TransFormedBB.Min.Y, TransFormedBB.Max.Y, -TransFormedBB.Max.Z, -TransFormedBB.Min.Z);
            }
        }
    }
}