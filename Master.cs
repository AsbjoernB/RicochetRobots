using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System;
using System.Numerics;

namespace RicochetRobots
{
    public class Master
    {
        public static IWindow window;
        public static Gameplay gameplay;

        public static readonly float tilecount = 16;
        public static readonly float sidebarwidth = 8;
        public static readonly float tilepixels = 55;

        // entry point
        private static void Main(string[] args)
        {
            Renderer.Init();
            gameplay = new SpeedrunGameplay();

            window.Run();
            Console.Write("hvis du ser det her og vinduet stadig ikke er åbnet har jeg ingen ide");
            Console.ReadLine();
        }
    }
}