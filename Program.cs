using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System;
using System.Numerics;

namespace RicochetRobots
{
    public class Program
    {
        public static IWindow window;

        private static void Main(string[] args)
        {
            Renderer.Init();
            Gameplay.Init();

            window.Run();
        }
    }
}