using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using VerticesEngine;
using VerticesEngine.Graphics;

namespace VerticesEngine.Utilities
{
    /// <summary>
    /// Collection of Statix Methods which provide helpful Gemoetry Functions.
    /// </summary>
    public class vxGeometryHelper
    {
        public static BoundingSphere GetModelBoundingSphere(Model model)
        {
            // Initialize minimum and maximum corners of the bounding box to max and min values
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            // For each mesh of the model
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    // Vertex buffer parameters
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vertexBufferSize = meshPart.NumVertices * vertexStride;

                    // Get vertex data as float
                    float[] vertexData = new float[vertexBufferSize / sizeof(float)];
                    meshPart.VertexBuffer.GetData<float>(vertexData);

                    // Iterate through vertices (possibly) growing bounding box, all calculations are done in world space
                    for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
                    {
                        Vector3 vert = new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]);

                        min = Vector3.Min(min, vert);
                        max = Vector3.Max(max, vert);
                    }
                }
            }

            min = Vector3.Transform(min, Matrix.CreateRotationX(-MathHelper.PiOver2));
            max = Vector3.Transform(max, Matrix.CreateRotationX(-MathHelper.PiOver2));

            // The Center will be the average of the Max and Min
            Vector3 Center = Vector3.Add(max, min) / 2;

            // The Radius will be half the difference between the max and min
            float Raduis = Vector3.Subtract(max, min).Length() / 2;


            // Create and return bounding box
            return new BoundingSphere(Center, Raduis);
        }

        /// <summary>
        /// Gets the model bounding box.
        /// </summary>
        /// <returns>The model bounding box.</returns>
        /// <param name="model">Model.</param>
        public static BoundingBox GetModelBoundingBox(Model model)
        {
            return GetModelBoundingBox(model, Matrix.CreateRotationX(-MathHelper.PiOver2));
        }

        /// <summary>
        /// Updates a Bounding Box based off of Model Data
        /// </summary>
        /// <param name="model"></param>
        /// <param name="worldTransform"></param>
        /// <returns></returns>
        public static BoundingBox GetModelBoundingBox(Model model, Matrix worldTransform)
        {
            // Initialize minimum and maximum corners of the bounding box to max and min values
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            try
            {
                // For each mesh of the model
                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    {
                        // Vertex buffer parameters
                        int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                        int vertexBufferSize = meshPart.NumVertices * vertexStride;

                        // Get vertex data as float
                        float[] vertexData = new float[vertexBufferSize / sizeof(float)];
                        meshPart.VertexBuffer.GetData<float>(vertexData);

                        // Iterate through vertices (possibly) growing bounding box, all calculations are done in world space
                        for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
                        {
                            Vector3 transformedPosition = Vector3.Transform(new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]), worldTransform);

                            min = Vector3.Min(min, transformedPosition);
                            max = Vector3.Max(max, transformedPosition);
                        }
                    }
                }
            }
            catch
            {
                min = Vector3.Zero;
                max = Vector3.One;
            }
            // Create and return bounding box
            return new BoundingBox(min, max);
        }



        /// <summary>
        /// Gets the Primitive Count for a given Model.
        /// </summary>
        /// <param name="model">Model to retrieve the primitive count from.</param>
        /// <returns>The Model Primitive Count.</returns>
        public static int GetModelPrimitiveCount(Model model)
        {
            int primCount = 0;

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    primCount += part.PrimitiveCount;
                }
            }

            return primCount;
        }

        /// <summary>
        /// Rotate a Point by a speciefied Matrix.
        /// </summary>
        /// <returns>The point.</returns>
        /// <param name="InitialMatrix">Initial matrix.</param>
        /// <param name="OffsetFromPosition">Offset from position.</param>
        public static Vector3 RotatePoint(Matrix InitialMatrix, Vector3 OffsetFromPosition)
        {
            Matrix temp_world = Matrix.Identity;

            //Set Initial Offset from Origin
            temp_world *= Matrix.CreateTranslation(OffsetFromPosition);

            //Rotate About the RELATIVE MODEL Origin
            temp_world *= InitialMatrix;

            return temp_world.Translation;
        }

        /// <summary>
        /// Returns a Matrix which is rotated first about the origin, then translated and rotated about another axis.
        /// </summary>
        /// <param name="InitialPosition"></param>
        /// <param name="OffsetFromPosition"></param>
        /// <param name="Rotation_Local"></param>
        /// <param name="Rotation_Global"></param>
        /// <returns></returns>
        public static Matrix GetRotatedMatrix(Vector3 InitialPosition, Vector3 OffsetFromPosition,
            Vector3 Rotation_Local, Vector3 Rotation_Global)
        {
            Matrix temp_world = Matrix.Identity;

            //Rotate About the Absolute Origin
            temp_world *= Matrix.CreateRotationX(Rotation_Local.X);
            temp_world *= Matrix.CreateRotationZ(Rotation_Local.Z);
            temp_world *= Matrix.CreateRotationY(Rotation_Local.Y);

            //Set Initial Offset from Origin
            temp_world *= Matrix.CreateTranslation(OffsetFromPosition);

            //Rotate About the RELATIVE MODEL Origin
            temp_world *= Matrix.CreateRotationX(Rotation_Global.X - MathHelper.PiOver2);
            temp_world *= Matrix.CreateRotationY(Rotation_Global.Y);
            temp_world *= Matrix.CreateRotationZ(Rotation_Global.Z);

            //Translate to final center position
            temp_world *= Matrix.CreateTranslation(InitialPosition);

            return temp_world;
        }

        /// <summary>
        /// Gets the rotated matrix.
        /// </summary>
        /// <returns>The rotated matrix.</returns>
        /// <param name="InitialMatrix">Initial matrix.</param>
        /// <param name="OffsetFromPosition">Offset from position.</param>
        /// <param name="Rotation_Local">Rotation local.</param>
        public static Matrix GetRotatedMatrix(Matrix InitialMatrix, Vector3 OffsetFromPosition,
        Vector3 Rotation_Local)
        {
            Matrix temp_world = Matrix.Identity;

            //Rotate About the Absolute Origin
            temp_world *= Matrix.CreateRotationX(Rotation_Local.X);
            temp_world *= Matrix.CreateRotationZ(Rotation_Local.Z);
            temp_world *= Matrix.CreateRotationY(Rotation_Local.Y);

            //Set Initial Offset from Origin
            temp_world *= Matrix.CreateTranslation(OffsetFromPosition);

            //Rotate About the RELATIVE MODEL Origin
            temp_world *= Matrix.CreateRotationX(-MathHelper.PiOver2);
            temp_world *= InitialMatrix;

            return temp_world;
        }


        /// <summary>
        /// Calculates the cursor ray.
        /// </summary>
        /// <returns>The cursor ray.</returns>
        /// <param name="projectionMatrix">Projection matrix.</param>
        /// <param name="viewMatrix">View matrix.</param>
        public static Ray CalculateCursorRay(Matrix projectionMatrix, Matrix viewMatrix)
        {
            Vector2 Position = new Vector2();
            MouseState mouseState = Mouse.GetState();
            Position.X = Math.Max(0, mouseState.X);
            Position.Y = Math.Max(0, mouseState.Y);

            // create 2 positions in screenspace using the cursor position. 0 is as
            // close as possible to the camera, 1 is as far away as possible.
            Vector3 nearSource = new Vector3(Position, 0f);
            Vector3 farSource = new Vector3(Position, 1f);

            // use Viewport.Unproject to tell what those two screen space positions
            // would be in world space. we'll need the projection matrix and view
            // matrix, which we have saved as member variables. We also need a world
            // matrix, which can just be identity.
            Vector3 nearPoint = vxGraphics.GraphicsDevice.Viewport.Unproject(nearSource,
                projectionMatrix, viewMatrix, Matrix.Identity);

            Vector3 farPoint = vxGraphics.GraphicsDevice.Viewport.Unproject(farSource,
                projectionMatrix, viewMatrix, Matrix.Identity);

            // find the direction vector that goes from the nearPoint to the farPoint
            // and normalize it....
            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            // and then create a new ray using nearPoint as the source.
            return new Ray(nearPoint, direction);
        }

        /// <summary>
        /// Transforms the bounding box.
        /// </summary>
        /// <returns>The bounding box.</returns>
        /// <param name="boundingBox">Bounding box.</param>
        /// <param name="m">M.</param>
        public static BoundingBox TransformBoundingBox(BoundingBox boundingBox, Matrix m)
        {
            var xa = m.Right * boundingBox.Min.X;
            var xb = m.Right * boundingBox.Max.X;

            var ya = m.Up * boundingBox.Min.Y;
            var yb = m.Up * boundingBox.Max.Y;

            var za = m.Backward * boundingBox.Min.Z;
            var zb = m.Backward * boundingBox.Max.Z;

            return new BoundingBox(
                Vector3.Min(xa, xb) + Vector3.Min(ya, yb) + Vector3.Min(za, zb) + m.Translation,
                Vector3.Max(xa, xb) + Vector3.Max(ya, yb) + Vector3.Max(za, zb) + m.Translation
            );
        }

        /// <summary>
        /// Splits a Camera frustum.
        /// </summary>
        /// <returns>The frustum.</returns>
        /// <param name="near">Near.</param>
        /// <param name="far">Far.</param>
        /// <param name="m">M.</param>
        public static IEnumerable<Vector3> splitFrustum(float near, float far, Matrix m)
        {
            var clipCorners = new BoundingBox(new Vector3(-1, -1, near), new Vector3(1, 1, far)).GetCorners();
            return clipCorners.Select(v =>
            {
                var vt = Vector4.Transform(v, m);
                vt /= vt.W;

                return new Vector3(vt.X, vt.Y, vt.Z);
            });
        }

        /// <summary>
        /// Sets up the Split Scheme for Cascade Shadow Mapping.
        /// </summary>
        /// <returns>The split scheme.</returns>
        /// <param name="numSplits">Number splits.</param>
        /// <param name="n">N.</param>
        /// <param name="f">F.</param>
        public static IEnumerable<float> practicalSplitScheme(int numSplits, float n, float f)
        {
            for (int i = 0; i < numSplits; ++i)
            {
                float p = ((float)i) / numSplits;
                float c_log = n * (float)System.Math.Pow(f / n, p);
                float c_lin = n + (f - n) * p;

                yield return 0.5f * (c_log + c_lin) / (numSplits - i);
            }

            yield return f;
        }

        /// <summary>
        /// Determines the shadow minimum max1 d.
        /// </summary>
        /// <returns>The shadow minimum max1 d.</returns>
        /// <param name="values">Values.</param>
        /// <param name="cam">Cam.</param>
        /// <param name="desiredSize">Desired size.</param>
        public static float[] determineShadowMinMax1D(IEnumerable<float> values, float cam, float desiredSize)
        {
            var min = values.Min();
            var max = values.Max();

            if (cam > max)
            {
                return new[] { max - desiredSize, max };
            }
            else if (cam < min)
            {
                return new[] { min, min + desiredSize };
            }
            else
            {
                var currentSize = max - min;
                var l = (cam - min) / currentSize * desiredSize;
                var r = (max - cam) / currentSize * desiredSize;

                return new[]
                    {
                    cam - l,
                    cam + r
                };
            }
        }

        /// <summary>
        /// Fullscreens the quad.
        /// </summary>
        /// <returns>The quad.</returns>
        /// <param name="color">Color.</param>
        /// <param name="depth">Depth.</param>
        public static IEnumerable<VertexPositionColorTexture> fullscreenQuad(Color color, float depth)
        {
            var clipCorners = new[]
            {
            new Vector2(-1, -1),
            new Vector2(-1,  1),
            new Vector2( 1, -1),
            new Vector2( 1,  1)
        };

            return clipCorners.Select(p =>
            {
                var pos = new Vector3(p.X, p.Y, depth);
                var texCoord = new Vector2((p.X + 1) / 2.0f, (-p.Y + 1) / 2.0f);

                return new VertexPositionColorTexture(pos, color, texCoord);
            });
        }

        /// <summary>
        /// Poissons the kernel.
        /// </summary>
        /// <returns>The kernel.</returns>
        public static IEnumerable<Vector2> poissonKernel()
        {
            return new[]
            {
            new Vector2(-0.326212f, -0.405810f),
            new Vector2(-0.840144f, -0.073580f),
            new Vector2(-0.695914f,  0.457137f),
            new Vector2(-0.203345f,  0.620716f),
            new Vector2( 0.962340f, -0.194983f),
            new Vector2( 0.473434f, -0.480026f),
            new Vector2( 0.519456f,  0.767022f),
            new Vector2( 0.185461f, -0.893124f),
            new Vector2( 0.507431f,  0.064425f),
            new Vector2( 0.896420f,  0.412458f),
            new Vector2(-0.321940f, -0.932615f),
            new Vector2(-0.791559f, -0.597710f)
        };
        }


        /// <summary>
        /// Cubes the triangle list.
        /// </summary>
        /// <returns>The triangle list.</returns>
        /// <param name="cubeCorners">Cube corners.</param>
        public static IEnumerable<Vector3> cubeTriangleList(Vector3[] cubeCorners)
        {
            return new[]
            {
            // face 1
            cubeCorners[6], cubeCorners[2], cubeCorners[1],
            cubeCorners[1], cubeCorners[5], cubeCorners[6],
 
            // face 2
            cubeCorners[3], cubeCorners[2], cubeCorners[6],
            cubeCorners[6], cubeCorners[7], cubeCorners[3],

            // face 3
            cubeCorners[0], cubeCorners[3], cubeCorners[7],
            cubeCorners[7], cubeCorners[4], cubeCorners[0],

            // face 4
            cubeCorners[5], cubeCorners[1], cubeCorners[0],
            cubeCorners[0], cubeCorners[4], cubeCorners[5],

            // face 5
            cubeCorners[6], cubeCorners[5], cubeCorners[4],
            cubeCorners[4], cubeCorners[7], cubeCorners[6],

            // face 6
            cubeCorners[0], cubeCorners[1], cubeCorners[2],
            cubeCorners[2], cubeCorners[3], cubeCorners[0],
        };
        }

        /// <summary>
        /// Normalizeds the grid vertices.
        /// </summary>
        /// <returns>The grid vertices.</returns>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public static IEnumerable<Vector4> normalizedGridVertices(int width, int height)
        {
            float border = 0.0251f;

            // figure out shading values
            var lightDirection = Vector3.Normalize(new Vector3(0.2f, 1, 0.25f));
            var faceNormals = new[]
                {
                Vector3.Right,
                Vector3.Backward,
                Vector3.Left,
                Vector3.Forward,
                Vector3.Up
            };

            var colors = faceNormals.Select(n => Vector3.Dot(n, lightDirection) * 0.25f + 0.75f).ToArray();
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    var min = new Vector3((x + border) / width, (y + border) / height, 0.0f);
                    var max = new Vector3((x + 1 - border) / width, (y + 1 - border) / height, 1.0f);

                    var axisAlignedCube = new BoundingBox(min, max);
                    var triangles = cubeTriangleList(axisAlignedCube.GetCorners()).Take(30).ToArray();

                    for (int i = 0; i < triangles.Length; ++i)
                    {
                        var c = colors[i / 6];
                        yield return new Vector4(triangles[i].X, triangles[i].Y, triangles[i].Z, c);
                    }
                }
            }
        }
    }
}