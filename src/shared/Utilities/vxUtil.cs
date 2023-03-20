using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.IO.Compression;
using Microsoft.Xna.Framework;
using VerticesEngine;

namespace VerticesEngine.Utilities
{
	/// <summary>
	/// Collection of static utility methods.
	/// </summary>
    public static class vxUtil
    {
        /// <summary>
        /// Returns the Next Value in an Enumeration, wrapping if it reaches the end.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentValue"></param>
        /// <returns></returns>
        public static T NextEnumValue<T>(T currentValue)
        {
            // not nice but simplifies a lot of code
            int nextValue = ((int)(object)currentValue + 1) % Enum.GetValues(typeof(T)).Length;
            return (T)(object)nextValue;
        }

        /// <summary>
        /// Returns the Previous Value in an Enumeration, wrapping if it reaches 0.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentValue"></param>
        /// <returns></returns>
        public static T PreviousEnumValue<T>(T currentValue)
        {
            // not nice but simplifies a lot of code
            int nextValue = ((int)(object)currentValue - 1);

            // if it's less than 1, then loop it back around to the top
            if (nextValue < 0)
                nextValue = Enum.GetValues(typeof(T)).Length - 1;

            return (T)(object)nextValue;
        }

        /// <summary>
        /// Get's all values for a specific enum type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }


        /// <summary>
        /// Returns a random Value in an Enumeration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentValue"></param>
        /// <returns></returns>
        public static T GetRandomEnumValue<T>(T currentValue, int Seed)
        {
            // not nice but simplifies a lot of code
            Random random = new Random(Seed * ((int)(object)currentValue));

            int nextValue = ((int)(object)random.Next(0, Enum.GetValues(typeof(T)).Length) % Enum.GetValues(typeof(T)).Length);
            return (T)(object)nextValue;
        }

  //      /// <summary>
  //      /// Wraps the multiline string. Useful for displaying Stack Traces which sometimes go off the screen.
  //      /// </summary>
  //      /// <returns>The wrapped multiline string.</returns>
  //      /// <param name="inputstring">Inputstring.</param>
  //      /// <param name="Width">Width.</param>
  //      [Obsolete("Use the Extention Method. This implementation is kept for compatibility but will be removed in a later release.")]
  //      public static string WrapMultilineString(string inputstring, int MaxCharsPerLine)
		//{
		//	string returnstring = "";
		//	using (StringReader reader = new StringReader(inputstring))
		//	{
		//		string line;
		//		while ((line = reader.ReadLine()) != null)
		//		{
		//			// Do something with the line
		//			toolongloop:
		//			//If the line is longer than the width of the string, then split it
		//			if (line.Length > MaxCharsPerLine) {
		//				returnstring += line.Substring (0, MaxCharsPerLine) + "\n";

		//				//Now set the line to the remainder and loop back up
		//				line = line.Substring (MaxCharsPerLine);
		//				goto toolongloop;
		//			} else {
		//				returnstring += line + "\n";
		//			}
		//		}
		//	}
		//	return returnstring;
  //      }

  //      [Obsolete("Use the Extention Method. This implementation is kept for compatibility but will be removed in a later release.")]
  //      public static string WrapMultilineString(string inputstring, SpriteFont Font, int width)
  //      {
  //          int CharacterWidth = (int)Font.MeasureString("A").X;

  //          int NumOfCharacters = width / CharacterWidth;

  //          return WrapMultilineString(inputstring, NumOfCharacters);

  //      }

  //      /// <summary>
  //      /// Gets the bordered rectangle.
  //      /// </summary>
  //      /// <returns>The border rectangle.</returns>
  //      /// <param name="rectangle">Rectangle.</param>
  //      /// <param name="borderSize">Border size.</param>
  //      [Obsolete("Use the Extention Method 'rect.GetBorder(BorderSize)'. This implementation is kept for compatibility but will be removed in a later release.")]
  //      public static Rectangle GetBorderRectangle(Rectangle rectangle, int borderSize)
		//{
		//	return new Rectangle(
		//		rectangle.X - borderSize,
		//		rectangle.Y - borderSize,
		//		rectangle.Width + borderSize * 2,
		//		rectangle.Height + borderSize * 2);
		//}


		static int spinner_index_i = 0;
		static int spinner_index_j = 0;
		static string spinner_text = "|";
		public static string GetTextSpinner()
		{
			spinner_index_i++;

			if (spinner_index_i % 10 == 0) {
				spinner_index_j++;

				switch(spinner_index_j%4)
				{
				case 0:
					spinner_text = "|";
					break;
				case 1:
					spinner_text = "/";
					break;
				case 2:
					spinner_text = "-";
					break;
				case 3:
					spinner_text = "\\";
					break;
				}
			}
			return spinner_text;
		}
    }
}
