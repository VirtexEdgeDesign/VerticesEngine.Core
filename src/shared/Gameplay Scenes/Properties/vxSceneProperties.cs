using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using VerticesEngine.Entities;

namespace VerticesEngine
{

    /// <summary>
    /// The scene properties object essentially exposes variables in the inspector
    /// </summary>
    public class vxSceneProperties : vxISelectable, IDisposable
    {
        public string GetTitle()
        {
            return "World Properties";
        }

        public Texture2D GetIcon(int w, int h)
        {
            return vxInternalAssets.Textures.Gradient;
        }

        [vxShowInInspector(vxInspectorCategory.BasicProperties, description: "The main light direction coming from the sun", IsReadOnly = true)]
        public int FileReversion
        {
            get { return Scene.SandBoxFile.FileReversion; }
        }

        #region - Lighting - 

        protected const string LightingTitle = "Lighting";

        [vxShowInInspector(LightingTitle, description: "The main light direction coming from the sun")]
        public TimeOfDay TimeOfDay
        {
            get { return Scene.SandBoxFile.Enviroment.TimeOfDay; }
            set { Scene.SandBoxFile.Enviroment.TimeOfDay = value; }
        }

        [vxShowInInspector(LightingTitle, description: "The main light direction coming from the sun")]
        public Vector3 LightDirection
        {
            get { return Scene.LightPositions; }
            set { Scene.LightPositions = value; }
        }

        #endregion

        #region - Sky and Sun - 

        protected const string SkyPropertiesTitle = "Sun and Sky";
        
        [vxShowInInspector(SkyPropertiesTitle)]
        public bool FlipX
        {
            get { return Scene.SandBoxFile.Enviroment.SkyBox.FlipX; }
            set { Scene.SandBoxFile.Enviroment.SkyBox.FlipX = value; }
        }

        [vxShowInInspector(SkyPropertiesTitle)]
        public bool FlipY
        {
            get { return Scene.SandBoxFile.Enviroment.SkyBox.FlipY; }
            set { Scene.SandBoxFile.Enviroment.SkyBox.FlipY = value; }
        }

        [vxShowInInspector(SkyPropertiesTitle)]
        public Color SkyColour1
        {
            get { return Scene.SandBoxFile.Enviroment.SkyBox.SkyColour1; }
            set { Scene.SandBoxFile.Enviroment.SkyBox.SkyColour1 = value; }
        }


        [vxShowInInspector(SkyPropertiesTitle)]
        public float SkyExp1
        {
            get { return Scene.SandBoxFile.Enviroment.SkyBox.SkyExp1; }
            set { Scene.SandBoxFile.Enviroment.SkyBox.SkyExp1 = value; }
        }

        [vxRange(0, 1)]
        [vxShowInInspector(SkyPropertiesTitle)]
        public float SkyColourStrength1
        {
            get { return Scene.SandBoxFile.Enviroment.SkyBox.SkyColourStrength1; }
            set { Scene.SandBoxFile.Enviroment.SkyBox.SkyColourStrength1 = value; }
        }

        [vxShowInInspector(SkyPropertiesTitle)]
        public Color SkyColour2
        {
            get { return Scene.SandBoxFile.Enviroment.SkyBox.SkyColour2; }
            set { Scene.SandBoxFile.Enviroment.SkyBox.SkyColour2 = value; }
        }


        [vxShowInInspector(SkyPropertiesTitle)]
        public float SkyExp2
        {
            get { return Scene.SandBoxFile.Enviroment.SkyBox.SkyExp2; }
            set { Scene.SandBoxFile.Enviroment.SkyBox.SkyExp2 = value; }
        }

        [vxRange(0, 1)]
        [vxShowInInspector(SkyPropertiesTitle)]
        public float SkyColourStrength2
        {
            get { return Scene.SandBoxFile.Enviroment.SkyBox.SkyColourStrength2; }
            set { Scene.SandBoxFile.Enviroment.SkyBox.SkyColourStrength2 = value; }
        }


        [vxShowInInspector(SkyPropertiesTitle)]
        public Color SkyColour3
        {
            get { return Scene.SandBoxFile.Enviroment.SkyBox.SkyColour3; }
            set { Scene.SandBoxFile.Enviroment.SkyBox.SkyColour3 = value; }
        }

        [vxRange(0, 1)]
        [vxShowInInspector(SkyPropertiesTitle)]
        public float SkyColourStrength3
        {
            get { return Scene.SandBoxFile.Enviroment.SkyBox.SkyColourStrength3; }
            set { Scene.SandBoxFile.Enviroment.SkyBox.SkyColourStrength3 = value; }
        }

        [vxShowInInspector(SkyPropertiesTitle)]
        public float SkyIntensity
        {
            get { return Scene.SandBoxFile.Enviroment.SkyBox.SkyIntensity; }
            set { Scene.SandBoxFile.Enviroment.SkyBox.SkyIntensity = value; }
        }

        //[vxShowInInspector(SkyPropertiesTitle)]
        public Color SunColor
        {
            get { return Scene.SandBoxFile.Enviroment.SkyBox.SunColor; }
            set { Scene.SandBoxFile.Enviroment.SkyBox.SunColor = value; }
        }

