using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Utilities;
using VerticesEngine;
using VerticesEngine.UI.Events;
using Microsoft.Xna.Framework.Audio;
using VerticesEngine.UI.Themes;
using System.Collections.Generic;
using System.Reflection;
using VerticesEngine.Graphics;

namespace VerticesEngine.UI.Controls
{
    public class vxPropertyItemModelMesh : vxPropertyItemBaseClass
	{
        vxModelMesh Mesh;

        // TODO: Fix
        //public Texture2D DiffuseTexture
        //{
        //    get
        //    { return Mesh.Material.UtilityEffect.DiffuseTexture;
        //    }
        //}

        //public Texture2D NormapMap { 
        //    get{
        //        return Mesh.Material.UtilityEffect.NormalMap;
        //    }
        //}
        //public Texture2D SurfaceMap
        //{
        //    get
        //    {
        //        return Mesh.Material.UtilityEffect.SurfaceMap;
        //    }
        //}

        //public Texture2D DistortionMap
        //{
        //    get
        //    {
        //        Texture2D dMap = Mesh.Material.UtilityEffect.DistortionMap;
        //        return dMap == null ? vxInternalAssets.Textures.Blank : dMap;
        //    }
        //}


        public int PrimitiveCount
		{
			get { return Mesh.MeshParts.Count > 0 ? Mesh.MeshParts[0].TriangleCount : 0; }
		}


		//public bool HasDistortionMap
		//{
		//	get { return Mesh.Material.IsDistortionEnabled; }
		//}

        public vxPropertyItemModelMesh(vxPropertyGroup propertyGroup, PropertyInfo PropertyInfo, List<object> TargetObjects) :
        base(propertyGroup, PropertyInfo, TargetObjects)
		{
            //Mesh = (vxModelMesh)PropertyInfo.GetValue(TargetObject);
            GetPropertyValue();

            vxPropertyItemBaseClass PrimitiveCountControl;
            //vxPropertyItemBaseClass RenderAsGlassControl;
            //vxPropertyItemBaseClass HasDistortionMapControl;
            //vxPropertyItemTexture2D DiffuseTextureControl;
            //vxPropertyItemTexture2D NormapMapControl;
            //vxPropertyItemTexture2D SurfaceMapControl;
            //vxPropertyItemTexture2D DistortionMapControl;

            List<object> slctnst = new List<object>();
            slctnst.Add(this);

            PrimitiveCountControl = new vxPropertyItemBaseClass(propertyGroup, GetType().GetProperty("PrimitiveCount"), slctnst);
            //RenderAsGlassControl = new vxPropertyItemBaseClass(propertyGroup, GetType().GetProperty("RenderAsGlass"), slctnst);
            //HasDistortionMapControl = new vxPropertyItemBaseClass(propertyGroup, GetType().GetProperty("HasDistortionMap"), slctnst);
            //DiffuseTextureControl = new vxPropertyItemTexture2D(propertyGroup, GetType().GetProperty("DiffuseTexture"), slctnst);
            //NormapMapControl = new vxPropertyItemTexture2D(propertyGroup, GetType().GetProperty("NormapMap"), slctnst);
            //SurfaceMapControl = new vxPropertyItemTexture2D(propertyGroup, GetType().GetProperty("SurfaceMap"), slctnst);
            //DistortionMapControl = new vxPropertyItemTexture2D(propertyGroup, GetType().GetProperty("DistortionMap"), slctnst);

            Items.Add(PrimitiveCountControl);
            //Items.Add(RenderAsGlassControl);
            //Items.Add(HasDistortionMapControl);
            //Items.Add(DiffuseTextureControl);
            //Items.Add(NormapMapControl);
            //Items.Add(SurfaceMapControl);
            //Items.Add(DistortionMapControl);
		}


        public override object GetPropertyValue()
        {
            object result = base.GetPropertyValue();

            if (result is vxModelMesh)
            {
                Mesh = ((vxModelMesh)result);
                title = Mesh.Name;
            }
            else if (result is PropertyResponse)
                title = VARIES_TEXT;

            return result;
        }
        string title = "";

        public override string GetPropertyValueAsString()
        {
            return title;
        }
	}
}
