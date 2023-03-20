using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VerticesEngine;
using VerticesEngine.Utilities;
using VerticesEngine.Graphics;

public static partial class vxExtensions
{
	/// <summary>
	/// Returns a new Rectangle that has been grown by the 'BorderSize' amount.
	/// </summary>
	/// <param name="rectangle"></param>
	/// <param name="BorderSize">The Border Size.</param>
	/// <returns></returns>
	public static Rectangle GetBorder(this Rectangle rectangle, int BorderSize)
	{
		return new Rectangle(
		rectangle.X - BorderSize,
		rectangle.Y - BorderSize,
		rectangle.Width + BorderSize * 2,
		rectangle.Height + BorderSize * 2);
	}

    /// <summary>
    /// Returns a new Rectangle that has been grown by the 'BorderSize' amount.
    /// </summary>
    /// <param name="rectangle"></param>
    /// <param name="BorderSize">The Border Size.</param>
    /// <returns></returns>
    public static Rectangle GetBorder(this Rectangle rectangle, int BorderSizeX, int BorderSizeY)
    {
        return new Rectangle(
        rectangle.X - BorderSizeX,
        rectangle.Y - BorderSizeY,
        rectangle.Width + BorderSizeX * 2,
        rectangle.Height + BorderSizeY * 2);
    }

    /// <summary>
    /// Returns a new Rectangle that has been grown by the 'BorderSize' amount.
    /// </summary>
    /// <returns>The border.</returns>
    /// <param name="rectangle">Rectangle.</param>
    /// <param name="BorderSize">Border size.</param>
    public static Rectangle GetBorder(this Rectangle rectangle, Point BorderSize)
    {
        return new Rectangle(
        rectangle.X - BorderSize.X,
        rectangle.Y - BorderSize.Y,
        rectangle.Width + BorderSize.X * 2,
        rectangle.Height + BorderSize.Y * 2);
    }


    /// <summary>
    /// Returns a new Rectangle that has been grown by the 'BorderSize' amount.
    /// </summary>
    /// <returns>The border.</returns>
    /// <param name="rectangle">Rectangle.</param>
    /// <param name="BorderSize">Border size.</param>
    public static Rectangle GetBorder(this Rectangle rectangle, Vector2 BorderSize)
    {
        return GetBorder(rectangle, BorderSize.ToPoint());
    }

    /// <summary>
    /// Gets the padded rectangle.
    /// </summary>
    /// <returns>The padded rectangle.</returns>
    /// <param name="rectangle">Rectangle.</param>
    /// <param name="Padding">Padding.</param>
    public static Rectangle GetPaddedRectangle(this Rectangle rectangle, Vector2 Padding)
    {
        return new Rectangle(
            (int)(rectangle.X - Padding.X),
            (int)(rectangle.Y - Padding.Y),
            (int)(rectangle.Width + 2 * Padding.X),
            (int)(rectangle.Height + 2 * Padding.Y));
    }

    /// <summary>
    /// Gets the padded rectangle.
    /// </summary>
    /// <returns>The padded rectangle.</returns>
    /// <param name="rectangle">Rectangle.</param>
    /// <param name="Padding">Padding.</param>
    public static Rectangle GetPaddedRectangle(this Rectangle rectangle, Point Padding)
    {
        return new Rectangle(
            (rectangle.X - Padding.X),
            (rectangle.Y - Padding.Y),
            (rectangle.Width + 2 * Padding.X),
            (rectangle.Height + 2 * Padding.Y));
    }


	/// <summary>
	/// Returns the location of in the blur map
	/// </summary>
	/// <param name="rectangle"></param>
	/// <param name="BorderSize"></param>
	/// <returns></returns>
    public static Rectangle GetBlurRectangle(this Rectangle rectangle, vxRenderPipeline Renderer)
	{

        int factor = vxGraphics.GraphicsDevice.Viewport.Width / rectangle.Width;
        Rectangle rect = new Rectangle(rectangle.X / factor, rectangle.Y / factor, rectangle.Width / factor, rectangle.Height / factor);

        return rect;
    }


