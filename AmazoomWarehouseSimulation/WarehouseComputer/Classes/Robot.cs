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

        private Location truckLocation;
        private Truck truck;
        private bool alreadyTriedTruck;
        //public Mutex robotmutex;

        public Robot(int id, double maxWeight, double currWeight, Mutex mutex)
        {
            this.id = id;
            this.maxWeight = maxWeight;
            this.currWeight = currWeight;

            this.currCell = null;
            this.currOrder = null;
            this.route = new List<Tuple<int, int>>();
            //this.robotmutex = mutex;
            alreadyTriedTruck = false;
        }

        public void SetRoute(List<Tuple<int, int>> route)
        {
            this.route = route;
        }

        public void Execute()
        {

            while (true)
            {
                GetOrder();
                GetAvailableTruck();
                List<Location> productlocations = new List<Location>();
                foreach (Product p in currOrder.Products)
                {
                    productlocations.Add(p.Location);
                }
                this.route = FindRoute(productlocations);
                ExecRoute();
            }
        }

        public void GetAvailableTruck()
        {
            Truck truck;
            lock (Program.Docks)
            {
                if(Program.Docks.Count == 0)
                {
                    Console.WriteLine($"Robot {id} is waiting for trucks...");
                    Monitor.Wait(Program.Docks);
                }
                
                if(Program.Docks.Count == 1)
                {
                    truck = Program.Docks[0];
                }
                else
                {
                    truck = Program.Docks[Program.NextTruck];
                    //Console.WriteLine($"Next truck picked is {Program.NextTruck}.");
                    Program.NextTruck = (Program.NextTruck + 1) % 2;
                }
                
            }

            Monitor.Enter(truck.WeightMonitor);

            //If a truck's reserved weight is too big for an order, this truck should leave
            //Or if this truck is already set to leave, the robot should not try to fill it more
            if (truck.ReservedWeight + this.currOrder.OrderWeight > truck.MaxWeight || truck.ReadyToLeave)
            {
                truck.ReadyToLeave = true;
                Monitor.Exit(truck.WeightMonitor);

                //if there is another truck at the dock, the robot will try with this one, otherwise it will wait.
                lock (Program.Docks)
                {
                    if(Program.Docks.Count == 2)
                    {
                        if (alreadyTriedTruck)
                        {
                            Console.WriteLine($"Robot {id} is waiting for trucks because all trucks are reserved...");
                            Monitor.Wait(Program.Docks);
                            alreadyTriedTruck = false;
                            GetAvailableTruck(); //the robot should start again if it was sleeping
                            return;
                        }
                        else
                        {
                            //Console.WriteLine($"Robot {id} will try another truck!");
                            alreadyTriedTruck = true;
                            GetAvailableTruck();
                            return;
                        }
                        
                    }
                    else
                    {
                        Console.WriteLine($"Robot {id} is waiting for trucks because all trucks are reserved (2)...");
                        Monitor.Wait(Program.Docks);
                        GetAvailableTruck(); //the robot should start again if it was sleeping
                        return;
                    }
                }

            }
            else
            {
                if(truck.Dock == 0)
                {
                    this.truckLocation = new Location(3, Program.map.NRow - 1, 0, 0);
                }
                else
                {
                    this.truckLocation = new Location(2, Program.map.NRow - 1, 0, 0);
                }
                truck.ReservedWeight += currOrder.OrderWeight;
                Console.WriteLine($"Truck {truck.Id} has {truck.ReservedWeight}kg reserved.");
                Monitor.Exit(truck.WeightMonitor);

            }
            this.truck = truck;
            alreadyTriedTruck = false;
            Console.WriteLine($"Robot {id} picked truck {truck.Id}.");
        }


        public void GetOrder()
        {
            lock (Program.WaitingOrders)
            {
                if(Program.WaitingOrders.Count == 0)
                {
                    Console.WriteLine($"Robot {id} is waiting for orders...");
                    Monitor.Wait(Program.WaitingOrders);
                }
                //Console.WriteLine($"Robot {id} is awake!");
                this.currOrder = Program.WaitingOrders.Dequeue();
            }

            /*Console.Write("Robot {0} starting route for : ", id);

            foreach (Product p in currOrder.Products)
            {
                Console.Write($"{p.ProductName} ");
            }
            Console.Write(".\n");*/
        }


        private void ExecRoute()
        {

            //Console.WriteLine("Robot {0} is starting a new route !", this.id);
            foreach (var nextLocation in this.route)
            {
                Cell nextCell = Program.map.GetCell(nextLocation.Item2, nextLocation.Item1); //Find route impl. with x-y, getCoord with row-col

                Monitor.Enter(nextCell);

                if(currCell != null)
                {
                    //Console.WriteLine("Robot {0} moved from cell {1}, {2} to cell {3}, {4}.", id, currCell.x, currCell.y, nextCell.x, nextCell.y);
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

                if(currCell.x == truckLocation.x && currCell.y == truckLocation.y)
                {
                    LoadTruck();
                }

                Thread.Sleep(500); //simulates time needed by robot to move to next cell
            }

            this.currWeight = 0;
            Monitor.Exit(this.currCell); //gets to the hangar --> frees last cell
            this.currCell = null;
            //Console.WriteLine("Robot {0} joined the hangar.", id);

            Order orderDone = this.currOrder;
            this.currOrder = null;
            this.route = null;
            //set status to waiting again
        }

        public void LoadTruck()
        {
            foreach (var product in currOrder.Products)
            {
                //Console.WriteLine($"Loading truck {truck.Id} with {product.ProductName}.");
                Thread.Sleep(1500); //simulates loading truck
                lock (truck.WeightMonitor)
                {
                    truck.CurrWeight += currOrder.OrderWeight;
                    Console.WriteLine($"Robot {id}loaded truck {truck.Id}, it now contains {truck.CurrWeight}kg out of {truck.MaxWeight}.");
                    if(truck.CurrWeight == truck.ReservedWeight && truck.ReadyToLeave)
                    {
                        TellTruckToLeave();
                    }
                }
                
            }
            //Console.WriteLine($"Truck {truck.Id} loaded !");
            this.truck = null;
        }

        private void TellTruckToLeave()
        {
            /**
             * Whole critical section synchronized on Docks, because we don't want to have inconsistent Docks-WaitingTrucks results
             * Ex. : If a robot tells a truck to leave and removes it from the dock while another truck was arriving at the warehouse,
             * then the program could have added the new truck to the dock (because a spot was free), but then when the robot tries to
             * dequeue another truck and add it to the dock as well, the spots will be filled. 
             * Hence, we need to lock both Docks and the WaitingTrucks queue together.
             */
            lock (Program.Docks)
            {
                Program.Docks.Remove(truck);
                lock (truck.WaitingObj)
                {
                    Monitor.Pulse(truck.WaitingObj);
                }
                if (Program.WaitingTrucks.Count > 0)
                {
                    Truck newTruck = Program.WaitingTrucks.Dequeue();
                    newTruck.Dock = Program.Docks.Count;
                    Program.Docks.Add(newTruck);
                    Console.WriteLine($"Truck {newTruck.Id} is waiting at dock {Program.Docks.Count - 1}");
                }
            }
        }

        public Cell GetCurrCell()
        {
            return this.currCell;
        }

        private void FetchItem(Product p)
        {
            //update weight
            this.currWeight += p.Weight;
            Console.WriteLine($"Robot {id} fetching {p.ProductName}...");
            Thread.Sleep(1500);
            //Console.WriteLine("{0} fetched !", p.ProductName);
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
