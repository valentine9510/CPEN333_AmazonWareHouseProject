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
        public int id_ { get; set; }
        public int status_ { get; set; }

        public List<(int,int)> route_ { get; set; }

        public double orderWeight { get; set; }

        public Dictionary<ProductsModel, int> listofProducts = new Dictionary<ProductsModel, int>(); // item and quantity
        public OrderModel(int id, int status)
        {
            id_ = id;
            status_ = status;
        }


        /**
        * set by computer to the particular order, which the robot will later access and retrieve items from
        * 
        * @param a list of coordinates (x,y) corresponding to where the items are located within the warehouse 
        */
        public void setRoute(List<(int,int)> r)
        {
            route_ = r;
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
            this.orderWeight += item.weight_ * quantity;
            int currentCount =0;
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
            this.orderWeight -= this.listofProducts[item]*item.weight_;
            if (listofProducts.ContainsKey(item))
            {
                listofProducts.Remove(item);
            }
        }



    }
}