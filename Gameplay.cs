using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RicochetRobots
{
    public static class Gameplay
    {
        private static Vector2[] RobotPositions = new Vector2[4];

        // exists twice
        private static float tilecount = 16;

        public static void StartGame()
        {
            Random r = new Random();
            for (int i = 0; i < 4; i++)
            {
                RobotPositions[i] = new Vector2(r.Next((int)tilecount + 1), r.Next((int)tilecount + 1));
            }
        }

        private static Board board = new Board()
        {
            walls = new List<Wall>() {
                new Wall(new Vector2(4, 4), Rotation.UR),
                new Wall(new Vector2(12, 3), Rotation.DR),
                new Wall(new Vector2(5, 10), Rotation.DL),
                new Wall(new Vector2(14, 11), Rotation.UL),
            }
        };

        public static void Draw()
        {
            Renderer.DrawBoard(board);
            Renderer.DrawRobots(RobotPositions);
        }
    }
}
