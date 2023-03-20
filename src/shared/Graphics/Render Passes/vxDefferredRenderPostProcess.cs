
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using VerticesEngine;
//using VerticesEngine;
//
//using VerticesEngine.Settings;
//using VerticesEngine.Utilities;
//using VerticesEngine;
//using System.Xml.Serialization;

//namespace VerticesEngine.Graphics
//{
//    public class vxFogGraphicsConfig : vxGraphicsConfig
//    {
//        public bool DoFog = false;
//        public float FogNear = 200;
//        public float FogFar = 500;
//        public float FogHeight = 50;
//        public Color FogColor = Color.Gray;
//    }
//    public class vxEdgeDetectGraphicsConfig : vxGraphicsConfig
//    {
//        // Set Edge Settings
//        public float EdgeWidth = 1;
//        public float EdgeIntensity = 1;

//        public float NormalThreshold = 0.5f;
//        public float DepthThreshold = 0.005f;

//        // How dark should the edges get in response to changes in the input data?
//        public float NormalSensitivity = 1.0f;
//        public float DepthSensitivity = 250;
//    }

//    public class vxGodRaysGraphicsConfig : vxGraphicsConfig
//    {
//        /// <summary>
//        /// Density
//        /// </summary>
//        [XmlElement("Density")]
//        public float Density = 0.75f;

//        /// <summary>
//        /// Decay of rays.
//        /// </summary>
//        [XmlElement("Decay")]
//        public float Decay = 0.85f;


//        [XmlElement("Weight")]
//        public float Weight = 1.0f;


//        [XmlElement("Exposure")]
//        public float Exposure = 0.15f;

//    }

//    public class vxDefferredRenderPostProcess : vxPostProcessor
//    {
//        public Matrix InverseViewProjection
//        {
//            set { Parameters["InverseViewProjection"].SetValue(value); }
//        }

//        #region Entity Fog Controls

//        /// <summary>
//        /// Should there be fog.
//        /// </summary>
//        [vxShowInInspector(vxInspectorShaderCategory.DeferredRenderer)]
//        public bool DoFog
//        {
//            get { return Parameters["DoFog"].GetValueBoolean(); }
//            set { Parameters["DoFog"].SetValue(value); }
//        }


//        /// <summary>
//        /// Gets or sets the fog near value.
//        /// </summary>
//        /// <value>The fog near.</value>
//        [vxShowInInspector(vxInspectorShaderCategory.DeferredRenderer)]
//        public float FogNear
//        {
//            get { return Parameters["FogNear"].GetValueSingle(); }
//            set { Parameters["FogNear"].SetValue(value); }
//        }



//        /// <summary>
//        /// Gets or sets the fog far value.
//        /// </summary>
//        /// <value>The fog far.</value>
//        [vxShowInInspector(vxInspectorShaderCategory.DeferredRenderer)]
//        public float FogFar
//        {
//            get { return Parameters["FogFar"].GetValueSingle(); }
//            set { Parameters["FogFar"].SetValue(value); }
//        }

//        [vxShowInInspector(vxInspectorShaderCategory.DeferredRenderer)]
//        public float FogHeight
//        {
//            get { return Parameters["FogHeight"].GetValueSingle(); }
//            set { Parameters["FogHeight"].SetValue(value); }
//        }



//        /// <summary>
//        /// Gets or sets the fog colour value.
//        /// </summary>
//        [vxShowInInspector(vxInspectorShaderCategory.DeferredRenderer)]
//        public Color FogColor
//        {
//            get { return _fogColor; }
//            set
//            {
//                _fogColor = value;
//                Parameters["FogColor"].SetValue(value.ToVector4());
//            }
//        }
//        Color _fogColor = Color.CornflowerBlue;

//        #endregion

//        #region Edge Detection

//        /// <summary>
//        /// Edge Width for edge detection.
//        /// </summary>
//        [vxShowInInspector(vxInspectorShaderCategory.EdgeDetection)]
//        public float EdgeWidth
//        {
//            get { return Parameters["EdgeWidth"].GetValueSingle(); }
//            set { Parameters["EdgeWidth"].SetValue(value); }
//        }

//        /// <summary>
//        /// Intensity of the edge dection.
//        /// </summary>
//        [vxShowInInspector(vxInspectorShaderCategory.EdgeDetection)]
//        public float EdgeIntensity
//        {
//            get { return Parameters["EdgeIntensity"].GetValueSingle(); }
//            set { Parameters["EdgeIntensity"].SetValue(value); }
//        }

