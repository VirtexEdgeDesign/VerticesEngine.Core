using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.Utilities
{
    public static class vxColourUtil
    {
        /// <summary>
        /// Gets an RGB colour from a hue value.
        /// </summary>
        /// <param name="hue">Hue value from 0 to 1</param>
        /// <returns></returns>
        public static Color HueToRGB(double hue)
        {
            return HsvToRGB(hue, 1, 1);
        }

        /// <summary>
        /// Converts an HSV to RGB value
        /// </summary>
        /// <param name="hue">Hue value from 0 to 1</param>
        /// <param name="saturation">Saturation value from 0 to 1</param>
        /// <param name="value">Value from 0 to 1</param>
        /// <returns></returns>
        public static Color HsvToRGB(double hue, double saturation, double value)
        {
            hue = hue * 360;
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return new Color(v, t, p, 255);
            else if (hi == 1)
                return new Color(q, v, p, 255);
            else if (hi == 2)
                return new Color(p, v, t, 255);
            else if (hi == 3)
                return new Color(p, q, v, 255);
            else if (hi == 4)
                return new Color(t, p, v, 255);
            else
                return new Color(v, p, q, 255);
        }

        /// <summary>
        /// Convertes a Hex Value to a RGB Colour
        /// </summary>
        /// <param name="hexValue">a 6 character hex value</param>
        /// <returns></returns>
        public static Color HexToRGB(string hexValue)
        {
            if (hexValue.Length != 6)
            {
                vxConsole.WriteError(string.Format("Hex Value '{0}' is not 6 characters long", hexValue));
                return Color.Magenta;
            }
            var r = int.Parse(hexValue.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            var g = int.Parse(hexValue.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            var b = int.Parse(hexValue.Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier);

            return new Color(r, g, b);
        }


        /// <summary>
        /// Converts a Colour to a hex code
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string RGBToHex(Color color)
        {
            return color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }


    }
}
