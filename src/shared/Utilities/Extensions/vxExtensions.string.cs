using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VerticesEngine;
using VerticesEngine.Utilities;

public static partial class vxExtensions
{
    /// <summary>
    /// Splits the string by upper case.
    /// </summary>
    /// <returns>The by upper case.</returns>
    /// <param name="source">Source.</param>
    public static string SplitIntoSentance(this string source)
    {
        var words = System.Text.RegularExpressions.Regex.Split(source, @"(?<!^)(?=[A-Z])");

        string result = words[0];

        for (int i = 1; i < words.Length; i++)
            result += " " + words[i];

        return result;
    }

    /// <summary>
    /// Converts a string into sentance case
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string ToSentanceCase(this string source)
    {
        return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(source);
    }


    public static string WrapMultilineStringBlock(this string text, int MaxCharsPerLine)
    {
        string[] strings = WrapMultilineString(text, MaxCharsPerLine);

        string finalLine = "";
        foreach (string line in strings)
            finalLine += line + "\n";

        return finalLine;
    }
    /// <summary>
    /// Wraps a String based off of the given Font and Width.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="Font">the XNA/Monogame SpriteFont drawing this font.</param>
    /// <param name="Width">The Bounds or Width of where the text is being drawn.</param>
    /// <returns></returns>
    public static string[] WrapMultilineString(this string text, int MaxCharsPerLine)
    {
        // How many Lines are present
        int NumOfLines = (int)Math.Floor((double)(text.Length-1) / MaxCharsPerLine) + 1;
        
        // The string to return
        List<string> multiLineString = new List<string>();

        for (int i = 0; i < NumOfLines; i++)
        {
            string line = text.Substring(i * MaxCharsPerLine) ;

            if(line.Length > MaxCharsPerLine)
                line = text.Substring(i * MaxCharsPerLine, MaxCharsPerLine);

            multiLineString.Add(line);
        }
        return multiLineString.ToArray();
    }


    /// <summary>
    /// Takes in a string and parses based off of the 'XML Tag'. If Tag is not found, No text is returned.
    /// </summary>
    /// <param name="Text">Text to be Parsed</param>
    /// <param name="XMLTag">XML Tag to Parse By</param>
    /// <returns>Return Text parsed by XML Tags</returns>
    public static string ReadXML(this string Text, string XMLTag)
    {
        string value = "";

        try
        {
            //Start and End tags of XML Tag
            string StartTag = "<" + XMLTag + ">";
            string EndTag = "</" + XMLTag + ">";

            value = Text.Substring(Text.IndexOf(StartTag) + StartTag.Length, Text.IndexOf(EndTag) - Text.IndexOf(StartTag) - EndTag.Length + 1);
        }
        catch
        {
            value = "ERROR PARSING XML TAG!";
        }
        return value;
    }

    /// <summary>
    /// Combines Text and XML Tag into String XML Tag line
    /// </summary>
    /// <param name="Text">Text to Combine into XML Tag</param>
    /// <param name="XMLTag">XML Tag to use</param>
    /// <returns>String that is XML Tagged</returns>
    public static string WriteXML(this string Text, string XMLTag)
    {
        string value = "";

        //Start and End tags of XML Tag
        string StartTag = "<" + XMLTag + ">";
        string EndTag = "</" + XMLTag + ">";

        value = StartTag + Text + EndTag;

        return value;
    }



    /// <summary>
    /// Parses this string as a File URL and returns the File Name without the Extention
    /// </summary>
    /// <param name="FilePath"></param>
    /// <returns></returns>
    public static string GetFileNameFromPath(this string FilePath)
    {
        char c = '/';

        // First Check if it has an extention

        if (FilePath != null)
        {
            if (FilePath.Contains('.'))
            {
                return FilePath.Substring(FilePath.LastIndexOf(c) + 1,
                        FilePath.LastIndexOf('.') - FilePath.LastIndexOf(c) - 1);
            }
            else
            {
                return FilePath.Substring(FilePath.LastIndexOf(c) + 1);
            }
        }
        return "null";
    }

    /// <summary>
    /// Parses this string as a File Path and returns the Path Portion
    /// </summary>
    /// <param name="FilePath"></param>
    /// <returns></returns>
    public static string GetParentPathFromFilePath(this string FilePath)
    {
        char c = '/';

        if (FilePath.Contains(c) == false)
            c = '\\';

        if (FilePath.Contains(c) == false)
            return FilePath;

        return FilePath.Substring(0, FilePath.LastIndexOf(c));
    }


}