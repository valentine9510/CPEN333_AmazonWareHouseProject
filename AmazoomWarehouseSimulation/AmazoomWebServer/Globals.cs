using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AmazoomClassLibrary;

namespace AmazoomWebServer
{
    public static class Globals
    {
        public static NamedPipeClientStream WebServerPipe;

        public static Queue<Order> OrderQueue = new Queue<Order>();

        public static string DatabaseName;

        private static Mutex mutex = new Mutex();

        public static void AddToQueue(Order OrderedProduct)
        {
            mutex.WaitOne();
            OrderQueue.Enqueue(OrderedProduct);
            mutex.ReleaseMutex();
        }

        public static Order TakeFromQueue()
        {
            mutex.WaitOne();
            Order ReturnOrder = OrderQueue.Dequeue();
            mutex.ReleaseMutex();
            return ReturnOrder;
        }

        public static int ReturnQueueCount()
        {
            mutex.WaitOne();
            int count = OrderQueue.Count;
            mutex.ReleaseMutex();
            return count;
        }
    }
}
