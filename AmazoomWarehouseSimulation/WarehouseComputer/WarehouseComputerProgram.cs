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

namespace WarehouseComputer
{

    class Program
    {
        public static WarehouseMap map;
        private static Queue<Robot> waitingRobots;
        private static Queue<Order> waitingOrders;

        static void Main(string[] args)
        {
            map = new WarehouseMap(15, 5, 5);
            waitingRobots = new Queue<Robot>();
            waitingOrders = new Queue<Order>();

            /*WarehouseComputerStartup StartupObject = new WarehouseComputerStartup();
            AnonymousPipeServerStream WarehouseComputerPipe = StartupObject.WarehouseComputerPipe;
            Queue<Order> OrdersFromWebServer = new Queue<Order>();
            Thread ReadOrder = new Thread(() => WarehouseComputerCommThread.Execute(WarehouseComputerPipe, OrdersFromWebServer));
            ReadOrder.Start();

            for (int i = 0; i < 2; i++)
            {
                Robot r = new Robot(i, 10, 0);
                waitingRobots.Enqueue(r);
                Thread t = new Thread(() => r.Execute());
                t.Start(); //should we join threads ?
            }*/

            InitDatabase();
            ReadProducts();
            map.PopulateShelves();
        }

        private static void InitDatabase()
        {
            string fileName = "Database.json";

            List<Product> products = new List<Product>();
            Product p = new Product("Apple", 100);
            products.Add(p);
            p = new Product("Playstation", 10);
            products.Add(p);
            p = new Product("UE Boom", 15);
            products.Add(p);
            p = new Product("Book", 65);
            products.Add(p);
            p = new Product("Bananas", 80);
            products.Add(p);
            p = new Product("MacBookPro", 20);
            products.Add(p);

            string jsonString = JsonSerializer.Serialize(products);
            File.WriteAllText(fileName, jsonString);

            Console.WriteLine("Files written in Database.json!");

        }


        private static void ReadProducts()
        {
            string fileName = "Database.json";
            string jsonString = File.ReadAllText(fileName);
            map.Inventory = JsonSerializer.Deserialize<List<Product>>(jsonString);
            for (int i = 0; i < map.Inventory.Count; i++)
            {
                Console.WriteLine($"Item {i} : {map.Inventory[i].ProductName}");
            }

        }

        private static void TestShelvesPopulation()
        {
            //edge cases -> too much products
            map.PopulateShelves();
        }

        private static void TestFindRoute()
        {
            // Route testing
            List<Location> locations = new List<Location>();
            locations.Add(new Location(4, 2, 0, 0));
            locations.Add(new Location(2, 3, 0, 0));
            locations.Add(new Location(2, 2, 0, 0));
            locations.Add(new Location(8, 3, 0, 0));
            locations.Add(new Location(12, 2, 0, 0));

            Robot robot = new Robot(1, 10, 0);
            List<Tuple<int, int>> route = FindRoute(locations, robot);
            int count = 0;
            foreach (var cell in route)
            {
                Console.WriteLine("Cell {0} of route : {1}, {2}", count++, cell.Item1, cell.Item2);
            }
        }

        private static void TestRobotsOrder()
        {
            Order order1 = new Order(0);
            Product p = new Product("banana", 1, new Location(4, 2, 0, 2), 1);
            order1.AddProduct(p);
            p = new Product("ananas", 1, new Location(2, 3, 1, 1), 1);
            order1.AddProduct(p);
            p = new Product("orange", 1, new Location(8, 3, 0, 1), 1);
            order1.AddProduct(p);
            p = new Product("lemon", 1, new Location(12, 2, 1, 2), 1);
            order1.AddProduct(p);

            Order order2 = new Order(1);
            p = new Product("apple", 1, new Location(6, 3, 1, 1), 1);
            order2.AddProduct(p);
            p = new Product("strawberry", 1, new Location(4, 3, 0, 1), 1);
            order2.AddProduct(p);
            p = new Product("raspberry", 1, new Location(4, 2, 1, 2), 1);
            order2.AddProduct(p);

            Order order3 = new Order(2);
            p = new Product("pear", 1, new Location(2, 3, 0, 2), 1);
            order3.AddProduct(p);
            p = new Product("blueberry", 1, new Location(14, 3, 0, 1), 1);
            order3.AddProduct(p);
            p = new Product("peach", 1, new Location(10, 2, 1, 2), 1);
            order3.AddProduct(p);

            SendOrder(order1);
            SendOrder(order2);
            SendOrder(order3);
        }

