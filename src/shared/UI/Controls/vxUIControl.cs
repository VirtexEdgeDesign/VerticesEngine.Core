using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using VerticesEngine.Audio;
using VerticesEngine.Graphics;
using VerticesEngine.Input;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Events;
using VerticesEngine.UI.Themes;

namespace VerticesEngine.UI
{
    public enum vxUIItemOrientation
    {
        Top,
        Left,
        Right,
        Bottom
    }

    /// <summary>
    /// Button types.
    /// </summary>
    public enum vxEnumButtonTypes
    {
        /// <summary>
        /// The ok.
        /// </summary>
        Ok,

        /// <summary>
        /// The ok cancel.
        /// </summary>
        OkCancel,

        /// <summary>
        /// The ok apply cancel.
        /// </summary>
        OkApplyCancel,


        /// <summary>
        /// No Buttons, and therefore state must be handled by the form itself.
        /// </summary>
        None,
    }

    /// <summary>
    /// Toggle State for Toggle Items
    /// </summary>
    public enum ToggleState
    {
        Off,
        On,
    }

    public enum vxHorizontalJustification
    {
      Left,
      Center,
      Right
    }



    public enum vxVerticalJustification
    {
        Top,
        Middle,
        Bottom
    }

    /// <summary>
    /// The most basic UI control which all other controls inherit from
    /// </summary>
    public abstract class vxUIControl : IDisposable
    {
        public float PositionDifference = 0;

        /// <summary>
        /// Object variable which allows arbitary data too be passed between methods.
        /// </summary>
        /// <value>The user data.</value>
        public object UserData;

        /// <summary>
        /// The owning GUI Manger.
        /// </summary>
        public vxUIManager UIManager;


        /// <summary>
        /// A string that is set in the base class of many items.
        /// </summary>
        public virtual Type GetBaseGuiType()
        {
            return typeof(vxUIControl);
        }

        public GraphicsDevice GraphicsDevice
        {
            get { return vxGraphics.GraphicsDevice; }
        }

        public Viewport Viewport
        {
            get { return vxGraphics.GraphicsDevice.Viewport; }
        }



        /// <summary>
        /// Scales an Integer by the Screen Scaler Size. This is used to keep GUI and Item Sizes
        /// consistent across different Screen Sizes and Resolutions.
        /// </summary>
        /// <returns>The scaled size.</returns>
        /// <param name="i">The index.</param>
        public int GetScaledSize(float i)
        {
            return (int)(i * vxLayout.ScaleAvg);
            //return (int)(i * Math.Max(1, ScaleAvg));
        }

        public float GetScalerAvg
        {
            get { return vxLayout.ScaleAvg; }
        }

        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <value>The state.</value>
        public vxEnumGUIElementState State
        {
            get
            {
                if (_isEnabled == false)
                    return vxEnumGUIElementState.Disabled;

                if (IsSelected)
                    return vxEnumGUIElementState.Selected;

                if (ToggleState)
                    return vxEnumGUIElementState.Selected;

                if (HasFocus)
                    return vxEnumGUIElementState.Hover;

                // Nothing, therefore it's Normal
                return vxEnumGUIElementState.Normal;
            }
        }

        /// <summary>
        /// Gets the sprite batch.
        /// </summary>
        /// <value>The sprite batch.</value>
        public vxSpriteBatch SpriteBatch
        {
            get { return vxGraphics.SpriteBatch; }
        }

        /// <summary>
        /// Gets the default texture.
        /// </summary>
        /// <value>The default texture.</value>
        public Texture2D DefaultTexture
        {
            get { return vxInternalAssets.Textures.Blank; }
        }


        #region Colours

        /// <summary>
        /// The item theme.
        /// </summary>
        public vxUIControlTheme Theme = new vxUIControlTheme();

        #endregion

        /// <summary>
        /// Gets or sets a value indicating whether this instance is visible.
        /// </summary>
        /// <value><c>true</c> if this instance is visible; otherwise, <c>false</c>.</value>
        public bool IsVisible = true;

        /// <summary>
        /// Name Of GUI Item to help Identify it, not to be confused with Text
        /// </summary>
        public string Name = "<name>";

