using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VerticesEngine.EnvTerrain;
using VerticesEngine.Graphics;

namespace VerticesEngine.Entities.Terrain
{
    public class vxTerrainRenderer : vxMeshRenderer
    {
        vxTerrainChunk terrain;

        protected override void Initialise()
        {
            base.Initialise();

            terrain = (vxTerrainChunk)Entity;
        }

        protected internal override void OnWillDraw(vxCamera Camera)
        {
            base.OnWillDraw(Camera);
        }

        public override void Draw(vxCamera Camera, string renderpass)
        {
            base.Draw(Camera, renderpass);

            if (terrain.IsInEditMode)
            {
                if (renderpass == vxRenderPipeline.Passes.TransparencyPass)
                    DrawWireFrame(Color.Black * 0.5f);
            }
        }
    }
}
