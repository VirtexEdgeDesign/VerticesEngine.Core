using System;
using System.Collections.Generic;
using System.IO;


namespace VerticesEngine.Graphics
{
    public enum vxImportResultStatus
    {
        Success,
        Warnings,
        Errors
    }
    public class vxImportResult
    {
        public vxImportResultStatus ImportResultStatus;
        public List<string> Errors = new List<string>();
        public List<string> Warnings = new List<string>();

        public readonly vxMesh ImportedModel;

        public vxImportResult()
        {
            ImportResultStatus = vxImportResultStatus.Errors;
        }

        public vxImportResult(vxMesh model)
        {
            ImportedModel = model;
            ImportResultStatus = vxImportResultStatus.Success;
        }

        public vxImportResult(vxMesh model, List<string> warnings)
        {
            ImportedModel = model;
            ImportResultStatus = vxImportResultStatus.Warnings;
            Warnings.AddRange(warnings);
        }

        public vxImportResult(List<string> errors)
        {
            ImportResultStatus = vxImportResultStatus.Errors;
            Errors.AddRange(errors);
        }
    }
    /// <summary>
    /// A Model Class which loads and processes all data at runtime. Although this add's to load times,
    /// it allows for more control as well as modding for any and all models which are used in the game.
    /// Using three different models to handle different types of rendering does add too over all installation
    /// size, it is necessary to allow the shaders to be compiled for cross platform use.
    /// </summary>
    public static partial class vxMeshHelper
    {
        public static vxImportResult Import(string filepath)
        {
            FileInfo file = new FileInfo(filepath);
            vxImportResult result = new vxImportResult();
            switch (file.Extension)
            {
                case ".obj":
                    result = ImportOBJ(filepath);
                    break;
            }
            return result;
        }
    }
}

