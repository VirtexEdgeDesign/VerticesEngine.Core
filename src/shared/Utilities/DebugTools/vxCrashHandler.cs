using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.IO;
using VerticesEngine.ContentManagement;
using VerticesEngine.Graphics;
using VerticesEngine.Utilities;

namespace VerticesEngine.Diagnostics
{
    /// <summary>
    /// The Crash Handler Game is a self contained game class which can be put in a 
    /// try-catch block at the initial entry point of the game. This allows for debug messages
    /// and crash output to be caught and shown on Releases and on non-easily-debugable systems (i.e. Mobile, Consoles etc...)
    /// </summary>
    public static class vxCrashHandler
	{
		/// <summary>
		/// The crash font.
		/// </summary>
		static SpriteFont CrashFont;

		/// <summary>
		/// The crash sprite batch.
		/// </summary>
		static SpriteBatch CrashSpriteBatch;

		/// <summary>
		/// The engine.
		/// </summary>
		static vxEngine Engine;

		/// <summary>
		/// The is initialised.
		/// </summary>
		public static bool IsInitialised = false;

		/// <summary>
		/// The text.
		/// </summary>
		static string text = "";

		/// <summary>
		/// The scrollpos.
		/// </summary>
		static float scrollpos = 0;

		/// <summary>
		/// The rect.
		/// </summary>
		static Rectangle rect;

        /// <summary>
        /// Is the crash handler enabled.
        /// </summary>
        public static bool IsEnabled = true;


