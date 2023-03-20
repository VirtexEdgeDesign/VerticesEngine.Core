using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using VerticesEngine.Diagnostics;
using VerticesEngine.Input;
using VerticesEngine.UI.Themes;
#if __IOS__ || __ANDROID__
using Microsoft.Xna.Framework.Input.Touch;
//using Microsoft.Xna.Framework.GamerServices;
#endif

namespace VerticesEngine.UI.Controls
{
    enum InputJustification
    {
        All,
        Letters,
        Numerical
    }

    /// <summary>
    /// Textbox Control for us in the vxGUI System.
    /// </summary>
    public class vxTextbox : vxUIControl
    {
        /// <summary>
        /// Gets or sets the art provider.
        /// </summary>
        /// <value>The art provider.</value>
        public vxTextboxArtProvider ArtProvider;

        /// <summary>
        /// Caret Alpha
        /// </summary>
        public float CaretAlpha = 0;
        
        /// <summary>
        /// The Cursor Position.
        /// </summary>
        public Vector2 CaretPosition;

        /// <summary>
        /// The Index the cursor is in in the string.
        /// </summary>
        public int CaretIndex = 0;

        private float m_caretBlink = 0;
        private float m_caretReqAlpha = 0;

        /// <summary>
        /// The Font from the Current Art Provider. This is needed to place the Cursor and handle text wrapping.
        /// </summary>
        new SpriteFont Font
        {
            //set { ArtProvider.Font = value; }
            get
            {
                return GetDefaultFont();
            }
        }

        /// <summary>
        /// Occurs when enabled state changed.
        /// </summary>
        public event EventHandler<EventArgs> TextChanged;

        /// <summary>
        /// Gets the default font.
        /// </summary>
        /// <returns>The font.</returns>
        public virtual SpriteFont GetDefaultFont()
        {
            if (ArtProvider != null)
                return ArtProvider.Font;
            else
                return vxInternalAssets.Fonts.BaseFont;
        }


        // Key that pressed last frame.
        private Keys pressedKey;

        // Timer for key repeating.
        private float keyRepeatTimer;

        // Key repeat duration in seconds for the first key press.
        private const float keyRepeatStartDuration = 0.3f;

        // Key repeat duration in seconds after the first key press.
        private const float keyRepeatDuration = 0.03f;

        public string TextboxTitle = "";

        public string TextboxDescription = "";

        public bool IsBackgroundTransparent = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxTextbox"/> class.
        /// </summary>
        /// <param name="text">This GUI Items Text.</param>
        /// <param name="position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
        /// <param name="TextboxTitle">Textbox title.</param>
        /// <param name="TextboxDescription">Textbox description.</param>
        public vxTextbox(string text, Vector2 position, string TextboxTitle = "Title", string TextboxDescription = "Description") :
        this( text, position, TextboxTitle, TextboxDescription, 200)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxTextbox"/> class.
        /// </summary>
        /// <param name="text">This GUI Items Text.</param>
        /// <param name="position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
        /// <param name="TextboxTitle">Textbox title.</param>
        /// <param name="TextboxDescription">Textbox description.</param>
        /// <param name="length">Length.</param>
        public vxTextbox(string text, Vector2 position,
                         string TextboxTitle, string TextboxDescription, int width)
        {
            //Set Text
            Text = text;

            //Set Textbox Length
            Width = width;

            this.TextboxTitle = TextboxTitle;
            this.TextboxDescription = TextboxDescription;

            //Set Position
            Position = position;

            //Set Justification
            //InputJustification = InputJustification.All;

            //Have this button get a clone of the current Art Provider
            ArtProvider = vxUITheme.ArtProviderForTextboxes;

            //Font = GetDefaultFont();
            
            CaretIndex = text.Length;
            OnTextChanged();


        }


        public override void Select()
        {
            CaretIndex = Text.Length;

            IsSelected = true;
            HasFocus = true;
        }

