using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Silk.NET.Vulkan;
using Silk.NET.Input;

namespace RicochetRobots
{
    public static class Gameplay
    {
        private static Vector2[] RobotPositions = new Vector2[4];

        // rgby
        private static int selectedRobot = 0;
        
        // exists twice
        private static float tilecount = 16;

        public static void Init()
        {
            Program.window.Load += OnLoad;
        }

        public static void OnLoad()
        {
            IInputContext input = Program.window.CreateInput();
            for (int i = 0; i < input.Keyboards.Count; i++)
            {
                input.Keyboards[i].KeyDown += KeyDown;
            }
            StartGame();
        }

        public static void StartGame()
        {
            Random r = new Random();
            board = Board.GenerateBoard(r);
            for (int i = 0; i < 4; i++)
            {
                RobotPositions[i] = new Vector2(r.Next((int)tilecount), r.Next((int)tilecount));
            }
        }

        private static Board board;

        private static void MoveRobot(int x, int y)
        {
            Console.WriteLine("move");
            Vector2 dir = new Vector2(x, y);
            Vector2 pos = RobotPositions[selectedRobot];
            Vector2 nextpos;
            bool br = false;
            for (int i = 0; i < tilecount; i++)
            {
                nextpos = pos + dir;
                if (nextpos.X < 0 || nextpos.X > tilecount - 1 || nextpos.Y < 0 || nextpos.Y > tilecount - 1)
                    br = true;

                for (int r = 0; r < RobotPositions.Length; r++)
                {
                    if (RobotPositions[r].Equals(nextpos))
                        br = true;
                    
                }
                foreach (Wall wall in board.walls)
                {
                    if (wall.position.Equals(pos))
                    {
                        if (wall.wallDir.X == dir.X || wall.wallDir.Y == dir.Y)
                            br = true;
                    }
                    if (wall.position.Equals(nextpos))
                    {
                        if (wall.wallDir.X == -dir.X || wall.wallDir.Y == -dir.Y)
                            br = true;
                    }

                }

                if (br)
                    break;
                pos = nextpos;
            }
            RobotPositions[selectedRobot] = pos;
            
        }
        static int h = 0;
        public static void Draw()
        {
            DrawBoard();
            h++;
            h = xMath.Wrap(h, 0, 360);
            for (int i = 0; i < 4; i++)
            {
                Renderer.DrawTexture(TextureBank.Textures["robot"], RobotPositions[i], hueShift: Renderer.hueShift((RGBY)i));
            }
        }
        public static void DrawBoard()
        {
            for (int x = 0; x < tilecount; x++)
            {
                for (int y = 0; y < tilecount; y++)
                {
                    Renderer.DrawTexture(TextureBank.Textures["tile"], new Vector2(x, y));
                }
            }

            foreach (Wall wall in board.walls)
            {
                Renderer.DrawTexture(TextureBank.Textures["wall"], wall.position, (int)wall.rotation * 90, 2f, 0);
                if (wall.item != Item.none)
                    Renderer.DrawTexture(TextureBank.Textures[Enum.GetName(typeof(Item), wall.item)], wall.position, hueShift: Renderer.hueShift(wall.itemColor));
            }
        }

        private static void KeyDown(IKeyboard arg1, Key arg2, int arg3)
        {
            switch (arg2)
            {
                case (Key.Right):
                    MoveRobot(1, 0);
                    break;
                case (Key.Left):
                    MoveRobot(-1, 0);
                    break;
                case (Key.Up):
                    MoveRobot(0, 1);
                    break;
                case (Key.Down):
                    MoveRobot(0, -1);
                    break;

                case (Key.Number1):
                    selectedRobot = 0;
                    break;
                case (Key.Number2):
                    selectedRobot = 1;
                    break;
                case (Key.Number3):
                    selectedRobot = 2;
                    break;
                case (Key.Number4):
                    selectedRobot = 3;
                    break;
            }
        }
    }
}
