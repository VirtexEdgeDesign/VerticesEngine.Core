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
    struct vxSoundEffectInfo
    {
        public object key;
        public float Volume;
        public float Pitch;
        public bool UseTransitionAlpha;
        public object sender;

        public vxSoundEffectInfo(object sender, object key, float Volume = 1, float Pitch = 0, bool UseTransitionAlpha = true)
        {
            this.sender = sender;
            this.key = key;
            this.Volume = Volume;
            this.Pitch = Pitch;
            this.UseTransitionAlpha = UseTransitionAlpha;
        }
    }

    /*
    /// <summary>
    /// This is the man sound effect manager for the Vertices Engine. Load Sound effects with a specefied 
    /// key at load and then play them by calling 'PlaySound(...)'.
    /// </summary>
    public static class vxAudioManager
    {


        public static void Update()
        {
           
        }
    }
    */
}
