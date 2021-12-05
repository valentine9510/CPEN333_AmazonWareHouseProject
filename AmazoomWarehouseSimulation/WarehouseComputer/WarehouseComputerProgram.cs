using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using WarehouseComputer.Classes;
using AmazoomClassLibrary;
using System.IO;
using System.Diagnostics;
using System.IO.Pipes;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json.Serialization;

namespace WarehouseComputer
{

    class Program
    {
        public static WarehouseMap map;
        public static Process AmazoomWebServerProcess;
        public static string CurrentDatabaseName;
        private static Queue<Robot> waitingRobots;
        public static Queue<Order> OrdersFromWebServer = new Queue<Order>();
        private static Queue<Order> waitingOrders;
        private static Mutex mutex = new Mutex();

        static void Main(string[] args)
        {
            map = WarehouseComputerStartup.InitWarehouseMap();
            CurrentDatabaseName = WarehouseComputerStartup.InitProductInventory(map);
            AmazoomWebServerProcess = WarehouseComputerStartup.InitWebServerProcessAndPipe();

            for (int i = 0; i < 1; i++)
            {
                Robot r = new Robot(i, 10, 0);
                Thread t = new Thread(() => r.Execute());
                t.Start(); //should we join threads ?
            }

            Console.ReadKey();
            Order testorder = new Order(1);
            testorder.AddProduct(map.Inventory[0]);
            testorder.AddProduct(map.Inventory[1]);
            AddToQueue(testorder);
            Console.ReadKey();

            //Console.ReadKey();
            //foreach(Order order in OrdersFromWebServer)
            //{
            //    Console.WriteLine("[WH COMPUTER] orderID:{0}, Product:{1}, Num:{2}", order.OrderID, order.Products[0].ProductName, order.Products[0].NumOfProduct);
            //}

        }


        public static void AddToQueue(Order OrderedProduct)
        {
            mutex.WaitOne();
            OrdersFromWebServer.Enqueue(OrderedProduct);
            mutex.ReleaseMutex();
        }

        public static Order TakeFromQueue()
        {
            mutex.WaitOne();
            Order ReturnOrder = OrdersFromWebServer.Dequeue();
            mutex.ReleaseMutex();
            return ReturnOrder;
        }

        public static int ReturnQueueCount()
        {
            mutex.WaitOne();
            int count = OrdersFromWebServer.Count;
            mutex.ReleaseMutex();
            return count;
        }
    }

}