//        public Vector2 ScreenResolution
//        {
//            set { Parameters["ScreenResolution"].SetValue(value); }
//        }





//        /// <summary>
//        /// Normal Threshold for edge detection.
//        /// </summary>
//        [vxShowInInspector(vxInspectorShaderCategory.EdgeDetection)]
//        public bool DoEdgeDetect
//        {
//            get { return Parameters["DoEdgeDetect"].GetValueBoolean(); }
//            set { Parameters["DoEdgeDetect"].SetValue(value); }
//        }

//        /// <summary>
//        /// Normal Threshold for edge detection.
//        /// </summary>
//        [vxShowInInspector(vxInspectorShaderCategory.EdgeDetection)]
//        public float NormalThreshold
//        {
//            get { return Parameters["NormalThreshold"].GetValueSingle(); }
//            set { Parameters["NormalThreshold"].SetValue(value); }
//        }

//        /// <summary>
//        /// Normal Sensitivity for edge detection.
//        /// </summary>
//        [vxShowInInspector(vxInspectorShaderCategory.EdgeDetection)]
//        public float NormalSensitivity
//        {
//            get { return Parameters["NormalSensitivity"].GetValueSingle(); }
//            set { Parameters["NormalSensitivity"].SetValue(value); }
//        }

//        /// <summary>
//        ///Depth Threshold for edge detection.
//        /// </summary>
//        [vxShowInInspector(vxInspectorShaderCategory.EdgeDetection)]
//        public float DepthThreshold
//        {
//            get { return Parameters["DepthThreshold"].GetValueSingle(); }
//            set { Parameters["DepthThreshold"].SetValue(value); }
//        }

//        /// <summary>
//        /// Depth Sensitivity for edge detection.
//        /// </summary>
//        [vxShowInInspector(vxInspectorShaderCategory.EdgeDetection)]
//        public float DepthSensitivity
//        {
//            get { return Parameters["DepthSensitivity"].GetValueSingle(); }
//            set { Parameters["DepthSensitivity"].SetValue(value); }
//        }

//        #endregion

//        #region God Rays
//        /// <summary>
//        /// Density
//        /// </summary>
//        [vxShowInInspector(vxInspectorShaderCategory.GodRays)]
//        public float Density
//        {
//            get { return Parameters["Density"].GetValueSingle(); }
//            set { Parameters["Density"].SetValue(value); }
//        }

//        /// <summary>
//        /// Decay of rays.
//        /// </summary>
//        [vxShowInInspector(vxInspectorShaderCategory.GodRays)]
//        public float Decay
//        {

//            get { return Parameters["Decay"].GetValueSingle(); }
//            set { Parameters["Decay"].SetValue(value); }
//        }

//        [vxShowInInspector(vxInspectorShaderCategory.GodRays)]
//        public float Weight
//        {
//            get { return Parameters["Weight"].GetValueSingle(); }
//            set { Parameters["Weight"].SetValue(value); }
//        }

//        //public float Exposure
//        //{
//        //    set { Parameters["Exposure"].SetValue(value); }
//        //}

//        /// <summary>
//        /// Texture Size for the Sun
//        /// </summary>
//        [vxShowInInspector(vxInspectorShaderCategory.GodRays)]
//        public Vector2 TextureSize
//        {

//            get { return _textureSize; }
//            set
//            {
//                _textureSize = value;
//                Parameters["TextureSize"].SetValue(value);
//            }
//        }
//        Vector2 _textureSize;

//        /// <summary>
//        /// Texture Scale for the Sun
//        /// </summary>
//        [vxShowInInspector(vxInspectorShaderCategory.GodRays)]
//        public Vector2 TextureScale
//        {
//            get { return _textureScale; }
//            set
//            {
//                _textureScale = value;
//                Parameters["TextureScale"].SetValue(value);
//            }
//        }
//        Vector2 _textureScale;

//        /// <summary>
//        /// The Sun Texture
//        /// </summary>
//        [vxShowInInspector(vxInspectorShaderCategory.GodRays)]
//        Texture2D SunTexture
//        {
//            get { return _sunTexture; }
//            set
//            {
//                _sunTexture = value;
//                Parameters["SunTexture"].SetValue(value);
//            }
//        }
//        Texture2D _sunTexture;

