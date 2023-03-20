
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

using VerticesEngine.Diagnostics;
using VerticesEngine.Graphics;
using VerticesEngine.Physics;
using VerticesEngine.Physics.BEPUWrapper;
using VerticesEngine.Util;

namespace VerticesEngine.Editor.Entities
{
    /// <summary>
    /// A bounding trigger volume 
    /// </summary>
    public class vxBoundingVolume : vxEntity3D
    {
        protected Texture VolumeTexture;

        [vxShowInInspector("Properties")]
        public Vector3 VolumeScale
        {
            get { return _volumeScale; }
            set
            {
                _volumeScale = value;
                OnScaleCubeMoved(this, null);
            }
        }
        public Vector3 _volumeScale = Vector3.One;


        // Scaling Cubes
        vxResizingGizmoHandle scLeft;
        vxResizingGizmoHandle scRight;
        vxResizingGizmoHandle scForward;
        vxResizingGizmoHandle scBack;
        vxResizingGizmoHandle scUp;
        vxResizingGizmoHandle scDown;

        private vxBEPUPhysicsBoxCollider collider;

        float ModelScale = 1;

        protected float Transparency = 0.5f;

        protected virtual bool IsPhysicsSupported
        {
            get { return true; }
        }

        public vxBoundingVolume(vxGameplayScene3D scene) : this(scene, vxInternalAssets.Models.UnitBox)
        {

        }

        /// <summary>
        /// Creates a New Instance of a Water Entity
        /// </summary>
        /// <param name="Engine"></param>
        /// <param name="StartPosition"></param>
        /// <param name="WaterScale"></param>
        public vxBoundingVolume(vxGameplayScene3D scene, vxMesh model) : base(scene, model, Vector3.Zero)
        {
            //Render even in debug mode
            RenderEvenInDebug = true;

            // Set the Water Scale
            this._volumeScale = new Vector3(100, 5, 100);// WaterScale / (ModelScale * 2);

            Tag = this;

            //Set up Scale Cubes
            SetupCubes();
            VolumeTexture = vxInternalAssets.Textures.DefaultDiffuse;

            this.IsEntityCullable = false;

            if (IsPhysicsSupported)
            {
                collider = AddComponent<vxBEPUPhysicsBoxCollider>();
                collider.IsTrigger = true;
            }
        }

        void SetupCubes()
        {
            scLeft = new vxScaleCube(Scene, StartPosition + Vector3.Backward * _volumeScale.Z, this);
            scLeft.Moved += OnScaleCubeMoved;
            scRight = new vxScaleCube(Scene, StartPosition - Vector3.Backward * _volumeScale.Z, this);
            scRight.Moved += OnScaleCubeMoved;

            scForward = new vxScaleCube(Scene, StartPosition + Vector3.Right * _volumeScale.X, this);
            scForward.Moved += OnScaleCubeMoved;
            scBack = new vxScaleCube(Scene, StartPosition - Vector3.Right * _volumeScale.X, this);
            scBack.Moved += OnScaleCubeMoved;

            scUp = new vxScaleCube(Scene, StartPosition + Vector3.Up * _volumeScale.Y, this);
            scUp.Moved += OnScaleCubeMoved;
            scDown = new vxScaleCube(Scene, StartPosition - Vector3.Up * _volumeScale.Y, this);
            scDown.Moved += OnScaleCubeMoved;

            Scene.EditorEntities.Add(scLeft);
            Scene.EditorEntities.Add(scRight);
            Scene.EditorEntities.Add(scForward);
            Scene.EditorEntities.Add(scBack);
            Scene.EditorEntities.Add(scUp);
            Scene.EditorEntities.Add(scDown);


        }

        protected internal override void OnAdded()
        {
            base.OnAdded();

            // Setup ID's
            scLeft.Id = this.Id + ".Left";
            scRight.Id = this.Id + ".Right";
            scForward.Id = this.Id + ".Forward";
            scBack.Id = this.Id + ".Back";
            scUp.Id = this.Id + ".Up";
            scDown.Id = this.Id + ".Down";
        }

        void DestroyCubes()
        {
            scLeft.Dispose();
            scRight.Dispose();
            scForward.Dispose();
            scBack.Dispose();
            scUp.Dispose();
            scDown.Dispose();
        }

        /// <summary>
        /// Called whenever the skybox is changed.
        /// </summary>
        public virtual void OnSkyboxChange()
        {
            InitShaders();
        }





        protected override void InitShaders()
        {
            base.InitShaders();

            foreach (vxModelMesh mesh in Model.Meshes)
            {
                //mesh.Material = new vxWaterMaterial();
            }
        }



        protected internal override void OnSandboxStatusChanged(bool IsRunning)
        {
            base.OnSandboxStatusChanged(IsRunning);

            ResetScaleCubes();

            if (IsPhysicsSupported)
            {
                collider.SetSize(_volumeScale * 2);
                collider.IsTrigger = true;
            }
        }

        protected override void OnFirstUpdate()
        {
            base.OnFirstUpdate();

            OnWorldTransformChanged();
        }

        private void OnScaleCubeMoved(object sender, EventArgs e)
        {
            //Set the Main Position as the average of the top scale cubes
            Vector3 NewPosition = new Vector3(
                (scForward.Position.X + scBack.Position.X) / 2,
                (scUp.Position.Y + scDown.Position.Y) / 2,
                (scLeft.Position.Z + scRight.Position.Z) / 2);

            Position += (NewPosition - Position) * 2 / 3;


            //Set the water scale
            _volumeScale = new Vector3(
                (scForward.Position.X - scBack.Position.X) / (2 * ModelScale),
                (scUp.Position.Y - scDown.Position.Y) / (2 * ModelScale),
                (scLeft.Position.Z - scRight.Position.Z) / (2 * ModelScale));



            //Now reset all items
            ResetScaleCubes();
        }

        


