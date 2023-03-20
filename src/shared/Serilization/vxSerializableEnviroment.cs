using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using VerticesEngine;

namespace VerticesEngine.Serilization
{
    public class vxFogState
    {
        [XmlAttribute("IsEnabled")]
        public bool IsEnabled = true;

        [XmlElement("StartDistance")]
        public float StartDistance = 1000;

        [XmlElement("Thickness")]
        public float Thickness = 3500;

        [XmlAttribute("IsHeightEnabled")]
        public bool IsHeightEnabled = false;

        [XmlElement("Height")]
        public float Height = -25;

        [XmlElement("HeightDepth")]
        public float HeightDepth = 20;

        [XmlElement("HeightStart")]
        public float HeightStart = 100;

        [XmlElement("HeightThickness")]
        public float HeightThickness = 350;

        [XmlElement("Colour")]
        public Color Colour = new Color(0.0375f, 0.6f, 0.8f, 1);
    }

    public class vxSkyBoxState
    {
        [XmlElement()]
        public bool FlipX = false;
        [XmlElement()]
        public bool FlipY = false;
        [XmlElement()]
        public Color SkyColour1 = new Color(0.0375f, 0.6f, 0.8f, 1);
        [XmlElement()]
        public float SkyExp1 = 8.5f;
        [XmlElement()]
        public float SkyColourStrength1 = 0.01f;

        [XmlElement()]
        public Color SkyColour2 = new Color(0.89f, 0.96f, 1f, 0);
        [XmlElement()]
        public float SkyExp2 = 3.0f;
        [XmlElement()]
        public float SkyColourStrength2 = 0.7f;

        [XmlElement()]
        public Color SkyColour3 = new Color(0.15f, 0.175f, 0.2f, 0);
        [XmlElement()]
        public float SkyColourStrength3 = 1.0f;

        [XmlElement()]
        public float SkyIntensity = 1.0f;
        [XmlElement()]
        public Color SunColor = new Color(1f, 0.99f, 0.87f, 1f);
        [XmlElement()]
        public float SunIntensity = 2.0f;
        //[XmlElement()]
        //public float SunAlpha = 550;
        //[XmlElement()]
        //public float SunBeta = 1.0f;
        //[XmlElement()]
        //public Vector4 SunVector = new Vector4(0.269f, 0.615f, 0.740f, 0);

        [XmlElement()]
        public float SunSize = 1f;

        [XmlElement()]
        public float SunRotX = 0.750f;

        [XmlElement()]
        public float SunRotY = 0.750f;

        [XmlElement()]
        public float SunRotZ = 0.60f;
    }

    /// <summary>
    /// This holds the Serializable data of a Enviroment Effects (i.e. Time of Day, Fog, Water etc...)
    /// </summary>
    public class vxSerializableEnviroment
    {
        // Time of day for this scene
        [XmlAttribute("TimeOfDay")]
        public TimeOfDay TimeOfDay = TimeOfDay.Day;

        [XmlElement("SunRotation")]
        public Vector2 SunRotations = new Vector2(1,0);

        [XmlElement("Fog")]
        public vxFogState Fog;

        [XmlElement()]
        public vxSkyBoxState SkyBox;

        public vxSerializableEnviroment()
        {
            //Fog = new vxSerializableFogState();
            Fog = new vxFogState();
            SkyBox = new vxSkyBoxState();
        }
    }
}
