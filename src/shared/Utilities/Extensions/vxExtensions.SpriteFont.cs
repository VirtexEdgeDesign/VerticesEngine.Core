using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VerticesEngine;
using VerticesEngine.Utilities;

public  static partial class vxExtensions
{
    //public static string WrapMultilineStringBlock(this SpriteFont Font, string text, int Width)
    //{
    //    // Get the Character Width
    //    int CharWidth = (int)Font.MeasureString("A").X;

    //    // Get the number of Characters Per Line.
    //    int MaxCharsPerLine = Width / CharWidth;

    //    // How many Lines are present
    //    return text.WrapMultilineStringBlock(MaxCharsPerLine);
    //}

    /// <summary>
    /// Returns the portion of the string within the width.
    /// </summary>
    /// <returns>The string.</returns>
    /// <param name="Font">Font.</param>
    /// <param name="text">This GUI Items Text.</param>
    /// <param name="width">Width.</param>
	public static string GetClampedString(this SpriteFont Font, string text, int width)
	{
		for (int i = 0; i < text.Length; i++)
		{
			// The Test String
			string testString = text.Substring(0, i);

			//Now get the String Lengnth
			float stringLength = Font.MeasureString(testString).X;

			// if it's greater then the width, then break out of the loop
			if (stringLength > width)
				return text.Substring(0, i);
		}

		// default is to return the original text.
		return text;
	}

    /// <summary>
    /// Wraps the string.
    /// </summary>
    /// <returns>The string.</returns>
    /// <param name="Font">Font.</param>
    /// <param name="text">This GUI Items Text.</param>
    /// <param name="Width">Width.</param>
    public static string WrapString(this SpriteFont Font, string text, int Width, float scale = 1)
    {
		string[] lines = Font.WrapStringToArray(text, Width);

        string finalString = "";

        // now add the strings together
        for (int i = 0; i < lines.Length; i++)
            finalString += ((i == 0) ?  "" : "\n") + lines[i];

        // return the result
        return finalString;
    }

    /// <summary>
    /// Wraps a String based off of the given Font and Width and returns it as an array of strings.
    /// </summary>
    /// <param name="Font">the XNA/Monogame SpriteFont drawing this font.</param>
    /// <param name="text">The Text to Wrap.</param>
    /// <param name="Width">The Bounds or Width of where the text is being drawn.</param>
    /// <returns></returns>
    public static string[] WrapStringToArray(this SpriteFont Font, string text, int Width, float scale = 1)
    {
        // first get a collection of all words in the line.
        //string[] words = System.Text.RegularExpressions.Regex.Split(text, @"\W|_");
        string[] words = text.Split(' ');

        // now put the words together until the overall length is longer
        // then the text box
        List<string> Lines = new List<string>();

        // get the first word
        string currentLine = words[0];

        for (int i = 1; i < words.Length; i++)
        {
            string word = words[i];

            // Get the new length
            float lineLength = Font.MeasureString(currentLine +" "+ word).X;

            // If the new line is too long, then create a new line, if not
            // add the word to the line.

            if(lineLength > Width)
            {
                Lines.Add(currentLine);
                //Lines.Add(new string(currentLine.ToArray()));
                currentLine = word;
            }
            else
            {
                currentLine += " " + word;
            }

        }

        // Add what ever is left
        Lines.Add(currentLine);
        //Lines.Add(new string(currentLine.ToArray()));

        // finally return the array.
        return Lines.ToArray();
    }

    /// <summary>
    /// A function which searches through a set of lines to see which line the cursor is on. X is the
    /// location on that line, and Y is which line it's on.
    /// </summary>
    /// <param name="Font"></param>
    /// <param name="text"></param>
    /// <param name="Width"></param>
    /// <param name="CursorIndex"></param>
    /// <returns></returns>
    public static Point GetCursorLocation(this SpriteFont Font, string text, int Width, int CursorIndex)
    {
        // Get the Character Width
        //int CharWidth = (int)Font.MeasureString("A").X;

        //// Get the number of Characters Per Line.
        //int MaxCharsPerLine = Width / CharWidth;

        //// How many Lines are present
        //int NumOfLines = (int)Math.Floor((double)(text.Length - 1) / MaxCharsPerLine) + 1;

        string[] lines = WrapStringToArray(Font, text, Width);


        // The string to return
        Point location = new Point(CursorIndex, 0);

        int RunningCharacterCount = 0;
        
        for (int i = 0; i < lines.Count(); i++)
        {
            RunningCharacterCount += lines[i].Length + 1;

            if (CursorIndex <= RunningCharacterCount)
            {
                // The cursor location is the current index minus all previous lines
                location = new Point(CursorIndex - (RunningCharacterCount - lines[i].Length) + 1, i);
                break;
            }
        }
        //Console.WriteLine("Cursor Pos: {0}", location);

        return location;
    }
}