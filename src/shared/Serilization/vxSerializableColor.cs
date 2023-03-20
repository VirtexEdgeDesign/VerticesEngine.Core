using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace VerticesEngine.Serilization
{
    public class vxSerializableColor
    {
        [XmlAttribute("R")]
        public float R = 200;
        [XmlAttribute("G")]
        public float G = 200;
        [XmlAttribute("B")]
        public float B = 200;
        [XmlAttribute("A")]
        public float A = 200;



        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                R = _color.R;
                G = _color.G;
                B = _color.B;
                A = _color.A;
            }
        }
        Color _color;

        public vxSerializableColor()
        {
            Color = Color.GhostWhite;
        }
        public vxSerializableColor(Color color)
        {
            Color = color;
        }
    }
}
