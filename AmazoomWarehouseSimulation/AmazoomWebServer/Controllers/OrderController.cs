using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmazoomClassLibrary;
using System.IO.MemoryMappedFiles;

namespace AmazoomWebServer.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index(string Product, int Number)
        {
            Console.WriteLine("Product:{0}, Number:{1}", Product, Number);
            Product orderedProduct = new Product() { name = Product, NumOfProduct = Number };
            Order sale = new Order(1);
            sale.AddProduct(orderedProduct);
            MemoryMappedFile mmf = MemoryMappedFile.OpenExisting("SalesFile", MemoryMappedFileRights.Write);
            AmazoomProcess.WriteMMF(sale, mmf);
            return Content(string.Format("Product:{0}, Number:{1}", Product, Number));
        }
    }
}
