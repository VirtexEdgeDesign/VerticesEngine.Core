using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Utilities;
using VerticesEngine;
using VerticesEngine.UI.Dialogs;
using VerticesEngine.UI.Themes;
using VerticesEngine.Graphics;

namespace VerticesEngine.UI.Controls
{
    /// <summary>
    ///  <see cref="VerticesEngine.UI.Controls.vxSettingsGUIItem"/> control which allows for spinning through options
    /// </summary>
	public class vxSettingsReadOnlyGUIItem : vxSettingsGUIItem 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxSettingsComboBoxGUIItem"/> class.
        /// </summary>
        /// <param name="Engine">Engine.</param>
        /// <param name="UIManager">GUIM anager.</param>
        /// <param name="Title">Title.</param>
        /// <param name="Value">Value.</param>
        public vxSettingsReadOnlyGUIItem(vxUIManager UIManager, string Name, string InitValue) :
            base(UIManager, Name, InitValue)
        {

        }

        protected override void UpdateIncrementButtons()
        {
            //base.UpdateIncrementButtons();
        }

        protected override void DrawIncrementButtons()
        {
            var valueFont = vxUITheme.Fonts.Size16;

            var valueTextSize = valueFont.MeasureString(Value) * vxLayout.ScaleAvg;

            var valuePos = new Vector2(Bounds.Right - Padding.X - valueTextSize.X, Position.Y + Height / 2 - valueTextSize.Y/2);

            SpriteBatch.DrawString(valueFont, Value, valuePos, Color.White, vxLayout.ScaleAvg);
        }
    }
}
