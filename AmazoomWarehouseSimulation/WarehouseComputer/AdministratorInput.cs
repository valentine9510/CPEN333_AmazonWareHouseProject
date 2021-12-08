using AmazoomClassLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using WarehouseComputer.Classes;

namespace WarehouseComputer
{
    class AdministratorInput
    {
        public static void ReadInput()
        {
            while(true)
            {
                string response;
                ConsoleKey keyInput = Console.ReadKey().Key;
                if (keyInput == ConsoleKey.Enter)
                {
                    Console.WriteLine("Administrator Console!!");
                    Console.Write("Please Enter desired action: ");
                    response = Console.ReadLine();

                    switch(response)
                    {
                        case ("AddRobot"):
                            Console.Write("Enter robot ID: ");
                            string Id;
                            Id = Console.ReadLine();
                            Robot newRobot = new Robot(int.Parse(Id), 10, 0);
                            Thread newThread = new Thread(() => newRobot.Execute());
                            newThread.Start();
                            Console.WriteLine("Added Robot {0}", Id);
                            break;
                        case ("AddProduct"):
                            string name;
                            string number;
                            Console.Write("Enter Product Name: ");
                            name = Console.ReadLine();
                            Console.Write("Enter Stock: ");
                            number = Console.ReadLine();
                            Product newProduct = new Product(name, int.Parse(number), 0.5);
                            Program.map.AddProductToInventory(newProduct, Program.CurrentDatabaseName);
                            Console.WriteLine("Add Product {0} to inventory", name);
                            break;

                        default:
                            break;
                    }
                }    

            }
        }
    }
}
