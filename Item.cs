using System.Numerics;

namespace RicochetRobots
{
    public struct Item
    {
        public ItemType itemType;
        public RGBY color;

        public Item(ItemType itemType, RGBY color)
        {
            this.itemType = itemType;
            this.color = color;
        }

        public static bool operator ==(Item c1, Item c2)
        {
            return c1.Equals(c2);
        }
        public static bool operator !=(Item c1, Item c2)
        {
            return !c1.Equals(c2);
        }
        public override string ToString()
        {
            return $"({itemType}, {color})";
        }

        public Vector4 GetColor()
        {
            if (itemType == ItemType.vortex)
                return new Vector4(1, 1, 1, 1);
            return Renderer.colors[color];
        }
    }


    public enum ItemType
    {
        star,
        sun,
        planet,
        moon,
        vortex,
        none
    }
}
