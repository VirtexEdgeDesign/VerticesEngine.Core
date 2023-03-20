using System;

namespace VerticesEngine
{

    /// <summary>
    /// Properties with this Attribute will be shown in the Sandbox Properties Control
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = true)]
    public class vxRangeAttribute : Attribute
    {
        public float Min;

        public float Max;

        public float Tick;

        /// <summary>
        /// Properties with this Attribute will be shown in the Sandbox Properties Control
        /// </summary>
        /// <param name="category">The Category for this Property</param>
        /// <param name="description">Description for this property</param>
        /// <param name="isDebugOnly">Should this Property only be added when the engine is in debug mode</param>
        public vxRangeAttribute(float min, float max)
        {
            this.Max = max;
            this.Min = min;
            Tick = (Max - Min) / 100;
        }

        public vxRangeAttribute(float min, float max, float tick)
        {
            this.Max = max;
            this.Min = min;
            this.Tick = tick;
        }
    }
}
