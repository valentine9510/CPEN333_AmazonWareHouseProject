using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace AmazoomClassLibrary
{
    public static class AmazoomProcess
    {


        public static Process StartProcess()
        {
            Process AmazoomWebServerProcess = new Process();
            AmazoomWebServerProcess.StartInfo.UseShellExecute = true;
            // You can start any process, HelloWorld is a do-nothing example.
            AmazoomWebServerProcess.StartInfo.FileName = "C:/Users/karth/Desktop/CPEN333_AmazonWareHouseProject/AmazoomWarehouseSimulation/AmazoomWebServer/bin/Debug/netcoreapp3.1/AmazoomWebServer.exe";
            AmazoomWebServerProcess.StartInfo.CreateNoWindow = false;
            AmazoomWebServerProcess.Start();
            return AmazoomWebServerProcess;
        }

        public static Order ReadMMF(MemoryMappedFile mmf)
        {
            MemoryMappedViewStream stream = mmf.CreateViewStream();
            Console.WriteLine("read offset: {0}", stream.PointerOffset);
            BinaryFormatter serializer = new BinaryFormatter();
            Order data = serializer.Deserialize(stream) as Order;
            stream.Close();
            return data;
        }

        public static void WriteMMF(Order Sale, MemoryMappedFile mmf)
        {
            MemoryMappedViewStream stream = mmf.CreateViewStream();
            Console.WriteLine("write offset: {0}", stream.PointerOffset);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(stream, Sale);
            stream.Seek(0, SeekOrigin.Begin);
            stream.Close();
        }

        public static void ReleaseAll(Process ProcessInstance, MemoryMappedFile mmf)
        {
            mmf.Dispose();
            ProcessInstance.Close();
        }
    }
}
