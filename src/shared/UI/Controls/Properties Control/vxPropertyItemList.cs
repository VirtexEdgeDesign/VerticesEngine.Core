using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using VerticesEngine.UI.Controls.Commands;
using VerticesEngine.UI.Themes;

namespace VerticesEngine.UI.Controls
{
    /// <summary>
    /// Label Class providing simple one line text as a vxGUI Item.
    /// </summary>
  //  public class vxPropertyComboBox : vxComboBox
  //  {
		///// <summary>
  //      /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxPropertyComboBox"/> class.
  //      /// </summary>
  //      /// <param name="Property">Property.</param>
  //      /// <param name="text">Text.</param>
  //      /// <param name="Value">Value.</param>
  //      public vxPropertyComboBox(vxPropertyItemBaseClass Property, string text, string Value) : 
  //      base(text, Vector2.Zero)
  //      {
  //          ItemPadding = 3;
  //          TextJustification = vxEnumTextHorizontalJustification.Left;
  //          this.Font = vxInternalAssets.Fonts.ViewerFont;

  //          Theme.Background = new vxColourTheme(
  //          Color.Transparent,
  //              Color.DarkOrange,
  //              Color.DarkOrange);
  //          Theme.Text = new vxColourTheme(
  //          Color.White * 0.75f,
  //              Color.White,
  //              Color.White);
            
  //          DoBorder = false;
  //      }
  //  }


    public class vxPropertyItemList : vxPropertyItemBaseClass
    {
        vxPropertyComboBox ComboBoxControl;

        public vxPropertyItemList(vxPropertyGroup propertyGroup, PropertyInfo PropertyInfo, List<object> TargetObjects) :
        base(propertyGroup, PropertyInfo, TargetObjects)
        {
           
        }


        public override vxUIControl CreatePropertyInputControl()
		{
			string InitValue = GetPropertyValueAsString();

			ComboBoxControl = new vxPropertyComboBox(this, InitValue, InitValue);

			ComboBoxControl.UIManager = vxEngine.Instance.CurrentScene.UIManager;

            ComboBoxControl.Font = Font;

            var val = (vxItemList)PropertyInfo.GetValue(TargetObjects[0]);

            foreach (string item in val)
                ComboBoxControl.AddItem(item.ToString());

            ComboBoxControl.SelectionChanged += ComboBoxControl_SelectionChanged;

			ComboBoxControl.Height = 16;

			return ComboBoxControl;
		}

		void ComboBoxControl_SelectionChanged(object sender, Events.vxComboBoxSelectionChangedEventArgs e)
        {
            vxConsole.WriteLine("Setting new value" + e.SelectedIndex);

            if (list != null)
                list.SelectedIndex = e.SelectedIndex;

            SetValue(list);
		}


        vxItemList list;

        public override object GetPropertyValue()
        {
            object result = base.GetPropertyValue();

            if (result is vxItemList)
            {
                list = (vxItemList)result;
            }

            return result;
        }

        public override string GetPropertyValueAsString()
        {
            object result = base.GetPropertyValue();

            if (result is vxItemList)
            {
                list = (vxItemList)result;
                return list.SelectedItem;
            }

            return base.GetPropertyValueAsString();
        }

    }
}
