using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class Comment : IEntit
    {
        public String comment { get; set; }
        public virtual Product product { get; set; }
    }
}