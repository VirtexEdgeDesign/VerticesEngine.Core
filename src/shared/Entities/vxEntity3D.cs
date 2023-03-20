using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using VerticesEngine.ContentManagement;
using VerticesEngine.Diagnostics;
using VerticesEngine.Graphics;

namespace VerticesEngine
{
    public enum vxEntityCategory
    {
        Axis,
        Rotator,
        Pan,
        Entity,
        Particle
    }


    /// <summary>
    /// Base Entity in the Virtex vxEngine which controls all Rendering and Provides
    /// position and world matrix updates to the other required entities.
    /// </summary>
    public class vxEntity3D : vxEntity
    {

        /// <summary>
        /// The handle identifier for this entity.
        /// </summary>
        [vxShowInInspector(vxInspectorCategory.BasicProperties, "The Handle ID for this Entity.")]
        public int HandleID
        {
            get
            {
                return _handleID;
            }
        }
        int _handleID = -1;

        internal Color IndexEncodedColour
        {
            get { return _indexEncodedColour; }
            set
            {
                _indexEncodedColour = value;
                foreach(var mat in m_meshRenderer.Materials)
                {
                    mat.UtilityEffect.Parameters["IndexEncodedColour"].SetValue(value.ToVector4());
                }
            }
        }
        Color _indexEncodedColour = Color.Black;

        /// <summary>
        /// This promotes another entity to be selected when this one is, 
        /// </summary>
        /// <param name="entity"></param>
        protected void OverrideSelectionWithOtherEntity(vxEntity3D entity)
        {
            this.IndexEncodedColour = entity.IndexEncodedColour;
        }

        /// <summary>
        /// The current scene of the game
        /// </summary>
        public vxGameplayScene3D Scene
        {
            get { return m_scene; }
        }
        private vxGameplayScene3D m_scene;


        public new vxGameplayScene3D CurrentScene
        {
            get { return (vxGameplayScene3D)base.CurrentScene; }
        }

        /// <summary>
        /// Location of Entity in world space.
        /// </summary>
        [vxShowInInspector(vxInspectorCategory.EntityTransformProperties, "The Entities 3D World Space Position")]
        public Vector3 Position
        {
            get
            {
                return Transform.Position;
            }
            set
            {
                //_position = value;
                if (Transform != null && Transform.Position!= value)
                {
                    Transform.Position = value;

                    if (m_hasWorldTransformBeenUpdatedThisFrame == false)
                    {
                        m_hasWorldTransformBeenUpdatedThisFrame = true;
                        OnTransformChanged();
                    }
                }
            }
        }
        // we only want the world transform to be updated once per frame, otherwise it can cause infinite loops
        private bool m_hasWorldTransformBeenUpdatedThisFrame = false;
        //Vector3 _position;



        [vxShowInInspector(vxInspectorCategory.EntityTransformProperties, "The Entities 3D World Space Rotation")]
        public Vector3 Rotation
        {
            get
            { 
                var rot = Transform.Rotation.ToEulerAngles();
                return rot;
            }
            set
            {
                if (Transform != null)
                {
                    Transform.Rotation = Quaternion.CreateFromYawPitchRoll(
                     MathHelper.ToRadians(value.Y),
                     MathHelper.ToRadians(value.Z),
                     MathHelper.ToRadians(value.X));

                    if (m_hasWorldTransformBeenUpdatedThisFrame == false)
                    {
                        m_hasWorldTransformBeenUpdatedThisFrame = true;
                        OnTransformChanged();
                    }
                }
            }
        }

        [vxShowInInspector(vxInspectorCategory.EntityTransformProperties, "The Entities 3D World Space Rotation")]
        public Vector3 Scale
        {
            get
            {
                return Transform.Scale;
            }
            set
            {
                if (Transform != null)
                {
                    Transform.Scale = value;

                    //m_boundingSphere = GetBoundingShape();

                    if (m_hasWorldTransformBeenUpdatedThisFrame == false)
                    {
                        m_hasWorldTransformBeenUpdatedThisFrame = true;
                        OnTransformChanged();
                    }
                }
            }
        }

        private void OnTransformChanged()
        {
            OnWorldTransformChanged();

            // now update all components
            foreach (var component in Components)
                component.OnTransformChanged();
        }
        /// <summary>
        /// Fired when the World Matrix is Updated or changed. Helpful for entities which are dependant on this one.
        /// </summary>
        protected virtual void OnWorldTransformChanged() { }