        /// <summary>
        /// Initialise the Crash Handler with the specified engine and exception.
        /// </summary>
        /// <param name="engine">Engine.</param>
        /// <param name="exception">Exception.</param>
        public static void Thwrow(Exception exception)
        {
            Engine = vxEngine.Instance;

            Console.WriteLine("Message");
            Console.WriteLine(exception.Message);

            Console.WriteLine("StackTrace");
            Console.WriteLine(exception.StackTrace);


            //First Load the Debug Font
            //ContentManager CrashContentManager = new ContentManager(vxEngine.CurrentGame.Services);

            //CrashContentManager.RootDirectory = engine.InternalAssets;

			CrashFont = vxInternalContentManager.Instance.Load<SpriteFont>("Fonts/font_debug");

            CrashSpriteBatch = new SpriteBatch(vxEngine.Game.GraphicsDevice);

            vxEngine.Game.IsMouseVisible = true;

            IsInitialised = true;



            //Build the Error Message
            int scrnwidth = vxEngine.Game.GraphicsDevice.Viewport.Width - 10;
            int CharWidth = (int)CrashFont.MeasureString("O").X;

            int MAX_NUM_OF_CHARS_PER_LINE = scrnwidth / CharWidth - 5;

            string header = "";

            header = "\n";


            header += new string('/', MAX_NUM_OF_CHARS_PER_LINE) + "\n";
            header += " __   _____ ___ _____ ___ ___ ___ ___    ___ _  _  ___ ___ _  _ ___     ___ ___    _   ___ _  _    _  _   _   _  _ ___  _    ___ ___ \n";
            header += " \\ \\ / / __| _ \\_   _|_ _/ __| __/ __|  | __| \\| |/ __|_ _| \\| | __|   / __| _ \\  /_\\ / __| || |  | || | /_\\ | \\| |   \\| |  | __| _ \\\n";
            header += "  \\ V /| _||   / | |  | | (__| _|\\__ \\  | _|| .` | (_ || || .` | _|   | (__|   / / _ \\\\__ \\ __ |  | __ |/ _ \\| .` | |) | |__| _||   /\n";
            header += "   \\_/ |___|_|_\\ |_| |___\\___|___|___/  |___|_|\\_|\\___|___|_|\\_|___|   \\___|_|_\\/_/ \\_\\___/_||_|  |_||_/_/ \\_\\_|\\_|___/|____|___|_|_\\\n";

            header += "\n" + new string('/', MAX_NUM_OF_CHARS_PER_LINE) + "\n";


            text += "\nVERTICES ENGINE CRASH HANDLER - [ v 0.1.2 ] \n";
            text += new string('=', MAX_NUM_OF_CHARS_PER_LINE) + " \n\n";
            text += "WHY AM I SEEING THIS???\n";
            text += "----------------------------\n";
            text += "Despite our best efforts, it seems a bug did make it's way through our QA. Don't worry, this is ";
            text += "an in game catch which displays any and all crash information.\n\n";

            text += "\nWHAT CAN I DO???\n";
            text += "----------------------------\n";

            switch (vxEngine.GraphicalBackend)
            {
                case vxGraphicalBackend.OpenGL:
                    text += "To get outside of this screen, you can [alt] + [tab] out or press [Enter] to close the game. ";
                    break;
                case vxGraphicalBackend.iOS:
                case vxGraphicalBackend.Android:
                    text += "To get outside of this screen, you can simply close the app.";
                    break;
            }
			text +=	"The log's files for this crash can be found at '"+vxIO.LogDirectory+"'\n\n";

			text += "If this is a reoccuring issue, please contact either Virtex Edge Design (contact@virtexedgedesign.com) or the Game Vendor.\n";
            
			text += "\n\n\n\nTECHNICAL DATA\n";
			text += new string ('=', MAX_NUM_OF_CHARS_PER_LINE) + " \n";

			text += "\nERROR INFORMATION [ " + DateTime.Now.ToString() + " ]\n";
			text += "-------------------------------------------------------\n";
			text += string.Format("Game Name:             {0}\n",vxEngine.Game.Name);
			text += string.Format("Game Version:          {0}\n",vxEngine.Game.Version);
			text += "-\n";
			text += string.Format("Engine Version:        {0}\n",vxEngine.EngineVersion);
			text += string.Format("Engine Platform:       {0}\n",vxEngine.PlatformOS);
			text += string.Format("Engine Build Config:   {0}\n", vxEngine.BuildType);
			text += string.Format("CMD Line Args:         {0}\n", Engine.CMDLineArgsToString());
			text += "-\n";
			text += string.Format("Error Source Method:   {0}\n",exception.TargetSite);
			text += string.Format("Error Messge:          {0}\n",exception.Message);
			text += string.Format("Error Data:            {0}\n",exception.Data);



			//Writeout the Stack Trace
			text += "\n\nSTACK TRACE\n";
			text += new string ('-', MAX_NUM_OF_CHARS_PER_LINE)+"\n";
			text += exception.StackTrace;




            text += "\n\n\nINNER EXCEPTION\n";
            text += "-------------------------------------------------------\n";
                if (exception.InnerException != null)
            {

                text += "-------------------------------------------------------\n";
                text += string.Format("Error Source Method:   {0}\n", exception.InnerException.TargetSite);
                text += string.Format("Error Messge:          {0}\n", exception.InnerException.Message);
                text += string.Format("Error Data:            {0}\n", exception.InnerException.Data);



                //Writeout the Stack Trace
                text += "\n\nINNER EXCEPTION STACK TRACE\n";
                text += new string('-', MAX_NUM_OF_CHARS_PER_LINE) + "\n";
                text += exception.InnerException.StackTrace;
            }

			string endmsg = " END OF ERROR MESSAGE ";
			string sides = new string ('=', MAX_NUM_OF_CHARS_PER_LINE / 2 - endmsg.Length);
			text += "\n\n" + sides + endmsg + sides + "\n";

			//text = text.WrapMultilineStringBlock(MAX_NUM_OF_CHARS_PER_LINE);

			//Now add on the initial header
			//text = header + text;
			string writeresult = "";
			try
			{
                string CrashDir = Path.Combine(vxIO.LogDirectory, "Crash Logs");
				if (Directory.Exists (CrashDir) == false)
					Directory.CreateDirectory (CrashDir);

                string FileName = string.Format("{1}-{0}.txt", DateTime.Now.ToFileTimeUtc().ToString(), vxEngine.Game.Name + "-" + vxEngine.Game.Version+"-"+vxEngine.PlatformOS);

                string LogFilePath = Path.Combine(CrashDir,FileName);
				StreamWriter writer = new StreamWriter (LogFilePath);
				writer.Write (header + text);
				writer.Close ();

				writeresult = string.Format("[ CRASH LOG SAVED to '{0}' ]", LogFilePath);;
			}
			catch (Exception writeException) {
				writeresult = string.Format(">>>> NOTE: CRASH LOG COULD NOT BE WRITTEN! <<<<\nERROR: {0} ",writeException.Message);
			}
			writeresult = writeresult.WrapMultilineStringBlock(MAX_NUM_OF_CHARS_PER_LINE);

			//Finally wrap the text
			text = CrashFont.WrapString(text, scrnwidth);

			DisplayText = header + "\n" + writeresult + "\n" + text;


            // Lastly, the crash handler isn't enabled on all platforms, this will throw the original error

            if (IsEnabled == false)
                throw exception;

		}
        static string DisplayText="";

