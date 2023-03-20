using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VerticesEngine;
using VerticesEngine.UI;
using VerticesEngine.Utilities;

public  static partial class vxExtensions
{
    public static void DrawString(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, float scale,
        vxHorizontalJustification horizontalJustification = vxHorizontalJustification.Left, vxVerticalJustification verticalJustification = vxVerticalJustification.Top,
                                  float rotation = 0)
    {
        var origin = Vector2.Zero;

        // If its centered, then set the origin
        if (horizontalJustification == vxHorizontalJustification.Center)
        {
            origin = new Vector2(font.MeasureString(text).X / 2, origin.Y);
        }

        if (verticalJustification == vxVerticalJustification.Middle)
        {
            origin = new Vector2(origin.X, font.MeasureString(text).Y / 2);
        }


        spriteBatch.DrawString(font, text, position, color, rotation, origin, scale, SpriteEffects.None, 1);
    }

    public static void DrawString(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, float scale, Vector2 origin)
    {
        spriteBatch.DrawString(font, text, position, color, 0, origin, scale, SpriteEffects.None, 1);
    }

    public static void DrawString(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, Vector2 scale, Vector2 origin)
    {
        spriteBatch.DrawString(font, text, position, color, 0, origin, scale, SpriteEffects.None, 1);
    }

    public static void DrawString(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, Vector2 scale)
    {
        spriteBatch.DrawString(font, text, position, color, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
    }
}