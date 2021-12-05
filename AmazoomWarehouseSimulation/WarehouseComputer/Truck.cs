using System;
using AmazoomClassLibrary;
using System.Threading;


namespace WarehouseComputer
{
    public class Truck
    {
        public int Id;
        public Location Location;
        public double MaxWeight;
        public double CurrWeight;
        public double ReservedWeight;
        public int Dock;
        public Object WeightMonitor;

        public Truck(int id, int max, int curr)
        {
            Id = id;
            MaxWeight = max;
            CurrWeight = curr;
        }

        public void LeaveDock()
        {
            Console.WriteLine($"Truck {Id} is leaving for delivery!");
            Thread.Sleep(15000);
            //Notify warehouse computer that it has arrived back
        }
    }
}
