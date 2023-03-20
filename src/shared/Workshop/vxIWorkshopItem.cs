using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.UI.Dialogs;


namespace VerticesEngine.Workshop
{
    public enum vxWorkshopItemType
    {
        SandboxFile,
        Mod
    }

    public interface vxIWorkshopItem 
    {
        string Id { get; }

        string PreviewImageURL { get; }

        Texture2D PreviewImage { get; set; }

        string InstallPath { get; }

        string Author { get; }

        string Title { get; }

        string Description { get; }

        ulong Size { get; }

        bool IsInstalled { get; }

        vxWorkshopItemType ItemType { get; }
    }
}
