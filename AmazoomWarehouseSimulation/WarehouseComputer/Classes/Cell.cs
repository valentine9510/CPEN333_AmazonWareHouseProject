﻿using System;
namespace WarehouseComputer.Classes
{
    public class Cell
    {
        public int x { get; set; }
        public int y { get; set; }

        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
