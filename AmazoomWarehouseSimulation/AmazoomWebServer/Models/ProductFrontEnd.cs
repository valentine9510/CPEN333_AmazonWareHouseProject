using AmazoomClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmazoomWebServer.Models
{
    //public class ProductFrontEnd : Product
    //{
    //    public int ProductStock;

    //    // public image

    //    //public url
    //}

    public class ProductFront
    {
        public string name { get; set; }

        public int numOfProduct { get; set; }

        public int weight { get; set; }

        public ProductFront(string name, int num, int weight)
        {
            this.name = name;
            numOfProduct = num;
            this.weight = weight;
        }
    }

    public static class ProductInventory
    {
        static List<ProductFront> availableProducts = new List<ProductFront>();
        
        static public void LoadRandomObjects()
        {
            var rand = new Random();
            availableProducts.Add(new ProductFront("Apple", rand.Next(10,1000), rand.Next(100, 5000)));
            availableProducts.Add(new ProductFront("Beer", rand.Next(10, 1000), rand.Next(100, 5000)));
            availableProducts.Add(new ProductFront("Orange", rand.Next(10, 1000), rand.Next(100, 5000)));
            availableProducts.Add(new ProductFront("Shirt", rand.Next(10, 1000), rand.Next(100, 5000)));
            availableProducts.Add(new ProductFront("Playstation", rand.Next(10, 1000), rand.Next(100, 5000)));
            availableProducts.Add(new ProductFront("Laptop", rand.Next(10, 1000), rand.Next(100, 5000)));
            availableProducts.Add(new ProductFront("Nike Air", rand.Next(10, 1000), rand.Next(100, 5000)));
        }

        static public void clearInventory()
        {
            availableProducts.Clear();
        }

        static public List<ProductFront> getInventory()
        {
            return availableProducts;
        }

    }
}
