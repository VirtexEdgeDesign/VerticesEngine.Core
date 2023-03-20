using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine;
using VerticesEngine.UI.Themes;

namespace VerticesEngine.UI.Controls
{
    public class vxRibbonToolbarLogoButtonControl : vxRibbonToolbarButtonControl
    {
        public vxRibbonToolbarLogoButtonControl(vxRibbonControl Ribbon, Texture2D Texture)
            : base(Ribbon, Texture)
        {

            Width = 48;
            Height = 48;
        }
    }
}
   