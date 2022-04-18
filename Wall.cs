using System;
using System.Numerics;

namespace RicochetRobots
{
    // since this is a class and not a struct, some boards will refer to eachother indirectly (maybe)
    public class Wall
    {

        public Wall(Vector2 position, Rotation rotation = Rotation.UR)
        {
            this.position = position;
            this.rotation = rotation;
        }
        public Wall(int x, int y, Rotation rotation = Rotation.UR)
        {
            this.position = new Vector2(x, y);
            this.rotation = rotation;
        }

        public Vector2 position;
        public Rotation rotation;

        public Vector2 wallDir
        {
            get
            {
                switch (rotation)
                {
                    case Rotation.UL:
                        return new Vector2(-1, 1);
                    case Rotation.DL:
                        return new Vector2(-1, -1);
                    case Rotation.DR:
                        return new Vector2(1, -1);
                    case Rotation.UR:
                        return new Vector2(1, 1);
                    default:
                        throw new Exception("Rotation: " + rotation + " not found");
                }
            }
        }

        public Wall Clone() => (Wall)MemberwiseClone();
    }
}
