using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseComputer.Classes;
using AmazoomClassLibrary;
using System.IO;
using System.Diagnostics;
using System.IO.Pipes;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace WarehouseComputer
{

    class Program
    {

        static void Main(string[] args)
        {
            WarehouseComputerStartup StartupObject = new WarehouseComputerStartup();
            AnonymousPipeServerStream WarehouseComputerPipe = StartupObject.WarehouseComputerPipe;
            Queue<Order> OrdersFromWebServer = new Queue<Order>();
            Thread ReadOrder = new Thread(() => WarehouseComputerCommThread.Execute(WarehouseComputerPipe, OrdersFromWebServer));
            ReadOrder.Start();

            Console.ReadKey();
            foreach (Order order in OrdersFromWebServer)
            {
                Console.WriteLine("ID:{0}, Product:{1}, Num:{2}\n", order.OrderID, order.Products[0].name, order.Products[0].NumOfProduct);
            }
        }
    }

}