        /// <summary>
        /// Text Of GUI Item
        /// </summary>
        public string Text
        {
            get { return m_text; }
            set
            {

                // Save the Previous Text
                PreviousText = m_text;

                // Now Fileter any Text
                m_text = FilterTextInput(value);

                // Now call the On Test Change event
                OnTextChanged();
            }
        }
       protected internal string m_text = "<text>";


        public string LocalisationKey;

        public virtual void OnLocalizationChanged()
        {
            Text = vxLocalizer.GetText(LocalisationKey);
        }

        public void SetLocalisedText(string key, string postface="")
        {
            Text = vxLocalizer.GetText(key) + postface;
        }

        public string PreviousText = "";

        public virtual string FilterTextInput(string input)
        {
            return input;
        }

        /// <summary>
        /// Called on text change.
        /// </summary>
        public virtual void OnTextChanged()
        {
            if (string.IsNullOrEmpty(m_text) == false && Font != null)
                TextSize = Font.MeasureString(m_text);
            else
                TextSize = Vector2.One;
        }

        public Vector2 TextSize = new Vector2();

        /// <summary>
        /// Text Of GUI Item
        /// </summary>
        public SpriteFont Font
        {
            get
            {
                if (_font == null)
                    _font = Font = vxUITheme.Fonts.Size16;
                return _font;
            }
            set { _font = value;OnFontSet(); }
        }
        SpriteFont _font;

        public virtual void OnFontSet()
        {
            
        }

        /// <summary>
        /// Gets or sets the opacity of the current GUI Item.
        /// </summary>
        /// <value>The opacity.</value>
        public float Opacity = 1;

        /// <summary>
        /// A transition Alpha which can be controled by an owning control such as a 
        /// Dialog or Message Box.
        /// </summary>
        public float TransitionAlpha = 1;

        /// <summary>
        /// Do selection border.
        /// </summary>
        public bool DoSelectionBorder = true;

        /// <summary>
        /// Gets or sets the opacity of the current GUI Item.
        /// </summary>
        /// <value>The opacity.</value>
        public bool DoBorder = false;


        /// <summary>
        /// The size of the border.
        /// </summary>
        public int BorderSize = 1;


        #region Drop Shadow Code

        /// <summary>
        /// Should the image draw a shadow.
        /// </summary>
        public bool IsShadowVisible
        {
            get { return _doShadow; }
            set
            {
                _doShadow = value;
                OnShadowStatusChange();
            }
        }
        bool _doShadow = false;

        /// <summary>
        /// Called when ever the DoShadow Property is changed.
        /// Override to change values for child elements.
        /// </summary>
        public virtual void OnShadowStatusChange() { }


        /// <summary>
        /// The shadow offset.
        /// </summary>
        public Vector2 ShadowOffset = new Vector2(5, 5);

        /// <summary>
        /// The shadow transparency.
        /// </summary>
        public float ShadowTransparency = 0.5f;


        /// <summary>
        /// The shadow colour.
        /// </summary>
        public Color ShadowColour = Color.Black;

        #endregion

        #region Item Status (Clicked, Hoverd Etc...)

        /// <summary>
        /// Gets or sets a value indicating whether this instance has focus.
        /// </summary>
        /// <value><c>true</c> if this instance has focus; otherwise, <c>false</c>.</value>
        public bool HasFocus
        {
            get { return _hasFocus; }
            set
            {
                _hasFocus = value;
                HoverAlphaReq = _hasFocus ? HoverAlphaMax : HoverAlphaMin;
            }
        }
        bool _hasFocus = false;


        /// <summary>
        /// Gets or sets a value indicating whether this instance has focus.
        /// </summary>
        /// <value><c>true</c> if this instance has focus; otherwise, <c>false</c>.</value>
        public bool CaptureInput
        {
            get { return _captureInput; }
            set
            {
                _captureInput = value;

                if (UIManager != null)
                {
                    if (_captureInput == true)
                        this.UIManager.FocusedItem = this;
                    else
                        this.UIManager.FocusedItem = null;
                }
            }
        }
        bool _captureInput = false;

        /// <summary>
        /// Is the Item a Toggleable?
        /// </summary>
        public bool IsTogglable
        {
            get { return _isTogglable; }
            set
            {
                _isTogglable = value;

                if (value == true)
                    ToggleState = false;

            }
        }
        bool _isTogglable = false;

