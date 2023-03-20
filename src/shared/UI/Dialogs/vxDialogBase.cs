using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using VerticesEngine;
using VerticesEngine.UI;
using VerticesEngine.Utilities;
using VerticesEngine.Input.Events;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Events;
using VerticesEngine.UI.Themes;
using VerticesEngine.Graphics;
using VerticesEngine.Input;

namespace VerticesEngine.UI.Dialogs
{
    /// <summary>
    /// A popup dialog scene.
    /// </summary>
    public class vxDialogBase : vxBaseScene
    {
        #region Fields

        /// <summary>
        /// The Dialog Title
        /// </summary>
        public string Title;


		/// <summary>
		/// The given Art Provider of the Menu Entry. 
		/// </summary>
		public vxDialogArtProvider ArtProvider { get; internal set; }


        /// <summary>
        /// The Internal GUI Manager so the Dialog Box can handle it's own items.
        /// </summary>
        public vxUIManager InternalGUIManager {get; set;}

        /// <summary>
        /// OK button UI control
        /// </summary>
        public vxButtonControl OKButton;

        /// <summary>
        /// Apply button UI control
        /// </summary>
        public vxButtonControl ApplyButton;

        /// <summary>
        /// Cancel button UI control
        /// </summary>
        public vxButtonControl CancelButton;


        /// <summary>
        /// Is the button position bottom right or placed manually
        /// </summary>
        public bool IsCustomButtonPosition = false;

        #endregion

        #region Events

        /// <summary>
        /// Called when the user clicks the 'OK' or 'Apply' button
        /// </summary>
        public event EventHandler<PlayerIndexEventArgs> Accepted;

        /// <summary>
        /// Called when the user clicks the 'Cancel' button
        /// </summary>
        public event EventHandler<PlayerIndexEventArgs> Cancelled;

        #endregion

        #region Initialization

        /// <summary>
        /// Buttons available in this dialog
        /// </summary>
        public readonly vxEnumButtonTypes ButtonTypes;

        /// <summary>
        /// Open a dialog with the given title and button types
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="ButtonTypes"></param>
		public vxDialogBase(string Title, vxEnumButtonTypes ButtonTypes)
        {
            this.Title = Title;

            this.ButtonTypes = ButtonTypes;

            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }
        #endregion

        public Vector2 viewportSize;



		/// <summary>
		/// Gets the size of the boundary.
		/// </summary>
		/// <returns>The boundary size.</returns>
		public virtual Vector2 GetBoundarySize()
		{
			return new Vector2(vxGraphics.GraphicsDevice.Viewport.Width, vxGraphics.GraphicsDevice.Viewport.Height);
		}

