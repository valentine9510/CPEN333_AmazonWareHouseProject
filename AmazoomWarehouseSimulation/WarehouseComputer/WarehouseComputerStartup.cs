using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            string startupPath = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            string fullPath = Path.Combine(startupPath, "AmazoomWebServer/bin/Debug/netcoreapp3.1/AmazoomWebServer.exe");
            AmazoomWebServerProcess.StartInfo.FileName = fullPath;
            AmazoomWebServerProcess.StartInfo.CreateNoWindow = false;

            WarehouseComputerPipe = new AnonymousPipeServerStream(PipeDirection.In, System.IO.HandleInheritability.Inheritable);
            AmazoomWebServerProcess.StartInfo.Arguments = WarehouseComputerPipe.GetClientHandleAsString();
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