        /// <summary>
        /// Toggle State of the GUI Item. Note: IsTogglable must be set too true.
        /// </summary>
        public bool ToggleState;


        /// <summary>
        /// Gets or sets the alpha of the GUI Item.
        /// </summary>
        /// <value>The alpha.</value>
        public float Alpha = 1;

        /// <summary>
        /// Gets or sets the hover alpha.
        /// </summary>
        /// <value>The hover alpha.</value>
        public float HoverAlpha = 0;

        /// <summary>
        /// Gets or sets the requested hover alpha for smoothing.
        /// </summary>
        /// <value>The hover alpha req.</value>
        public float HoverAlphaReq = 0;

        /// <summary>
        /// Gets or sets the hover alpha max.
        /// </summary>
        /// <value>The hover alpha max.</value>
        public float HoverAlphaMax = 1;

        /// <summary>
        /// Gets or sets the hover alpha minimum.
        /// </summary>
        /// <value>The hover alpha minimum.</value>
        public float HoverAlphaMin = 0;

        /// <summary>
        /// Gets or sets the hover alpha delta speed of smoothing.
        /// </summary>
        /// <value>The hover alpha delta speed.</value>
        public float HoverAlphaDeltaSpeed = 4;

        /// <summary>
        /// Returns Whether or not the item is Selected
        /// </summary>
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                HoverAlphaReq = 0;
            }
        }
        bool isSelected = false;

        /// <summary>
        /// Event Raised when the item is clicked
        /// </summary>
        public event EventHandler<vxUIControlClickEventArgs> Clicked;

        /// <summary>
        /// Forces a click event
        /// </summary>
        protected void Click()
        {
            if (Clicked != null)
                Clicked(this, new vxUIControlClickEventArgs(this));
        }

        /// <summary>
        /// Occurs when double clicked.
        /// </summary>
        public event EventHandler<vxUIControlClickEventArgs> DoubleClicked;

        /// <summary>
        /// Event Raised when this item is added to a GUI Manager
        /// </summary>
        public event EventHandler<vxUIManagerItemAddEventArgs> AddedToGUIManager;

        /// <summary>
        /// Event Raised when the Mouse First Begins too Hover over this item.
        /// </summary>
        public event EventHandler<EventArgs> OnInitialHover;

        /// <summary>
        /// Returns Whether or not the item is Enabled
        /// </summary>
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;

                OnEnableStateChanged();

                // Raise the 'Changed' event.
                if (EnabledStateChanged != null)
                    EnabledStateChanged(this, new EventArgs());
            }
        }
        private bool _isEnabled = true;

        /// <summary>
        /// Called when the elabled state changes
        /// </summary>
        protected virtual void OnEnableStateChanged() { }

        /// <summary>
        /// Occurs when enabled state changed.
        /// </summary>
        public event EventHandler<EventArgs> EnabledStateChanged;

        #endregion

        #region Item Layout Properties

        /// <summary>
        /// Position Of GUI Item
        /// </summary>
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;

                Bounds = new Rectangle((int)_position.X, (int)_position.Y, _width, _height);

                OnItemPositionChange();

                if (PositionChanged != null)
                    PositionChanged(this, new EventArgs());
            }
        }
        Vector2 _position = Vector2.Zero;

        public virtual void OnItemPositionChange() { }

        /// <summary>
        /// Position Of GUI Item
        /// </summary>
        public Vector2 OriginalPosition = Vector2.Zero;

        /// <summary>
        /// Padding Of GUI Item
        /// </summary>
        public Vector2 Padding = new Vector2(5);


        public virtual void ResetLayout() { }

        public bool IsFullWidth = false;

        public bool IsFullHeight = false;

        /// <summary>
        /// Width Of GUI Item
        /// </summary>
        public int Width
        {
            get { return Bounds.Width; }
            set
            {
                _width = value;
                Bounds = new Rectangle((int)_position.X, (int)_position.Y, _width, _height);
            }
        }
        int _width = 48;

        /// <summary>
        /// Width Of GUI Item
        /// </summary>
        public int Height
        {
            get { return Bounds.Height; }
            set
            {
                _height = value;
                Bounds = new Rectangle((int)_position.X, (int)_position.Y, _width, _height);
            }
        }
        int _height = 48;

        /// <summary>
        /// Event raised when Item Position is Changed
        /// </summary>
        public event EventHandler<EventArgs> PositionChanged;

        /// <summary>
        /// Bounding Rectangle Of GUI Item
        /// </summary>
        public Rectangle Bounds
        {
            get { return _bounds; }
            set
            {
                _bounds = value;
                _width = value.Width;
                _height = value.Height;
                BorderBounds = _bounds.GetBorder(BorderSize);

                if (_isLayoutResetting == false)
                {
                    _isLayoutResetting = true;
                    OnLayoutInvalidated();
                    _isLayoutResetting = false;
                }
            }
        }
        Rectangle _bounds = Rectangle.Empty;
        bool _isLayoutResetting = false;

        /// <summary>
        /// The border bounds of the gui item.
        /// </summary>
        public Rectangle BorderBounds = Rectangle.Empty;

        /// <summary>
        /// The border colour.
        /// </summary>
        public Color BorderColour = Color.Black;

        public Vector4 BoundingRectangleMargins = new Vector4(0);

        /// <summary>
        /// GUI Item Orientation
        /// </summary>
        public vxUIItemOrientation ItemOrientation
        {
            get { return itemOreintation; }
            set
            {
                itemOreintation = value;

                // Raise the 'Changed' event.
                if (ItemOreintationChanged != null)
                    ItemOreintationChanged(this, new EventArgs());
            }
        }
        vxUIItemOrientation itemOreintation = vxUIItemOrientation.Top;

        public bool IsOrientationHorizontal
        {
            get
            {
                return (ItemOrientation == vxUIItemOrientation.Left || itemOreintation == vxUIItemOrientation.Right);                     
            }
        }

        /// <summary>
        /// Event Raised when Item Orientation is Changed
        /// </summary>
        public event EventHandler<EventArgs> ItemOreintationChanged;






        #endregion


        /// <summary>
        /// Previous Mouse State
        /// </summary>
        public MouseState PreviousMouseState
        {
            get { return vxInput.PreviousMouseState; }
        }



        /// <summary>
        /// Element Index.
        /// </summary>
        public int Index = 0;

        /// <summary>
        /// The index in the gui system.
        /// </summary>
        public int GUIIndex = 0;

        public ScreenState ScreenState = ScreenState.TransitionOn;


        bool InitialHover = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="VerticesEngine.UI.vxUIControl"/> class.
        /// </summary>
		public vxUIControl() { }


        /// <summary>
        /// Initializes a new instance of the <see cref="VerticesEngine.UI.vxUIControl"/> class.
        /// </summary>
        /// <param name="position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
        public vxUIControl(Vector2 position)
        {
            this.Position = position;
            this.OriginalPosition = position;
        }

        /// <summary>
        /// Initialise this instance.
        /// </summary>
        //public virtual void Initialise()
        //{

        //}

        private bool isDisposed = false;

        /// <summary>
        /// Call this to dispose the UI control. This should only be called by the <see cref="vxUIManager"/> or the <see cref="vxCanvas"/>
        /// </summary>
        public void Dispose()
        {
            if (!isDisposed)
            {
                Clicked = null;
                DoubleClicked = null;
                OnInitialHover = null;

                OnDisposed();
                isDisposed = true;
            }
        }

        /// <summary>
        /// Called when this UI control is disposed
        /// </summary>
        protected virtual void OnDisposed() { }


        /// <summary>
        /// When the Mouse is NOT over the GUIItem
        /// </summary>
        public virtual void NotHover()
        {
            //isSelected = false;
            HasFocus = false;

            //Reset Initial Hover Flag
            InitialHover = true;

            // Cut off the Tool Tip Count;
            if (IsToolTipEnabled)
                ToolTipeCount = 0;
        }

        /// <summary>
        /// When the Mouse is over the GUIItem
        /// </summary>
        public virtual void Hover()
        {
            //isSelected = false;
            HasFocus = true;

            if (InitialHover)
            {
                InitialHover = false;

                if (OnInitialHover != null)
                    OnInitialHover(this, new EventArgs());
            }

            // Increment for the tool tip showing.
            if (IsToolTipEnabled)
                ToolTipeCount++;
        }
        int doubleClickCounter = 0;
        /// <summary>
        /// When the GUIItem is Selected
        /// </summary>
        public virtual void Select()
        {
            if (IsEnabled && HasMouseBeenUpYet)
            {
                //Set Toggle Status if this item is Toggleable
                if (IsTogglable)
                    ToggleState = !ToggleState;

                //To Show some visible cure the click was registered.
                HoverAlpha = 0;

                if (doubleClickCounter < 20 && DoubleClicked != null)
                {
                    // Raise the Clicked event.
                    if (DoubleClicked != null)
                        DoubleClicked(this, new vxUIControlClickEventArgs(this));
                }
                else
                {
                    // Raise the Clicked event.
                    if (Clicked != null)
                        Clicked(this, new vxUIControlClickEventArgs(this));
                }


                doubleClickCounter = 0;

                isSelected = true;
                HasFocus = true;
            }
        }

        protected internal virtual void HandleInput()
        {

        }

        public virtual void OnGUIManagerAdded(vxUIManager UIManager)
        {
            this.UIManager = UIManager;

            // Raise the Added event.
            if (AddedToGUIManager != null)
                AddedToGUIManager(this, new vxUIManagerItemAddEventArgs(UIManager));
        }

        /// <summary>
        /// This is called whenever the layout is changed, content changed, or any method has been
        /// called which requires the control to be re-layedout.
        /// </summary>
        public virtual void OnLayoutInvalidated()
        {

        }


        /// <summary> 
        /// Plays the speciefied Sound Effect. Note this is to encapsulate the Sound Playing
        /// in one place around a try-catch block. Although this is not the ideal way of doing it, it allows
        /// for better cross-platform support since different platforms, namely mobile, have different maximums
        /// of letting sound effects play at the same time.
        /// </summary>
        /// <param name="SoundEffect">Sound effect.</param>
        /// <param name="Volume">Volume.</param>
        public virtual void PlaySound(SoundEffect SoundEffect, float Volume = 1, float Pitch = 0)
        {
            try
            {
                // Max the Volume at 1.
                Volume = MathHelper.Min(Volume, 1);

                // Create the Instance of the Sound Effect.
                SoundEffectInstance sndEfctInstnc = SoundEffect.CreateInstance();

                // Set the Volume
                sndEfctInstnc.Volume = Volume * vxAudioManager.SoundEffectVolume;

                // Set the Pitch
                sndEfctInstnc.Pitch = Pitch;

                // Play It
                sndEfctInstnc.Play();
            }
            catch (Exception ex)
            {
                vxConsole.WriteException(this,new Exception("Error Playing GUI Sound Effect", ex));
            }
        }

        /// <summary>
        /// Gets a value indicating whether the mouse left button been up yet.
        /// </summary>
        /// <remarks>A check for if the Left Button has been in the Release State yet. This is to prevent
        // the 'Click' event being fired if the button is created where the mouse is 
        // and if the mouse is currently being clicked.</remarks>
        /// <value><c>true</c> The mouse left button has been in the 'Release' state since this GUI item has been instantiated; otherwise, <c>false</c>.</value>
        public bool HasMouseBeenUpYet
        {
            get
            {
                return _hasMouseBeenUpYet;
            }
            set { _hasMouseBeenUpYet = value; }
        }
        private bool _hasMouseBeenUpYet = false;

        /// <summary>
        /// The initial touch location. This is to catch scrolling.
        /// </summary>
        Vector2 InitialTouchLocation = Vector2.Zero;

        /// <summary>
        /// This bool holds a value of whether or not Select should be disabled
        /// if the item is moved a certain distance in between 'TouchLocationState.Pressed'
        /// and 'TouchLocationState.Released'. The threshold distance is 'DisableScrollThreshold'.
        /// </summary>
        public bool DisableTouchSelectOnScroll = false;

        /// <summary>
        /// The disable scroll threshold. This is only used if 'DisableTouchSelectOnScroll' is true.
        /// </summary>
        public float DisableScrollThreshold = 5;

        public vxEnumClickType ClickType = vxEnumClickType.OnPress;

        #if __MOBILE__
        Vector2 CursorPos;
        //bool FirstTouchDown = true;
