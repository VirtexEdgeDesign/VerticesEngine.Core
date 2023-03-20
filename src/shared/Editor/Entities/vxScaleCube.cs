
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
    public class vxScaleCube : vxResizingGizmoHandle
    {
        public vxScaleCube(vxGameplayScene3D scene, Vector3 StartPosition, vxEntity3D Parent) : base(scene, StartPosition, Parent)
        {

        }

        protected override vxMesh OnLoadModel()
        {
            return vxInternalAssets.Models.UnitBox;
        }

        protected override Matrix GetTransform()
        {
            return Matrix.CreateScale(ScreenSpaceZoomFactor / 100 * Vector3.One) * rotMatrix * Matrix.CreateTranslation(Position);
        }
    }
}