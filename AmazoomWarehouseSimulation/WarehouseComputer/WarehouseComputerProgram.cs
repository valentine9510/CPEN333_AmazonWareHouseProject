using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseComputer.Classes;

namespace WarehouseComputer
{

    class Program
    {
        public static WarehouseMap map;

        static void Main(string[] args)
        {
            map = new WarehouseMap(15, 5);

            // Route testing
            List<Location> locations = new List<Location>();
            locations.Add(new Location(4, 2, 0, 0));
            locations.Add(new Location(2, 3, 0, 0));
            locations.Add(new Location(2, 2, 0, 0));
            locations.Add(new Location(8, 3, 0, 0));
            locations.Add(new Location(12, 2, 0, 0));

            Robot robot = new Robot(1, 1, 1, new Tuple<int, int>(4, 4));
            List<Tuple<int, int>> route = FindRoute(locations, robot);
            int count = 0;
            foreach (var cell in route)
            {
                Console.WriteLine("Cell {0} of route : {1}, {2}", count++, cell.Item1, cell.Item2);
            }
        }

        private static List<Tuple<int, int>> FindRoute(List<Location> locations, Robot robot)
        {
            List<Tuple<int, int>> route = new List<Tuple<int, int>>();
            locations.Sort();
            Console.WriteLine("locations sorted :");
            foreach (var l in locations)
            {
                Console.WriteLine("{0}, {1}", l.x, l.y);
            }
            Tuple<int, int> current = robot.GetLocation();

            /**
             * Step 1 : Get to A1
             * Assumption : Robots end their route at bottom row
             */
            //get to A-up-5
            if (current.Item1 > 1)
            {
                for (int i = current.Item1; i > 1; i--)
                {
                    route.Add(new Tuple<int, int>(i - 1, current.Item2));
                }
            }

            //move on A up
            for (int i = current.Item2; i > 0; i--)
            {
                route.Add(new Tuple<int, int>(1, i - 1));
            }
            Console.WriteLine("current location is {0}, {1}", 1, 0);


            /**
             * Step 2 : Get to each location
             */
            int currCol = 1;
            int currRow = 0; //we start at A1

            foreach (var loc in locations)
            {
                //items on A aisle are already collected while going to A1
                if (loc.x != 0)
                {
                    //if we have to switch columns --> get to right column
                    if (loc.x > currCol)
                    {
                        //if we're not on row 1 --> we were on down aisle
                        if (currRow != 0)
                        {
                            route.Add(new Tuple<int, int>(++currCol, currRow)); //move on up aisle
                            Console.WriteLine("Adding cell {0}, {1}", currCol, currRow);
                            for (int i = currRow; i > 0; i--)
                            {
                                route.Add(new Tuple<int, int>(currCol, i - 1)); //get to row1
                                Console.WriteLine("Adding cell {0}, {1}", currCol, i - 1);
                            }
                            currRow = 0;
                        }

                        //get to the right column
                        for (int i = currCol; i < loc.x; i++)
                        {
                            route.Add(new Tuple<int, int>(i + 1, currRow));
                            Console.WriteLine("Adding cell {0}, {1}", i + 1, currRow);
                        }
                        currCol = loc.x;
                    }

                    //go down to the right location
                    for (int i = currRow; i < loc.y; i++)
                    {
                        route.Add(new Tuple<int, int>(currCol, i + 1));
                        Console.WriteLine("Adding cell {0}, {1}", currCol, i + 1);
                    }
                    currRow = loc.y;

                    Console.WriteLine("current location is {0}, {1}", currCol, currRow);
                }
            }

            /**
             * Step 3 : Get to truck
             */

            //if last item was not on last column, get there
            if (currCol != map.nCol - 1)
            {
                route.Add(new Tuple<int, int>(++currCol, currRow)); //move on up aisle
                for (int i = currRow; i > 0; i--)
                {
                    route.Add(new Tuple<int, int>(currCol, i - 1)); //get to row1
                }
                currRow = 0;
                for (int i = currCol; i < map.nCol - 1; i++)
                {
                    route.Add(new Tuple<int, int>(i + 1, currRow));
                }
            }
            currCol = map.nCol - 1;

            for (int i = currRow; i < map.nRow - 1; i++)
            {
                route.Add(new Tuple<int, int>(currCol, i + 1));
            }
            currRow = map.nRow - 1;

            //for now : truck at H5, later : truck.location.x
            for (int i = currCol; i > 5 * 2 - 1; i--)
            {
                route.Add(new Tuple<int, int>(i - 1, currRow));
            }
            return route;
        }
    }

}