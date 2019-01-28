using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class Category : IEntit
    {
        public String name { get; set; }
        virtual public ICollection<Product> Product { get; set; }

        public int? parentId { get; set; }
        virtual public Category parent { get; set; }
    }
}