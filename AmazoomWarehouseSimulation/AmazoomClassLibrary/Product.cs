using System;
using WarehouseComputer.Classes;

namespace AmazoomClassLibrary
{
    [Serializable]
    public class Product
    {
        public Location location { get; set; }

        public string name { get; set; }

        public int numOfProduct { get; set; }

        public int weight { get; set; }

        public Product(string name, int num, Location loc, int weight)
        {
            this.name = name;
            location = loc;
            numOfProduct = num;
            this.weight = weight;
        }

    }
}
