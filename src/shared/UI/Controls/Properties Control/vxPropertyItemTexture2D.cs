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
    public class vxPropertyControlTexture2D : vxUIControl
    {
        public Texture2D Texture;

        /// <summary>
        /// Initializes a new instance of the <see cref="VerticesEngine.UI.Controls.vxLabel"/> class.
        /// </summary>
        /// <param name="Engine">The Vertices Engine Reference.</param>
        /// <param name="text">This GUI Items Text.</param>
        /// <param name="position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
        public vxPropertyControlTexture2D(vxPropertyItemBaseClass Property, Texture2D Value, int size = 128)
        {
            ////Colour_Text = Engine.GUITheme.vxLabelColorNormal;

            this.Font = vxInternalAssets.Fonts.ViewerFont;
            //System.IO.FileInfo fi;// = System.IO.File.

            Text = Value.Name.GetFileNameFromPath();

            Height = Width = size;
            Texture = Value;
            this.size = size;

            ColourRect = new Rectangle(0, 0, size, size);
        }

        int size = 128;
        Rectangle ColourRect = new Rectangle(0, 0, 12, 12);

        /// <summary>
        /// Draws the GUI Item
        /// </summary>
        public override void Draw()
        {
            ColourRect.Location = (Position + new Vector2(2, -2)).ToPoint();
            //SpriteBatch.Draw(DefaultTexture, ColourRect.GetBorder(1), Color.Black);
            //SpriteBatch.Draw(Texture, ColourRect, Color.White);
            SpriteBatch.Draw(DefaultTexture, ColourRect.GetBorder(-1), Color.Black);
            SpriteBatch.Draw(Texture, ColourRect.GetBorder(-2), Color.White);

            if(size != 128)
                SpriteBatch.DrawString(Font, Text, Position + new Vector2(20, 0), GetStateColour(Theme.Text));
        }
    }
    public class vxPropertyItemTexture2D : vxPropertyItemBaseClass
    {
        vxPropertyControlTexture2D Image;

        public Texture2D Texture
        {
            get { return _texture; }
        }
        Texture2D _texture;

        public new string Name
        {
            get { return Texture.Name; }
        }

        public string Size
        {
            get { return string.Format("{0}x{1}", Texture.Bounds.Width, Texture.Bounds.Height); }
        }

        public string Format
        {
            get { return Texture.Format.ToString(); }
        }


        public string LevelCount
        {
            get { return Texture.LevelCount.ToString(); }
        }

        public string Tag
        {
            get
            {
                string tag = Texture.Tag == null ? "" : Texture.Tag.ToString();
                return tag;
            }
        }


        public vxPropertyItemTexture2D(vxPropertyGroup propertyGroup, PropertyInfo PropertyInfo, List<object> TargetObjects) :
        base(propertyGroup, PropertyInfo, TargetObjects)
        {
            //GetPropertyValue();
            List<object> slctnst = new List<object>();
            slctnst.Add(this);

            vxPropertyItemTexture2DLargeView TextureControl;

            TextureControl = new vxPropertyItemTexture2DLargeView(propertyGroup, GetType().GetProperty("Texture"), slctnst);

            Items.Add(new vxPropertyItemBaseClass(propertyGroup, GetType().GetProperty("Name"), slctnst));
            Items.Add(new vxPropertyItemBaseClass(propertyGroup, GetType().GetProperty("Size"), slctnst));
            Items.Add(new vxPropertyItemBaseClass(propertyGroup, GetType().GetProperty("Format"), slctnst));
            Items.Add(new vxPropertyItemBaseClass(propertyGroup, GetType().GetProperty("LevelCount"), slctnst));
            Items.Add(new vxPropertyItemBaseClass(propertyGroup, GetType().GetProperty("Tag"), slctnst));

            Items.Add(TextureControl);
        }

        public override vxUIControl CreatePropertyInputControl()
        {
            //Texture = (Texture2D)PropertyInfo.GetValue(TargetObject);
            Image = new vxPropertyControlTexture2D(this, (Texture2D)GetPropertyValue(), 16);
            return Image;
        }


        public override object GetPropertyValue()
        {
            object result = base.GetPropertyValue();

            if (result is Texture2D)
                _texture = (Texture2D)result;
            else if (result is PropertyResponse)
                _texture = DefaultTexture;

            return Texture;
        }


        public override bool OnSelectionSetPropertyCompairson(object currentObject, object previousObject)
        {
            return ((Texture2D)currentObject == (Texture2D)previousObject);
        }

        public override string GetPropertyValueAsString()
        {
            try
            {
                return ((Texture2D)GetPropertyValue()).Name;
            }
            catch
            {
                return "";
            }
        }

        //public override int GetHeight()
        //{
        //    return 128;
        //}
    }


    public class vxPropertyItemTexture2DLargeView : vxPropertyItemBaseClass
    {
        vxPropertyControlTexture2D Image;

        Texture2D Texture;

        public vxPropertyItemTexture2DLargeView(vxPropertyGroup propertyGroup, PropertyInfo PropertyInfo, List<object> TargetObjects) :
        base(propertyGroup, PropertyInfo, TargetObjects)
        {

        }

        public override vxUIControl CreatePropertyInputControl()
        {
            //Texture = (Texture2D)PropertyInfo.GetValue(TargetObject);
            Image = new vxPropertyControlTexture2D(this, (Texture2D)GetPropertyValue());
            return Image;
        }


        public override object GetPropertyValue()
        {
            object result = base.GetPropertyValue();

            if (result is Texture2D)
                Texture = (Texture2D)result;
            else if (result is PropertyResponse)
                Texture = DefaultTexture;

            return Texture;
        }


        public override bool OnSelectionSetPropertyCompairson(object currentObject, object previousObject)
        {
            return ((Texture2D)currentObject == (Texture2D)previousObject);
        }

        public override string GetPropertyValueAsString()
        {
            try
            {
                return ((Texture2D)GetPropertyValue()).Name;
            }
            catch
            {
                return "";
            }
        }

        public override int GetHeight()
        {
            return 128;
        }
    }
}
