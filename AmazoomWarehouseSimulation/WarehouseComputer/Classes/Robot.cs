using System;
using System.Collections.Generic;

namespace WarehouseComputer.Classes
{
    public class Robot
    {
        private int id;
        private int maxWeight;
        private int currWeight;
        private Tuple<int, int> location;
        private List<Tuple<int, int>> route;
        //private Order currOrder;
        //WarehouseMap as global variable --> defined in WarehouseComputer (?)

        public Robot(int id, int maxWeight, int currWeight, Tuple<int, int> location)
        {
            this.id = id;
            this.maxWeight = maxWeight;
            this.currWeight = currWeight;
            this.location = location;
            this.route = new List<Tuple<int, int>>();
        }

        public void SetRoute(List<Tuple<int, int>> route)
        {
            this.route = route;
        }

        private void ExecRoute()
        {
            Console.WriteLine("Robot {0} is starting a new route !", this.id);
            foreach (var cell in this.route)
            {
                /**
                 * Get lock of cell, check if cell available
                 * If it's available go there (+ signal cv of prev cell), else sleep on cv
                 * Release lock
                 */
                this.location = cell;
                //if item at that location --> call FetchItem()
                //ideally: sort items in Order per location, so that location check doesn't take too much time
            }
        }

        public Tuple<int, int> GetLocation()
        {
            return this.location;
        }

        private void FetchItem()
        {
            //update weight
            //spend some time on that (to simulate operation)
            Console.WriteLine("Item fetched !");
        }
    }
}
