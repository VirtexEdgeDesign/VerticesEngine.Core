using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VerticesEngine.Graphics;

namespace VerticesEngine.Entities
{
    /// <summary>
    /// Imported sandbox entity 
    /// </summary>
    public class vxImportedEntity3D : vxEntity3D
    {
        [vxSerialise]
        public string ImportedModelGuid
        {
            get { return m_guid; }
            set { m_guid = value; }
        }
        public string m_guid = string.Empty;

        public vxImportedEntity3D(vxGameplayScene3D scene) : base(scene, vxInternalAssets.Models.UnitBox, Vector3.Zero)
        {

        }

        public void InitImportedEntity(string guid)
        {
            if (guid == null)
                return;

                m_guid = guid;

            var importedFiles = vxEngine.Instance.GetCurrentScene<vxGameplayScene3D>().importedFiles;

            if (importedFiles.ContainsKey(m_guid))
            {
                Model = importedFiles[m_guid].Model;
                for (int m = 0; m < Model.Meshes.Count; m++)
                {
                    var mat = MeshRenderer.GetMaterial(m);

                    OnRefreshMaterialTextures(Model.Meshes[m], mat);
                }
            }
            else
            {
                vxConsole.WriteError("MISSING IMPORTED GUID - " + this.m_guid);
            }
        }

        public override void OnAfterEntityDeserialized()
        {
            base.OnAfterEntityDeserialized();

            // set the guid
            InitImportedEntity(m_guid);
        }

        protected override vxMesh OnLoadModel()
        {
            return base.OnLoadModel();
        }

        protected override vxMaterial OnMapMaterialToMesh(vxModelMesh mesh)
        {
            return base.OnMapMaterialToMesh(mesh);
        }

        protected override void OnRefreshMaterialTextures(vxModelMesh mesh, vxMaterial material)
        {
            base.OnRefreshMaterialTextures(mesh, material);
        }
    }
}
