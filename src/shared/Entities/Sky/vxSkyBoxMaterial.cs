using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.Graphics
{
    public class vxSkyBoxMaterial : vxMaterial
    {
        public vxSkyBoxMaterial() : base (vxInternalAssets.Shaders.SkyboxShader)
        {
            IsShadowCaster = false;
            IsDefferedRenderingEnabled = false;
        }
    }
}
