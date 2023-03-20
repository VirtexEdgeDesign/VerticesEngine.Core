using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;
using System.Diagnostics;
using VerticesEngine;
using VerticesEngine.UI;
using VerticesEngine.UI.MessageBoxs;
using VerticesEngine.UI.Themes;
using VerticesEngine.Graphics;

namespace VerticesEngine.UI.Menus
{
    /// <summary>
    /// This screen is feedback to the user that it is updating the
    /// localization content.
    /// </summary>
    public class vxLocalizationUpdateScreen : vxMessageBox
    {
        /// <summary>
        /// The key to update the localization with
        /// </summary>
        string localKey = "";


        // the timer to measure when to fire off the update
        float currentTimer = 0;

        // the timer limit
        float timerLimit = 0.5f;

        bool hasUpdated = false;

        /// <summary>
        /// Provides a UI for updating the Localizations and content
        /// </summary>
        /// <param name="newKey"></param>
        public vxLocalizationUpdateScreen(string newKey) : base("Updating Language Settings...", "Localization", vxEnumButtonTypes.None)
        {
            localKey = newKey;
        }


        protected internal override void Update()
        {
            base.Update();

            currentTimer += vxTime.DeltaTime;

            if(currentTimer > timerLimit && hasUpdated == false)
            {
                vxLocalizer.SetLocalization(localKey);
                vxSettings.Save();

                hasUpdated = true;
                OnCancel();
            }
        }
    }
}
