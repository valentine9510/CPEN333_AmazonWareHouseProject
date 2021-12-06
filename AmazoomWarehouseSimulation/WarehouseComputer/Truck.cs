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
        public Object WaitingObj;
        //public EventWaitHandle Ewh;
        public bool ReadyToLeave;

        public Truck(int id, int max, int curr)
        {
            Id = id;
            MaxWeight = max;
            CurrWeight = curr;
            WeightMonitor = new object();
            WaitingObj = new object();
            //Ewh = new EventWaitHandle(false, EventResetMode.ManualReset);
            ReadyToLeave = false;
        }

        public void WaitForDelivery()
        {
            while (true)
            {
                Program.AddTruck(this);
                //Ewh.WaitOne();
                lock (WaitingObj)
                {
                    Monitor.Wait(WaitingObj);
                }
                LeaveDock();
            }
        }

        public void LeaveDock()
        {
            Console.WriteLine($"Truck {Id} is leaving for delivery!");
            Thread.Sleep(30000);
            lock (WeightMonitor)
            {
                this.ReservedWeight = 0;
                this.CurrWeight = 0;
                this.ReadyToLeave = false;
            }
            Console.WriteLine($"Truck {Id} successfully delivered its orders!");
        }
    }
}
