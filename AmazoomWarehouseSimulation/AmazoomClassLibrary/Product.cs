using System;

namespace AmazoomClassLibrary
{
    [Serializable]
    public class Product
    {

        public string ProductName;

        public static int numOfProduct { get; set; }

        public double Weight { get; set; }

        public Location Location { get; set; }

        public Product() { }

        public Product(string name, int num)
        {
            this.ProductName = name;
            this.NumOfProduct = num;
        }

        public Product(string name, int num, Location loc, double weight)
        {
            this.ProductName = name;
            this.NumOfProduct = num;
            this.Location = loc;
            this.Weight = weight;
        }
    }
}
