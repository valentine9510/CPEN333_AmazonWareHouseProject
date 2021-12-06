using System;
using System.Collections.Generic;
using System.Text;
using AmazoomClassLibrary;

namespace WarehouseComputer.Classes
{
    class Manager
    {
        public void getOrderStatus(int ID)
        {
            Order temp = new Order();
            foreach(Order o in Program.OrdersFromWebServer)
            {
                if(o.OrderID == ID)
                {
                    temp = o;
                }
            }
            switch(temp.status_)
            {
                case Order.Status.NEW_ORDER: Console.WriteLine("NEW ORDER");
                break;
                case Order.Status.ORDER_IN_TRANSIT: Console.WriteLine("ORDER IN TRANSIT");
                break;
                case Order.Status.DELIVERED: Console.WriteLine("DELIVERED");
                break;
            }

        }
        public void checkItemStock(Product p)
        {
            if (Program.map.Inventory.Contains(p))
            {
                Console.WriteLine("There are currently {0} {1}s in stock", p.NumOfProduct, p.ProductName);
            }
            else
            {
                Console.WriteLine("There are currently none of that item stocked.");
            }
        }

        public void alertLowStock()
        {
            foreach(Product p in Program.map.Inventory)
            {
                if(p.NumOfProduct < 2)
                {
                    Console.WriteLine("{0} is low on stock", p);
                }
            }
        }





    }
}
