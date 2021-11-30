using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using AmazoomClassLibrary;

namespace WarehouseComputer
{
    public class WarehouseComputerStartup
    {
        private Process AmazoomWebServerProcess;
        public AnonymousPipeServerStream WarehouseComputerPipe;

        public WarehouseComputerStartup()
        {
            AmazoomWebServerProcess = new Process();
            AmazoomWebServerProcess.StartInfo.UseShellExecute = false;
            AmazoomWebServerProcess.StartInfo.FileName = "C:/Users/karth/Desktop/CPEN333_AmazonWareHouseProject/AmazoomWarehouseSimulation/AmazoomWebServer/bin/Debug/netcoreapp3.1/AmazoomWebServer.exe";
            AmazoomWebServerProcess.StartInfo.CreateNoWindow = false;
            
            WarehouseComputerPipe = new AnonymousPipeServerStream(PipeDirection.In, System.IO.HandleInheritability.Inheritable);
            AmazoomWebServerProcess.StartInfo.Arguments = WarehouseComputerPipe.GetClientHandleAsString();
            //Console.WriteLine(AmazoomWebServerProcess.StartInfo.Arguments);
            AmazoomWebServerProcess.Start();
            Console.WriteLine("[WH COMPUTER] Web Server process started \n");
            WarehouseComputerPipe.DisposeLocalCopyOfClientHandle();
        }

        ~WarehouseComputerStartup()
        {
            AmazoomWebServerProcess.Dispose();
        }
    }
}
