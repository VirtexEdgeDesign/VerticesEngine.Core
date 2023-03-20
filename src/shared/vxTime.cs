using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine
{
    /// <summary>
    /// A static class which holds a number of different classes for handling time
    /// </summary>
    public static class vxTime
    {
        /// <summary>
        /// The amount of time in seconds elapsed since the last frame 
        /// </summary>
        public static float DeltaTime
        {
            get { return m_deltaTime; }
        }
        private static float m_deltaTime;
        
        /// <summary>
        /// The total amount of time in seconds elapsed since the game started.
        /// </summary>
        public static float TotalGameTime
        {
            get { return m_totalGameTime; }
        }
        private static float m_totalGameTime;

        public static float ActualTotalGameTime
        {
            get { return m_actualTotalGameTime; }
        }
        private static float m_actualTotalGameTime;

        /// <summary>
        /// The number of frames since the start of the game
        /// </summary>
        public static long FrameCount
        {
            get { return _frameCount; }
        }
        private static long _frameCount;

        /// <summary>
        /// The factor for what the ideal and current framerate is
        /// </summary>
        public static float FramerateFactor
        {
            get { return _framerateFactor; }
        }
        private static float _framerateFactor = 1;

        /// <summary>
        /// The target frame rate for this game
        /// </summary>
        public static float TargetFramerate = 1f / 60f;

        public static bool IsFixed = false;

        /// <summary>
        /// The current fps for this frame. 
        /// </summary>
        public static float Fps
        {
            get { return m_fps; }
        }
        public static float m_fps = 0;

        /// <summary>
        /// Updates the time variables internally in the engine
        /// </summary>
        /// <param name="gameTime"></param>
        internal static void Update(GameTime gameTime)
        {
            m_actualTotalGameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;// * (vxEngine.PlatformOS == vxPlatformOS.Windows && vxScreen.IsFullScreen == false ? 2 : 1);

            m_fps = 1 / m_actualTotalGameTime;

            IsFixed = (m_fps > 65 || m_fps < 50);
            
            m_deltaTime = IsFixed ? TargetFramerate : (float)gameTime.ElapsedGameTime.TotalSeconds;// * (vxEngine.PlatformOS == vxPlatformOS.Windows && vxScreen.IsFullScreen == false ? 2 : 1);
             
            
            m_totalGameTime = (float)gameTime.TotalGameTime.TotalSeconds;

            _framerateFactor = m_deltaTime / TargetFramerate;

            if (vxEngine.BuildType == vxBuildType.Debug)
            {
                vxConsole.WriteToScreen("_elapsedTime", m_deltaTime);
                vxConsole.WriteToScreen("_framerateFactor", _framerateFactor);
            }
            _frameCount++;
        }

        /// <summary>
        /// Relapse the elapsed game time to zero.
        /// </summary>
        public static void ResetElapsedTime()
        {
            vxEngine.Game.ResetElapsedTime();
        }
    }
}
