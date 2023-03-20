#region Using Statements
using Microsoft.Xna.Framework;
using System;
using System.IO;
using VerticesEngine.Input.Events;
using VerticesEngine.UI;
using VerticesEngine.UI.Events;
using VerticesEngine.UI.MessageBoxs;
using VerticesEngine.Utilities;


#endregion

namespace VerticesEngine
{
    public partial class vxGameplayScene3D
    {


        /// <summary>
        /// Exports the Current File too an STL file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void ExportFileToolbarItem_Clicked(object sender, vxUIControlClickEventArgs e)
        {
            var exportMsgBox = vxMessageBox.Show("Export Scene?", "Would you like to export this scene to an stl file?", vxEnumButtonTypes.OkCancel);

            exportMsgBox.Accepted += ExportMsgBox_Accepted;
        }

        private void ExportMsgBox_Accepted(object sender, PlayerIndexEventArgs e)
        {
            vxSceneManager.AddScene(new vxSceneExportDialog(this));
        }
        /*
        /// <summary>
        /// Exports the scene to an STL file
        /// </summary>
        protected void ExportSceneToSTL()
        {
            try
            {
                //string path = vxIO.Path_Sandbox + "\\" + sandBoxFile.Name;
                string path = Path.Combine(vxIO.PathToSandbox, "_Exports", this.FileName);

                vxIO.EnsureDirExists(path);

                Console.Write("Exporting File...");
                StreamWriter writer = new StreamWriter(Path.Combine(path, FileName + "_export.stl"));
                writer.WriteLine("solid Exported from Vertices Engine");
                foreach (vxEntity3D entity in Entities)
                {
                    Matrix correctionMatrix = entity.Transform.Matrix4x4Transform * Matrix.CreateRotationX(MathHelper.PiOver2);

                    if (entity.HasSandboxOption(SandboxOptions.Export) == true)
                    {
                        // loop through each mesh
                        foreach (var mesh in entity.Model.Meshes)
                        {
                            // loop through each part
                            foreach (var part in mesh.MeshParts)
                            {
                                Console.WriteLine(entity + "-" + part.StartIndex + "-" + part.VertexOffset);
                                // loop through each face
                                for (int i = part.StartIndex; i < part.Indices.Length - 3; i += 3)
                                {

                                    Vector3 Pt1 = Vector3.Transform(part.MeshVertices[part.Indices[i + 0]].Position, correctionMatrix);
                                    Vector3 Pt2 = Vector3.Transform(part.MeshVertices[part.Indices[i + 1]].Position, correctionMatrix);
                                    Vector3 Pt3 = Vector3.Transform(part.MeshVertices[part.Indices[i + 2]].Position, correctionMatrix);

                                    Vector3 Normal = part.MeshVertices[part.Indices[i]].Normal;
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
                writer.WriteLine("endsolid");
                writer.Close();
                Console.WriteLine("Done!");
            }
            catch (Exception ex)
            {
                vxConsole.WriteException(this, ex);
            }
        }
        */
    }
}
