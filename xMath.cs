using Silk.NET.OpenGL;
using System;

namespace RicochetRobots
{
    public static class xMath
    {
        public static float DegreesToRadians(float degrees)
        {
            return MathF.PI / 180f * degrees;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="min">inclusive</param>
        /// <param name="max">inclusive</param>
        /// <returns></returns>
        public static int Wrap(int value, int min, int max)
        {
            if (value < min)
                value = max - (min - value - 1);
            if (value > max)
                value = min + (value - max - 1);
            return value;
        }
    }
}