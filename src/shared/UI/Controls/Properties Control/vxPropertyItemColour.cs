using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Reflection;

namespace VerticesEngine.UI.Controls
{
    /// <summary>
    /// Label Class providing simple one line text as a vxGUI Item.
    /// </summary>
    public class vxPropertyControlColour : vxLabel
    {
        public Color Colour
        {
            get { return _colour; }
            set
            {
                _colour = value;
                DisplayText = _colour.ToString();
            }
        }
        Color _colour;
        public string DisplayText = "";

		/// <summary>
		/// Initializes a new instance of the <see cref="VerticesEngine.UI.Controls.vxLabel"/> class.
		/// </summary>
		/// <param name="Engine">The Vertices Engine Reference.</param>
		/// <param name="text">This GUI Items Text.</param>
		/// <param name="position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
        public vxPropertyControlColour(vxPropertyItemBaseClass Property, string text, string Label) : 
        base(text, Vector2.Zero)
        {
			////Colour_Text = Engine.GUITheme.vxLabelColorNormal;

            this.Font = vxInternalAssets.Fonts.ViewerFont;
            Width = (int)this.Font.MeasureString(Text).X;


            //Colour = Value;
        }
        Rectangle ColourRect = new Rectangle(0, 0, 12, 12);

		/// <summary>
		/// Draws the GUI Item
		/// </summary>
		public override void Draw()
        {
            ColourRect.Location = (Position + new Vector2(2, 0)).ToPoint();
            SpriteBatch.Draw(DefaultTexture, ColourRect.GetBorder(1), Color.Black);
            SpriteBatch.Draw(DefaultTexture, ColourRect, Colour);
		}

		public override void DrawText()
		{
            SpriteBatch.DrawString(Font, DisplayText, Position + new Vector2(20, 0), Theme.Text.Color);

		}
    }

    public class vxPropertyItemColour : vxPropertyItemBaseClass
    {
        vxPropertyControlColour ColourControl;

        /// <summary>
        /// Gets or sets the colour value.
        /// </summary>
        /// <value>The colour value.</value>
        Color ColourValue
        {
            get { return _colourValue; }   
            set 
            { 
                _colourValue = value;
                Value = _colourValue.ToString();
                ColourControl.Colour = value;
            }
        }
        Color _colourValue;

        public float R
        {
            get { return ColourValue.R; }
            set {
                //Console.WriteLine(ColourValue.G);
                ColourValue = new Color(value / 255.0f, ColourValue.G/ 255.0f, ColourValue.B / 255.0f); UpdateValue(); }
        }

        public float G
        {
            get { return ColourValue.G; }
            set { ColourValue = new Color(ColourValue.R / 255.0f, value / 255.0f, ColourValue.B / 255.0f); UpdateValue(); }
        }
        public float B
        {
            get { return ColourValue.B; }
            set { ColourValue = new Color(ColourValue.R / 255.0f, ColourValue.G / 255.0f, value / 255.0f); UpdateValue(); }
        }


        vxPropertyItemBaseClass XsubItem;
        vxPropertyItemBaseClass YsubItem;
        vxPropertyItemBaseClass ZsubItem;

        public vxPropertyItemColour(vxPropertyGroup propertyGroup, PropertyInfo PropertyInfo, List<object> TargetObjects) :
        base(propertyGroup, PropertyInfo, TargetObjects)
        {
            PropertyInfo rProperty = this.GetType().GetProperty("R");
            PropertyInfo gProperty = this.GetType().GetProperty("G");
            PropertyInfo bProperty = this.GetType().GetProperty("B");

            
            // Create temp collection
            List<object> slctnst = new List<object>();
            slctnst.Add(this);

            XsubItem = new vxPropertyItemFloatRange(propertyGroup, rProperty, slctnst, 0, 255, 1);
            YsubItem = new vxPropertyItemFloatRange(propertyGroup, gProperty, slctnst, 0, 255, 1);
            ZsubItem = new vxPropertyItemFloatRange(propertyGroup, bProperty, slctnst, 0, 255, 1);

            Items.Add(XsubItem);
            Items.Add(YsubItem);
            Items.Add(ZsubItem);
            //Console.WriteLine(GetPropertyValue());
            GetPropertyValue();
        }

        public void UpdateValue()
        {
            SetValue(ColourValue);

            ColourControl.Colour = ColourValue;
        }

        public override object GetPropertyValue()
        {
            object result = base.GetPropertyValue();

            if (result is Color)
            {
                ColourValue = (Color)result;
                XsubItem.Value = ColourValue.R.ToString();
                YsubItem.Value = ColourValue.G.ToString();
                ZsubItem.Value = ColourValue.B.ToString();
            }
            else if (result is PropertyResponse)
            {
                Value = VARIES_TEXT;

                XsubItem.Value = VARIES_TEXT;
                YsubItem.Value = VARIES_TEXT;
                ZsubItem.Value = VARIES_TEXT;
            }

            return result;
        }

        public override void OnValueChange()
        {
            base.OnValueChange();

            if(ColourControl != null)
                ColourControl.DisplayText = Value;
        }

        public override bool OnSelectionSetPropertyCompairson(object currentObject, object previousObject)
        {
            return ((Color)currentObject == (Color)previousObject);
        }

		public override vxUIControl CreatePropertyInputControl()
        {
            ColourControl = new vxPropertyControlColour(this, Text, GetPropertyValueAsString());

			return ColourControl;
		}
    }
}
