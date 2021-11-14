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
            // might have to also add code to initialize dictionary with list of products
        }

        /**
        * adds item to shelf. if item already present, adds to item from what is already there
        * 
        * @param item to add
        * @param quantity to add
        * @return false if item already present and added, true otherwise
        * @weight parameters NOT adjusted - might have to fix this function to add extra functionality 
        */
        public bool addItem(ProductsModel item, int quantity)
        {
            int currentCount;
            listofProducts.TryGetValue(item, out currentCount); //trygetvalue sets currentCount to 0 if key doesnt exist
            if (currentCount != 0) //if item exists already on shelf add to the quantity
            {
                listofProducts[item] = currentCount + quantity;
                return false;
            }
            listofProducts.Add(item, quantity);
            return true;
        }


        /**
        * remove quantity of item specified, removes item key from dictionary if quantity depleted
        * 
        * @param item to search
        * @param number to take off shelf 
        * @return 0 if item not on shelf
        *         or if (quantity > available amount) returns max quantity
        *         or if (quantity < available amount) returns quantity
        */
       public int removeItem(ProductsModel item, int quantity)
        {
            if (listofProducts.ContainsKey(item))
            {
                int amountOfItemInInventory = listofProducts[item];
                int quantityCompare = amountOfItemInInventory - quantity;
                if (quantityCompare <= 0)
                {
                    listofProducts.Remove(item);
                    return quantityCompare < 0 ? amountOfItemInInventory : quantity;
                }
                else
                {
                    listofProducts[item] -= quantity;
                    return quantity;
                }
            }
            return 0;
        }

        /**
        * searches the dictionary associated with shelf to see if product already exists
        * 
        * @param item to search
        * @return true if item is on shelf
        */
        public bool hasProduct(ProductsModel item)
        {
            if (listofProducts.ContainsKey(item))
            {
                return true;
            }
            return false;
        }


        

    }
}