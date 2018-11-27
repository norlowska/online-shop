using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class Order
    {
        public string Address { get; set; }


        virtual public ICollection<Product> Products { get; set; }
    }
}