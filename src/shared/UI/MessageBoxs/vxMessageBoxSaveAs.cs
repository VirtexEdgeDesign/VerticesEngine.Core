//#region Using Statements
//using System;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.Graphics;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework.Input;
//using VerticesEngine.UI;
//using VerticesEngine.UI.MessageBoxs;
//using VerticesEngine.UI.Controls;
//using VerticesEngine;


//#endregion

//namespace VerticesEngine.UI.MessageBoxs
//{
//    /// <summary>
//    /// A popup message box screen, used to display "are you sure?"
//    /// confirmation messages.
//    /// </summary>
//	public class vxMessageBoxSaveAs : vxMessageBox 
//    {
//        #region Fields
        
//        public vxTextbox Textbox;
        
//        //File Name
//        string FileName = "";
        
//        #endregion
        
//        #region Initialization


//        /// <summary>
//        /// Constructor automatically includes the standard "A=ok, B=cancel"
//        /// usage text prompt.
//        /// </summary>
//		public vxMessageBoxSaveAs(string message, string title, string fileName)
//            : base(message, title)
//        {
//            FileName = fileName;
//            this.Message = message;

//            IsPopup = true;

//            TransitionOnTime = TimeSpan.FromSeconds(0.2);
//            TransitionOffTime = TimeSpan.FromSeconds(0.2);
//        }
                
//        /// <summary>
//        /// Loads graphics content for this screen. This uses the shared ContentManager
//        /// provided by the Game class, so the content will remain loaded forever.
//        /// Whenever a subsequent MessageBoxScreen tries to load this same content,
//        /// it will just get back another reference to the already loaded data.
//        /// </summary>
//        public override void LoadContent()
//        {
//            base.LoadContent();

//            OKButton.Text = "Save As";

//            //First Add Textbox
//            Textbox = new vxTextbox(FileName, Vector2.Zero,
//                                    "Save As", "Save the level as...",300);
            
            
//            InternalGUIManager.Add(Textbox);
//		}
//        protected override void OnFirstDraw()
//        {
//            base.OnFirstDraw();

//            Textbox.Position = ArtProvider.TextPosition + new Vector2(0, ArtProvider.TextSize.Y + Padding.Y);
//            Textbox.Width = Bounds.Width - 2 * (int)ArtProvider.Padding.X;
//            ArtProvider.ButtonBuffer = Textbox.Height;
//        }

//        #endregion
//    }
//}