        /// <summary>
        /// The Start Position of the Entity
        /// </summary>
        public Vector3 StartPosition
        {
            get { return _startPosition;}
           // set { _startPosition = value; }
        }

        private Vector3 _startPosition;

        protected void SetStartPosition(Vector3 startPOs)
        {
            _startPosition = startPOs;
        }
        

        //[vxSerialise]
        [vxShowInInspector(vxInspectorCategory.EntityTransformProperties, "The Entities 3D Scale")]
        public float RenderScale
        {
            get { return m_renderScale; }
            set { 
                m_renderScale = value;

                // Get the Bounding Shadpw used for Camera Frustrum Culling.
                m_boundingSphere = GetBoundingShape();

                // Set the Model Center as the Bounding Shape Center.
                m_modelCenter = m_boundingSphere.Center;

                OnRenderScaleChanged();

                Transform.Scale = new Vector3(m_renderScale);
            }
        }
        private float m_renderScale = 1.0f;

        protected virtual void OnRenderScaleChanged() { }



        /// <summary>
        /// Item Bounding Box
        /// </summary>
        [vxShowInInspector(vxInspectorCategory.ModelProperties, "The Entities Bounding Box")]
        public BoundingBox BoundingBox
        {
            get { return _boundingBox; }
        }
        BoundingBox _boundingBox = new BoundingBox();

        /// <summary>
        /// The Model Center. This is not to be confused with the Model Position. 
        /// </summary>
        [vxShowInInspector(vxInspectorCategory.ModelProperties, "The Entities Geometric Center")]
        public Vector3 ModelCenter
        {
            get { return m_modelCenter; }
            set { m_modelCenter = value; }
        }
        Vector3 m_modelCenter = new Vector3();


        public string UserDefinedData;// = "no-data";
        public string UserDefinedData01;// = "no-data";
        public string UserDefinedData02;// = "no-data";
        public string UserDefinedData03;// = "no-data";
        public string UserDefinedData04;// = "no-data";
        public string UserDefinedData05;// = "no-data";


        public object Tag = "";

        //Load in mesh data and create the collision mesh.
        // public Vector3[] MeshVertices;
        //
        // public int[] MeshIndices;

        public bool CanBePlacedOnSurface = false;

        public vxTransform PreSelectionWorld = vxTransform.Identity;


        #region -- Sandbox Fields and Properties --

        /// <summary>
        /// The type of the sandbox entity.
        /// </summary>
        [vxShowInInspector(vxInspectorCategory.BasicProperties)]
        public vxEntityCategory SandboxEntityType
        {
            get { return m_sandboxEntityType; }
        }
        public readonly vxEntityCategory m_sandboxEntityType;

        /// <summary>
        /// Description of the Entity
        /// </summary>
        public string Description;


        public Color SelectionColour
        {
            set
            {
                _selectionColour = value;

                foreach (var mat in m_meshRenderer.Materials)
                {
                    if(mat.UtilityEffect != null)
                    mat.UtilityEffect.SelectionColour = value;
                }
            }
            get { return _selectionColour; }
        }
        Color _selectionColour;


        public string ItemKey = "<none>";




        /// <summary>
        /// Called when the gimbal translates this entity.
        /// </summary>
        protected internal virtual void OnGimbalTranslate(Vector3 delta) { }


        /// <summary>
        /// Called when the gimbal rotates this entity.
        /// </summary>
        protected internal virtual void OnGimbalRotate(Vector3 axis, float delta) { }

        #endregion

        /// <summary>
        /// The mesh renderer for this entity
        /// </summary>
        public vxMeshRenderer MeshRenderer
        {
            get { return m_meshRenderer; }
        }
        private vxMeshRenderer m_meshRenderer;

        public vxEntity3D() : this(vxEngine.Instance.GetCurrentScene<vxGameplayScene3D>(), Vector3.Zero)
        {

        }

