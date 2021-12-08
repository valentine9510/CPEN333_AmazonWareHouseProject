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
            /* Update uncompleted orders from Global queue */
            ProductInventory.uncompletedOrders = Globals.OrderQueue;            

            return View(tempInventory);
        }

        [HttpPost]
        public ActionResult AddToOrder(int qty, string currentItemName, int currentItemNum)
        {

            //Product tempProduct = currentItem;
            if (qty > currentItemNum || qty <= 0) /*Avoid negative values*/
            {
                if(qty > currentItemNum)
                {
                    tempInventory.currentAlert.alertMessage = "Inventory has maximum of "+ currentItemNum + " " + currentItemName + "s";
                    tempInventory.currentAlert.showAlert = true;
                } else if (qty < 0)
                {
                    tempInventory.currentAlert.alertMessage = "Cannot order negative " + currentItemName + "s";
                    tempInventory.currentAlert.showAlert = true;
                } else
                {
                    tempInventory.currentAlert.alertMessage = "Cannot order 0 " + currentItemName + "s";
                    tempInventory.currentAlert.showAlert = true;
                }
                return View("Orderspage", tempInventory);
            }
            else
            {
                /*Reduce on the number of products in inventory*/
                Location productlocation = null; 
                double productweight = 0;
                foreach (var item in ProductInventory.availableProducts)
                {
                    if (item.ProductName == currentItemName)
                    {
                        item.NumOfProduct-= qty;
                        productlocation = item.Location;
                        productweight = item.Weight;
                    }
                }
                ProductInventory.currentOrder.AddProduct(new Product(currentItemName, qty, productlocation, productweight)); //Add to current order
            }
            return View("Orderspage",tempInventory);
        }

        [HttpPost]
        public ActionResult AddOrderToQueue()
        {
            if (ProductInventory.currentOrder.Products.Count >= 1) /* Only add if it has at least a product */
            {
                Order tempOrder = ProductInventory.currentOrder;
                //ProductInventory.uncompletedOrders.Enqueue(tempOrder);
                Globals.AddToQueue(tempOrder);

                /* Reset order, add one to ID for next Order */
                ProductInventory.currentOrder = new Order(ProductInventory.currentOrder.OrderID + 1);

                /* Update database */
                JSONFile.ConvertProductToJSON(ProductInventory.availableProducts, Globals.DatabaseName);
            }

            /* Update uncompleted orders from Global queue */
            ProductInventory.uncompletedOrders = Globals.OrderQueue;

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

        [HttpPost]
        public ActionResult AddToInventory( string newProductName, int newProductQty)
        {
            if (newProductQty < 0 || newProductName.Length < 1)
            {
                return View("Orderspage", tempInventory);
            }
            
            foreach( var product in ProductInventory.availableProducts)
            {
                if(product.ProductName == newProductName)
                {
                    product.NumOfProduct += newProductQty;
                    return View("Orderspage", tempInventory);

                }
            }

            ProductInventory.availableProducts.Add(new Product(newProductName, newProductQty));

            return View("Orderspage", tempInventory);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
