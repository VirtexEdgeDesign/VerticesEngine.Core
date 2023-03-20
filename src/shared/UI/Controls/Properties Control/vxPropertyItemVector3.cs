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
    public class vxPropertyItemVector3 : vxPropertyItemBaseClass
    {
        Vector3 Vector;

        public float X
        {
            get { return Vector.X; }
            set { Vector = new Vector3(value, Vector.Y, Vector.Z); SetValue(); }
        }

        public float Y
        {
            get { return Vector.Y; }
            set { Vector = new Vector3(Vector.X, value, Vector.Z); SetValue(); }
        }
        public float Z
        {
            get { return Vector.Z; }
            set { Vector = new Vector3(Vector.X, Vector.Y, value); SetValue(); }
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
        vxPropertyItemBaseClass ZsubItem;

        public vxPropertyItemVector3(vxPropertyGroup propertyGroup, PropertyInfo PropertyInfo, List<object> TargetObjects) :
        base(propertyGroup, PropertyInfo, TargetObjects)
        {
            PropertyInfo xProperty = this.GetType().GetProperty("X");
            PropertyInfo yProperty = this.GetType().GetProperty("Y");
            PropertyInfo zProperty = this.GetType().GetProperty("Z");
            this.IsReadOnly = true;
            this.IsEnabled = false;
            // Create temp collection
            List<object> slctnst = new List<object>();
            slctnst.Add(this);

            XsubItem = new vxPropertyItemBaseClass(propertyGroup, xProperty, slctnst);
            YsubItem = new vxPropertyItemBaseClass(propertyGroup, yProperty, slctnst);
            ZsubItem = new vxPropertyItemBaseClass(propertyGroup, zProperty, slctnst);

            Items.Add(XsubItem);
            Items.Add(YsubItem);
            Items.Add(ZsubItem);

            //Console.WriteLine(GetPropertyValue());
            GetPropertyValue();

            this.PropertyBox.IsEnabled = false;
        }

        public override object GetPropertyValue()
        {
            object result = base.GetPropertyValue();

            if (result is Vector3)
            {
                Vector = (Vector3)result;
                if (XsubItem != null)
                    XsubItem.Value = Vector.X.ToString();
                if (YsubItem != null)
                    YsubItem.Value = Vector.Y.ToString();

                if (ZsubItem != null)
                    ZsubItem.Value = Vector.Z.ToString();
            }
            else if (result is PropertyResponse)
            {
                Value = VARIES_TEXT;

                if (XsubItem != null)
                    XsubItem.Value = VARIES_TEXT;
                if (YsubItem != null)
                    YsubItem.Value = VARIES_TEXT;
                if (ZsubItem != null)
                    ZsubItem.Value = VARIES_TEXT;
            }

            return result;
        }

        const string SIG_DIGs = "#,##0.0#";

        public override string GetPropertyValueAsString()
        {
            GetPropertyValue();

            return $"(" +
                $"{Vector.X.ToString(SIG_DIGs)}, " +
                $"{Vector.Y.ToString(SIG_DIGs)}, " +
                $"{Vector.Z.ToString(SIG_DIGs)}" +
                $")";
        }


        public override bool OnSelectionSetPropertyCompairson(object currentObject, object previousObject)
        {
            return ((Vector3)currentObject == (Vector3)previousObject);
        }
    }
}