        /// <summary>
        /// Base Entity Object for the Engine.
        /// <para>If a model is not specefied, this entity will call the GetModel() method. Override this method
        /// with the and return the model for this Entity.
        /// Otherwise this Entity is used as an 'Empty' in the Engine.</para>
        /// </summary>
        /// <param name="scene">The current Scene that will own this entity.</param>
        /// <param name="startPosition">The Start Position of the Entity</param>
        public vxEntity3D(vxGameplayScene3D scene, Vector3 startPosition) : this(scene, null, startPosition) { }
        
        
        /// <summary>
        /// Base Entity Object for the Engine.
        /// <para>If a model is not specefied, this entity will call the GetModel() method. Override this method
        /// with the and return the model for this Entity.
        /// Otherwise this Entity is used as an 'Empty' in the Engine.</para>
        /// </summary>
        /// <param name="Scene">The current Scene that will own this entity.</param>
        /// <param name="startPosition">The Start Position of the Entity</param>
        public vxEntity3D(Vector3 startPosition) : this(vxEngine.Instance.GetCurrentScene<vxGameplayScene3D>(), null, startPosition) { }
        
        
        /// <summary>
        /// Base Entity Object for the Engine.
        /// <para>If a model is not specefied, this entity will call the GetModel() method. Override this method
        /// with the and return the model for this Entity.
        /// Otherwise this Entity is used as an 'Empty' in the Engine.</para>
        /// </summary>
        /// <param name="entityMesh">The mesh to use with this entity.</param>
        /// <param name="startPosition">The Start Position of the Entity</param>
        public vxEntity3D(vxMesh entityMesh, Vector3 startPosition) : this(vxEngine.Instance.GetCurrentScene<vxGameplayScene3D>(), entityMesh, startPosition) { }

        /// <summary>
        /// Base Entity Object for the Engine.
        /// </summary>
        /// <param name="scene">The current Scene that will own this entity.</param>
        /// <param name="entityMesh">The Entities Model to be used.</param>
        /// <param name="startPosition">The Start Position of the Entity.</param>
        public vxEntity3D(vxGameplayScene3D scene, vxMesh entityMesh, Vector3 startPosition, vxEntityCategory entity3DType = vxEntityCategory.Entity) :
        base(scene)
        {
            m_scene = scene;

            Transform.Position = startPosition;
            Transform.OnTransformUpdated += OnWorldTransformChanged;
            Transform.OnTransformUpdated += OnTransformChanged;

            m_sandboxEntityType = entity3DType;

            _startPosition = startPosition;

            m_meshRenderer = (vxMeshRenderer)EntityRenderer;

            // If the model parameter is passed, then add it in, if not, then fire the virtual method GetModel
            // to try and see if any inheriting classes have overriden this method.
            if (entityMesh != null)
                Model = entityMesh;
            else
                Model = OnLoadModel();

            MapMaterialsToMesh(Model);

            // Add to the main list
            if (Scene != null)
            {
                Scene.Entities.Add(this);

                // Set Handle
                _handleID = Scene.GetNewEntityHandle();

                // Initialise Shaders
                InitShaders();
            }
        }


        protected override vxEntityRenderer CreateRenderer()
        {
            return AddComponent<vxMeshRenderer>();
        }

        protected void MapMaterialsToMesh(vxMesh model)
        {
            m_meshRenderer.Materials.Clear();

            if(model == null)
            {
                vxDebug.Warn("Model is null for " + this.GetType());
                return;
            }

            foreach (var mesh in model.Meshes)
            {
                var mat = OnMapMaterialToMesh(mesh);

                OnRefreshMaterialTextures(mesh, mat);

                m_meshRenderer.Materials.Add(mat);
            }
        }



        /// <summary>
        /// This refreshes the textures for this material, useful if the mesh has changed
        /// and the materials require new meshes
        /// </summary>
        protected virtual void OnRefreshMaterialTextures(vxModelMesh mesh, vxMaterial material)
        {
            if (material != null)
            {
                material.Texture = mesh.GetTexture(MeshTextureType.Diffuse);
                material.NormalMap = mesh.GetTexture(MeshTextureType.NormalMap);
                material.RMAMap = mesh.GetTexture(MeshTextureType.RMAMap);
                material.DistortionMap = mesh.GetTexture(MeshTextureType.DistortionMap);
                material.EmissionMap = mesh.GetTexture(MeshTextureType.EmissiveMap);
            }
        }


        /// <summary>
        /// This method is fired if a model is not passed through the constructor of if 'null' is passed.
        /// This is handy for specifing different models for different conditions for a certain Entity as well as althoughts the model 
        /// not to be loaded until needed.
        /// </summary>
        /// <returns></returns>
        protected virtual vxMesh OnLoadModel()
        {
            // loop through attributes and load the model for the first sandbox item attribute
            foreach (var attribute in GetType().GetCustomAttributes<vxRegisterAsSandboxEntityAttribute>())
            {
                var _filePath = attribute.AssetPath;
                return vxContentManager.Instance.LoadMesh(_filePath);
            }

            vxConsole.WriteVerboseLine("Missing Model for Entity " + this.GetType());

    
            return vxInternalAssets.Models.UnitSphere;
        }


