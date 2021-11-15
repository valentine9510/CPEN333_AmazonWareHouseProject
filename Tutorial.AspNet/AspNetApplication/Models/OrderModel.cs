using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetApplication.Models
{
    public class OrderModel
    {
        enum Status
        {
            NEW_ORDER,
            PLACED,
            READY,
            DELIVERED
        }
        int status_;
        int id_;
        Tuple<int, int> route_;
        Dictionary<ProductsModel, int> listofProducts;
        
        public OrderModel(int id, int status)
        {
            
        }

        /**
        * sets the status of the order 
        * 
        * @param status between 0-3
        */
        public void setStatus(int new_status)
        {
            status_ = new_status;
        }

        /**
        * add quantity of item to order
        * 
        * @param item to add
        * @param quantity of item to add
        * @return false if item add is already present in order, true if new item
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
        * removes an item from the order 
        * 
        * @param item to remove
        */
        public void removeItem(ProductsModel item)
        {
            if (listofProducts.ContainsKey(item))
            {
                listofProducts.Remove(item);
            }
        }

        /**
        * returns dictionary of products associated with order 
        * 
        * @return dictionary list of products
        */
        public IDictionary<ProductsModel,int> returnListOfProducts()
        {
            return listofProducts;
        }

    }
}