//        vxGameplayScene3D Scene
//        {
//            get { return (vxGameplayScene3D)vxEngine.Instance.CurrentScene; }
//        }

//        public vxSunEntity SunEntity
//        {
//            get { return Scene.SunEmitter; }
//        }

//        #endregion

//        public vxDefferredRenderPostProcess(vxRenderPipeline3D Renderer) :
//        base(Renderer, "Defferred Lighting", vxInternalAssets.PostProcessShaders.DrfrdRndrCombineFinal)
//        {

//        }


//        public override void LoadContent(vxRenderPipelineConfig config)
//        {
//            base.LoadContent(config);

//            DoFog = config.Fog.DoFog;
//            FogNear = config.Fog.FogNear;
//            FogFar = config.Fog.FogFar;
//            FogHeight = config.Fog.FogHeight;
//            FogColor = config.Fog.FogColor;

//            // Edge Detection
//            // ----------------------------

//            // Set Edge Settings
//            EdgeWidth = config.EdgeDetect.EdgeWidth;
//            EdgeIntensity = config.EdgeDetect.EdgeIntensity;

//            // How sensitive should the edge detection be to tiny variations in the input data?
//            // Smaller settings will make it pick up more subtle edges, while larger values get
//            // rid of unwanted noise.

//            NormalThreshold = config.EdgeDetect.NormalThreshold;
//            DepthThreshold = config.EdgeDetect.DepthThreshold;

//            // How dark should the edges get in response to changes in the input data?
//            NormalSensitivity = config.EdgeDetect.NormalSensitivity;
//            DepthSensitivity = config.EdgeDetect.DepthSensitivity;

//            ScreenResolution = new Vector2(Viewport.Width, Viewport.Height);


//            // God Rays
//            // ----------------------------
//            Density = config.GodRays.Density;
//            //DepthThreshold = 0.0005f;
//            Decay = config.GodRays.Decay;
//            Weight = config.GodRays.Weight;
//            //Exposure = .15f;



//            SunTexture = vxInternalAssets.Textures.Texture_Sun_Glow;
//            float Width = SunTexture.Width * 2;
//            float Height = SunTexture.Height * 2;

//            TextureScale = new Vector2(
//                Viewport.Width / Width,
//                Viewport.Height / Height);

//            TextureSize = new Vector2((Width / 2) / Viewport.Width, (Height / 2) / Viewport.Height);

//            fog = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/terrain/Heightmap");
//        }

//        public override void RefreshSettings()
//        {
//            base.RefreshSettings();

//            //DoFog = Settings.Graphics.DefferredLighting.Fog.DoFog;
//        }

//        public override void OnGraphicsRefresh()
//        {
//            base.OnGraphicsRefresh();
//            vxInternalAssets.PostProcessShaders.DrfrdRndrPointLight.Parameters["halfPixel"].SetValue(HalfPixel);
//        }

//        public override void Prepare()
//        {
//            GeneratePrepPass();
//            GenerateDataMaskPass();
//            GenerateSunMask();
//        }

//        void GeneratePrepPass()
//        {
//            // Set Multi Rendertargets
//            vxGraphics.GraphicsDevice.SetRenderTargets(
//                RenderTargets.SurfaceDataMap,
//                RenderTargets.NormalMap,
//                RenderTargets.CubeReflcMap,
//                RenderTargets.DistortionMap);


//            //Setup initial graphics states for prep pass            
//            vxGraphics.GraphicsDevice.BlendState = BlendState.Opaque;
//            vxGraphics.GraphicsDevice.DepthStencilState = DepthStencilState.None;
//            //
//            //Reset Appropriate Values for Rendertargets
//            //Engine.Assets.Shaders.DrfrdRndrClearGBuffer.Parameters["SkyColour"].SetValue(Color.White.ToVector4());
//            vxInternalAssets.PostProcessShaders.DrfrdRndrClearGBuffer.Techniques[0].Passes[0].Apply();
//            Scene.Renderer.RenderQuad(Vector2.One * -1, Vector2.One);

//            //Set the Depth Buffer appropriately
//            vxGraphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

//            foreach (vxCamera3D Camera in Scene.Cameras)
//            {
//                Camera.SetAsActiveCamera();
//                foreach (vxEntity3D entity in Scene.Entities)
//                    entity.RenderPrepPass(Camera);
//            }

