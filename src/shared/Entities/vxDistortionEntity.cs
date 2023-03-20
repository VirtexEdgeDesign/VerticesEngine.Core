//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Linq;

//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
////Virtex vxEngine Declaration
//using VerticesEngine;
//using VerticesEngine.Utilities;
//using VerticesEngine.Cameras;
//using VerticesEngine.Graphics;
//using VerticesEngine.Scenes;

//namespace VerticesEngine.Entities
//{
//    /// <summary>
//    /// An entity which is set up specially for Distortion Effects. This is useful for
//    /// elements such as engine exhuast and glass panes.
//    /// </summary>
//    public class vxDistortionEntity : vxEntity3D
//    {
//        /// <summary>
//        /// The Models main diffuse texture.
//        /// </summary>
//        //public Texture2D DistortionMap
//        //{
//        //    set
//        //    {
//        //        _distortionMap = value;
//        //        if (Model != null)
//        //            foreach (var part in Model.ModelMain.Meshes.SelectMany(m => m.MeshParts))
//        //            {
//        //                part.Effect.Parameters["DisplacementMap"].SetValue(_distortionMap);
//        //            }

//        //        // Finally, if the second displacement map hasn't been set yet, then set it
//        //        SecondaryDisplacementMap = value;
//        //    }
//        //    get { return _distortionMap; }
//        //}
//        //Texture2D _distortionMap;


//		// The alpha value of the underlying model
//		public float Alpha
//		{
//			get { return _alpha; }
//			set
//			{
//				_alpha = value;
//				if (Model.ModelMain != null)
//					foreach (var part in Model.ModelMain.Meshes.SelectMany(m => m.MeshParts))
//					{
//						//if (part.Effect.Parameters["DistortionScale"] != null)
//						part.Effect.Parameters["Alpha"].SetValue(_alpha);
//					}
//			}
//		}
//		float _alpha = 1;


//        public Texture SecondaryDisplacementMap
//        {
//            set
//            {
//                _secondaryDisplacementMap = value;
//                if (Model != null)
//                    foreach (var part in Model.ModelMain.Meshes.SelectMany(m => m.MeshParts))
//                    {
//                        part.Effect.Parameters["SecondaryDisplacementMap"].SetValue(_secondaryDisplacementMap);
//                    }
//            }
//            get { return _secondaryDisplacementMap; }
//        }
//        Texture _secondaryDisplacementMap;


//        /// <summary>
//        /// The Texture OFfset
//        /// </summary>
//        public Vector2 DistortionMapUVOffset
//        {
//            get { return _textureUVOffset; }
//            set
//            {
//                _textureUVOffset = value;
//                if (Model.ModelMain != null)
//                    foreach (var part in Model.ModelMain.Meshes.SelectMany(m => m.MeshParts))
//                    {
//                        if (part.Effect.Parameters["DistortionMapUVOffset"] != null)
//                            part.Effect.Parameters["DistortionMapUVOffset"].SetValue(_textureUVOffset);
//                    }
//            }
//        }
//        Vector2 _textureUVOffset = Vector2.Zero;

//        public Vector2 DistortionMapUVOffset2
//        {
//            get { return _textureUVOffset2; }
//            set
//            {
//                _textureUVOffset2 = value;
//                if (Model.ModelMain != null)
//                    foreach (var part in Model.ModelMain.Meshes.SelectMany(m => m.MeshParts))
//                    {
//                        if (part.Effect.Parameters["DistortionMapUVOffset2"] != null)
//                            part.Effect.Parameters["DistortionMapUVOffset2"].SetValue(_textureUVOffset2);
//                    }
//            }
//        }
//        Vector2 _textureUVOffset2 = Vector2.Zero;


//        /// <summary>
//        /// The Distortion Scale
//        /// </summary>
//        //public float DistortionScale
//        //{
//        //    get { return _distortionScale; }
//        //    set
//        //    {
//        //        _distortionScale = value;
//        //        if (Model.ModelMain != null)
//        //            foreach (var part in Model.ModelMain.Meshes.SelectMany(m => m.MeshParts))
//        //            {
//        //                //if (part.Effect.Parameters["DistortionScale"] != null)
//        //                    part.Effect.Parameters["DistortionScale"].SetValue(_distortionScale);
//        //            }
//        //    }
//        //}
//        //float _distortionScale = 0.525f;

//        /// <summary>
//        /// If the distorter is used as glass, then it will reflect light back
//        /// </summary>
//        public bool IsGlass = false;

