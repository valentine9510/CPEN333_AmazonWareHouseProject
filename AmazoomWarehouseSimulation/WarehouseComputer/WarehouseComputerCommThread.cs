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
        public static void Execute(AnonymousPipeServerStream Pipe)
        {
            BinaryFormatter StreamReader = new BinaryFormatter();
            while (true)
            {
                ProcessCommunication.SyncPipeServer(Pipe);
                //Console.WriteLine("[WH COMPUTER] SYNCED");
                Order RecievedOrder = new Order();
                RecievedOrder = StreamReader.Deserialize(Pipe) as Order;
                Program.AddOrder(RecievedOrder);
                //Console.WriteLine("[WH COMPUTER] OrderId:{0}, Product:{1}, Num:{2}, Weight:{3}", RecievedOrder.OrderID, RecievedOrder.Products[0].ProductName, RecievedOrder.Products[0].NumOfProduct, RecievedOrder.OrderWeight);
                //Console.WriteLine(RecievedOrder.Products[0].Location);
            }
        }
    }
}