	/// <summary>
	/// Centers a specified Inner rectangle within an Outter rectangle.
	/// </summary>
	/// <returns>The center.</returns>
	/// <param name="Inner">The Inner Rectangle to center.</param>
	/// <param name="Outter">The Outter Rectangle.</param>
	/// <param name="Scale">The Scale to be applied to the inner rectangle when scaling.</param>
	public static Rectangle Center(this Rectangle Inner, Rectangle Outter, float Scale=1)
	{
        return new Rectangle(
            Outter.X + Outter.Width / 2 - (int)(Inner.Width * Scale / 2 ),
            Outter.Y + Outter.Height / 2 - (int)(Inner.Height * Scale / 2),
            (int)(Inner.Width * Scale),
            (int)(Inner.Height * Scale));
	
	}

    /// <summary>
    /// Gets the offset rectangle.
    /// </summary>
    /// <returns>The offset.</returns>
    /// <param name="rectangle">Rectangle.</param>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
	public static Rectangle GetOffset(this Rectangle rectangle, int x, int y)
	{
		return new Rectangle(
            rectangle.X + x,
			rectangle.Y + y,
			rectangle.Width,
			rectangle.Height);

	}

	/// <summary>
	/// Gets the offset rectangle.
	/// </summary>
	/// <returns>The offset.</returns>
	/// <param name="rectangle">Rectangle.</param>
	/// <param name="Offset">Offset.</param>
	public static Rectangle GetOffset(this Rectangle rectangle, Point Offset)
    {
        return new Rectangle(
            rectangle.X + Offset.X,
            rectangle.Y + Offset.Y,
            rectangle.Width,
            rectangle.Height);

    }

    /// <summary>
    /// Gets the offset rectangle.
    /// </summary>
    /// <returns>The offset.</returns>
    /// <param name="rectangle">Rectangle.</param>
    /// <param name="Offset">Offset.</param>
    public static Rectangle GetOffset(this Rectangle rectangle, Vector2 Offset)
    {
        return GetOffset(rectangle, Offset.ToPoint());

    }

    /// <summary>
    /// Gets the offset rectangle.
    /// </summary>
    /// <returns>The offset.</returns>
    /// <param name="rectangle">Rectangle.</param>
    /// <param name="xy">Xy coordinate offset.</param>
    public static Rectangle GetOffset(this Rectangle rectangle, int xy)
	{
		return new Rectangle(
			rectangle.X + xy,
			rectangle.Y + xy,
			rectangle.Width,
			rectangle.Height);

	}

    /// <summary>
    /// Calculates the signed depth of intersection between two rectangles.
    /// </summary>
    /// <returns>
    /// The amount of overlap between two intersecting rectangles. These
    /// depth values can be negative depending on which wides the rectangles
    /// intersect. This allows callers to determine the correct direction
    /// to push objects in order to resolve collisions.
    /// If the rectangles are not intersecting, Vector2.Zero is returned.
    /// </returns>
    public static Vector2 GetIntersectionDepth(this Rectangle rectA, Rectangle rectB)
    {
        // Calculate half sizes.
        float halfWidthA = rectA.Width / 2.0f;
        float halfHeightA = rectA.Height / 2.0f;
        float halfWidthB = rectB.Width / 2.0f;
        float halfHeightB = rectB.Height / 2.0f;

        // Calculate centers.
        Vector2 centerA = new Vector2(rectA.Left + halfWidthA, rectA.Top + halfHeightA);
        Vector2 centerB = new Vector2(rectB.Left + halfWidthB, rectB.Top + halfHeightB);

        // Calculate current and minimum-non-intersecting distances between centers.
        float distanceX = centerA.X - centerB.X;
        float distanceY = centerA.Y - centerB.Y;
        float minDistanceX = halfWidthA + halfWidthB;
        float minDistanceY = halfHeightA + halfHeightB;

        // If we are not intersecting at all, return (0, 0).
        if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
            return Vector2.Zero;

        // Calculate and return intersection depths.
        float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
        float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
        return new Vector2(depthX, depthY);
    }

    /// <summary>
    /// Gets the position of the center of the bottom edge of the rectangle.
    /// </summary>
    public static Vector2 GetBottomCenter(this Rectangle rect)
    {
        return new Vector2(rect.X + rect.Width / 2.0f, rect.Bottom);
    }
}