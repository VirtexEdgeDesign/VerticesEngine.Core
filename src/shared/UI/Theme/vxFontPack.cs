using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine;

namespace VerticesEngine.UI
{
    /// <summary>
    /// This loads and holds a Font Pack for a number of different font sizes.
    /// It wil hold sizes of 12, 16, 20, 24 and 36. You must have your fonts in the path you
    /// sent to the constructor labeled as 'font_12' 'font_16' etc...
    /// </summary>
    public class vxFontPack
    {
        public SpriteFont Size08;
        public SpriteFont Size10;
        public SpriteFont Size12;
        public SpriteFont Size16;
        public SpriteFont Size20;
        public SpriteFont Size24;
        public SpriteFont Size36;
        public SpriteFont Size48;
        public SpriteFont Size64;
        public SpriteFont Size72;
        public SpriteFont Size96;
        public SpriteFont Size108;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.vxFont"/> class.
        /// </summary>
        /// <param name="path">The path to the folder containing the font packs. Files must be written as font_en_12.xnb, font_en_16.xnb etc... from 8 to 64</param>
        /// <param name="loc">The localization key. The default is "en"</param>
        public vxFontPack(string path, string loc = "en")
        {
            Size08 = vxEngine.Game.Content.Load<SpriteFont>(Path.Combine(path, loc, $"font_{loc}_8"));
            Size10 = vxEngine.Game.Content.Load<SpriteFont>(Path.Combine(path, loc, $"font_{loc}_10"));
            Size12 = vxEngine.Game.Content.Load<SpriteFont>(Path.Combine(path, loc, $"font_{loc}_12"));
            Size16 = vxEngine.Game.Content.Load<SpriteFont>(Path.Combine(path, loc, $"font_{loc}_16"));
            Size20 = vxEngine.Game.Content.Load<SpriteFont>(Path.Combine(path, loc, $"font_{loc}_20"));
            Size24 = vxEngine.Game.Content.Load<SpriteFont>(Path.Combine(path, loc, $"font_{loc}_24"));
            Size36 = vxEngine.Game.Content.Load<SpriteFont>(Path.Combine(path, loc, $"font_{loc}_36"));
            Size48 = vxEngine.Game.Content.Load<SpriteFont>(Path.Combine(path, loc, $"font_{loc}_48"));
            Size64 = vxEngine.Game.Content.Load<SpriteFont>(Path.Combine(path, loc, $"font_{loc}_64"));
            Size72 = vxEngine.Game.Content.Load<SpriteFont>(Path.Combine(path, loc, $"font_{loc}_72"));
            Size96 = vxEngine.Game.Content.Load<SpriteFont>(Path.Combine(path, loc, $"font_{loc}_96"));
            Size108 = vxEngine.Game.Content.Load<SpriteFont>(Path.Combine(path, loc, $"font_{loc}_108"));
        }
    }
}
