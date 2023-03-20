
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

using VerticesEngine.Input;
using VerticesEngine.Utilities;

namespace VerticesEngine.Graphics
{
    /// <summary>
    /// Grab a scene that has already been rendered, 
    /// and add a distortion effect over the top of it.
    /// </summary>
    public class vxDebugRenderPass : vxRenderPass, vxIRenderPass
    {
       
        vxMainScene3DRenderPass mainPass;

        vxGBufferRenderingPass prepPass;

        vxCascadeShadowRenderPass shadowPass;

        internal vxEnumSceneDebugMode SceneDebugDisplayMode = vxEnumSceneDebugMode.Default;

        public RenderTarget2D DebugMap;

        public vxDebugRenderPass() : base("Debug Render Pass", vxInternalAssets.PostProcessShaders.DistortSceneEffect)
        {

        }

        protected override void OnInitialised()
        {
            base.OnInitialised();
            mainPass = Renderer.GetRenderingPass<vxMainScene3DRenderPass>();
            prepPass = Renderer.GetRenderingPass<vxGBufferRenderingPass>();
            shadowPass = Renderer.GetRenderingPass<vxCascadeShadowRenderPass>();
        }

        public override void OnGraphicsRefresh()
        {
            base.OnGraphicsRefresh();
            //vxInternalAssets.PostProcessShaders.DistortSceneEffect.Parameters["MatrixTransform"].SetValue(MatrixTransform);

            PresentationParameters pp = vxGraphics.GraphicsDevice.PresentationParameters;

            DebugMap = new RenderTarget2D(vxGraphics.GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, pp.BackBufferFormat, pp.DepthStencilFormat);

        }

        protected override void OnDisposed()
        {
            base.OnDisposed();

            //DebugMap.Dispose();
            //DebugMap = null;
        }

        public void Update()
        {

            // check for other keys pressed on keyboard
            if (vxInput.IsNewKeyPress(Keys.OemCloseBrackets))
            {
                var previousSceneShadowMode = SceneDebugDisplayMode;
                SceneDebugDisplayMode = vxUtil.NextEnumValue(SceneDebugDisplayMode);

                //if (previousSceneShadowMode < vxEnumSceneDebugMode.BlockPattern && SceneDebugDisplayMode >= vxEnumSceneDebugMode.BlockPattern ||

                //    previousSceneShadowMode >= vxEnumSceneDebugMode.BlockPattern && SceneDebugDisplayMode < vxEnumSceneDebugMode.BlockPattern)
                //{
                //    //Renderer.swapShadowMapWithBlockTexture();
                //}

                //foreach (vxEntity3D entity in Entities)
                //    entity.renderShadowSplitIndex = SceneDebugDisplayMode >= vxEnumSceneDebugMode.SplitColors;
            }

            if (vxInput.IsNewKeyPress(Keys.OemOpenBrackets))
            {
                var previousSceneShadowMode = SceneDebugDisplayMode;
                SceneDebugDisplayMode = vxUtil.PreviousEnumValue(SceneDebugDisplayMode);

                //if (previousSceneShadowMode < vxEnumSceneDebugMode.BlockPattern && SceneDebugDisplayMode >= vxEnumSceneDebugMode.BlockPattern ||

                //    previousSceneShadowMode >= vxEnumSceneDebugMode.BlockPattern && SceneDebugDisplayMode < vxEnumSceneDebugMode.BlockPattern)
                //{
                //    Renderer.swapShadowMapWithBlockTexture();
                //}

                //foreach (vxEntity3D entity in Entities)
                //    entity.renderShadowSplitIndex = SceneDebugDisplayMode >= vxEnumSceneDebugMode.SplitColors;
            }

        }

        public void Prepare(vxCamera camera)
        {
            switch (SceneDebugDisplayMode)
            {
                case vxEnumSceneDebugMode.PhysicsDebug:

                    vxGraphics.GraphicsDevice.SetRenderTarget(DebugMap);
                    vxGraphics.GraphicsDevice.Clear(Color.Black);
                    vxGraphics.GraphicsDevice.BlendState = BlendState.Opaque;
                    vxGraphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                    vxEngine.Instance.CurrentScene.DrawPhysicsDebug(camera);

                    break;

                case vxEnumSceneDebugMode.SplitColors:

                    vxGraphics.GraphicsDevice.SetRenderTarget(DebugMap);
                    vxGraphics.GraphicsDevice.Clear(Color.Black);
                    vxGraphics.GraphicsDevice.BlendState = BlendState.Opaque;
                    vxGraphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                    shadowPass.DrawDebug(camera);

                    break;
            }
        }

