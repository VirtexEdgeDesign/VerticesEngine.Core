using System.Collections.Generic;
using System.Reflection;
using VerticesEngine.Commands;

namespace VerticesEngine.UI.Controls.Commands
{
    class PropertyEntityStruct
    {
        public object Entity;

        public object OriginalValue;

        public PropertyEntityStruct(object Entity, object OriginalValue)
        {
            this.Entity = Entity;
            this.OriginalValue = OriginalValue;
        }
    }

    /// <summary>
    /// CMD which handles Property value change.
    /// </summary>
    public class vxCMDPropertyValueChange : vxCommand
	{
        // The selection set being altered with the previous values.
        List<PropertyEntityStruct> SelectionSet = new List<PropertyEntityStruct>();

        // The Property which is being modified
        PropertyInfo PropertyInfo;

        // The New Value to Apply
        object NewValue;

        vxPropertyItemBaseClass PropertyItemControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.vxCMDPropertyValueChange"/> class.
        /// </summary>
        /// <param name="Scene">Scene.</param>
        /// <param name="SelectionSet">Selection set.</param>
        /// <param name="PropertyInfo">Property info.</param>
        /// <param name="NewValue">New value.</param>
        public vxCMDPropertyValueChange(vxPropertyItemBaseClass PropertyItemControl, 
                                        List<object> SelectionSet, 
                                       object NewValue):
        base(vxEngine.Instance.CurrentScene)
		{
            this.PropertyItemControl = PropertyItemControl;
            this.PropertyInfo = PropertyItemControl.PropertyInfo;

            this.NewValue = NewValue;

            // Process the Items
            foreach (var item in SelectionSet)
            {
                this.SelectionSet.Add(new PropertyEntityStruct(item,
                                                               PropertyInfo.GetValue(item)));
            }
		}

		public override void Do()
		{
			foreach(var obj in SelectionSet)
                PropertyInfo.SetValue(obj.Entity, NewValue);

            PropertyItemControl.RefreshValue();
		}

		public override void Undo()
		{
            foreach (var obj in SelectionSet)
                PropertyInfo.SetValue(obj.Entity, obj.OriginalValue);

            PropertyItemControl.RefreshValue();
		}
	}
}
