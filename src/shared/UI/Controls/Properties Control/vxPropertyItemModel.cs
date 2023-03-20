using System.Collections.Generic;
using System.Reflection;
using VerticesEngine.Graphics;

namespace VerticesEngine.UI.Controls
{
    public class vxPropertyItemModel : vxPropertyItemBaseClass
	{
        vxMesh Model;


        public new string Name
        {
            get { return Model.Name; }
        }


        public int TotalPrimitiveCount
		{
			get { return Model.TotalPrimitiveCount; }
		}

        public int MeshCount
        {
            get { return Model.Meshes.Count; }
        }

        public vxModelMesh Mesh0 { get { return Model.Meshes[0]; } }
        public vxModelMesh Mesh1 { get { return Model.Meshes[1]; } }
        public vxModelMesh Mesh2 { get { return Model.Meshes[2]; } }
        public vxModelMesh Mesh3 { get { return Model.Meshes[3]; } }
        public vxModelMesh Mesh4 { get { return Model.Meshes[4]; } }
        public vxModelMesh Mesh5 { get { return Model.Meshes[5]; } }
        public vxModelMesh Mesh6 { get { return Model.Meshes[6]; } }
        public vxModelMesh Mesh7 { get { return Model.Meshes[7]; } }
        public vxModelMesh Mesh8 { get { return Model.Meshes[8]; } }
        public vxModelMesh Mesh9 { get { return Model.Meshes[9]; } }
        public vxModelMesh Mesh10 { get { return Model.Meshes[10]; } }
        public vxModelMesh Mesh11 { get { return Model.Meshes[11]; } }


        public vxPropertyItemModel(vxPropertyGroup propertyGroup, PropertyInfo PropertyInfo, List<object> TargetObjects) :
        base(propertyGroup, PropertyInfo, TargetObjects)
		{
            //Model = (vxModel)PropertyInfo.GetValue(TargetObject);
            GetPropertyValue();

            List<object> slctnst = new List<object>();
            slctnst.Add(this);


            // Only add these items if it's the same
            if (title != VARIES_TEXT && Model != null)
            {
                Items.Add(new vxPropertyItemBaseClass(propertyGroup, GetType().GetProperty("Name"), slctnst));
                //Items.Add(new vxPropertyItemBaseClass(propertyGroup, GetType().GetProperty("ModelPath"), slctnst));
                Items.Add(new vxPropertyItemBaseClass(propertyGroup, GetType().GetProperty("TotalPrimitiveCount"), slctnst));
                Items.Add(new vxPropertyItemBaseClass(propertyGroup, GetType().GetProperty("MeshCount"), slctnst));

            	for (int i = 0; i < MeshCount; i++)
                {
                    if (i < 12)
                        Items.Add(new vxPropertyItemModelMesh(propertyGroup, GetType().GetProperty("Mesh"+i), slctnst));
                }
            }
		}

        public override object GetPropertyValue()
        {
            object result = base.GetPropertyValue();

            if (result is vxMesh)
            {
                Model = ((vxMesh)result);
                title = Model.Name;
            }
            else if (result is PropertyResponse)
                title = VARIES_TEXT;

            return result;
        }
        string title = "";

        public override string GetPropertyValueAsString()
        {
            GetPropertyValue();
            return title;
        }
	}
}
