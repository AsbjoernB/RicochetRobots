using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RicochetRobots
{
    public class Board
    {
        public List<Wall> walls;


    }

    public struct Wall
    {
        public Wall(Vector2 position, Rotation rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }

        public Vector2 position;
        public Rotation rotation;


    }

    public enum Rotation
    {
        UL = 0,
        DL = 1,
        DR = 2,
        UR = 3
    }

}