        /// <inheritdoc/>
        public override void LoadContent()
        {
            InternalGUIManager = new vxUIManager();

			this.ArtProvider = (vxDialogArtProvider)vxUITheme.ArtProviderForDialogs.Clone();

			//And just so that all is set up properly, resize anything based off of current resolution scale.
			ArtProvider.SetBounds();

			viewportSize = GetBoundarySize();

            ApplyButton = new vxButtonControl(vxLocKeys.Apply, new Vector2(viewportSize.X / 2 - 115, viewportSize.Y / 2 + 20));
            OKButton = new vxButtonControl(vxLocKeys.OK, new Vector2(viewportSize.X / 2 - 115, viewportSize.Y / 2 + 20));
            CancelButton = new vxButtonControl(vxLocKeys.Cancel, new Vector2(viewportSize.X / 2 + 15, viewportSize.Y / 2 + 20));

            ApplyButton.Clicked += OnApplyButtonClicked;
            OKButton.Clicked += OnOKButtonClicked;
            CancelButton.Clicked += OnCancelButtonClicked;


            if (ButtonTypes != vxEnumButtonTypes.None)
            {
                if (ButtonTypes == vxEnumButtonTypes.OkApplyCancel)
                    InternalGUIManager.Add(ApplyButton);

                InternalGUIManager.Add(OKButton);

                if (ButtonTypes != vxEnumButtonTypes.Ok)
                    InternalGUIManager.Add(CancelButton);
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            ApplyButton.Clicked -= OnApplyButtonClicked;
            OKButton.Clicked -= OnOKButtonClicked;
            CancelButton.Clicked -= OnCancelButtonClicked;
            InternalGUIManager.Dispose();
        }


        /// <summary>
        /// Called when the Dialogs Apply Button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnApplyButtonClicked(object sender, vxUIControlClickEventArgs e)
		{
			// Raise the accepted event.
			if (Accepted != null)
                Accepted(this, new PlayerIndexEventArgs(ControllingPlayer.Value));
		}

        /// <summary>
        /// Called when the Dialogs OK Button is Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnOKButtonClicked(object sender, vxUIControlClickEventArgs e)
		{
			// Raise the accepted event, then exit the dialog.
			OnApplyButtonClicked(sender, e);

			ExitScreen();

		}

        /// <summary>
        /// Called when the Dialogs Cancel button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnCancelButtonClicked(object sender, vxUIControlClickEventArgs e)
        {
			// Raise the cancelled event, then exit the message box.
			if (Cancelled != null)
				Cancelled(this, new PlayerIndexEventArgs(ControllingPlayer.Value));
			
            ExitScreen();
        }

        /// <summary>
        /// Sets the button positions
        /// </summary>
        protected virtual void OnButtonPositionSet()
        {
            if (IsCustomButtonPosition == false)
            {
                Rectangle GUIBounds = ArtProvider.GUIBounds;

                ApplyButton.Position = new Vector2(GUIBounds.Right, GUIBounds.Bottom + ArtProvider.ButtonBuffer)
                    - new Vector2(
                        ApplyButton.Width + OKButton.Width + CancelButton.Width + ArtProvider.Padding.X * 3,
                        ApplyButton.Height + ArtProvider.Padding.Y);

                OKButton.Position = new Vector2(GUIBounds.Right, GUIBounds.Bottom + ArtProvider.ButtonBuffer)
                    - new Vector2(
                        OKButton.Width + CancelButton.Width + ArtProvider.Padding.X * 2,
                        OKButton.Height + ArtProvider.Padding.Y);

                CancelButton.Position = new Vector2(GUIBounds.Right, GUIBounds.Bottom + ArtProvider.ButtonBuffer)
                    - new Vector2(
                        CancelButton.Width + ArtProvider.Padding.X,
                        CancelButton.Height + ArtProvider.Padding.Y);
            }
        }


        /// <summary>
        /// Responds to user input, accepting or cancelling the message box.
        /// </summary>
        protected internal override void HandleInput()
        {
            if (ButtonTypes != vxEnumButtonTypes.None)
            {
                if (vxInput.IsMenuCancel())
                    OnCancelButtonClicked(this, null);
            }
        }

        protected internal override void Update()
        {
            base.Update();

            foreach (vxUIControl item in InternalGUIManager.Items)
            {
                item.TransitionAlpha = TransitionAlpha;
                item.ScreenState = ScreenState;
            }

            OnButtonPositionSet();

            //Update GUI Manager
            if(IsActive)
                InternalGUIManager.Update();
        }

        #region Draw


        /// <summary>
        /// Draws the dialog
        /// </summary>
        public override void Draw()
        {
            // Darken down any other screens that were drawn beneath the popup.
            vxSceneManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            vxGraphics.SpriteBatch.Begin("UI - Dialog");

			//First Draw the Dialog Art Provider
			ArtProvider.Draw(this);

            //Draw the GUI
            InternalGUIManager.DrawByOwner();

            vxGraphics.SpriteBatch.End();
		}

        /// <summary>
        /// Set the art provider for this dialog. This can be useful if you'd like to provide custom rendering for a given dialog
        /// </summary>
        /// <param name="NewArtProvider">The new art provider to apply to this dialog</param>
		public virtual void SetArtProvider(vxDialogArtProvider NewArtProvider)
		{
			ArtProvider = (vxDialogArtProvider)NewArtProvider.Clone();
		}

        #endregion
    }
}
