
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using VerticesEngine.Diagnostics;
using VerticesEngine.Editor.Entities;
using VerticesEngine.Graphics;
using VerticesEngine.Util;

namespace VerticesEngine
{
    public class vxWaterEntity : vxEntity3D
    {
        [vxShowInInspector("Water Properties")]
        public Vector3 WaterScale
        {
            get { return _waterScale; }
            set
            {
                _waterScale = value;
                OnScaleCubeMoved(this, null);
            }
        }
        public Vector3 _waterScale = Vector3.One;

        // Scaling Cubes
        vxScaleBar scLeft;
        vxScaleBar scRight;
        vxScaleBar scForward;
        vxScaleBar scBack;



        float ModelScale = 1;

        /// <summary>
        /// Creates a New Instance of a Water Entity
        /// </summary>
        /// <param name="Engine"></param>
        /// <param name="StartPosition"></param>
        /// <param name="WaterScale"></param>
        public vxWaterEntity(vxGameplayScene3D scene) : base(scene, vxInternalAssets.Models.WaterPlane, Vector3.Zero)
        {
            //Render even in debug mode
            RenderEvenInDebug = true;

            // Set the Water Scale
            this._waterScale = new Vector3(100, 5, 100);// WaterScale / (ModelScale * 2);

            // Finally Set up the Physics Bodies
            SetUpPhysics();

            Tag = this;

            //Set up Scale Cubes
            SetupCubes();

        }

        void SetupCubes()
        {
            scLeft = new vxScaleBar(Scene, StartPosition + Vector3.Backward * _waterScale.Z, this);
            scLeft.Moved += OnScaleCubeMoved;
            scRight = new vxScaleBar(Scene, StartPosition - Vector3.Backward * _waterScale.Z, this);
            scRight.Moved += OnScaleCubeMoved;

            scForward = new vxScaleBar(Scene, StartPosition + Vector3.Right * _waterScale.X, this);
            scForward.Moved += OnScaleCubeMoved;
            scBack = new vxScaleBar(Scene, StartPosition - Vector3.Right * _waterScale.X, this);
            scBack.Moved += OnScaleCubeMoved;

            Scene.EditorEntities.Add(scLeft);
            Scene.EditorEntities.Add(scRight);
            Scene.EditorEntities.Add(scForward);
            Scene.EditorEntities.Add(scBack);
        }



        protected internal override void OnAdded()
        {
            base.OnAdded();

            scLeft.Id = this.Id + ".Left";
            scRight.Id = this.Id + ".Right";
            scForward.Id = this.Id + ".Forward";
            scBack.Id = this.Id + ".Back";
        }

        void DestroyCubes()
        {
            scLeft.Dispose();
            scRight.Dispose();
            scForward.Dispose();
            scBack.Dispose();
        }

        /// <summary>
        /// Called whenever the skybox is changed.
        /// </summary>
        public virtual void OnSkyboxChange()
        {
            InitShaders();
        }


        vxWaterMaterial waterMaterial;

        protected override vxMaterial OnMapMaterialToMesh(vxModelMesh mesh)
        {
            if(waterMaterial == null)
            waterMaterial = new vxWaterMaterial();

            return waterMaterial;
        }

        /// <summary>
        /// Sets up the Physics Entities
        /// </summary>
        public virtual void SetUpPhysics()
        {

        }

        protected internal override void OnSandboxStatusChanged(bool IsRunning)
        {
            base.OnSandboxStatusChanged(IsRunning);

            ResetScaleCubes();
        }

        protected override void OnFirstUpdate()
        {
            base.OnFirstUpdate();
            //ResetScaleCubes();
            OnWorldTransformChanged();
        }

        private void OnScaleCubeMoved(object sender, EventArgs e)
        {
            //Set the Main Position as the average of the top scale cubes
            Vector3 NewPosition = new Vector3((scForward.Position.X + scBack.Position.X) / 2, Position.Y, (scLeft.Position.Z + scRight.Position.Z) / 2);

            Position += (NewPosition - Position) * 2 / 3;


            //Set the water scale
            _waterScale = new Vector3(
                (scForward.Position.X - scBack.Position.X) / (2 * ModelScale),
                _waterScale.Y,
                (scLeft.Position.Z - scRight.Position.Z) / (2 * ModelScale));



            //Now reset all items
            ResetScaleCubes();
        }


