using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace VerticesEngine.Serilization
{
    /// <summary>
    /// This holds the Serializable data of a Texture2D
    /// </summary>
    public class vxSerializableTexture2D
    {
        //Importer Version
        [XmlAttribute("w")]
        public int Width;

        //Importer Version
        [XmlAttribute("h")]
        public int Height;

        [XmlElement("img")]
        public byte[] ImageData;


        public vxSerializableTexture2D()
        {

        }

        public vxSerializableTexture2D(Texture2D Texture)
        {
            Width = Texture.Width;
            Height = Texture.Height;
            //Texture.Format

            ImageData = Texture.ToByteArray();
        }

        public Texture2D ToTexture2D(GraphicsDevice device)
        {
            return ImageData.ToTexture2D(device, Width, Height);
        }
    }
}
