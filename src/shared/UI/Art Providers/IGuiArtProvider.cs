using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VerticesEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VerticesEngine.UI
{
    /// <summary>
    /// The GUI Art provider acts as the "Renderer" for GUI Elements. This provides an easy way to override
    /// graphical style for a given GUI Element.
    /// </summary>
    [System.Obsolete("You should upgrade this Art Provider to use vxUIArtProvider<T>")]
    public interface IGuiArtProvider : ICloneable
    {
        /// <summary>
        /// The object past through can be anything. Often times what you're wanting to draw.
        /// </summary>
        /// <param name="guiItem"></param>
        void Draw(object guiItem);

        /// <summary>
        /// Draws the text.
        /// </summary>
        /// <param name="guiItem">GUI item.</param>
        //void DrawText(object guiItem);
    }
}
