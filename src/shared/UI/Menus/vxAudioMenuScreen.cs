using Microsoft.Xna.Framework;
using VerticesEngine.Audio;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Events;

namespace VerticesEngine.UI.Dialogs
{
    /// <summary>
    /// The graphic settings dialog.
    /// </summary>
    public class vxAudioMenuScreen : vxDialogBase
    {
		// The previous values if the Cancel button is clicked.
		float prevMusicValue = 0;
		float prevSFXValue = 0;

        /// <summary>
        /// The Graphics Settings Dialog
        /// </summary>
        public vxAudioMenuScreen()
			: base("Audio Settings", vxEnumButtonTypes.OkCancel)
        {

        }


		/// <summary>
		/// Load graphics content for the screen.
		/// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            vxSettingsGUIItem.ArrowSpacing = 0;

            var ScrollPanel = new vxScrollPanel(new Vector2(
ArtProvider.GUIBounds.X + ArtProvider.Padding.X,
ArtProvider.GUIBounds.Y + ArtProvider.Padding.Y) + ArtProvider.PosOffset,
ArtProvider.GUIBounds.Width - (int)ArtProvider.Padding.X * 2,
ArtProvider.GUIBounds.Height - OKButton.Bounds.Height - (int)ArtProvider.Padding.Y * 4);
            InternalGUIManager.Add(ScrollPanel);

            this.Title = vxLocalizer.GetText(vxLocKeys.Settings_Audio);

			prevMusicValue = vxAudioManager.MusicVolume * 10;
			prevSFXValue = vxAudioManager.SoundEffectVolume * 10;

            
			//vxIncrementControl.ArrowStartOffset = 350;
			var MusicSettingItem = new vxSettingsGUIItem(InternalGUIManager, vxLocalizer.GetText(vxLocKeys.Settings_Audio_Music), prevMusicValue.ToString(), false);
			MusicSettingItem.ValueChangedEvent += delegate
            {
                vxAudioManager.MusicVolume = MathHelper.Clamp((float)MusicSettingItem.SelectedIndex / 10.0f, 0, 1);
                Microsoft.Xna.Framework.Media.MediaPlayer.Volume = MathHelper.Clamp((float)MusicSettingItem.SelectedIndex / 10.0f, 0, 1);
            };
            for (int i = 0; i <= 10; i++)
                MusicSettingItem.AddOption(i);

            ScrollPanel.AddItem(MusicSettingItem);



            var SndFXSettingItem = new vxSettingsGUIItem(InternalGUIManager, vxLocalizer.GetText(vxLocKeys.Settings_Audio_SoundEffects), prevSFXValue.ToString(), false);
			SndFXSettingItem.ValueChangedEvent += delegate
            {
                vxAudioManager.SoundEffectVolume = MathHelper.Clamp((float)SndFXSettingItem.SelectedIndex / 10.0f, 0, 1);
            };
            for (int i = 0; i <= 10; i++)
                SndFXSettingItem.AddOption(i);

            ScrollPanel.AddItem(SndFXSettingItem);
        }


        /// <inheritdoc/>
        protected override void OnOKButtonClicked(object sender, vxUIControlClickEventArgs e)
        {
            SetSettings();
            ExitScreen();
        }

        void SetSettings()
        {
            vxSettings.Save();
        }

        /// <inheritdoc/>
        protected override void OnCancelButtonClicked(object sender, vxUIControlClickEventArgs e)
		{

			vxAudioManager.MusicVolume = MathHelper.Clamp(prevMusicValue/10.0f, 0, 1);
			vxAudioManager.SoundEffectVolume = MathHelper.Clamp(prevSFXValue/ 10.0f, 0, 1);

			base.OnCancelButtonClicked(sender, e);
		}
    }
}
