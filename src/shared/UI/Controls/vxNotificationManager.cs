using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VerticesEngine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using VerticesEngine.UI.Themes;

namespace VerticesEngine.UI.Controls
{
    /// <summary>
    /// Notification Manager, which allows cross platform notifications to be posted to the screen for the user, similar
    /// to the 'Toast' notifications in Android
    /// </summary>
    public static class vxNotificationManager
    {
        private static readonly List<vxNotification> _notifications = new List<vxNotification>();

        public static class Configs
        {
            public static bool IsOnBottom = true;

            public static SpriteFont NotificationFont = vxUITheme.Fonts.Size16;

            /// <summary>
            /// Time in seconds to show notifications
            /// </summary>
            public static float NotificationTime = 2;
        }

        /// <summary>
        /// Adds a new notification to the screen to be shown imediately. This is useful for passing custom notifications
        /// through to the notification system. If you want to show the generic notifications you can call 'Show(...)'
        /// </summary>
        /// <param name="notification"></param>
        public static void Add(vxNotification notification)
        {
            _notifications.Add(notification);
        }

        /// <summary>
        /// Shows a general notification to the screen
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="state"></param>
        public static void Show(string Text, Color state)
        {
            _notifications.Add(new vxNotification(Text, state));
        }
        public static void Show(string Text, Texture2D Icon)
        {
            _notifications.Add(new vxNotification(Text, Icon));
        }

        internal static void Update()
        {
            for (int n = 0; n < _notifications.Count; n++)
            {
                if (_notifications[n] != null)
                {
                    _notifications[n].Update();
                }
            }
        }

        /// <summary>
        /// Draws the GUI Item
        /// </summary>
        internal static void Draw()
        {
            for(int n = 0; n < _notifications.Count; n++)
            {
                if (_notifications[n] != null)
                {
                    _notifications[n].Draw();
                }
            }
        }
    }
}
