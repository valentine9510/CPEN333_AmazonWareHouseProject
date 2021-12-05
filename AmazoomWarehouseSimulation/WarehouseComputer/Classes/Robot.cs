using System;
using System.Collections.Generic;
using AmazoomClassLibrary;
using System.Threading;

namespace WarehouseComputer.Classes
{
    // may be best to use semaphore with the orderqueue instead of mutex
    public class Robot
    {
        public enum Status
        {
            WAITING_FOR_ORDER,
            FULFILLING_ORDER
        }

        private int id;
        private double maxWeight;
        private double currWeight;
        private Cell currCell;
        private List<Tuple<int, int>> route;
        public Order currOrder;
        public Mutex robotmutex;

        public Robot(int id, double maxWeight, double currWeight, Mutex mutex)
        {
            this.id = id;
            this.maxWeight = maxWeight;
            this.currWeight = currWeight;

            this.currCell = null;
            this.currOrder = null;
            this.route = new List<Tuple<int, int>>();
            this.robotmutex = mutex;
        }

        public void SetRoute(List<Tuple<int, int>> route)
        {
            this.route = route;
        }

        public void Execute()
        {

            while (true)
            {

                // set robot status to fulfilling order
                robotmutex.WaitOne();
                bool readorder = GetOrder();
                robotmutex.ReleaseMutex();
                if (readorder == true)
                {
                    List<Location> productlocations = new List<Location>();
                    foreach (Product p in currOrder.Products)
                    {
                        productlocations.Add(p.Location);
                    }
                    this.route = FindRoute(productlocations);
                    ExecRoute();
                }
            }
        }

        public bool GetOrder()
        {
            bool readsuccess = false;
            if (Program.OrdersFromWebServer.Count != 0)
            {
                this.currOrder = Program.OrdersFromWebServer.Dequeue();
                foreach (Product p in currOrder.Products)
                {
                    Console.Write("{0} ", p.ProductName);
                }
                Console.Write(".\n");
                readsuccess = true;
                return readsuccess;
            }
            else
            {
                readsuccess = false;
                return readsuccess;
            }
        }


        private void ExecRoute()
        {

            Console.WriteLine("Robot {0} is starting a new route !", this.id);
            foreach (var nextLocation in this.route)
            {
                Cell nextCell = Program.map.GetCell(nextLocation.Item2, nextLocation.Item1); //Find route impl. with x-y, getCoord with row-col

                Monitor.Enter(nextCell);

                if(currCell != null)
                {
                    Console.WriteLine("Robot {0} moved from cell {1}, {2} to cell {3}, {4}.", id, currCell.x, currCell.y, nextCell.x, nextCell.y);
                    Monitor.Exit(this.currCell);
                }
                this.currCell = nextCell;

                //ideally: sort items in Order per location, so that location check doesn't take too much time
                foreach (var product in currOrder.Products)
                {
                    if(product.Location.x == this.currCell.x &&
                        product.Location.y == this.currCell.y)
                    {
                        FetchItem(product);
                    }
                }

                Thread.Sleep(500); //simulates time needed by robot to move to next cell
            }

            this.currWeight = 0;
            Monitor.Exit(this.currCell); //gets to the hangar --> frees last cell
            this.currCell = null;
            Console.WriteLine("Robot {0} joined the hangar.", id);

            Order orderDone = this.currOrder;
            this.currOrder = null;
            this.route = null;
            //set status to waiting again
        }

        public Cell GetCurrCell()
        {
            return this.currCell;
        }

        private void FetchItem(Product p)
        {
            //update weight
            this.currWeight += p.Weight;
            Console.WriteLine("Fetching {0}...", p.ProductName);
            Thread.Sleep(1500);
            Console.WriteLine("{0} fetched !", p.ProductName);
        }

        private static List<Tuple<int, int>> FindRoute(List<Location> locations)
        {
            List<Tuple<int, int>> route = new List<Tuple<int, int>>();
            locations.Sort();

            /**
             * Step 1 : Get to A1
             * Assumption : Robots are waiting in a hangar
             */

            //get to A-up-5
            route.Add(new Tuple<int, int>(0, Program.map.NRow - 1));
            route.Add(new Tuple<int, int>(1, Program.map.NRow - 1));

            //move on A up
            for (int i = Program.map.NRow; i > 0; i--)
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
            if (currCol != Program.map.NCol - 1)
            {
                route.Add(new Tuple<int, int>(++currCol, currRow)); //move on up aisle
                for (int i = currRow; i > 0; i--)
                {
                    route.Add(new Tuple<int, int>(currCol, i - 1)); //get to row1
                }
                currRow = 0;
                for (int i = currCol; i < Program.map.NCol - 1; i++)
                {
                    route.Add(new Tuple<int, int>(i + 1, currRow));
                }
            }
            currCol = Program.map.NCol - 1;

            for (int i = currRow; i < Program.map.NRow - 1; i++)
            {
                route.Add(new Tuple<int, int>(currCol, i + 1));
            }
            currRow = Program.map.NRow - 1;

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
