using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Text;
using System.Diagnostics;
using VerticesEngine.Utilities;

namespace VerticesEngine.Diagnostics
{
    public static class vxProfiler
    {
        [vxEngineSettingsAttribute("Profiler.IsEnabled")]
        public static bool IsEnabled = false;

        [vxEngineSettingsAttribute("Profiler.FPSUpdateRate")]
        public static float UpdateRate = 0.5f;

        /// <summary>
        /// Profiler tags
        /// </summary>
        public static class Tags
        {
            public const string FRAME_UPDATE = "Update";
            public const string FRAME_DRAW = "Draw";
            public const string UI_UPDATE = "UI Update";
            public const string UI_DRAW = "UI Draw";
            public const string PHYSICS_UPDATE = "Physics";
        }

        internal static void Init()
        {
            FPS = 0;
            sampleFrames = 0;
            stopwatch = Stopwatch.StartNew();
            SampleSpan = TimeSpan.FromSeconds(UpdateRate);
        }

        #region -- FPS --

        /// <summary>
        /// Gets current Frames Per Second
        /// </summary>
        public static float FPS { get; internal set; }

        // Stopwatch for fps measuring.
        private static Stopwatch stopwatch;

        /// <summary>
        /// Gets/Sets FPS sample duration.
        /// </summary>
        public static TimeSpan SampleSpan;

        private static int sampleFrames;

        #endregion

        #region -- Time Ruler --

        /// <summary>
        /// The Colleciton of Timers
        /// </summary>
        public static Dictionary<object, vxDebugTimerGraphSet> TimerCollection = new Dictionary<object, vxDebugTimerGraphSet>();


        public static void RegisterMark(string markID, Color color)
        {
            TimerCollection.Add(markID, new vxDebugTimerGraphSet(markID, color));
        }

        public static void BeginMark(string mark)
        {
            if (TimerCollection.ContainsKey(mark) && IsEnabled)
                TimerCollection[mark].Start();
        }

        public static void EndMark(string mark)
        {
            if (IsEnabled && TimerCollection.ContainsKey(mark))
                TimerCollection[mark].Stop();
        }

        #endregion

        internal static void Update()
        {
            if (IsEnabled)
            {
                sampleFrames++;

                // Handle FPS here
                if (stopwatch != null)
                {
                    if (stopwatch.Elapsed > SampleSpan)
                    {
                        // Update FPS value and start next sampling period.
                        FPS = (float)sampleFrames / (float)stopwatch.Elapsed.TotalSeconds;

                        stopwatch.Reset();
                        stopwatch.Start();
                        sampleFrames = 0;

                    }
                }
            }
        }
    }
}
