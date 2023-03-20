
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VerticesEngine.Graphics
{

    /// <summary>
    /// The base class for creating a Post Processor for the Vertices Engine
    /// </summary>
    public abstract class vxRenderPass : vxGameObject
    {
        /// <summary>
        /// This represents a Render pass int which can be overrid to pas a custom designed
        /// enum for specifing the rendering pass.
        /// </summary>
        public virtual string RenderPass
        {
            get { return "renderpass_" + this.ToString(); }
        }

        /// <summary>
        /// The name of this post processor.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }        
        private string _name = "";


        /// <summary>
        /// Has this render feature been initialised yet? Note that render features must be added during game startup in the <see cref="vxGame.OnRenderPipelineInitialised(vxRenderPipeline)"/> method.
        /// </summary>
        public bool IsInitialised
        {
            get { return _isInitialised; }
        }
        private bool _isInitialised = false;

        /// <summary>
        /// The main effect for this Post Process.
        /// </summary>
        public Effect Effect
        {
            get
            {
                return _effect;
            }
        }
        private Effect _effect;

        /// <summary>
        /// The parameters for the main effect.
        /// </summary>
        public EffectParameterCollection Parameters
        {
            get { return _effect.Parameters; }
        }

        /// <summary>
        /// The renderering engine which will be using this Post Process.
        /// </summary>
        public vxRenderPipeline Renderer
        {
            get { return _renderer; }
        }
        private vxRenderPipeline _renderer;


        public Vector2 ScreenResolution
        {
            set
            {
                if(Parameters["ScreenResolution"] != null)
                    Parameters["ScreenResolution"].SetValue(value);
            }
        }

        public Matrix OrthogonalProjection;

        public Matrix HalfPixelOffset;

        //[vxPostProcessingPropertyAttribute(Title = "Half Pixel Size")]
        public Vector2 HalfPixel
        {
            get { return _halfPixel; }
            set
            {
                _halfPixel = value;
                if (Parameters["HalfPixel"] != null)
                    Parameters["HalfPixel"].SetValue(value); }
        }
        public Vector2 _halfPixel;

        public Matrix MatrixTransform
        {
            get { return _matrixTransform; }
            set
            {
                _matrixTransform = value;
                if (Parameters["MatrixTransform"] != null)
                    Parameters["MatrixTransform"].SetValue(value);
            }
        }
        Matrix _matrixTransform;


        /// <summary>
        /// Constructor for a render feature.
        /// </summary>
        /// <param name="name">The human readable name for this render feature.</param>
        /// <param name="shader">The main <see cref="Microsoft.Xna.Framework.Graphics.Effect"/> to used with this render feature.</param>
        public vxRenderPass(string name, Effect shader)
        {            
            _name = name;
            _effect = shader;
        }

        /// <summary>
        /// Initialises the render feature.
        /// </summary>
        /// <param name="Renderer"></param>
        internal void Initialise(vxRenderPipeline Renderer)
        {
            _renderer = Renderer;

            OnInitialised();

            _isInitialised = true;
        }

        /// <summary>
        /// Called when the render pipeline is initialised. This is where you can query the renderer for other render passes.
        /// </summary>
        protected virtual void OnInitialised() { }

        protected override void OnDisposed()
        {
            base.OnDisposed();
            _renderer = null;
        }

        /// <summary>
        /// Register any render targets with the debug system here
        /// </summary>
        public virtual void RegisterRenderTargetsForDebug() { }

        public virtual void LoadContent()
        {
            OnGraphicsRefresh();
        }

        public virtual void RefreshSettings()
        {

        }



        public override void OnGraphicsRefresh()
        {
            base.OnGraphicsRefresh();

            var viewport = vxGraphics.FinalViewport;

            ScreenResolution = new Vector2(viewport.Width, viewport.Height);
            
            OrthogonalProjection = Matrix.CreateOrthographicOffCenter(0,
                viewport.Width,
                viewport.Height,
                0, 0, 1);
            
            HalfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);

            //HalfPixel = new Vector2(.5f / (float)Renderer.Camera.Viewport.Width, .5f / (float)Renderer.Camera.Viewport.Height);
            //MatrixTransform = HalfPixelOffset * OrthogonalProjection;

            HalfPixel = Vector2.Zero;
            MatrixTransform = OrthogonalProjection;
        }


        /// <summary>
        /// Helper for drawing a texture into a rendertarget, using
        /// a custom shader to apply postprocessing effects.
        /// </summary>
        public void DrawRenderTargetIntoOther(string tag, Texture2D texture, RenderTarget2D renderTarget, Effect effect)
        {
            vxGraphics.GraphicsDevice.SetRenderTarget(renderTarget);

            DrawFullscreenQuad(tag, texture,
                               renderTarget.Width, renderTarget.Height,
                               effect);
        }


        /// <summary>
        /// Helper for drawing a texture into the current rendertarget,
        /// using a custom shader to apply postprocessing effects.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="effect"></param>
        public void DrawFullscreenQuad(string tag, Texture2D texture, int width, int height, Effect effect)
        {
            vxGraphics.SpriteBatch.Begin(tag, 0, BlendState.Opaque, null, null, null, effect);
            vxGraphics.SpriteBatch.Draw(texture, new Rectangle(0, 0, width, height), Color.White);
            vxGraphics.SpriteBatch.End();
        }

        /// <summary>
        /// Draws With No Effect Used
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void DrawFullscreenQuad(string tag, Texture2D texture, int width, int height)
        {
            vxGraphics.SpriteBatch.Begin(tag);
            vxGraphics.SpriteBatch.Draw(texture, new Rectangle(0, 0, width, height), Color.White);
            vxGraphics.SpriteBatch.End();
        }


        public void SetEffectParameter(string param, float value)
        {
            if (Parameters[param] != null)
                Parameters[param].SetValue(value);
        }

        public void SetEffectParameter(string param, Vector2 value)
        {
            if (Parameters[param] != null)
                Parameters[param].SetValue(value);
        }

        public void SetEffectParameter(string param, Vector3 value)
        {
            if (Parameters[param] != null)
                Parameters[param].SetValue(value);
        }

        public void SetEffectParameter(string param, Vector4 value)
        {
            if (Parameters[param] != null)
                Parameters[param].SetValue(value);
        }

        public void SetEffectParameter(string param, Color value)
        {
            if (Parameters[param] != null)
                Parameters[param].SetValue(value.ToVector3());
        }

        public void SetEffectParameter(string param, Matrix value)
        {
            if (Parameters[param] != null)
                Parameters[param].SetValue(value);
        }

        public void SetEffectParameter(string param, Texture2D value)
        {
            if (Parameters[param] != null)
                Parameters[param].SetValue(value);
        }
    }

}