        public override void NotHover()
        {
            base.NotHover();
        }

#if __IOS__ || __ANDROID__
        bool IsGuideUp = false;
#endif
        protected internal override void Update()
        {
            MouseState mouseState = vxInput.MouseState;

            Vector2 cursor = vxInput.Cursor;

            //if (cursor.X > Bounds.Left && cursor.X < Bounds.Right &&
            //	cursor.Y < Bounds.Bottom && cursor.Y > Bounds.Top)
            if (Bounds.Contains(cursor))
            {
#if __IOS__ || __ANDROID__
				if (vxInput.TouchCollection.Count > 0)
				{

					//Only Fire Select Once it's been released
					if (vxInput.TouchCollection[0].State == TouchLocationState.Pressed)
						Select();
					//Hover if and only if Moved is selected
					else if (vxInput.TouchCollection[0].State == TouchLocationState.Moved)
						Hover();
				}
#else

                if (mouseState.LeftButton == ButtonState.Pressed && PreviousMouseState.LeftButton == ButtonState.Released)
                    Select();
                else if (IsSelected == false)
                    Hover();
#endif
            }

#if __IOS__ || __ANDROID__

			else if (vxInput.TouchCollection.Count > 0)
			{
				if (vxInput.TouchCollection[0].State == TouchLocationState.Pressed ||
					 vxInput.TouchCollection[0].State == TouchLocationState.Moved ||
				IsSelected == false)
				{
					NotHover();
				}
			}
#else
            else if (mouseState.LeftButton == ButtonState.Pressed && PreviousMouseState.LeftButton == ButtonState.Released ||
                IsSelected == false)
            {
                NotHover();
                IsSelected = false;
            }
#endif

            //Set State for next Loop
            //PreviousMouseState = mouseState;

            if (IsSelected == true && IsEnabled)
            {
                m_caretBlink++;

                if (m_caretBlink < 30)
                {
                    m_caretReqAlpha = 1;
                }
                else if (m_caretBlink < 60)
                {
                    m_caretReqAlpha = 0;
                }
                else
                    m_caretBlink = 0;

                // Use the internet system keyboards for Mobile Platforms
                try
                {
#if __IOS__ || __ANDROID__
				if (IsGuideUp == false)
				{
					IsGuideUp = true;
                        //Guide.BeginShowKeyboardInput(PlayerIndex.One, TextboxTitle, TextboxDescription, Text, HandleTextAsyncCallback, null);
                        ShowInputDialog(TextboxTitle, TextboxDescription, Text, (txt) =>
                        {
                            if(!string.IsNullOrEmpty(txt))
                                Text = txt;

                            IsGuideUp = false;

                            IsSelected = false;
                            HasFocus = false;
                        });
				}
#else
                    ProcessKeyInputs(0.0167f);
#endif
                }
                catch (Exception ex) {
                    vxConsole.WriteException(ex);
                    vxConsole.WriteError("Error Processing Text");
                }

            }
            else
            {
                m_caretReqAlpha = 0;
                m_caretBlink = 0;
            }


            CaretAlpha = m_caretReqAlpha;//vxMathHelper.Smooth(CaretAlpha, ReqCaretAlpha, 4);

            //Make sure the cursor index doesn't go past the text length
            CaretIndex = MathHelper.Clamp(CaretIndex, 0, Text.Length);

        }


        public void ShowInputDialog(string title, string text, string initialText, Action<string> callback)
        {
#if __IOS__
            ShowiOS(title, text, initialText, callback);
#elif __ANDROID__
            ShowAndroid(title, text, initialText, callback);
#endif
        }

        private void ShowAndroid(string title, string msg, string input, Action<string> callback)
        {
#if __ANDROID__
            var _view = vxEngine.Game.Services.GetService(typeof(Android.Views.View)) as Android.Views.View;

            Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(_view.Context);


            Android.Widget.EditText editText = new Android.Widget.EditText(_view.Context);
            editText.InputType = Android.Text.InputTypes.ClassText;
            editText.Text = input;

            Android.App.AlertDialog alert = dialog.Create();

            alert.SetView(editText);
            alert.SetTitle(title);
            alert.SetMessage(msg);
            alert.SetButton("Ok", (c, ev) =>
            {
                System.Diagnostics.Debug.WriteLine("OK");
                System.Diagnostics.Debug.WriteLine(editText.Text);

                callback.Invoke(editText.Text);
                // Ok button click task 
                //Console.WriteLine("Okay was clicked");
                //taskCompletionSource.SetResult(true);
            });
            alert.SetButton2("CANCEL", (c, ev) =>
            {
                //Console.WriteLine("Cancel was clicked");
                //taskCompletionSource.SetResult(false);
            });
            alert.Show();
#endif
        }


