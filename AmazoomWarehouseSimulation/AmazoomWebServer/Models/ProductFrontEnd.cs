using AmazoomClassLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmazoomWebServer.Models
{
    public class ProductFrontEnd : Product
    {
        public int ProductStock;

        // public image

        //public url
    }

    public class Alert
    {
        public bool showAlert { get; set; }
        public string alertMessage { get; set; }

        public Alert(bool show, string msg)
        {
            this.showAlert = show;
            this.alertMessage = alertMessage;
        }
    }

    public class ProductInventory
    {
        static public List<Product> availableProducts  { get; set; }
        static public Order currentOrder { get; set; }
        public Product currentProduct { get; set; }
        static public Queue<Order> uncompletedOrders { get; set; }
        static public Queue<Order> completedOrders { get; set; }
        public Alert currentAlert { get; set; }

        public ProductInventory()
        {
            //availableProducts = new List<Product>();
            availableProducts = JSONFile.GetProducts("FruitDatabase.json");
            uncompletedOrders = new Queue<Order>();
            completedOrders = new Queue<Order>();
            currentOrder = new Order(1);
            this.currentAlert = new Alert(false, "");
            //this.LoadRandomObjects();
        }
        public void LoadRandomObjects()
        {
            var rand = new Random();
            availableProducts.Add(new Product("Apple", rand.Next(10,1000)));
            availableProducts.Add(new Product("Beer", rand.Next(10, 1000)));
            availableProducts.Add(new Product("Orange", rand.Next(10, 1000)));
            availableProducts.Add(new Product("Shirt", rand.Next(10, 1000)));
            availableProducts.Add(new Product("Playstation", rand.Next(10, 1000)));
            availableProducts.Add(new Product("Laptop", rand.Next(10, 1000)));
            availableProducts.Add(new Product("Nike-Air", rand.Next(10, 1000)));

            //Populate completed orders
            Order temp = new Order(rand.Next(1, 1000));
            temp.AddProduct(new Product("Apple", rand.Next(1,10)));
            temp.AddProduct(new Product("Toaster", rand.Next(1, 10)));

            Order temp2 = new Order(rand.Next(1, 1000));
            temp2.AddProduct(new Product("Shirt", rand.Next(1, 10)));
            temp2.AddProduct(new Product("Beer", rand.Next(1, 10)));

            completedOrders.Enqueue(temp);
            completedOrders.Enqueue(temp2);


            //Populate uncompleted orders
            temp.OrderID = 12;
            uncompletedOrders.Enqueue(temp);
            temp2.OrderID = 43;
            temp2.AddProduct(new Product("Toaster", rand.Next(1, 10)));
            uncompletedOrders.Enqueue(temp2);

            Order temp3 = new Order(rand.Next(1, 1000));
            temp3.AddProduct(new Product("Orange", rand.Next(1, 10)));
            temp3.AddProduct(new Product("Nike-Air", rand.Next(1, 10)));

            Order temp4 = new Order(rand.Next(1, 1000));
            temp4.AddProduct(new Product("Beer", rand.Next(1, 10)));
            temp4.AddProduct(new Product("Apple", rand.Next(1, 10)));
            temp4.AddProduct(new Product("Bag", rand.Next(1, 10)));

            completedOrders.Enqueue(temp3);
            uncompletedOrders.Enqueue(temp4);

            //Add to current order
            currentOrder.AddProduct(new Product("Shirt", rand.Next(1, 10)));
            currentOrder.AddProduct(new Product("Apple", rand.Next(1, 10)));
            currentOrder.AddProduct(new Product("Laptop", rand.Next(1, 10)));
            currentOrder.AddProduct(new Product("Orange", rand.Next(1, 10)));
            currentOrder.AddProduct(new Product("Beer", rand.Next(1, 10)));

        }

        public void clearInventory()
        {
            availableProducts.Clear();
        }

        public List<Product> getInventory()
        {
            return availableProducts;
        }

        public Queue<Order> getUncompletedOrdersQueue()
        {
            return uncompletedOrders;
        }

        public Queue<Order> getCompletedOrdersQueue()
        {
            return completedOrders;
        }

        public Order getCurrentOrder()
        {
            return currentOrder;
        }

        public string RemoveAlert()
        {
            this.currentAlert.alertMessage = "";
            this.currentAlert.showAlert = false;
            return "";
        }

    }
}