#endif


		/// <summary>
		/// Is this item Updatable.
		/// </summary>
		public bool IsUpdateable = true;

        //[Obsolete("This will be removed")]
        public void UpdateUI()
        {
            Update();
        }
        /// <summary>
        /// Updates the GUI Item
        /// </summary>
        protected internal virtual void Update()
        {
            doubleClickCounter++;

            //Get Position of either Mouse or Touch Panel Click
            //Vector2 cursorPos = mouseState.Position.ToVector2();

            if (vxInput.MouseState.LeftButton == ButtonState.Released)
                IsSelected = false;

            //Vector2 cursorPos = new Vector2(vxInput.MouseState.X, vxInput.MouseState.Y);



			//try
			//{
#if __MOBILE__
                if (vxInput.TouchCollection.Count > 0)
                {
                    CursorPos = vxInput.TouchCollection[0].Position;
                }
#endif
			// if it's not updatable, only check if the mouse button is down
			if (IsUpdateable || vxInput.IsMouseButtonPressed(MouseButtons.LeftButton))
			{
				if (Bounds.Intersects(vxInput.CursorPixel) && vxInput.IsInit)
				{
#if __IOS___OLD || __ANDROID___OLD
                        if (vxInput.TouchCollection.Count > 0 && vxInput.IsDragging == false)
                        {
							// this.vxInput.Cursor = touchCollection[0].Position;


							if (ClickType == vxEnumClickType.OnPress)
							{
								if (vxInput.TouchCollection[0].State == TouchLocationState.Pressed)
									Select();
								//Hover if and only if Moved is selected
								else if (vxInput.TouchCollection[0].State == TouchLocationState.Moved)
									Hover();
							}
							else if (ClickType == vxEnumClickType.OnRelease)
							{
								// If it's pressed, then record the location. TouchLocationState.Pressed doesn't always fire
								// on all platforms reliably, so this is a hardcoded fix.
								if (FirstTouchDown == true)
								{
									InitialTouchLocation = CursorPos;
									FirstTouchDown = false;
								}

								//Only Fire Select Once it's been released
								if (vxInput.TouchCollection[0].State == TouchLocationState.Released)
								{
									if (DisableTouchSelectOnScroll == true)
									{
										// Reset First Touch Down
										FirstTouchDown = true;

										if (Vector2.Subtract(InitialTouchLocation, CursorPos).Length() < DisableScrollThreshold)
											Select();
									}
									else
										Select();
								}
								//else if (vxInput.touchCollection[0].State == TouchLocationState.Pressed)
								//	Select();
								//Hover if and only if Moved is selected
								else if (vxInput.TouchCollection[0].State == TouchLocationState.Moved)
									Hover();
							}
                        }
#else

                    if (_hasMouseBeenUpYet)
                    {
                        if (vxInput.IsNewMainInputUp())
                            Select();
                        else
                            Hover();
                    }
#endif
				}
				else
					NotHover();
			}
			else
				NotHover();
            
            //If it's a Toggle Item, set Toggle State
            if (IsTogglable)
            {
                if (ToggleState)
                    HoverAlphaReq = HoverAlphaMax;
                else
                    HoverAlphaReq = _hasFocus ? HoverAlphaMax : HoverAlphaMin;
            }

            HoverAlpha = vxMathHelper.Smooth(HoverAlpha, HoverAlphaReq, HoverAlphaDeltaSpeed);

            Theme.SetState(this);

            // A check for if the Left Button has been in the Release State yet. This is to prevent
            // the 'Click' event being fired if the button is created where the mouse is 
            // and if the mouse is currently being clicked.
            if (vxInput.MouseState.LeftButton == ButtonState.Released)
                _hasMouseBeenUpYet = true;
        }



        /// <summary>
        /// Draws the GUI Item. 
        /// </summary>
        public virtual void Draw()
        {
            DrawToolTip();
        }


		public Color GetStateColour(vxColourTheme theme)
		{
            Color baseState = (ToggleState || IsSelected) ? theme.SelectedColour : (HasFocus ? theme.HoverColour : theme.NormalColour);

            return IsEnabled ? baseState : theme.NormalColour * 0.75f;
		}

        /// <summary>
        /// Draws the GUI Items text. This is kept seperate for effiecency when using
        /// Sprite Sheets.
        /// </summary>
        public virtual void DrawText(){}


