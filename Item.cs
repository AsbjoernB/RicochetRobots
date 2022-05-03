using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
