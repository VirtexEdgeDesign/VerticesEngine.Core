using System.Collections.Generic;
using System.Reflection;

namespace VerticesEngine.UI.Controls
{
    public class vxPropertyControlFloatTextbox : vxPropertyControlTextbox
    {
        public vxPropertyControlFloatTextbox(vxPropertyItemBaseClass Property, string InitialValue) :
        base(Property, InitialValue)
        {

        }

        public override string FilterTextInput(string input)
        {
            float result;
            if (float.TryParse(input, out result))
            {
                return input;
            }
            else
            {
                return PreviousText;
            }
        }
    }

	public class vxPropertyItemFloat : vxPropertyItemBaseClass
    {
        public vxPropertyItemFloat(vxPropertyGroup propertyGroup, PropertyInfo PropertyInfo, List<object> TargetObjects) :
        base(propertyGroup, PropertyInfo, TargetObjects)
        {
			
        }

        public override vxUIControl CreatePropertyInputControl()
        {
            Value = Value == null ? "" : Value;
            return new vxPropertyControlFloatTextbox(this, Value);
        }
    }

}