//        }


//        void GenerateDataMaskPass()
//        {
//            // Set Multi Rendertargets
//            vxGraphics.GraphicsDevice.SetRenderTargets(
//                RenderTargets.EncodedIndexResult,
//                RenderTargets.EntityMaskValues,
//                RenderTargets.DepthMap,
//                RenderTargets.AuxDepthMap);

//            GraphicsDevice.Clear(Color.Black);


//            foreach (vxCamera3D Camera in Scene.Cameras)
//            {
//                Camera.SetAsActiveCamera();
//                foreach (vxEntity3D entity in Scene.Entities)
//                    entity.RenderDataMaskPass(Camera);


//                //vxGraphics.GraphicsDevice.DepthStencilState = DepthStencilState.None;

//                foreach (vxCamera3D camera in Scene.Cameras)
//                    foreach (var item in Scene.OverlayEntities)
//                        item.RenderDataMaskPass(camera);
//            }

//        }

//        public void GenerateSunMask()
//        {
//            vxGraphics.GraphicsDevice.SetRenderTarget(RenderTargets.GodRaysMaskMap);
//            //vxGraphics.GraphicsDevice.BlendState = BlendState.Opaque;
//            //vxGraphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
//            vxGraphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

//            Scene.SunEmitter.DrawGlow(Scene.Camera);

//            // Screen Space Position Normalised against Viewport Width and Height
//            Vector2 lighScreenSourcePos = new Vector2(
//                SunEntity.ScreenSpacePosition.X / vxGraphics.GraphicsDevice.Viewport.Width,
//                SunEntity.ScreenSpacePosition.Y / vxGraphics.GraphicsDevice.Viewport.Height);

//            Effect.CurrentTechnique = Effect.Techniques["GenerateSunMaskTechnique"];

//            Effect.Parameters["DepthMap"].SetValue(RenderTargets.DepthMap);
//            Parameters["lightScreenPosition"].SetValue(lighScreenSourcePos);

//            Effect.Techniques["GenerateSunMaskTechnique"].Passes[0].Apply();
//            Scene.Renderer.RenderQuad(Vector2.One * -1, Vector2.One);
//        }

//        Effect pointLightEffect
//        {
//            get { return vxInternalAssets.PostProcessShaders.DrfrdRndrPointLight; }
//        }

//        /// <summary>
//        /// Draws all of the vxLightEntities in the scene to the Light Map Render Target
//        /// </summary>
//        public void GenerateLightMap(List<vxCamera3D> Cameras)
//        {
//            //Render Light Pass
//            vxGraphics.GraphicsDevice.SetRenderTarget(RenderTargets.LightMap);
//            vxGraphics.GraphicsDevice.Clear(Color.Transparent);
//            //vxGraphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
//            vxGraphics.GraphicsDevice.DepthStencilState = DepthStencilState.None;
//            vxGraphics.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

//            // Set Light Values
//            //Effect pointLightEffect = vxInternalAssets.PostProcessShaders.DrfrdRndrPointLight;
//            //set the G-Buffer parameters
//            pointLightEffect.Parameters["colorMap"].SetValue(RenderTargets.SurfaceDataMap);
//            pointLightEffect.Parameters["normalMap"].SetValue(RenderTargets.NormalMap);
//            pointLightEffect.Parameters["depthMap"].SetValue(RenderTargets.DepthMap);

//            //DrawDirectionalLight(-Vector3.Normalize(Scene.Renderer.LightPosition), Color.White * 0);

//            Scene.ViewportManager.ResetViewport();
//            // Draw the light entities
//            foreach (var camera in Cameras)
//            {
//                camera.SetAsActiveCamera();
//                foreach (vxLightEntity lightEntity in Scene.LightItems)
//                    lightEntity.Draw(camera);
//            }


//            vxGraphics.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
//            vxGraphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
//        }

//        public RenderTarget2D DeferredLitScene
//        {
//            get { return _deferredLitScene; }
//        }
//        RenderTarget2D _deferredLitScene;


//        public RenderTarget2D LitScene
//        {
//            get { return _litScene; }
//        }
//        RenderTarget2D _litScene;