        Vector3 OutOfSightOffset = Vector3.Zero;
        void ResetScaleCubes()
        {
            if (scLeft.SelectionState != vxSelectionState.Selected)
                scLeft.Position = Position + Vector3.Backward * _waterScale.Z * ModelScale + OutOfSightOffset;
            if (scRight.SelectionState != vxSelectionState.Selected)
                scRight.Position = Position - Vector3.Backward * _waterScale.Z * ModelScale + OutOfSightOffset;

            if (scForward.SelectionState != vxSelectionState.Selected)
                scForward.Position = Position + Vector3.Right * _waterScale.X * ModelScale + OutOfSightOffset;
            if (scBack.SelectionState != vxSelectionState.Selected)
                scBack.Position = Position - Vector3.Right * _waterScale.X * ModelScale + OutOfSightOffset;

            scLeft.ScaleDirection = ScaleDirection.Z;
            scRight.ScaleDirection = ScaleDirection.Z;
            scForward.ScaleDirection = ScaleDirection.X;
            scBack.ScaleDirection = ScaleDirection.X;
                        
            renderScale = new Vector3(_waterScale.X * 2, 1, _waterScale.Z * 2);
        }

        private Vector3 renderScale = Vector3.One;

        protected override void OnWorldTransformChanged()
        {
            base.OnWorldTransformChanged();

            ResetScaleCubes();
        }

        protected internal override void OnWillDraw(vxCamera Camera)
        {
            //WorldTransform = Matrix.CreateScale(renderScale / 250) * Matrix.CreateTranslation(Position);
            Transform.Scale = renderScale / 250;
            waterMaterial.IsDefferedRenderingEnabled = false;
            waterMaterial.IsAuxDepthCalculated = true;
            waterMaterial.MaterialRenderPass = vxRenderPipeline.Passes.OpaquePass;
            waterMaterial.TextureUVOffset += new Vector2(-0.0125f, 0.00751f) * 0.125f;
            waterMaterial.SetEffectParameter("VX_CAMERA_POS", Camera.Position);
            waterMaterial.SetEffectParameter("DepthMap", vxRenderPipeline.Instance.DepthMap);
            waterMaterial.SetEffectParameter("AuxDepthMap", vxRenderPipeline.Instance.AuxDepthMap);
            waterMaterial.SetEffectParameter("VX_ProjectionParams", Camera.Util_VX_ProjectionParams);

            base.OnWillDraw(Camera);
        }



        public override void OnBeforeEntitySerialize()
        {
            base.OnBeforeEntitySerialize();

            UserDefinedData02 = string.Format("{0};{1};{2}", _waterScale.X, _waterScale.Y, _waterScale.Z);
        }

        public override void OnAfterEntityDeserialized()
        {
            try
            {
                if (UserDefinedData02 != null)
                {
                    if (UserDefinedData02.Contains(";"))
                    {
                        string[] vars = UserDefinedData02.Split(';');

                        if (vars.Length > 2)
                            _waterScale = new Vector3(
                                float.Parse(vars[0], System.Globalization.CultureInfo.InvariantCulture), 
                                float.Parse(vars[1], System.Globalization.CultureInfo.InvariantCulture), 
                                float.Parse(vars[2], System.Globalization.CultureInfo.InvariantCulture));

                        ResetScaleCubes();
                    }
                }

                base.OnAfterEntityDeserialized();
            }
            catch(Exception ex)
            {
                vxDebug.Error(new {
                    error= "Water OnAfterEntityDeserialized Error",
                    msg= ex.Message,
                    usrData =UserDefinedData02
                });

                _waterScale = new Vector3(700, 5, 700);
            }
        }

        protected override void OnDisposed()
        {
            DestroyCubes();

            base.OnDisposed();
        }
    }
}
