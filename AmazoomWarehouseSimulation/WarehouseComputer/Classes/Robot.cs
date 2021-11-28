using System;
using System.Collections.Generic;
using AmazoomClassLibrary;
using System.Threading;

namespace WarehouseComputer.Classes
{
    public class Robot
    {
        private int id;
        private int maxWeight;
        private int currWeight;
        private Cell currCell;
        private List<Tuple<int, int>> route;
        public Order currOrder;
        public EventWaitHandle ewh;

        public Robot(int id, int maxWeight, int currWeight)
        {
            this.id = id;
            this.maxWeight = maxWeight;
            this.currWeight = currWeight;

            this.currCell = null;
            this.currOrder = null;
            this.route = new List<Tuple<int, int>>();
            ewh = new EventWaitHandle(false, EventResetMode.ManualReset);
        }

        public void SetRoute(List<Tuple<int, int>> route)
        {
            this.route = route;
        }

        public void GetOrder()
        {
            Console.Write("Robot {0} starting route for : ", id);
            foreach (var p in currOrder.products)
            {
                Console.Write("{0} ", p.name);
            }
            Console.Write(".\n");

            ExecRoute();
        }

        public void Execute()
        {

            while (true)
            {
                if(currOrder == null)
                {
                    ewh.WaitOne();
                }
                else
                {
                    GetOrder();
                }
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
                foreach (var product in currOrder.products)
                {
                    if(product.location.x == this.currCell.x &&
                        product.location.y == this.currCell.y)
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
            Program.OrderDone(orderDone, this);
        }

        public Cell GetCurrCell()
        {
            return this.currCell;
        }

        private void FetchItem(Product p)
        {
            //update weight
            this.currWeight += p.weight;
            Console.WriteLine("Fetching {0}...", p.name);
            Thread.Sleep(1500);
            Console.WriteLine("{0} fetched !", p.name);
        }
    }
}
