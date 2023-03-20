#region File Description
//-----------------------------------------------------------------------------
// MessageBoxScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
using VerticesEngine.UI.Themes;


#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using VerticesEngine.Input;
using VerticesEngine.Input.Events;
using VerticesEngine.UI.Controls;
using VerticesEngine.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace VerticesEngine.UI.MessageBoxs
{
    /// <summary>
    /// A popup message box screen, used to display "are you sure?"
    /// confirmation messages.
    /// </summary>
    public class vxMessageBox : vxBaseScene
    {
        #region Fields

        /// <summary>
        /// Main text for the Message box
        /// </summary>
        public string Message;

        /// <summary>
        /// Message Box Title
        /// </summary>
        public string Title;

        /// <summary>
        /// Message Box GUI Manager
        /// </summary>
        public vxUIManager InternalGUIManager;


		public vxButtonControl ApplyButton;
        public vxButtonControl OKButton;
        public vxButtonControl CancelButton;

		/// <summary>
		/// The given Art Provider of the Menu Entry. 
		/// </summary>
		public vxMessageBoxArtProvider ArtProvider { get; internal set; }

        /// <summary>
        /// Gets the bounds of the art providers bounding GUI rectangle.
        /// </summary>
        /// <value>The bounds.</value>
        public Vector2 Padding 
        {
            get { return ArtProvider.Padding; }
        }

        #endregion

        #region Events

		/// <summary>
		/// Occurs when apply. This can also act as a Miscelanous third button.
		/// </summary>
		public event EventHandler<PlayerIndexEventArgs> Apply;

		/// <summary>
		/// Occurs when accepted.
		/// </summary>
        public event EventHandler<PlayerIndexEventArgs> Accepted;

		/// <summary>
		/// Occurs when cancelled.
		/// </summary>
        public event EventHandler<PlayerIndexEventArgs> Cancelled;

        #endregion

        #region Initialization

		/// <summary>
		/// The button types for this Message Box.
		/// </summary>
		public vxEnumButtonTypes ButtonTypes;


        public bool IsCustomButtonPosition = false;

        /// <summary>
        /// Constructor lets the caller specify whether to include the standard
        /// "A=ok, B=cancel" usage text prompt.
        /// </summary>
		public vxMessageBox(string message, string title) :this(message, title, vxEnumButtonTypes.OkCancel)
        {
            
        }


		public vxMessageBox(string message, string title, vxEnumButtonTypes ButtonTypes)
		{
			this.Message = message;

			Title = title;

			IsPopup = true;

			TransitionOnTime = TimeSpan.FromSeconds(0.2);
			TransitionOffTime = TimeSpan.FromSeconds(0.2);

			this.ButtonTypes = ButtonTypes;
		}

		/// <summary>
		/// Sets the button text.
		/// </summary>
		public virtual void SetButtonText()
		{
            if (OKButton != null)
            {
                OKButton.SetLocalisedText(vxLocKeys.OK);
                CancelButton.SetLocalisedText(vxLocKeys.Cancel);
            }
		}

        public Rectangle Bounds
        {
            get { return ArtProvider.FormBounds; }
        }

        /// <summary>
        /// Loads graphics content for this screen. This uses the shared ContentManager
        /// provided by the Game class, so the content will remain loaded forever.
        /// Whenever a subsequent MessageBoxScreen tries to load this same content,
        /// it will just get back another reference to the already loaded data.
        /// </summary>
        public override void LoadContent()
        {
            this.ArtProvider = (vxMessageBoxArtProvider)vxUITheme.ArtProviderForMessageBoxes.Clone();

            InternalGUIManager = new vxUIManager();

            //Padding = ArtProvider.Padding;

            Vector2 viewportSize = new Vector2(vxGraphics.GraphicsDevice.Viewport.Width, vxGraphics.GraphicsDevice.Viewport.Height);

			//Setup Buttons
			ApplyButton = new vxButtonControl("Apply", new Vector2(viewportSize.X / 2 - 115, viewportSize.Y / 2 + 20));
			ApplyButton.Clicked += OnApplyButtonClicked;
			OKButton = new vxButtonControl("OK", new Vector2 (viewportSize.X / 2 - 115, viewportSize.Y / 2 + 20));
			OKButton.Clicked += OnOKButtonClicked;
            CancelButton = new vxButtonControl("Cancel", new Vector2(viewportSize.X / 2 + 15, viewportSize.Y / 2 + 20));
			CancelButton.Clicked += OnCancelButtonClicked;


            if (ButtonTypes != vxEnumButtonTypes.None)
            {
                if (ButtonTypes == vxEnumButtonTypes.OkApplyCancel)
                    InternalGUIManager.Add(ApplyButton);

                InternalGUIManager.Add(OKButton);

                if (ButtonTypes != vxEnumButtonTypes.Ok)
                    InternalGUIManager.Add(CancelButton);
            }

            ArtProvider.SetBounds();

            SetButtonText();
        }

        public override void UnloadContent()
        {
            ApplyButton.Clicked -= OnApplyButtonClicked;
            OKButton.Clicked -= OnOKButtonClicked;
            CancelButton.Clicked -= OnCancelButtonClicked;

            base.UnloadContent();
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Buttons the apply clicked.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public virtual void OnApplyButtonClicked (object sender, VerticesEngine.UI.Events.vxUIControlClickEventArgs e)
		{
			if (Apply != null)
				Apply(this, new PlayerIndexEventArgs(ControllingPlayer.Value));

			ExitScreen();
		}

		/// <summary>
		/// Raise the accepted event, then exit the message box.
		/// </summary>
		public virtual void OnOKButtonClicked (object sender, VerticesEngine.UI.Events.vxUIControlClickEventArgs e)
		{
			if (Accepted != null)
                Accepted(this, new PlayerIndexEventArgs(ControllingPlayer.Value));

			ExitScreen();
		}

		/// <summary>
		// Raise the cancelled event, then exit the message box.
		/// </summary>
		public virtual void OnCancelButtonClicked (object sender, VerticesEngine.UI.Events.vxUIControlClickEventArgs e)
		{
			OnCancel ();
		}


		public void OnCancel()
		{
			// Raise the cancelled event, then exit the message box.
			if (Cancelled != null)
				Cancelled(this, new PlayerIndexEventArgs(ControllingPlayer.Value));

			ExitScreen();
		}

        /// <summary>
        /// Responds to user input, accepting or cancelling the message box.
        /// </summary>
        protected internal override void HandleInput()
        {
            if (ButtonTypes != vxEnumButtonTypes.None)
            {
                if (vxInput.IsNewKeyPress(Keys.Enter))
                    OnOKButtonClicked(this, null);
                                
                if (vxInput.IsMenuCancel())
                    OnCancelButtonClicked(this, null);
            }

            if(vxInput.InputType == InputType.Controller)
            {
                if(vxInput.IsNewButtonPressed(Buttons.A))
                {
                    OnOKButtonClicked(this, null);
                }
                else if(vxInput.IsNewButtonPressed(Buttons.B))
                {
                    OnCancelButtonClicked(this, null);
                }
            }
        }

        #endregion

        protected internal override void Update()
        {
            base.Update();

            OnButtonPositionSet();

            //Update GUI Manager
            InternalGUIManager.Update();
        }

        public virtual void OnButtonPositionSet()
        {
            if (IsCustomButtonPosition == false)
            {
                Rectangle GUIBounds = ArtProvider.FormBounds;

                ApplyButton.Position = new Vector2(GUIBounds.Right, GUIBounds.Bottom)
                    - new Vector2(
                        ApplyButton.Width + OKButton.Width + CancelButton.Width + ArtProvider.Padding.X * 3,
                        ApplyButton.Height + ArtProvider.Padding.Y);

                int width = (ButtonTypes == vxEnumButtonTypes.Ok ? 0 : CancelButton.Width);

                OKButton.Position = new Vector2(GUIBounds.Right, GUIBounds.Bottom)
                    - new Vector2(
                        OKButton.Width + width + ArtProvider.Padding.X * 2,
                        OKButton.Height + ArtProvider.Padding.Y);

                CancelButton.Position = new Vector2(GUIBounds.Right, GUIBounds.Bottom)
                    - new Vector2(
                        CancelButton.Width + ArtProvider.Padding.X,
                        CancelButton.Height + ArtProvider.Padding.Y);
            }
        }

        #region Draw


        /// <summary>
        /// Draws the message box.
        /// </summary>
        public override void Draw()
        {
            vxInput.IsCursorVisible = true;
            // Darken down any other screens that were drawn beneath the popup.
            vxSceneManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            vxGraphics.SpriteBatch.Begin("UI - Message Box");
			ArtProvider.Draw(this);

            //Draw the GUI
            InternalGUIManager.Alpha = TransitionAlpha;
            InternalGUIManager.DrawByOwner();

            vxGraphics.SpriteBatch.End();

            base.Draw();

        }


		public virtual void SetArtProvider(vxMessageBoxArtProvider NewArtProvider)
		{
			ArtProvider = (vxMessageBoxArtProvider)NewArtProvider.Clone();
		}


        #endregion

        #region Static Code

        /// <summary>
        /// Shows a Message Box
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        public static vxMessageBox Show(string title, string text, vxEnumButtonTypes buttonTypes = vxEnumButtonTypes.Ok)
        {
            var msgBox = new vxMessageBox(text, title, buttonTypes);
            vxSceneManager.AddScene(msgBox);
            return msgBox;
        }

        #endregion
    }
}
