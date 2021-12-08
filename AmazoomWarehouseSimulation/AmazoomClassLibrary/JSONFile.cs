using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;

namespace AmazoomClassLibrary
{
    public static class JSONFile
    {
        public static List<Product> GetProducts(string filename) 
        {
            string startupPath = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            return JsonConvert.DeserializeObject<List<Product>>(File.ReadAllText(Path.Combine(startupPath, filename)));
        }

        public static void ConvertProductToJSON(List<Product> inputproduct, string filename)
        {
            string startupPath = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            string json = JsonConvert.SerializeObject(inputproduct);
            //Console.WriteLine(json);
            File.WriteAllText(Path.Combine(startupPath, filename), json);
        }

        public static List<Order> GetOrders(string filename = "CompletedOrders.json")
        {
            //"C:\\Users\\hp\\source\\repos\\CPEN333_AmazonWareHouseProject\\AmazoomWarehouseSimulation\\CompletedOrders.json"
            string startupPath = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            var text = JsonConvert.DeserializeObject<List<Order>>(File.ReadAllText(Path.Combine(startupPath, filename)));
            if(text == null)
            {
                return new List<Order>();
            }

            return text;
        }

        public static void ConvertOrderToJSON(List<Order> inputOrder, string filename = "CompletedOrders.json")
        {
            //"C:\\Users\\hp\\source\\repos\\CPEN333_AmazonWareHouseProject\\AmazoomWarehouseSimulation\\CompletedOrders.json"
            string startupPath = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            string json = JsonConvert.SerializeObject(inputOrder);
            Console.WriteLine(json);
            File.WriteAllText(Path.Combine(startupPath, filename), json);
        }

        public static void AddOrderToJSONDatabase(Order inputOrder)
        {

            List<Order> pastOrders = GetOrders();
            if (pastOrders.Count >= 1)
            {
                pastOrders.Add(inputOrder);
                //Write to DB
                ConvertOrderToJSON(pastOrders);
                //Console.WriteLine("______Adding to old file________");
            } else
            {
                List<Order> tempList = new List<Order>();
                tempList.Add(inputOrder);
                //Console.WriteLine("______Creating new list________");
                ConvertOrderToJSON(tempList);
            }
            
        }

        public static void ClearJSONFile(string filename = "CompletedOrders.json")
        {
            string startupPath = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            string json = "";
            File.WriteAllText(Path.Combine(startupPath, filename), json);
        }

    }
}
