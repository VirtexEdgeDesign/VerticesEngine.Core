using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.UI.Themes;

namespace VerticesEngine.UI.Controls
{
    /// <summary>
    /// A notification ui control
    /// </summary>
    public class vxNotification : vxUIControl
    {

        /// <summary>
        /// Gets or sets the texture for this Menu Entry Background.
        /// </summary>
        /// <value>The texture.</value>
        public Texture2D BackgroundTexture;

		/// <summary>
		/// The icon for this button.
		/// </summary>
		public Texture2D Icon;



        #region Private Vars
               
        Color state;

        float count = 0;

        float Offset = 0;

        float ReqOffset = 1;

        #endregion

        /// <summary>
        /// Creates a new notification with text and a color for it's state
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="state"></param>
        public vxNotification(string Text, Color state) : base(Vector2.Zero)
        {
            //Text
            this.Text = Text;

            //Set up Font
            Font = vxUITheme.Fonts.Size24;

            DoBorder = true;

            this.state = state;

            Icon = DefaultTexture;
		}


        /// <summary>
        /// Creates a new notification with text and an icon
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="Icon"></param>
        public vxNotification(string Text, Texture2D Icon) : base(Vector2.Zero)
        {
            //Text
            this.Text = Text;

            //Set up Font
            Font = vxUITheme.Fonts.Size24;

            DoBorder = true;

            this.Icon = Icon;
            this.state = Color.White;
        }

        /// <summary>
        /// Draws the GUI Item
        /// </summary>
        public override void Draw()
        {
            //Now get the Art Provider to draw the scene

            base.Draw();

            Height = (int)(vxNotificationManager.Configs.NotificationFont.LineSpacing * 1.125f + vxScreen.SafeArea.Y);

            int border = 15;

            count += vxTime.DeltaTime;

            if (count > vxNotificationManager.Configs.NotificationTime)
                ReqOffset = -1;


            Offset = vxMathHelper.Smooth(Offset, ReqOffset, 8);

            if (vxNotificationManager.Configs.IsOnBottom)
            {

                Bounds = new Rectangle(Viewport.Width * 10 / border,
                                      Viewport.Height - Height,
                                       Viewport.Width / (100 - border * 2),
                                       Height);


                Bounds = new Rectangle(0,
                                       (int)(Viewport.Height - Height * Offset),
                                       Viewport.Width,
                                       Height);
            }
            else
            {


                Bounds = new Rectangle(0,
                                       (int)(Height * (Offset - 1)),
                                       Viewport.Width,
                                       Height);
            }


            var txtPos = new Vector2(Bounds.Location.X, Bounds.Bottom - (int)(vxNotificationManager.Configs.NotificationFont.LineSpacing * 1.125f));

            // draw base
            //SpriteBatch.Draw(DefaultTexture, vxLayout.GetRect(0, 0, Height + vxScreen.SafeArea.Y, Bounds.Height), new Color(new Vector3(0.15f)));
            SpriteBatch.Draw(DefaultTexture, Bounds, new Color(new Vector3(0.15f)));
            SpriteBatch.Draw(Icon, new Rectangle(txtPos.ToPoint() + new Point(4), new Point(24)), state);
            SpriteBatch.DrawString(vxNotificationManager.Configs.NotificationFont, Text, txtPos + new Vector2(36, 2), Color.White);
        }
        public static void Show(string Text, Color state)
        {
            vxNotificationManager.Show(Text, state);
        }
        public static void Show(string Text, Texture2D Icon)
        {
            vxNotificationManager.Show(Text, Icon);
        }

    }
}