        private void ShowiOS(string title, string msg, string input, Action<string> callback)
        {
#if __IOS__

            var ViewController = vxEngine.Game.Services.GetService(typeof(UIKit.UIViewController)) as UIKit.UIViewController;
            UIKit.UIAlertController alert = UIKit.UIAlertController.Create(title, msg, UIKit.UIAlertControllerStyle.Alert);

            alert.AddAction(UIKit.UIAlertAction.Create("OK", UIKit.UIAlertActionStyle.Default, action =>
            {
                // This code is invoked when the user taps on login, and this shows how to access the field values
                Console.WriteLine("Text Set To: {0}", alert.TextFields[0].Text);
                callback.Invoke(alert.TextFields[0].Text);
            }));

            alert.AddAction(UIKit.UIAlertAction.Create("Cancel", UIKit.UIAlertActionStyle.Cancel, cancel => { }));
            alert.AddTextField((field) =>
            {
                //field.Placeholder = "Enter a Name";
                field.Text = input;
            });

            ViewController.PresentViewController(alert, animated: true, completionHandler: null);
#endif
        }


        public int DisplayTextStart
        {
            get { return _displayTextStart; }
            set
            {
                _displayTextStart = MathHelper.Clamp(value, 0, Text.Length);
            }
        }
        int _displayTextStart = 0;

        /// <summary>
        /// The lines of text.
        /// </summary>
        public string[] Lines;

        /// <summary>
        /// The index of the current line.
        /// </summary>
        public int CurrentLineIndex = 0;

        public string DisplayText
        {
            get { return _displayText; }
        }
        string _displayText;

        /// <summary>
        /// The text position offset.
        /// </summary>
        public Vector2 TextPositionOffset = new Vector2();

        void HandleTextAsyncCallback(IAsyncResult ar)
        {
            //Console.WriteLine(ar.IsCompleted);
            try
            {
#if __IOS__ || __ANDROID__
                string newText = "";// Guide.EndShowKeyboardInput(ar);
                if (newText != null)
                    Text = newText;
                
            IsGuideUp = false;
#endif
            }
            catch { }
            IsSelected = false;
        }

        public bool IsMultiLine = false;

        public override void Draw()
        {
            //Now get the Art Provider to draw the scene
            this.ArtProvider.DrawUIControl(this);

            base.Draw();
        }

        public override void OnLayoutInvalidated()
        {
            OnTextChanged();

            base.OnLayoutInvalidated();
        }


        protected void HandleCursorPosition()
        {
            string leftText = _displayText;

            if (ArtProvider != null)
            {
                float cursorHalfWidth = vxLayout.GetScaledWidth(ArtProvider.Font.MeasureString(ArtProvider.Caret).X/2);

                // clamp the cursor index
                CaretIndex = MathHelper.Clamp(CaretIndex, 0, Text.Length);

                leftText = Text.Substring(DisplayTextStart, CaretIndex - DisplayTextStart);
                CaretPosition.Y = Position.Y;
                CaretPosition.X = Position.X + ArtProvider.Font.MeasureString(leftText).X - cursorHalfWidth;
            }
        }

        protected void HandleCursorMultilinePosition()
        {
            string leftText = _displayText;
            if (ArtProvider != null)
            {
                int runningLength = 0;
                CurrentLineIndex = 0;
                for(int l = 0; l < Lines.Length; l++)
                {

                    if(CaretIndex <= runningLength + Lines[l].Length + Lines.Length)
                    {
                        CurrentLineIndex = l;                        
                        break;
                    }
                    else
                    {
                        runningLength += Lines[l].Length;
                    }
                }
                float cursorHalfWidth = vxLayout.GetScaledWidth(ArtProvider.Font.MeasureString(ArtProvider.Caret).X/2);

                // clamp the cursor index
                var lineCaretIndex = MathHelper.Clamp(CaretIndex - runningLength, 0, Lines[CurrentLineIndex].Length);

                leftText = Lines[CurrentLineIndex].Substring(0, lineCaretIndex);
                CaretPosition.Y = Position.Y + Font.LineSpacing * CurrentLineIndex;
                CaretPosition.X = Position.X + ArtProvider.Font.MeasureString(leftText).X - cursorHalfWidth;
            }
        }

        //int updateCount = 0;
        //bool hasDisplayTxtUpdated = false;

        //public List<string> lines = new List<string>();