#region ToolTip

        public string ToolTipText = "";

        /// <summary>
        /// Are Tooltips Enabled for this GUI Item.
        /// </summary>
        public bool IsToolTipEnabled = false;

        public int ToolTipeCount = 0;

        /// <summary>
        /// The amount of update calls with this item recieving focus before it'll 
        /// show the Tool Tip if it's Enabled.
        /// </summary>
        public int ToolTipMax = 15;

        bool CanDrawToolTip
        {
            get { return (IsToolTipEnabled && (ToolTipeCount > ToolTipMax)); }
        }

        public void SetToolTip(string ToolTipText)
        {
            IsToolTipEnabled = true;
            this.ToolTipText = ToolTipText;
        }

        public float ToolTipAlpha = 0;


        /// <summary>
        /// If Possible, will draw the Tool Tip.
        /// </summary>
        public virtual void DrawToolTip()
        {
            if (CanDrawToolTip)
            {
                vxUITheme.ArtProviderForToolTips.Draw(this);
            }
            else
            {
                ToolTipAlpha = 0;
            }
        }
#endregion

        public virtual void DrawBorder()
        {
            
        }


#region Utilties

		/// <summary>
		/// Gets the centered text position.
		/// </summary>
		/// <returns>The centered text position.</returns>
		/// <param name="font">Font.</param>
		/// <param name="text">This GUI Items Text.</param>
		public Vector2 GetCenteredTextPosition(SpriteFont font, string text)
		{
			Vector2 TextSize = font.MeasureString(text);

			return new Vector2((Bounds.Width - TextSize.X) / 2, (Bounds.Height - TextSize.Y) / 2);
		}

        /// <summary>
        /// Draws the debug point.
        /// </summary>
        /// <param name="Pos">Position.</param>
        public void DrawDebugPoint(Vector2 Pos) { DrawDebugPoint(Pos, Color.DeepPink); }

        /// <summary>
        /// Draws the debug point.
        /// </summary>
        /// <param name="Pos">Position.</param>
        /// <param name="color">Color.</param>
        public void DrawDebugPoint(Vector2 Pos, Color color)
        {
            int size = 4;
            Rectangle bounds = new Rectangle((int)Pos.X - size, (int)Pos.Y - size, size, size);
            SpriteBatch.Draw(DefaultTexture, bounds, color);
        }

