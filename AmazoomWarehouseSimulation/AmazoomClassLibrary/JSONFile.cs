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
    }
}
