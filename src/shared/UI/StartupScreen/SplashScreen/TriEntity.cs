#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using VerticesEngine.Graphics;

#endregion

namespace VerticesEngine.UI.StartupScreen
{
    /// <summary>
    /// The opening Plexus-style Tri Elements
    /// </summary>
    internal class TitleScreenPlexusTri
    {
        Matrix World;

        BasicEffect basicEffect;

        VertexPositionColor[] vertices;

        vxMesh model;

        VertexBuffer vertexBuffer;

        vxTitleScreen titleScreen;

        Vector3 triPosition;

        Color triCol;

        float triAngle = 0.0f;


        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.Screens.Tri"/> class.
        /// </summary>
        /// <param name="titleScreen">Game.</param>
        /// <param name="position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
        public TitleScreenPlexusTri(vxTitleScreen titleScreen, Vector3 position)
        {
            this.titleScreen = titleScreen;
            triPosition = position;
            World = Matrix.CreateTranslation(triPosition);


            basicEffect = new BasicEffect(vxGraphics.GraphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.LightingEnabled = false;
            //basicEffect.EnableDefaultLighting();
            basicEffect.FogEnabled = true;


            List<VertexPositionColor> VPCs = new List<VertexPositionColor>();

            float Size = 1;

            // set the tri colour
            triCol = vxTitleScreen.IsDarkStart ? vxTitleScreen.LightCol : vxTitleScreen.DarkCol;
            basicEffect.DiffuseColor = triCol.ToVector3();

            VPCs.Add(new VertexPositionColor(new Vector3(0, Size, 0), triCol));
            VPCs.Add(new VertexPositionColor(new Vector3(Size / 2, 0, 0), triCol));
            VPCs.Add(new VertexPositionColor(new Vector3(-Size / 2, 0, 0), triCol));
            VPCs.Add(new VertexPositionColor(new Vector3(0, Size, 0), triCol));

            vertices = VPCs.ToArray();

            vertexBuffer = new VertexBuffer(vxGraphics.GraphicsDevice, typeof(VertexPositionColor), VPCs.Count, BufferUsage.None);
            vertexBuffer.SetData<VertexPositionColor>(vertices);

            triAngle += position.Length();

            model = titleScreen.SphereModel;

        }

        public void Update()
        {
            triAngle += 0.005f;

            float factor = (float)Math.Sin(triAngle);

            World =
                Matrix.CreateScale(new Vector3(1 + factor / 4)) *
                Matrix.CreateRotationZ(triAngle) *
                Matrix.CreateRotationY(triAngle) *
                Matrix.CreateTranslation(triPosition);
        }

        public void Draw()
        {
            triPosition += new Vector3(0.001f, 0, 0);

            basicEffect.World = World;
            basicEffect.VertexColorEnabled = true;
            basicEffect.FogEnabled = true;
            basicEffect.View = titleScreen.CameraView;
            basicEffect.Projection = titleScreen.CameraProjection;
            basicEffect.FogColor = vxTitleScreen.IsDarkStart ? vxTitleScreen.DarkCol.ToVector3() : Vector3.One;
            basicEffect.FogStart = vxTitleScreen.FogStart;
            basicEffect.FogEnd = vxTitleScreen.FogEnd;

            // first render the trianle line elements
            vxGraphics.GraphicsDevice.SetVertexBuffer(vertexBuffer);

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                vxGraphics.GraphicsDevice.DrawPrimitives(PrimitiveType.LineStrip, 0, 3);
            }

            // now draw the actual tri vertices as spheres
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            vxGraphics.GraphicsDevice.RasterizerState = rasterizerState;

            foreach (VertexPositionColor vpc in vertices)
            {
                basicEffect.World = Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(vpc.Position) * World;

                basicEffect.AmbientLightColor = Vector3.Zero;
                basicEffect.DiffuseColor = vxTitleScreen.IsDarkStart ? vxTitleScreen.LightCol.ToVector3() : vxTitleScreen.DarkCol.ToVector3();
                basicEffect.EmissiveColor = Vector3.Zero;
                basicEffect.VertexColorEnabled = false;

                foreach (var mesh in model.Meshes)
                {
                    mesh.Draw(basicEffect);
                }
            }
        }
    }
}
