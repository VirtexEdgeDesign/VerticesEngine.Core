using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VerticesEngine.ContentManagement;
using VerticesEngine.Utilities;

namespace VerticesEngine.Graphics
{
    /// <summary>
    /// A set of textures which include a 
    /// </summary>
    public class TextureSet
    {
        public Texture2D DiffuseTexture = vxInternalAssets.Textures.ErrorTexture;
        public Texture2D NormalMap = vxInternalAssets.Textures.DefaultNormalMap;
        public Texture2D RMAMap = vxInternalAssets.Textures.DefaultSurfaceMap;
    }

    /// <summary>
    /// The vxMesh Helper Class helps
    /// </summary>
    public static partial class vxMeshHelper
    {
        static TextureSet LoadOBJMaterial(string path)
        {
            // loads an obj material and returns the texture
            TextureSet txtrSet = new TextureSet();


            // Read in the file.
            StreamReader reader = new StreamReader(path);

            // reaad in all text
            string fileText = reader.ReadToEnd();

            // split the line into lines
            string[] lines = System.Text.RegularExpressions.Regex.Split(fileText, @"\r?\n|\r");

            var rootPath = new DirectoryInfo(path).Parent.FullName;

            foreach (string line in lines)
            {
                string[] tokens = line.Split(new char[0]);

                if (tokens.Length > 0)
                {
                    switch (tokens[0])
                    {
                        case "map_Kd":

                            var txtrPath = Path.Combine(rootPath, tokens[1]);
                            txtrSet.DiffuseTexture = vxIO.LoadImage(txtrPath, false);

                            break;
                        case "map_Bump":

                            var nmPath = Path.Combine(rootPath, tokens[1]);
                            txtrSet.NormalMap = vxIO.LoadImage(nmPath, false);

                            break;
                    }
                }
            }


            return txtrSet;
        }

        static vxImportResult ImportOBJ(string path)
        {
            vxImportResult ImportResult = new vxImportResult();

            try
            {
                if(!File.Exists(path))
                {
                    var missingFileResults = new List<string>();
                    missingFileResults.Add("Could not find path");
                    return new vxImportResult(missingFileResults);
                }

                // Read in the file.
                StreamReader reader = new StreamReader(path);

                // reaad in all text
                string fileText = reader.ReadToEnd();

                // split the line into lines
                string[] lines = System.Text.RegularExpressions.Regex.Split(fileText, @"\r?\n|\r");

                // Variable holding the Normal for this current 'facet' group
                Vector3 currentNormal = Vector3.Zero;

                // Model to return
                vxMesh model = new vxMesh(path);
                //model.ModelMain = vxInternalAssets.Models.UnitBox.ModelMain;

                // initial mesh
                vxModelMesh mesh = new vxModelMesh();

                List<Vector3> VerticesPoints = new List<Vector3>();
                List<Vector3> Normals = new List<Vector3>();
                List<Vector2> UVs = new List<Vector2>();

                List<vxMeshVertex> Vertices = new List<vxMeshVertex>();
                List<ushort> Indices = new List<ushort>();
                vxModelMeshPart part;

                TextureSet latestTexture = new TextureSet();

                // Now loop through the 'lines'
                foreach (string line in lines)
                {
                    string[] tokens = line.Split(new char[0]);

                    if (tokens.Length > 0)
                    {
                        switch (tokens[0])
                        {
                            // create new mesh object
                            case "o":

                                if (Vertices.Count > 0)
                                {
                                    part = new vxModelMeshPart(Vertices.ToArray(), Indices.ToArray(), Indices.Count / 3);
                                    mesh.MeshParts.Add(part);
                                }

                                mesh = new vxModelMesh();
                                mesh.Name = tokens[1];
                                model.AddModelMesh(mesh);

                                mesh.AddTexture(MeshTextureType.Diffuse, latestTexture.DiffuseTexture);
                                mesh.AddTexture(MeshTextureType.NormalMap, latestTexture.NormalMap);

                                Vertices = new List<vxMeshVertex>();
                                Indices = new List<ushort>();

                                // Clear All
                                VerticesPoints.Clear();
                                Normals.Clear();
                                UVs.Clear();
                                break;
                            case "mtllib":

                                var materialFileName = tokens[1];
                                DirectoryInfo rootDir = new DirectoryInfo(path);

                                latestTexture = LoadOBJMaterial(Path.Combine(rootDir.Parent.FullName, materialFileName));

                                //mesh.AddTexture(MeshTextureType.Diffuse, txtr);

                                break;
                            case "v":
                                VerticesPoints.Add(new Vector3(float.Parse(tokens[1]), float.Parse(tokens[2]), float.Parse(tokens[3])));
                                break;
                            case "vn":
                                Normals.Add(new Vector3(float.Parse(tokens[1]), float.Parse(tokens[2]), float.Parse(tokens[3])));
                                break;
                            case "vt":
                                UVs.Add(new Vector2(float.Parse(tokens[1]), 1 - float.Parse(tokens[2])));
                                break;
                            case "f":

                                // split token by bracket
                                for (int i = 1; i < 4; i++)
                                {
                                    string[] indices = tokens[4 - i].Split('/');

                                    var tempVert = new vxMeshVertex();

                                    var v = int.Parse(indices[0]) - 1;
                                    var vt = int.Parse(indices[1]) - 1;
                                    var vn = int.Parse(indices[2]) - 1;

                                    tempVert.Position = VerticesPoints[v];
                                    tempVert.Normal = Normals[vn];
                                    tempVert.BiNormal = Normals[vn];
                                    tempVert.Tangent = Normals[vn];
                                    tempVert.TextureCoordinate = UVs[vt];

                                    Vertices.Add(tempVert);
                                    Indices.Add((ushort)Indices.Count());
                                }

                                break;

                        }
                    }
                }
                part = new vxModelMeshPart(Vertices.ToArray(), Indices.ToArray(), Indices.Count / 3);
                mesh.MeshParts.Add(part);

                vxContentManager.Instance.LoadModelTextures(model, "", vxEngine.Game.Content, "", true);

                ImportResult = new vxImportResult(model);

            }
            catch (Exception ex)
            {
                List<string> errors = new List<string>();
                errors.Add(ex.Message);
                ImportResult = new vxImportResult(errors);
            }

            return ImportResult;
        }
    }
}

