using Microsoft.Xna.Framework;
using VerticesEngine.UI.Themes;

namespace VerticesEngine.UI.Controls
{
    /// <summary>
    /// Label Class providing simple one line text as a vxGUI Item.
    /// </summary>
    public class vxLabel : vxUIControl
    {
        /// <summary>
        /// The rotation.
        /// </summary>
		public float Rotation = 0;

        /// <summary>
        /// The scale.
        /// </summary>
		public float Scale
        {
            get { return _scale; }
            set 
            { 
                _scale = value;
                //ResetBounds();
            }
        }
        float _scale = 1;

        /// <summary>
        /// This is the width of the Label without any scaling or modifcation. Essentially the Width of the text
        /// given the current SpriteFont
        /// </summary>
        public int DefaultWidth
        {
            get { return _defaultWidth; }
        }
        int _defaultWidth = 10;

        /// <summary>
        /// The max width of this label
        /// </summary>
        public int MaxWidth
        {
            get { return _maxWidth; }
            set
            {
                _maxWidth = value;
                ResetBounds();
            }
        }
        int _maxWidth = 100;

        /// <summary>
        /// Whether or not the label scales with resolution
        /// </summary>
        public bool IsScaleFixed = false;


        void ResetBounds()
        {
            Text = Font.WrapString(Text, (int)(_maxWidth / Scale * (IsScaleFixed ? 1 : vxLayout.Scale.X)));
        }

        /// <summary>
        /// The origin.
        /// </summary>
		public Vector2 Origin = new Vector2(0);

        public vxLabelArtProvider ArtProvider;


        /// <summary>
        /// Creates a label with default text
        /// </summary>
        public vxLabel() : this("Text", Vector2.Zero)
        {

        }

        public vxLabel(string localKeyName, string value, Vector2 position) : 
        this(vxLocalizer.GetText(localKeyName) + ": " + value , position)
        {
            Text = "";
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="VerticesEngine.UI.Controls.vxLabel"/> class.
		/// </summary>
		/// <param name="Engine">The Vertices Engine Reference.</param>
		/// <param name="text">This GUI Items Text.</param>
		/// <param name="position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
		public vxLabel(string text, Vector2 position) : base(position)
        {
            Text = text;
            ArtProvider = (vxLabelArtProvider)vxUITheme.ArtProviderForLabels.Clone();

            ShadowOffset = new Vector2(3) * vxLayout.Scale;
            ShadowTransparency = 0.75f;
            //this.Font = vxUITheme.Fonts.Size24;
            this.Font = ArtProvider.Font;

            // Initial Width is the Font
            Width = (int)Font.MeasureString (Text).X;
            Bounds = new Rectangle((int)(Position.X - Padding.X), (int)(Position.Y - Padding.Y / 2), Width, (int)(Font.MeasureString(Text).Y + Padding.Y / 2));


        }

        protected internal override void Update()
		{

		}

        public override void OnItemPositionChange()
        {
            base.OnItemPositionChange();
            OnTextChanged();
        }

        public override void OnTextChanged()
        {
            base.OnTextChanged();

            Bounds = new Rectangle(
                (int)(Position.X - Padding.X), 
                (int)(Position.Y - Padding.Y),
                (int)(TextSize.X * Scale + Padding.X * 2),
                (int)(TextSize.Y * Scale + Padding.Y * 2));


            _defaultWidth = (int)Font.MeasureString(Text).X;
        }

        /// <summary>
        /// Draws the GUI Item
        /// </summary>
        public override void Draw()
        {
            base.Draw();

            ArtProvider.DrawUIControl(this);
		}
    }
}
