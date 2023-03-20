using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;

using VerticesEngine;
using System.Collections.Generic;

using VerticesEngine.Utilities;
using VerticesEngine.Audio.Exceptions;


namespace VerticesEngine.Audio
{

    public class vxSoundEffectManager
    {
#if __MOBILE__
        const int MAX_NUMBER_OF_SOUNDEFFECTS_PLAYING = 30;
#else
        const int MAX_NUMBER_OF_SOUNDEFFECTS_PLAYING = 32;
#endif
        /// <summary>
        /// Is it safe to play a sound effect?
        /// </summary>
        public bool IsSafeToPlaySoundEffect
        {
            get { return _isPlayingSndFxOk; }
        }
        private bool _isPlayingSndFxOk = false;


        public static vxSoundEffectManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new vxSoundEffectManager();
                }
                return _instance;
            }
        }
        private static vxSoundEffectManager _instance;

        private vxSoundEffectManager() { }

        /// <summary>
        /// The list of the currently playing sound effects
        /// </summary>
        SoundEffectInstance[] currentlyPlayingSndFx = new SoundEffectInstance[512];

        /// <summary>
        /// The number of currently playign sound effects.
        /// </summary>
        public int CurrentPlayingCount
        {
            get { return _currentCount; }
        }
        int _currentCount = 0;

        /// <summary>
        /// Called when ever a sound effect is played to keep track of how many are being played currently
        /// </summary>
        /// <param name="instance"></param>
        public void OnSndEffectPlay(SoundEffectInstance instance)
        {
            currentlyPlayingSndFx[_currentCount] = instance;
            _currentCount++;
        }

        public void UpdatePlayingState()
        {
            int startCount = _currentCount;

            while (_currentCount > 0 && currentlyPlayingSndFx[_currentCount - 1].State != SoundState.Playing)
            {
                _currentCount--;
            }

            // loop through all snd fxs
            for (int i = 0; i < _currentCount; i++)
            {
                // if we aren't playing, then swap this out for the last item
                if (currentlyPlayingSndFx[i].State != SoundState.Playing)
                {
                    // loop down until we find a sndfx that's playing
                    currentlyPlayingSndFx[i] = currentlyPlayingSndFx[_currentCount - 1];
                    _currentCount--;
                    i--;
                }
            }

            _isPlayingSndFxOk = _currentCount < MAX_NUMBER_OF_SOUNDEFFECTS_PLAYING;
        }

        /// <summary>
        /// Stops all sound effects
        /// </summary>
        public void StopAll()
        {
            for (int i = 0; i < _currentCount; i++)
            {
                currentlyPlayingSndFx[i].Stop();
            }
        }
    }
}
