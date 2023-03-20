using System;
using System.Collections.Generic;
using System.Text;
using VerticesEngine.Graphics;

namespace VerticesEngine.Editor.Entities
{
    /// <summary>
    /// This material is used for editor entities which allows them to update their visual state based on wheth
    /// the mouse is over top or not
    /// </summary>
    public class vxEditorEntityMaterial : vxMaterial
    {
        public vxEditorEntityMaterial() : base(vxInternalAssets.Shaders.EditorControlShader)
        {
            this.IsShadowCaster = false;
            this.IsDefferedRenderingEnabled = false;
        }
    }
}
