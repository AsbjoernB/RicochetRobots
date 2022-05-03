using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Silk.NET.Vulkan;
using Silk.NET.Input;
using System.Diagnostics;

namespace RicochetRobots
{
    public class Gameplay
    {
        protected Vector2[] RobotPositions = new Vector2[4];

        // rgby
        protected int selectedRobot = 0;

        protected List<Item> itemorder;

        protected int itemIndex = 0;


        protected float tilecount => Master.tilecount;

        protected IInputContext input;

        protected Board board;



        protected Gameplay()
        {
            Master.window.Load += OnLoad;
            Master.window.Render += Draw;
        }

        protected virtual void OnLoad()
        {
            input = Master.window.CreateInput();
            for (int i = 0; i < input.Keyboards.Count; i++)
            {
                input.Keyboards[i].KeyDown += KeyDown;
            }
            StartGame();
        }

        protected virtual void StartGame()
        {
            selectedRobot = 0;
            itemIndex = 0;
            Random r = new Random();
            board = Board.GenerateBoard(r);
            for (int i = 0; i < 4; i++)
            {
                RobotPositions[i] = new Vector2(r.Next((int)tilecount), r.Next((int)tilecount));
            }

            itemorder = new List<Item>();
            
            for (int c = 0; c < 4; c++)
            {
                for (int t = 0; t < (int)ItemType.vortex; t++)
                {
                    itemorder.Add(new Item((ItemType)t, (RGBY)c));
                }
            }
            itemorder.Add(new Item(ItemType.vortex, RGBY.red));
            itemorder = itemorder.OrderBy(_ => r.Next()).ToList();


        }

        protected virtual Vector2 MoveRobot(int xDir, int yDir)
        {
            Vector2 dir = new Vector2(xDir, yDir);
            Vector2 pos = RobotPositions[selectedRobot];
            for (int i = 0; i < tilecount; i++)
            {
                if (CanMakeMove(pos, dir))
                    pos += dir;
                else
                    break;
            }
            RobotPositions[selectedRobot] = pos;
            return pos;
        }
        protected virtual bool CanMakeMove(Vector2 fromPos, Vector2 dir)
        {

            Vector2 toPos = fromPos + dir;

            Console.WriteLine(toPos);

            if (toPos.X < 0 || toPos.X > tilecount - 1 || toPos.Y < 0 || toPos.Y > tilecount - 1)
                return false;

            for (int r = 0; r < RobotPositions.Length; r++)
            {
                if (RobotPositions[r].Equals(toPos))
                    return false;

            }
            foreach (Wall wall in board.walls)
            {
                if (wall.position.Equals(fromPos))
                {
                    if (wall.wallDir.X == dir.X || wall.wallDir.Y == dir.Y)
                        return false;
                }
                if (wall.position.Equals(toPos))
                {
                    if (wall.wallDir.X == -dir.X || wall.wallDir.Y == -dir.Y)
                        return false;
                }

            }
            return true;
        }

        public virtual void Draw(double delta)
        {
            DrawBoard();
            for (int i = 0; i < 4; i++)
            {
                Renderer.DrawTexture(TextureBank.Textures["robot"], RobotPositions[i], hueShift: Renderer.hueShift((RGBY)i));
            }
            DrawSelectionHighlight();
        }
        protected virtual void DrawBoard()
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
                if (wall.item.itemType != ItemType.none)
                    Renderer.DrawTexture(TextureBank.Textures[Enum.GetName(typeof(ItemType), wall.item.itemType)], wall.position, hueShift: Renderer.hueShift(wall.item.color));
            }

            if (itemIndex < itemorder.Count())
                Renderer.DrawTexture(TextureBank.Textures[Enum.GetName(typeof(ItemType), itemorder[itemIndex].itemType)], new Vector2(tilecount / 2 - 0.5f), scale:2, hueShift: Renderer.hueShift(itemorder[itemIndex].color));
        }
        protected virtual void DrawSelectionHighlight()
        {
            Renderer.DrawTexture(TextureBank.Textures["colrect"], RobotPositions[selectedRobot], 0, 1, Renderer.hueShift((RGBY)selectedRobot), 0.25f);
            Vector2 fromPos = RobotPositions[selectedRobot];
            
            // copied 4 times, but how to loop (0;-1),(0;1),(-1;0),(1;0)
            while (CanMakeMove(fromPos, -Vector2.UnitX))
            {
                fromPos += -Vector2.UnitX;
                Renderer.DrawTexture(TextureBank.Textures["colrect"], fromPos, 0, 1, Renderer.hueShift((RGBY)selectedRobot), 0.25f);
            }
            fromPos = RobotPositions[selectedRobot];
            while (CanMakeMove(fromPos, Vector2.UnitX))
            {
                fromPos += Vector2.UnitX;
                Renderer.DrawTexture(TextureBank.Textures["colrect"], fromPos, 0, 1, Renderer.hueShift((RGBY)selectedRobot), 0.25f);
            }
            fromPos = RobotPositions[selectedRobot];
            while (CanMakeMove(fromPos, -Vector2.UnitY))
            {
                fromPos += -Vector2.UnitY;
                Renderer.DrawTexture(TextureBank.Textures["colrect"], fromPos, 0, 1, Renderer.hueShift((RGBY)selectedRobot), 0.25f);
            }
            fromPos = RobotPositions[selectedRobot];
            while (CanMakeMove(fromPos, Vector2.UnitY))
            {
                fromPos += Vector2.UnitY;
                Renderer.DrawTexture(TextureBank.Textures["colrect"], fromPos, 0, 1, Renderer.hueShift((RGBY)selectedRobot), 0.25f);
            }
        }

        protected virtual void KeyDown(IKeyboard arg1, Key arg2, int arg3)
        {
            if (arg2 == Key.Backspace)
            {
                StartGame();
                return;
            }
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
