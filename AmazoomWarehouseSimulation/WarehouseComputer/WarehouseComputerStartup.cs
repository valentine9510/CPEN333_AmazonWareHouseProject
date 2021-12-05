using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using AmazoomClassLibrary;
using WarehouseComputer.Classes;

namespace WarehouseComputer
{
    public static class WarehouseComputerStartup
    {
        public static Process InitWebServerProcessAndPipe()
        {
            Process ProcessInstance = new Process();
            ProcessInstance.StartInfo.UseShellExecute = false;
            string startupPath = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            string fullPath = Path.Combine(startupPath, "AmazoomWebServer/bin/Debug/netcoreapp3.1/AmazoomWebServer.exe");
            ProcessInstance.StartInfo.FileName = fullPath;
            ProcessInstance.StartInfo.CreateNoWindow = false;

            AnonymousPipeServerStream Pipe = new AnonymousPipeServerStream(PipeDirection.In, System.IO.HandleInheritability.Inheritable);
            ProcessInstance.StartInfo.Arguments = Pipe.GetClientHandleAsString();
            ProcessInstance.Start();
            Console.WriteLine("[WH COMPUTER] Web Server process started \n");
            Pipe.DisposeLocalCopyOfClientHandle();
            Thread ReadOrder = new Thread(() => WarehouseComputerCommThread.Execute(Pipe, Program.OrdersFromWebServer));
            ReadOrder.Start();
            return ProcessInstance;
        }

        public static WarehouseMap InitWarehouseMap()
        {
            Console.WriteLine("Hello! Welcome to the Amazoom Warehouse Simulation\n");
            Console.WriteLine("Please enter the Warehouse Dimensions below:-");
            string row;
            string column;
            string shelf;
            do
            {
                Console.Write("No. of Rows: ");
                row = Console.ReadLine();
            } while (int.TryParse(row, out int value) == false);
            do
            {
                Console.Write("No. of Columns: ");
                column = Console.ReadLine();
            } while (int.TryParse(column, out int value) == false);
            do
            {
                Console.Write("No. of shelves: ");
                shelf = Console.ReadLine();
            } while (int.TryParse(shelf, out int value) == false);

            Console.WriteLine("");
            return new WarehouseMap(int.Parse(row), int.Parse(column), int.Parse(shelf));
        }

        public static string InitProductInventory(WarehouseMap map)
        {
            bool stay_in_while = true;
            string filename = "";
            while (stay_in_while)
            {
                Console.WriteLine("Please enter the name of the preferred database file: ");
                filename = Console.ReadLine();
                switch (filename)
                {
                    case ("FruitDatabase.json"):
                        stay_in_while = false;
                        break;
                    case ("ShoeDatabase.json"):
                        stay_in_while = false;
                        break;
                    default:
                        Console.WriteLine("Sorry, this database doesnt exit. Try again");
                        break;
                }
            }

            map.Inventory = JSONFile.GetProducts(filename);
            map.PopulateShelves();
            JSONFile.ConvertProductToJSON(map.Inventory, filename);
            return filename;
        }
    }
}
