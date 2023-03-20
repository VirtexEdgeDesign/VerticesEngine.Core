using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using VerticesEngine.Diagnostics;
using VerticesEngine.Input;
using VerticesEngine.Utilities;

namespace VerticesEngine.Graphics
{
    /// <summary>
    /// This component renders a scene given the camera
    /// </summary>
    public class vxRenderPipeline : vxGameObject
    {
        #region Engine Constants
        /// <summary>
        /// Collection of Engine Passes
        /// </summary>
        public static class Passes
        {
            public const string OpaquePass = "OpaquePass";

            public const string TransparencyPass = "TransparencyPass";

            public const string EditorEntityPass = "EditorEntities";

            //public const string ParticlePrePass = "ParticlePrePass";

            //public const string ParticlePostPass = "ParticlePostPass";
        }
        #endregion


        /// <summary>
        /// A list of rendering passes to be applied to by this camera
        /// </summary>
        private List<vxIRenderPass> RenderingPasses = new List<vxIRenderPass>();



        #region -- Render Quad Fields --

        /// <summary>
        /// The quad renderer vertices buffer.
        /// </summary>
        VertexPositionTexture[] quadRendererVerticesBuffer = null;

        /// <summary>
        /// The quad renderer index buffer.
        /// </summary>
        short[] quadRendererIndexBuffer = null;

        #endregion
        
        #region -- Render Targets --

        // Public Render Targets
        // **********************************************


        /// <summary>
        /// Normal Render Target.
        /// </summary>
        public RenderTarget2D NormalMap;

        /// <summary>
        /// Depth Render Target.
        /// </summary>
        public RenderTarget2D DepthMap;




        // Internal Render Targets
        // **********************************************

        /// <summary>
        ///Render Target which holds Surface Data such as Specular Power, Intensity as well as Shadow Factor.
        /// </summary>
        public RenderTarget2D SurfaceDataMap;


        /// <summary>
        /// This render target holds mask information for different entities (i.e. do edge detection? do motion blur, etc...)
        /// </summary>
        public RenderTarget2D EntityMaskValues;

        /// <summary>
        /// Encoded Index Result. This is needed only internally
        /// </summary>
        internal RenderTarget2D EncodedIndexResult;


        /// <summary>
        /// Aux Depth Map
        /// </summary>
        public RenderTarget2D AuxDepthMap;


        public RenderTarget2D BlurredScene;

        #region - Temp Targets -

        /// <summary>
        /// The post process temp targets collection.
        /// </summary>
        public RenderTarget2D[] PostProcessTargets;// = new List<RenderTarget2D>();

        // Temp Targets Index
        int tempTargetIndex = 0;

        /// <summary>
        /// The number of temp targets used.
        /// </summary>
        public int TempTargetsUsed = 0;

        public int TempTargetCount = 12;

        #endregion

        #endregion


        /// <summary>
        /// Has this render pipeline been initialised yet?
        /// </summary>
        public bool IsInitialised
        {
            get { return _isInitialised; }
        }
        private bool _isInitialised = false;


        public static vxRenderPipeline Instance
        {
            get { return m_instance; }
        }
        private static vxRenderPipeline m_instance = new vxRenderPipeline();

        /// <summary>
        /// Light Direction. In 2D only 'x' and 'y' are taken
        /// </summary>
        public Vector3 LightDirection;

        private vxRenderPipeline()
        {
            // quad renderer vert buffer
            quadRendererVerticesBuffer = new VertexPositionTexture[]
            {
                new VertexPositionTexture(new Vector3(0,0,0), new Vector2(1,1)),
                new VertexPositionTexture(new Vector3(0,0,0), new Vector2(0,1)),
                new VertexPositionTexture(new Vector3(0,0,0), new Vector2(0,0)),
                new VertexPositionTexture(new Vector3(0,0,0), new Vector2(1,0))
            };

            // quad renderer index buffer
            quadRendererIndexBuffer = new short[] { 0, 1, 2, 2, 3, 0 };
        }

        /// <summary>
        /// Initialises the renderer. 
        /// </summary>
        internal void Initialise()
        {
            _isInitialised = true;

            foreach (var pass in RenderingPasses)
            {
                if(pass is vxRenderPass)
                {
                    ((vxRenderPass)pass).Initialise(this);
                }    
            }

            OnGraphicsRefresh();
        }

