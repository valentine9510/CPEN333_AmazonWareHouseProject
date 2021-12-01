using System;
using System.Collections.Generic;
using AmazoomClassLibrary;

namespace WarehouseComputer.Classes
{
    public class WarehouseMap
    {
        public int nCol, nRow;
        public Cell[] cells; //cells used for locking access
        public List<Product> Inventory;

        public WarehouseMap(int nCol, int nRow)
        {
            this.nCol = nCol;
            this.nRow = nRow;

            this.cells = new Cell[nCol * nRow];
            for (int i = 0; i < nRow; i++)
            {
                for (int j = 0; j < nCol; j++)
                {
                    this.cells[i * nCol + j] = new Cell(j, i);
                }
            }
            Inventory = new List<Product>();
            Product Banana = new Product("banana", 1, new Location(4, 2, 0, 2), 1);
            Inventory.Add(Banana);
            Product Apple = new Product("ananas", 1, new Location(2, 3, 1, 1), 1);
            Inventory.Add(Apple);
            Product Orange = new Product("orange", 1, new Location(8, 3, 0, 1), 1);
            Inventory.Add(Orange);
            Product Lemon = new Product("lemon", 1, new Location(12, 2, 1, 2), 1);
            Inventory.Add(Lemon);

        }

        public Cell GetCell(int row, int col)
        {
            return cells[row * nCol + col];
        }
    }
}
