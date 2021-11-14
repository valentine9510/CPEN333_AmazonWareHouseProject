using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetApplication.Models
{
    abstract class TruckModel
    {
        int type_;
        double maxWeight_;

        public TruckModel(double maxWeight)
        {
            maxWeight_ = maxWeight;
        }
    }

    public class InventoryTruck : TruckModel
    {
        public InventoryTruck(double maxWeight)
        {
            
        }
    }
}