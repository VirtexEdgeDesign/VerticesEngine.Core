using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VerticesEngine.Graphics
{
    public class vxSelectedItemEdgePostProcess : vxRenderPass, vxIRenderPass
    {
        [vxGraphicalSettings("IsEdgeDetectionEnabled")]
        public static bool IsEdgeDetectionEnabled = true;

        vxMainScene3DRenderPass mainPass;

        //vxScenePrepRenderingPass prepPass;

        public RenderTarget2D NormalMap
        {
            set { Parameters["NormalTexture"].SetValue(value); }
        }

        /// <summary>
        /// Normal Map.
        /// </summary>
        public RenderTarget2D SceneTexture
        {
            set { Parameters["SceneTexture"].SetValue(value); }
        }

        /// <summary>
        /// Depth map of the scene.
        /// </summary>
        public RenderTarget2D DepthMap
        {
            set { Parameters["DepthTexture"].SetValue(value); }
        }
        public RenderTarget2D SceneDepthMap
        {
            set { Parameters["SceneDepthTexture"].SetValue(value); }
        }

        //public RenderTarget2D EntityMaskSampler
        //{
        //    set { Parameters["EntityMaskMap"].SetValue(value); }
        //}

        public RenderTarget2D RMAMap
        {
            set { Parameters["maps_RMA"].SetValue(value); }
        }


        /// <summary>
        /// Edge Width for edge detection.
        /// </summary>
        public float EdgeWidth
        {
            get { return m_edgeWidth; }
            set {
                m_edgeWidth = value;
                Parameters["EdgeWidth"].SetValue(m_edgeWidth); }
        }
        private float m_edgeWidth;

        /// <summary>
        /// Intensity of the edge dection.
        /// </summary>
        public float EdgeIntensity
        {
            get { return m_edgeIntensity; }
            set {

                m_edgeIntensity = value;
                Parameters["EdgeIntensity"].SetValue(value); }
        }
        private float m_edgeIntensity;





        public float NormalThreshold
        {
            get { return m_NormalThreshold; }
            set
            {

                m_NormalThreshold = value; Parameters["NormalThreshold"].SetValue(value); }
        }
        private float m_NormalThreshold;


        public float DepthThreshold
        {
            get { return m_DepthThreshold; }
            set
            {

                m_DepthThreshold = value; Parameters["DepthThreshold"].SetValue(value); }
        }
        private float m_DepthThreshold;

        public float NormalSensitivity
        {
            get { return m_NormalSensitivity; }
            set
            {

                m_NormalSensitivity = value; Parameters["NormalSensitivity"].SetValue(value); }
        }
        private float m_NormalSensitivity;


        public float DepthSensitivity
        {
            get { return m_DepthSensitivity; }
            set
            {

                m_DepthSensitivity = value; Parameters["DepthSensitivity"].SetValue(value); }
        }
        private float m_DepthSensitivity;

        // Selection Render Targets
        // **********************************************

        /// <summary>
        /// Normal Render Target.
        /// </summary>
        private RenderTarget2D SelectionSurface;

        /// <summary>
        /// Normal Render Target.
        /// </summary>
        private RenderTarget2D SelectionNormalMap;

        /// <summary>
        /// Depth Render Target.
        /// </summary>
        private RenderTarget2D SelectionDepthMap;

        public vxSelectedItemEdgePostProcess() :base("Edge Detect", vxInternalAssets.PostProcessShaders.SelectionEdgeDetection)
        {
            // Set Edge Settings
            EdgeWidth = 1;
            EdgeIntensity = 1;


            // How sensitive should the edge detection be to tiny variations in the input data?
            // Smaller settings will make it pick up more subtle edges, while larger values get
            // rid of unwanted noise.
            NormalThreshold = 0.5f;
            DepthThreshold = 0.00005f;

            // How dark should the edges get in response to changes in the input data?
            NormalSensitivity = 10.0f;
            DepthSensitivity = 100;
        }

        protected override void OnInitialised()
        {
            base.OnInitialised();

            mainPass = Renderer.GetRenderingPass<vxMainScene3DRenderPass>();
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();

            mainPass = null;
        }

        public override void OnGraphicsRefresh()
        {
            base.OnGraphicsRefresh();
            
            var viewport = vxGraphics.FinalViewport;
#if __MOBILE__
            SelectionDepthMap = new RenderTarget2D(vxGraphics.GraphicsDevice, viewport.Width, viewport.Height, false, SurfaceFormat.Color, DepthFormat.None);
#else
            SelectionDepthMap = new RenderTarget2D(vxGraphics.GraphicsDevice, viewport.Width, viewport.Height, false, SurfaceFormat.Single, DepthFormat.None);
#endif
            SelectionSurface = new RenderTarget2D(vxGraphics.GraphicsDevice, viewport.Width, viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            SelectionNormalMap = new RenderTarget2D(vxGraphics.GraphicsDevice, viewport.Width, viewport.Height, false, SurfaceFormat.Color, DepthFormat.None);
        }

        public override void RegisterRenderTargetsForDebug()
        {
            base.RegisterRenderTargetsForDebug();

            Renderer.RegisterDebugRenderTarget("Selection Surface Map", SelectionSurface);
            Renderer.RegisterDebugRenderTarget("Selection Normal Map", SelectionNormalMap);
            Renderer.RegisterDebugRenderTarget("Selection Depth Map", SelectionDepthMap);
        }

        public void Prepare(vxCamera camera) {
            // Set Multi Rendertargets
            vxGraphics.GraphicsDevice.SetRenderTargets(SelectionSurface, SelectionNormalMap, SelectionDepthMap);


            //Setup initial graphics states for prep pass            
            vxGraphics.GraphicsDevice.BlendState = BlendState.Opaque;
            vxGraphics.GraphicsDevice.DepthStencilState = DepthStencilState.None;

            //Reset Appropriate Values for Rendertargets
            vxInternalAssets.PostProcessShaders.DrfrdRndrClearGBuffer.Techniques[0].Passes[0].Apply();
            Renderer.RenderQuad(Vector2.One * -1, Vector2.One);

            //Set the Depth Buffer appropriately
            vxGraphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            var selectedItems = vxEngine.Instance.GetCurrentScene<vxGameplayScene3D>().SelectedItems;

            if (camera.IsRenderListEnabled)
            {
                for (int i = 0; i < camera.selectedCount; i++)
                {
                    var renderData = camera.selectedRenderList[i];
                    //DrawEntityToGBuffer(renderData);
                }
            }

            foreach(var item in selectedItems)
            {
                if (item.MeshRenderer.IsRenderedThisFrame)
                {
                    DrawEntityToGBuffer(item.MeshRenderer);
                }
            }
        }


        public float ShadowBrightness = 0.5f;

        void DrawEntityToGBuffer(vxCamera.RenderMeshEntry renderData)
        {
            var lightDir = vxEngine.Instance.GetCurrentScene<vxGameplayScene3D>().LightPositions;

            var material = renderData.material; // meshRenderer.GetMaterial(mi);

            //if (material.MaterialRenderPass == vxRenderPipeline.Passes.OpaquePass && material.IsDefferedRenderingEnabled ||
            //    material.MaterialRenderPass == vxRenderPipeline.Passes.TransparencyPass && material.IsTransparentDefferedRenderingEnabled)
            {
                material.UtilityEffect.CurrentTechnique = material.UtilityEffect.PrepPassTechnique; //.Techniques["Technique_PrepPass"];
                material.UtilityEffect.World = renderData.renderPassData.World;
                material.UtilityEffect.WVP = renderData.renderPassData.WVP;

                //if (vxCascadeShadowRenderPass.ShadowQaulity > vxEnumQuality.None)
                {
                    // Shadow Parameters
                    //material.UtilityEffect.ShadowBrightness = 1.0f;
                    //material.UtilityEffect.ShadowBlurStart = 2;
                    //material.UtilityEffect.ShadowMap = null;
                    //material.UtilityEffect.ShadowTransform = null;
                    //material.UtilityEffect.TileBounds = null;
                    material.UtilityEffect.LightDirection = lightDir;
                }

                renderData.mesh.Draw(material.UtilityEffect);
            }
        }


        void DrawEntityToGBuffer(vxMeshRenderer meshRenderer)
        {
            var lightDir = vxEngine.Instance.GetCurrentScene<vxGameplayScene3D>().LightPositions;
            if (meshRenderer.IsEnabled && meshRenderer.IsMainRenderingEnabled)
            {
                for (int mi = 0; mi < meshRenderer.Mesh.Meshes.Count; mi++)
                {
                    var material = meshRenderer.GetMaterial(mi);

                    //if (material.MaterialRenderPass == vxRenderPipeline.Passes.OpaquePass &&
                    //    material.IsDefferedRenderingEnabled ||
                    //    material.MaterialRenderPass == vxRenderPipeline.Passes.TransparencyPass &&
                    //    material.IsTransparentDefferedRenderingEnabled)
                    {
                        material.UtilityEffect.CurrentTechnique =
                            material.UtilityEffect.PrepPassTechnique; //.Techniques["Technique_PrepPass"];
                        material.UtilityEffect.World = meshRenderer.RenderPassData.World;
                        material.UtilityEffect.WVP = meshRenderer.RenderPassData.WVP;

                        //if (vxCascadeShadowRenderPass.ShadowQaulity > vxEnumQuality.None)
                        {
                            // Shadow Parameters
                            //material.UtilityEffect.ShadowBrightness = shadowPass.IsEnabled ? ShadowBrightness : 1.0f;
                            //material.UtilityEffect.ShadowBlurStart = 2;
                            //material.UtilityEffect.ShadowMap = shadowPass.ShadowMap;
                            //material.UtilityEffect.ShadowTransform = shadowPass.ShadowSplitProjectionsWithTiling;
                            //material.UtilityEffect.TileBounds = shadowPass.ShadowSplitTileBounds;
                            material.UtilityEffect.LightDirection = lightDir;
                        }

                        meshRenderer.Mesh.Meshes[mi].Draw(material.UtilityEffect);
                    }
                }
            }
        }

        public void Apply(vxCamera camera)
        {
            if (IsEdgeDetectionEnabled)
            {
                HalfPixel = new Vector2(.5f / (float)camera.Viewport.Width, .5f / (float)camera.Viewport.Height);

                if (vxEngine.Instance.CurrentScene.Cameras.Count > 1)
                {
                    NormalSensitivity = 10.0f / vxEngine.Instance.CurrentScene.Cameras.Count;
                    DepthSensitivity = 500 / vxEngine.Instance.CurrentScene.Cameras.Count;
                }

                //Set Render Target
                vxGraphics.GraphicsDevice.SetRenderTarget(Renderer.GetNewTempTarget("Edge Pass"));
                vxGraphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                vxGraphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                // Pass in the Normal Map
                SceneTexture = Renderer.GetCurrentTempTarget();
                NormalMap = SelectionNormalMap;
                RMAMap = SelectionSurface;

                this.SetEffectParameter("EdgeColour", Color.DarkOrange);
                // Pass in the Depth Map
                DepthMap = SelectionDepthMap;
                //SceneDepthMap = Renderer.AuxDepthMap;

                //EntityMaskSampler = Renderer.EntityMaskValues;
                EdgeIntensity = 1;
                EdgeWidth= 1.25f;
                // Activate the appropriate effect technique.
                Effect.CurrentTechnique = Effect.Techniques["EdgeDetect"];

                foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    Renderer.RenderQuad(Vector2.One * -1, Vector2.One);
                }

            }
        }
    }
}