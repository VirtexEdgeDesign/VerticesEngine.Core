using Microsoft.Xna.Framework;
using System;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using VerticesEngine.EnvTerrain;
using VerticesEngine.Graphics;
using VerticesEngine.Serilization;
using VerticesEngine.UI.MessageBoxs;
using VerticesEngine.Utilities;

namespace VerticesEngine.UI
{
    /// <summary>
    /// Exports a scene to a given file format
    /// </summary>
    public class vxSceneExportDialog : vxSaveBusyScreen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.vxSaveBusyScreen"/> class.
        /// </summary>
        public vxSceneExportDialog(vxGameplayScene3D Scene) : base(Scene)
        {

        }


        private string outPath = "";

        protected virtual void ReportProgress(int perc)
        {
            if (SaveFileAsyncWriter != null && SaveFileAsyncWriter.IsBusy)
                SaveFileAsyncWriter.ReportProgress(perc);
        }

        public override void OnAsyncWriteSaveFile(object sender, DoWorkEventArgs e)
        {
            //SaveFile((vxGameplayScene3D)e.Argument);
            vxGameplayScene3D CurrentScene = (vxGameplayScene3D)e.Argument;

            //string path = vxIO.Path_Sandbox + "\\" + sandBoxFile.Name;
            string path = Path.Combine(vxIO.PathToSandbox, "_Exports", CurrentScene.FileName);
            outPath = path;
            try
            {
                vxIO.EnsureDirExists(path);

                Console.Write("Exporting File...");
                StreamWriter writer = new StreamWriter(Path.Combine(path, CurrentScene.FileName + "_export.stl"));
                writer.WriteLine("solid Exported from Vertices Engine");
                float currentCount = 0;
                float maxCount = CurrentScene.Entities.Count;
                foreach (vxEntity3D entity in CurrentScene.Entities)
                {
                    try
                    {
                        currentCount++;
                        ReportProgress((int)(currentCount / maxCount * 100));
                        Matrix correctionMatrix = Matrix.CreateScale(entity.RenderScale) * entity.Transform.Matrix4x4Transform * Matrix.CreateRotationX(MathHelper.PiOver2);

                        if (entity.HasSandboxOption(SandboxOptions.Export) == true)
                        {
                            // loop through each mesh
                            foreach (var mesh in entity.Model.Meshes)
                            {
                                // loop through each part
                                foreach (var part in mesh.MeshParts)
                                {
                                    if (part is vxTerrainMeshPart)
                                    {
                                        var terrainPart = (vxTerrainMeshPart)part;
                                        // loop through each face
                                        for (int i = terrainPart.StartIndex; i < terrainPart.Indices.Length; i += 3)
                                        {
                                            
                                            Vector3 Pt1 = Vector3.Transform(terrainPart.MeshVertices[terrainPart.Indices[i + 0]].Position, correctionMatrix);
                                            Vector3 Pt2 = Vector3.Transform(terrainPart.MeshVertices[terrainPart.Indices[i + 1]].Position, correctionMatrix);
                                            Vector3 Pt3 = Vector3.Transform(terrainPart.MeshVertices[terrainPart.Indices[i + 2]].Position, correctionMatrix);

                                            Vector3 Normal = terrainPart.MeshVertices[terrainPart.Indices[i]].Normal;
                                            //Normal.Normalize();
                                            writer.WriteLine(string.Format("facet normal {0} {1} {2}", Normal.X, Normal.Y, Normal.Z));
                                            writer.WriteLine("outer loop");
                                            writer.WriteLine(string.Format("vertex {0} {1} {2}", Pt1.X, Pt1.Y, Pt1.Z));
                                            writer.WriteLine(string.Format("vertex {0} {1} {2}", Pt2.X, Pt2.Y, Pt2.Z));
                                            writer.WriteLine(string.Format("vertex {0} {1} {2}", Pt3.X, Pt3.Y, Pt3.Z));
                                            writer.WriteLine("endloop");
                                            writer.WriteLine("endfacet");
                                        }
                                    }
                                    else
                                    {
                                        part.GetData(out var partMeshVertices, out var partIndices);
                                        // loop through each face
                                        for (int i = part.StartIndex; i < partIndices.Length; i += 3)
                                        {

                                            Vector3 Pt1 = Vector3.Transform(partMeshVertices[partIndices[i + 0]].Position, correctionMatrix);
                                            Vector3 Pt2 = Vector3.Transform(partMeshVertices[partIndices[i + 1]].Position, correctionMatrix);
                                            Vector3 Pt3 = Vector3.Transform(partMeshVertices[partIndices[i + 2]].Position, correctionMatrix);

                                            Vector3 Normal = partMeshVertices[partIndices[i]].Normal;
                                            //Normal.Normalize();
                                            writer.WriteLine(string.Format("facet normal {0} {1} {2}", Normal.X, Normal.Y, Normal.Z));
                                            writer.WriteLine("outer loop");
                                            writer.WriteLine(string.Format("vertex {0} {1} {2}", Pt1.X, Pt1.Y, Pt1.Z));
                                            writer.WriteLine(string.Format("vertex {0} {1} {2}", Pt2.X, Pt2.Y, Pt2.Z));
                                            writer.WriteLine(string.Format("vertex {0} {1} {2}", Pt3.X, Pt3.Y, Pt3.Z));
                                            writer.WriteLine("endloop");
                                            writer.WriteLine("endfacet");
                                        }
                                    }
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        vxConsole.WriteError("Error with entity " + entity);
                        // we hit an error with this entity, but lets keep looping
                        vxConsole.WriteException(entity.ToString(), ex);
                    }
                }
                writer.WriteLine("endsolid");
                writer.Close();
                Console.WriteLine("Done!");

                // open the result
                //System.Diagnostics.Process.Start(path);

                e.Result = true;
            }
            catch (Exception ex)
            {
                vxConsole.WriteException(this, ex);

                e.Result = false;
            }
        }

        protected override void OnFinished(bool success)
        {
            base.OnFinished(success);

            if (success)
            {
                vxMessageBox.Show("Export Complete", "Scene exported to file:\n" + outPath);
            }
            else 
            { 
                vxMessageBox.Show("Error Exporting File", "There were some errors while exporting the file.");
            }
        }
    }
}
