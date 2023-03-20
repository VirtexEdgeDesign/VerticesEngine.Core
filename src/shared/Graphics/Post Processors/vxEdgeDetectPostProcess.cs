using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VerticesEngine.Graphics
{
    public class vxEdgeDetectPostProcess : vxRenderPass, vxIRenderPass
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

        public RenderTarget2D EntityMaskSampler
        {
            set { Parameters["EntityMaskMap"].SetValue(value); }
        }

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

        public vxEdgeDetectPostProcess() :base("Edge Detect", vxInternalAssets.PostProcessShaders.CartoonEdgeDetect)
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

        public void Prepare(vxCamera camera) { }

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
                NormalMap = Renderer.NormalMap;
                RMAMap = Renderer.SurfaceDataMap;
                

                // Pass in the Depth Map
                DepthMap = Renderer.DepthMap;

                EntityMaskSampler = Renderer.EntityMaskValues;

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