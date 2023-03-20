using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace VerticesEngine.Serilization
{
    /// <summary>
    /// This holds the Serializable data of a Enviroment Effects (i.e. Time of Day, Fog, Water etc...)
    /// </summary>
    public class vxSerializableVersion
    {
        
        [XmlAttribute("major")]
        public int Major = 0;

		[XmlAttribute("minor")]
		public int Minor = 0;

		[XmlAttribute("build")]
		public int Build = 0;

		[XmlAttribute("revision")]
		public int Revision = 0;

        public vxSerializableVersion()
        {
            
        }


		public vxSerializableVersion(int major, int minor, int build, int revision)
		{
            Major = major;
            Minor = minor;
            Build = build;
            Revision = revision;
		}

		public vxSerializableVersion(Version version)
		{
			Major = version.Major;
			Minor = version.Minor;
			Build = version.Build;
			Revision = version.Revision;
		}


        public vxSerializableVersion(string version):this(new Version(version))
		{
			
		}

        public Version ToSystemVersion()
        {
            return new Version(Major, Minor, Build, Revision);
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}.{3}", Major, Minor, Build, Revision);

        }
    }
}
