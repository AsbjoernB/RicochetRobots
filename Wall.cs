using System;
using System.Numerics;

namespace RicochetRobots
{
    // since this is a class and not a struct, some boards will refer to eachother indirectly (maybe)
    public class Wall
    {

        public Vector2 position;
        public Rotation rotation;
        public Item item;
        public RGBY itemColor;

        public Wall(Vector2 position, Rotation rotation = Rotation.UR, Item item = Item.none, RGBY itemColor = RGBY.red)
        {
            this.position = position;
            this.rotation = rotation;
            this.item = item;
            this.itemColor = itemColor;
        }
        public Wall(int x, int y, Rotation rotation = Rotation.UR, Item item = Item.none, RGBY itemColor = RGBY.red) : this(new Vector2(x, y), rotation, item, itemColor) {}

        

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

    public enum Rotation
    {
        UL = 0,
        DL = 1,
        DR = 2,
        UR = 3
    }

    public enum Item
    {
        none,
        star,
        sun,
        planet,
        moon,
        vortex
    }
}