        public RasterizerState rs_wire = new RasterizerState() { FillMode = FillMode.WireFrame, };
        public RasterizerState rs_solid = new RasterizerState() { FillMode = FillMode.Solid, };

        public void Apply(vxCamera camera)
        {
            switch (SceneDebugDisplayMode)
            {
                case vxEnumSceneDebugMode.EncodedIndex:

                    vxGraphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                    vxGraphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                    vxGraphics.SpriteBatch.Begin("Physics Debug Call");
                    vxGraphics.SpriteBatch.Draw(Renderer.EncodedIndexResult, camera.Viewport.Bounds, Color.White);
                    vxGraphics.SpriteBatch.End();

                    DrawSceneWireFrame(Color.WhiteSmoke*0.025f);

                    // Draw Debug Text
                    AddDebugString("Encoded Index");
                    //AddDebugString("# of Physics Entities: " + PhyicsSimulation.Entities.Count);
                    DrawDebugStrings();
                    break;

                case vxEnumSceneDebugMode.SplitColors:

                    vxGraphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                    vxGraphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                    vxGraphics.SpriteBatch.Begin("Split Colours");
                    vxGraphics.SpriteBatch.Draw(DebugMap, DebugMap.Bounds, Color.White * 0.75f);
                    vxGraphics.SpriteBatch.End();

                    DrawSceneWireFrame(Color.WhiteSmoke * 0.025f);

                    // Draw Debug Text
                    AddDebugString("Split Colours");
                    //AddDebugString("# of Physics Entities: " + PhyicsSimulation.Entities.Count);
                    DrawDebugStrings();
                    break;


                case vxEnumSceneDebugMode.PhysicsDebug:

                    vxGraphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                    vxGraphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                    vxGraphics.SpriteBatch.Begin("Physics Debug Call");
                    vxGraphics.SpriteBatch.Draw(DebugMap, DebugMap.Bounds, Color.White * 0.5f);
                    vxGraphics.SpriteBatch.End();

                    DrawSceneWireFrame(Color.WhiteSmoke*0.125f);

                    // Draw Debug Text
                    AddDebugString("Physics Bodies");
                    //AddDebugString("# of Physics Entities: " + PhyicsSimulation.Entities.Count);
                    DrawDebugStrings();
                    break;
            }

        }

        List<string> DebugText = new List<string>();

        void DrawSceneWireFrame(Color wireColour)
        {
            //vxGraphics.GraphicsDevice.RasterizerState = rs_wire;

            //for (int i = 0; i < Renderer.totalItemsToDraw; i++)
            //{
            //    vxEntity3D entity = (vxEntity3D)vxEngine.Instance.CurrentScene.Entities[Renderer.drawList[i]];
            //    if (entity.IsEntityDrawable)
            //    {
            //        Matrix TempWorld = entity.WorldTransform;// * Renderer.Camera.ViewProjection;
            //        Matrix TempWVP = entity.WorldTransform * Renderer.Camera.ViewProjection;

            //        // TODO: Fix
            //        //for (int mi = 0; mi < entity.Model.Meshes.Count; mi++)
            //        //{
            //        //    var mesh = entity.Model.Meshes[mi];
            //        //    mesh.Material.DebugEffect.DoDebugWireFrame = true;
            //        //    mesh.Material.DebugEffect.WireColour = wireColour;
            //        //    //mesh.Material.DebugEffect.CurrentTechnique = entity.Model.Meshes[mi].Material.DebugEffect.Techniques["Technique_PrepPass"];
            //        //    mesh.Material.DebugEffect.World = TempWorld;
            //        //    mesh.Material.DebugEffect.WVP = TempWVP;

            //        //    mesh.Draw(mesh.Material.DebugEffect);
            //        //}
            //    }
            //}
            //vxGraphics.GraphicsDevice.RasterizerState = rs_solid;
        }

        public void AddDebugString(string text)
        {
            DebugText.Add(text);
        }

        public void DrawDebugStrings()
        {
            vxGraphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            vxGraphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Vector2 Padding = new Vector2(1);
            Vector2 Position = new Vector2(5);

            vxGraphics.SpriteBatch.Begin("Debug - Gameplay3D Text");

            // Now loop through all text and draw it
            foreach (string text in DebugText)
            {
                vxGraphics.SpriteBatch.DrawString(vxInternalAssets.Fonts.DebugFont, text, Position + Padding, Color.Black);
                vxGraphics.SpriteBatch.DrawString(vxInternalAssets.Fonts.DebugFont, text, Position, Color.White);
                Position += new Vector2(0, vxInternalAssets.Fonts.DebugFont.LineSpacing);
            }
            DebugText.Clear();

            vxGraphics.SpriteBatch.End();
        }
    }
}
