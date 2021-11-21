using System;
using System.Collections.Generic;

namespace WarehouseComputer.Classes
{
    public class WarehouseMap
    {
        public int nCol, nRow;
        public int[] coordinates; //for each coordinate, 0 if free, 1 if occupied
        //public ConditionVariable[] locks;

        public WarehouseMap(int nCol, int nRow)
        {
            this.nCol = nCol;
            this.nRow = nRow;

            this.coordinates = new int[nCol * nRow];
            for (int i = 0; i < nRow; i++)
            {
                for (int j = 0; j < nCol; j++)
                {
                    this.coordinates[i * nRow + j] = 0;
                    //init lock/cv
                }
            }
        }

        public int GetCoord(int row, int col)
        {
            //get lock from outside, check value, release lock
            return row * nCol + col;
        }
    }
}