        void DisposeRenderTargets()
        {
            if (PostProcessTargets != null)
                for (int p = 0; p < PostProcessTargets.Length; p++)
                {
                    PostProcessTargets[p].Dispose();
                    PostProcessTargets[p] = null;
                }

            if (NormalMap != null)
                NormalMap.Dispose();
            NormalMap = null;
            if (DepthMap != null)
                DepthMap.Dispose();
            DepthMap = null;
            if (SurfaceDataMap != null)
                SurfaceDataMap.Dispose();
            SurfaceDataMap = null;
            if (AuxDepthMap != null)
                AuxDepthMap.Dispose();
            AuxDepthMap = null;
            if (EncodedIndexResult != null)
                EncodedIndexResult.Dispose();
            EncodedIndexResult = null;
            if (BlurredScene != null)
                BlurredScene.Dispose();
            BlurredScene = null;


            if (m_debugRenderTargets != null)
                for (int r = 0; r < m_debugRenderTargets.Count; r++)
                {
                    m_debugRenderTargets[r].Dispose();
                    m_debugRenderTargets[r] = null;
                }
        }

        int prevWidth = 0;
        int prevHeight = 0;
        int cnt;
        public override void OnGraphicsRefresh()
        {
            base.OnGraphicsRefresh();

            var viewport = vxGraphics.FinalViewport;

            if (prevWidth == viewport.Width && prevHeight == viewport.Height)
                return;
            
            DisposeRenderTargets();

            prevWidth =viewport.Width;
            prevHeight = viewport.Height;

            vxDebug.Log(new
            {
                src=this.GetType(),
                graphicsRefreshCount=cnt++
            });

#if __MOBILE__
            DepthMap = new RenderTarget2D(vxGraphics.GraphicsDevice, viewport.Width, viewport.Height, false, SurfaceFormat.Color, DepthFormat.None);
            AuxDepthMap = new RenderTarget2D(vxGraphics.GraphicsDevice, viewport.Width, viewport.Height, false, SurfaceFormat.Color, DepthFormat.None);
#else
            DepthMap = new RenderTarget2D(vxGraphics.GraphicsDevice, viewport.Width, viewport.Height, false, SurfaceFormat.Single, DepthFormat.None);
            AuxDepthMap = new RenderTarget2D(vxGraphics.GraphicsDevice, viewport.Width, viewport.Height, false, SurfaceFormat.Single, DepthFormat.None);
#endif
            NormalMap = new RenderTarget2D(vxGraphics.GraphicsDevice, viewport.Width, viewport.Height, false, SurfaceFormat.Color, DepthFormat.None);
            SurfaceDataMap = new RenderTarget2D(vxGraphics.GraphicsDevice, viewport.Width, viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            EncodedIndexResult = new RenderTarget2D(vxGraphics.GraphicsDevice, viewport.Width, viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);

            BlurredScene = new RenderTarget2D(vxGraphics.GraphicsDevice, viewport.Width, viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);



            EncodedIndexPixels = new Color[EncodedIndexResult.Width * EncodedIndexResult.Height];

            var tempTrgts = new List<RenderTarget2D>();

#if __ANDROID__ || __IOS__
            //var w = vxEngine.CurrentGame.GraphicsDeviceManager.PreferredBackBufferWidth;
            //var h = vxEngine.CurrentGame.GraphicsDeviceManager.PreferredBackBufferHeight;
            PresentationParameters pp = vxGraphics.GraphicsDevice.PresentationParameters;
            //Console.WriteLine("CREATING: " + w + "," + h);

            for (int i = 0; i < TempTargetCount; i++)
            {
                tempTrgts.Add(new RenderTarget2D(vxGraphics.GraphicsDevice, vxScreen.Width, vxScreen.Height, false,
                                                      pp.BackBufferFormat, pp.DepthStencilFormat));
            }

            //PostProcessTargets[i] = new RenderTarget2D(GraphicsDevice, w, h, false,
                                                  //pp.BackBufferFormat, pp.DepthStencilFormat);
#else
            PresentationParameters pp = vxGraphics.GraphicsDevice.PresentationParameters;
            
            for (int i = 0; i < TempTargetCount; i++)
            {
                tempTrgts.Add(vxGraphics.GetNewRenderTarget(vxGraphics.FinalViewport.Width, vxGraphics.FinalViewport.Height));
            }
#endif
            PostProcessTargets = tempTrgts.ToArray();



            foreach (var pass in RenderingPasses)
                pass.OnGraphicsRefresh();

            if(m_debugRenderTargets != null)
                m_debugRenderTargets.Clear();

            foreach (var pass in RenderingPasses)
                pass.RegisterRenderTargetsForDebug();

        }

        /// <summary>
        /// This renders the scene for the given camera
        /// </summary>
        /// <param name="camera"></param>
        public void RenderScene(vxCamera camera)
        {
            tempTargetIndex = 0;
            // perform all prep work here
            for (int p = 0; p< RenderingPasses.Count; p++)
                RenderingPasses[p].Prepare(camera);


            // now apply the rendering passes
            for (int p = 0; p < RenderingPasses.Count; p++)
                RenderingPasses[p].Apply(camera);

        }

        public void DrawDebug()
        {
            if (vxDebug.IsDebugRenderTargetsVisible)
                DrawDebugRenderTargets();
        }


        protected override void OnDisposed()
        {
            base.OnDisposed();

            m_debugRenderTargets.Clear();
            m_debugRenderTargets = null;

            System.GC.Collect();
        }

        #region -- Utility Functions --


        /// <summary>
        /// Gets a new temp target.
        /// </summary>
        /// <returns>The new temp target.</returns>
        public RenderTarget2D GetNewTempTarget(string name)
        {
            tempTargetIndex++;
            PostProcessTargets[tempTargetIndex].Name = name;
            return PostProcessTargets[tempTargetIndex];
        }

        /// <summary>
        /// Gets the current temp target. This pushes the stack forward. Use peek if you arent' drawing with this one to the new one.
        /// </summary>
        /// <returns>The current temp target.</returns>
        public RenderTarget2D GetCurrentTempTarget()
        {
            return PostProcessTargets[tempTargetIndex-1];
        }

        public virtual RenderTarget2D Finalise()
        {            
            TempTargetsUsed = tempTargetIndex;

            if (vxEngine.BuildType == vxBuildType.Debug)
                vxConsole.WriteToScreen(this, "Temp Targets Used: " + TempTargetsUsed);

            return PostProcessTargets[tempTargetIndex];
        }

        /// <summary>
        /// Render the specified v1 and v2.
        /// </summary>
        /// <param name="v1">V1.</param>
        /// <param name="v2">V2.</param>
        public void RenderQuad(Vector2 v1, Vector2 v2)
        {
            quadRendererVerticesBuffer[0].Position.X = v2.X;
            quadRendererVerticesBuffer[0].Position.Y = v1.Y;

            quadRendererVerticesBuffer[1].Position.X = v1.X;
            quadRendererVerticesBuffer[1].Position.Y = v1.Y;

            quadRendererVerticesBuffer[2].Position.X = v1.X;
            quadRendererVerticesBuffer[2].Position.Y = v2.Y;

            quadRendererVerticesBuffer[3].Position.X = v2.X;
            quadRendererVerticesBuffer[3].Position.Y = v2.Y;

            vxGraphics.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, quadRendererVerticesBuffer, 0, 4, quadRendererIndexBuffer, 0, 2);
        }

