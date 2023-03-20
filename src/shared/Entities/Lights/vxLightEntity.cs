
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VerticesEngine.Graphics
{
    public enum LightType
	{
		Point,
		Directional
	}

	/// <summary>
	/// Base class for a 3D Light Entity used in Defferred Rendering.
	/// </summary>
	public class vxLightEntity
	{
		public LightType LightType;


		/// <summary>
		/// Location of Entity in world space.
		/// </summary>
		public Vector3 Position;

		/// <summary>
		/// The Light Colour.
		/// </summary>
		public Color Color;

		public float lightRadius;

		public float lightIntensity;

		/// <summary>
		/// Whether or not the light should fade out at a given distance defined by the 
		/// NearPlane and FarPlane variables in this class. This is to help with performance. 
		/// In some instsances though, it may be required or beneficial to keep the light visible always.
		/// In those cases, this variable should be set to 'false'.
		/// </summary>
		public bool FadeOnDistance = true;

		/// <summary>
		/// The Near Plane of when the Light will being to dim if it get's 
		/// further away.
		/// </summary>
		public float NearPlane = 50;

		/// <summary>
		/// The Far Plane of where after the light will not show.
		/// </summary>
		public float FarPlane = 200;

        public vxGameplayScene3D Scene;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:VerticesEngine.Graphics.vxLightEntity"/> class.
		/// </summary>
		/// <param name="Engine">Engine.</param>
		/// <param name="StartPosition">Start position.</param>
		/// <param name="LightType">Light type.</param>
		/// <param name="Colour">Colour.</param>
		/// <param name="lightRadius">Light radius.</param>
		/// <param name="lightIntensity">Light intensity.</param>
        public vxLightEntity(vxGameplayScene3D Scene, Vector3 StartPosition, LightType LightType, Color Colour, float lightRadius, float lightIntensity)
		{
            this.Scene = Scene;

            Scene.LightItems.Add(this);

			Position = StartPosition;

			this.Color = Colour;

			this.LightType = LightType;

			this.lightRadius = lightRadius;

			this.lightIntensity = lightIntensity;
            PointLightEffect = vxInternalAssets.PostProcessShaders.DrfrdRndrPointLight.Clone();
		}

		public virtual void Dispose()
		{
            Scene.LightItems.Remove(this);
		}

		public virtual void Update()
		{
            
		}

		public virtual void Draw(vxCamera3D Camera)
		{
			switch (LightType)
			{
				case LightType.Point:
                    //DrawPointLight(Position, Color, lightRadius, lightIntensity);
                    DrawPointLight(Camera);
					break;
			}
		}

		float GetDistanceFadeFactor(vxCamera3D Camera)
		{
			if (FadeOnDistance)
			{
				float length = Vector3.Subtract(Camera.WorldMatrix.Translation, Position).Length();

				// If the length is inside of the NearPlane, then return 1;
				if (length < NearPlane)
					return 1;

				// If it's not less than the NearPlane, then check if it's less than the FarPlane
				else if (length < FarPlane)
					return (FarPlane - length) / (FarPlane - NearPlane);

				// Finally, if it's greater than the far plane, return 0;
				else
					return 0;
			}
			else
				return 1;
		}
        Effect PointLightEffect;// = vxInternalAssets.PostProcessShaders.DrfrdRndrPointLight;
		//private void DrawPointLight(Vector3 lightPosition, Color color, float lightRadius, float lightIntensity)
        private void DrawPointLight(vxCamera3D Camera)
		{
				float blendFactor = GetDistanceFadeFactor(Camera);

				if (blendFactor > 0)
				{
					Effect pointLightEffect = vxInternalAssets.PostProcessShaders.DrfrdRndrPointLight;

					//compute the light world matrix
					//scale according to light radius, and translate it to light position
					pointLightEffect.Parameters["World"].SetValue(Matrix.CreateScale(lightRadius) * Matrix.CreateTranslation(Position));
					//light position
					pointLightEffect.Parameters["lightPosition"].SetValue(Position);


					//set the color, radius and Intensity
					pointLightEffect.Parameters["Color"].SetValue(Color.ToVector3());
					pointLightEffect.Parameters["lightRadius"].SetValue(lightRadius);
					pointLightEffect.Parameters["lightIntensity"].SetValue(lightIntensity * blendFactor);

					pointLightEffect.Parameters["View"].SetValue(Camera.View);
					pointLightEffect.Parameters["Projection"].SetValue(Camera.Projection);
					//parameters for specular computations
					pointLightEffect.Parameters["cameraPosition"].SetValue(Camera.Position);
					//pointLightEffect.Parameters["InvertViewProjection"].SetValue(Matrix.Invert(Camera.View * Camera.Projection));
                    pointLightEffect.Parameters["InvertViewProjection"].SetValue(Camera.InverseViewProjection);
					//calculate the distance between the camera and light center
					//float cameraToCenter = Vector3.Distance(Camera.Position, lightPosition);
					//if we are inside the light volume, draw the sphere's inside face

					//if (cameraToCenter < lightRadius)
					//	vxGraphics.GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;
					//else
					//	vxGraphics.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

						vxGraphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;

					pointLightEffect.Techniques[0].Passes[0].Apply();

					foreach (ModelMesh mesh in vxInternalAssets.Models.PointLightSphere.Meshes)
					{
						foreach (ModelMeshPart meshPart in mesh.MeshParts)
						{
							vxGraphics.GraphicsDevice.Indices = meshPart.IndexBuffer;
							vxGraphics.GraphicsDevice.SetVertexBuffer(meshPart.VertexBuffer);

                        //vxGraphics.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, meshPart.NumVertices, meshPart.StartIndex, meshPart.PrimitiveCount);
                        vxGraphics.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, meshPart.StartIndex, meshPart.PrimitiveCount);
                    }
					}
				}
			
		}
	}
}