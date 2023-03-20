using System.Collections.Generic;
using System.Reflection;

namespace VerticesEngine.UI.Controls
{
    // TODO: Implement
    public class vxPropertyItemFloatArray : vxPropertyItemBaseClass
	{
		float[] array;

		public float value0 { get { return array[0]; } set { array[0] = value; }}
		public float value1 { get { return array[1]; } set { array[1] = value; } }
		public float value2 { get { return array[2]; } set { array[2] = value; } }
		public float value3 { get { return array[3]; } set { array[3] = value; } }
		public float value4 { get { return array[4]; } set { array[4] = value; } }
		public float value5 { get { return array[5]; } set { array[5] = value; } }
		public float value6 { get { return array[6]; } set { array[6] = value; } }
		public float value7 { get { return array[7]; } set { array[7] = value; } }
		public float value8 { get { return array[8]; } set { array[8] = value; } }
		public float value9 { get { return array[9]; } set { array[9] = value; } }
		public float value10 { get { return array[10]; } set { array[10] = value; } }
		public float value11 { get { return array[11]; } set { array[11] = value; } }

        public vxPropertyItemFloatArray(vxPropertyGroup propertyGroup, PropertyInfo PropertyInfo, List<object> TargetObjects) :
        base(propertyGroup, PropertyInfo, TargetObjects)
		{
            //array = (float[])PropertyInfo.GetValue(TargetObject);
            array = new List<float>(12).ToArray();
            //for (int i = 0; i < array.Count(); i++)
            //{
            //	if (i < 12)
            //		Items.Add(new vxPropertyItemFloat(propertyGroup, GetType().GetProperty("value" + i), this));
            //}
        }
	}
}
