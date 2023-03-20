using System;
using Microsoft.Xna.Framework;

namespace VerticesEngine.UI
{
    public enum vxEnumGUIElementState
    {
        Normal,
        Hover,
        Selected,
        Disabled
    }

    public class vxColourTheme
    {
        /// <summary>
        /// The Normal Colour.
        /// </summary>
        public Color NormalColour = new Color(0.15f, 0.15f, 0.15f, 1);

        /// <summary>
        /// The Hover colour.
        /// </summary>
        public Color HoverColour = Color.DeepSkyBlue;

        /// <summary>
        /// The selected colour.
        /// </summary>
        public Color SelectedColour = Color.DarkOrange;

        /// <summary>
        /// The Color to draw when the element is Disabled.
        /// </summary>
        public Color DisabledColour = Color.DarkGray;


        /// <summary>
        /// Gets or sets the element state.
        /// </summary>
        /// <value>The state.</value>
        public vxEnumGUIElementState State
        {
            get { return _state; }
            set 
            {
                _state = value; 
                SetColor((_state));
            }
        }
        vxEnumGUIElementState _state = vxEnumGUIElementState.Normal;

        void SetColor(vxEnumGUIElementState newState)
        {
            switch (newState)
            {
                case vxEnumGUIElementState.Hover:
                    _color = HoverColour;
                    break;
                case vxEnumGUIElementState.Selected:
                    _color = SelectedColour;
                    break;
                case vxEnumGUIElementState.Disabled:
                    _color = DisabledColour;
                    break;
                default:
                    _color = NormalColour;
                    break;
            }
        }

        /// <summary>
        /// Gets the color given the current Element State.
        /// </summary>
        /// <value>The color.</value>
        public Color Color
        {
            get { return _color; }
        }
        private Color _color;


        /// <summary>
        /// The disabled alpha.
        /// </summary>
        public float DisabledAlpha = 0.5f;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Virtex.Lib.Iris.Gui.Theme.vxThemeColour"/> class with the 
        /// default colours.
        /// </summary>
        public vxColourTheme()
        {
            State = vxEnumGUIElementState.Normal;
        }

        public vxColourTheme(Color NormalColour) :
        this(NormalColour, NormalColour, NormalColour)
        {

        }

        public void SetTheme(Color NormalColour)
        {
            this.NormalColour = NormalColour;
            this.HoverColour = NormalColour;
            this.SelectedColour = NormalColour;
            DisabledColour = NormalColour * DisabledAlpha;
            DisabledColour.A = NormalColour.A;
            State = vxEnumGUIElementState.Normal;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Themes.vxColourTheme"/> class.
        /// </summary>
        /// <param name="NormalColour">Normal colour.</param>
        /// <param name="HoverColour">Hover colour.</param>
        public vxColourTheme(Color NormalColour, Color HoverColour) :
        this(NormalColour, HoverColour, NormalColour)
        {

        }

        public void SetTheme(Color NormalColour, Color HoverColour)
        {
            this.NormalColour = NormalColour;
            this.HoverColour = HoverColour;
            this.SelectedColour = NormalColour;
            DisabledColour = NormalColour * DisabledAlpha;
            DisabledColour.A = NormalColour.A;
            State = vxEnumGUIElementState.Normal;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="T:Virtex.Lib.Iris.Gui.Theme.vxThemeColour"/> class.
        /// </summary>
        /// <param name="NormalColour">Normal.</param>
        /// <param name="HoverColour">Hover.</param>
        /// <param name="SelectedColour">Selected.</param>
        public vxColourTheme(Color NormalColour, Color HoverColour, Color SelectedColour)
        {
            this.NormalColour = NormalColour;
            this.HoverColour = HoverColour;
			this.SelectedColour = SelectedColour;
            DisabledColour = NormalColour * DisabledAlpha;
            DisabledColour.A = NormalColour.A;
			State = vxEnumGUIElementState.Normal;
        }

        public void SetTheme(Color NormalColour, Color HoverColour, Color SelectedColour)
        {
            this.NormalColour = NormalColour;
            this.HoverColour = HoverColour;
            this.SelectedColour = SelectedColour;
            DisabledColour = NormalColour * DisabledAlpha;
            DisabledColour.A = NormalColour.A;
            State = vxEnumGUIElementState.Normal;
        }

        public vxColourTheme(Color NormalColour, Color HoverColour, Color SelectedColour, Color DisabledColour)
        {
            this.NormalColour = NormalColour;
            this.HoverColour = HoverColour;
            this.SelectedColour = SelectedColour;
            this.DisabledColour = DisabledColour;
            State = vxEnumGUIElementState.Normal;
        }


        public void SetTheme(Color NormalColour, Color HoverColour, Color SelectedColour, Color DisabledColour)
        {
            this.NormalColour = NormalColour;
            this.HoverColour = HoverColour;
            this.SelectedColour = SelectedColour;
            this.DisabledColour = DisabledColour;
            State = vxEnumGUIElementState.Normal;
        }
    }
}
