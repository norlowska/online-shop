using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Linq;

namespace OnlineShop.Models
{
    public class Product : IEntit
    {
        
        public Product()
        {
            image = new Dictionary<string, string>();
            
            //files = new List<String>();
        }

        [NotMapped]
        public Dictionary<string, string> image { get; set; }

        [Required, MaxLength(120)]
        public String name { get; set; }
        [AllowHtml]
        public String description { get; set; }
        [Required, Range(0.0, (Double)decimal.MaxValue)]
        public decimal price { get; set; }

        public string jsonDictionary { get; set; }

        //public IList<String> files { get; set; }

        public int count { get; set; }

        public virtual Category cat_pro { get; set; }
        //public ICollection<Comment> comments { get; set; }

        public void toJson()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            jsonDictionary = serializer.Serialize((object)image);

        }

        public void toDictionary()
        {
            if(jsonDictionary != null)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                image = serializer.Deserialize<Dictionary<string, string>>(jsonDictionary);
            }
            
        }
        
    }
}