        //[vxShowInInspector(SkyPropertiesTitle)]
        public float SunIntensity
        {
            get { return Scene.SandBoxFile.Enviroment.SkyBox.SunIntensity; }
            set { Scene.SandBoxFile.Enviroment.SkyBox.SunIntensity = value; }
        }


        [vxRange(0.0001f, 3f)]
        [vxShowInInspector(SkyPropertiesTitle)]
        public float SunSize
        {
            get { return Scene.SandBoxFile.Enviroment.SkyBox.SunSize; }
            set { Scene.SandBoxFile.Enviroment.SkyBox.SunSize = value; }
        }

        //[vxRange(-vxMathHelper.PI, vxMathHelper.PI)]
        //[vxShowInInspector(SkyPropertiesTitle)]
        public float SunRotX
        {
            get { return Scene.SandBoxFile.Enviroment.SkyBox.SunRotX    ; }
            set { Scene.SandBoxFile.Enviroment.SkyBox.SunRotX = value; }
        }

        [vxRange(-vxMathHelper.PI, vxMathHelper.PI)]
        [vxShowInInspector(SkyPropertiesTitle)]
        public float SunRotY
        {
            get { return Scene.SandBoxFile.Enviroment.SkyBox.SunRotY; }
            set { Scene.SandBoxFile.Enviroment.SkyBox.SunRotY = value; }
        }

        [vxRange(-vxMathHelper.PI, vxMathHelper.PI)]
        [vxShowInInspector(SkyPropertiesTitle)]
        public float SunRotZ
        {
            get { return Scene.SandBoxFile.Enviroment.SkyBox.SunRotZ; }
            set { Scene.SandBoxFile.Enviroment.SkyBox.SunRotZ = value; }
        }


        protected const string EnviromentTitle = "Enviroment";

        [vxShowInInspector(EnviromentTitle, description: "Does this scene have fog")]
        public bool IsFogEnabled
        {
            get { return Scene.SandBoxFile.Enviroment.Fog.IsEnabled; }
            set { Scene.SandBoxFile.Enviroment.Fog.IsEnabled = value; }
        }

        [vxRangeAttribute(1, 10000)]
        [vxShowInInspector(EnviromentTitle, description: "The Fog Start Position")]
        public float FogStartPosition
        {
            get { return Scene.SandBoxFile.Enviroment.Fog.StartDistance; }
            set { Scene.SandBoxFile.Enviroment.Fog.StartDistance = value; }
        }

        [vxRangeAttribute(1, 10000)]
        [vxShowInInspector(EnviromentTitle, description: "The Fog Thickness")]
        public float FogThickness
        {
            get { return Scene.SandBoxFile.Enviroment.Fog.Thickness; }
            set { Scene.SandBoxFile.Enviroment.Fog.Thickness = value; }
        }

        [vxShowInInspector(EnviromentTitle, description: "The Fog Height Enabled")]
        public bool IsFogHeightEnabled
        {
            get { return Scene.SandBoxFile.Enviroment.Fog.IsHeightEnabled; }
            set { Scene.SandBoxFile.Enviroment.Fog.IsHeightEnabled = value; }
        }

        [vxRangeAttribute(-500, 500)]
        [vxShowInInspector(EnviromentTitle, description: "The Fog Height")]
        public float FogHeight
        {
            get { return Scene.SandBoxFile.Enviroment.Fog.Height; }
            set { Scene.SandBoxFile.Enviroment.Fog.Height = value; }
        }

        //[vxRangeAttribute(1, 250)]
        [vxShowInInspector(EnviromentTitle, description: "The Fog Height Depth")]
        public float FogHeightDepth
        {
            get { return Scene.SandBoxFile.Enviroment.Fog.HeightDepth; }
            set { Scene.SandBoxFile.Enviroment.Fog.HeightDepth = value; }
        }

        [vxShowInInspector(EnviromentTitle, description: "The Fog Height Start Depth")]
        public float FogHeightStart
        {
            get { return Scene.SandBoxFile.Enviroment.Fog.HeightStart; }
            set { Scene.SandBoxFile.Enviroment.Fog.HeightStart = value; }
        }

        [vxRangeAttribute(1, 10000)]
        [vxShowInInspector(EnviromentTitle, description: "The Fog Height Thickness")]
        public float FogHeightThickness
        {
            get { return Scene.SandBoxFile.Enviroment.Fog.HeightThickness; }
            set { Scene.SandBoxFile.Enviroment.Fog.HeightThickness = value; }
        }

        [vxShowInInspector(EnviromentTitle, description: "The Fog Colour")]
        public Color FogColour
        {
            get { return Scene.SandBoxFile.Enviroment.Fog.Colour; }
            set { Scene.SandBoxFile.Enviroment.Fog.Colour = value; }
        }


        #endregion

        private vxGameplayScene3D Scene;

        public vxSceneProperties(vxGameplayScene3D Scene)
        {
            this.Scene = Scene;
        }

        public virtual void Dispose()
        {
            Scene = null;
        }
    }
}
