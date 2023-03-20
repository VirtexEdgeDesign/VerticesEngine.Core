
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using VerticesEngine.Graphics;
using VerticesEngine.Input;

namespace VerticesEngine.Editor.Entities
{
    public enum GizmoAxis
    {
        X,
        Y,
        Z,
        XY,
        ZX,
        YZ,
        None
    }

    enum SelectedGizmoItem
    {
        Axis,
        Rotator,
        Pan,
        None
    }


    /// <summary>
    /// Transformation type.
    /// </summary>
    public enum TransformationType
    {
        Local,
        Global
    }



    /// <summary>
    /// A 3D gimbal entity for use in editing Sandbox Entity Position and Rotation.
    /// </summary>
	public sealed class vxGizmo : vxEditorEntity
    {
        /// <summary>
        /// The instance of the gizmo. This is a per-level singleton that doesn't persist between sandboxes
        /// </summary>
        public static vxGizmo Instance
        {
            get { return _instance; }
        }
        private static vxGizmo _instance;

        float CubeSize = 2.5f;

        public static float ScreenSpaceZoomFactor = 25;

        public override void OnEnabled()
        {
            OnStateChanged(true);
        }

        public override void OnDisabled()
        {
            OnStateChanged(false);
        }

        void OnStateChanged(bool active)
        {
            for (int a = 0; a < axesHandles.Count; a++)
            {
                axesHandles[a].IsEnabled = active;
            }
            for (int p = 0; p < panHandles.Count; p++)
            {
                panHandles[p].IsEnabled = active;
            }
            for (int r = 0; r < rotationHandles.Count; r++)
            {
                rotationHandles[r].IsEnabled = active;
            }
        }

        public bool IsInGrabMode
        {
            get { return translationHotKeyHandler.IsInGrabMode; }
            set { translationHotKeyHandler.IsInGrabMode = value; }
        }


        private List<vxGizmoAxisTranslationEntity> axesHandles = new List<vxGizmoAxisTranslationEntity>();
        private List<vxGizmoAxisRotationEntity> rotationHandles = new List<vxGizmoAxisRotationEntity>();
        private List<vxGizmoPanTranslationEntity> panHandles = new List<vxGizmoPanTranslationEntity>();

        private vxGizmoHotKeyTranslationEntity translationHotKeyHandler;

        Vector3 CursorAverage = Vector3.Zero;

        //public List<vxEntity3D> SelectedItems = new List<vxEntity3D>();

        //Reset
        public int scale = 8;

        /// <summary>
        /// The type of the rotation.
        /// </summary>
        public TransformationType TransformationType
        {
            get { return m_transformationType; }
            set
            {
                m_transformationType = value;

                // it's being set to global mode, then clear the rotation
                if (m_transformationType == TransformationType.Global)
                    Transform.Rotation = Quaternion.Identity;
            }
        }
        private TransformationType m_transformationType = TransformationType.Global;


        protected override vxMesh OnLoadModel() { return vxInternalAssets.Models.UnitBox; }

        /// <summary>
        /// Get's whether the Mouse is currently Hovering under ANY Axis in the Cursor.
        /// </summary>
        public bool IsMouseHovering
        {
            get { return GetIsMouseHovering(); }
        }

