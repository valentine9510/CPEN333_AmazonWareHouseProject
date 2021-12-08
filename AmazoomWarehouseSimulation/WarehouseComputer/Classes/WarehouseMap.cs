using System;
using System.Collections.Generic;
using AmazoomClassLibrary;

namespace WarehouseComputer.Classes
{
    public class WarehouseMap
    {
        public int NCol, NRow;
        public int NShelves;
        public Cell[] Cells; //cells used for locking access
        public List<Product> Inventory;
        public int[,,] AvailableLocations;

        public WarehouseMap(int nCol, int nRow, int nShelves)
        {
            this.NCol = nCol;
            this.NRow = nRow;
            this.NShelves = nShelves;

            this.Cells = new Cell[nCol * nRow];
            for (int i = 0; i < nRow; i++)
            {
                for (int j = 0; j < nCol; j++)
                {
                    this.Cells[i * nCol + j] = new Cell(j, i);
                }
            }
            Inventory = new List<Product>();
            AvailableLocations = new int[NCol - 1, NRow - 2, nShelves]; //no product can be placed on top+bottomRow/first column
        }

        public void PopulateShelves()
        {
            Location location;

            if(Inventory.Count > (NCol - 1) * (NRow - 2) * (NShelves))
            {
                throw new Exception("Too many items in the inventory for the warehouse !");
            }

            foreach (var product in Inventory)
            {
                location = FindFreeLocation();
                //Console.WriteLine($"Product {product.ProductName} is placed @ {location}.");
                product.Location = location;
            }

        }

        private Location FindFreeLocation()
        {
            /**
             * Policies of population :
             * - products are placed on even numbered aisles, except for the A column (placed on aisle 1)
             * - the "leftOrRight" coordinate of the products will determine on which shelf they are placed
             * - there can be at most one product per location
             * These policies are applied before determining the actual location of the product.
             * For the indexing of available locations, it's easier to keep a 3D-array.
             * These policies are applied because they make the route algorithm and robots paths easier.
             */

            Location freeLocation = new Location(0, 0, 0, 0);
            Boolean found = false;
            int row = 0, col = 0, lr = 0, s = 0;
            Random rand = new Random();

            while (!found)
            {
                col = rand.Next(NCol - 1);
                row = rand.Next(NRow - 2);
                s = rand.Next(NShelves);

                if (AvailableLocations[col, row, s] == 0)
                {
                    found = true;
                    AvailableLocations[col, row, s] = 1;

                    //Corrections of indices for the actual product's location
                    lr = 0;
                    if(col % 2 == 0)
                    {
                        lr = 1; //in AvailableLocations col 0 is A-up (right), col 1 is B-down(left), etc.
                    }

                    col = (int)Math.Floor((double)(col + 1) / 2)*2;
                    if(col == 0)
                    {
                        col = 1; //product on A-aisle should be placed on the right
                    }

                    row += 1; //top row is not occupied with shelves

                    

                }
            }

            freeLocation.x = col;
            freeLocation.y = row;
            freeLocation.leftOrRight = lr;
            freeLocation.shelf = s;

            return freeLocation;
        }

        public Cell GetCell(int row, int col)
        {
            return Cells[row * NCol + col];
        }

        public void AddProductToInventory(Product inputproduct, string filename)
        {
            Location location = FindFreeLocation();
            inputproduct.Location = location;
            Inventory.Add(inputproduct);
            JSONFile.ConvertProductToJSON(Inventory, filename);
        }
    }
}