#endregion

        /// <summary>
        /// Tries to convert keyboard input to characters and prevents repeatedly returning the 
        /// same character if a key was pressed last frame, but not yet unpressed this frame.
        /// </summary>
        /// <param name="keyboard">The current KeyboardState</param>
        /// <param name="oldKeyboard">The KeyboardState of the previous frame</param>
        /// <param name="key">When this method returns, contains the correct character if conversion succeeded.
        /// Else contains the null, (000), character.</param>
        /// <returns>True if conversion was successful</returns>
        [Obsolete("Kept for Posterity")]
        public bool TryConvertKeyboardInput(KeyboardState keyboard, KeyboardState oldKeyboard, out char key)
        {
            Keys[] keys = keyboard.GetPressedKeys();
            bool shift = keyboard.IsKeyDown(Keys.LeftShift) || keyboard.IsKeyDown(Keys.RightShift);

            if (keys.Length > 0 && !oldKeyboard.IsKeyDown(keys[0]))
            {
                switch (keys[0])
                {
                    //Alphabet keys
                    case Keys.A: if (shift) { key = 'A'; } else { key = 'a'; } return true;
                    case Keys.B: if (shift) { key = 'B'; } else { key = 'b'; } return true;
                    case Keys.C: if (shift) { key = 'C'; } else { key = 'c'; } return true;
                    case Keys.D: if (shift) { key = 'D'; } else { key = 'd'; } return true;
                    case Keys.E: if (shift) { key = 'E'; } else { key = 'e'; } return true;
                    case Keys.F: if (shift) { key = 'F'; } else { key = 'f'; } return true;
                    case Keys.G: if (shift) { key = 'G'; } else { key = 'g'; } return true;
                    case Keys.H: if (shift) { key = 'H'; } else { key = 'h'; } return true;
                    case Keys.I: if (shift) { key = 'I'; } else { key = 'i'; } return true;
                    case Keys.J: if (shift) { key = 'J'; } else { key = 'j'; } return true;
                    case Keys.K: if (shift) { key = 'K'; } else { key = 'k'; } return true;
                    case Keys.L: if (shift) { key = 'L'; } else { key = 'l'; } return true;
                    case Keys.M: if (shift) { key = 'M'; } else { key = 'm'; } return true;
                    case Keys.N: if (shift) { key = 'N'; } else { key = 'n'; } return true;
                    case Keys.O: if (shift) { key = 'O'; } else { key = 'o'; } return true;
                    case Keys.P: if (shift) { key = 'P'; } else { key = 'p'; } return true;
                    case Keys.Q: if (shift) { key = 'Q'; } else { key = 'q'; } return true;
                    case Keys.R: if (shift) { key = 'R'; } else { key = 'r'; } return true;
                    case Keys.S: if (shift) { key = 'S'; } else { key = 's'; } return true;
                    case Keys.T: if (shift) { key = 'T'; } else { key = 't'; } return true;
                    case Keys.U: if (shift) { key = 'U'; } else { key = 'u'; } return true;
                    case Keys.V: if (shift) { key = 'V'; } else { key = 'v'; } return true;
                    case Keys.W: if (shift) { key = 'W'; } else { key = 'w'; } return true;
                    case Keys.X: if (shift) { key = 'X'; } else { key = 'x'; } return true;
                    case Keys.Y: if (shift) { key = 'Y'; } else { key = 'y'; } return true;
                    case Keys.Z: if (shift) { key = 'Z'; } else { key = 'z'; } return true;

                    //Decimal keys
                    case Keys.D0: if (shift) { key = ')'; } else { key = '0'; } return true;
                    case Keys.D1: if (shift) { key = '!'; } else { key = '1'; } return true;
                    case Keys.D2: if (shift) { key = '@'; } else { key = '2'; } return true;
                    case Keys.D3: if (shift) { key = '#'; } else { key = '3'; } return true;
                    case Keys.D4: if (shift) { key = '$'; } else { key = '4'; } return true;
                    case Keys.D5: if (shift) { key = '%'; } else { key = '5'; } return true;
                    case Keys.D6: if (shift) { key = '^'; } else { key = '6'; } return true;
                    case Keys.D7: if (shift) { key = '&'; } else { key = '7'; } return true;
                    case Keys.D8: if (shift) { key = '*'; } else { key = '8'; } return true;
                    case Keys.D9: if (shift) { key = '('; } else { key = '9'; } return true;

                    //Decimal numpad keys
                    case Keys.NumPad0: key = '0'; return true;
                    case Keys.NumPad1: key = '1'; return true;
                    case Keys.NumPad2: key = '2'; return true;
                    case Keys.NumPad3: key = '3'; return true;
                    case Keys.NumPad4: key = '4'; return true;
                    case Keys.NumPad5: key = '5'; return true;
                    case Keys.NumPad6: key = '6'; return true;
                    case Keys.NumPad7: key = '7'; return true;
                    case Keys.NumPad8: key = '8'; return true;
                    case Keys.NumPad9: key = '9'; return true;

                    //Special keys
                    case Keys.OemTilde: if (shift) { key = '~'; } else { key = '`'; } return true;
                    case Keys.OemSemicolon: if (shift) { key = ':'; } else { key = ';'; } return true;
                    case Keys.OemQuotes: if (shift) { key = '"'; } else { key = '\''; } return true;
                    case Keys.OemQuestion: if (shift) { key = '?'; } else { key = '/'; } return true;
                    case Keys.OemPlus: if (shift) { key = '+'; } else { key = '='; } return true;
                    case Keys.OemPipe: if (shift) { key = '|'; } else { key = '\\'; } return true;
                    case Keys.OemPeriod: if (shift) { key = '>'; } else { key = '.'; } return true;
                    case Keys.OemOpenBrackets: if (shift) { key = '{'; } else { key = '['; } return true;
                    case Keys.OemCloseBrackets: if (shift) { key = '}'; } else { key = ']'; } return true;
                    case Keys.OemMinus: if (shift) { key = '_'; } else { key = '-'; } return true;
                    case Keys.OemComma: if (shift) { key = '<'; } else { key = ','; } return true;
                    case Keys.Space: key = ' '; return true;
                }
            }

            key = (char)0;
            return false;
        }
    }
}
