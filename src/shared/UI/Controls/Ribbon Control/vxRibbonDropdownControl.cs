using Microsoft.Xna.Framework;

namespace VerticesEngine.UI.Controls
{
    /// <summary>
    /// Label Class providing simple one line text as a vxGUI Item.
    /// </summary>
    public class vxRibbonDropdownControl : vxPanel
    {
        public vxComboBox Dropdown;

        public vxRibbonLabelControl label;
        /// <summary>
        /// Initializes a new instance of the <see cref="VerticesEngine.UI.Controls.vxLabel"/> class.
        /// </summary>
        /// <param name="Engine">The Vertices Engine Reference.</param>
        /// <param name="text">This GUI Items Text.</param>
        /// <param name="position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
        public vxRibbonDropdownControl(vxRibbonControlGroup RibbonControlGroup, string text, string Value) : base(Vector2.Zero, 164, 24)
        {
            UIManager = RibbonControlGroup.UIManager;

            label = new vxRibbonLabelControl(text);
            label.UIManager = UIManager;
            label.Font = vxInternalAssets.Fonts.ViewerFont;

            Dropdown = new vxComboBox(Value, Vector2.Zero);
            Dropdown.ItemPadding = 3;
            Dropdown.UIManager = UIManager;
            Dropdown.TextJustification = vxEnumTextHorizontalJustification.Left;

            Dropdown.Font = vxInternalAssets.Fonts.ViewerFont;
            Dropdown.Width = 100;
            Dropdown.Height = 20;
            Dropdown.Theme.Background = new vxColourTheme(
                Color.Transparent,
                RibbonControlGroup.RibbonTabPage.RibbonControl.ForegroundColour,
                Color.DarkOrange);
            Dropdown.Theme.Text = new vxColourTheme(
                Color.Black,
                Color.Black,
                Color.Black);
            Dropdown.Theme.Border = new vxColourTheme(
                Color.Transparent,
                Color.DeepSkyBlue,
                Color.DarkOrange);

            //DoBorder = false;

            RibbonControlGroup.Add(this);
        }

        public void AddItem(string item)
        {
            Dropdown.AddItem(item);
        }

        protected internal override void Update()
        {
            base.Update();

            Dropdown.Update();
            label.Update();

            label.Position = Position;
            Dropdown.Position = Position + Vector2.UnitX * 72;
        }

        public override void Draw()
        {
            //base.Draw();
            //base.DrawText();

            label.Position = Position;
            Dropdown.Position = Position + Vector2.UnitX * 72;

            label.Draw();
            Dropdown.Draw();
            Dropdown.DrawText();
        }
    }
}
