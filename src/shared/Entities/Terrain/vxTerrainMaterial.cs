using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine;
using VerticesEngine.Graphics;

namespace VerticesEngine.EnvTerrain
{
    public class vxTerrainMaterial : vxMaterial
    {
        float[] _toonThreshold = { 0.8f, 0.4f };
        float[] _toonBrightnessLevels = { 1.3f, 0.9f, 0.5f };

        public vxTerrainMaterial() : base(new vxShader(vxInternalAssets.Shaders.HeightMapTerrainShader))
        {
            RenderTechnique = "Terrain";
        }

        public override void Initalise()
        {
            base.Initalise();
            /*
                TxtrUVScale : Single
                textureSize : Single
                CursorPosition : Single
                CursorScale : Single
                CursorColour : Single
                world : Single
                wvp : Single
                maxHeight : Single
                CursorMap : Texture2D
                Texture01 : Texture2D
                Texture02 : Texture2D
                Texture03 : Texture2D
                Texture04 : Texture2D
             */
            //vxConsole.WriteLine(this.ToString());
            //foreach(var t in Shader.Parameters)
            //{
            //    System.Console.WriteLine(string.Format("{0} : {1}", t.Name, t.ParameterType));
            //}

            if (Shader.Parameters["TnThresholds"] != null)
                Shader.Parameters["TnThresholds"].SetValue(_toonThreshold);

            if (Shader.Parameters["TnBrightnessLevels"] != null)
                Shader.Parameters["TnBrightnessLevels"].SetValue(_toonBrightnessLevels);

            if (Shader.Parameters["LightDirection"] != null)
                Shader.Parameters["LightDirection"].SetValue(Vector3.One);

            if (Shader.Parameters["TxtrUVScale"] != null)
                Shader.Parameters["TxtrUVScale"].SetValue(16f);

            if (Shader.Parameters["textureSize"] != null)
                Shader.Parameters["textureSize"].SetValue(128f);
            //Shader.Parameters["CursorPosition"].SetValue(Vector2.Zero);
            //Shader.Parameters["CursorScale"].SetValue(1f);
            //Shader.Parameters["CursorColour"].SetValue(new Color(0f, 0.25f, 1f, 1f).ToVector4());

            if (Shader.Parameters["maxHeight"] != null)
                Shader.Parameters["maxHeight"].SetValue(48f);

            if (Shader.Parameters["randomMap"] != null)
                Shader.Parameters["randomMap"].SetValue(vxInternalAssets.Textures.RandomValues);

            //Shader.Parameters["CursorMap"].SetValue(vxTerrainManager.Instance.TextureBrush);
            //Shader.Parameters["Texture01"].SetValue(vxTerrainManager.Instance.Textures[0]);
            //Shader.Parameters["Texture02"].SetValue(vxTerrainManager.Instance.Textures[1]);
            //Shader.Parameters["Texture03"].SetValue(vxTerrainManager.Instance.Textures[2]);
            //Shader.Parameters["Texture04"].SetValue(vxTerrainManager.Instance.Textures[3]);

            //BumpMap = vxInternalContentManager.Instance.Load<Texture2D>("Models/water/waterbump");
            //BumpMap = vxInternalAssets.LoadInternal<Texture2D>("Models/water/water_plane_dm");
            //UVFactor = new Vector2(0.5f, 0.5f);
            //Shader.Parameters["ReflectionCube"].SetValue(vxEngine.Instance.GetCurrentScene<vxGameplayScene3D>().SkyBox.SkyboxTextureCube);
            //Shader.Parameters["fFresnelPower"].SetValue(20.0f);
            //Shader.Parameters["vDeepColor"].SetValue(Color.DeepSkyBlue.ToVector4() * 0.25f);
            //Shader.Parameters["vShallowColor"].SetValue(Color.DeepSkyBlue.ToVector4());
            //Shader.Parameters["fWaterAmount"].SetValue(0.0125f);

            //// Load the Distortion Map
            ////DistortionMap = vxInternalAssets.Textures.RandomValues;
            //DistortionMap = vxInternalAssets.LoadInternal<Texture2D>("Models/water/water_plane_dm");
            //IsShadowCaster = false;

            //Shader.Parameters["EmissiveColour"].SetValue(Color.TransparentBlack.ToVector4());

            //DistortionScale = 0.25f;
            //DoSSR = true;
            //DoAuxDepth = false;
            ////DoSSRReflection;
            //IsDistortionEnabled = true;

            //LightDirection = Vector3.Normalize(Vector3.One);

            //ShadowBrightness = 0.51f;

            //MaterialRenderPass = "Terrain";
        }

        
    }
}
