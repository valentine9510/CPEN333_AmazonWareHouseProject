using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetApplication.Models
{
    public class ShelfModel
    {
        double _maxWeight;
        Tuple<int, int, int, int> _location; //xCoord, yCoord, left(0) right(1), shelfnum(0-6)
        Dictionary<ProductsModel, int> listofProducts = new Dictionary<ProductsModel, int>();
        
        public ShelfModel(double maxWeight, Tuple<int, int, int, int> location)
        {
            _maxWeight = maxWeight;
            _location = location;
        }

        public bool add(ProductsModel item, int quantity)
        {

        }

        

    }
}