using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.OpenGL.Extensions.ImGui;
using System.Numerics;
using System.Diagnostics;
using Silk.NET.Input;

namespace RicochetRobots
{
    public class SpeedrunGameplay : Gameplay
    {

        long itemPickupTime = 0;

        public Stopwatch stopwatch { get; private set; }

        List<TimeSpan> splits = new List<TimeSpan>();

        int moveCount = 0;

        List<int> moveCounts = new List<int>();

        static ImGuiController imgui { get; set; }

        protected override void OnLoad()
        {
            base.OnLoad();

            imgui = new ImGuiController(Renderer.gl, Master.window, input);





        }

        protected override void StartGame()
        {
            base.StartGame();

            moveCount = 0;

            splits = new List<TimeSpan>();

            moveCounts = new List<int>();

            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        public override void Draw(double delta)
        {
            base.Draw(delta);

            imgui.Update((float)delta);

            ImGuiNET.ImGui.Begin("Livesplit", ImGuiNET.ImGuiWindowFlags.NoTitleBar | ImGuiNET.ImGuiWindowFlags.NoMove);

            ImGuiNET.ImGui.SetWindowFontScale(1);
            ImGuiNET.ImGui.SetWindowPos(new Vector2(Master.window.Size.Y, 0));
            ImGuiNET.ImGui.SetWindowSize(new Vector2((Master.window.Size.X - Master.window.Size.Y)/2f, Master.window.Size.Y));

            //ImGuiNET.ImGui.ShowDemoWindow();

            ImGuiNET.ImGui.BeginTable("mainTable", 2);
            ImGuiNET.ImGui.TableNextColumn();
            ImGuiNET.ImGui.Text($"{stopwatch.Elapsed.Minutes.ToString("00")}:{stopwatch.Elapsed.Seconds.ToString("00")}");
            ImGuiNET.ImGui.TableNextColumn();
            ImGuiNET.ImGui.Text($"{moveCount}");
            ImGuiNET.ImGui.EndTable();

            ImGuiNET.ImGui.BeginTable("splitsTable", 3, ImGuiNET.ImGuiTableFlags.RowBg | ImGuiNET.ImGuiTableFlags.BordersH);
            ImGuiNET.ImGui.TableHeader("forntietniet");

            for (int i = 0; i < splits.Count; i++)
            {
                ImGuiNET.ImGui.TableNextColumn();
                ImGuiNET.ImGui.Text($"{i+1}:");
                ImGuiNET.ImGui.TableNextColumn();
                ImGuiNET.ImGui.Text($"{splits[i].Minutes.ToString("00")}:{splits[i].Seconds.ToString("00")}");
                ImGuiNET.ImGui.TableNextColumn();
                ImGuiNET.ImGui.Text($"{moveCounts[i]}");
            }
            if (itemIndex < itemorder.Count)
            {
                ImGuiNET.ImGui.TableNextColumn();
                ImGuiNET.ImGui.Text($"{splits.Count + 1}:");
                ImGuiNET.ImGui.TableNextColumn();
                TimeSpan currentSplit = new TimeSpan(stopwatch.ElapsedTicks - itemPickupTime);
                ImGuiNET.ImGui.Text($"{currentSplit.Minutes.ToString("00")}:{currentSplit.Seconds.ToString("00")}");
                ImGuiNET.ImGui.TableNextColumn();
                ImGuiNET.ImGui.Text($"{moveCount - moveCounts.Sum()}");
            }

            ImGuiNET.ImGui.EndTable();

            ImGuiNET.ImGui.End();

            imgui.Render();
        }

        protected override Vector2 MoveRobot(int xDir, int yDir)
        {
            Vector2 prevPos = RobotPositions[selectedRobot];
            Vector2 pos = base.MoveRobot(xDir, yDir);
            if (prevPos == pos)
                return pos;

            moveCount++;

            Item item = board.getItem(pos);

            //if (item.itemType != ItemType.none)
            //    Console.WriteLine(itemorder[itemIndex] + ", " + item);

            if (item == itemorder[itemIndex] && (item.itemType == ItemType.vortex || item.color == (RGBY)selectedRobot))
            {
                CollectItem();
            }

            return pos;
        }
        protected void CollectItem()
        {
            itemIndex++;
            if (itemIndex == itemorder.Count)
                stopwatch.Stop();

            splits.Add(new TimeSpan(stopwatch.ElapsedTicks - itemPickupTime));
            moveCounts.Add(moveCount - moveCounts.Sum());

            itemPickupTime = stopwatch.ElapsedTicks;


        }
        protected override void KeyDown(IKeyboard arg1, Key arg2, int arg3)
        {
            if (itemIndex < itemorder.Count)
                base.KeyDown(arg1, arg2, arg3);
        }
    }
}
