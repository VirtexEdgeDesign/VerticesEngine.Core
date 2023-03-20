using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Utilities;
using VerticesEngine;
using VerticesEngine.UI.Events;
using Microsoft.Xna.Framework.Audio;
using VerticesEngine.UI.Themes;
using System.Collections.Generic;
using System.Reflection;

namespace VerticesEngine.UI.Controls
{
    public class vxPropertyItemVector2 : vxPropertyItemBaseClass
    {
        Vector2 Vector;

        public float X
        {
            get { return Vector.X; }
            set { Vector = new Vector2(value, Vector.Y); SetValue(); }
        }

        public float Y
        {
            get { return Vector.Y; }
            set { Vector = new Vector2(Vector.X, value); SetValue(); }
        }


        public void SetValue()
        {
            try
            {
                this.SetValue(Vector);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        vxPropertyItemBaseClass XsubItem;
        vxPropertyItemBaseClass YsubItem;

        public vxPropertyItemVector2(vxPropertyGroup propertyGroup, PropertyInfo PropertyInfo, List<object> TargetObjects) :
        base(propertyGroup, PropertyInfo, TargetObjects)
        {
            PropertyInfo xProperty = this.GetType().GetProperty("X");
            PropertyInfo yProperty = this.GetType().GetProperty("Y");

            // Create temp collection
            List<object> slctnst = new List<object>();
            slctnst.Add(this);

            XsubItem = new vxPropertyItemBaseClass(propertyGroup, xProperty, slctnst);
            YsubItem = new vxPropertyItemBaseClass(propertyGroup, yProperty, slctnst);

            Items.Add(XsubItem);
            Items.Add(YsubItem);

            PropertyBox.IsEnabled = false;

            //Console.WriteLine(GetPropertyValue());
            GetPropertyValue();
        }

        public override string GetPropertyValueAsString()
        {
            object result = base.GetPropertyValue();

            if (result is Vector2)
            {
                Vector = (Vector2)result;
                return string.Format("(x:{0}, y:{1})", Vector.X.ToString("0.##"), Vector.Y.ToString("0.##"));
            }

            return VARIES_TEXT;
        }
        
        public override object GetPropertyValue()
        {
            object result = base.GetPropertyValue();

            if (result is Vector2)
            {
                Vector = (Vector2)result;
                XsubItem.Value = Vector.X.ToString();
                YsubItem.Value = Vector.Y.ToString();
            }
            else if (result is PropertyResponse)
            {
                Value = VARIES_TEXT;

                XsubItem.Value = VARIES_TEXT;
                YsubItem.Value = VARIES_TEXT;
            }


            return result;
        }


        public override bool OnSelectionSetPropertyCompairson(object currentObject, object previousObject)
        {
            return ((Vector2)currentObject == (Vector2)previousObject);
        }
    }
}
