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

    public static bool IsDivisibleBy(this float value, int divider)
    {
        return (value % divider == 0);
    }

    public static bool IsDivisibleBy(this int value, int divider)
    {
        if (divider != 0)
            return (value % divider == 0);
        else
            return true;
    }


    public static bool IsEven(this int i)
    {
        return (i % 2 == 0);
    }

    public static bool IsOdd(this int i)
    {
        return (i % 2 != 0);
    }
}