//        Texture2D fog;
//        public override void Apply()
//        {
//            //Set Render State
//            if (Settings.Graphics.DefferredLighting.Quality > vxEnumQuality.None)
//            {
//                /*
//                _litScene = Renderer.GetNewTempTarget("Lit Scene Renderer");
//                vxGraphics.GraphicsDevice.SetRenderTarget(_litScene);

//                vxGraphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
//                vxGraphics.GraphicsDevice.DepthStencilState = DepthStencilState.None;
//                vxGraphics.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

//                //Combine everything
//                Parameters["lightMap"].SetValue(RenderTargets.LightMap);
//                Renderer.GetCurrentTempTarget();
//                Effect.CurrentTechnique = Effect.Techniques["Technique_LightingScene"];

//                foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
//                {
//                    pass.Apply();
//                    Scene.Renderer.RenderQuad(Vector2.One * -1, Vector2.One);
//                }

//                */





//                _deferredLitScene = Renderer.GetNewTempTarget("Deferred Renderer");
//                vxGraphics.GraphicsDevice.SetRenderTarget(_deferredLitScene);

//                vxGraphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
//                vxGraphics.GraphicsDevice.DepthStencilState = DepthStencilState.None;
//                vxGraphics.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;


//                //Effect finalCombineEffect = vxInternalAssets.PostProcessShaders.DrfrdRndrCombineFinal;

//                if (Settings.Graphics.GodRays.Quality != vxEnumQuality.None &&
//   vxGraphics.GraphicsDevice.Viewport.Bounds.Contains(SunEntity.ScreenPosition.ToPoint()) &&
//                    Scene.SunEmitter.IsOnScreen)
//                {

//                    Parameters["DoGodRays"].SetValue(true);
//                    Parameters["SunMaskTexture"].SetValue(RenderTargets.GodRaysMaskMap);
//                }
//                else
//                {
//                    Parameters["DoGodRays"].SetValue(false);
//                }


//                //Combine everything
//                Parameters["SceneMap"].SetValue(Renderer.GetCurrentTempTarget());
//                Parameters["lightMap"].SetValue(RenderTargets.LightMap);
//                Parameters["SurfaceDataMap"].SetValue(RenderTargets.SurfaceDataMap);
//                Parameters["ReflectionMap"].SetValue(RenderTargets.CubeReflcMap);

//                //Parameters["FogVolumeTexture"].SetValue(fog);

//                Parameters["normalMap"].SetValue(RenderTargets.NormalMap);
//                Parameters["DepthMap"].SetValue(RenderTargets.DepthMap);
//                Parameters["DoEdgeDetect"].SetValue(Settings.Graphics.EdgeDetection.IsEnabled);

//                Parameters["VX_CAMERA_POS"].SetValue(Scene.Camera.WorldMatrix.Translation);

//                InverseViewProjection = Matrix.Invert(Scene.Camera.View * Scene.Camera.Projection);


//                Effect.CurrentTechnique = Effect.Techniques["Technique_Combine"];

//                foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
//                {
//                    pass.Apply();
//                    Scene.Renderer.RenderQuad(Vector2.One * -1, Vector2.One);
//                }

//            }
//        }


//        private void DrawDirectionalLight(Vector3 lightDirection, Color color)
//        {
//            Effect directionalLightEffect = vxInternalAssets.PostProcessShaders.DrfrdRndrDirectionalLight;

//            directionalLightEffect.Parameters["colorMap"].SetValue(RenderTargets.SurfaceDataMap);
//            directionalLightEffect.Parameters["normalMap"].SetValue(RenderTargets.NormalMap);
//            directionalLightEffect.Parameters["depthMap"].SetValue(RenderTargets.DepthMap);

//            directionalLightEffect.Parameters["lightDirection"].SetValue(lightDirection);
//            directionalLightEffect.Parameters["Color"].SetValue(color.ToVector3());

//            directionalLightEffect.Parameters["halfPixel"].SetValue(HalfPixel);

//            foreach (vxCamera3D Camera in Scene.Cameras)
//            {
//                Camera.SetAsActiveCamera();

//                directionalLightEffect.Parameters["cameraPosition"].SetValue(Camera.Position);
//                directionalLightEffect.Parameters["InvertViewProjection"].SetValue(Matrix.Invert(Camera.View * Camera.Projection));


//                directionalLightEffect.Techniques[0].Passes[0].Apply();
//            }
//            Scene.Renderer.RenderQuad(Vector2.One * -1, Vector2.One);
//        }
//    }
//}
