using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using VerticesEngine.Entities;
using VerticesEngine.Graphics;
using VerticesEngine.UI.Controls;

namespace VerticesEngine
{
    /// <summary>
    /// Base Camera Entity for rendering scenes and views
    /// </summary>
    public class vxCamera : vxEntity
    {
        /// <summary>
        /// What is the back colour for this camera
        /// </summary>
        public Color BackBufferColour = Color.Black;


        /// <summary>
        /// The type of the camera.
        /// </summary>
        public vxCameraType CameraType
        {
            get { return _cameraType; }
            set
            {
                _cameraType = value;
                OnCameraTypeChanged();
            }
        }

        private vxCameraType _cameraType = vxCameraType.Freeroam;

        /// <summary>
        /// Called when the Camera Type is Changed
        /// </summary>
        protected virtual void OnCameraTypeChanged()
        {
        }


        public vxCameraProjectionType DefaultProjectionType = vxCameraProjectionType.Perspective;

        public vxCameraProjectionType EditorProjectionType = vxCameraProjectionType.Perspective;


        /// <summary>
        /// Gets or sets the type of the projection.
        /// </summary>
        /// <value>The type of the projection.</value>
        public vxCameraProjectionType ProjectionType
        {
            get { return _projectionType; }
            set
            {
                _projectionType = value;
                CalculateProjectionMatrix();

                EditorProjectionType = value;

                if (vxSkyBox.Instance != null)
                {
                    vxSkyBox.Instance.IsEnabled = _projectionType == vxCameraProjectionType.Perspective;
                }

                if (_projectionType == vxCameraProjectionType.Orthographic)
                    CameraType = vxCameraType.Orbit;
            }
        }

        vxCameraProjectionType _projectionType = vxCameraProjectionType.Perspective;

        /// <summary>
        /// The viewport for this Camera.
        /// </summary>
        public Viewport Viewport
        {
            get { return _viewport; }
            set
            {
                _viewport = value;
                AspectRatio = _viewport.AspectRatio;
                OnGraphicsRefresh();
            }
        }

        Viewport _viewport;


        const int MAX_DRAWLIST_SIZE = 4096;

        /// <summary>
        /// The index list of items to draw from the culling list
        /// </summary>
        internal int[] drawList = new int[MAX_DRAWLIST_SIZE];

        internal int totalItemsToDraw = 0;

        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        /// <value>The view.</value>
        public Matrix View
        {
            get { return _viewMatrix; }
            set { _viewMatrix = value; }
        }

        private Matrix _viewMatrix;


        /// <summary>
        /// Gets or sets the projection.
        /// </summary>
        /// <value>The projection.</value>
        public Matrix Projection
        {
            get { return _projectionMatrix; }
            set { _projectionMatrix = value; }
        }

        private Matrix _projectionMatrix;

        /// <summary>
        /// Gets the view projection.
        /// </summary>
        /// <value>The view projection.</value>
        public Matrix ViewProjection
        {
            get { return _viewProjection; }
        }

        private Matrix _viewProjection;


        public Matrix InverseView;

        public Matrix InverseProjection;

        /// <summary>
        /// Gets the invert view projection matrix.
        /// </summary>
        /// <value>The invert view projection.</value>
        public Matrix InverseViewProjection
        {
            get { return _invertViewProjection; }
        }

        private Matrix _invertViewProjection = Matrix.Identity;


        /// <summary>
        /// Gets the previous view projection matrix for use in Temporal Effects 
        /// such as Camera Motion Blur.
        /// </summary>
        /// <value>The previous view projection.</value>
        public Matrix PreviousViewProjection
        {
            get { return _previousviewProjection; }
        }

        private Matrix _previousviewProjection = Matrix.Identity;


        public BoundingFrustum BoundingFrustum
        {
            get { return _boundingFrustum; }
        }

        private BoundingFrustum _boundingFrustum;


        /// <summary>
        /// Gets or sets the field of view of the camera in radians
        /// </summary>
        /// <value>The field of view.</value>
        public float FieldOfView
        {
            get { return _fieldOfView; }
            set
            {
                _fieldOfView = value;
                CalculateProjectionMatrix();
            }
        }

        protected internal float _fieldOfView;

        [vxGraphicalSettings("Camera.FieldOfView")]
        public static float DefaultFieldOfView
        {
            get { return _defaultFieldOfView; }
            set { _defaultFieldOfView = value; }
        }

        static float _defaultFieldOfView = 60;


        /// <summary>
        /// Gets or sets the aspect ratio.
        /// </summary>
        /// <value>The aspect ratio.</value>
        public float AspectRatio
        {
            get { return _aspectRatio; }
            set
            {
                _aspectRatio = value;
                CalculateProjectionMatrix();
            }
        }

