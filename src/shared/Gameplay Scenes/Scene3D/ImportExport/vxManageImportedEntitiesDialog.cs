using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VerticesEngine;
using VerticesEngine.Input.Events;
using VerticesEngine.UI;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Dialogs;
using VerticesEngine.UI.Events;
using VerticesEngine.Utilities;
using VerticesEngine.Graphics;

namespace VerticesEngine.UI
{

    public class vxManageImportedEntitiesDialog : vxDialogBase
    {
        internal vxGameplayScene3D Level;

        public vxManageImportedEntitiesDialog(vxGameplayScene3D Level) : base("Manage Imported Entities", vxEnumButtonTypes.Ok)
        {
            this.Level = Level;
        }

        public override Vector2 GetBoundarySize()
        {
            return new Vector2(vxScreen.Viewport.Width, vxScreen.Viewport.Height);
        }


        public override void LoadContent()
        {
            base.LoadContent();

            Vector2 Padding = new Vector2(20);
            Vector2 LeftStart = ArtProvider.GUIBounds.Location.ToVector2() + Padding;

            //vxLabel explanationLabel = new vxLabel()

            Vector2 scrollPanelPos = ArtProvider.GUIBounds.Location.ToVector2() + new Vector2(0, 24);

            vxScrollPanel importedEntitiesScrollPanel = new vxScrollPanel(scrollPanelPos, ArtProvider.GUIBounds.Width, ArtProvider.GUIBounds.Height - 128);

            InternalGUIManager.Add(importedEntitiesScrollPanel);

            //InternalGUIManager.Add(new vxLabel("Track Settings", scrollPanelPos));

            foreach(var importedEntity in Level.importedFiles)
            {
                importedEntitiesScrollPanel.AddItem(new vxManageImportedEntityUIItem(this, importedEntity.Value));
            }
        }
    }
}