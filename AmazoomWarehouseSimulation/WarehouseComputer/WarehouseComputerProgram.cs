using System;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.Threading;
using AmazoomClassLibrary;

namespace WarehouseComputer
{
    class WarehouseComputerProgram
    {
        static void Main(string[] args)
        {
            MemoryMappedFile mmf = MemoryMappedFile.CreateNew("SalesFile", 4096, MemoryMappedFileAccess.ReadWrite);
            Process AmazoomWebServerProcess = AmazoomProcess.StartProcess();
            Console.ReadKey();
            Order orderdata = AmazoomProcess.ReadMMF(mmf);
            Console.WriteLine("OrderID:{0}, Product:{1}, Number:{2}", orderdata.OrderID, orderdata.Products[0].name, orderdata.Products[0].NumOfProduct);
            Console.ReadKey();
            AmazoomProcess.ReleaseAll(AmazoomWebServerProcess, mmf);
        }
    }
}
