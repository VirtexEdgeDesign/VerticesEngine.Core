using System;
using System.Collections.Generic;
using System.Text;
using VerticesEngine.UI;
using VerticesEngine;
using VerticesEngine.UI.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.UI.Themes;
using VerticesEngine.ContentManagement;
using VerticesEngine.UI.Events;

namespace VerticesEngine.Localization.UI
{
    /// <summary>
    /// A combo box which is used for displaying a drop down for language selection
    /// </summary>
    public class vxLocComboBox : vxComboBox
    {
        public vxLocComboBox(): base(vxLocalizer.CurrentLanguage, Vector2.Zero)
        {

        }

        protected override SpriteFont GetFont()
        {
            return vxUITheme.Fonts.Size20;
        }

        public override void AddItem(string isoCode)
        {
            var languageName = vxLocalizer.SupportedLangagues[isoCode];

            var item = new vxLocComboBoxItem(isoCode, languageName, Choices.Count, new Vector2((int)(Position.X), Bounds.Bottom + (Choices.Count) * LineHeight));

            item.Clicked += OnItemClicked;

            Choices.Add(item);

            if (this.Text == languageName)
                SelectedIndex = Choices.Count - 1;

            SetItemPositions();
        }

        protected override void OnItemClicked(object sender, vxUIControlClickEventArgs e)
        {
            base.OnItemClicked(sender, e);

            vxLocComboBoxItem item = (vxLocComboBoxItem)e.GUIitem;

            Font = item.LocFont;
        }

        public override void DrawText()
        {
            var th = Font.MeasureString(Text).Y;

            float bh = Bounds.Height;

            SpriteBatch.DrawString(Font, Text, TextPosition - Vector2.UnitY * 2, GetStateColour(Theme.Text) * Opacity, Vector2.One * (bh / th*1.025f));
        }
    }
}
