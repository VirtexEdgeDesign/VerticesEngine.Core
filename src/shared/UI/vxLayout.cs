using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace VerticesEngine.UI
{

    /// <summary>
    /// Alignment for layout.
    /// </summary>
    [Flags]
    public enum Alignment
    {
        None = 0,

        // Horizontal layouts
        Left = 1,
        Right = 2,
        HorizontalCenter = 4,

        // Vertical layouts
        Top = 8,
        Bottom = 16,
        VerticalCenter = 32,

        // Combinations
        TopLeft = Top | Left,
        TopRight = Top | Right,
        TopCenter = Top | HorizontalCenter,

        BottomLeft = Bottom | Left,
        BottomRight = Bottom | Right,
        BottomCenter = Bottom | HorizontalCenter,

        CenterLeft = VerticalCenter | Left,
        CenterRight = VerticalCenter | Right,
        Center = VerticalCenter | HorizontalCenter
    }

    /// <summary>
    /// Layout class that supports title safe area.
    /// </summary>
    /// <remarks>
    /// You have to support various resolutions when you develop multi-platform
    /// games. Also, you have to support title safe area for Xbox 360 games.
    /// 
    /// This structure places given rectangle with specified alignment and margin
    /// based on layout area (client area) with safe area.
    /// 
    /// Margin is percentage of client area size.
    /// 
    /// Example:
    /// 
    /// Place( region, 0.1f, 0.2f, Aligment.TopLeft );
    /// 
    /// Place region at 10% from left side of the client area,
    /// 20% from top of the client area.
    /// 
    /// 
    /// Place( region, 0.3f, 0.4f, Aligment.BottomRight );
    /// 
    /// Place region at 30% from right side of client,
    /// 40% from the bottom of the client area.
    /// 
    /// 
    /// You can individually specify client area and safe area.
    /// So, it is useful when you have split screen game which layout happens based
    /// on client and it takes care of the safe at same time.
    /// 
    /// </remarks>
    public class vxLayout
    {
        /// <summary>
        /// The ideal screen size used for UI Scaling.
        /// </summary>
        public static Point IdealScreenSize
        {
            get { return _idealScreenSize; }
            set { _idealScreenSize = value; }
        }
        private static Point _idealScreenSize = new Point(1280, 720);
        
        /// <summary>
        /// The screen size scaler that is the average of the x and y components.
        /// </summary>
        public static float ScaleAvg
        {
            get { return _scaleAvg; }
        }
        private static float _scaleAvg = 1;


        /// <summary>
        /// The screen size scaler for handling different Screen Sizes
        /// </summary>
        public static Vector2 Scale
        {
            get { return _scale; }
        }
        private static Vector2 _scale = new Vector2();

        public static Vector2 GetScaledSize(float x, float y)
        {
            return GetScaledSize(new Vector2(x, y));
        }



        public static Vector2 GetScaledSize(Vector2 vector2)
        {
            return vector2 * Scale;
        }



        /// <summary>
        /// Returns an integer scaled value based off of the screen size.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static int GetScaledSize(float i)
        {
            return (int)(i * ScaleAvg);
        }

        public static int GetScaledWidth(float i)
        {
            return (int)(i * Scale.X);
        }
        public static int GetScaledHeight(float i)
        {
            return (int)(i * Scale.Y);
        }



        public static void SetLayoutScale(Point screenSize, Point IdealScreenSize)
        {
            _scaleAvg = ((float)screenSize.X / IdealScreenSize.X +
           (float)screenSize.Y / IdealScreenSize.Y) / 2;

            // Calcualte the Per Direction Screen Size Scales
            _scale = new Vector2(
                (float)screenSize.X / IdealScreenSize.X,
                (float)screenSize.Y / IdealScreenSize.Y);
        }


        #region Fields

        /// <summary>
        /// Gets/Sets client area.
        /// </summary>
        public Rectangle ClientArea;

        /// <summary>
        /// Gets/Sets safe area.
        /// </summary>
        public Rectangle SafeArea;

        #endregion

        #region Initialization

        /// <summary>
        /// Construct layout object by specify both client area and safe area.
        /// </summary>
        /// <param name="client">Client area</param>
        /// <param name="safeArea">safe area</param>
        public vxLayout(Rectangle clientArea, Rectangle safeArea)
        {
            ClientArea = clientArea;
            SafeArea = safeArea;
        }

        /// <summary>
        /// Construct layout object by specify client area.
        /// Safe area becomes same size as client area.
        /// </summary>
        /// <param name="client">Client area</param>
        public vxLayout(Rectangle clientArea)
            : this(clientArea, clientArea)
        {
        }

        /// <summary>
        /// Construct layout object by specify viewport.
        /// Safe area becomes same as Viewpoert.TItleSafeArea.
        /// </summary>
        public vxLayout(Viewport viewport)
        {
            ClientArea = new Rectangle((int)viewport.X, (int)viewport.Y,
                                        (int)viewport.Width, (int)viewport.Height);
            SafeArea = viewport.TitleSafeArea;
        }

        #endregion


        public static Vector2 GetVector2(Rectangle rectangle, float padding = 0)
        {
            return GetVector2( rectangle, Vector2.One * padding);
        }


        public static Vector2 GetVector2(Rectangle rectangle, float paddingX, float paddingY)
        {
            return GetVector2(rectangle, new Vector2(paddingX, paddingY));
        }


        public static Vector2 GetVector2(Rectangle rectangle, Vector2 padding)
        {
            return rectangle.Location.ToVector2() + padding;
        }


        public static Vector2 GetVector2(Point point)
        {
            return point.ToVector2();
        }

        //public static Vector2 GetVector2(Rectangle sourceRect, Rectangle parentRect, Vector2 paddings, Alignment alignment)
        //{
            
        //}

        #region Rectangle Helper Functions

        public static Rectangle GetRect(float x, float y, float w, float h)
        {
            return new Rectangle((int)x, (int)y, (int)w, (int)h);
        }

        public static Rectangle GetRect(Point pos, float w, float h)
        {
            return new Rectangle(pos.X, pos.Y, (int)w, (int)h);
        }

        public static Rectangle GetRect(Vector2 pos, float w, float h)
        {
            return new Rectangle((int)pos.X, (int)pos.Y, (int)w, (int)h);
        }

        public static Rectangle GetRect(Vector2 pos, Vector2 size)
        {
            return new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
        }

        public static Rectangle GetRect(Vector2 pos, float sqSize)
        {
            return new Rectangle((int)pos.X, (int)pos.Y, (int)sqSize, (int)sqSize);
        }

        public static Rectangle GetRect(Point pos, float sqSize)
        {
            return new Rectangle((int)pos.X, (int)pos.Y, (int)sqSize, (int)sqSize);
        }

        #endregion

        /// <summary>
        /// Layouting specified region
        /// </summary>
        /// <param name="region">placing region</param>
        /// <returns>Placed position</returns>
        public Vector2 Place(Vector2 size, float horizontalMargin,
                                            float verticalMargine, Alignment alignment)
        {
            Rectangle rc = new Rectangle(0, 0, (int)size.X, (int)size.Y);
            rc = Place(rc, horizontalMargin, verticalMargine, alignment);
            return new Vector2(rc.X, rc.Y);
        }

        /// <summary>
        /// Layouting specified region
        /// </summary>
        /// <param name="region">placing rectangle</param>
        /// <returns>placed rectangle</returns>
        public Rectangle Place(Rectangle region, float horizontalMargin,
                                            float verticalMargine, Alignment alignment)
        {
            // Horizontal layout.
            if ((alignment & Alignment.Left) != 0)
            {
                region.X = ClientArea.X + (int)(ClientArea.Width * horizontalMargin);
            }
            else if ((alignment & Alignment.Right) != 0)
            {
                region.X = ClientArea.X +
                            (int)(ClientArea.Width * (1.0f - horizontalMargin)) -
                            region.Width;
            }
            else if ((alignment & Alignment.HorizontalCenter) != 0)
            {
                region.X = ClientArea.X + (ClientArea.Width - region.Width) / 2 +
                            (int)(horizontalMargin * ClientArea.Width);
            }
            else
            {
                // Don't do layout.
            }

            // Vertical layout.
            if ((alignment & Alignment.Top) != 0)
            {
                region.Y = ClientArea.Y + (int)(ClientArea.Height * verticalMargine);
            }
            else if ((alignment & Alignment.Bottom) != 0)
            {
                region.Y = ClientArea.Y +
                            (int)(ClientArea.Height * (1.0f - verticalMargine)) -
                            region.Height;
            }
            else if ((alignment & Alignment.VerticalCenter) != 0)
            {
                region.Y = ClientArea.Y + (ClientArea.Height - region.Height) / 2 +
                            (int)(verticalMargine * ClientArea.Height);
            }
            else
            {
                // Don't do layout.
            }

            // Make sure layout region is in the safe area.
            if (region.Left < SafeArea.Left)
                region.X = SafeArea.Left;

            if (region.Right > SafeArea.Right)
                region.X = SafeArea.Right - region.Width;

            if (region.Top < SafeArea.Top)
                region.Y = SafeArea.Top;

            if (region.Bottom > SafeArea.Bottom)
                region.Y = SafeArea.Bottom - region.Height;

            return region;
        }

    }
}
