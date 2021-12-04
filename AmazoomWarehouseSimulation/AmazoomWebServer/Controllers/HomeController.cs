using AmazoomWebServer.Models;
using AmazoomClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AmazoomWebServer.Controllers
{
    public class HomeController : Controller
    {
        static ProductInventory tempInventory = new ProductInventory();

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Orderspage()
        {
            

            return View(tempInventory);
        }

        [HttpPost]
        public ActionResult AddToOrder(int qty, string currentItemName, int currentItemNum)
        {
            //Product tempProduct = currentItem;
            if (qty > currentItemNum || qty < 0) /*Avoid negative values*/
            {
                if(qty > currentItemNum)
                {
                    tempInventory.currentAlert = new Alert(true, "Input of order must be less that available quantity of " + currentItemName);
                } else if (qty < 0)
                {
                    tempInventory.currentAlert = new Alert(true, "Cannot input negative numbers");
                }
                return View("Orderspage", tempInventory);
            }
            else
            {
                /*Reduce on the number of products in inventory*/
                foreach(var item in ProductInventory.availableProducts)
                {
                    if (item.ProductName == currentItemName)
                    {
                        item.NumOfProduct-= qty;
                    }
                }
                ProductInventory.currentOrder.AddProduct(new Product(currentItemName, qty)); //Add to current order
            }
            return View("Orderspage",tempInventory);
        }

        [HttpPost]
        public ActionResult AddOrderToQueue()
        {
            if (ProductInventory.currentOrder.Products.Count >= 1) /* Only add if it has at least a product */
            {
                Order tempOrder = ProductInventory.currentOrder;
                ProductInventory.uncompletedOrders.Enqueue(tempOrder);

                /* Reset order, add one to ID for next Order */
                ProductInventory.currentOrder = new Order(ProductInventory.currentOrder.OrderID + 1);
            }
            
            return View("Orderspage", tempInventory);
        }

        [HttpPost]
        public ActionResult ClearOrder()
        {
            /* Clear  order*/
            if(ProductInventory.currentOrder.Products != null)
            {
                // Add back items to inventory
                //Clear current order
                foreach (var item in ProductInventory.currentOrder.Products)
                {
                    foreach (var inventoryItem in ProductInventory.availableProducts)
                    {
                        if (item.ProductName == inventoryItem.ProductName)
                        {
                            inventoryItem.NumOfProduct += item.NumOfProduct; 
                        }
                    }
                }
                ProductInventory.currentOrder.Products.Clear();
            }
            

            return View("Orderspage", tempInventory);
        }

        [HttpPost]
        public ActionResult ClearsOrdersInProgress()
        {
            if (ProductInventory.uncompletedOrders.Count >= 1)
            {
                ProductInventory.uncompletedOrders.Clear();
            }
            return View("Orderspage", tempInventory);
        }

        [HttpPost]
        public ActionResult ClearsCompletedOrder()
        {
            if (ProductInventory.completedOrders.Count >= 1)
            {

                ProductInventory.completedOrders.Clear();
            }
            return View("Orderspage", tempInventory);
        }

        public ActionResult removeAlert()
        {
            tempInventory.currentAlert.alertMessage = "";
            tempInventory.currentAlert.showAlert = false;
            return View("Orderspage", tempInventory);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
