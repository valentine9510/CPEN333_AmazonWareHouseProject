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
        public static Queue<Order> OrdersFromWebServer = new Queue<Order>();
        public static Queue<Order> WaitingOrders = new Queue<Order>();
        //private static Mutex mutex = new Mutex();


        public static Queue<Truck> WaitingTrucks = new Queue<Truck>();
        public static List<Truck> Docks = new List<Truck>(); //used for whole sync
        public static Location[] dockLocations;
        public static int NextTruck = 0;

        static void Main(string[] args)
        {
            map = WarehouseComputerStartup.InitWarehouseMap(true);
            CurrentDatabaseName = WarehouseComputerStartup.InitProductInventory(map, true);
            AmazoomWebServerProcess = WarehouseComputerStartup.InitWebServerProcessAndPipe();

            int RobotNum = 5;
            int truckNum = 3;
            Robot[] r = new Robot[RobotNum];
            Thread[] t = new Thread[RobotNum];

            for (int i = 0; i < RobotNum; i++)
            {
                int threadnum = i;
                r[threadnum] = new Robot(threadnum, 10, 0);
                t[threadnum] = new Thread(() => r[threadnum].Execute());
                t[threadnum].Start(); //should we join threads ?
            }
            
            //Order testorder = new Order(1);
            //Product p = new Product("Apple", 10, new Location(8, 4, 0, 1), 1.5);
            //testorder.AddProduct(p);

            
            //Thread.Sleep(1000);
            //for (int i = 0; i < 20; i++)
            //{
            //    AddOrder(testorder);
            //}
            //Thread.Sleep(1000);

            List<Truck> trucks = new List<Truck>();
            List<Thread> truckThreads = new List<Thread>();
            for (int i = 0; i < truckNum; i++)
            {
                Thread.Sleep(5000);
                trucks.Add(new Truck(i, 5, 0));
                int threadnum = i;
                truckThreads.Add(new Thread(() => trucks[threadnum].WaitForDelivery()));
                truckThreads[i].Start();
            }

            
        }

        public static void AddOrder(Order order)
        {
            lock (WaitingOrders)
            {
                //Console.WriteLine("Order added to the queue!");
                WaitingOrders.Enqueue(order);
                Monitor.Pulse(WaitingOrders);
            }
        }

        public static void AddTruck(Truck truck)
        {
            lock (Docks)
            {
                if (Docks.Count < 2)
                {
                    Docks.Add(truck);
                    Console.WriteLine($"Truck {truck.Id} is waiting at dock {Docks.Count - 1}");
                    Monitor.PulseAll(Docks);
                }
                else
                {
                    WaitingTrucks.Enqueue(truck);
                    Console.WriteLine($"Truck {truck.Id} is queuing up...");
                }
            }
        }   
    }
}