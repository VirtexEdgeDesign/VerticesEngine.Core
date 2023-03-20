//using System;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VerticesEngine.Graphics.Animation
//{
//    /// <summary>
//    /// Controls playback of an Animation.
//    /// </summary>
//	public struct vxAnimationPlayer2D
//    {
//        //
//        /// <summary>
//        /// Gets the animation which is currently playing.
//        /// </summary>
//        public vxAnimationSprite2D Animation
//        {
//            get { return animation; }
//        }
//		vxAnimationSprite2D animation;

//        /// <summary>
//        /// Gets the index of the current frame in the animation.
//        /// </summary>
//        public int FrameIndex
//        {
//            get { return frameIndex; }
//        }
//        int frameIndex;

//        /// <summary>
//        /// The amount of time in seconds that the current frame has been shown for.
//        /// </summary>
//        private float time;


//		public bool DisposeAnimation;

//        /// <summary>
//        /// Gets a texture origin at the bottom center of each frame.
//        /// </summary>
//        public Vector2 Origin
//        {
//            get { return new Vector2(Animation.FrameWidth / 2.0f, Animation.FrameHeight); }
//        }

//        /// <summary>
//        /// Begins or continues playback of an animation.
//        /// </summary>
//		public void PlayAnimation(vxAnimationSprite2D animation)
//        {
//            // If this animation is already running, do not restart it.
//            if (Animation == animation)
//                return;

//            // Start the new animation.
//            this.animation = animation;
//            this.frameIndex = 0;
//            this.time = 0.0f;
//			DisposeAnimation = false;
//        }

//        /// <summary>
//        /// Advances the time position and draws the current frame of the animation.
//        /// </summary>
//        public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects)
//        {
//            if (Animation != null)
//			{

//			// Process passing time.
////			if (gameTime != null)
////				time += (float)gameTime.ElapsedGameTime.TotalSeconds;
//				time += 0.0167f;


//            while (time > Animation.FrameTime)
//            {
//                time -= Animation.FrameTime;

//				//First Check if it's finished
//				if (frameIndex == Animation.FrameCount - 1)
//					DisposeAnimation = true;

//                // Advance the frame index; looping or clamping as appropriate.
//                if (Animation.IsLooping)
//                {
//                    frameIndex = (frameIndex + 1) % Animation.FrameCount;
//                }
//                else
//                {
//                    frameIndex = Math.Min(frameIndex + 1, Animation.FrameCount - 1);
//                }
//            }

//            // Calculate the source rectangle of the current frame.
//            Rectangle source = new Rectangle(FrameIndex * Animation.Texture.Height, 0, Animation.Texture.Height, Animation.Texture.Height);

//            // Draw the current frame.
//            spriteBatch.Draw(Animation.Texture, position, source, Color.White, 0.0f, Origin, 1.0f, spriteEffects, 0.0f);
//			}
//        }
//    }
//}
