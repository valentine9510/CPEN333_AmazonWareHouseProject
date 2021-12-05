using System;
using System.Collections.Generic;
using System.Text;


namespace AmazoomClassLibrary
{
    [Serializable]
    public class Order
    {
        public int OrderID;

        public List<Product> Products; // could possibly change this to IEnumerable

        public enum Status // figure out how enum works and if you have to initialise in constructor
        {
            // Get better idea of what statuses we should have
            NEW_ORDER,
            ORDER_IN_TRANSIT,
            DELIVERED
        }

        public double OrderWeight { get; set; }

        public Status status_;

        public Order()
        {
            Products = new List<Product>();
            OrderWeight = 0;
            // maybe have to initialise Status enum
        }

        public Order(int inputOrderID)
        {
            OrderID = inputOrderID;
            Products = new List<Product>();
            // maybe have to initialise Status enum
        }

        public void AddProduct(Product inputProduct)
        {
            this.Products.Add(inputProduct);
            OrderWeight += inputProduct.Weight;
        }

        public void RemoveProduct(string name)
        {
            // TO DO MAYBE: Can implement this function here, based on the given name
            // check if the product is in the products list and remove if there or print a
            // message if not there.
        }

        public void setStatus(Status inputstatus)
        {
            status_ = inputstatus;
        }
    }
}
