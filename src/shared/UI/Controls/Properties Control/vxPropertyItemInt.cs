using System.Collections.Generic;
using System.Reflection;

namespace VerticesEngine.UI.Controls
{
    public class vxPropertyItemIntTextbox : vxPropertyControlTextbox
    {
        public vxPropertyItemIntTextbox(vxPropertyItemBaseClass Property, string InitialValue) :
        base(Property, InitialValue)
        {
            IsFloat = false;
        }

        public override string FilterTextInput(string input)
        {
            int result;
            if (int.TryParse(input, out result))
            {
                return input;
            }
            else
            {
                return PreviousText;
            }
        }
    }

	public class vxPropertyItemInt : vxPropertyItemBaseClass
    {
        public vxPropertyItemInt(vxPropertyGroup propertyGroup, PropertyInfo PropertyInfo, List<object> TargetObjects) :
        base(propertyGroup, PropertyInfo, TargetObjects)
        {
			
        }

        public override vxUIControl CreatePropertyInputControl()
        {
            Value = Value == null ? "" : Value;
            return new vxPropertyItemIntTextbox(this, Value);
        }
    }

}