        Vector3 OutOfSightOffset = Vector3.Zero;
        void ResetScaleCubes()
        {
            if (scLeft.SelectionState != vxSelectionState.Selected)
                scLeft.Position = Position + Vector3.Backward * _volumeScale.Z * ModelScale + OutOfSightOffset;
            if (scRight.SelectionState != vxSelectionState.Selected)
                scRight.Position = Position - Vector3.Backward * _volumeScale.Z * ModelScale + OutOfSightOffset;

            if (scForward.SelectionState != vxSelectionState.Selected)
                scForward.Position = Position + Vector3.Right * _volumeScale.X * ModelScale + OutOfSightOffset;
            if (scBack.SelectionState != vxSelectionState.Selected)
                scBack.Position = Position - Vector3.Right * _volumeScale.X * ModelScale + OutOfSightOffset;


            if (scUp.SelectionState != vxSelectionState.Selected)
                scUp.Position = Position + Vector3.Up * _volumeScale.Y * ModelScale + OutOfSightOffset;
            if (scDown.SelectionState != vxSelectionState.Selected)
                scDown.Position = Position - Vector3.Up * _volumeScale.Y * ModelScale + OutOfSightOffset;

            scLeft.ScaleDirection = ScaleDirection.Z;
            scRight.ScaleDirection = ScaleDirection.Z;
            scForward.ScaleDirection = ScaleDirection.X;
            scBack.ScaleDirection = ScaleDirection.X;
            scUp.ScaleDirection = ScaleDirection.Y;
            scDown.ScaleDirection = ScaleDirection.Y;

            renderScale = new Vector3(_volumeScale.X * 2, _volumeScale.Y * 2, _volumeScale.Z * 2);
        }

        private Vector3 renderScale = Vector3.One;

        protected override void OnWorldTransformChanged()
        {
            base.OnWorldTransformChanged();

            ResetScaleCubes();
        }



        public float EdgeSize = 0.0125f;


        protected internal override void OnWillDraw(vxCamera Camera)
        {
            //WorldTransform = Matrix.CreateScale(renderScale / 2) * Matrix.CreateTranslation(Position);
            Transform.Scale = renderScale / 2;
            foreach (var mat in MeshRenderer.Materials)
            {
                mat.MaterialRenderPass = vxRenderPipeline.Passes.TransparencyPass;
                mat.SetEffectParameter("AlphaOverride", Transparency);
                mat.SetEffectParameter("EdgeSize", EdgeSize);
                mat.SetEffectParameter("TintColour", SelectionState == vxSelectionState.Selected ? Color.DeepSkyBlue : Color.Red);
                mat.SetEffectParameter("EdgeColour", new Color(1, 0.5f, 0.5f, 1));
            }
            base.OnWillDraw(Camera);
        }


        //public override void Draw(vxCamera Camera, string renderpass)
        //{
        //    if (renderpass == vxRenderPipeline.Passes.TransparencyPass)
        //    {
        //        foreach (vxModelMesh mesh in Model.Meshes)
        //        {
        //            mesh.Material.IsDefferedRenderingEnabled = false;

        //            WorldTransform = Matrix.CreateScale(renderScale / 2) * Matrix.CreateTranslation(Position);

        //            mesh.Material.DiffuseColor = new Color(0f, 1f, 0.5f, 0.5f);
        //            mesh.Material.World = WorldTransform;
        //            mesh.Material.WorldInverseTranspose = Matrix.Transpose(Matrix.Invert(WorldTransform));
        //            mesh.Material.WVP = WorldTransform * Camera.ViewProjection;
        //            mesh.Material.View = Camera.View;
        //            mesh.Material.Projection = Camera.Projection;
        //            //mesh.Material.TextureUVOffset += new Vector2(-0.0125f, 0.00751f) * 0.125f;

        //            //mesh.Material.Shader.Parameters["TintColour"].SetValue(Colo);
        //            //mesh.Material.Shader.Parameters["DepthMap"].SetValue(Camera.Renderer.DepthMap);
        //            //mesh.Material.Shader.Parameters["AuxDepthMap"].SetValue(Camera.Renderer.AuxDepthMap);
        //            //mesh.Material.Shader.Parameters["VX_ProjectionParams"].SetValue(Camera.Util_VX_ProjectionParams);

        //            mesh.Draw();
        //        }
        //    }
        //}

        public override void OnBeforeEntitySerialize()
        {
            base.OnBeforeEntitySerialize();

            UserDefinedData02 = string.Format("{0};{1};{2}", _volumeScale.X, _volumeScale.Y, _volumeScale.Z);
        }

        public override void OnAfterEntityDeserialized()
        {
            if (UserDefinedData02 != null)
                if (UserDefinedData02.Contains(";"))
                {
                    string[] vars = UserDefinedData02.Split(';');

                    if (vars.Length > 2)
                        _volumeScale = new Vector3(
                            float.Parse(vars[0], System.Globalization.CultureInfo.InvariantCulture), 
                            float.Parse(vars[1], System.Globalization.CultureInfo.InvariantCulture), 
                            float.Parse(vars[2], System.Globalization.CultureInfo.InvariantCulture));

                    ResetScaleCubes();
                }
            base.OnAfterEntityDeserialized();
        }

        protected override void OnDisposed()
        {
            DestroyCubes();

            base.OnDisposed();
        }
    }
}