        /// <summary>
        /// Fired whenerer the text changed (or a key is pressed).
        /// </summary>
        public override void OnTextChanged()
        {
            // Get Cursor Position

            // Fire the Text Change Event
            if (TextChanged != null)
                TextChanged(this, new EventArgs());

            // First set the Display text as the text
            _displayText = Text;

            HandleCursorPosition();


            // first, is the text wider than the width then chop display end values
            if (Width > 0)
            {
                if (IsMultiLine)
                {

                    Lines = Font.WrapStringToArray(Text, Width);
                    

                    _displayText = string.Empty;
                    foreach (var line in Lines)
                    {
                        _displayText += line + "\n";
                    }

                    HandleCursorMultilinePosition();
                }
                else
                {
                    //hasDisplayTxtUpdated = false;

                    // The Caret is beyond the right side
                    if (CaretPosition.X > Bounds.Right)
                    {
                        DisplayTextStart += 5;
                        //hasDisplayTxtUpdated = true;
                    }
                    else if (CaretPosition.X < Bounds.Left)
                    {
                        DisplayTextStart -= 10;
                        //hasDisplayTxtUpdated = true;
                    }

                    _displayText = Text.Substring(DisplayTextStart);

                    //Console.WriteLine((updateCount++) + " OnTextChanged");

                    // get the length that's visible
                    if (Font.MeasureString(_displayText).X > Width)
                    {
                        int txtLength = _displayText.Length;

                        //Console.WriteLine((updateCount) + " LONGER THAN TXT BX");

                        while (Font.MeasureString(_displayText).X > Width)
                        {
                            _displayText = Text.Substring(DisplayTextStart, txtLength--);
                        }
                    }

                    HandleCursorPosition();
                }
            }
        }

        bool NewKey = false;
        /// <summary>
        /// Hand keyboard input.
        /// </summary>
        /// <param name="dt"></param>
        public void ProcessKeyInputs(float dt)
        {
            NewKey = false;
            KeyboardState keyState = vxInput.KeyboardState;
            Keys[] keys = keyState.GetPressedKeys();

            bool shift = keyState.IsKeyDown(Keys.LeftShift) ||
                keyState.IsKeyDown(Keys.RightShift);

            foreach (Keys key in keys)
            {
                if (!IsKeyPressed(key, dt)) continue;

                char ch;
                if (KeyboardUtils.KeyToString(key, shift, out ch))
                {
                    // Handle typical character input.
                    m_text = Text.Insert(CaretIndex, new string(ch, 1));
                    CaretIndex++;
                }
                else
                {
                    switch (key)
                    {
                        case Keys.Back:
                            if (CaretIndex > 0)
                                m_text = Text.Remove(--CaretIndex, 1);
                            break;
                        case Keys.Delete:
                            if (CaretIndex < Text.Length)
                                m_text = Text.Remove(CaretIndex, 1);
                            break;
                        case Keys.Left:
                            if (CaretIndex > 0)
                                CaretIndex--;
                            break;
                        case Keys.Right:
                            if (CaretIndex < Text.Length)
                                CaretIndex++;
                            break;
                        case Keys.Up:
                            if (IsMultiLine)
                            {
                                if(CurrentLineIndex > 0)
                                    CaretIndex -= Lines[CurrentLineIndex].Length;
                            }
                            break;
                        case Keys.Down:
                            if (IsMultiLine)
                            {
                                CaretIndex += Lines[CurrentLineIndex].Length;
                            }
                            break;
                    }
                }
                NewKey = true;


            }

            // Raise the 'TextChanged' event if theres any keys pressed, this chatches
            // not only character additions or removals, but also cusor traversal
            if (NewKey)
                OnTextChanged();
        }

        /// <summary>
        /// Pressing check with key repeating.
        /// </summary>
        /// <returns><c>true</c> if this instance is key pressed the specified key dt; otherwise, <c>false</c>.</returns>
        /// <param name="key">Key.</param>
        /// <param name="dt">Dt.</param>
        bool IsKeyPressed(Keys key, float dt)
        {
            // Treat it as pressed if given key has not pressed in previous frame.
            if (vxInput.PreviousKeyboardState.IsKeyUp(key))
            {
                keyRepeatTimer = keyRepeatStartDuration;
                pressedKey = key;
                return true;
            }

            // Handling key repeating if given key has pressed in previous frame.
            if (key == pressedKey)
            {
                keyRepeatTimer -= dt;
                if (keyRepeatTimer <= 0.0f)
                {
                    keyRepeatTimer += keyRepeatDuration;
                    return true;
                }
            }

            return false;
        }
    }
}
