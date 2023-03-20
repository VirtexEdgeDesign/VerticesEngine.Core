using Microsoft.Xna.Framework;
using VerticesEngine.Graphics;
using VerticesEngine.Input;
using VerticesEngine.Util;

namespace VerticesEngine.Editor.Entities
{
    /// <summary>
    /// Editor Entity
    /// </summary>
    public class vxEditorEntity : vxEntity3D
    {
        protected vxEditorEntityMaterial EditorEntityMaterial = new vxEditorEntityMaterial();

        /// <summary>
        /// The Colour used for certain shaders (i.e. Highliting, and Plain Color)
        /// </summary>
        protected Color PlainColor = Color.White;

        public Color HoverColour = Color.Yellow;

        public Color SelectedColour = Color.DeepSkyBlue;

        public vxEditorEntity(vxGameplayScene3D scene, vxEntityCategory category)
            : base(scene, null, Vector3.Zero, category)
        {
            RemoveSandboxOption(SandboxOptions.Save);
            RemoveSandboxOption(SandboxOptions.Delete);
            RemoveSandboxOption(SandboxOptions.Export);

            //Remove from the main list so that it can be drawn over the entire scene
            //Scene.Entities.Remove(this);
            Scene.EditorEntities.Add(this);
            this.EntityRenderer.IsRenderedForUtilCamera = false;


            EditorEntityMaterial.IsShadowCaster = false;
            EditorEntityMaterial.IsDefferedRenderingEnabled = false;
        }

        protected override vxMaterial OnMapMaterialToMesh(vxModelMesh mesh)
        {
            return EditorEntityMaterial;
        }

        protected internal override void Update()
        {
            base.Update();


            // If it's released, then apply the Command 
            if (vxInput.IsNewMouseButtonRelease(MouseButtons.LeftButton))
            {
                // Finally Deselect this
                SelectionState = vxSelectionState.None;
            }
        }


        protected override void OnDisposed()
        {
            Scene.EditorEntities.Remove(this);
            base.OnDisposed();

            EditorEntityMaterial.Dispose();
            EditorEntityMaterial = null;
        }



        protected internal override void OnWillDraw(vxCamera Camera)
        {
            if (IsDisposed)
                return;

            base.OnWillDraw(Camera);
            EditorEntityMaterial.World = Transform.Matrix4x4Transform;
            EditorEntityMaterial.WVP = Transform.RenderPassData.WVP;
            EditorEntityMaterial.View = Camera.View;
            EditorEntityMaterial.Projection = Camera.Projection;
            EditorEntityMaterial.SetEffectParameter("SelectedColour", Color.DarkOrange);
            EditorEntityMaterial.SetEffectParameter("HoverColour", Color.Yellow);
            EditorEntityMaterial.SetEffectParameter("EntityIndexedColour", this.IndexEncodedColour);
            EditorEntityMaterial.SetEffectParameter("_handleID", this.HandleID);

            EditorEntityMaterial.SetEffectParameter("IndexColourTexture", vxRenderPipeline.Instance.EncodedIndexResult);

            var mouseCoor = new Vector2(vxInput.Cursor.X / vxScreen.Width, vxInput.Cursor.Y / vxScreen.Height);
            EditorEntityMaterial.SetEffectParameter("mouseCoords", mouseCoor);

            EditorEntityMaterial.SetEffectParameter("_isSelected", (SelectionState == vxSelectionState.Selected) ? 1.0f : 0.0f);
            EditorEntityMaterial.SetEffectParameter("_isMouseUp", (vxInput.IsMouseButtonPressed(MouseButtons.LeftButton) == false) ? 1.0f : 0.0f);
        }

        public override void RenderOverlayMesh(vxCamera3D Camera)
        {           
            
            if (Scene.SandboxCurrentState == vxEnumSandboxStatus.EditMode && Model != null)
            {
                for (int m = 0; m < Model.Meshes.Count; m++)
                {
                    EditorEntityMaterial.View = Camera.View;
                    EditorEntityMaterial.Projection = Camera.Projection;
                    EditorEntityMaterial.SetEffectParameter("SelectedColour", SelectedColour);
                    EditorEntityMaterial.SetEffectParameter("HoverColour", HoverColour);
                    EditorEntityMaterial.SetEffectParameter("EntityIndexedColour", this.IndexEncodedColour);
                    EditorEntityMaterial.SetEffectParameter("_handleID", this.HandleID);

                    EditorEntityMaterial.SetEffectParameter("IndexColourTexture", vxRenderPipeline.Instance.EncodedIndexResult);

                    var mouseCoor = new Vector2(vxInput.Cursor.X / vxScreen.Width, vxInput.Cursor.Y / vxScreen.Height);
                    EditorEntityMaterial.SetEffectParameter("mouseCoords", mouseCoor);
                    
                    EditorEntityMaterial.SetEffectParameter("_isSelected", (SelectionState == vxSelectionState.Selected) ? 1.0f : 0.0f);
                    EditorEntityMaterial.SetEffectParameter("_isMouseUp", (vxInput.IsMouseButtonPressed(MouseButtons.LeftButton) == false) ? 1.0f : 0.0f);
                    
                    Model.Meshes[m].Draw(EditorEntityMaterial);
                }
            }
        }
    }
}
