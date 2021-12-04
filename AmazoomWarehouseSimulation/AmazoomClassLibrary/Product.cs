using System;

namespace AmazoomClassLibrary
{
    [Serializable]
    public class Product
    {

        public string ProductName { get; set; }

        public int NumOfProduct { get; set; }

        public double Weight { get; set; }

        public Location Location { get; set; }

        public Product() { }

        public Product(string name, int num)
        {
            this.ProductName = name;
            this.NumOfProduct = num;
            this.Location = new Location();
        }

        public Product(string name, int num, double weight)
        {
            this.ProductName = name;
            this.NumOfProduct = num;
            this.Weight = weight;
        }

        public Product(string name, int num, Location loc, double weight)
        {
            this.ProductName = name;
            this.NumOfProduct = num;
            this.Location = new Location(loc.x, loc.y, loc.leftOrRight, loc.shelf);
            this.Weight = weight;
        }
    }
}
