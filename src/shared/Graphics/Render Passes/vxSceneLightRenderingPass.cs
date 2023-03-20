using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VerticesEngine.Graphics
{
    /// <summary>
    /// The Scene Lighting Pass applies all lighting including both applying shadows and lights to the scene
    /// </summary>
    public class vxSceneLightRenderingPass : vxRenderPass, vxIRenderPass
    {
        /// <summary>
        /// Light Map Render Target.
        /// </summary>
        RenderTarget2D LightMap;

        /// <summary>
        /// Shadow Pass
        /// </summary>
        vxCascadeShadowRenderPass shadowPass;

        vxMainScene3DRenderPass mainPass;

        /// <summary>
        /// Scene Lighting Pass
        /// </summary>
        /// <param name="Renderer"></param>
        public vxSceneLightRenderingPass() : base("Scene Light Pass", vxInternalAssets.PostProcessShaders.LightingCombine)
        {

            PresentationParameters pp = vxGraphics.GraphicsDevice.PresentationParameters;

            LightMap = new RenderTarget2D(vxGraphics.GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, pp.BackBufferFormat, pp.DepthStencilFormat);
            _deferredLitScene = new RenderTarget2D(vxGraphics.GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, pp.BackBufferFormat, pp.DepthStencilFormat);


        }

        protected override void OnInitialised()
        {
            base.OnInitialised();
            shadowPass = Renderer.GetRenderingPass<vxCascadeShadowRenderPass>();
            mainPass = Renderer.GetRenderingPass<vxMainScene3DRenderPass>();
        }
        public override void RegisterRenderTargetsForDebug()
        {
            base.RegisterRenderTargetsForDebug();

            //Renderer.RegisterDebugRenderTarget("Light Map", _deferredLitScene);
            //Renderer.RegisterDebugRenderTarget("Light Map", LightMap);

        }

        RenderTarget2D _deferredLitScene;

        public void Prepare(vxCamera camera)
        {
            // TODO: Genereate Light Map
        }
        public void Apply(vxCamera camera) {

            // pass in the RMA map and light map and apply it to the scene
            vxGraphics.GraphicsDevice.SetRenderTarget(Renderer.GetNewTempTarget("Lighting Pass"));

            vxGraphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            vxGraphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            //vxGraphics.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            Parameters["SceneMap"].SetValue(Renderer.GetCurrentTempTarget());
            Parameters["SurfaceDataMap"].SetValue(Renderer.SurfaceDataMap);
            //Parameters["EntityMaskMap"].SetValue(Renderer.EntityMaskValues);

            //EntityMaskSampler = Renderer.EntityMaskValues;

            Effect.CurrentTechnique = Effect.Techniques["BasicColorDrawing"];
            foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Renderer.RenderQuad(Vector2.One * -1, Vector2.One);
            }
        }
    }
}


// NB:
/*
 * 
    1) Shaders using this methodlogy can use a regular Vertex Shader and don't need to register the samplers 
    
    foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
    {
        pass.Apply();
        Renderer.RenderQuad(Vector2.One * -1, Vector2.One);
    }


    2) Shaders using the below method need to have their samplers registered and the vertex shader must have the MatrixTransform setup

    DrawFullscreenQuad("PostProcess.FinalScene.Apply()", Renderer.GetCurrentTempTarget(), vxGraphics.GraphicsDevice.Viewport.Width, vxGraphics.GraphicsDevice.Viewport.Height, Effect);
     
     */
