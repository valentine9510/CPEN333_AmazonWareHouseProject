using System;
using System.Collections.Generic;
using System.Text;

namespace AmazoomClassLibrary
{
    [Serializable]
    public class Order
    {
        public int orderID;

        public List<Product> products;

        public Order()
        {
            orderID = inputOrderID;
            products = new List<Product>();
        }

        public void AddProduct(Product inputProduct)
        {
            this.products.Add(inputProduct);
        }
    }
}