        private bool GetIsMouseHovering()
        {
            //Search through each axis too see if either one is Hovered or Selected (i.e. Not Unselected)
            for (int a = 0; a < axesHandles.Count; a++)
            {
                if (axesHandles[a].SelectionState != vxSelectionState.None)
                    return true;
            }

            //If they're all Unselected, then return false
            return false;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.Entities.Util.vxGizmo3D"/> class.
        /// </summary>
        /// <param name="scene">Scene.</param>
        internal vxGizmo(vxGameplayScene3D scene) : base(scene, vxEntityCategory.Entity)
        {
            _instance = this;

            //Always Start The Cursor at the Origin
            RenderScale = (CubeSize);

            AddRotator(new vxGizmoAxisRotationEntity(this, GizmoAxis.X));
            AddRotator(new vxGizmoAxisRotationEntity(this, GizmoAxis.Y));
            AddRotator(new vxGizmoAxisRotationEntity(this, GizmoAxis.Z));

            AddAxis(new vxGizmoAxisTranslationEntity(this, GizmoAxis.X));
            AddAxis(new vxGizmoAxisTranslationEntity(this, GizmoAxis.Y));
            AddAxis(new vxGizmoAxisTranslationEntity(this, GizmoAxis.Z));

            AddPan(new vxGizmoPanTranslationEntity(this, GizmoAxis.YZ));
            AddPan(new vxGizmoPanTranslationEntity(this, GizmoAxis.XY));
            AddPan(new vxGizmoPanTranslationEntity(this, GizmoAxis.ZX));

            translationHotKeyHandler = new vxGizmoHotKeyTranslationEntity(this);

            IsVisible = false;
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();

            foreach (var item in axesHandles)
                item.Dispose();
            axesHandles.Clear();
            foreach (var item in rotationHandles)
                item.Dispose();
            rotationHandles.Clear();
            foreach (var item in panHandles)
                item.Dispose();
            panHandles.Clear();
        }

        void AddAxis(vxGizmoAxisTranslationEntity axis)
        {
            axesHandles.Add(axis);
        }

        void AddRotator(vxGizmoAxisRotationEntity rotator)
        {
            rotationHandles.Add(rotator);
        }

        void AddPan(vxGizmoPanTranslationEntity pan)
        {
            panHandles.Add(pan);
        }



        /// <summary>
        /// This is called every time a new entity is selected. This allows the gimbal to
        /// reset it's delta values.
        /// </summary>
        public void OnNewEntitySelection()
        {
            for (int i = 0; i < Scene.SelectedItems.Count; i++)
            {
                Scene.SelectedItems[i].PreSelectionWorld = Scene.SelectedItems[i].Transform;
            }
        }



        public void Update(Ray MouseRay)
        {
            //Now Base Update to set Highlighting Colours
            base.Update();

            //Always re-set the Selection State as it dependas on the child elements
            SelectionState = vxSelectionState.None;
            for (int a = 0; a < axesHandles.Count; a++)
            {
                if (axesHandles[a].SelectionState == vxSelectionState.Selected)
                {
                    SelectionState = vxSelectionState.Selected;

                }
            }
            for (int p = 0; p < panHandles.Count; p++)
            {
                if (panHandles[p].SelectionState == vxSelectionState.Selected)
                {
                    SelectionState = vxSelectionState.Selected;

                }
            }
            for (int r = 0; r < rotationHandles.Count; r++)
            {
                if (rotationHandles[r].SelectionState == vxSelectionState.Selected)
                {
                    SelectionState = vxSelectionState.Selected;

                }
            }

            if (TransformationType == TransformationType.Global)
            {
                for (int i = 0; i < Scene.SelectedItems.Count; i++)
                {
                    Transform.Position = Scene.SelectedItems[i].Position;
                }
            }
            else if (TransformationType == TransformationType.Local)
            {
                for (int i = 0; i < Scene.SelectedItems.Count; i++)
                {
                    Transform = Scene.SelectedItems[i].Transform;
                }
            }


            // Update Cursor                        
            //**********************************************************

            CursorAverage = Vector3.Zero;
            for (int ind = 0; ind < Scene.SelectedItems.Count; ind++)
            {
                Scene.SelectedItems[ind].SelectionState = vxSelectionState.Selected;
                CursorAverage += Scene.SelectedItems[ind].Transform.Position;
            }

            {
                CursorAverage /= Scene.SelectedItems.Count;
                Transform.Position = CursorAverage;
            }
        }

        protected internal override void PostUpdate()
        {
            base.PostUpdate();
            Update(new Ray());
        }

        protected internal override void OnWillDraw(vxCamera Camera)
        {
            base.OnWillDraw(Camera);
            ScreenSpaceZoomFactor = Math.Abs(Vector3.Subtract(Position, Camera.Position).Length());
        }

        public override void RenderOverlayMesh(vxCamera3D Camera)
        {
            if (Scene.SandboxCurrentState == vxEnumSandboxStatus.EditMode)
            {
                SelectionColour = Color.White;
                //Set the Zoom Factor based off of distance from camera
                ScreenSpaceZoomFactor = Math.Abs(Vector3.Subtract(Position, Camera.Position).Length());

                vxGraphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                vxGraphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                vxGraphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            }
        }
    }
}
