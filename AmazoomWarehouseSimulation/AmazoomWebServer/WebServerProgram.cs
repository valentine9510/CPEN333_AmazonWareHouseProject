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

namespace AmazoomWebServer
{
    public class WebServerProgram
    {
        public static void Main(string[] args)
        {
            //MemoryMappedFile mmf = MemoryMappedFile.OpenExisting("SalesFile", MemoryMappedFileRights.Write);
            //Order Sale1 = new Order();
            //Sale1.OrderID = 1;
            //AmazoomProcess.WriteMMF(Sale1, mmf);
            CreateHostBuilder(args).Build().Run();
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
