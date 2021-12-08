using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AmazoomWebServer.Models;
using System.Runtime.Serialization.Formatters.Binary;
using AmazoomClassLibrary;
using System.IO.Pipes;

namespace AmazoomWebServer
{
    public class WebServerProgram
    {
        public static void Main(string[] args)
        {
            //string PipeHandle = args[0];
            //ServerCommThread ThreadInstance = new ServerCommThread(PipeHandle);
            //Thread SendOrders = new Thread(new ThreadStart(ThreadInstance.Execute));
            //SendOrders.Start();
            /* Sample */

            CreateHostBuilder(args).Build().Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<WebServerStartup>();
                });
    }
}