        public static void SendOrder(Order newOrder)
        {
            if(waitingRobots.Count == 0)
            {
                Console.WriteLine("Order {0} waiting!", newOrder.OrderID);
                waitingOrders.Enqueue(newOrder);
            }
            else
            {
                Robot robot = waitingRobots.Dequeue();
                SendOrderToRobot(newOrder, robot);
            }
        }

        private static void SendOrderToRobot(Order order, Robot robot)
        {
            List<Location> locations = new List<Location>();
            foreach (var product in order.Products)
            {
                locations.Add(product.Location);
            }

            List<Tuple<int, int>> route = FindRoute(locations, robot);
            robot.SetRoute(route);
            robot.currOrder = order;
            robot.ewh.Set();
        }


        public static void OrderDone(Order order, Robot robot)
        {
            Console.WriteLine("Order {0} is in a delivery truck !", order.OrderID);
            if(waitingOrders.Count == 0)
            {
                waitingRobots.Enqueue(robot);
            }
            else
            {
                Order nextOrder = waitingOrders.Dequeue();
                SendOrderToRobot(nextOrder, robot);
            }
        }


        private static List<Tuple<int, int>> FindRoute(List<Location> locations, Robot robot)
        {
            List<Tuple<int, int>> route = new List<Tuple<int, int>>();
            locations.Sort();

            /**
             * Step 1 : Get to A1
             * Assumption : Robots are waiting in a hangar
             */

            //get to A-up-5
            route.Add(new Tuple<int, int>(0, map.NRow - 1));
            route.Add(new Tuple<int, int>(1, map.NRow - 1));

            //move on A up
            for (int i = map.NRow; i > 0; i--)
            {
                route.Add(new Tuple<int, int>(1, i - 1));
            }


            /**
             * Step 2 : Get to each location
             */
            int currCol = 1;
            int currRow = 0; //we start at A1

            foreach (var loc in locations)
            {
                //items on A aisle are already collected while going to A1
                if (loc.x != 1)
                {
                    //if we have to switch columns --> get to right column
                    if (loc.x > currCol)
                    {
                        //if we're not on row 1 --> we were on down aisle
                        if (currRow != 0)
                        {
                            route.Add(new Tuple<int, int>(++currCol, currRow)); //move on up aisle
                            for (int i = currRow; i > 0; i--)
                            {
                                route.Add(new Tuple<int, int>(currCol, i - 1)); //get to row1
                            }
                            currRow = 0;
                        }

                        //get to the right column
                        for (int i = currCol; i < loc.x; i++)
                        {
                            route.Add(new Tuple<int, int>(i + 1, currRow));
                        }
                        currCol = loc.x;
                    }

                    //go down to the right location
                    for (int i = currRow; i < loc.y; i++)
                    {
                        route.Add(new Tuple<int, int>(currCol, i + 1));
                    }
                    currRow = loc.y;
                    
                }
            }

            /**
             * Step 3 : Get to truck
             */

            //if last item was not on last column, get there
            if (currCol != map.NCol - 1)
            {
                route.Add(new Tuple<int, int>(++currCol, currRow)); //move on up aisle
                for (int i = currRow; i > 0; i--)
                {
                    route.Add(new Tuple<int, int>(currCol, i - 1)); //get to row1
                }
                currRow = 0;
                for (int i = currCol; i < map.NCol - 1; i++)
                {
                    route.Add(new Tuple<int, int>(i + 1, currRow));
                }
            }
            currCol = map.NCol - 1;

            for (int i = currRow; i < map.NRow - 1; i++)
            {
                route.Add(new Tuple<int, int>(currCol, i + 1));
            }
            currRow = map.NRow - 1;

            //for now : truck at H5, later : truck.location.x
            for (int i = currCol; i > 5 * 2 - 1; i--)
            {
                route.Add(new Tuple<int, int>(i - 1, currRow));
            }
            currCol = 5 * 2 - 1;

            /**
             * Step 4 : Go back to hangar
             */
            for (int i = currCol; i > 1; i--)
            {
                route.Add(new Tuple<int, int>(i - 1, currRow));
            }


            return route;
        }
    }

}