
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VerticesEngine.Graphics
{
    /// <summary>
    /// Base class for a 3D Light Entity used in Defferred Rendering.
    /// </summary>
    public class vxDirectionalLight : vxLightEntity
    {
		/// <summary>
		/// Direction Entity is facing.
		/// </summary>
		public Vector3 LightDirection;

		/// <summary>
		/// Initializes a new instance of the <see cref="VerticesEngine.Graphics.vxDirectionalLight"/> class.
		/// </summary>
		/// <param name="Engine">The Vertices Engine Reference.</param>
		/// <param name="LightDirection">Light direction.</param>
        public vxDirectionalLight(vxGameplayScene3D scene, Vector3 LightDirection, Color Colour)
			: base(scene, Vector3.Zero, LightType.Directional, Colour, 1, 2)
        {
			this.LightDirection = LightDirection;
        }



		public override void Draw(vxCamera3D Camera)
		{
			Effect directionalLightEffect = vxInternalAssets.PostProcessShaders.DrfrdRndrDirectionalLight;
			directionalLightEffect.Parameters["lightDirection"].SetValue(this.LightDirection);
			directionalLightEffect.Parameters["Color"].SetValue(this.Color.ToVector3());

			directionalLightEffect.Techniques[0].Passes[0].Apply();
			//Scene.Renderer.RenderQuad(Vector2.One * -1, Vector2.One);   
		}
    }
}