        protected internal float _aspectRatio;


        /// <summary>
        /// Gets or sets the near plane.
        /// </summary>
        /// <value>The near plane.</value>
        public float NearPlane
        {
            get { return _nearPlane; }
            set
            {
                _nearPlane = value;
                CalculateProjectionMatrix();
            }
        }

        protected internal float _nearPlane;


        /// <summary>
        /// Gets or sets the far plane.
        /// </summary>
        /// <value>The far plane.</value>
        public float FarPlane
        {
            get { return _farPlane; }
            set
            {
                _farPlane = value;
                CalculateProjectionMatrix();
            }
        }

        protected internal float _farPlane;

        public Vector4 Util_VX_ProjectionParams
        {
            get { return new Vector4(1, _nearPlane, _farPlane, 1 / _farPlane); }
        }

        public float Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = value;
                _zoom = MathHelper.Clamp(_zoom, MinZoom, MaxZoom);
                //vxConsole.WriteLine(_zoom);
            }
        }

        protected internal float _zoom = -15;

        public float MinZoom = 0.02f;
        public float MaxZoom = 80000f;

        /// <summary>
        /// The orbit target of the Camera in Orbit mode.
        /// </summary>
        public Vector3 OrbitTarget = Vector3.Zero;

        /// <summary>
        /// Gets or sets the Requested orbit zoom factor.
        /// </summary>
        /// <value>The orbit zoom.</value>
        public float OrbitZoom
        {
            get { return _reqZoom; }
            set { _reqZoom = value; }
        }

        float _reqZoom = -15;


        /// <summary>
        /// Gets or sets the requested yaw rotation of the camera.
        /// </summary>
        public float ReqYaw
        {
            get { return _reqYaw; }
            set { _reqYaw = MathHelper.WrapAngle(value); }
        }

        private float _reqYaw;

        /// <summary>
        /// Gets or sets the requested pitch rotation of the camera.
        /// </summary>
        public float ReqPitch
        {
            get { return _reqPitch; }
            set
            {
                _reqPitch = value;
                if (_reqPitch > MathHelper.PiOver2 * .99f)
                    _reqPitch = MathHelper.PiOver2 * .99f;
                else if (_reqPitch < -MathHelper.PiOver2 * .99f)
                    _reqPitch = -MathHelper.PiOver2 * .99f;
            }
        }

        private float _reqPitch;


        /// <summary>
        /// Gets the world transformation of the camera.
        /// </summary>
        public Matrix WorldMatrix
        {
            get { return _worldMatrix; }
            set { _worldMatrix = value; }
        }

        private Matrix _worldMatrix;

        /// <summary>
        /// Position of camera in world space.
        /// </summary>
        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        private Vector3 _position;

        /// <summary>
        /// Final scene for this camera
        /// </summary>
        public RenderTarget2D FinalScene
        {
            get { return _finalScene; }
        }

        private RenderTarget2D _finalScene;


        public vxEnumSceneDebugMode SceneDebugDisplayMode
        {
            get { return _debugRenderPass.SceneDebugDisplayMode; }
            set { _debugRenderPass.SceneDebugDisplayMode = value; }
        }

        vxDebugRenderPass _debugRenderPass;

        /// <summary>
        /// Whether or not the Camera should or can take input currently.
        /// </summary>
        public bool CanTakeInput = true;


        private List<vxCanvas> m_uiCanvasCollection = new List<vxCanvas>();

        public RenderMeshEntry[] renderList = new RenderMeshEntry[MAX_DRAWLIST_SIZE];

        public RenderMeshEntry[] selectedRenderList = new RenderMeshEntry[MAX_DRAWLIST_SIZE];

        /// <summary>
        /// An entry for the render mesh queue. This holds a reference to a <see cref="vxModelMesh"/>, a <see cref="vxMaterial"/> and any other
        /// render data that's required. 
        /// </summary>
        public class RenderMeshEntry : System.IDisposable
        {
            public vxModelMesh mesh;
            public vxMaterial material;
            public RenderPassTransformData renderPassData;

            public void Dispose()
            {
                mesh = null;
                material = null;
                renderPassData = null;
            }
        }


        public bool IsRenderListEnabled = true;

        // the opaque render list allows for the 75% of the full MAX_DRAWLIST_SIZE
        private RenderMeshEntry[] tempRenderListForOpaques = new RenderMeshEntry[MAX_DRAWLIST_SIZE * 3 / 4];
        private RenderMeshEntry[] tempRenderListForTransparent = new RenderMeshEntry[MAX_DRAWLIST_SIZE * 1 / 4];

        public int opaqueCount = 0;
        public int transparentCount = 0;
        public int selectedCount = 0;

        public vxCamera(vxGameplaySceneBase sceneBase) : base(sceneBase)
        {
            _viewport = vxGraphics.GraphicsDevice.Viewport;

            _boundingFrustum = new BoundingFrustum(Matrix.Identity);

            // initialise the renderer
            vxRenderSettings.Instance.Init(this);

            _debugRenderPass = vxRenderPipeline.Instance.GetRenderingPass<vxDebugRenderPass>();

            // now reset the graphics
            OnGraphicsRefresh();


            if (IsRenderListEnabled)
            {
                for (int r = 0; r < renderList.Length; r++)
                {
                    renderList[r] = new RenderMeshEntry();

                    selectedRenderList[r] = new RenderMeshEntry();

                }
                for (int r = 0; r < tempRenderListForOpaques.Length; r++)
                {
                    tempRenderListForOpaques[r] = new RenderMeshEntry();
                }
                for (int r = 0; r < tempRenderListForTransparent.Length; r++)
                {
                    tempRenderListForTransparent[r] = new RenderMeshEntry();
                }
            }
        }

        protected internal override void Update()
        {
            base.Update();

            // Get the Previous ViewProjection matrix
            _previousviewProjection = _viewProjection; // View * Projection;

            foreach (var canvas in m_uiCanvasCollection)
                canvas.Update();

            _debugRenderPass?.Update();
        }

        public virtual void ResetCamera()
        {
        }

        /// <summary>
        /// This is the normalised bounds of where this camera is rendered too.
        /// </summary>
        public Rectangle NormalisedBounds = new Rectangle(0, 0, 1, 1);

        public override void OnGraphicsRefresh()
        {
            if (cameraRefresh > 0)
                return;

            base.OnGraphicsRefresh();

            vxRenderPipeline.Instance.OnGraphicsRefresh();

            vxConsole.WriteLine("Renderer.OnGraphicsRefresh() " + cameraRefresh++);
        }

        int cameraRefresh = 0;


        public void AddUICanvas(vxCanvas canvas)
        {
            canvas.Init(this);
            m_uiCanvasCollection.Add(canvas);
        }


        protected internal void CalculateProjectionMatrix()
        {
            if (ProjectionType == vxCameraProjectionType.Perspective)
                _projectionMatrix =
                    Matrix.CreatePerspectiveFieldOfView(_fieldOfView, _aspectRatio, _nearPlane, _farPlane);
            else
                _projectionMatrix = Matrix.CreateOrthographic(_viewport.Width * _zoom / 10000,
                    _viewport.Height * _zoom / 10000, -_farPlane, _farPlane);
        }

        protected virtual bool IsUtilCamera
        {
            get { return false; }
        }


        
        /// <summary>
        /// Performs Frustum Culling on the current list of items
        /// </summary>
        void PreCull()
        {
            if (IsRenderListEnabled)
            {
                opaqueCount = 0;
                transparentCount = 0;
                totalItemsToDraw = 0;

                selectedCount = 0;

                // loop through each Mesh Renderer
                var renderers = vxEngine.Instance.CurrentScene.MeshRenderers;
                for (int r = 0; r < renderers.Count; r++)
                {
                    var renderer = renderers[r];
                    renderer.IsRenderedThisFrame = false;
                    if (renderer.IsEnabled && renderer.Mesh != null && renderer.IsDisposed == false)
                    {
                        if (this.IsUtilCamera && renderer.IsRenderedForUtilCamera == false)
                            continue;

                        if (renderer.IsCullable && BoundingFrustum.Intersects(renderer.BoundingShape) ||
                            renderer.IsCullable == false)
                        {
                            // tell the entity it's being drawn
                            renderer.OnWillDraw(this);
                            renderer.IsRenderedThisFrame = true;

                            // loop through each mesh and sort out if it's opaque or transparent
                            for (int mi = 0; mi < renderer.Mesh.Meshes.Count; mi++)
                            {
                                var meshMaterial = renderer.GetMaterial(mi);
                                if (meshMaterial.MaterialRenderPass == vxRenderPipeline.Passes.TransparencyPass)
                                {
                                    var transparentMesh = tempRenderListForTransparent[transparentCount];
                                    transparentMesh.material = meshMaterial;
                                    transparentMesh.mesh = renderer.Mesh.Meshes[mi];
                                    transparentMesh.renderPassData= renderer.RenderPassData;
                                    tempRenderListForTransparent[transparentCount] = transparentMesh;
                                    transparentCount++;
                                }
                                else
                                {
                                    var opauqeMesh = tempRenderListForOpaques[opaqueCount];
                                    opauqeMesh.material = meshMaterial;
                                    opauqeMesh.mesh = renderer.Mesh.Meshes[mi];
                                    opauqeMesh.renderPassData= renderer.RenderPassData;

                                    tempRenderListForOpaques[opaqueCount] = opauqeMesh;
                                    opaqueCount++;
                                }

                                if(renderer.IsSelected)
                                {
                                    var selectedMesh = selectedRenderList[opaqueCount];
                                    selectedMesh.material = meshMaterial;
                                    selectedMesh.mesh = renderer.Mesh.Meshes[mi];
                                    selectedMesh.renderPassData = renderer.RenderPassData;
                                    selectedRenderList[selectedCount] = selectedMesh;
                                    selectedCount++;
                                }
                            }
                        }
                    }
                }

                
                // now that we have the two temp buffers full, now we'll combine them into the main buffer
                totalItemsToDraw = 0;

                for (int o = 0; o < opaqueCount; o++)
                {
                    renderList[totalItemsToDraw++] = tempRenderListForOpaques[o];
                }

                for (int t = 0; t < transparentCount; t++)
                {
                    renderList[totalItemsToDraw++] = tempRenderListForTransparent[t];
                }
                //vxConsole.WriteToScreen("totalItemsToDraw:" + totalItemsToDraw, Color.LimeGreen);
                //vxConsole.WriteToScreen("opaqueCount:" + opaqueCount, Color.Blue);
                //vxConsole.WriteToScreen("transparentCount:" + transparentCount, Color.Magenta);
            }
            else
            {
                totalItemsToDraw = 0;

                // loop through each Mesh Renderer
                var renderers = vxEngine.Instance.CurrentScene.MeshRenderers;
                for (int r = 0; r < renderers.Count; r++)
                {
                    var renderer = renderers[r];
                    renderer.IsRenderedThisFrame = false;
                    if (renderer.IsEnabled && renderer.Mesh != null && renderer.IsDisposed == false)
                    {
                        if (this.IsUtilCamera && renderer.IsRenderedForUtilCamera == false)
                            continue;

                        if (renderer.IsCullable && BoundingFrustum.Intersects(renderer.BoundingShape) ||
                            renderer.IsCullable == false)
                        {
                            // tell the entity it's being drawn
                            renderer.OnWillDraw(this);
                            renderer.IsRenderedThisFrame = true;

                            // add it's index to the draw list
                            drawList[totalItemsToDraw] = r;
                            totalItemsToDraw++;
                        }
                    }
                }
            }

            foreach (var poolKeyPair in vxEngine.Instance.CurrentScene.ParticleSystem.ParticlePools)
            {
                foreach (var prtcl in poolKeyPair.Value.Pool)
                {
                    if (prtcl.IsAlive)
                    {
                        ((vxEntity)prtcl).EntityRenderer.OnWillDraw(this);
                    }
                }
            }
        }

        void ReCalculateAllMatrices()
        {
            // calculate ViewProjection
            _viewProjection = _viewMatrix * _projectionMatrix;

            // calculate inverse projection mat
            _invertViewProjection = Matrix.Invert(_viewProjection);

            InverseView = Matrix.Invert(_viewMatrix);

            InverseProjection = Matrix.Invert(_projectionMatrix);

            _boundingFrustum.Matrix = _viewProjection;
        }


        public void Render()
        {
            // set this camera as the active camera
            vxGraphics.GraphicsDevice.Viewport = Viewport;

            // first recalculate all matrices
            ReCalculateAllMatrices();

            // now perform the culling step
            PreCull();

            // render this camera using the current render pipeline
            vxRenderPipeline.Instance.RenderScene(this);

            vxGraphics.SpriteBatch.Begin();
            foreach (var canvas in m_uiCanvasCollection)
                canvas.Draw();
            vxGraphics.SpriteBatch.End();

            _finalScene = vxRenderPipeline.Instance.Finalise();
        }


        protected override void OnDisposed()
        {
            base.OnDisposed();

            //vxRenderPipeline.Instance.Dispose();

            drawList = null;
            
            for (int r = 0; r < renderList.Length; r++)
            {
                renderList[r].Dispose();
                renderList[r] = null;
            }
            for (int r = 0; r < tempRenderListForOpaques.Length; r++)
            {
                tempRenderListForOpaques[r].Dispose();
                tempRenderListForOpaques[r] = null;
            }
            for (int r = 0; r < tempRenderListForTransparent.Length; r++)
            {
                tempRenderListForTransparent[r].Dispose();
                tempRenderListForTransparent[r] = null;
            }

            renderList = null;
            tempRenderListForOpaques = null;
            tempRenderListForTransparent = null;

            m_uiCanvasCollection.Clear();
        }


        public new T CastAs<T>() where T : vxCamera
        {
            return (T)this;
        }

        public Vector3 ProjectToScreenSpace(Vector3 WorldPosition)
        {
            return _viewport.Project(WorldPosition, _projectionMatrix, _viewMatrix, Matrix.Identity);
        }
    }
}