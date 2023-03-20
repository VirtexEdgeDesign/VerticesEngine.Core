
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Graphics;

namespace VerticesEngine
{

    public class vxDistortionEntity2D : vxEntity2D
    {
        /// <summary>
        /// The Distortion Scale
        /// </summary>
        public float DistortionScale = 0.525f;


        /// <summary>
        /// The distortion technique.
        /// </summary>
        public DistortionTechniques DistortionTechnique = DistortionTechniques.PullIn;

        /// <summary>
        /// The distortion blur.
        /// </summary>
        public bool DistortionBlur = true;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:VerticesEngine.Entities.vxDistortionEntity2D"/> class.
		/// </summary>
        /// <param name="Scene">Scene.</param>
		/// <param name="distortionMap">Distortion map.</param>
		/// <param name="PhsyicsSim">Phsyics sim.</param>
        /// <param name="Position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
        public vxDistortionEntity2D(vxGameplayScene2D Scene, Texture2D distortionMap, Vector2 Position) :
        base(Scene, distortionMap, Position)
        {
			//Engine.Current2DSceneBase.Entities.Remove(this);
			Scene.DistortionEntities.Add(this);
        }


		/// <summary>
		/// Draws the distortion.
		/// </summary>
		public virtual void DrawDistortion()
		{
			vxGraphics.SpriteBatch.Draw(Texture,
				Position,
				null,
				Color.White * Alpha * 0.505f,
				Rotation,
			                        new Vector2(Texture.Width/2,Texture.Height / 2),
				1f,
				SpriteEffect,
				LayerDepth);
		}
    }
}