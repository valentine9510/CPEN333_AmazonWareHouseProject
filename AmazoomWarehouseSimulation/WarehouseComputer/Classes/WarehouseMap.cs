using System;
using System.Collections.Generic;

namespace WarehouseComputer.Classes
{
    public class WarehouseMap
    {
        public int nCol, nRow;
        public Cell[] cells; //cells used for locking access

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
        }

        public Cell GetCell(int row, int col)
        {
            return cells[row * nCol + col];
        }
    }
}
