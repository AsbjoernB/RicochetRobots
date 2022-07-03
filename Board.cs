using Silk.NET.OpenGL;
using Silk.NET.SDL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace RicochetRobots
{

    public class Board
    {
        private static bool hasLoadedBoards = false;
        public static Dictionary<RGBY, List<string>> subBoards = new Dictionary<RGBY, List<string>>()
        {
            { RGBY.red, new List<string>() },
            { RGBY.green, new List<string>() },
            { RGBY.blue, new List<string>() },
            { RGBY.yellow, new List<string>() }
        };

        public List<Wall> walls = new List<Wall>();

        private Board()
        {
            if (!hasLoadedBoards)
                LoadSubBoards();
        }

        public Item getItem(Vector2 pos)
        {
            foreach (Wall wall in walls)
            {
                if (wall.position == pos)
                    return wall.item;
            }
            return new Item(ItemType.none, RGBY.red);
        }

        private static void LoadSubBoards()
        {
            hasLoadedBoards = true;
            Console.WriteLine("loading boards...");
            // boardData = ReadFromFile..,

            RGBY currentHeader = RGBY.red;
            string currentSubBoard = "";

            using (var reader = new StreamReader("Boards.txt"))
            {
                Console.WriteLine(Regex.Replace(reader.ReadToEnd(), "([01234567])", m => (int.Parse(m.Value) + 1).ToString()));
            }

            using (var reader = new StreamReader("Boards.txt"))
            {
                Console.WriteLine("start");

                for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    Match header = Regex.Match(line, @"\[(.+)\]");
                    if (header.Success)
                    {
                        currentHeader = (RGBY)Enum.Parse(typeof(RGBY), header.Groups[1].Value);
                        continue;
                    }
                    else if (Regex.Match(line, "end").Success)
                    {
                        Console.WriteLine(currentSubBoard);
                        Console.WriteLine();
                        subBoards[currentHeader].Add(currentSubBoard);
                        currentSubBoard = "";
                        continue;
                    }

                    if (currentSubBoard != "")
                        currentSubBoard += "|";
                    currentSubBoard += line;

                }
                Console.WriteLine("loading boards complete");
            }
        }

        public static Board GenerateBoard(Random r)
        {
            Board finalboard = new Board();

            if (r == null)
                r = new Random();

            List<string> subboards = new List<string>();
            for (int i = 0; i < 4; i++)
            {
                // pick random board from each color and add to array
                int index = r.Next(subBoards[(RGBY)i].Count);
                subboards.Add(subBoards[(RGBY)i][index]);
                Console.WriteLine((RGBY)i + ": " + index);
            }
            subboards = subboards.OrderBy(b => r.Next()).ToList(); // shuffle

            for (int i = 0; i < 4; i++)
            {
                var walls = ParseBoard(subboards[i]);
                Console.WriteLine(walls[0].position);
                for (int wc = 0; wc < walls.Count; wc++)
                {
                    walls[wc].rotation = (Rotation)xMath.Wrap(((int)walls[wc].rotation + i),0,3);

                    for (int rotTimes = 0; rotTimes < i; rotTimes++)
                    {
                        walls[wc].position = new Vector2(-walls[wc].position.Y, walls[wc].position.X);
                    }

                    walls[wc].position += new Vector2(8, 8) + new Vector2(0 < i && i < 3 ? -1 : 0, i > 1 ? -1 : 0);
                }

                finalboard.walls.AddRange(walls);
            }
            Console.WriteLine(finalboard.walls.Count);
            return finalboard;
        }

        private static List<Wall> ParseBoard(string input)
        {

            List<Wall> walls = new List<Wall>();

            string[] walldatas = input.Split('|');
            foreach (string walldata in walldatas)
            {

                string[] values = walldata.Split(",");

                for (int i = 0; i < values.Length; i++)
                    Console.Write(values[i] + " | ");
                Console.WriteLine();

                Wall w = new Wall(int.Parse(values[0]), int.Parse(values[1]));
                w.rotation = (Rotation)Enum.Parse(typeof(Rotation), values[2]);
                if (values[3] != "none")
                {
                    w.item = new Item((ItemType)Enum.Parse(typeof(ItemType), values[3]), (RGBY)Enum.Parse(typeof(RGBY), values[4]));
                }

                walls.Add(w);
            }

            return walls;

        }


        /*
        private static bool hasLoadedBoards = false;
        public static Dictionary<RGBY, List<Board>> subBoards = new Dictionary<RGBY, List<Board>>()
        {
            { RGBY.red, new List<Board>() },
            { RGBY.green, new List<Board>() },
            { RGBY.blue, new List<Board>() },
            { RGBY.yellow, new List<Board>() }
        };

        public List<Wall> walls = new List<Wall>();

        private Board()
        {
            if (!hasLoadedBoards)
                LoadSubBoards();
        }

        public Item getItem(Vector2 pos)
        {
            foreach (Wall wall in walls)
            {
                if (wall.position == pos)
                    return wall.item;
            }
            return new Item(ItemType.none, RGBY.red);
        }

        public static Board GenerateBoard(Random r)
        {
            LoadSubBoards();
            // TODO: always fixed seed? testing
            if (r == null)
                r = new Random();

            Board finalBoard = new Board();

            List<Board> sb = new List<Board>();
            for (int i = 0; i < 4; i++)
            {
                // pick random board from each color and add to array
                int index = r.Next(subBoards[(RGBY)i].Count);
                sb.Add(subBoards[(RGBY)i][index]);
                Console.WriteLine((RGBY)i + ": " + index);
            }

            var shuffled = sb.OrderBy(b => r.Next()).ToList();
            shuffled[0].walls.ForEach(w => w.position += new Vector2(8, 8));
            shuffled[1].walls.ForEach(w => {
                w.position *= new Vector2(-1, 1);
                w.rotation = (Rotation)xMath.Wrap((int)w.rotation + 1, 0, 3);
                w.position += new Vector2(7, 8);
            });
            shuffled[2].walls.ForEach(w => {
                w.position *= new Vector2(-1, -1);
                w.rotation = (Rotation)xMath.Wrap((int)w.rotation + 2, 0, 3);
                w.position += new Vector2(7, 7);
            });
            shuffled[3].walls.ForEach(w => {
                w.position *= new Vector2(1, -1);
                w.rotation = (Rotation)xMath.Wrap((int)w.rotation + 3, 0, 3);
                w.position += new Vector2(8, 7);
            });
            for (int i = 0; i < 4; i++)
                shuffled[i].walls.ForEach(w => finalBoard.walls.Add(w.Clone()));
            return finalBoard;
        }

        private static void LoadSubBoards()
        {
            hasLoadedBoards = true;
            Console.WriteLine("loading boards...");
            // boardData = ReadFromFile..,

            RGBY currentHeader = RGBY.red;
            Board currentSubBoard = new Board();

            foreach (List<Board> bl in subBoards.Values)
                bl.Clear();

            using (var reader = new StreamReader("Boards.txt"))
            {
                Console.WriteLine("start");

                for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    Match header = Regex.Match(line, @"\[(.+)\]");
                    if (header.Success)
                    {
                        currentHeader = (RGBY)Enum.Parse(typeof(RGBY), header.Groups[1].Value);
                        continue;
                    }
                    if (Regex.Match(line, "end").Success)
                    {
                        subBoards[currentHeader].Add(currentSubBoard);
                        currentSubBoard = new Board();
                        continue;
                    }

                    string[] values = line.Split(",");

                    for (int i = 0; i < values.Length; i++)
                        Console.Write(values[i] + " | ");
                    Console.WriteLine();

                    Wall w = new Wall(int.Parse(values[0]), int.Parse(values[1]));
                    w.rotation = (Rotation)Enum.Parse(typeof(Rotation), values[2]);
                    if (values[3] != "none")
                    {
                        w.item = new Item((ItemType)Enum.Parse(typeof(ItemType), values[3]), (RGBY)Enum.Parse(typeof(RGBY), values[4]));
                    }

                    currentSubBoard.walls.Add(w);
                }
                Console.WriteLine("loading boards complete");
            }
        }
        */
    }
}
