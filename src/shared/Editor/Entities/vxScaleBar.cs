
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using VerticesEngine.Graphics;
using VerticesEngine;

namespace VerticesEngine.Editor.Entities
{
    /// <summary>
    /// Scale bar which is used for water scaling
    /// </summary>
    public class vxScaleBar : vxResizingGizmoHandle
    {
        public vxScaleBar(vxGameplayScene3D scene, Vector3 StartPosition, vxEntity3D Parent) : base(scene, StartPosition, Parent)
        {

        }

        protected override vxMesh OnLoadModel()
        {
            return vxInternalAssets.Models.UnitCylinder;
        }
    }
}