        /// <summary>
        /// Returns a Rendering Pass that is registered to this Renderer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetRenderingPass<T>() where T : vxIRenderPass
        {
            if(!_isInitialised)
            {
                throw new Exception("You cannot get a render pass until the Render Pipeline is fully initialised. Make sure you're calling 'GetRenderingPass' in the OnInitialised() method?");
            }

            foreach (var pass in RenderingPasses)
            {
                if (pass.GetType() == typeof(T))
                    return (T)pass;
            }
            return default(T);
        }

        /// <summary>
        /// Add a render feature to this renderer.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>A reference to the created render feature.</returns>
        public T AddRenderFeature<T>() where T : vxIRenderPass
        {
            // create a new component
            var renderFeature = (T)Activator.CreateInstance(typeof(T));

            // add it to the list
            this.RenderingPasses.Add(renderFeature);

            return renderFeature;
        }

        /// <summary>
        /// Inserts a render feature to this renderer.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="insertIndex"></param>
        /// <returns>A reference to the created render feature.</returns>
        public T AddRenderFeature<T>(int insertIndex) where T : vxIRenderPass
        {
            // create a new component
            var renderFeature = (T)Activator.CreateInstance(typeof(T));

            // add it to the list
            this.RenderingPasses.Insert(insertIndex, renderFeature);

            return renderFeature;
        }

        public Color GetEncodedIndex(int x, int y)
        {
            var i = (y * EncodedIndexResult.Width) + x;
            EncodedIndexResult.GetData<Color>(EncodedIndexPixels);
            
            return i < EncodedIndexPixels.Length ? EncodedIndexPixels[i] : Color.Black;
        }