        /// <summary>
        /// Initialise the Main Shader.
        /// <para>If a different shader is applied to this model, this method should be overridden</para>
        /// </summary>
        protected virtual void InitShaders() { }


        protected override void OnDisposed()
        {
            base.OnDisposed();
            m_scene = null;
            Model = null;
        }


        /// <summary>
        /// This Method Is Called when the item is successfully added into the world.
        /// </summary>
        protected internal virtual void OnAdded() { }


        #region -- Update Code --

        protected override void OnFirstUpdate()
        {
            base.OnFirstUpdate();

            // set the index colour 
            IndexEncodedColour = new Color(
                (int)(_handleID % 255),
                (int)Math.Floor((((float)_handleID) / 255.0f) % (255)),
                (int)Math.Floor((((float)_handleID) / (255.0f * 255.0f)) % (255)));
        }

        /// <summary>
        /// Updates the Entity
        /// </summary>
        protected internal override void Update()
        {
            m_hasWorldTransformBeenUpdatedThisFrame = false;
            base.Update();

            //Set the Selection Colour based off of Selection State
            switch (SelectionState)
            {
                case vxSelectionState.Selected:
                    SelectionColour = Color.DarkOrange;
                    break;
                case vxSelectionState.None:
                    SelectionColour = Color.Black;
                    break;
            }

            // Reset the Bounding Sphere's Center Position
            m_boundingSphere.Center = Vector3.Transform(ModelCenter, Transform.Matrix4x4Transform);
        }


        #endregion


        #region -- Rendering Fields --


        /// <summary>
        /// The vxModel model which holds are graphical, shader and vertices data to be shown.
        /// </summary>
        [vxShowInInspector(vxInspectorCategory.ModelProperties)]
        public vxMesh Model
        {
            get { return m_meshRenderer.Mesh; }
            set
            {
                m_meshRenderer.Mesh = value;
                //_model = value;
                if (m_meshRenderer.Mesh != null)
                {
                    // Recalculate the Bounding Box
                    _boundingBox = m_meshRenderer.Mesh.BoundingBox;

                    // Get the Bounding Shadpw used for Camera Frustrum Culling.
                    m_boundingSphere = GetBoundingShape();

                    // Set the Model Center as the Bounding Shape Center.
                    ModelCenter = m_boundingSphere.Center;

                    // Now Initialise Shaders
                    InitShaders();
                }
            }
        }




        public bool IsMotionBlurEnabled
        {
            get { return _isMotionBlurEnabled; }
            set
            {
                _isMotionBlurEnabled = value;
                OnSetMotionBlurMask();
            }
        }
        bool _isMotionBlurEnabled = true;

        public float MotionBlurFactor
        {
            get { return _motionBlurFactor; }
            set
            {
                _motionBlurFactor = value;
                OnSetMotionBlurMask();
            }
        }
        float _motionBlurFactor = 1;


        void OnSetMotionBlurMask()
        {
            MaskPropertiesColor.G = (byte)(_isMotionBlurEnabled ? 0 : 255 * MotionBlurFactor);

            foreach (var mat in m_meshRenderer.Materials)
            {
                mat.UtilityEffect.IndexEncodedColour = IndexEncodedColour;
                mat.UtilityEffect.Parameters["MaskPropertiesColor"].SetValue(MaskPropertiesColor.ToVector4());
            }
        }

        #endregion

        /// <summary>
        /// This color holds Mask Properties for different Post Processes.
        /// R: TODO
        /// G: Camera Motion Blur Factor (0: Do Motion Blur. 1: None),
        /// B: TODO
        /// </summary>
        protected internal Color MaskPropertiesColor = Color.Black;


        /// <summary>
        /// Renders the overlay mesh.
        /// </summary>
        /// <param name="Camera">Camera.</param>
        public virtual void RenderOverlayMesh(vxCamera3D Camera) { }


        #region -- Utility Methods --


        protected virtual BoundingSphere GetBoundingShape()
        {
            var bs = BoundingSphere.CreateFromBoundingBox(Model.BoundingBox);
            //bs.Radius *= m_renderScale;
            bs.Radius *= Transform.Scale.Length();
            return bs;
        }


        #endregion
    }
}