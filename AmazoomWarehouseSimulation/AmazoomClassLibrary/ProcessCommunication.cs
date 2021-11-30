using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.IO.Pipes;
using System.Threading;

namespace AmazoomClassLibrary
{
    public static class ProcessCommunication
    {
        public static void SyncPipeClient(AnonymousPipeClientStream Pipe)
        {
            byte[] bytes = { 0x20 };
            Pipe.Write(bytes);
            Pipe.WaitForPipeDrain();
            Console.WriteLine("[WEB SERVER] Pipes SYNCED");
        }

        public static void SyncPipeServer(AnonymousPipeServerStream Pipe)
        {
            byte[] bytes = new byte[1];
            do
            {
                Pipe.Read(bytes, 0, 1);
            } while (bytes[0] != 0x20);
            Thread.Sleep(10);
        }
    }
}