        Color[] EncodedIndexPixels;// = new Color[rt.Width * rt.Height];

        #endregion

        #region -- Debug --

        protected internal Vector2 debugRTPos = new Vector2(0, 0);
        protected internal int debugRTWidth = 200;
        protected internal int debugRTHeight = 200;
        protected internal int debugRTPadding = 2;
        protected internal int debugRTScale = 4;
        protected internal int rtDb_count = 0;

        class RenderTargetDebug
        {
            public string name;

            public RenderTarget2D renderTarget;

            public RenderTargetDebug(string name, RenderTarget2D renderTarget)
            {
                this.name = name;
                this.renderTarget = renderTarget;
            }
        }

        List<RenderTarget2D> m_debugRenderTargets = new List<RenderTarget2D>();

        
        public void RegisterDebugRenderTarget(string name, RenderTarget2D renderTarget)
        {
            renderTarget.Name = name;

            if (m_debugRenderTargets == null)
                m_debugRenderTargets = new List<RenderTarget2D>();

            m_debugRenderTargets.Add(renderTarget);
        }
        

        void DrawDebugRenderTargets()
        {

            SpriteFont font = vxInternalAssets.Fonts.DebugFont;


            if (vxInput.IsKeyDown(Keys.OemPlus))
                debugRTPos -= Vector2.UnitX * 15;
            else if (vxInput.IsKeyDown(Keys.OemMinus))
                debugRTPos += Vector2.UnitX * 15;


            // Clamp Debug Position
            debugRTPos.X = MathHelper.Clamp(debugRTPos.X, -debugRTWidth * rtDb_count, 0);
            rtDb_count = 0;

            vxGraphics.SpriteBatch.Begin("Debug.RenderTargetsDump", 0, BlendState.Opaque, SamplerState.PointClamp, null, null);

            
            vxGraphics.SpriteBatch.Draw(DefaultTexture, new Rectangle(0, 0, vxGraphics.GraphicsDevice.Viewport.Width, debugRTHeight + font.LineSpacing + 2 * debugRTPadding), Color.Black * 0.5f);


            // first draw all registered RTs
            if(m_debugRenderTargets != null)
            foreach(var rt in m_debugRenderTargets)
            {
                DrawRT(rt.Name, rt);
            }

            for (int i = 0; i < TempTargetsUsed; i++)
            {
                var trgt = PostProcessTargets[i];
                if (trgt != null)
                    DrawRT(trgt.Name, trgt);
            }

            vxGraphics.SpriteBatch.End();
        }

        public void DrawRT(string name, RenderTarget2D rt)
        {
            if (rt != null)
            {
                var viewport = vxGraphics.FinalViewport;
                SpriteFont font = vxInternalAssets.Fonts.DebugFont;
                debugRTHeight = viewport.Height / debugRTScale;
                debugRTWidth = viewport.Width / debugRTScale;//* debugRTHeight / Viewport.Height;

                Point rtDb_pos = new Point(rtDb_count * debugRTWidth, 0) + debugRTPos.ToPoint();
                vxGraphics.SpriteBatch.Draw(rt, new Rectangle(rtDb_pos.X, rtDb_pos.Y + 16, debugRTWidth, debugRTHeight), Color.White);
                //vxGraphics.SpriteBatch.Draw(vxInternalAssets.Textures.Blank, new Rectangle(rtDb_pos.X, rtDb_pos.Y + 16, debugRTWidth/2, debugRTHeight/2), Color.Pink);
                //vxGraphics.SpriteBatch.Draw(rt, Viewport.Bounds, Color.White);
                if (name != null)
                {
                    string txt = $"{name} : {rt.Width}x{rt.Height}";
                    //DrawDebugStrings();
                    var pos = new Vector2(debugRTWidth / 2 - font.MeasureString(txt).X / 2,
                                          debugRTHeight + debugRTPadding) + rtDb_pos.ToVector2();


                    pos = rtDb_pos.ToVector2() + Vector2.One;
                    //vxGraphics.SpriteBatch.Draw(DefaultTexture, new Rectangle(0,0, Viewport.Width, 24), Color.Black);

                    // vxGraphics.SpriteBatch.DrawString(font, name, pos + Vector2.One, Color.Black);
                    vxGraphics.SpriteBatch.DrawString(font, txt, pos, Color.White);
                    rtDb_count++;
                }
            }
        }

        #endregion
    }
}
