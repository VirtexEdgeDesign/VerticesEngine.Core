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
    /// <summary>
    /// Gets the Minute Values of the Time Span as a string.
    /// </summary>
    /// <param name="timespan"></param>
    /// <returns></returns>
    public static string GetMinuteTimespanString(this TimeSpan timespan)
    {
        string min;
        string secs;

        if (timespan.Minutes < 10)
            min = "0" + timespan.Minutes.ToString();
        else
            min = timespan.Minutes.ToString();

        if (timespan.Seconds < 10)
            secs = "0" + timespan.Seconds.ToString();
        else
            secs = timespan.Seconds.ToString();

        return min + ":" + secs + ":" + timespan.Milliseconds.ToString();
    }

    /// <summary>
    /// Returns a String of a Shortened Time Span
    /// </summary>
    /// <param name="timespan"></param>
    /// <returns></returns>
    public static string GetShortenedTimespanString(this TimeSpan timespan)
    {
        string time = timespan.Duration().ToString();

        int length = time.Length;

        if (time.LastIndexOf('.') != -1)
            length = time.LastIndexOf('.') + 4;

        return timespan.Duration().ToString().Substring(0, length);
    }
}