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
            Product OrderedProduct = new Product(Product, Number);
            Order Sale = new Order();
            //Globals.OrderIDCounter++;
            Sale.AddProduct(OrderedProduct);
            Globals.AddToQueue(Sale);

            
            return Content(string.Format("Product:{0}, Number:{1}", Product, Number));
        }
    }
}
