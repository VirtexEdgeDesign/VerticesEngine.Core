using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Plugins;

namespace VerticesEngine
{
    /// <summary>
    /// This is a Entity Sprite Sheet Definition, which holds sprite retreival info for a speifici type. The sprite sheet used is the
    /// content pack's main sprite sheet.
    /// </summary>
    public class vxEntitySpriteSheetDefinition
    {
        public string ItemType { get; private set; }

        public Point Location { get; private set; }

        public Point Size { get; private set; }

        public Rectangle Source { get; private set; }

        public Texture2D SpriteSheet { get { return plugin.MainSpriteSheet; } }

        public Rectangle IconSource { get; set; }

        public float IconScale = 1;

        vxIPlugin plugin;

        public vxEntitySpriteSheetDefinition(string ItemType, vxIPlugin plugin, Rectangle Source):
            this(ItemType, plugin, Source, Source)
        {

        }

        public vxEntitySpriteSheetDefinition(string ItemType, vxIPlugin plugin, Rectangle Source, Rectangle IconSource)
        {
            this.ItemType = ItemType;
            this.Location = Source.Location;
            this.Size = Source.Size;

            this.Source = Source;


            this.IconSource = IconSource;
            this.plugin = plugin;
        }
    }
}
