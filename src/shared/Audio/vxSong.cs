using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.Audio
{
    public class vxSong
    {
        public Song Song { get; private set; }

        public string Title { get; private set; }

        public string Author { get; private set; }

        public vxSong(Song song, string title, string author)
        {
            Song = song;
            Title = title;
            Author = author;
        }
    }
}
