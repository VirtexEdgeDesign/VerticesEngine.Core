using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VerticesEngine;
using VerticesEngine.UI;
using VerticesEngine.Utilities;

public static partial class vxExtensions
{
    public static bool IsNan(this Vector3 vector)
    {
        return (float.IsNaN(vector.X) || float.IsNaN(vector.Y) || float.IsNaN(vector.Z));
    }
    public static bool IsNan(this Quaternion quaternion)
    {
        return (float.IsNaN(quaternion.X) || float.IsNaN(quaternion.Y) || float.IsNaN(quaternion.Z) || float.IsNaN(quaternion.W));
    }

    /// <summary>
    /// Adds two different Points from each other.
    /// </summary>
    /// <param name="vectr"></param>
    /// <param name="pnt1"></param>
    /// <returns></returns>
    public static Point Add(this Point vectr, Point pnt1)
    {
        return new Point(pnt1.X + vectr.X, pnt1.Y + vectr.Y);
    }

    /// <summary>
    /// Adds two different Points from each other.
    /// </summary>
    /// <param name="vectr"></param>
    /// <param name="pnt1"></param>
    /// <param name="pnt2"></param>
    /// <returns></returns>
    public static Point Add(this Point vectr, Point pnt1, Point pnt2)
    {
        return new Point(pnt1.X + pnt2.X, pnt1.Y + pnt2.Y);
    }

    /// <summary>
    /// Subtracts two different Points from each other.
    /// </summary>
    /// <param name="vectr"></param>
    /// <param name="pnt1"></param>
    /// <returns></returns>
    public static Point Subtract(this Point vectr, Point pnt1)
    {
        return new Point(vectr.X - pnt1.X, vectr.Y - pnt1.Y);
    }

    /// <summary>
    /// Subtracts two different Points from each other.
    /// </summary>
    /// <param name="vectr"></param>
    /// <param name="pnt1"></param>
    /// <param name="pnt2"></param>
    /// <returns></returns>
    public static Point Subtract(this Point vectr, Point pnt1, Point pnt2)
    {
        return new Point(pnt1.X - pnt2.X, pnt1.Y - pnt2.Y);
    }


    /// <summary>
    /// Extension method which converts a Vector2 to a Point;
    /// </summary>
    /// <param name="vector">The Vector2 to Convert</param>
    /// <returns>The Vector as a Point.</returns>
    public static Point ToPoint(this Vector2 vector)
    {
        return new Point((int)vector.X, (int)vector.Y);
    }


    /// <summary>
    /// Extension method which converts a Vector2 to a Point;
    /// </summary>
    /// <param name="point">The Point to Convert</param>
    /// <returns>The Point as a Vector2.</returns>
    public static Vector2 ToVector2(this Point point)
    {
        return new Vector2(point.X, point.Y);
    }

    /// <summary>
    /// Resizes a vector2 based off of the vxLayout.Scale value
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public static Vector2 ToLayoutScale(this Vector2 point)
    {
        return point * vxLayout.Scale;
    }

    /// <summary>
    /// Converts the Vector2 X and Y components to integer values (i.e. whole numbers).
    /// </summary>
    /// <returns>The int value.</returns>
    /// <param name="point">Point.</param>
    public static Vector2 ToIntValue(this Vector2 point)
    {
        return new Vector2((int)point.X, (int)point.Y);
    }


    /// <summary>
    /// Converts the Vector3 X, Y & Z components to integer values (i.e. whole numbers).
    /// </summary>
    /// <returns>The int value.</returns>
    /// <param name="point">Point.</param>
    public static Vector3 ToIntValue(this Vector3 point)
    {
        return new Vector3((int)point.X, (int)point.Y, (int)point.Z);
    }

    /// <summary>
    /// Converts a Vector3 to a Vector2 by taking the X and Z coordinates as X and Y coordinates.
    /// This is often helpful in Height Maps.
    /// </summary>
    /// <param name="vector3"></param>
    /// <returns></returns>
    public static Vector2 ToVector2(this Vector3 vector3)
    {
        return new Vector2(vector3.X, vector3.Z);
    }

    public static Color ToColor(this Vector3 vector3)
    {
        return new Color(vector3);
    }

    /// <summary>
    /// Converts a Vector4 To a Colour.
    /// </summary>
    /// <param name="vector4"></param>
    /// <returns></returns>
    public static Color ToColor(this Vector4 vector4)
    {
        return new Color(vector4);
    }

    /// <summary>
    /// Rotates a Vector
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="radians"></param>
    /// <returns></returns>
    public static Vector2 Rotate(this Vector2 vector, float radians)
    {
        return Vector2.Transform(vector, Matrix.CreateRotationZ(radians));
    }


    /// <summary>
    /// Gets the rotated vector.
    /// </summary>
    /// <returns>The rotated vector.</returns>
    /// <param name="rotation">Rotation.</param>
    public static Vector2 GetRotatedVector(this Vector2 vector, float rotation)
    {
        return new Vector2(vector.X * (float)Math.Cos(rotation), vector.Y * (float)Math.Sin(rotation));
    }

    /// <summary>
    /// Projects the position to screen scape and returns a Vector 2.
    /// </summary>
    /// <returns>The to screen position.</returns>
    /// <param name="graphicsDevice">Graphics device.</param>
    /// <param name="position">Position.</param>
    /// <param name="projection">Projection.</param>
    /// <param name="view">View.</param>
    public static Vector2 ProjectToScreenPosition(this GraphicsDevice graphicsDevice,
                                                  Vector3 position, Matrix projection, Matrix view)

    {
        Vector3 pnt = graphicsDevice.Viewport.Project(position,
                                                      projection,
                                                      view,
                                           Matrix.Identity);

        return new Vector2(pnt.X, pnt.Y);
    }
}