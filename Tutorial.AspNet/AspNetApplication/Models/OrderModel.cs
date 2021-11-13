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
        Dictionary<ProductsModel, int> products_;
        
        public OrderModel(int id, int status)

        public void setStatus(int new_status)
        {
            status_ = new_status;
        }

    }
}