using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace OnlineShop.Models
{
    public class Product : IEntit
    {
        public String name { get; set; }
        public String description { get; set; }
        public decimal price { get; set; }

        //public virtual Category cat_pro { get; set; }
        //public ICollection<Comment> comments { get; set; }
    }
}