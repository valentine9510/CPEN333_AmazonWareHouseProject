using System;
using System.Collections.Generic;
using System.Text;

namespace AmazoomClassLibrary
{
    [Serializable]
    public class Order
    {
        public int OrderID;

        public List<Product> Products;

        public Order(int inputOrderID)
        {
            OrderID = inputOrderID;
            Products = new List<Product>();
        }

        public void AddProduct(Product inputProduct)
        {
            this.Products.Add(inputProduct);
        }
    }
}