//        /// <summary>
//        /// Should the Shader use Two Displacement Maps
//        /// </summary>
//        public bool UseTwoDisplacementMaps
//        {
//            get { return _useTwoDisplacementMaps; }
//            set
//            {
//                _useTwoDisplacementMaps = value;
//				if (Model != null && Model.ModelMain != null)
//                    foreach (var part in Model.ModelMain.Meshes.SelectMany(m => m.MeshParts))
//                    {
//                        if (part.Effect.Parameters["UseTwoDisplacementMaps"] != null)
//                            part.Effect.Parameters["UseTwoDisplacementMaps"].SetValue(_useTwoDisplacementMaps);
//                    }
//            }
//        }
//        bool _useTwoDisplacementMaps = false;


//        /// <summary>
//        /// The distortion technique.
//        /// </summary>
//        public DistortionTechniques DistortionTechnique = DistortionTechniques.DisplacementMapped;

//        /// <summary>
//        /// The distortion blur.
//        /// </summary>
//        public bool DistortionBlur = true;

//        /// <summary>
//        /// A Distortion Entity
//        /// </summary>
//        /// <param name="Engine"></param>
//        /// <param name="model">The model to use for the distortions</param>
//        /// <param name="Position">Starting Position</param>
//        public vxDistortionEntity(vxGameplayScene3D scene, vxModel model, Texture2D distortionMap, Vector3 Position) :
//            base(scene, model, Position)
//        {
//            DistortionMap = distortionMap;
//            SecondaryDisplacementMap = distortionMap;

//            // Always Initialise with the Default Value.
//            DistortionScale = 0.015f;
            

//            UseTwoDisplacementMaps = false;

//            //Engine.Current3DSceneBase.Distorters.Add(this);

//            WorldTransform = Matrix.CreateRotationX(-MathHelper.PiOver2) * Matrix.CreateTranslation(Position);

//            WorldTransform = Matrix.CreateTranslation(Position);

//			if (Model != null && Model.ModelMain != null)
//				foreach (var part in Model.ModelMain.Meshes.SelectMany(m => m.MeshParts))
//				{
//					//if (part.Effect.Parameters["Alpha"] != null)
//						part.Effect.Parameters["Alpha"].SetValue(1.0f);
//				}

//            if(Model != null)
//            foreach (var mesh in Model.Meshes)
//            {
//                mesh.UtilityEffect.DistortionMap = this.DistortionMap;
//                    //mesh.UtilityEffect.ReflectionMap = this.DistortionMap;
//                }
//            //InitShaders();
//        }

//        public override void Dispose()
//        {
//            //Engine.Current3DSceneBase.Distorters.Remove(this);
//            base.Dispose();
//        }

//        public override void RenderToShadowMap(int ShadowMapIndex)
//        {
//            if (IsGlass)
//                base.RenderToShadowMap(ShadowMapIndex);
//        }

//        public override void RenderPrepPass(vxCamera3D Camera)
//        {
//            if (IsGlass)
//                base.RenderPrepPass(Camera);
//        }

//        public override void RenderMesh(vxCamera3D Camera)
//        {
//            if (IsGlass)
//                base.RenderMesh(Camera);
//        }

//        public virtual void DrawModelDistortion(vxCamera3D Camera)
//        {
//            //RenderMask();
//			 if (Model != null)
//			{
//				// draw the distorter
//				Matrix worldView = WorldTransform * Camera.View;


//				if (vxEngine.BuildType == vxBuildType.Debug)
//					Engine.GlobalPrimitiveCount += Model.TotalPrimitiveCount;

//				// make sure the depth buffering is on, so only parts of the scene
//				// behind the distortion effect are affected
//				Engine.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

//				foreach (ModelMesh mesh in Model.ModelMain.Meshes)
//				{
//					foreach (Effect effect in mesh.Effects)
//					{
//						effect.CurrentTechnique =
//							effect.Techniques[DistortionTechnique.ToString()];

//						effect.Parameters["World"].SetValue(WorldTransform);
//						effect.Parameters["WorldView"].SetValue(worldView);
//						effect.Parameters["WorldViewProjection"].SetValue(worldView *
//							Camera.Projection);

//						//effect.Parameters["DisplacementMap"].SetValue(DistortionMap);

//						//effect.Parameters["offset"].SetValue(0);
//						//effect.Parameters["DepthMap"].SetValue(RenderTargets.RT_DepthMap);

//						//effect.Parameters["DistortionScale"].SetValue(DistortionScale);
//						//effect.Parameters["Time"].SetValue((float)gameTime.TotalGameTime.TotalSeconds);
//					}
//					mesh.Draw();
//				}
//			}
//        }
//    }
//}