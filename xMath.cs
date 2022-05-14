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

        public static bool isInMandelbrot(float x, float y)
        {
            x = ((x-8) / 2048) + 0.26f;
            y = ((y-8) / 2048);

            float re = x;
            float im = y;

            Console.WriteLine(re + ", " + im);
            for (int iterCount = 0; iterCount < 1000; iterCount++)
            {
                float newRe = re * re - im * im;
                float newIm = 2 * re * im;
                re = newRe + x;
                im = newIm + y;
            }
            return (re * re + im * im > 0.001);
        }
    }
}