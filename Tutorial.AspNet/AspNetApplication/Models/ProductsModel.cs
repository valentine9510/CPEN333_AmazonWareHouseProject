using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetApplication.Models
{
    public class ProductsModel
    {
        private double weight_; //weight of product *note double
        private string name_;   //name of item

        public ProductsModel(double weight, string name)
        {
            weight_ = weight;
            name_ = name;
        }

    }
    
}