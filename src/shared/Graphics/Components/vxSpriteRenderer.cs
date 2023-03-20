using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using VerticesEngine;

namespace VerticesEngine.Graphics
{
    public class vxSpriteRenderer : vxEntityRenderer
    {
        public class Passes
        {
            public const string PreDraw = "PreDraw";
            public const string MainDraw = "MainDraw";
            public const string PostDraw = "PostDraw";
        }

        public Texture2D SpriteSheet;

        public Rectangle SpriteSheetSource = new Rectangle(); 

        /// <summary>
        /// Called once when created
        /// </summary>
        protected override void Initialise()
        {
            base.Initialise();
        }

        /// <summary>
        /// Called each frame 
        /// </summary>
        protected internal override void Update()
        {
            base.Update();
        }


        /// <summary>
        /// When the component or owning entity is disposed
        /// </summary>
        protected override void OnDisposed()
        {
            base.OnDisposed();
        }

        protected vxEntity2D PairedEntity
        {
            get { return (vxEntity2D)Entity; }
        }


        /// <summary>
        /// When should this entity be drawn in the grand scheme of things!
        /// </summary>
        public string MainRenderPass = Passes.MainDraw;


        public override void Draw(vxCamera Camera, string renderpass)
        {
            // Smooth out the Alpha Value
            PairedEntity.Alpha = vxMathHelper.Smooth(PairedEntity.Alpha, PairedEntity.Alpha_Req, PairedEntity.AlphaChnageSteps);

            if (PairedEntity.UsesSpriteSheet)
            {
                // Now draw the main Sprite
                vxGraphics.SpriteBatch.Draw(PairedEntity.MainSpriteSheet,
                                        PairedEntity.Position,
                                        PairedEntity.SpriteSourceRectangle,
                                        PairedEntity.DisplayColor * PairedEntity.Alpha,
                                        PairedEntity.Rotation,
                                        PairedEntity.Origin,
                                        PairedEntity.Scale,
                                        PairedEntity.SpriteEffect,
                                        PairedEntity.LayerDepth);
            }
            else
            {

                // Draw the texture
                if (PairedEntity.Texture != null && PairedEntity.PhysicCollider != null)
                    vxGraphics.SpriteBatch.Draw(PairedEntity.Texture,
                        PairedEntity.Position,
                        null,
                        PairedEntity.DisplayColor * PairedEntity.Alpha,
                        PairedEntity.Rotation,
                        PairedEntity.Origin,
                        PairedEntity.Scale,
                        PairedEntity.SpriteEffect,
                        PairedEntity.LayerDepth);
            }
        }
    }
}
