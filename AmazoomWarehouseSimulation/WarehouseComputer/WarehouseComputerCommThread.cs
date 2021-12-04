using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using AmazoomClassLibrary;

namespace WarehouseComputer
{
    public static class WarehouseComputerCommThread
    {
        public static void Execute(AnonymousPipeServerStream Pipe, Queue<Order> OrderQueue)
        {
            BinaryFormatter StreamReader = new BinaryFormatter();
            while (true)
            {
                ProcessCommunication.SyncPipeServer(Pipe);
                Console.WriteLine("[WH COMPUTER] SYNCED");
                Order RecievedOrder = new Order();
                RecievedOrder = StreamReader.Deserialize(Pipe) as Order;
                OrderQueue.Enqueue(RecievedOrder);
                //Console.WriteLine("[WH COMPUTER] OrderId:{0}, Product:{1}, Num:{2}", RecievedOrder.OrderID, RecievedOrder.Products[0].ProductName, RecievedOrder.Products[0].NumOfProduct);
            }
        }
    }
}
