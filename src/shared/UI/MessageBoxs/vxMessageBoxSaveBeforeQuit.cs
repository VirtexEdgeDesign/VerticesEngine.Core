#region Using Statements


#endregion

namespace VerticesEngine.UI.MessageBoxs
{
    /// <summary>
    /// A popup message box screen, used to display "are you sure?"
    /// confirmation messages.
    /// </summary>
    public class vxMessageBoxSaveBeforeQuit : vxMessageBox
	{
		

		#region Initialization


		/// <summary>
		/// Constructor automatically includes the standard "A=ok, B=cancel"
		/// usage text prompt.
		/// </summary>
		public vxMessageBoxSaveBeforeQuit(string message, string title)
			: base(message, title, vxEnumButtonTypes.OkApplyCancel)
		{
			
		}

		public override void SetButtonText()
		{
            if (ApplyButton != null)
            {
                ApplyButton.Text = "Save";
                OKButton.Text = "Don't";
                CancelButton.SetLocalisedText(vxLocKeys.Cancel);
            }
		}

		/// <summary>
		/// Loads graphics content for this screen. This uses the shared ContentManager
		/// provided by the Game class, so the content will remain loaded forever.
		/// Whenever a subsequent MessageBoxScreen tries to load this same content,
		/// it will just get back another reference to the already loaded data.
		/// </summary>
		public override void LoadContent()
		{
			base.LoadContent();

            SetButtonText();
		}
		#endregion
	}
}
