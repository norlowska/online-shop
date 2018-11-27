using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Models
{
    public class Product : IEntit
    {
        [Required, MaxLength(120)]
        public String name { get; set; }
        [AllowHtml]
        public String description { get; set; }
        //[Required, Range(0.0, (Double)decimal.MaxValue)]
        public decimal price { get; set; }
        public String filePath { get; set; }

        public int count { get; set; }

        public virtual Category cat_pro { get; set; }
        //public ICollection<Comment> comments { get; set; }
    }
}