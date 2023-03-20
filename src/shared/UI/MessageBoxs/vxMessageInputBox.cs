#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using VerticesEngine.UI;
using VerticesEngine.UI.MessageBoxs;
using VerticesEngine.UI.Controls;
using VerticesEngine;
using VerticesEngine.UI.Events;


#endregion

namespace VerticesEngine.UI.MessageBoxs
{

    /// <summary>
    /// A popup message box which can take user input
    /// </summary>
	public class vxMessageInputBox : vxMessageBox 
    {
        #region Fields
        
        private vxTextbox m_textbox;
        
        /// <summary>
        /// Player Input Text
        /// </summary>
        public string PlayerInputText
        {
            get { return m_textbox.Text; }
        }
        


        private string m_initialText = "";
        
        #endregion
        
        #region Initialization


        /// <summary>
        /// A popup message box that takes user input
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="initialText"></param>
		private vxMessageInputBox(string title, string message, string initialText)
            : base(message, title)
        {
            m_initialText = initialText;
            this.Message = message;

            IsPopup = true;
        }


        public override void LoadContent()
        {
            base.LoadContent();

            OKButton.Text = "OK";

            // add user input textbox
            m_textbox = new vxTextbox(m_initialText, Vector2.Zero, Title, Message,300);            
            
            InternalGUIManager.Add(m_textbox);
		}
        protected override void OnFirstDraw()
        {
            base.OnFirstDraw();

            m_textbox.Position = ArtProvider.TextPosition + new Vector2(0, ArtProvider.TextSize.Y + Padding.Y);
            m_textbox.Width = Bounds.Width - 2 * (int)ArtProvider.Padding.X;
            ArtProvider.ButtonBuffer = m_textbox.Height;
        }

        #endregion

        /// <summary>
        /// Shows a Message Box
        /// </summary>
        /// <param name="text"></param>
        /// <param name="title"></param>
        public static void Show(string title, string text, string initialText, Action<string> callback)
        {
#if __IOS__
            ShowiOS(title, text, initialText, callback);
#elif __ANDROID__
            ShowAndroid(title, text, initialText, callback);
#else
            var msgBox = new vxMessageInputBox(title, text, initialText);
            vxSceneManager.AddScene(msgBox);
            msgBox.Accepted += (sender, e) =>
            {
                callback(msgBox.PlayerInputText);
            };
#endif
        }


        private static void ShowiOS(string title, string msg, string input, Action<string> callback)
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


        private static void ShowAndroid(string title, string msg, string input, Action<string> callback)
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
    }
}