		static TouchCollection touchCollection;

		static Vector2 PreviousPosition;

		/// <summary>
		/// Draw the Crash Handler Screen.
		/// </summary>
		/// <param name="gameTime">Game time.</param>
		public static void Draw(GameTime gameTime)
		{
			if (IsInitialised == true) {
                

				//First Get input states
				touchCollection = TouchPanel.GetState();

					//Check if the game should exit
				//#if !__IOS__
				//	if (Keyboard.GetState ().IsKeyDown (Keys.Enter))
				//		vxEngine.CurrentGame.Exit ();
				//#endif

				// Handle Scrolling for Keyboard
					if (Keyboard.GetState ().IsKeyDown (Keys.PageUp))
						scrollpos -= (int)CrashFont.MeasureString ("A").Y;
					else if (Keyboard.GetState ().IsKeyDown (Keys.PageDown))
						scrollpos += CrashFont.MeasureString ("A").Y;

				//Handle Scrolling for Touch Controls
				if (touchCollection.Count > 0)
				{
					scrollpos += Vector2.Subtract(touchCollection[0].Position, PreviousPosition).Y;

					PreviousPosition = touchCollection[0].Position;
				}
				
				scrollpos = Math.Max (scrollpos, 0);

				string txt = ">> DON'T PANIC " + vxUtil.GetTextSpinner () + DisplayText + "\n\n";

				//Get Max Scroll Travel
				int maxheight = (int)CrashFont.MeasureString (txt).Y;

				//Get Height of Screen
				int scrnHeight = vxGraphics.GraphicsDevice.Viewport.Height;

				//Get the Max allowed travel
				int maxtravel = Math.Max(0, maxheight - scrnHeight);

				//Now set the scrollpos 
				scrollpos = Math.Min(scrollpos, maxtravel);
				int px = 5;
				int py = 5;
				rect = new Rectangle (
					vxEngine.Game.GraphicsDevice.Viewport.Width - 2 * px,
					py + (int)scrollpos,
					3,
					scrnHeight - maxtravel - 2 * py);

				float c = 0.1f;

				Color TextColor = Color.WhiteSmoke;

				//Draw Info
				//vxEngine.CurrentGame.GraphicsDevice.Clear (Color.Purple * 0.75f);
				vxEngine.Game.GraphicsDevice.Clear (new Color(c,c,c,1));
				CrashSpriteBatch.Begin ();
				CrashSpriteBatch.DrawString (CrashFont, txt,
					new Vector2(2 * px, 2 * py - scrollpos), TextColor * 1.2f);

				CrashSpriteBatch.Draw (vxInternalAssets.Textures.Blank,
					rect, TextColor);


				CrashSpriteBatch.Draw (vxInternalAssets.Textures.Blank,
					new Rectangle (
						vxEngine.Game.GraphicsDevice.Viewport.Width - 3 * px,
						py,
						1,
						scrnHeight - 2 * py), TextColor);

				CrashSpriteBatch.End ();

			}
		}
	}
}

