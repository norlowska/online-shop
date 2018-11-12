using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class Category : IEntit
    {
        public String name { get; set; }
        public int? ParentId { get; set; }
        /*public virtual Category Parent { get; set; }*/

        public virtual ICollection<Category> Children { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}