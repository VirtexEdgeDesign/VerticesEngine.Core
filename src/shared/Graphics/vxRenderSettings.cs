using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.Graphics
{
    public class vxRenderSettings : vxISelectable
    {
        public static vxRenderSettings Instance
        {
            get { return m_instance; }
        }
        private static vxRenderSettings m_instance = new vxRenderSettings();

        public string GetTitle()
        {
            return "Render Settings";
        }

        public Texture2D GetIcon(int w, int h)
        {
            return vxInternalAssets.Textures.DefaultDiffuse;
        }

        #region FXAA

        const string FXAACategory = "FXAA";
        //vxAntiAliasPostProcess m_antiAliasPostProcess;

        [vxRange(0.0001f, 3f)]
        [vxShowInInspector(FXAACategory)]
        public float Decayk
        {
            get { return m_sunlight.Decay; }
            set { m_sunlight.Decay = value; }
        }

        //[vxRange(0.0001f, 3f)]
        //[vxShowInInspector(FXAACategory)]
        //public float Density
        //{
        //    get { return m_sunlight.Density; }
        //    set { m_sunlight.Density = value; }
        //}

        //[vxRange(0.0001f, 3f)]
        //[vxShowInInspector(FXAACategory)]
        //public float Weight
        //{
        //    get { return m_sunlight.Weight; }
        //    set { m_sunlight.Weight = value; }
        //}

        #endregion

        #region Edge Detection

        [vxRange(0.0001f, 3f)]
        [vxShowInInspector("Edge Detection")]
        public float EdgeWidth
        {
            get { return m_edgeDetect.EdgeWidth; }
            set { m_edgeDetect.EdgeWidth = value; }
        }




        [vxRange(0.0001f, 50f)]
        [vxShowInInspector("Edge Detection")]
        public float EdgeIntensity
        {
            get { return m_edgeDetect.EdgeIntensity; }
            set { m_edgeDetect.EdgeIntensity = value; }
        }


        [vxRange(0.0001f, 1f)]
        [vxShowInInspector("Edge Detection")]
        public float NormalThreshold
        {
            get { return m_edgeDetect.NormalThreshold; }
            set { m_edgeDetect.NormalThreshold = value; }
        }


        [vxRange(0.0001f, 10f)]
        [vxShowInInspector("Edge Detection")]
        public float DepthThreshold
        {
            get { return m_depthThres; }
            set
            {
                m_depthThres = value;
                m_edgeDetect.DepthSensitivity = 0.00005f * m_depthThres;
            }
        }
        float m_depthThres = 1;


        [vxRange(0.0001f, 1000f)]
        [vxShowInInspector("Edge Detection")]
        public float NormalSensitivity
        {
            get { return m_edgeDetect.NormalSensitivity; }
            set { m_edgeDetect.NormalSensitivity = value; }
        }


        [vxRange(0.0001f, 1000f)]
        [vxShowInInspector("Edge Detection")]
        public float DepthSensitivity
        {
            get { return m_edgeDetect.DepthSensitivity; }
            set { m_edgeDetect.DepthSensitivity = value; }
        }

        vxEdgeDetectPostProcess m_edgeDetect;
        #endregion

        #region Crepuscular Rays

        const string GodRaysCategory = "Crepuscular Rays";
        vxSunLightPostProcess m_sunlight;

        [vxRange(0.0001f, 3f)]
        [vxShowInInspector(GodRaysCategory)]
        public float Decay
        {
            get { return m_sunlight.Decay; }
            set { m_sunlight.Decay = value; }
        }

        [vxRange(0.0001f, 3f)]
        [vxShowInInspector(GodRaysCategory)]
        public float Density
        {
            get { return m_sunlight.Density; }
            set { m_sunlight.Density = value; }
        }

        [vxRange(0.0001f, 3f)]
        [vxShowInInspector(GodRaysCategory)]
        public float Weight
        {
            get { return m_sunlight.Weight; }
            set { m_sunlight.Weight = value; }
        }

        #endregion

        public void Init(vxCamera camera)
        {
            m_edgeDetect = vxRenderPipeline.Instance.GetRenderingPass<vxEdgeDetectPostProcess>();

            // thin line anime style 
            if (m_edgeDetect != null)
            {
                m_edgeDetect.EdgeWidth = 0.69f;// 0.775f;
                m_edgeDetect.EdgeIntensity = 7;// 5;
                m_edgeDetect.NormalThreshold = 0.62f;// 0.5f;
                m_edgeDetect.DepthThreshold = 4.1f;
                m_edgeDetect.NormalSensitivity = 10f;
                m_edgeDetect.DepthSensitivity = 0.0001f;
            }

            m_sunlight = vxRenderPipeline.Instance.GetRenderingPass<vxSunLightPostProcess>();
        }
    }
}
