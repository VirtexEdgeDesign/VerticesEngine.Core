using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Utilities;
using VerticesEngine;
using VerticesEngine.UI.Events;
using VerticesEngine.UI.Themes;
using VerticesEngine.UI.Controls;
using System.Collections.Generic;
using VerticesEngine.Graphics;
using VerticesEngine.ContentManagement;

namespace VerticesEngine.UI.Dialogs
{
    public class vxFileExplorerURLBarSplitter : vxFileExplorerURLBarButton
    {
        public vxFileExplorerURLBarSplitter(vxFileExplorerURLBar ControlBar, Vector2 position) :
        base(ControlBar, "/", position, "")
        {

            Theme = new vxUIControlTheme(
                new vxColourTheme(Color.Gray * 0.25f),
                new vxColourTheme(Color.White));
        }
    }
}
