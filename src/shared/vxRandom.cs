using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine
{
    /// <summary>
    /// Random Value Generator
    /// </summary>
    public static class vxRandom
    {
        static Random random = new Random();

        internal static void Init(int seed)
        {
            random = new Random(seed);
            _seed = (seed ^ LARGE_PRIME) & ((1L << 48) - 1);
        }

        /// <summary>
        /// Returns a random float between 0.0 and 1.0
        /// </summary>
        /// <returns></returns>
        public static float GetRandomValue()
        {
            return (float)random.NextDouble();
        }
        public static Vector2 GetRandomVector2()
        {
            Vector2 randVec = new Vector2(GetRandomValue(), GetRandomValue());

            randVec.Normalize();

            return randVec;
        }

        public static Vector3 GetRandomVector3(bool isNormalised = true)
        {
            Vector3 randVec = new Vector3(GetRandomValue(-1, 1), GetRandomValue(-1, 1), GetRandomValue(-1, 1));

            if(randVec != Vector3.Zero && isNormalised)
                randVec.Normalize();

            return randVec;
        }


        /// <summary>
        /// Returns a random value between the min and max
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float GetRandomValue(int min, int max)
        {
            var r = (float)(random.Next(min * 100, max * 100));
            randSeedShift = r;

            if (randSeedShift == 0)
                randSeedShift = 1;

            //Console.WriteLine("Rand Shift: " + randSeedShift);
            //Console.WriteLine("GetRandomValue: " + r);
            return r / 100.0f;
        }

        static float randSeedShift = 1;


        /// <summary>
        /// Returns a random value from an enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetRandomEnum<T>() where T : IConvertible
        {
            // get a random int first
            int i = (int)GetRandomValue(1, Enum.GetNames(typeof(T)).Length);

            // now convert it to an onject
            return (T)Enum.ToObject(typeof(T), Math.Min(i, Enum.GetNames(typeof(T)).Length - 1));
        }


        public static Color GetRandomColour()
        {
            return new Color((int)GetRandomValue(0, 256), (int)GetRandomValue(0, 256), (int)GetRandomValue(0, 256));
        }


        public static int NextInt(int n)
        {
            if (n <= 0)
                throw new ArgumentOutOfRangeException("n", n, "n must be positive");

            if ((n & -n) == n)  // i.e., n is a power of 2
                return (int)((n * (long)next(31)) >> 31);

            int bits, val;

            do
            {
                bits = next(31);
                val = bits % n;
            } while (bits - val + (n - 1) < 0);
            return val;
        }

        private static int next(int bits)
        {
            _seed = (_seed * LARGE_PRIME + SMALL_PRIME) & ((1L << 48) - 1);
            return (int)(((uint)_seed) >> (48 - bits));
        }

        private static long _seed;

        private const long LARGE_PRIME = 0x5DEECE66DL;
        private const long SMALL_PRIME = 0xBL;
    }
}
