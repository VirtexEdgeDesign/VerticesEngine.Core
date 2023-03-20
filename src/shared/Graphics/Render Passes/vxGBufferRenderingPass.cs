#define MESH_RENDERERS
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace VerticesEngine.Graphics
{
    public class vxGBufferRenderingPass : vxRenderPass, vxIRenderPass
    {
        public RenderTarget2D CubeReflcMap;
        public RenderTarget2D DistortionMap;

        vxCascadeShadowRenderPass shadowPass;


        [vxGraphicalSettingsAttribute("NormalsQuality")]
        public static vxEnumQuality NormalsQuality = vxEnumQuality.Medium;

        [vxGraphicalSettingsAttribute("DoDistortion")]
        public static bool DoDistortion = true;


        public vxGBufferRenderingPass() : base("G Buffer Pass", vxInternalAssets.Shaders.PrepPassShader)
        {

        }
        protected override void OnInitialised()
        {
            base.OnInitialised();
            shadowPass = Renderer.GetRenderingPass<vxCascadeShadowRenderPass>();
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();
            
            CubeReflcMap.Dispose();
            DistortionMap.Dispose();
        }

        public override void OnGraphicsRefresh()
        {
            base.OnGraphicsRefresh();
            PresentationParameters pp = vxGraphics.GraphicsDevice.PresentationParameters;

            var viewport = vxGraphics.FinalViewport;
            CubeReflcMap = new RenderTarget2D(vxGraphics.GraphicsDevice, viewport.Width, viewport.Height, false,
                pp.BackBufferFormat, pp.DepthStencilFormat);
            DistortionMap = new RenderTarget2D(vxGraphics.GraphicsDevice, viewport.Width, viewport.Height, false,
                pp.BackBufferFormat, pp.DepthStencilFormat);
            Renderer.EntityMaskValues = new RenderTarget2D(vxGraphics.GraphicsDevice, viewport.Width, viewport.Height,
                false, pp.BackBufferFormat, pp.DepthStencilFormat);
        }

        public override void RegisterRenderTargetsForDebug()
        {
            base.RegisterRenderTargetsForDebug();

            Renderer.RegisterDebugRenderTarget("RMA Map", Renderer.SurfaceDataMap);
            Renderer.RegisterDebugRenderTarget("Normal Map", Renderer.NormalMap);
            Renderer.RegisterDebugRenderTarget("Depth Map", Renderer.DepthMap);
            Renderer.RegisterDebugRenderTarget("AuxDepthMap Map", Renderer.AuxDepthMap);
            Renderer.RegisterDebugRenderTarget("Distortion Map", DistortionMap);
            Renderer.RegisterDebugRenderTarget("EncodedIndexResult Map", Renderer.EncodedIndexResult);
            Renderer.RegisterDebugRenderTarget("EntityMaskValues Map", Renderer.EntityMaskValues);
        }

        public void Prepare(vxCamera camera)
        {
            GeneratePrePass(camera);
            GenerateDataMaskPass(camera);
        }

        void GeneratePrePass(vxCamera camera)
        {
            // Set Multi Rendertargets
            vxGraphics.GraphicsDevice.SetRenderTargets(
                Renderer.SurfaceDataMap,
                Renderer.NormalMap,
                Renderer.DepthMap);


            //Setup initial graphics states for prep pass            
            vxGraphics.GraphicsDevice.BlendState = BlendState.Opaque;
            vxGraphics.GraphicsDevice.DepthStencilState = DepthStencilState.None;

            //Reset Appropriate Values for Rendertargets
            vxInternalAssets.PostProcessShaders.DrfrdRndrClearGBuffer.Techniques[0].Passes[0].Apply();
            Renderer.RenderQuad(Vector2.One * -1, Vector2.One);

            //Set the Depth Buffer appropriately
            vxGraphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            if (camera.IsRenderListEnabled)
            {
                for (int i = 0; i < camera.totalItemsToDraw; i++)
                {
                    var renderData = camera.renderList[i];
                    DrawEntityToGBuffer(renderData);
                }
            }
            else
            {
                // render everything for this pass except particles
                for (int i = 0; i < camera.totalItemsToDraw; i++)
                {
                    int drawIndex = camera.drawList[i];
                    if (drawIndex < vxEngine.Instance.CurrentScene.MeshRenderers.Count)
                    {
                        var meshRenderer = vxEngine.Instance.CurrentScene.MeshRenderers[drawIndex];
                        DrawEntityToGBuffer(meshRenderer);
                    }
                }
            }

            foreach (var poolKeyPair in vxEngine.Instance.CurrentScene.ParticleSystem.ParticlePools)
            {
                foreach (var prtcl in poolKeyPair.Value.Pool)
                {
                    if (prtcl.IsAlive)
                    {
                        DrawEntityToGBuffer(((vxEntity3D)prtcl).MeshRenderer);
                    }
                }
            }
        }


        public float ShadowBrightness = 0.5f;

        void DrawEntityToGBuffer(vxCamera.RenderMeshEntry renderData)
        {
            var lightDir = vxEngine.Instance.GetCurrentScene<vxGameplayScene3D>().LightPositions;

            var material = renderData.material; // meshRenderer.GetMaterial(mi);

            if (material.MaterialRenderPass == vxRenderPipeline.Passes.OpaquePass && material.IsDefferedRenderingEnabled ||
                material.MaterialRenderPass == vxRenderPipeline.Passes.TransparencyPass && material.IsTransparentDefferedRenderingEnabled)
            {
                material.UtilityEffect.CurrentTechnique = material.UtilityEffect.PrepPassTechnique; //.Techniques["Technique_PrepPass"];
                material.UtilityEffect.World = renderData.renderPassData.World;
                material.UtilityEffect.WVP = renderData.renderPassData.WVP;

                //if (vxCascadeShadowRenderPass.ShadowQaulity > vxEnumQuality.None)
                {
                    // Shadow Parameters
                    material.UtilityEffect.ShadowBrightness = shadowPass.IsEnabled ? ShadowBrightness : 1.0f;
                    material.UtilityEffect.ShadowBlurStart = 2;
                    material.UtilityEffect.ShadowMap = shadowPass.ShadowMap;
                    material.UtilityEffect.ShadowTransform = shadowPass.ShadowSplitProjectionsWithTiling;
                    material.UtilityEffect.TileBounds = shadowPass.ShadowSplitTileBounds;
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

                    if (material.MaterialRenderPass == vxRenderPipeline.Passes.OpaquePass &&
                        material.IsDefferedRenderingEnabled ||
                        material.MaterialRenderPass == vxRenderPipeline.Passes.TransparencyPass &&
                        material.IsTransparentDefferedRenderingEnabled)
                    {
                        material.UtilityEffect.CurrentTechnique =
                            material.UtilityEffect.PrepPassTechnique; //.Techniques["Technique_PrepPass"];
                        material.UtilityEffect.World = meshRenderer.RenderPassData.World;
                        material.UtilityEffect.WVP = meshRenderer.RenderPassData.WVP;

                        //if (vxCascadeShadowRenderPass.ShadowQaulity > vxEnumQuality.None)
                        {
                            // Shadow Parameters
                            material.UtilityEffect.ShadowBrightness = shadowPass.IsEnabled ? ShadowBrightness : 1.0f;
                            material.UtilityEffect.ShadowBlurStart = 2;
                            material.UtilityEffect.ShadowMap = shadowPass.ShadowMap;
                            material.UtilityEffect.ShadowTransform = shadowPass.ShadowSplitProjectionsWithTiling;
                            material.UtilityEffect.TileBounds = shadowPass.ShadowSplitTileBounds;
                            material.UtilityEffect.LightDirection = lightDir;
                        }

                        meshRenderer.Mesh.Meshes[mi].Draw(material.UtilityEffect);
                    }
                }
            }
        }


        void GenerateDataMaskPass(vxCamera camera)
        {
            // Set Multi Rendertargets
            vxGraphics.GraphicsDevice.SetRenderTargets(
                Renderer.EncodedIndexResult,
                Renderer.EntityMaskValues,
                Renderer.AuxDepthMap,
                DistortionMap);

            vxGraphics.GraphicsDevice.BlendState = BlendState.Opaque;
            vxGraphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            vxGraphics.GraphicsDevice.Clear(Color.Transparent);

            if (camera.IsRenderListEnabled)
            {
                for (int i = 0; i < camera.totalItemsToDraw; i++)
                {
                    var renderData = camera.renderList[i];
                    DrawEntityToAuxGBuffer(renderData);
                }
            }
            else
            {
                // render everything for this pass except particles
                for (int i = 0; i < camera.totalItemsToDraw; i++)
                {
                    int drawIndex = camera.drawList[i];
                    if (drawIndex < vxEngine.Instance.CurrentScene.MeshRenderers.Count)
                    {
                        var meshRenderer = vxEngine.Instance.CurrentScene.MeshRenderers[drawIndex];
                        DrawEntityToAuxGBuffer(meshRenderer);
                    }
                }
            }

            foreach (var poolKeyPair in vxEngine.Instance.CurrentScene.ParticleSystem.ParticlePools)
            {
                foreach (var prtcl in poolKeyPair.Value.Pool)
                {
                    if (prtcl.IsAlive)
                    {
                        DrawEntityToAuxGBuffer(((vxEntity3D)prtcl).MeshRenderer);
                    }
                }
            }

            vxGraphics.GraphicsDevice.DepthStencilState = DepthStencilState.None;
            vxGraphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            foreach (var entity in vxEngine.Instance.CurrentScene.EditorEntities)
            {
                DrawEntityToAuxGBuffer(entity);
            }
        }

        void DrawEntityToAuxGBuffer(vxEntity3D entity)
        {
            if (entity.IsVisible && entity.IsEnabled && entity.Model != null)
            {
                for (int mi = 0; mi < entity.Model.Meshes.Count; mi++)
                {
                    var material = entity.MeshRenderer.GetMaterial(mi);

                    material.UtilityEffect.CurrentTechnique =
                        material.UtilityEffect.DataMaskTechnique; //.Techniques["Technique_DataMaskPass"];

                    //if(material.MaterialRenderPass == vxRenderPipeline.Passes.TransparencyPass)
                    //{
                    //    vxConsole.WriteLine(entity);
                    //}

                    material.UtilityEffect.IsTransparent =
                        (material.MaterialRenderPass == vxRenderPipeline.Passes.TransparencyPass);
                    material.UtilityEffect.IndexEncodedColour = entity.IndexEncodedColour;
                    material.UtilityEffect.World = entity.Transform.Matrix4x4Transform;
                    material.UtilityEffect.WVP = entity.Transform.RenderPassData.WVP;

                    entity.Model.Meshes[mi].Draw(material.UtilityEffect);
                }
            }
        }

        void DrawEntityToAuxGBuffer(vxMeshRenderer meshRenderer)
        {
            if (meshRenderer.IsEnabled && meshRenderer.IsMainRenderingEnabled)
            {
                for (int mi = 0; mi < meshRenderer.Mesh.Meshes.Count; mi++)
                {
                    var material = meshRenderer.GetMaterial(mi);

                    material.UtilityEffect.CurrentTechnique =
                        material.UtilityEffect.DataMaskTechnique; //.Techniques["Technique_DataMaskPass"];

                    //if (material.MaterialRenderPass == vxRenderPipeline.Passes.TransparencyPass)
                    //{
                    //    vxConsole.WriteLine(entity);
                    //}

                    material.UtilityEffect.IsTransparent =
                        (material.MaterialRenderPass == vxRenderPipeline.Passes.TransparencyPass);
                    material.UtilityEffect.IndexEncodedColour = meshRenderer.IndexEncodedColour;
                    material.UtilityEffect.World = meshRenderer.RenderPassData.World;
                    material.UtilityEffect.WVP = meshRenderer.RenderPassData.WVP;

                    meshRenderer.Mesh.Meshes[mi].Draw(material.UtilityEffect);
                }
            }
        }
        
        void DrawEntityToAuxGBuffer(vxCamera.RenderMeshEntry renderData)
        {
            // if (meshRenderer.IsEnabled && meshRenderer.IsMainRenderingEnabled)
            {
                // for (int mi = 0; mi < meshRenderer.Mesh.Meshes.Count; mi++)
                {
                    var material = renderData.material;// meshRenderer.GetMaterial(mi);

                    material.UtilityEffect.CurrentTechnique =
                        material.UtilityEffect.DataMaskTechnique; //.Techniques["Technique_DataMaskPass"];

                    //if (material.MaterialRenderPass == vxRenderPipeline.Passes.TransparencyPass)
                    //{
                    //    vxConsole.WriteLine(entity);
                    //}

                    material.UtilityEffect.IsTransparent =
                        (material.MaterialRenderPass == vxRenderPipeline.Passes.TransparencyPass);
                    material.UtilityEffect.IndexEncodedColour = renderData.renderPassData.IndexColour;
                    material.UtilityEffect.World = renderData.renderPassData.World;
                    material.UtilityEffect.WVP = renderData.renderPassData.WVP;

                    renderData.mesh.Draw(material.UtilityEffect);
                }
            }
        }

        public void Apply(vxCamera camera)
        {
            //
        }
    }
}