
using Microsoft.Xna.Framework;
using System;
using VerticesEngine.Input;
using VerticesEngine.Input.Events;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Events;

namespace VerticesEngine.UI.Dialogs
{
    /// <summary>
    /// Open File Dialog
    /// </summary>
    public class vxSandboxNewItemDialog : vxDialogBase
    {
        #region Fields

        public vxScrollPanel ScrollPanel;


        int CurrentlySelected = -1;

		//List<vxFileDialogItem> List_Items = new List<vxFileDialogItem>();

        float TimeSinceLastClick = 1000;

        int HighlightedItem_Previous = -1;

        public vxTabControl TabControl;

        vxGameplayScene3D Sandbox;

        #endregion

        #region Events

        //public new event EventHandler<PlayerIndexEventArgs> Accepted;
        public new event EventHandler<PlayerIndexEventArgs> Cancelled;

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Dialogs.vxSandboxNewItemDialog"/> class.
        /// </summary>
        /// <param name="Engine">Engine.</param>
        /// <param name="path">Path.</param>
        /// <param name="FileExtentionFilter">File extention filter.</param>
        public vxSandboxNewItemDialog(vxGameplayScene3D Sandbox)
			: base("Add New Item", vxEnumButtonTypes.OkCancel)
        {

            this.Sandbox = Sandbox;

            base.LoadContent();

            Rectangle tabBounds = ArtProvider.GUIBounds.GetBorder(-10);
            tabBounds.Height = tabBounds.Bottom - tabBounds.Top - OKButton.Height;
            TabControl = new vxTabControl(tabBounds);
            InternalGUIManager.Add(TabControl);
        }

        public override Vector2 GetBoundarySize()
        {
            return base.GetBoundarySize();
        }


        public override void Dispose()
        {
            //base.Dispose();
            this.IsRemoved = false;
        }

        public void OnSceneUnload()
        {
            base.Dispose();
            this.IsRemoved = true;
        }

        public override void UnloadContent()
        {
			//foreach (vxFileDialogItem fd in List_Items)
   //             fd.ButtonImage.Dispose();

            base.UnloadContent();
        }

        #endregion

        #region Handle Input


        protected override void OnOKButtonClicked(object sender, vxUIControlClickEventArgs e)
        {
            ExitScreen();
        }

        protected override void OnCancelButtonClicked(object sender, vxUIControlClickEventArgs e)
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
			// Handle Double Click
            if (vxInput.IsNewMouseButtonPress(MouseButtons.LeftButton))
            {
                if (TimeSinceLastClick < 20)
                {
                    if(CurrentlySelected == HighlightedItem_Previous)
            			OKButton.Select();
                }
                else
                {
                    TimeSinceLastClick = 0;
                }

                HighlightedItem_Previous = CurrentlySelected;
            }
        }

        #endregion

        protected internal override void Update()
        {
            base.Update();

            TimeSinceLastClick++;
        }

        #region Draw



		public void GetHighlitedItem(object sender, vxUIControlClickEventArgs e)
        {
			//foreach (vxFileDialogItem fd in List_Items)
   //         {
   //             fd.UnSelect();
   //         }
			//int i = e.GUIitem.Index;

   //         List_Items[i].ThisSelect();
            //CurrentlySelected = i;           
        }

        #endregion
    }
}