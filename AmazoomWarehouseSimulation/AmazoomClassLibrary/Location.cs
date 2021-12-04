using System;

namespace AmazoomClassLibrary
{
    [Serializable]
    public class Location : IComparable
    {
        public int x, y, leftOrRight, shelf;

        public Location() { }
        public Location(int x, int y, int leftOrRight, int shelf)
        {
            this.x = x;
            this.y = y;
            this.leftOrRight = leftOrRight; //Left -> leftOrRight = 0, Right -> leftOrRight = 1
            this.shelf = shelf;
        }

        public int CompareTo(object obj)
        {
            Location loc = (Location)obj;
            if (this.x < loc.x)
            {
                return -1;
            }

            if (this.x > loc.x)
            {
                return 1;
            }

            if (this.y < loc.y)
            {
                return -1;
            }

            if (this.y > loc.y)
            {
                return 1;
            }

            return 1; //we just need to sort them in whatever order of same row + column
        }

        public override string ToString()
        {
            string lor = leftOrRight == 1 ? "right" : "left";
            return $"Column {x}, Row {y}, on the {lor}, Shelf {shelf}";
        }
    }
}
