using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using VerticesEngine.ContentManagement;

namespace VerticesEngine.Graphics
{
    public static partial class vxMeshHelper
    {
        /// <summary>
        /// Get's the Bounding Box for a Model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="worldTransform"></param>
        /// <returns></returns>
        public static BoundingBox GetModelBoundingBox(vxMesh model, Matrix worldTransform)
        {
            // Initialize minimum and maximum corners of the bounding box to max and min values
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            // For each mesh of the model
            foreach (vxModelMesh mesh in model.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    if (meshPart.VertexBuffer != null && meshPart.VertexBuffer.BufferUsage == BufferUsage.None)
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
            }

            // Create and return bounding box
            return new BoundingBox(min, max);
        }


        /// <summary>
        /// Get Vertices and Indies from a vxMesh
        /// </summary>
        /// <param name="model"></param>
        /// <param name="vertices"></param>
        /// <param name="indices"></param>
        public static void GetVerticesAndIndicesFromModel(vxMesh model, out Vector3[] vertices, out int[] indices)
        {
            var verticesList = new List<Vector3>();
            var indicesList = new List<int>();
            //var transforms = new Matrix[collisionModel.Bones.Count];
            //collisionModel.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (var mesh in model.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {

                    part.GetData(out var partMeshVertices, out var seedPartIndices);
                    foreach (var v in partMeshVertices)
                        verticesList.Add(v.Position * 100);

                    foreach (var ind in seedPartIndices)
                        indicesList.Add(ind);
                }
            }
            vertices = verticesList.ToArray();
            indices = indicesList.ToArray();
        }

        /// <summary>
        /// Converts an XNA Model into a vxMesh
        /// </summary>
        /// <param name="pathToModel"></param>
        /// <param name="content"></param>
        /// <param name="texturePath"></param>
        /// <returns></returns>
        public static vxMesh FromXNAModel(string pathToModel, ContentManager content, string texturePath = "")
        {
            vxConsole.WriteVerboseLine("     Importing Model: " + pathToModel);

            // Create the Model Object to return
            var newModel = new vxMesh(pathToModel);

            // load the monogame/xna version of the model
            var mgModel = vxContentManager.Instance.Load<Model>(pathToModel);

            // Now extract the vertice info
            var tempModelMesh = new vxModelMesh();

            // Load the Textures for hte Model Main.
            foreach (ModelMesh mesh in mgModel.Meshes)
            {
                tempModelMesh.Name = mesh.Name;
                foreach (var effect in mesh.Effects)
                {
                    if (effect is BasicEffect)
                    {
                        var beffect = (BasicEffect)effect;

                        if (beffect.Texture != null)
                        {
                            // we've found a default texture, so let's add it to here
                            tempModelMesh.AddTexture(MeshTextureType.Diffuse, beffect.Texture);
                        }
                    }
                }
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    if (part.Tag == null)
                        part.Tag = mesh;

                    var tempModelMeshPart = new vxModelMeshPart(pathToModel, part);
                    tempModelMeshPart.Tag = part.Tag;
                    tempModelMesh.MeshParts.Add(tempModelMeshPart);
                }

                newModel.AddModelMesh(tempModelMesh);
                tempModelMesh = new vxModelMesh();
            }
            // Set the Bounding Box
            newModel.UpdateBoundingBox();

            //First Load in the Texture packs based off of the mesh name
            vxContentManager.Instance.LoadModelTextures(newModel, pathToModel, content, texturePath);

            return newModel;
        }
    }
}
