using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetApplication.Models
{
    public class ProductsModel
    {
        public double weight_ { get; set; }
        public string name_ { get; set; }
        public ProductsModel(double weight, string name)
        {
            weight_ = weight;
            name_ = name;
        }

    }
    
}