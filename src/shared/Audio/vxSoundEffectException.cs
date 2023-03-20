using System;
using Microsoft.Xna.Framework.Audio;

using VerticesEngine.Utilities;

namespace VerticesEngine.Audio.Exceptions
{
	public class vxSoundEffectException : Exception
	{
		public vxSoundEffectException(object sender, SoundEffect SoundEffect, Exception inner) :
		base((SoundEffect != null ? "Error Playing Sound Effect: " + SoundEffect.Name : "Sound Effect is Null"), inner)
		{
			vxConsole.WriteLine("Error Playing Sound Effect from: " + sender);
		}

        public vxSoundEffectException(object sender, string SoundEffectKey, Exception inner) :
        base((SoundEffectKey != null ? "Error Playing Sound Effect: " + SoundEffectKey : "Sound Effect is Null"), inner)
        {
            vxConsole.WriteLine("Error Playing Sound Effect from: " + sender);
        }
	}
}
